using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using AILinkFactoryAuto.Task.SmartBracelet.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AILinkFactoryAuto.Task.SmartBracelet.Executer
{
    public class FilterOutCharsExecuter : IExecuter
    {
        FilterOutCharsProperties config;
        private GlobalVaribles configGv;
        private ILog log;
        private string pattern;
        public FilterOutCharsExecuter()
        {
            this.pattern = GlobalVaribles.PATTERN;
        }
        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {
            config = properties as FilterOutCharsProperties;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;
            log = globalDic[typeof(ILog).ToString()] as ILog;

            if (config.GlobalVariblesKey != null )
            {
                for (int i = 0; i < config.GlobalVariblesKey.Length; i++)
                {
                    if (!string.IsNullOrEmpty(config.GlobalVariblesKey[i]))
                    {
                        Match matchKey = Regex.Match(config.GlobalVariblesKey[i], this.pattern);
                        if (!matchKey.Success)
                        {
                            throw new BaseException(string.Format("read info {0} key fail", i));
                        }
                        string key = matchKey.Groups[1].ToString();

                        string value= configGv.Get(key);
                        for (int j = 0; j < config.FilterOutChars.Length; j++)
                        {
                            value = value.Replace(config.FilterOutChars[j], "");
                        }
                        configGv.Add(key, value);
                    }
                }
            }
        }

        private string Parse_r_n(string value)
        {
            value = value.Replace("\\r", "\r");
            value = value.Replace("\\R", "\r");
            value = value.Replace("\\n", "\n");
            value = value.Replace("\\N", "\n");
            return value;
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

    }
}
