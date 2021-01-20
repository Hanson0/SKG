using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class CheckNbVersionProperties : CommonProperties
    {
        private string version;
        private string globalVarible;
        
        //private string key;

        [Category("Key"), Description("要检查的关键字")]
        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        [Category("GlobalVarible"), Description("GlobalVarible")]
        public string GlobalVarible
        {
            get { return globalVarible; }
            set { globalVarible = value; }
        }
    }
}
