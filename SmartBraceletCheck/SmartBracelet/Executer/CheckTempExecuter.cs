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
    public class CheckTempExecuter : IExecuter
    {
        CheckTempProperties config;
        private GlobalVaribles configGv;
        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {
            config = properties as CheckTempProperties;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;

            string strNbTemp = configGv.Get("NBTemp");
            float floatNbTemp;
            try
            {
                floatNbTemp = float.Parse(strNbTemp);
                if (floatNbTemp > config.MaxValue)
                {
                    log.Fail(string.Format("NB 温度传感器检查：\r\n模块温度传感器温度值：{0}，大于设定最大值：{1} FAIL \r\n", floatNbTemp, config.MaxValue));
                    throw new BaseException(string.Format("NB 温度传感器检查：\r\n模模块温度传感器温度值：{0}，大于设定最大值：{1} FAIL \r\n", floatNbTemp, config.MaxValue));
                }
                else if (floatNbTemp < config.MinValue)
                {
                    log.Fail(string.Format("NB 温度传感器检查：\r\n模模块温度传感器温度值：{0}，小于设定最小值：{1} FAIL \r\n", floatNbTemp, config.MinValue));
                    throw new BaseException(string.Format("NB 信号强度检查：\r\n模块温度传感器温度值：{0}，小于设定最小值：{1} FAIL \r\n", floatNbTemp, config.MinValue));
                }

                log.Info(string.Format("NB 温度传感器检查：\r\n模模块温度传感器温度值：{0}，在设定范围：{1} ~ {2}之间 PASS \r\n", floatNbTemp, config.MinValue, config.MaxValue));


            }
            catch (Exception ex)
            {
                throw new Exception("NB 温度传感器检查出错，" + ex.Message);
            }

        }
    }
}
