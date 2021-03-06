﻿using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Dut.AtCommand;
using AILinkFactoryAuto.Dut.AtCommand.Property;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using AILinkFactoryAuto.Task.SmartBracelet.Property;
using AILinkFactoryAuto.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace AILinkFactoryAuto.Task.SmartBracelet.Executer
{
    public class SKGControlCommandExecuter : IExecuter
    {
        private string pattern;

        private GlobalVaribles configGv;


        private ILog log;
        private List<byte> buffer = new List<byte>(4096);       //uart 接受数据buffer 未做任何处理

        public SKGControlCommandExecuter()
        {
            this.pattern = GlobalVaribles.PATTERN;
        }

        private string PreTranslateAtCommand(string atCommand)
        {
            string newAtCommand = atCommand;
            Match match = Regex.Match(atCommand, @"{([0-9a-zA-Z_]+)}");
            for (int i = 1; i < match.Groups.Count; i++)
            {
                string key = match.Groups[i].ToString();
                string value = configGv.Get(key);

                if (string.IsNullOrEmpty(value))
                {
                    throw new BaseException("null value in GlobalVaribles");
                }

                newAtCommand = newAtCommand.Replace("{" + key + "}", value);
            }

            return newAtCommand;
        }

        private string PreTranslateCheckInfo(string checkInfo)
        {
            string newCheckInfo = checkInfo;
            Match match = Regex.Match(checkInfo, pattern);
            for (int i = 1; i < match.Groups.Count; i++)
            {
                string key = match.Groups[i].ToString();
                newCheckInfo = configGv.Get(key);
            }

            return newCheckInfo;
        }

        private string Parse_r_n(string value)
        {
            value = value.Replace("\\r", "\r");
            value = value.Replace("\\R", "\r");
            value = value.Replace("\\n", "\n");
            value = value.Replace("\\N", "\n");
            return value;
        }

        public void Run(IProperties properties, GlobalDic<string, object> globalDic)
        {
            SKGControlCommandProperties config = properties as SKGControlCommandProperties;
            List<ComDut> comDutList = globalDic[typeof(List<ComDut>).ToString()] as List<ComDut>;
            //ComDut comDut = globalDic[typeof(ComDut).ToString()] as ComDut;
            ComDut comDut = null;
            foreach (var item in comDutList)
            {
                if (item.PortName == config.PortName)
                {
                    comDut = item;
                    break;
                }

            }
            if (comDut == null)
            {
                throw new BaseException(string.Format("ComDut No PortName:{0}", config.PortName));
            }



            //OpenPhoneProperties configOpenPhone = globalDic[typeof(OpenPhoneProperties).ToString()] as OpenPhoneProperties;
            List<OpenPhoneProperties> configList = globalDic[typeof(List<OpenPhoneProperties>).ToString()] as List<OpenPhoneProperties>;
            OpenPhoneProperties configOpenPhone = null;
            foreach (var item in configList)
            {
                if (item.PortName == config.PortName)
                {
                    configOpenPhone = item;
                    break;
                }

            }
            if (configOpenPhone == null)
            {
                throw new BaseException(string.Format("OpenPhoneProperties No PortName:{0}", config.PortName));
            }

            log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;

            comDut.Parity = configOpenPhone.Parity;
            string endLine = configOpenPhone.EndLine;
            if (!string.IsNullOrEmpty(endLine))
            {
                endLine = Parse_r_n(endLine);
            }

            string command = "";
            command += config.Head;

            //int型的处理-转2字节byte,连接到16进制字符串中
            int allLength = 9 + 17; //config.DataLength
            byte[] byteTemp = new byte[2];
            byte[] oneByteTemp = new byte[1];

            byteTemp = intToBytes(allLength);
            command += byteToHexStr(byteTemp);

            command += config.ReservedWord;
            command += config.CommandWord;

            //DataContent部分
            //枚举类的处理
            int intTemp = (int)config.PowerOnOffSetting;//开关机设置
            if (config.PowerOnOffSetting != 0)
            {
                log.Info(string.Format("开关机设置:{0}", config.PowerOnOffSetting));
            }
            command += intTemp.ToString().PadLeft(2,'0');

            intTemp = (int)config.LedModeSetting;
            if (config.LedModeSetting != 0)
            {
                log.Info(string.Format("LED 模式设置:{0}", config.LedModeSetting));
            }
            command += intTemp.ToString().PadLeft(2, '0');

            intTemp = (int)config.EmsTestSwitch;
            if (config.EmsTestSwitch != 0)
            {
                log.Info(string.Format("EMS 测试开关设置:{0}", config.EmsTestSwitch));
            }
            command += intTemp.ToString().PadLeft(2, '0');

            //int型的处理
            byteTemp = intToBytes(config.EmsPWSetting);
            if (config.EmsPWSetting!=0)
            {
                log.Info(string.Format("EMS 脉宽设置:{0}uS", config.EmsPWSetting));
            }
            command += byteToHexStr(byteTemp);

            byteTemp = intToBytes(config.EmsFreqSetting);
            if (config.EmsFreqSetting != 0)
            {
                log.Info(string.Format("EMS 频率设置:{0}Hz", config.EmsFreqSetting));
            }
            command += byteToHexStr(byteTemp);

            byteTemp = intToBytes(config.EmsAmplitudeSetting);
            if (config.EmsAmplitudeSetting != 0)
            {
                log.Info(string.Format("EMS 幅度设置:{0}mV", config.EmsAmplitudeSetting));
            }

            command += byteToHexStr(byteTemp);

            //枚举类的处理
            intTemp = (int)config.HeatingGearControl;
            if (config.HeatingGearControl != 0)
            {
                log.Info(string.Format("加热档位控制:{0}", config.HeatingGearControl));
            }
            command += intTemp.ToString().PadLeft(2, '0');

            intTemp = (int)config.VoiceControl;
            command += intTemp.ToString().PadLeft(2, '0');

            intTemp = (int)config.WritePcbaFinishFlag;
            if (config.WritePcbaFinishFlag != 0)
            {
                log.Info(string.Format("PCBA 测试完成标志:{0}", config.WritePcbaFinishFlag));
            }
            command += intTemp.ToString().PadLeft(2, '0');

            intTemp = (int)config.WholeMachineFinishFlag;
            if (config.WholeMachineFinishFlag != 0)
            {
                log.Info(string.Format("PCBA 测试完成标志:{0}", config.WholeMachineFinishFlag));
            }

            command += intTemp.ToString().PadLeft(2, '0');

            intTemp = (int)config.BtTestOnOffSetting;
            if (config.BtTestOnOffSetting != 0)
            {
                log.Info(string.Format("蓝牙模块测试:{0}", config.BtTestOnOffSetting));
            }
            command += intTemp.ToString().PadLeft(2, '0');

            intTemp = (int)config.MotorControl;
            if (config.MotorControl != 0)
            {
                log.Info(string.Format("电机控制:{0}", config.MotorControl));
            }
            command += intTemp.ToString().PadLeft(2, '0');

            intTemp = (int)config.AginTestOnOffSetting;
            if (config.AginTestOnOffSetting != 0)
            {
                log.Info(string.Format("老化测试开关:{0}", config.AginTestOnOffSetting));
            }
            command += intTemp.ToString().PadLeft(2, '0');

            //int型的处理-转为1字节byte
            oneByteTemp = intToOneBytes(config.AginTestTime);
            if (config.AginTestTime != 0)
            {
                log.Info(string.Format("老化测试时间:{0}分钟", config.AginTestTime*10));
            }
            command += byteToHexStr(oneByteTemp);
            #region 多路振动控制
            //高位在前，第16位，为第16个电机
            if (true)
            {
                //int bit1 = (int)config.VibrationControl16;
                //bit1 = bit1 << 7;

                //int bit2 = (int)config.VibrationControl15;
                //bit2 = bit2 << 6;

                //int bit3 = (int)config.VibrationControl14;
                //bit3 = bit3 << 5;

                //int bit4 = (int)config.VibrationControl13;
                //bit4 = bit4 << 4;

                //int bit5 = (int)config.VibrationControl12;
                //bit5 = bit5 << 3;

                //int bit6 = (int)config.VibrationControl11;
                //bit6 = bit6 << 2;

                //int bit7 = (int)config.VibrationControl10;
                //bit7 = bit7 << 1;

                //int bit8 = (int)config.VibrationControl9;

                //int byte2 = bit1 | bit2 | bit3 | bit4 | bit5 | bit6 | bit7 | bit8;
                //command += byte2.ToString("X");//.PadLeft(2, '0')

                //bit1 = (int)config.VibrationControl8;
                //bit1 = bit1 << 7;

                //bit2 = (int)config.VibrationControl7;
                //bit2 = bit2 << 6;

                //bit3 = (int)config.VibrationControl6;
                //bit3 = bit3 << 5;

                //bit4 = (int)config.VibrationControl5;
                //bit4 = bit4 << 4;

                //bit5 = (int)config.VibrationControl4;
                //bit5 = bit5 << 3;

                //bit6 = (int)config.VibrationControl3;
                //bit6 = bit6 << 2;

                //bit7 = (int)config.VibrationControl2;
                //bit7 = bit7 << 1;

                //bit8 = (int)config.VibrationControl1;

                //int byte1 = bit1 | bit2 | bit3 | bit4 | bit5 | bit6 | bit7 | bit8;

                //command += byte1.ToString("X").PadLeft(2, '0');
            }
            #endregion
            #region 640nm 红光控制,G7 新增功能
            if (true)
            {
                //intTemp = (int)config.RedLightControl640nm;
                //command += intTemp.ToString("X").PadLeft(2, '0');//
            }
            #endregion
            //在加校验位
            byte[] exceptXor = strToToHexByte(command);
            byte xor = ByteToXOR(exceptXor);
            command += xor.ToString("X").PadLeft(2, '0');

            //发送完整控制命令
            byte[] atCommand = strToToHexByte(command);

            //string atCommand = PreTranslateAtCommand(config.AtCommand);
            UartDisplay(atCommand, "T");

            //log.Info("AT Commond=" + config.AtCommand);

            comDut.DtrEnable = configOpenPhone.Dtr;
            comDut.RtsEnable = configOpenPhone.Rts;
            comDut.Write(atCommand, 0, atCommand.Length);
            Thread.Sleep(config.AtCommandInterval);

            #region
            //DateTime start = DateTime.Now;
            //DateTime now = DateTime.Now;
            //TimeSpan timeSpan = now - start;
            //while (timeSpan.TotalMilliseconds <= config.Timeout)
            //{
            //    try
            //    {
            //        comDut.Write(atCommand, 0, atCommand.Length);
            //        Thread.Sleep(config.AtCommandInterval);

            //        int n = comDut.BytesToRead;
            //        byte[] buf = new byte[n];
            //        comDut.Read(buf, 0, n);
            //        //解析是否收到正确的包,包头，命令，校验位

            //        //如果收到的包正确
            //        if (true)
            //        {
            //            break;
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e);
            //    }

            //    Thread.Sleep(100);
            //    now = DateTime.Now;
            //    timeSpan = now - start;
            //}

            //if (timeSpan.TotalMilliseconds > config.Timeout)
            //{
            //    throw new BaseException("超时：");
            //}
            #endregion

            TimeUtils.Execute(() =>
            {
                int n = comDut.BytesToRead;
                //if (n == 0 )
                //{
                //    //继续循环执行
                //    return false;
                //}
                log.Info(string.Format("接收的长度:{0}", n));
                if (n!=0)
                {
                    byte[] buf = new byte[n];
                    comDut.Read(buf, 0, n);
                    UartDisplay(buf, "R");

                    buffer.Clear();
                    buffer.AddRange(buf);

                }
                //byte[] buf = new byte[n];
                //comDut.Read(buf, 0, n);
                //buffer.AddRange(buf);

                if (buffer.Count > 9)
                {
                    if (buffer[0] == 0xA5)
                    {
                        //throw new BaseException(string.Format("Head Error"));
                        //if (&& buf[1] == 0xC3))
                        //{

                        //}
                        if (buffer[1] == 0xC3)
                        {
                            byte high = buffer[2];
                            byte low = buffer[3];
                            int iTotalLength = (high & 0xFF) << 8 | low;

                            if (buffer.Count < iTotalLength) //数据区尚未接收完整
                            {
                                //继续循环执行
                                log.Info(string.Format("数据尚未接收完整,已接收:{0},总长度:{1}", buffer.Count, iTotalLength));
                                return false;
                            }
                            byte[] oneFrameBytes = new byte[iTotalLength];
                            buffer.CopyTo(0, oneFrameBytes, 0, iTotalLength);
                            buffer.RemoveRange(0, iTotalLength);

                            UartDisplay(oneFrameBytes, "R");

                            #region //开始校验-命令字
                            if (config.CommandType == SKGControlCommandProperties.EnumCommandType.查询指令)
                            {
                                if (!(oneFrameBytes[6] == 0xA0 && oneFrameBytes[7] == 0x37))
                                {
                                    log.Info(string.Format("CMD Type Error"));
                                    //继续循环执行
                                    return false;
                                }
                            }
                            else if (config.CommandType == SKGControlCommandProperties.EnumCommandType.控制指令)
                            {
                                if (!(oneFrameBytes[6] == 0xA0 && oneFrameBytes[7] == 0x40))
                                {
                                    log.Info(string.Format("CMD Type Error"));
                                    //继续循环执行
                                    return false;

                                }
                            }
                            else if (config.CommandType == SKGControlCommandProperties.EnumCommandType.下发授权)
                            {
                                if (!(oneFrameBytes[6] == 0xA0 && oneFrameBytes[7] == 0x23))
                                {
                                    log.Info(string.Format("CMD Type Error"));
                                    //继续循环执行
                                    return false;

                                }
                            }
                            else
                            {
                                log.Info(string.Format("非法指令类型"));
                                //继续循环执行
                                return false;

                            }

                            //异或校验 1字节
                            byte[] bufOutOxr = new byte[oneFrameBytes.Length - 1];
                            Array.Copy(oneFrameBytes, 0, bufOutOxr, 0, oneFrameBytes.Length - 1);
                            byte retXor = ByteToXOR(bufOutOxr);

                            if (oneFrameBytes[iTotalLength - 1] != retXor)
                            {
                                log.Info(string.Format("核验RX的校验位：错误,返回校验位:{0:X2},计算校验位:{1:X2}", oneFrameBytes[iTotalLength - 1], retXor));
                                //继续循环执行
                                return false;
                            }
                            log.Info(string.Format("核验RX的校验位：正确,返回校验位:{0:X2},计算校验位:{1:X2}", oneFrameBytes[iTotalLength - 1], retXor));

                            //数据内容 32个字节——长度是不固定的
                            byte[] dataArry = new byte[iTotalLength - 9];//config.DataLength
                            Array.Copy(oneFrameBytes, 8, dataArry, 0, iTotalLength - 9);//config.DataLength

                            //添加到全局变量中
                            if (config.GlobalVariblesKey != null)
                            {
                                for (int i = 0; i < config.GlobalVariblesKey.Length; i++)
                                {
                                    Match matchKey = Regex.Match(config.GlobalVariblesKey[i], this.pattern);
                                    if (!matchKey.Success)
                                    {
                                        throw new BaseException("read info key fail");
                                    }

                                    string key = matchKey.Groups[1].ToString();

                                    configGv.Add(key, dataArry);
                                    log.Info("应答报文格式检查PASS");
                                }
                            }

                            //跳出循环执行
                            return true;

                            #endregion
                        }
                        else
                        {
                            log.Info(string.Format("头部不是0xA5,0xC3，buffer.RemoveAt：{0:X2},{1:X2}", buffer[0], buffer[1]));

                            buffer.RemoveRange(0, 2);
                            //继续循环执行
                            return false;

                        }

                    }
                    else
                    {
                        log.Info(string.Format("头部不是0xA5，buffer.RemoveAt：{0:X2}", buffer[0]));

                        buffer.RemoveAt(0);
                        //继续循环执行
                        return false;

                    }
                }
                else
                {
                    log.Info(string.Format("buffer.Count:{0}小于最小长度9", buffer.Count));
                    //继续循环执行
                    return false;
                }

            }, config.Timeout);

            #region 原发送命令后等待一固定时间，再接收数据，对应不上则重试

            ////解析是否收到正确的包,包头，命令，校验位
            ////起始标志 数据长度 保留字     命令字     数据内容    异或校验码 
            ////0XA5C3    9+N     0x0000      0x0037      Data        异或 
            ////2 字节  2 字节    2 字节      2 字节      N 字节     1 字节
            //UartDisplay(buf, "R");
            //if (buf.Length < 9)
            //{
            //    throw new BaseException(string.Format("Response Length Not enough"));
            //}
            ////包头
            //if (!(buf[0] == 0xA5 && buf[1] == 0xC3))
            //{
            //    throw new BaseException(string.Format("Response Head Error"));
            //}
            ////长度 2字节
            //byte high = buf[2];
            //byte low = buf[3];
            //int iTotalLength = (high & 0xFF) << 8 | low;

            //if (iTotalLength != n)
            //{
            //    throw new BaseException(string.Format("Response Length Error,长度位：{0},实际长度:{1}", iTotalLength, n));
            //}

            ////命令字
            //if (config.CommandType == SKGControlCommandProperties.EnumCommandType.查询指令)
            //{
            //    if (!(buf[6] == 0xA0 && buf[7] == 0x37))
            //    {
            //        throw new BaseException(string.Format("Response Command Type Error"));
            //    }
            //}
            //else if (config.CommandType == SKGControlCommandProperties.EnumCommandType.控制指令)
            //{
            //    if (!(buf[6] == 0xA0 && buf[7] == 0x40))
            //    {
            //        throw new BaseException(string.Format("Response Command Type Error"));
            //    }
            //}
            //else if (config.CommandType == SKGControlCommandProperties.EnumCommandType.下发授权)
            //{
            //    if (!(buf[6] == 0xA0 && buf[7] == 0x23))
            //    {
            //        throw new BaseException(string.Format("Response Command Type Error"));
            //    }
            //}
            //else
            //{
            //    throw new BaseException(string.Format("Response为非法指令类型"));
            //}


            ////异或校验 1字节
            //byte[] bufOutOxr = new byte[buf.Length - 1];
            //Array.Copy(buf, 0, bufOutOxr, 0, buf.Length - 1);
            //byte retXor = ByteToXOR(bufOutOxr);

            //if (buf[n - 1] != retXor)
            //{
            //    throw new BaseException(string.Format("核验RX的校验位：错误,返回校验位:{0:X2},计算校验位:{1:X2}", buf[n - 1], retXor));
            //}
            //log.Info(string.Format("核验RX的校验位：正确,返回校验位:{0:X2},计算校验位:{1:X2}", buf[n - 1], retXor));


            ////数据内容 32个字节
            ////byte[] dataArry = new byte[config.DataLength];
            ////Array.Copy(buf, 8, dataArry, 0, config.DataLength);

            //byte[] dataArry = new byte[iTotalLength - 9];//config.DataLength
            //Array.Copy(buf, 8, dataArry, 0, iTotalLength - 9);//config.DataLength

            ////添加到全局变量中
            //if (config.GlobalVariblesKey != null)
            //{
            //    for (int i = 0; i < config.GlobalVariblesKey.Length; i++)
            //    {
            //        //string pattern = "-([0-9]{2}) ";
            //        //Match matchValue = Regex.Match(response, pattern);
            //        //if (!matchValue.Success)
            //        //{
            //        //    throw new BaseException("read info value fail");
            //        //}

            //        //string value = matchValue.Groups[1].ToString();

            //        Match matchKey = Regex.Match(config.GlobalVariblesKey[i], this.pattern);
            //        if (!matchKey.Success)
            //        {
            //            throw new BaseException("read info key fail");
            //        }

            //        string key = matchKey.Groups[1].ToString();

            //        configGv.Add(key, dataArry);
            //        log.Info("应答报文格式检查PASS");
            //    }
            //}
            #endregion
            //检查是否有效果!!!
            //通过另外一个AT口,去获取电流值




            ////如果收到的包正确
            //string str = byteToHexStr(buf);


            //string response = comDut.ReadExisting();
            //comDut.DtrEnable = false;
            //comDut.RtsEnable = false;

            //log.Info("AT Response=" + response);

            //if (!string.IsNullOrEmpty(config.AtCommandError))
            //{
            //    if (response.Contains(config.AtCommandError))
            //    {
            //        throw new BaseException(string.Format("AT response=[{0}] contain error=[{1}]", response, config.AtCommandError));
            //    }
            //}

            //if (!string.IsNullOrEmpty(config.AtCommandOk))
            //{
            //    if (!response.Contains(config.AtCommandOk))
            //    {
            //        throw new BaseException(string.Format("AT response=[{0}] not contain error=[{1}]", response, config.AtCommandOk));
            //    }
            //}

            //if (config.CheckInfo != null)
            //{
            //    for (int i = 0; i < config.CheckInfo.Length; i++)
            //    {
            //        if (!string.IsNullOrEmpty(config.CheckInfo[i]))
            //        {
            //            string checkInfo = PreTranslateCheckInfo(config.CheckInfo[i]);
            //            if (!response.Contains(checkInfo))
            //            {
            //                throw new BaseException(string.Format("AT response=[{0}] not contain check info=[{1}]", response, checkInfo));
            //            }
            //        }
            //    }
            //}

            //if (config.GlobalVariblesKey != null && config.GlobalVariblesKeyPattern != null)
            //{
            //    if (config.GlobalVariblesKey.Length != config.GlobalVariblesKeyPattern.Length)
            //    {
            //        throw new BaseException("请保持GlobalVariblesKey与GlobalVariblesKeyPattern成对存在");
            //    }
            //    for (int i = 0; i < config.GlobalVariblesKey.Length; i++)
            //    {
            //        string pattern = "-([0-9]{2}) ";
            //        Match matchValue = Regex.Match(response, pattern);
            //        if (!matchValue.Success)
            //        {
            //            throw new BaseException("read info value fail");
            //        }

            //        string value = matchValue.Groups[1].ToString();

            //        Match matchKey = Regex.Match(config.GlobalVariblesKey[i], this.pattern);
            //        if (!matchKey.Success)
            //        {
            //            throw new BaseException("read info key fail");
            //        }

            //        string key = matchKey.Groups[1].ToString();

            //        configGv.Add(key, value);
            //    }
            //}
        }
        /// <summary>
        /// 字符串转16进制字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        private static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// <summary>
        /// 字节数组转16进制字符串并反转
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string byteToHexStrReverse(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = bytes.Length - 1; i >= 0; i--)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
        /// <summary>
        /// int转化为byte[2]
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public byte[] intToBytes(int value)
        {
            byte[] src = new byte[2];
            //src[3] = (byte)((value >> 24) & 0xFF);
            //src[2] = (byte)((value >> 16) & 0xFF);
            src[0] = (byte)((value >> 8) & 0xFF);
            src[1] = (byte)(value & 0xFF);
            return src;
        }
        /// <summary>
        /// int转化为byte[1]
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public byte[] intToOneBytes(int value)
        {
            byte[] src = new byte[1];
            //src[3] = (byte)((value >> 24) & 0xFF);
            //src[2] = (byte)((value >> 16) & 0xFF);
            //src[0] = (byte)((value >> 8) & 0xFF);
            //src[1] = (byte)(value & 0xFF);
            src[0] = (byte)(value & 0xFF);
            return src;
        }


        public void UartDisplay(byte[] by, string type)
        {
            string log = "";
            for (int i = 0; i < by.Length; i++)
            {
                log += by[i].ToString("X2") + " ";
            }
            if (type == "R")
            {
                this.log.Info("Rx:" + log);
            }
            else if (type == "T")
            {
                this.log.Info("Tx:" + log);
            }
            else
            {
                this.log.Info(log);
            }

        }
        /// <summary>
        /// 对同一数组内数据进行异或
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        private byte ByteToXOR(byte[] arr)
        {
            byte xor = 0;

            for (int i = 0; i < arr.Length; i++)
            {
                xor ^= arr[i];

            }
            //Console.WriteLine("0x{0:x}", xor);
            //Console.ReadKey();
            return xor;
        }

    }
}
