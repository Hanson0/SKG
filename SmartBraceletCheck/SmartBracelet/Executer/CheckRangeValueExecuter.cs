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
    public class CheckRangeValueExecuter : IExecuter
    {
        CheckRangeValueProperties config;
        private GlobalVaribles configGv;
        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {
            config = properties as CheckRangeValueProperties;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;

            string strMoudleBtRssi = configGv.Get(config.GlobalVarible);
            double intMoudleBtRssi;
            try
            {
                intMoudleBtRssi = double.Parse(strMoudleBtRssi);
                if (intMoudleBtRssi > config.MaxValue)
                {
                    //log.Fail(string.Format("{0}：{1}，大于设定最大值：{2} FAIL \r\n", config.TestValueName, intMoudleBtRssi, config.MaxValue));
                    throw new BaseException(string.Format("{0}：{1}，大于设定最大值：{2} FAIL \r\n", config.TestValueName, intMoudleBtRssi, config.MaxValue));
                }
                else if (intMoudleBtRssi < config.MinValue)
                {
                    //log.Fail(string.Format("{0}：{1}，小于设定最小值：{2} FAIL \r\n", config.TestValueName,intMoudleBtRssi, config.MinValue));
                    throw new BaseException(string.Format("{0}：{1}，小于设定最小值：{2} FAIL \r\n", config.TestValueName, intMoudleBtRssi, config.MinValue));
                }

                log.Info(string.Format("{0}：{1}，在设定范围：{2} ~ {3}之间 PASS \r\n", config.TestValueName, intMoudleBtRssi, config.MinValue, config.MaxValue));


            }
            catch (Exception ex)
            {
                throw new Exception("检查出错，" + ex.Message);
            }

        }
    }
}
