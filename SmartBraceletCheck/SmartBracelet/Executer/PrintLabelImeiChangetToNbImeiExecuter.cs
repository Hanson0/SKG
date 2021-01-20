using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Executer
{
    public class PrintLabelImeiChangetToNbImeiExecuter : IExecuter
    {
        //CheckBtRssiProperties config;
        private GlobalVaribles configGv;
        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {
            //config = properties as CheckBtRssiProperties;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;

            try
            {

                string labImei = configGv.Get("IMEI");
                string nbImei = configGv.Get("NBIMEI");
                if (string.IsNullOrEmpty(labImei) && !string.IsNullOrEmpty(nbImei))
                {
                    configGv.Add("IMEI", nbImei);
                }
                else if (!string.IsNullOrEmpty(labImei))//有标签BT MAC，则肯定经过一致性检查
                {
                    configGv.Add("IMEI", nbImei);
                }
                else
                {
                    log.Fail(string.Format("打印标签的IMEI 设置为NB IMEI失败\r\n,NB IMEI:{0} \r\n", nbImei));
                }
                log.Info(string.Format("打印标签的IMEI 设置为NB IMEI：\r\n{0} PASS \r\n", nbImei));


            }
            catch (Exception ex)
            {
                throw new Exception("打印标签的IMEI 设置为NB IMEI出错，" + ex.Message);
            }

        }
    }

}
