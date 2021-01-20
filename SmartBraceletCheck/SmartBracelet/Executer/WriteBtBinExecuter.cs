using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using AILinkFactoryAuto.Task.SmartBracelet.Property;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Executer
{
    public class WriteBtBinExecuter : IExecuter
    {
        WriteBtBinProperties config;

        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {
            config = properties as WriteBtBinProperties;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;

            int ret = -1;
            string returnString = "";

            //
            log.Info(string.Format("擦除BT Flash中...\r\n"));
            ret = ExeCommand(config.EraseCmd, out returnString);
            if (ret != 0)
            {
                log.Fail(string.Format("详情：{0}，异常 \r\n", returnString));
                throw new BaseException(string.Format("详情：{0}，异常 \r\n", returnString));

            }
            if (returnString.Contains("ERROR"))
            {
                log.Fail(string.Format("详情：{0}\r\n执行{1}错误 \r\n", returnString, config.EraseCmd));
                throw new BaseException(string.Format("详情：{0}\r\n执行{1}错误 \r\n", returnString, config.EraseCmd));
            }

            if (!returnString.Contains("Applying system reset"))//
            {
                throw new BaseException(string.Format("详情：{0}\r\n执行{1}错误 \r\n", returnString, config.EraseCmd));
            }
            log.Info(string.Format("详情：{0}\r\n执行{1}成功 \r\n", returnString, config.EraseCmd));

            //
            log.Info(string.Format("写BT 固件中...\r\n"));
            ret = ExeCommand(string.Format(config.ProgramCmd, config.BinFileFullPath), out returnString);
            if (ret != 0)
            {
                log.Fail(string.Format("详情：{0}，异常 \r\n", returnString));
                throw new BaseException(string.Format("详情：{0}，异常 \r\n", returnString));

            }
            if (returnString.Contains("ERROR"))
            {
                log.Fail(string.Format("详情：{0}\r\n执行{1}错误 \r\n", returnString, config.ProgramCmd));
                throw new BaseException(string.Format("详情：{0}\r\n执行{1}错误 \r\n", returnString, config.ProgramCmd));
            }
            if (!returnString.Contains("Verified OK"))//
            {
                throw new BaseException(string.Format("详情：{0}\r\n执行{1}错误 \r\n", returnString, config.EraseCmd));
            }

            log.Info(string.Format("详情：{0}\r\n执行{1}成功 \r\n", returnString, config.ProgramCmd));


            //
            log.Info(string.Format("BT 应用系统复位中...\r\n"));
            ret = ExeCommand(config.ResetCmd, out returnString);
            if (ret != 0)
            {
                log.Fail(string.Format("详情：{0}，异常 \r\n", returnString));
                throw new BaseException(string.Format("详情：{0}，异常 \r\n", returnString));

            }
            if (returnString.Contains("ERROR"))
            {
                log.Fail(string.Format("详情：{0}\r\n执行{1}错误 \r\n", returnString, config.ResetCmd));
                throw new BaseException(string.Format("详情：{0}\r\n执行{1}错误 \r\n", returnString, config.ResetCmd));
            }
            if (!returnString.Contains("Applying system reset"))//
            {
                throw new BaseException(string.Format("详情：{0}\r\n执行{1}错误 \r\n", returnString, config.EraseCmd));
            }

            log.Info(string.Format("详情：{0}\r\n执行{1}成功 \r\n", returnString, config.ResetCmd));
            log.Info(string.Format("擦除BT Flash成功，写BT 固件成功，BT应用系统复位成功 \r\n"));


            //string MAC = "E000E64CCACD";//00E0 4CE6 CDCA

            //string MAC_BT = "E000E64CCACE";
            //GlobalVaribles configGv = globalDic.Get<GlobalVaribles>();
            //configGv.Add("MAC", MAC);

            //configGv.Add("MAC_BT", MAC_BT);

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

    }
}
