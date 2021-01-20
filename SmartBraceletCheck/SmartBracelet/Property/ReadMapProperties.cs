using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class ReadMapProperties : CommonProperties
    {
        private string binFolderPath;


        [Category("BIN"), Description("Bin文件所在文件夹路径")]
        public string BinFolderPath
        {
            get { return binFolderPath; }
            set { binFolderPath = value; }
        }




    }
}
