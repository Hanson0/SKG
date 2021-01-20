using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using AILinkFactoryAuto.Task.SmartBracelet.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Executer
{
    public class CheckBtRssiExecuter : IExecuter
    {
        CheckBtRssiProperties config;
        private GlobalVaribles configGv;
        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {
            config = properties as CheckBtRssiProperties;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;

            string strMoudleBtRssi = configGv.Get("RSSI");
            int intMoudleBtRssi;
            try
            {
                intMoudleBtRssi = int.Parse(strMoudleBtRssi);
                if (intMoudleBtRssi > config.MaxValue)
                {
                    log.Fail(string.Format("信号强度检查：\r\n模块信号强度：{0}，大于设定最大值：{1} FAIL \r\n", intMoudleBtRssi, config.MaxValue));
                    throw new BaseException(string.Format("信号强度检查：\r\n模块信号强度：{0}，大于设定最大值：{1} FAIL \r\n", intMoudleBtRssi, config.MaxValue));
                }
                else if (intMoudleBtRssi < config.MinValue)
                {
                    log.Fail(string.Format("信号强度检查：\r\n模块信号强度：{0}，小于设定最小值：{1} FAIL \r\n", intMoudleBtRssi, config.MinValue));
                    throw new BaseException(string.Format("信号强度检查：\r\n模块信号强度：{0}，小于设定最小值：{1} FAIL \r\n", intMoudleBtRssi, config.MinValue));
                }
                
                log.Info(string.Format("信号强度检查：\r\n模块信号强度：{0}，在设定范围：{1} ~ {2}之间 PASS \r\n", intMoudleBtRssi, config.MinValue, config.MaxValue));


            }
            catch (Exception ex)
            {
                throw new Exception("信号强度检查出错，" + ex.Message);
            }

        }
    }

}
