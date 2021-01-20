using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class CmdReadMacProperties : CommonProperties
    {
        private string getMacCmd;
        //private string globalVarible;
        private string pattern;
        private string toolFolder;
        private string deviceMac;
        
        //private string key;

        [Category("Cmd"), Description("获取MAC的CMD命令")]
        public string GetMacCmd
        {
            get { return getMacCmd; }
            set { getMacCmd = value; }
        }

        [Category("Regex"), Description("解析MAC的正则")]
        public string Pattern
        {
            get { return pattern; }
            set { pattern = value; }
        }

        [Category("ToolFolder"), Description("工具所在文件夹名")]
        public string ToolFolder
        {
            get { return toolFolder; }
            set { toolFolder = value; }
        }

        [Category("DeviceMac"), Description("存放产品MAC的变量名")]
        public string DeviceMac
        {
            get { return deviceMac; }
            set { deviceMac = value; }
        }

        //[Category("GlobalVarible"), Description("GlobalVarible")]
        //public string GlobalVarible
        //{
        //    get { return globalVarible; }
        //    set { globalVarible = value; }
        //}
    }
}
