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
            List<ComDut> comDutList = globalDic[typeof(List<ComDut>).ToString()] as List<ComDut>;
            if (comDutList==null || comDutList.Count==0)
            {
                throw new BaseException("COM为空");
            }
            ComDut comDut = null;
            foreach (var item in comDutList)
            {
                if (item.PortName == config.PortName)
                {
                    comDut = item;
                    break;
                }

            }
            if (comDut == null)
            {
                throw new BaseException(string.Format("ComDut No PortName:{0}", config.PortName));
            }


            TipAndCheckUartForm formUserConfirm = new TipAndCheckUartForm(config, log, comDut)
            {
            };
            formUserConfirm.ShowDialog();

            if (!formUserConfirm.Result)
            {
                throw new BaseException("Fail");
            }
        }
    }

}
