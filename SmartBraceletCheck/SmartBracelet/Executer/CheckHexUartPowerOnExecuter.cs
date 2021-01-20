using AILinkFactoryAuto.Core;
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
    public class CheckHexUartPowerOnExecuter : IExecuter
    {
        private string pattern;
        CheckHexUartPowerOnProperties config;

        private GlobalVaribles configGv;

        public CheckHexUartPowerOnExecuter()
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
            config = properties as CheckHexUartPowerOnProperties;
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
            //byte[] atCommand = strToToHexByte(config.AtCommand);
            ////string atCommand = PreTranslateAtCommand(config.AtCommand);
            //log.Info("AT Commond=" + config.AtCommand);

            //comDut.DtrEnable = configOpenPhone.Dtr;
            //comDut.RtsEnable = configOpenPhone.Rts;
            //comDut.Parity = System.IO.Ports.Parity.Even;

            TimeUtils.Execute(() =>
            {
                Thread.Sleep(config.AtCommandInterval);
                int length = comDut.ReadBufferSize;
                byte[] byteArray = new byte[length];
                comDut.Read(byteArray, 0, length);
                configGv.Add("RetBytes", byteArray);

                byte[] retArray = configGv.GetObject("RetBytes") as byte[];
                for (int i = 0; i < retArray.Length; i++)
                {//55 AA 03
                    if (retArray[i] == 0x55
                        && retArray[i + 1] == 0xAA
                        && retArray[i + 2] == 0x03
                        )
                    {
                        //flageRightRes = true;
                        //startIndex = i;
                        //break;
                        log.Info(string.Format("检测到产品上电，55 AA 03"));
                        //跳出循环执行
                        return true;
                    }
                }
                //继续循环执行
                return false;
            }, config.Timeout);

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
