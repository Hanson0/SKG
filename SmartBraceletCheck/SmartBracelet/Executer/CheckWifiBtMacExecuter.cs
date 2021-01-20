using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Executer
{
    public class CheckWifiBtMacExecuter : IExecuter
    {
        private GlobalVaribles configGv;
        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;
            //取存储的关键字为MoudleBtMac的value值
            string DevWifi_MAC = configGv.Get("DevWifi_MAC");
            string DevBt_MAC = configGv.Get("DevBt_MAC");

            DevWifi_MAC = DevWifi_MAC.Replace(":", "").Replace("：", "").ToUpper();
            DevBt_MAC = DevBt_MAC.Replace(":", "").Replace("：", "").ToUpper();
            if (Convert.ToInt64(DevWifi_MAC, 16) + 1 != Convert.ToInt64(DevBt_MAC, 16))
            {
                throw new BaseException(string.Format("WIFI MAC：{0}，BT MAC：{1} 非+1关系，未通过校验 FAIL\r\n", DevWifi_MAC, DevBt_MAC));
            }
            log.Info(string.Format(string.Format("WIFI MAC：{0}，BT MAC：{1} 通过 +1关系校验 PASS\r\n", DevWifi_MAC, DevBt_MAC)));

            string labelMAC = configGv.Get(GlobalVaribles.MAC);
            if (string.IsNullOrEmpty(labelMAC))
            {
                configGv.Add(GlobalVaribles.MAC, DevWifi_MAC);
            }

        }
    }
}
