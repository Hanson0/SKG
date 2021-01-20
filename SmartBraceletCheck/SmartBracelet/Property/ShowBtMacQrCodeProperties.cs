using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class ShowBtMacQrCodeProperties : CommonProperties
    {
        private int showTime;
        private string strQrcode;


        [Category("Time"), Description("FormShowTime")]
        public int ShowTime
        {
            get { return showTime; }
            set { showTime = value; }
        }
        [Category("Qrcode"), Description("sring二维码内容")]
        public string StrQrcode
        {
            get { return strQrcode; }
            set { strQrcode = value; }
        }
    }

}
