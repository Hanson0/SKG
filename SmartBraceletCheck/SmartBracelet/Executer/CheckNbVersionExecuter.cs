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
    public class CheckNbVersionExecuter : IExecuter
    {
        CheckNbVersionProperties config;
        private GlobalVaribles configGv;

        List<string> netWorkList;
        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {
            config = properties as CheckNbVersionProperties;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;

            
            string devVersion = configGv.Get(config.GlobalVarible);
            if (devVersion != config.Version)
            {
                throw new BaseException(string.Format("关键字检查:{0},配置关键字:{1}不一致", devVersion, config.Version));
            }
            log.Info(string.Format("关键字检查:{0},配置关键字:{1}一致", devVersion, config.Version));
        }

    }
}
