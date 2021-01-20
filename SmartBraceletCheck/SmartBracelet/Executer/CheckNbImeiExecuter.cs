using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class CheckNbImeiExecuter : IExecuter
    {
        //FindDeviceProperties config;
        private GlobalVaribles configGv;
        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {
            //config = properties as FindDeviceProperties;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;

            //取存储的关键字为MoudleBtMac的value值
            string labelImei = configGv.Get("IMEI");
            string moudleImei = configGv.Get("NBIMEI");

            if (labelImei != moudleImei)
            {
                log.Fail(string.Format("NB IMEI检查：\r\n模块IMEI：{0}，与标签IMEI：{1} 不一致 \r\n", moudleImei, labelImei));
                throw new BaseException(string.Format("NB IMEI检查：\r\n模块IMEI：{0}，与标签IMEI：{1} 不一致 \r\n", moudleImei, labelImei));
            }
            log.Info(string.Format("NB IMEI检查：\r\n模块IMEI：{0}，与标签IMEI：{1} 一致 \r\n", moudleImei, labelImei));

        }
    }

}
