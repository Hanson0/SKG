using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Dut.AtCommand;
using AILinkFactoryAuto.Dut.AtCommand.Property;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using AILinkFactoryAuto.Task.SmartBracelet.Executer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class GetBtRssiAtCommandExecuter : IExecuter
    {
        private string pattern;

        private GlobalVaribles configGv;

        public GetBtRssiAtCommandExecuter()
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
            GetBtRssiAtCommandProperties config = properties as GetBtRssiAtCommandProperties;

            ComDut comDut = globalDic[typeof(ComDut).ToString()] as ComDut;
            OpenPhoneProperties configOpenPhone = globalDic[typeof(OpenPhoneProperties).ToString()] as OpenPhoneProperties;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;

            string moudleBtMac = configGv.Get("MoudleBtMac");
            string endLine = configOpenPhone.EndLine;
            if (!string.IsNullOrEmpty(endLine))
            {
                endLine = Parse_r_n(endLine);
            }

            string atCommand = PreTranslateAtCommand(config.AtCommand);
            log.Info("AT Commond=" + atCommand);

            comDut.DtrEnable = configOpenPhone.Dtr;
            comDut.RtsEnable = configOpenPhone.Rts;
            comDut.Write(atCommand + endLine);
            Thread.Sleep(config.AtCommandInterval);
            string response = comDut.ReadExisting();
            comDut.DtrEnable = false;
            comDut.RtsEnable = false;

            log.Info("AT Response=" + response);

            if (!string.IsNullOrEmpty(config.AtCommandError))
            {
                if (response.Contains(config.AtCommandError))
                {
                    throw new BaseException(string.Format("AT response=[{0}] contain error=[{1}]", response, config.AtCommandError));
                }
            }

            if (!string.IsNullOrEmpty(config.AtCommandOk))
            {
                if (!response.Contains(config.AtCommandOk))
                {
                    throw new BaseException(string.Format("AT response=[{0}] not contain error=[{1}]", response, config.AtCommandOk));
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
                            throw new BaseException(string.Format("AT response=[{0}] not contain check info=[{1}]", response, checkInfo));
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(config.GlobalVariblesKey) && !string.IsNullOrEmpty(config.GlobalVariblesKeyPattern))
            {
                string pattern = Parse_r_n(config.GlobalVariblesKeyPattern);//"E000E64CCACE:RSSI:([0-9-]{3,4})[:]"     E000E64CCACE:RSSI:-052:
                pattern = moudleBtMac + pattern;
                Match matchValue = Regex.Match(response, pattern);
                if (!matchValue.Success)
                {
                    throw new BaseException("read info value fail");
                }

                string value = matchValue.Groups[1].ToString();

                Match matchKey = Regex.Match(config.GlobalVariblesKey, this.pattern);
                if (!matchKey.Success)
                {
                    throw new BaseException("read info key fail");
                }

                string key = matchKey.Groups[1].ToString();

                configGv.Add(key, value);
            }
        }
    }
}
