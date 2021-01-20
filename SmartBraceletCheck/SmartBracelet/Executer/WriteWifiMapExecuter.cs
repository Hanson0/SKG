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
    public class WriteWifiMapExecuter : IExecuter
    {
        WriteWifiMapProperties config;
        private GlobalVaribles configGv;
        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {
            config = properties as WriteWifiMapProperties;
            ComDut comDut = globalDic[typeof(ComDut).ToString()] as ComDut;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;
            String mapFileString = configGv.Get("MapFileStringMacUpdate");
            //int ret = -1;
            //string returnString = "";

            //循环写
            string[] splits = { "\r", "\n", "\r\n" };
            string[] mapLine = mapFileString.Split(splits, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < mapLine.Length; i++)
            {
                //TimeUtils.Execute(() =>
                //{
                //循环写SSS
                int retry = 5;
                int retryCount = 0;
                bool result = false;
                do
                {
                    //Thread.Sleep(100);
                    comDut.Write(string.Format(config.WriteMapAtCommand, Convert.ToString(i * 0x10, 16), mapLine[i].Replace(" ", "")) + "\r\n");
                    Thread.Sleep(config.AtCommandInterval);
                    string response = comDut.ReadExisting();

                    if (!string.IsNullOrEmpty(config.AtCommandOk))
                    {
                        if (response.Contains(config.AtCommandOk) && !response.Contains(config.AtCommandError) && !response.Contains("FAIL") && !response.Contains("error"))
                        {
                            //log.Info(string.Format("，AT response=[{0}]  not contain =[{1}]", response, config.AtCommandOk));
                            //跳出循环执行
                            //return true;
                            //throw new BaseException(string.Format("AT response=[{0}] not contain error=[{1}]", response, config.AtCommandOk));
                            log.Info(string.Format("写入第{0}行MAP \r\nAT response:\r\n[{1}] \r\ncontain OK=[{2}] \r\nnot contain ERROR=[{3}] [FAIL]", i + 1, response, config.AtCommandOk, config.AtCommandError));
                            result = true;
                            break;
                        }
                        //else
                        //{
                        //    if (reTryCnt > 3)
                        //    {
                        //        throw new Exception(string.Format("写入错误 行数={0} \r\n串口返回:\r\n{1}", i + 1, response));
                        //    }
                        //}
                    }
                    Thread.Sleep(config.RetryInterval);
                    retry--;
                    retryCount++;
                    if (retry >= 0)
                    {
                        log.Info(string.Format("写入第{0}行MAP错误 \r\nAT response:\r\n[{1}] \r\ncontain OK=[{2}] \r\nnot contain ERROR=[{3}] [FAIL]", i + 1, response, config.AtCommandOk, config.AtCommandError));
                        log.Warn("重试第" + retryCount + "次");
                    }

                } while (retry >= 0);
                if (!result)
                {
                    throw new Exception(string.Format("写入错误 行数={0} \r\n", i + 1));
                }

                //comDut.Write(string.Format(config.WriteMapAtCommand, 0x00 + i * 0x10, mapLine[i].Replace(" ", "")) + "\r\n");
                //Thread.Sleep(config.AtCommandInterval);
                //string response = comDut.ReadExisting();

                //if (!string.IsNullOrEmpty(config.AtCommandOk))
                //{
                //    if (response.Contains(config.AtCommandOk) && !response.Contains(config.AtCommandError) && !response.Contains("FAIL") && !response.Contains("error"))
                //    {
                //        //log.Info(string.Format("，AT response=[{0}]  not contain =[{1}]", response, config.AtCommandOk));
                //        //跳出循环执行
                //        //return true;
                //        //throw new BaseException(string.Format("AT response=[{0}] not contain error=[{1}]", response, config.AtCommandOk));
                //        log.Info(string.Format("写入第{0}行MAP \r\nAT response:\r\n[{1}] \r\ncontain OK=[{2}] \r\nnot contain ERROR=[{3}] [FAIL]", i + 1, response, config.AtCommandOk, config.AtCommandError));

                //        //continue;
                //        //跳出循环执行
                //    }
                //    else
                //    {
                //        if (reTryCnt > 3)
                //        {
                //            throw new Exception(string.Format("写入错误 行数={0} \r\n串口返回:\r\n{1}", i + 1, response));
                //        }
                //    }
                //}



                //    //继续循环执行
                //    return false;
                //}, config.Timeout);


            }

        }

        private string PreTranslateCheckInfo(string checkInfo)
        {
            string newCheckInfo = checkInfo;
            string pattern = @"^{([0-9a-zA-Z]+)}$";
            Match match = Regex.Match(checkInfo, pattern);
            for (int i = 1; i < match.Groups.Count; i++)
            {
                string key = match.Groups[i].ToString();
                newCheckInfo = configGv.Get(key);
            }

            return newCheckInfo;
        }
    }
}
