using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using AILinkFactoryAuto.Task.SmartBracelet.Property;
using AILinkFactoryAuto.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace AILinkFactoryAuto.Task.SmartBracelet.Executer
{
    //class CmdSplThdCheckExecuter
    public class CmdSplThdCheckExecuter : IExecuter
    {
        private string pattern;

        CmdSplThdCheckProperties config;
        private GlobalVaribles configGv;
        private string dataRev="";
        ILog log;
        public CmdSplThdCheckExecuter()
        {
            this.pattern = GlobalVaribles.PATTERN;
        }

        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {
            config = properties as CmdSplThdCheckProperties;
            log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;

            int ret = -1;
            string returnString = "";

            log.Info("CMD Commond=\r\n" + config.Command);
            //string cmd = String.Format("HiBurnV3.1.exe -com:{0} -bin:{1} -signalbaud:2000000 -2ms -forceread:10 -timeout:{2} -console", listPortsName[index - 1].Replace("COM", ""), textBox1.Text, powerOnTimeout);
            ////%ERRORLEVEL%
            //CmdThread cmdThread;
            string path = string.Format("./adb/mic.bat");
            string[] contentSrtList = new string[] { config.Command};
            try
            {
                WriteForTxt(path, contentSrtList);
            }
            catch (Exception ex)
            {
                throw new BaseException(string.Format("MIC测试,写bat失败：{0}", ex.Message));
            }
            string fileName = Path.GetFileName(path);

            dataRev = "";
            ret = ExeCommand(fileName, out returnString);
            TimeUtils.Execute(() =>
            {
                if (string.IsNullOrEmpty(dataRev))
                {
                    return false;
                }
                if (!string.IsNullOrEmpty(config.CommandError))
                {
                    if (dataRev.Contains(config.CommandError))
                    {
                        ret = -1;
                        //跳出循环执行
                        return true;

                    }
                }

                if (!string.IsNullOrEmpty(config.CommandOk))
                {
                    if (dataRev.Contains(config.CommandOk))
                    {
                        log.Info(string.Format("ADB Response接收完毕：\r\ncontain：{0}", config.CommandOk));
                        ret = 0;

                        //跳出循环执行
                        return true;
                    }
                }

                //if (returnString.Contains("\tdevice\r\n\r\n"))
                //{
                //    log.Info(string.Format("详情：{0}，检测到设备 \r\n", returnString));
                //    //跳出循环执行
                //    return true;
                //}

                //继续循环执行
                return false;
            }, config.Timeout);
            if (ret!=0)
            {
                throw new BaseException(string.Format("CMD response,contain error=[{0}]", config.CommandError));

            }

            if (config.CheckInfo != null)
            {
                for (int i = 0; i < config.CheckInfo.Length; i++)
                {
                    if (!string.IsNullOrEmpty(config.CheckInfo[i]))
                    {
                        //string checkInfo = PreTranslateCheckInfo(config.CheckInfo[i]);
                        string checkInfo = config.CheckInfo[i];
                        if (!dataRev.Contains(checkInfo))
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
                    Match matchValue = Regex.Match(dataRev, pattern);
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
            //
            string[] splits = new string[] {"channel" };
            string[] channelDataLists = dataRev.Split(splits, StringSplitOptions.RemoveEmptyEntries);

            string[] splitsLine = new string[] { "\r\n" };
            bool channelContain984 = false;

            float maxSpl = -100;
            float minSpl = -1;

            float maxThd = 0;
            float minThd = 100;
            for (int i = 0; i < channelDataLists.Length-1; i++)
            {
                //一个通道
                channelContain984 = false;
                string[] eachLines= channelDataLists[i].Split(splitsLine, StringSplitOptions.RemoveEmptyEntries);

                ////本通道最大,最小spl,thd
                //float thisChanelMaxSpl = -100;
                //float thisChanelMinSpl = -1;

                //float thisChanelMaxThd = 0;
                //float thisChanelMinThd = 100;
                for (int j = 0; j < eachLines.Length; j++)
                {
                    if (eachLines[j].Contains(config.CheckFreqValue.ToString()))
                    {
                        channelContain984 = true;
                        //解析每个通道的spl thd值
                        //281	 -52.983902	 18.399385
                        //bool isMatch = Regex.IsMatch(eachLines[j], config.CheckFreqValue.ToString() + "\t ([-.0-9]{9,10})\t ([.0-9]{8,9})");
                        //if (!isMatch)
                        //{
                        //    throw new BaseException(string.Format("channel{0}:{1}HZ所在行不匹配正则格式", i, config.CheckFreqValue));
                        //}

                        //MatchCollection matchValue = Regex.Matches(eachLines[j], config.CheckFreqValue.ToString()+"\t ([-.0-9]{9,10})\t ([.0-9]{8,9})");
                        //float[] result = new float[matchValue.Count];
                        //if (matchValue.Count > 0)
                        //{
                        //    for (int k = 0; k < matchValue.Count; k++)
                        //    {
                        //        float splValue = float.Parse(matchValue[k].Groups[1].ToString());
                        //        float thdValue = float.Parse(matchValue[k].Groups[2].ToString());
                        //        if (splValue > thisChanelMaxSpl)
                        //        {
                        //            thisChanelMaxSpl = splValue;
                        //        }
                        //        if (splValue < thisChanelMinSpl)
                        //        {
                        //            thisChanelMinSpl = splValue;
                        //        }

                        //        if (thdValue > thisChanelMaxThd)
                        //        {
                        //            thisChanelMaxThd = thdValue;
                        //        }
                        //        if (thdValue < thisChanelMinThd)
                        //        {
                        //            thisChanelMinThd = thdValue;
                        //        }
                        //        //matchValue[k].Groups[1].ToString();
                        //        //result[k] = float.Parse(matchValue[k].Value);
                        //    }
                        //}
                        Match matchValue = Regex.Match(eachLines[j], config.CheckFreqValue.ToString() + "\t ([-.0-9]{9,10})\t ([.0-9]{8,9})");
                        if (!matchValue.Success)
                        {
                            throw new BaseException(string.Format("channel{0}:{1}HZ所在行不匹配正则格式", i, config.CheckFreqValue));
                        }
                        //string strSpl = matchValue.Groups[1].ToString();
                        //string strThd = matchValue.Groups[2].ToString();
                        float splValue = float.Parse(matchValue.Groups[1].ToString());
                        float thdValue = float.Parse(matchValue.Groups[2].ToString());

                        //log.Info(string.Format("Channel{0},测试频率:{1}Hz,最大Spl:{2},最小Spl:{3},最大Thd:{4},最小Thd:{5}", i, config.CheckFreqValue, thisChanelMaxSpl, thisChanelMinSpl, thisChanelMaxThd, thisChanelMinThd));
                        if (splValue > maxSpl)
                        {
                            maxSpl = splValue;
                        }
                        if (splValue < minSpl)
                        {
                            minSpl = splValue;
                        }

                        if (thdValue > maxThd)
                        {
                            maxThd = thdValue;
                        }
                        if (thdValue < minThd)
                        {
                            minThd = thdValue;
                        }

                    }

                }
                //一个通道解析完成
                if (!channelContain984)
                {
                    throw new BaseException(string.Format("channel{0}:不包含要检查的{1}HZ", i, config.CheckFreqValue));
                }

            }
            //所有通道解析完成
            float splDifferValue = maxSpl - minSpl;
            float thdDifferValue = maxThd - minThd;
            if (splDifferValue > config.SplMaxOffset)
            {
                throw new BaseException(string.Format("\r\nMIC通道间最大灵敏度值:{0}dB\r\n最小灵敏度值:{1}dB\r\n差值{2}dB超出标准{3}dB", maxSpl,minSpl,splDifferValue,config.SplMaxOffset));
            }
            log.Info(string.Format("\r\nMIC通道间最大灵敏度值:{0}dB\r\n最小灵敏度值:{1}dB\r\n差值{2}dB在标准{3}dB内", maxSpl, minSpl, splDifferValue, config.SplMaxOffset));
            if (thdDifferValue > config.ThdMaxOffset)
            {
                throw new BaseException(string.Format("\r\nMIC通道间谐波失真最大值:{0}%\r\n最小失真值:{1}%\r\n差值{2}%超出标准{3}%", maxThd, minThd, thdDifferValue,config.ThdMaxOffset));
            }
            log.Info(string.Format("\r\nMIC通道间谐波失真最大值:{0}%\r\n最小失真值:{1}%\r\n差值{2}%在标准{3}%内", maxThd, minThd, thdDifferValue, config.ThdMaxOffset));


            //if (true)
            //{

            //}



        }
        /// <summary>
        /// 向txt文件写入内容
        /// </summary>
        /// <param name="path">txt文件保存的路径，没有就创建，有内容就覆盖。例："E:\\text.txt"</param>
        /// <param name="contentSrt">要写入的内容</param>
        private void WriteForTxt(string path, string[] contentSrt)
        {
            string folder = System.IO.Path.GetDirectoryName(path);
            // 创建目录
            if (Directory.Exists(folder) == false)
            {
                Directory.CreateDirectory(folder);
            }
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            //FileMode mode, FileAccess access, FileShare share);
            StreamWriter wr = null;
            wr = new StreamWriter(fs);
            foreach (var item in contentSrt)
            {
                wr.Write(item);
            }
            wr.Close();
        }


        public int ExeCommand(string fileName, out string strOutput)
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
                p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived1);
                p.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived1);
                p.EnableRaisingEvents = true;                      // 启用Exited事件  
                p.Exited += new EventHandler(CmdProcess_Exited1);   // 注册进程结束事件  


                p.Start();
                //p.StandardInput.WriteLine("C:");    //将CMD命令写入StandardInput流中

                p.StandardInput.WriteLine(string.Format("cd  {0}", config.ToolFolder));    //将CMD命令写入StandardInput流中
                //if (config.IsMicorsoftInit)
                //{
                //    p.StandardInput.WriteLine("InitializeCommandPrompt");
                //}
                //p.StandardInput.WriteLine();

                p.StandardInput.WriteLine(fileName);    //将CMD命令写入StandardInput流中
                //Thread.Sleep(config.CommandDelayTime);
                //p.StandardInput.WriteLine("exit");         //将 exit 命令写入StandardInput流中

                ////strOutput = p.StandardOutput.ReadToEnd();   //读取所有输出的流的所有字符
                //p.WaitForExit();                           //无限期等待，直至进程退出
                //p.Close();                                  //释放进程，关闭进程
                strOutput = "";
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();

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
        private void p_OutputDataReceived1(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                string result = e.Data;
                // 4. 异步调用，需要invoke  
                //MainForm.Invoke(ReadStdOutput, new object[] { e.Data });
                if (string.IsNullOrEmpty(result))
                {
                    return;
                }
                dataRev += result+"\r\n";
                log.Info(result);
                //if (result.Contains("Connecting, please reset device"))
                //{
                //    log.Info("请上电!!!!!!!");
                //}

                //if (result.Contains("Wait connect success flag (hisilicon) overtime") || result.Contains("ERROR"))
                //{
                //    log.Info(string.Format("超时"), -1, true, DevId);
                //}

            }
        }

        private void p_ErrorDataReceived1(object sender, DataReceivedEventArgs e)
        {

            if (e.Data != null)
            {
                string result = e.Data;
                //MainForm.Invoke(ReadErrOutput, new object[] { e.Data });
                dataRev += result + "\r\n";

                log.Info(result);
                //if (result.Contains("或批处理文件"))
                //{
                //    return;
                //}
                //if (result.Contains("'0' 不是内部或外部命令"))
                //{
                //    log.Info(string.Format("写程成功"), 1, true, DevId);
                //}
                //else
                //{
                //    log.Info(string.Format("写程失败"), -1, true, DevId);
                //}

            }
        }

        private void CmdProcess_Exited1(object sender, EventArgs e)
        {
            //Thread.Sleep(1200);
            // 执行结束后触发  
                log.Info("cmd has exit!!!");
                ////if (!textBox1.Text.Contains("Error") && !textBox1.Text.Contains("ERROR"))
                ////{
                ////    isError = false;
                ////    //this.textBox1.AppendText("错误" + "\r\n");
                ////    ////红色
                ////    //textBox1.BackColor = Color.Red;
                ////}
                //if (TextBoxPro.Text.Contains("esp_mac:"))//SUCCESS
                //{

                //    //UpdateUiFunc("OS升级完成!!");

                //    //解析MAC，Did
                //    Match matchMac = Regex.Match(TextBoxPro.Text, "esp_mac:([-0-9a-fA-F]{17})");
                //    if (!matchMac.Success)
                //    {
                //        UpdateUiFunc(string.Format("产品:{0}，获取MAC失败", DevId), -1, true, DevId);//Device ID:{0}
                //        return;

                //    }
                //    string mac = matchMac.Groups[1].ToString();
                //    mac=Regex.Replace(mac, "-", "");
                //    mac=mac.ToUpper();

                //    Match matchDid = Regex.Match(TextBoxPro.Text, "did data: ([0-9]+)");
                //    if (!matchDid.Success)
                //    {
                //        UpdateUiFunc(string.Format("产品:{0}，获取DID失败", DevId), -1, true, DevId);//Device ID:{0}
                //        return;
                //    }

                //    string did = matchDid.Groups[1].ToString();

                //    UpdateUiFunc(string.Format("产品:{0}获取成功\r\nMAC={1},DID={2}|", DevId, mac, did), 1, true, DevId);
                //    //Thread thread = new Thread(new ThreadStart(GetMacThread));
                //    //thread.IsBackground = true;
                //    //thread.Start();

                //}
                //else
                //{
                //    UpdateUiFunc(string.Format("产品:{0}，获取MAC,DID失败", DevId), -1, true, DevId);//Device ID:{0}
                //}

        }

        //private void ReadStdOutputAction(string result)
        //{
        //    if (string.IsNullOrEmpty(result))
        //    {
        //        return;
        //    }
        //    log.Info(result);
        //    if (result.Contains("Connecting, please reset device"))
        //    {
        //        log.Info("请上电!!!!!!!");
        //    }

        //    if (result.Contains("Wait connect success flag (hisilicon) overtime") || result.Contains("ERROR"))
        //    {
        //        log.Info(string.Format("超时"), -1, true, DevId);
        //    }

        //    //if (result.Contains("Execution Successful") &&
        //    //    TextBoxPro.Text.Contains(MainForm.SuccessKey))
        //    //{
        //    //    log.Info(string.Format("包号关键字:{0},写完成", MainForm.SuccessKey), 1, true, DevId);
        //    //}
        //}

        //private void ReadErrOutputAction(string result)
        //{

        //    log.Info(result);
        //    if (result.Contains("或批处理文件"))
        //    {
        //        return;
        //    }
        //    if (result.Contains("'0' 不是内部或外部命令"))
        //    {
        //        log.Info(string.Format("写程成功"), 1, true, DevId);
        //    }
        //    else
        //    {
        //        log.Info(string.Format("写程失败"), -1, true, DevId);
        //    }

        //    //this.textBox1.AppendText(result + "\r\n");
        //}

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
