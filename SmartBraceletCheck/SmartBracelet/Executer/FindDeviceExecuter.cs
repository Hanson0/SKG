using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Dut.AtCommand;
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
    public class FindDeviceExecuter : IExecuter
    {
        FindDeviceProperties config;
        private GlobalVaribles configGv;
        private string pattern;
        public FindDeviceExecuter()
        {
            this.pattern = GlobalVaribles.PATTERN;
        }
        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {
            config = properties as FindDeviceProperties;
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


            ILog log = globalDic[typeof(ILog).ToString()] as ILog;

            //int ret = -1;
            //string returnString = "";
            string endLine = config.EndLine;
            string response = "";
            TimeUtils.Execute(() =>
            {
                comDut.Write(config.TestPowerOnAT + endLine);
                Thread.Sleep(config.AtCommandInterval);
                response += comDut.ReadExisting();
                if (!string.IsNullOrEmpty(config.AtCommandOk))
                {
                    if (response.Contains(config.AtCommandOk))
                    {
                        log.Info(string.Format("检测到产品上电，AT response=[{0}]  contain =[{1}]", response, config.AtCommandOk));
                        //跳出循环执行
                        return true;
                    }
                }
                //继续循环执行
                return false;
            }, config.Timeout);

            if (!string.IsNullOrEmpty(config.AtCommandError))
            {
                if (response.Contains(config.AtCommandError))
                {
                    throw new BaseException(string.Format("AT response contain error:\r\n[{0}]", config.AtCommandError));
                }
            }


            if (config.CheckInfo != null)
            {
                for (int i = 0; i < config.CheckInfo.Length; i++)
                {
                    if (!string.IsNullOrEmpty(config.CheckInfo[i]))
                    {
                        string checkInfo = PreTranslateCheckInfo(config.CheckInfo[i]);
                        if (!response.Contains(checkInfo))
                        {
                            throw new BaseException(string.Format("AT response not contain check info:\r\n{0}", checkInfo));
                        }
                        log.Info(string.Format("AT response contain check info:\r\n{0}", checkInfo));
                    }
                }
            }
            if (config.GlobalVariblesKey != null && config.GlobalVariblesKeyPattern != null)
            {
                if (config.GlobalVariblesKey.Length != config.GlobalVariblesKey.Length)
                {
                    throw new BaseException("GlobalVariblesKey's length !=  GlobalVariblesKeyPattern's length");
                }
                for (int i = 0; i < config.GlobalVariblesKey.Length; i++)
                {
                    if (!string.IsNullOrEmpty(config.GlobalVariblesKey[i]))
                    {
                        string pattern = Parse_r_n(config.GlobalVariblesKeyPattern[i]);
                        //response = response.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
                        Match matchValue = Regex.Match(response, pattern);
                        if (!matchValue.Success)
                        {
                            throw new BaseException(string.Format("read info {0} value fail", i));
                        }
                        string value = matchValue.Groups[1].ToString();
                        Match matchKey = Regex.Match(config.GlobalVariblesKey[i], this.pattern);
                        if (!matchKey.Success)
                        {
                            throw new BaseException(string.Format("read info {0} key fail", i));
                        }
                        string key = matchKey.Groups[1].ToString();
                        configGv.Add(key, value);

                    }
                }
            }

            //configGv.Add("PowerOnLog", response);




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
    }
}
