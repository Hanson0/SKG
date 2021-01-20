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

namespace AILinkFactoryAuto.Task.SmartBracelet.Executer
{
    public class CmdReadMacExecuter : IExecuter
    {
        CmdReadMacProperties config;
        private GlobalVaribles configGv;
        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {
            config = properties as CmdReadMacProperties;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;
            ////取存储的关键字为MoudleBtMac的value值
            //string DevWifi_MAC = configGv.Get("DevWifi_MAC");
            //DevWifi_MAC = DevWifi_MAC.Replace(":", "").Replace("：", "");
            //string labelMAC = configGv.Get(GlobalVaribles.MAC);
            //if (DevWifi_MAC != labelMAC)
            //{
            //    throw new BaseException(string.Format("WIFI MAC：{0}，与标签MAC：{1} 不一致 FAIL\r\n", DevWifi_MAC, labelMAC));
            //}
            //log.Info(string.Format(string.Format("WIFI MAC：{0}，与标签MAC：{1} 一致 PASS\r\n", DevWifi_MAC, labelMAC)));

            int ret = -1;
            string returnString = "";

            log.Info(string.Format("读取产品MAC中...\r\n"));
            ret = ExeCommand(config.GetMacCmd, out returnString);
            if (ret != 0)
            {
                log.Fail(string.Format("详情：{0}，异常 \r\n", returnString));
                throw new BaseException(string.Format("详情：{0}，异常 \r\n", returnString));

            }
            if (returnString.Contains("ERROR")|| returnString.Contains("Error"))
            {
                log.Fail(string.Format("详情：{0}\r\n执行{1}错误 \r\n", returnString, config.GetMacCmd));
                throw new BaseException(string.Format("详情：{0}\r\n执行{1}错误,返回包含ERROR \r\n", returnString, config.GetMacCmd));
            }


            if (!returnString.Contains("ESP_MAC:"))//
            {
                throw new BaseException(string.Format("详情：{0}\r\n执行{1}错误，返回不包含: ESP_MAC:\r\n", returnString, config.GetMacCmd));
            }
            log.Info(string.Format("详情：{0}\r\n执行{1}成功 \r\n", returnString, config.GetMacCmd));
            string mac = ReadMac(returnString);
            if (mac == null)
            {
                mac= "FFFFFFFFFFFF";
                configGv.Add(config.DeviceMac, mac);

                throw new BaseException(string.Format("详情：{0}\r\n获取MAC失败 \r\n", returnString));
            }
            ////returnString = returnString.Substring(returnString.IndexOf("SYS_STAGE: 2"));
            ////正则匹配MAC
            //Match matchKey = Regex.Match(returnString, config.Pattern);//this.pattern
            //if (!matchKey.Success)
            //{
            //   throw new BaseException(string.Format("详情：{0}\r\n获取MAC失败，返回字符串未匹配获取MAC的正则 \r\n", returnString));
            //}


            //string key = matchKey.Groups[1].ToString();
            log.Info(string.Format("获取产品MAC成功：{0}\r\n", mac));

            configGv.Add(config.DeviceMac, mac);

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
                
                p.StandardInput.WriteLine(string.Format("cd  {0}",config.ToolFolder));    //将CMD命令写入StandardInput流中
                p.StandardInput.WriteLine(cmdLine);    //将CMD命令写入StandardInput流中
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
