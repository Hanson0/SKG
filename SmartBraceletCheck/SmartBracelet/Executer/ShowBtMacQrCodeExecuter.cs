using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Task.Common;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using AILinkFactoryAuto.Task.SmartBracelet.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Executer
{
    public class ShowBtMacQrCodeExecuter : IExecuter
    {
        ShowBtMacQrCodeProperties config;
        private GlobalVaribles configGv;
        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {
            config = properties as ShowBtMacQrCodeProperties;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;
            //int ret = -1;
            //string returnString = "";

            //取存储的关键字为MoudleBtMac的value值
            string moudleBtMac = configGv.Get("MoudleBtMac");

            QrCodeForm qrCodeForm = new QrCodeForm(moudleBtMac, config.ShowTime);
            qrCodeForm.ShowDialog();
            //if (moudleBtMac != btMacFromImes)
            //{
            //    log.Fail(string.Format("BT MAC核验：读出BT MAC：{0}，与预写入MAC：{1} 不一致 FAIL\r\n", moudleBtMac, btMacFromImes));
            //    throw new BaseException(string.Format("BT MAC核验：读出BT MAC：{0}，与预写入MAC：{1} 不一致 \r\n", moudleBtMac, btMacFromImes));
            //}
            ////string moudleBtMac = configGv.Get("");
            //log.Info(string.Format("BT MAC核验：读出BT MAC：{0}，与预写入MAC：{1} 一致，PASS \r\n", moudleBtMac, btMacFromImes));

        }
    }

}
