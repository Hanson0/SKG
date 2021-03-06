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
    public class SKGAuthorizeCommandExecuter : IExecuter
    {
        private string pattern;

        private GlobalVaribles configGv;


        private ILog log;
        private List<byte> buffer = new List<byte>(4096);       //uart 接受数据buffer 未做任何处理

        public SKGAuthorizeCommandExecuter()
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
            SKGAuthorizeCommandProperties config = properties as SKGAuthorizeCommandProperties;
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
            int allLength = 9 + 26;//config.DataLength
            byte[] byteTemp = new byte[2];
            byte[] oneByteTemp = new byte[1];
            byteTemp = intToBytes(allLength);
            command += byteToHexStr(byteTemp);

            command += config.ReservedWord;
            command += config.CommandWord;

            //DataContent部分
            //枚举类的处理
            int intTemp = (int)config.AuthorizeEvent;
            command += intTemp.ToString().PadLeft(2, '0');

            //int型处理
            string snPcbaId= configGv.Get(GlobalVaribles.LABEL_SN);


            if (config.AuthorizeEvent== SKGAuthorizeCommandProperties.EnumAuthorizeEvent.SN_授权 ||
                config.AuthorizeEvent == SKGAuthorizeCommandProperties.EnumAuthorizeEvent.PCBA_ID授权
                )
            {
                #region //扫描的SN/PCBA码是直接16进制的连接
                //int effectiveLength = snPcbaId.Length / 2;
                //command += effectiveLength.ToString("X2");
                //command += snPcbaId.PadRight(48, '0');
                #endregion

                #region //扫描的SN/PCBA码是字符型，ASCII码的方式
                byte[] bytesSnPcba = System.Text.Encoding.ASCII.GetBytes(snPcbaId);
                command += bytesSnPcba.Length.ToString("X2");
                command += byteToHexStr(bytesSnPcba).PadRight(48, '0');
                #endregion
            }
            else if(config.AuthorizeEvent == SKGAuthorizeCommandProperties.EnumAuthorizeEvent.授权写入蓝牙广播报信息)
            {
                //V16版的协议不写蓝牙广播
                byte[] bytesSnPcba = System.Text.Encoding.ASCII.GetBytes(config.BleBroadcastName);
                command += bytesSnPcba.Length.ToString("X2");
                command += byteToHexStr(bytesSnPcba).PadRight(17, '0');

                command += config.DeviceType;
                command += config.ReservedWord1;
                command += config.ReservedWord2;
            }
            else if (config.AuthorizeEvent == SKGAuthorizeCommandProperties.EnumAuthorizeEvent.解锁SWD)
            {
                byte[] seroArry = new byte[25];
                command += byteToHexStr(seroArry);
            }


            #region
            //SN / PCBAID 需要填满 24 字节，不够 长的在后面补 0x00（例：SN 有效长度 为 16 字 节 则 第 17-24 字 节 填 充 0x00）
            //command += config.Sn_PCBA_ID.PadRight(48,'0');

            //intTemp = (int)config.LedModeSetting;
            //command += intTemp.ToString().PadLeft(2, '0');

            //intTemp = (int)config.EmsTestSwitch;
            //command += intTemp.ToString().PadLeft(2, '0');

            ////int型的处理
            //byteTemp = intToBytes(config.EmsPWSetting);
            //command += byteToHexStr(byteTemp);

            //byteTemp = intToBytes(config.EmsFreqSetting);
            //command += byteToHexStr(byteTemp);

            //byteTemp = intToBytes(config.EmsAmplitudeSetting);
            //command += byteToHexStr(byteTemp);

            ////枚举类的处理
            //intTemp = (int)config.HeatingGearControl;
            //command += intTemp.ToString().PadLeft(2, '0');

            //intTemp = (int)config.VoiceControl;
            //command += intTemp.ToString().PadLeft(2, '0');

            //intTemp = (int)config.WritePcbaFinishFlag;
            //command += intTemp.ToString().PadLeft(2, '0');

            //intTemp = (int)config.WholeMachineFinishFlag;
            //command += intTemp.ToString().PadLeft(2, '0');

            //intTemp = (int)config.BtTestOnOffSetting;
            //command += intTemp.ToString().PadLeft(2, '0');

            //intTemp = (int)config.MotorControl;
            //command += intTemp.ToString().PadLeft(2, '0');

            //intTemp = (int)config.AginTestOnOffSetting;
            //command += intTemp.ToString().PadLeft(2, '0');

            ////int型的处理-转为1字节byte
            //oneByteTemp = intToOneBytes(config.AginTestTime);
            //command += byteToHexStr(byteTemp);
            //#region 多路振动控制
            //intTemp = (int)config.VibrationControl1;
            //command += intTemp.ToString().PadLeft(2, '0');

            //intTemp = (int)config.VibrationControl2;
            //command += intTemp.ToString().PadLeft(2, '0');

            //intTemp = (int)config.VibrationControl3;
            //command += intTemp.ToString().PadLeft(2, '0');

            //intTemp = (int)config.VibrationControl4;
            //command += intTemp.ToString().PadLeft(2, '0');

            //intTemp = (int)config.VibrationControl5;
            //command += intTemp.ToString().PadLeft(2, '0');

            //intTemp = (int)config.VibrationControl6;
            //command += intTemp.ToString().PadLeft(2, '0');

            //intTemp = (int)config.VibrationControl7;
            //command += intTemp.ToString().PadLeft(2, '0');

            //intTemp = (int)config.VibrationControl8;
            //command += intTemp.ToString().PadLeft(2, '0');

            //intTemp = (int)config.VibrationControl9;
            //command += intTemp.ToString().PadLeft(2, '0');

            //intTemp = (int)config.VibrationControl10;
            //command += intTemp.ToString().PadLeft(2, '0');

            //intTemp = (int)config.VibrationControl11;
            //command += intTemp.ToString().PadLeft(2, '0');

            //intTemp = (int)config.VibrationControl12;
            //command += intTemp.ToString().PadLeft(2, '0');

            //intTemp = (int)config.VibrationControl13;
            //command += intTemp.ToString().PadLeft(2, '0');

            //intTemp = (int)config.VibrationControl4;
            //command += intTemp.ToString().PadLeft(2, '0');

            //intTemp = (int)config.VibrationControl5;
            //command += intTemp.ToString().PadLeft(2, '0');

            //intTemp = (int)config.VibrationControl6;
            //command += intTemp.ToString().PadLeft(2, '0');
            //#endregion
            //intTemp = (int)config.RedLightControl640nm;
            //command += intTemp.ToString().PadLeft(2, '0');

            #endregion
            //byteToHexStr();

            //byte[] atCommand = strToToHexByte(config.AtCommand);

            //计算校验位
            byte[] atCommandOutXor = strToToHexByte(command);
            byte[] atCommand = new byte[atCommandOutXor.Length + 1];

            byte byteXor = ByteToXOR(atCommandOutXor);

            Array.Copy(atCommandOutXor, 0, atCommand, 0, atCommandOutXor.Length);
            //赋值最后一个校验位
            atCommand[atCommandOutXor.Length] = byteXor;

            //string atCommand = PreTranslateAtCommand(config.AtCommand);
            UartDisplay(atCommand, "T");

            //log.Info("AT Commond=" + config.AtCommand);

            comDut.DtrEnable = configOpenPhone.Dtr;
            comDut.RtsEnable = configOpenPhone.Rts;
            comDut.Write(atCommand, 0, atCommand.Length);
            Thread.Sleep(config.AtCommandInterval);

            //发命令后一直循环接收->检验->拼接，超过500ms还没检验通过则超时FAIL
            TimeUtils.Execute(() =>
            {
                int n = comDut.BytesToRead;
                //if (n == 0)
                //{
                //    //继续循环执行
                //    return false;
                //}
                //byte[] buf = new byte[n];
                //comDut.Read(buf, 0, n);
                //buffer.AddRange(buf);
                log.Info(string.Format("接收的长度:{0}", n));

                if (n != 0)
                {
                    byte[] buf = new byte[n];
                    comDut.Read(buf, 0, n);
                    UartDisplay(buf, "R");

                    buffer.Clear();
                    buffer.AddRange(buf);

                }


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
                                log.Info(string.Format("数据尚未接收完整,已接收:{0},总长度:{1}", buffer.Count, iTotalLength));
                                //继续循环执行
                                return false;
                            }
                            byte[] oneFrameBytes = new byte[iTotalLength];
                            buffer.CopyTo(0, oneFrameBytes, 0, iTotalLength);
                            buffer.RemoveRange(0, iTotalLength);

                            UartDisplay(oneFrameBytes, "R");

                            #region //开始校验-命令字
                            if (config.CommandType == SKGAuthorizeCommandProperties.EnumCommandType.查询指令)
                            {
                                if (!(oneFrameBytes[6] == 0xA0 && oneFrameBytes[7] == 0x37))
                                {
                                    log.Info(string.Format("CMD Type Error"));
                                    //继续循环执行
                                    return false;
                                }
                            }
                            else if (config.CommandType == SKGAuthorizeCommandProperties.EnumCommandType.控制指令)
                            {
                                if (!(oneFrameBytes[6] == 0xA0 && oneFrameBytes[7] == 0x40))
                                {
                                    log.Info(string.Format("CMD Type Error"));
                                    //继续循环执行
                                    return false;

                                }
                            }
                            else if (config.CommandType == SKGAuthorizeCommandProperties.EnumCommandType.下发授权)
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
                            byte xor = ByteToXOR(bufOutOxr);

                            if (oneFrameBytes[iTotalLength - 1] != xor)
                            {
                                log.Fail(string.Format("核验RX的校验位：错误,返回校验位:{0:X2},计算校验位:{1:X2}", oneFrameBytes[iTotalLength - 1], xor));
                                //继续循环执行
                                return false;
                            }
                            log.Info(string.Format("核验RX的校验位：正确,返回校验位:{0:X2},计算校验位:{1:X2}", oneFrameBytes[iTotalLength - 1], xor));

                            //数据内容 32个字节——长度是不固定的
                            byte[] dataArrys = new byte[iTotalLength - 9];//config.DataLength
                            Array.Copy(oneFrameBytes, 8, dataArrys, 0, iTotalLength - 9);//config.DataLength

                            //添加到全局变量中
                            if (config.GlobalVariblesKey != null)
                            {
                                for (int i = 0; i < config.GlobalVariblesKey.Length; i++)
                                {
                                    Match matchKey = Regex.Match(config.GlobalVariblesKey[i], this.pattern);
                                    if (!matchKey.Success)
                                    {
                                        //throw new BaseException("read info key fail");
                                        log.Info("read info key fail");
                                        //继续循环执行
                                        return false;

                                    }

                                    string key = matchKey.Groups[1].ToString();

                                    configGv.Add(key, dataArrys);
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

            //返回的数据部分处理
            byte[] dataArry=null;
            if (config.GlobalVariblesKey != null)
            {
                for (int i = 0; i < config.GlobalVariblesKey.Length; i++)
                {
                    Match matchKey = Regex.Match(config.GlobalVariblesKey[i], this.pattern);
                    if (!matchKey.Success)
                    {
                        throw new BaseException("read info key fail");
                        //log.Info("read info key fail");
                        ////继续循环执行
                        //return false;

                    }

                    string key = matchKey.Groups[1].ToString();

                   dataArry = (byte[])configGv.GetObject(key);
                }
            }
            if (dataArry==null)
            {
                throw new BaseException("未获取到回复的数据部分");
            }
            string revAuthorizeContent = byteToHexStr(dataArry).Substring(4, 48);
            string sendAuthorizeContent = command.Substring(20, 48);
            if (revAuthorizeContent != sendAuthorizeContent)
            {
                throw new BaseException(string.Format("授权失败\r\n回复的授权内容:{0}\r\n发送的授权内容:{1}\r\n不一致", revAuthorizeContent, sendAuthorizeContent));
                //log.Fail(string.Format("授权失败\r\n回复的授权内容:{0}\r\n发送的授权内容:{1}\r\n不一致", revAuthorizeContent, sendAuthorizeContent));
                ////继续循环执行
                //return false;
            }
            log.Info(string.Format("回复的授权内容:{0}\r\n发送的授权内容:{1}\r\n一致", revAuthorizeContent, sendAuthorizeContent));
            //MCU ID
            byte[] dataMcuId = new byte[12];
            Array.Copy(dataArry, 26, dataMcuId, 0, 12);
            string strDataMcuId = byteToHexStr(dataMcuId);
            log.Info(string.Format("MCU ID:{0}", strDataMcuId));
            if (strDataMcuId.Contains("00000000000000000000"))
            {
                throw new BaseException(string.Format("授权失败,数据为00的MCUID"));
                //log.Fail(string.Format("授权失败,数据为00的MCUID"));
                ////继续循环执行
                //return false;

            }
            //BLE MAC
            byte[] dataBleMac = new byte[6];
            Array.Copy(dataArry, 38, dataBleMac, 0, 6);
            //ASCII码的方式
            //string strDataMcuId = System.Text.Encoding.ASCII.GetString(dataMcuId);
            //BLE MAC：1F39A109,低位在前0x09,0xA1的直接16进制拼接方式
            string strDataBleMac = byteToHexStrReverse(dataBleMac);
            log.Info(string.Format("BLE MAC:{0}", strDataBleMac));
            log.Info(string.Format("授权成功"));



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
            //

            #region 原发送命令后等待一固定时间，再接收数据，对应不上则重试
            //int n = comDut.BytesToRead;
            //byte[] buf = new byte[n];
            //comDut.Read(buf, 0, n);
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
            //if (config.CommandType == SKGAuthorizeCommandProperties.EnumCommandType.查询指令)
            //{
            //    if (!(buf[6] == 0xA0 && buf[7] == 0x37))
            //    {
            //        throw new BaseException(string.Format("Response Command Type Error"));
            //    }
            //}
            //else if (config.CommandType == SKGAuthorizeCommandProperties.EnumCommandType.控制指令)
            //{
            //    if (!(buf[6] == 0xA0 && buf[7] == 0x40))
            //    {
            //        throw new BaseException(string.Format("Response Command Type Error"));
            //    }
            //}
            //else if (config.CommandType == SKGAuthorizeCommandProperties.EnumCommandType.下发授权)
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
            //byte[] bufOutOxr = new byte[buf.Length-1];
            //Array.Copy(buf, 0, bufOutOxr, 0, buf.Length - 1);
            //byte xor = ByteToXOR(bufOutOxr);

            //if (buf[n - 1] != xor)
            //{
            //    throw new BaseException(string.Format("核验RX的校验位：错误,返回校验位:{0:X2},计算校验位:{1:X2}", buf[n - 1], xor));
            //}
            //log.Info(string.Format("核验RX的校验位：正确,返回校验位:{0:X2},计算校验位:{1:X2}", buf[n - 1], xor));



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
            //检查是否授权正确：
            //授权事件确认检查

            //string revAuthorizeContent = byteToHexStr(dataArry).Substring(4, 48);
            //string sendAuthorizeContent = command.Substring(20, 48);
            //if (revAuthorizeContent!= sendAuthorizeContent)
            //{
            //    throw new BaseException(string.Format("授权失败\r\n回复的授权内容:{0}\r\n发送的授权内容:{1}\r\n不一致", revAuthorizeContent, sendAuthorizeContent));
            //}
            //log.Info(string.Format("回复的授权内容:{0}\r\n发送的授权内容:{1}\r\n一致", revAuthorizeContent, sendAuthorizeContent));
            ////MCU ID
            //byte[] dataMcuId = new byte[12];
            //Array.Copy(dataArry, 26, dataMcuId, 0, 12);
            ////ASCII码的方式
            ////string strDataMcuId = System.Text.Encoding.ASCII.GetString(dataMcuId);
            ////MCU ID：1F39A109高位在前1F,39的直接16进制拼接方式
            //string strDataMcuId = byteToHexStr(dataMcuId);
            //log.Info(string.Format("MCU ID:{0}", strDataMcuId));
            //if (strDataMcuId.Contains("00000000000000000000"))
            //{
            //    throw new BaseException(string.Format("授权失败,数据为00的MCUID"));
            //}
            ////BLE MAC
            //byte[] dataBleMac = new byte[6];
            //Array.Copy(dataArry, 38, dataBleMac, 0, 6);
            ////ASCII码的方式
            ////string strDataMcuId = System.Text.Encoding.ASCII.GetString(dataMcuId);
            ////BLE MAC：1F39A109,低位在前0x09,0xA1的直接16进制拼接方式
            //string strDataBleMac = byteToHexStrReverse(dataBleMac);
            //log.Info(string.Format("BLE MAC:{0}", strDataBleMac));
            //log.Info(string.Format("授权成功"));




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
