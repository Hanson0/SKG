using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Executer
{
    public class CheckReScanLabelsExecuter : IExecuter
    {
        //FindDeviceProperties config;
        private GlobalVaribles configGv;
        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {
            //config = properties as FindDeviceProperties;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;
            //int ret = -1;
            //string returnString = "";

            //取存储的关键字为MoudleBtMac的value值
            string moudleBtMac = configGv.Get(GlobalVaribles.MAC);
            string btMacFromImes = configGv.Get(GlobalVaribles.LABEL_SN); ;//configGv.Get("MAC_BT");

            if (moudleBtMac != btMacFromImes)
            {
                //log.Fail(string.Format("再次扫码的标签一致性检查：\r\n第一次标签 MAC：{0}\r\n第二次标签MAC：{1} 不一致 FAIL\r\n", moudleBtMac, btMacFromImes));
                throw new BaseException(string.Format("再次扫码的标签一致性检查：\r\n第一次标签 MAC：{0}\r\n第二次标签MAC：{1} \r\n不一致 \r\n", moudleBtMac, btMacFromImes));
            }
            //string moudleBtMac = configGv.Get("");
            log.Info(string.Format("再次扫码的标签一致性检查：\r\n第一次标签 MAC：{0}\r\n第二次标签MAC：{1} \r\n一致 \r\n", moudleBtMac, btMacFromImes));

        }
    }
}
