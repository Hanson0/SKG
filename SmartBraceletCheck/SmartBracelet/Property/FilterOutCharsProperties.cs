using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class FilterOutCharsProperties : CommonProperties
    {
        private string[] globalVariblesKey;

        [Category("GlobalVariblesKey"), Description("原字符串变量名")]
        public string[] GlobalVariblesKey { get { return globalVariblesKey; } set { globalVariblesKey = value; } }



        private string[] filterOutChars;
        [Category("FilterOutChars"), Description("要过滤掉的字符/字符串")]
        public string[] FilterOutChars { get { return filterOutChars; } set { filterOutChars = value; } }

    }
}
