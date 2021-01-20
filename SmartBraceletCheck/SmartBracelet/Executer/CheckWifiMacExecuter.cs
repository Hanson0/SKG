using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Executer
{
    public class CheckWifiMacExecuter : IExecuter
    {
        //FindDeviceProperties config;
        private GlobalVaribles configGv;
        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {
            //config = properties as FindDeviceProperties;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;
            //取存储的关键字为MoudleBtMac的value值
            string DevWifi_MAC = configGv.Get("DevWifi_MAC");
            DevWifi_MAC = DevWifi_MAC.Replace(":", "").Replace("：", "");
            string labelMAC = configGv.Get(GlobalVaribles.MAC);
            if (DevWifi_MAC != labelMAC)
            {
                throw new BaseException(string.Format("WIFI MAC：{0}，与标签MAC：{1} 不一致 FAIL\r\n", DevWifi_MAC, labelMAC));
            }
            log.Info(string.Format(string.Format("WIFI MAC：{0}，与标签MAC：{1} 一致 PASS\r\n", DevWifi_MAC, labelMAC)));

        }
    }
}
