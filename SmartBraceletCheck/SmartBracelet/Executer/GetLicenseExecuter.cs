using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Dut.AtCommand;
using AILinkFactoryAuto.Dut.AtCommand.Property;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using AILinkFactoryAuto.Task.SmartBracelet.Property;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace AILinkFactoryAuto.Task.SmartBracelet.Executer
{
    public class GetLicenseExecuter : IExecuter
    {
        private string pattern;

        private GlobalVaribles configGv;
        public GetLicenseExecuter()
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
            GetLicenseProperties config = properties as GetLicenseProperties;

            ComDut comDut = globalDic[typeof(ComDut).ToString()] as ComDut;
            OpenPhoneProperties configOpenPhone = globalDic[typeof(OpenPhoneProperties).ToString()] as OpenPhoneProperties;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;

            SerialPort comlog = new SerialPort();
            comlog.PortName = config.PortName;
            comlog.DataBits = 8;
            comlog.StopBits = StopBits.One;
            comlog.Parity = Parity.None;
            comlog.BaudRate = config.BaudRate;
            //comlog.DtrEnable = config.Dtr;
            //comlog.RtsEnable = config.Rts;
            comlog.Open();

            //string moudleBtMac = configGv.Get("MAC_BT");
            string endLine = config.EndLine;
            if (!string.IsNullOrEmpty(endLine))
            {
                endLine = Parse_r_n(endLine);
            }

            string atCommand = PreTranslateAtCommand(config.AtCommand);
            log.Info("AT Commond=" + atCommand);

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

            //log口获取接收
            string responseLog = comlog.ReadExisting();
            if (config.IsToUpper)
            {
                responseLog = responseLog.ToUpper();
            }
            responseLog=responseLog.Replace("\r\n", "");

            if (config.CheckInfo != null)
            {
                for (int i = 0; i < config.CheckInfo.Length; i++)
                {
                    if (!string.IsNullOrEmpty(config.CheckInfo[i]))
                    {
                        string checkInfo = PreTranslateCheckInfo(config.CheckInfo[i]);
                        if (!responseLog.Contains(checkInfo))
                        {
                            throw new BaseException(string.Format("responseLog =\r\n{0} \r\n不包含License文件:\r\n{1}", responseLog, checkInfo));
                        }
                    }
                }
            }
            log.Info("LOG Response：\r\n" + responseLog);

            if (comlog.IsOpen)
            {
                comlog.Close();
            }


        }
    }
}
