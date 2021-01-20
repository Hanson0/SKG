using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class LabelVerifyProperties : CommonProperties
    {
        private string fixedPart;

        [Category("FixedPart"), Description("固定部分")]
        public string FixedPart
        {
            get { return fixedPart; }
            set { fixedPart = value; }
        }

    }
}
