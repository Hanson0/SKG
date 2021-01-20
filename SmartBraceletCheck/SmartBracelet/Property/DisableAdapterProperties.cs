using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class DisableAdapterProperties : CommonProperties
    {
        private string nicName;
        [Category("NIC"), Description("NICname")]
        public string NICname { get { return nicName; } set { nicName = value; } }
    }
}
