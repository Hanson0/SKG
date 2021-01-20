using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Dut.AtCommand;
using AILinkFactoryAuto.Dut.AtCommand.Property;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using AILinkFactoryAuto.Task.SmartBracelet.Property;
using Mes.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace AILinkFactoryAuto.Task.SmartBracelet.Executer
{
    //public class AccessToMacExecuter : IExecuter
    //{
    //    ////FindDeviceProperties config;
    //    //private GlobalVaribles configGv;
    //    //public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
    //    //{
    //    //    //config = properties as FindDeviceProperties;
    //    //    ILog log = globalDic[typeof(ILog).ToString()] as ILog;
    //    //    configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;
    //    //    //int ret = -1;
    //    //    //string returnString = "";

    //    //    //取存储的关键字为MoudleBtMac的value值
    //    //    string moudleBtMac = configGv.Get("MoudleBtMac");
    //    //    if (string.)
    //    //    {

    //    //    }
    //    //    string btMacFromImes = configGv.Get("MAC_BT"); ;//configGv.Get("MAC_BT");

    //    //    if (moudleBtMac != btMacFromImes)
    //    //    {
    //    //        log.Fail(string.Format("BT MAC核验：读出BT MAC：{0}，与预写入MAC：{1} 不一致 FAIL\r\n", moudleBtMac, btMacFromImes));
    //    //        throw new BaseException(string.Format("BT MAC核验：读出BT MAC：{0}，与预写入MAC：{1} 不一致 \r\n", moudleBtMac, btMacFromImes));
    //    //    }
    //    //    //string moudleBtMac = configGv.Get("");
    //    //    log.Info(string.Format("BT MAC核验：读出BT MAC：{0}，与预写入MAC：{1} 一致，PASS \r\n", moudleBtMac, btMacFromImes));

    //    //}


    //}
    public class AccessToMacExecuter : IExecuter
    {
        private string pattern;

        private GlobalVaribles configGv;
        public AccessToMacExecuter()
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
            AccessToMacProperties config = properties as AccessToMacProperties;

            ComDut comDut = globalDic[typeof(ComDut).ToString()] as ComDut;
            OpenPhoneProperties configOpenPhone = globalDic[typeof(OpenPhoneProperties).ToString()] as OpenPhoneProperties;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;

            //string moudleBtMac = configGv.Get("MAC_BT");
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
            if (!response.Contains("BLE MAC:"))
            {
                throw new BaseException("不包含BLE MAC:信息");
            }


            if (!string.IsNullOrEmpty(config.GlobalVariblesKey) && !string.IsNullOrEmpty(config.GlobalVariblesKeyPattern))
            {
                string pattern = Parse_r_n(config.GlobalVariblesKeyPattern);
                Match matchValue = Regex.Match(response, pattern);
                if (!matchValue.Success)
                {
                    //ble mac为空
                    //log.Info("BLE未写过MAC");
                    throw new BaseException("BLE未写过MAC,请在前面工序写BLE MAC");
                }
                else
                {
                    string value = matchValue.Groups[1].ToString();
                    if (value == "000000000000")
                    {
                        log.Info(string.Format("BLE未写过MAC:{0}", value));
                        throw new BaseException("BLE未写过MAC,请在前面工序写BLE MAC");
                        //configGv.Add(GlobalVaribles.MAC_BT, value);
                    }
                    else
                    {
                        log.Info(string.Format("BLE已写过MAC:{0}", value));
                        //预检查{MoudleBtMac}

                        Match matchKey = Regex.Match(config.GlobalVariblesKey, this.pattern);//this.pattern
                        if (!matchKey.Success)
                        {
                            throw new BaseException("read info key fail");
                        }

                        string key = matchKey.Groups[1].ToString();
                        //预检查
                        MesProcess.PreCheck(value);

                        //赋值BT MAC,WIFI MAC,取消取号
                        configGv.Add(key, value);
                        //configGv.Add(GlobalVaribles.MAC_BT, value);
                        UInt64 macUInt64 = Convert.ToUInt64(value, 16);
                        string macWifi = string.Format("{0:X}", macUInt64 - 1).PadLeft(12, '0');
                        configGv.Add(GlobalVaribles.MAC, macWifi);

                        log.Info(string.Format("自动计算WIFI MAC号:{0}", macWifi));
                    }
                }

            }
        }
        //public void Run(IProperties properties, GlobalDic<string, object> globalDic)
        //{
        //    MesProcess.PreCheck("601D9D0F460B");
        //}
    }

}
