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
    public class CheckHexUartExecuter : IExecuter
    {
        private string pattern;
        CheckHexUartProperties config;

        private GlobalVaribles configGv;

        public CheckHexUartExecuter()
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
            config = properties as CheckHexUartProperties;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;
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


            string endLine = configOpenPhone.EndLine;
            if (!string.IsNullOrEmpty(endLine))
            {
                endLine = Parse_r_n(endLine);
            }
            byte[] atCommand = strToToHexByte(config.AtCommand);
            //string atCommand = PreTranslateAtCommand(config.AtCommand);
            log.Info("AT Commond=" + config.AtCommand);

            //comDut.DtrEnable = configOpenPhone.Dtr;
            //comDut.RtsEnable = configOpenPhone.Rts;
            //comDut.Parity = System.IO.Ports.Parity.Even;
            comDut.Write(atCommand, 0, atCommand.Length);
            log.Info("Tx:" + config.AtCommand);

            Thread.Sleep(config.AtCommandInterval);
            int length = comDut.ReadBufferSize;
            byte[] byteArray = new byte[length];
            comDut.Read(byteArray, 0 , length);
            #region
            //string hexStr = byteToHexStr(byteArray);
            ////log.Info("Rx:" + hexStr);
            //string response = System.Text.Encoding.ASCII.GetString(byteArray);
            ////response = response.Replace("\0", "~");
            ////string response = comDut.ReadExisting();
            ////byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(response);
            configGv.Add("RetBytes", byteArray);

            //comDut.DtrEnable = false;
            //comDut.RtsEnable = false;

            ////log.Info("Response:\r\n" + response);

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

            //if (!string.IsNullOrEmpty(config.GlobalVariblesKey) && !string.IsNullOrEmpty(config.GlobalVariblesKeyPattern))
            //{
            //    string pattern = Parse_r_n(config.GlobalVariblesKeyPattern);
            //    Match matchValue = Regex.Match(response, pattern);
            //    if (!matchValue.Success)
            //    {
            //        throw new BaseException("read info value fail");
            //    }

            //    string value = matchValue.Groups[1].ToString();

            //    Match matchKey = Regex.Match(config.GlobalVariblesKey, this.pattern);
            //    if (!matchKey.Success)
            //    {
            //        throw new BaseException("read info key fail");
            //    }

            //    string key = matchKey.Groups[1].ToString();

            //    configGv.Add(key, value);
            //}
            #endregion

             
            byte[] retArray = configGv.GetObject("RetBytes") as byte[];
            int startIndex = -1;
            //bool flageRightRes = false;
            for (int i = 0; i < retArray.Length; i++)
            {
                if (retArray[i] == 0xA6
                    && retArray[i + 1] == 0xA6)
                {
                    //flageRightRes = true;
                    startIndex = i;
                    break;

                    ////记录i的位置
                    //////核验校验位
                    ////sum += byteArray[i];
                }
            }

            //byte[] dataArray = new byte[6];

            //Array.Copy(retArray, startIndex, dataArray, 0, 6);
            //hexStr = byteToHexStr(dataArray);

            //log.Info(string.Format("收到7A 7A 21 D1 1B的回复:{0}\r\n", hexStr));

            //log.Info(string.Format("收到7e 7e 03 bc 94 53的回复:{0}\r\n", hexStr));


            if (startIndex == -1)
            {
                throw new BaseException("没有发现A6 A6 的回复");
            }


            byte[] versionBtyes = new byte[3];
            Array.Copy(retArray, startIndex+3, versionBtyes, 0, 3);


            string strVersion = versionBtyes[2].ToString() + "." + versionBtyes[1].ToString() + "." + versionBtyes[0].ToString();
            configGv.Add("VERSION", strVersion);
            log.Info(string.Format("版本号:{0}", strVersion));


            byte[] wifiMacBtyes = new byte[12];
            Array.Copy(retArray, startIndex + 6, wifiMacBtyes, 0, 12);
            string strImei = System.Text.Encoding.ASCII.GetString(wifiMacBtyes);
            configGv.Add("DevWifi_MAC", strImei);
            log.Info("WIFI MAC:" + strImei);

            byte[] softNumberBtyes = new byte[9];
            Array.Copy(retArray, startIndex + 18, softNumberBtyes, 0, 9);
            string strSoftNumber = System.Text.Encoding.ASCII.GetString(softNumberBtyes);
            configGv.Add("SoftNumber", strSoftNumber);
            log.Info("软件编号:" + strSoftNumber);


            byte[] resultBtyes = new byte[4];
            Array.Copy(retArray, startIndex + 27, resultBtyes, 0, 4);
            string strResult = System.Text.Encoding.ASCII.GetString(resultBtyes);
            configGv.Add("RESULT", strResult);
            log.Info("联网测试结果:" + strResult);


            byte[] btMacBtyes = new byte[12];
            Array.Copy(retArray, startIndex + 31, btMacBtyes, 0, 12);
            string strBtMac = System.Text.Encoding.ASCII.GetString(btMacBtyes);
            log.Info("BT MAC:" + strBtMac);
            configGv.Add("DevBt_MAC", strBtMac);

            byte[] rssiBtyes = new byte[1];
            Array.Copy(retArray, startIndex + 43, rssiBtyes, 0, 1);

            string strRssi = "-"+ rssiBtyes[0].ToString();

            log.Info("信号强度:" + strRssi);
            configGv.Add("RSSI", strRssi);


            //////byte[] iccidBtyes = new byte[20];
            //////Array.Copy(retArray, startIndex + 88, iccidBtyes, 0, 20);
            //////string strIccid = System.Text.Encoding.ASCII.GetString(iccidBtyes);

            //////log.Info("ICCID:" + strIccid);
            //////if (strImsi == "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0")
            //////{
            //////    throw new BaseException("ICCID为空，获取失败");
            //////}
            //////configGv.Add("ICCID", strIccid);

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
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string byteToHexStr(byte[] bytes)
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
    }
}
