using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using AILinkFactoryAuto.Task.SmartBracelet.Property;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace AILinkFactoryAuto.Task.SmartBracelet.Executer
{
    public class CmdExecuter : IExecuter
    {
        private string pattern;

        CmdProperties config;
        private GlobalVaribles configGv;
        public CmdExecuter()
        {
            this.pattern = GlobalVaribles.PATTERN;
        }

        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {
            config = properties as CmdProperties;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;

            int ret = -1;
            string returnString = "";

            log.Info("CMD Commond=\r\n" + config.Command);
            ret = ExeCommand(config.Command, out returnString);

            log.Info("CMD Response=\r\n" + returnString);

            if (!string.IsNullOrEmpty(config.CommandError))
                {
                    if (returnString.Contains(config.CommandError))
                    {
                        throw new BaseException(string.Format("CMD response,contain error=[{0}]", config.CommandError));
                    }
                }

                if (!string.IsNullOrEmpty(config.CommandOk))
                {
                    if (!returnString.Contains(config.CommandOk))
                    {
                        throw new BaseException(string.Format("CMD response,not contain Ok=[{0}]", config.CommandOk));
                    }
                }

                if (config.CheckInfo != null)
                {
                    for (int i = 0; i < config.CheckInfo.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(config.CheckInfo[i]))
                        {
                            //string checkInfo = PreTranslateCheckInfo(config.CheckInfo[i]);
                            string checkInfo = config.CheckInfo[i];
                            if (!returnString.Contains(checkInfo))
                            {
                                throw new BaseException(string.Format("CMD response,not contain check info=[{0}]", checkInfo));
                            }
                            log.Info(string.Format("contain check info=[{0}]", checkInfo));
                        }
                    }
                }

                if (config.GlobalVariblesKey != null && config.GlobalVariblesKeyPattern != null)
                {
                    if (config.GlobalVariblesKey.Length != config.GlobalVariblesKeyPattern.Length)
                    {
                        throw new BaseException("请保持GlobalVariblesKey与GlobalVariblesKeyPattern成对存在");
                    }
                    for (int i = 0; i < config.GlobalVariblesKey.Length; i++)
                    {
                        string pattern = Parse_r_n(config.GlobalVariblesKeyPattern[i]);
                        Match matchValue = Regex.Match(returnString, pattern);
                        if (!matchValue.Success)
                        {
                            throw new BaseException("read info value fail");
                        }

                        string value = matchValue.Groups[1].ToString();

                        Match matchKey = Regex.Match(config.GlobalVariblesKey[i], this.pattern);
                        if (!matchKey.Success)
                        {
                            throw new BaseException("read info key fail");
                        }

                        string key = matchKey.Groups[1].ToString();

                        configGv.Add(key, value);
                    }
                }


        }

        public int ExeCommand(string cmdLine, out string strOutput)
        {
            int ret = -1;
            Process p = new Process();  //创建并实例化一个操作进程的类：Process
            p.StartInfo.FileName = "cmd.exe";    //设置要启动的应用程序
            p.StartInfo.UseShellExecute = false;   //设置是否使用操作系统shell启动进程
            p.StartInfo.RedirectStandardInput = true;  //指示应用程序是否从StandardInput流中读取
            p.StartInfo.RedirectStandardOutput = true; //将应用程序的输入写入到StandardOutput流中
            p.StartInfo.RedirectStandardError = true;  //将应用程序的错误输出写入到StandarError流中
            p.StartInfo.CreateNoWindow = true;    //是否在新窗口中启动进程
            //string strOutput = null;
            try
            {

                p.Start();
                //p.StandardInput.WriteLine("C:");    //将CMD命令写入StandardInput流中

                p.StandardInput.WriteLine(string.Format("cd  {0}", config.ToolFolder));    //将CMD命令写入StandardInput流中
                if (config.IsMicorsoftInit)
                {
                    p.StandardInput.WriteLine("InitializeCommandPrompt");
                }
                p.StandardInput.WriteLine(cmdLine);    //将CMD命令写入StandardInput流中
                Thread.Sleep(config.CommandDelayTime);
                p.StandardInput.WriteLine("exit");         //将 exit 命令写入StandardInput流中

                strOutput = p.StandardOutput.ReadToEnd();   //读取所有输出的流的所有字符
                p.WaitForExit();                           //无限期等待，直至进程退出
                p.Close();                                  //释放进程，关闭进程
                ret = 0;
            }
            catch (Exception e)
            {
                strOutput = e.Message;
                ret = -1;
                return ret;
            }
            return ret;

        }
        private string Parse_r_n(string value)
        {
            value = value.Replace("\\r", "\r");
            value = value.Replace("\\R", "\r");
            value = value.Replace("\\n", "\n");
            value = value.Replace("\\N", "\n");
            return value;
        }

        public string ReadMac(string recive)
        {
            string mac = null;
            //string recive = getMacFromExe();
            if (!string.IsNullOrEmpty(recive) && recive.Contains("ESP_MAC:"))
            {
                string[] reviceRows = recive.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var row in reviceRows)
                {
                    if (row.Contains("ESP_MAC:"))
                    {
                        mac = row.Replace(" ", "").Replace(":", "").Replace(":", "").Replace(":", "").Replace(":", "").Replace(":", "");
                        mac = mac.Substring(7, 12);
                        if (mac != "000000000000")
                        {
                            return mac;
                        }
                    }
                }
            }
            return null;
        }

    }
}
