using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Task.Common;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AILinkFactoryAuto.Task.SmartBracelet.Executer
{
    public class ReScanLabelExecuter : IExecuter
    {
        private static object myLock = new object();

        //FindDeviceProperties config;
        private GlobalVaribles configGv;
        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {


            //config = properties as FindDeviceProperties;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;
            //int ret = -1;
            //string returnString = "";
            FormScanCode formScanCode = new FormScanCode()
            {
                //DutName = config.DutName,
                //ShowInTaskbar = true,
                //StartPosition = FormStartPosition.CenterScreen
            };
            DialogResult dialogResult = DialogResult.None;
            lock (myLock)
            {
                dialogResult = formScanCode.ShowDialog();
            }
            if (dialogResult != DialogResult.OK)
            {
                //sync.CountDown("ScanCodeExecuter");
                throw new BaseException("user discard scan code");
            }
            configGv.Add(GlobalVaribles.LABEL_SN, formScanCode.LabelSn);

        }
    }
}
