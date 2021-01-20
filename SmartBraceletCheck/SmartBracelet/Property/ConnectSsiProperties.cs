using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class ConnectSsiProperties : CommonProperties
    {
        private string ssid;
        private string key;


        [Category("WIFI")]
        [Description("SSID")]
        public string SSID
        {
            get { return ssid; }
            set { ssid = value; }
        }


        [Category("WIFI")]
        [Description("Key")]
        public string Key
        {
            get { return key; }
            set { key = value; }
        }

    }
}
