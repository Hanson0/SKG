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
    public class CheckWifiProbeExecuter : IExecuter
    {
        CheckWifiProbeProperties config;
        private GlobalVaribles configGv;
        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {
            config = properties as CheckWifiProbeProperties;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;

            string strNbProbe = configGv.Get("NBWIFIPROBE");
            int intNbProbe;
            try
            {
                intNbProbe = int.Parse(strNbProbe);
                if (intNbProbe > config.MaxValue)
                {
                    log.Fail(string.Format("WIFI探针-信号强度检查：\r\n信号强度：{0}，大于设定最大值：{1} FAIL \r\n", intNbProbe, config.MaxValue));
                    throw new BaseException(string.Format("NB 探针强度检查：\r\n信号强度：{0}，大于设定最大值：{1} FAIL \r\n", intNbProbe, config.MaxValue));
                }
                else if (intNbProbe < config.MinValue)
                {
                    log.Fail(string.Format("WIFI探针-信号强度检查：\r\n信号强度：{0}，小于设定最小值：{1} FAIL \r\n", intNbProbe, config.MinValue));
                    throw new BaseException(string.Format("WIFI 探针强度检查：\r\n信号强度：{0}，小于设定最小值：{1} FAIL \r\n", intNbProbe, config.MinValue));
                }

                log.Info(string.Format("WIFI探针-信号强度检查：\r\n信号强度：{0}，在设定范围：{1} ~ {2}之间 PASS \r\n", intNbProbe, config.MinValue, config.MaxValue));


            }
            catch (Exception ex)
            {
                throw new Exception("WIFI探针-信号强度检查出错，" + ex.Message);
            }

        }
    }

}
