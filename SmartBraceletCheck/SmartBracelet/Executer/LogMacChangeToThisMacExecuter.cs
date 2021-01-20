using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{

    public class LogMacChangeToThisMacExecuter : IExecuter
    {
        LogMacChangeToThisMacProperties config;
        private GlobalVaribles configGv;
        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {
            //config = properties as CheckBtRssiProperties;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;
            config = properties as LogMacChangeToThisMacProperties;
            try
            {

                //string strBtMac = configGv.Get("MAC_BT");
                //string strMoudleBtMac = configGv.Get("MoudleBtMac");
                //if (string.IsNullOrEmpty(strBtMac) && !string.IsNullOrEmpty(strMoudleBtMac))
                //{
                //    configGv.Add("MAC", strMoudleBtMac);
                //}
                //else if (!string.IsNullOrEmpty(strBtMac) && !string.IsNullOrEmpty(strMoudleBtMac))//有标签BT MAC，则肯定经过一致性检查
                //{
                //    configGv.Add("MAC", strMoudleBtMac);
                //}
                //else if (!string.IsNullOrEmpty(strBtMac) && string.IsNullOrEmpty(strMoudleBtMac))//有标签BT MAC，则肯定经过一致性检查
                //{
                //    configGv.Add("MAC", strBtMac);
                //}
                //else
                //{
                //    log.Fail(string.Format("LOG MAC设置为BT MAC失败\r\n,取号BT MAC{0}，模块BT MAC:{1} \r\n", strBtMac, strMoudleBtMac));
                //}
                string strMoudleBtMac = configGv.Get(config.GlobalVarible);
                if (string.IsNullOrEmpty(strMoudleBtMac))
                {
                    throw new Exception("要设置的MAC为空");
                }
                configGv.Add("MAC", strMoudleBtMac);
                log.Info(string.Format("LOG MAC设置为：\r\n{0} PASS \r\n", strMoudleBtMac));

            }
            catch (Exception ex)
            {
                throw new Exception("LOG MAC设置出错，" + ex.Message);
            }

        }
    }

}
