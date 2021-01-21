using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Dut.AtCommand;
using AILinkFactoryAuto.Dut.AtCommand.Property;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using AILinkFactoryAuto.Task.SmartBracelet.Property;
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
            int allLength = 9 + config.DataLength;
            byte[] byteTemp = new byte[2];
            byte[] oneByteTemp = new byte[1];

            byteTemp = intToBytes(allLength);
            command += byteToHexStr(byteTemp);

            command += config.ReservedWord;
            command += config.CommandWord;

            //DataContent部分
            //枚举类的处理
            int intTemp = (int)config.PowerOnOffSetting;
            command += intTemp.ToString().PadLeft(2,'0');

            intTemp = (int)config.LedModeSetting;
            command += intTemp.ToString().PadLeft(2, '0');

            intTemp = (int)config.EmsTestSwitch;
            command += intTemp.ToString().PadLeft(2, '0');

            //int型的处理
            byteTemp = intToBytes(config.EmsPWSetting);
            command += byteToHexStr(byteTemp);

            byteTemp = intToBytes(config.EmsFreqSetting);
            command += byteToHexStr(byteTemp);

            byteTemp = intToBytes(config.EmsAmplitudeSetting);
            command += byteToHexStr(byteTemp);

            //枚举类的处理
            intTemp = (int)config.HeatingGearControl;
            command += intTemp.ToString().PadLeft(2, '0');

            intTemp = (int)config.VoiceControl;
            command += intTemp.ToString().PadLeft(2, '0');

            intTemp = (int)config.WritePcbaFinishFlag;
            command += intTemp.ToString().PadLeft(2, '0');

            intTemp = (int)config.WholeMachineFinishFlag;
            command += intTemp.ToString().PadLeft(2, '0');

            intTemp = (int)config.BtTestOnOffSetting;
            command += intTemp.ToString().PadLeft(2, '0');

            intTemp = (int)config.MotorControl;
            command += intTemp.ToString().PadLeft(2, '0');

            intTemp = (int)config.AginTestOnOffSetting;
            command += intTemp.ToString().PadLeft(2, '0');

            //int型的处理-转为1字节byte
            oneByteTemp = intToOneBytes(config.AginTestTime);
            command += byteToHexStr(byteTemp);
            #region 多路振动控制
            intTemp = (int)config.VibrationControl1;
            command += intTemp.ToString().PadLeft(2, '0');

            intTemp = (int)config.VibrationControl2;
            command += intTemp.ToString().PadLeft(2, '0');

            intTemp = (int)config.VibrationControl3;
            command += intTemp.ToString().PadLeft(2, '0');

            intTemp = (int)config.VibrationControl4;
            command += intTemp.ToString().PadLeft(2, '0');

            intTemp = (int)config.VibrationControl5;
            command += intTemp.ToString().PadLeft(2, '0');

            intTemp = (int)config.VibrationControl6;
            command += intTemp.ToString().PadLeft(2, '0');

            intTemp = (int)config.VibrationControl7;
            command += intTemp.ToString().PadLeft(2, '0');

            intTemp = (int)config.VibrationControl8;
            command += intTemp.ToString().PadLeft(2, '0');

            intTemp = (int)config.VibrationControl9;
            command += intTemp.ToString().PadLeft(2, '0');

            intTemp = (int)config.VibrationControl10;
            command += intTemp.ToString().PadLeft(2, '0');

            intTemp = (int)config.VibrationControl11;
            command += intTemp.ToString().PadLeft(2, '0');

            intTemp = (int)config.VibrationControl12;
            command += intTemp.ToString().PadLeft(2, '0');

            intTemp = (int)config.VibrationControl13;
            command += intTemp.ToString().PadLeft(2, '0');

            intTemp = (int)config.VibrationControl4;
            command += intTemp.ToString().PadLeft(2, '0');

            intTemp = (int)config.VibrationControl5;
            command += intTemp.ToString().PadLeft(2, '0');

            intTemp = (int)config.VibrationControl6;
            command += intTemp.ToString().PadLeft(2, '0');
            #endregion
            intTemp = (int)config.RedLightControl640nm;
            command += intTemp.ToString().PadLeft(2, '0');


            //byteToHexStr();

            //byte[] atCommand = strToToHexByte(config.AtCommand);
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

            int n = comDut.BytesToRead;
            byte[] buf = new byte[n];
            comDut.Read(buf, 0, n);
            #region
            ////1.缓存数据
            //buffer.AddRange(buf);

            //if (!flagCmdHandle)
            //{
            //    //如果接受到的上一帧数据未处理，软件只接受数据进buffer，不提取指令
            //    return;
            //}

            //2.完整性判断
            //while (buffer.Count >= 9) //至少包含帧头（2字节）、长度（1字节）、校验位（1字节）；根据设计不同而不同
            //{
            //    //2.1 查找数据头
            //    if (buffer[0] == 0xAA) //传输数据有帧头1，用于判断
            //    {
            //        //if (buffer[1] == 0xFF) //传输数据有帧头2，用于判断
            //        //{
            //        int iDataLength = buffer[1];

            //        int iTotalLength = iDataLength + 1;//前两个与最后一个

            //        int cnt = 0;
            //        //for (int i = 0; i < buffer.Count; i++)
            //        //{
            //        //    if (buffer[i] == 0x55 && buffer[i - 1] == 0xFF)
            //        //    {
            //        //        cnt++;
            //        //    }
            //        //}

            //        //if (buffer.Count>)
            //        //{

            //        //}

            //        if (buffer.Count < iTotalLength + cnt) //数据区尚未接收完整
            //        {
            //            break;
            //        }

            //        //得到完整的数据，复制到rawBytes中进行校验

            //        iTotalLength = buffer.Count;

            //        byte[] rawBytes = new byte[iTotalLength];
            //        buffer.CopyTo(0, rawBytes, 0, iTotalLength);
            //        buffer.Clear();

            //        //if (buffer[10] == 0x01)
            //        //{

            //        //    byte[] rawBytes = { 0xFF, 0xFF, 0x2A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xA5, 0x01, 0x04, 0x09, 0xE0, 0xB6, 0xF5, 0x18, 0x8D, 0x40, 0x13, 0x78, 0x0D, 0xE4, 0xDE, 0xE1, 0x15, 0x96, 0x4A, 0x3E, 0xDC, 0x1C, 0xE1, 0xAA, 0xFF, 0x55, 0x85, 0xDB, 0xC6, 0xFD, 0x76, 0x32, 0x39, 0x6F, 0x8E, 0x67, 0xFA };
            //        //}

            //        byte checksum = 0; //开始校验

            //        //if (rawBytes[rawBytes.Length - 1] == 0x55 && rawBytes[rawBytes.Length - 2] == 0xFF)
            //        //{
            //        //    for (int i = 2; i < rawBytes.Length - 2; i++)
            //        //    {
            //        //        checksum += rawBytes[i];
            //        //    }

            //        //    if (checksum != rawBytes[rawBytes.Length - 2]) //校验失败，最后一个字节是校验位
            //        //    {
            //        //        UartDisplay("Rx Error\r\n");
            //        //        continue;
            //        //    }
            //        //}
            //        //else
            //        //{
            //        for (int i = 1; i < rawBytes.Length - 1; i++)
            //        {
            //            checksum += rawBytes[i];
            //        }
            //        checksum = (byte)~checksum;
            //        checksum += 0x01;
            //        if (checksum != rawBytes[rawBytes.Length - 1]) //校验失败，最后一个字节是校验位
            //        {
            //            UartDisplay("Rx Error\r\n");
            //            continue;
            //        }
            //        //}

            //        UartDisplay(rawBytes, "R");


            //        //ReceiveBytes = new byte[rawBytes.Length];
            //        //rawBytes.CopyTo(ReceiveBytes, 0);
            //        flagCmdHandle = false;
            //        RecCmd_AC(rawBytes);
            //        //}
            //        rawBytes = null;
            //        flagCmdHandle = true;
            //    }
            //    else //帧头1不正确时，记得清除
            //    {
            //        buffer.RemoveAt(0);
            //    }
            //}
            #endregion

            //解析是否收到正确的包,包头，命令，校验位
            //起始标志 数据长度 保留字     命令字     数据内容    异或校验码 
            //0XA5C3    9+N     0x0000      0x0037      Data        异或 
            //2 字节  2 字节    2 字节      2 字节      N 字节     1 字节
            UartDisplay(buf, "R");
            if (buf.Length < 9)
            {
                throw new BaseException(string.Format("Response Length Not enough"));
            }
            //包头
            if (!(buf[0] == 0xA5 && buf[1] == 0xC3))
            {
                throw new BaseException(string.Format("Response Head Error"));
            }
            //长度 2字节
            byte high = buf[2];
            byte low = buf[3];
            int iTotalLength = (high & 0xFF) << 8 | low;

            if (iTotalLength != n)
            {
                throw new BaseException(string.Format("Response Length Error,长度位：{0},实际长度:{1}", iTotalLength, n));
            }

            //命令字
            if (config.CommandType == SKGControlCommandProperties.EnumCommandType.查询指令)
            {
                if (!(buf[6] == 0xA0 && buf[7] == 0x37))
                {
                    throw new BaseException(string.Format("Response Command Type Error"));
                }
            }
            else if (config.CommandType == SKGControlCommandProperties.EnumCommandType.控制指令)
            {
                if (!(buf[6] == 0xA0 && buf[7] == 0x40))
                {
                    throw new BaseException(string.Format("Response Command Type Error"));
                }
            }
            else if (config.CommandType == SKGControlCommandProperties.EnumCommandType.下发授权)
            {
                if (!(buf[6] == 0xA0 && buf[7] == 0x23))
                {
                    throw new BaseException(string.Format("Response Command Type Error"));
                }
            }
            else
            {
                throw new BaseException(string.Format("Response为非法指令类型"));
            }


            //异或校验 1字节
            byte[] bufOutOxr = new byte[buf.Length - 1];
            Array.Copy(buf, 0, bufOutOxr, 0, buf.Length - 1);
            byte xor = ByteToXOR(bufOutOxr);

            if (!(buf[n - 1] != xor))
            {
                throw new BaseException(string.Format("Response 校验位不正确"));
            }



            //数据内容 32个字节
            byte[] dataArry = new byte[config.DataLength];
            Array.Copy(buf, 8, dataArry, 0, config.DataLength);
            //添加到全局变量中
            if (config.GlobalVariblesKey != null)
            {
                for (int i = 0; i < config.GlobalVariblesKey.Length; i++)
                {
                    //string pattern = "-([0-9]{2}) ";
                    //Match matchValue = Regex.Match(response, pattern);
                    //if (!matchValue.Success)
                    //{
                    //    throw new BaseException("read info value fail");
                    //}

                    //string value = matchValue.Groups[1].ToString();

                    Match matchKey = Regex.Match(config.GlobalVariblesKey[i], this.pattern);
                    if (!matchKey.Success)
                    {
                        throw new BaseException("read info key fail");
                    }

                    string key = matchKey.Groups[1].ToString();

                    configGv.Add(key, dataArry);
                    log.Info("应答报文正确");
                }
            }
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
