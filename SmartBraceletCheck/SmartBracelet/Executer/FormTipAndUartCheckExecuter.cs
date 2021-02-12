using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Dut.AtCommand;
using AILinkFactoryAuto.Task.Common;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using AILinkFactoryAuto.Task.SmartBracelet.Property;
using AILinkFactoryAuto.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Executer
{

    public class FormTipAndUartCheckExecuter : IExecuter
    {
        public void Run(IProperties properties, GlobalDic<string, object> globalDic)
        {
            FormTipAndUartCheckProperties config = properties as FormTipAndUartCheckProperties;
            ILog log = globalDic.Get<ILog>();

            TipAndCheckUartForm formUserConfirm = new TipAndCheckUartForm(config, log)
            {
            };
            formUserConfirm.ShowDialog();

            if (!formUserConfirm.Result)
            {
                throw new BaseException("User Confirm Fail");
            }
        }
    }

}
