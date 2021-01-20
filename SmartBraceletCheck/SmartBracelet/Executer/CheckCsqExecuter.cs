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
    public class CheckCsqExecuter : IExecuter
    {
        CheckCsqProperties config;
        private GlobalVaribles configGv;
        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {
            config = properties as CheckCsqProperties;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;

            string strNbCSQ = configGv.Get("NBCSQ");
            int intNbCSQ;
            try
            {
                intNbCSQ = int.Parse(strNbCSQ);
                if (intNbCSQ > config.MaxValue)
                {
                    log.Fail(string.Format("NB 信号强度检查：\r\n模块信号强度：{0}，大于设定最大值：{1} FAIL \r\n", intNbCSQ, config.MaxValue));
                    throw new BaseException(string.Format("NB 信号强度检查：\r\n模块信号强度：{0}，大于设定最大值：{1} FAIL \r\n", intNbCSQ, config.MaxValue));
                }
                else if (intNbCSQ < config.MinValue)
                {
                    log.Fail(string.Format("NB 信号强度检查：\r\n模块信号强度：{0}，小于设定最小值：{1} FAIL \r\n", intNbCSQ, config.MinValue));
                    throw new BaseException(string.Format("NB 信号强度检查：\r\n模块信号强度：{0}，小于设定最小值：{1} FAIL \r\n", intNbCSQ, config.MinValue));
                }

                log.Info(string.Format("NB 信号强度检查：\r\n模块信号强度：{0}，在设定范围：{1} ~ {2}之间 PASS \r\n", intNbCSQ, config.MinValue, config.MaxValue));


            }
            catch (Exception ex)
            {
                throw new Exception("NB 信号强度检查出错，"+ex.Message);
            }

        }
    }

}
