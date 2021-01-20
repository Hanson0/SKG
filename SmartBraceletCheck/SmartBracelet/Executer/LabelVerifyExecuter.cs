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
    public class LabelVerifyExecuter : IExecuter
    {
        private static object myLock = new object();

        LabelVerifyProperties config;
        private GlobalVaribles configGv;
        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {
            config = properties as LabelVerifyProperties;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;
            string reScanLabel =configGv.Get(GlobalVaribles.LABEL_SN);
            string reScanLabelFixedPart = reScanLabel.Substring(0, 12);
            string reScanLabelMac = reScanLabel.Substring(12, 12);

            string macLabel = configGv.Get(GlobalVaribles.MAC);
            if (! (reScanLabelFixedPart==config.FixedPart))
            {
                throw new BaseException(string.Format("重扫标签的固定部分：{0}与标准固定部分：{1}不一致", reScanLabelFixedPart, config.FixedPart));
            }
            log.Info(string.Format("重扫标签的固定部分：{0}与标准固定部分：{1}一致", reScanLabelFixedPart, config.FixedPart));

            if (! (reScanLabelMac == macLabel))
            {
                throw new BaseException(string.Format("重扫标签的MAC部分：{0}与标签MAC：{1}不一致", reScanLabelMac, macLabel));
            }
            log.Info(string.Format("重扫标签的MAC部分：{0}与标签MAC：{1}一致", reScanLabelMac, macLabel));

        }
    }
}
