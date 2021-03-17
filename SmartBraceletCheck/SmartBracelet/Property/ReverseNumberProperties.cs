using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class ReverseNumberProperties : CommonProperties
    {
        private string preGlobalVariable;

        [Category("Reverse"), Description("反序前的全局变量名")]
        public string PreGlobalVariable
        {
            get { return preGlobalVariable; }
            set { preGlobalVariable = value; }
        }


        private string afterGlobalVariable;

        [Category("Reverse"), Description("存放反序后的全局变量名")]
        public string AfterGlobalVariable
        {
            get { return afterGlobalVariable; }
            set { afterGlobalVariable = value; }
        }

    }
}
