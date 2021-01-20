using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Executer
{

    public class CheckBtMacExecuter : IExecuter
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
            string moudleBtMac = configGv.Get("MoudleBtMac");
            string btMacFromImes = configGv.Get("MAC_BT"); ;//configGv.Get("MAC_BT");

            if (moudleBtMac != btMacFromImes)
            {
                log.Fail(string.Format("BT MAC核验：读出BT MAC：{0}，与预写入MAC：{1} 不一致 FAIL\r\n", moudleBtMac, btMacFromImes));
                throw new BaseException(string.Format("BT MAC核验：读出BT MAC：{0}，与预写入MAC：{1} 不一致 \r\n", moudleBtMac, btMacFromImes));
            }
            //string moudleBtMac = configGv.Get("");
            log.Info(string.Format("BT MAC核验：读出BT MAC：{0}，与预写入MAC：{1} 一致，PASS \r\n", moudleBtMac, btMacFromImes));

        }
    }
}

