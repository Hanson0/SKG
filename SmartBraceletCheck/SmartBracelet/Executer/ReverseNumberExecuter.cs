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

    public class ReverseNumberExecuter : IExecuter
    {
        private static object myLock = new object();

        ReverseNumberProperties config;
        private GlobalVaribles configGv;
        //private string pattern;

        //public ReverseNumberExecuter()
        //{
        //    this.pattern = GlobalVaribles.PATTERN;
        //}

        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {
            config = properties as ReverseNumberProperties;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;
            try
            {
                string number = configGv.Get(config.PreGlobalVariable);
                //string mac = configGv.Get(GlobalVaribles.MAC);
                byte[] byteMac = strToToHexByte(number);
                Array.Reverse(byteMac);

                string reverseMac= byteToHexStr(byteMac);
                configGv.Add(config.AfterGlobalVariable, reverseMac);

                log.Info(string.Format("倒序16进制Number成功：{0}", reverseMac));
            }
            catch (Exception ex)
            {
                throw new BaseException(string.Format("倒序16进制Number转换失败，{0}", ex));
            }


            //string reverseMac = mac.Substring(0, 12);
            //string reScanLabelMac = reScanLabel.Substring(12, 12);


            //string macLabel = configGv.Get(GlobalVaribles.MAC);
            //if (!(reScanLabelFixedPart == config.FixedPart))
            //{
            //    throw new BaseException(string.Format("重扫标签的固定部分：{0}与标准固定部分：{1}不一致", reScanLabelFixedPart, config.FixedPart));
            //}
            //log.Info(string.Format("重扫标签的固定部分：{0}与标准固定部分：{1}一致", reScanLabelFixedPart, config.FixedPart));

            //if (!(reScanLabelMac == macLabel))
            //{
            //    throw new BaseException(string.Format("重扫标签的MAC部分：{0}与标签MAC：{1}不一致", reScanLabelMac, macLabel));
            //}
            //log.Info(string.Format("重扫标签的MAC部分：{0}与标签MAC：{1}一致", reScanLabelMac, macLabel));

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
        //private string PreTranslateGlobalVaribles(string checkInfo)
        //{
        //    string newCheckInfo = checkInfo;
        //    Match match = Regex.Match(checkInfo, pattern);
        //    for (int i = 1; i < match.Groups.Count; i++)
        //    {
        //        string key = match.Groups[i].ToString();
        //        newCheckInfo = configGv.Get(key);
        //    }

        //    return newCheckInfo;
        //}

    }


}
