using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class LogMacChangeToThisMacProperties : CommonProperties
    {
        private string globalVarible;

        [Category("GlobalVarible"), Description("GlobalVarible")]
        public string GlobalVarible
        {
            get { return globalVarible; }
            set { globalVarible = value; }
        }
    }
}
