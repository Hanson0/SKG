using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class WriteBtBinProperties : CommonProperties
    {
        private string eraseCmd;
        private string programCmd;
        private string resetCmd;

        private int cmdCommandInterval;
        private string binFileFullPath;




        [Category("Cmd"), Description("EraseCmd")]
        public string EraseCmd
        {
            get { return eraseCmd; }
            set { eraseCmd = value; }
        }

        [Category("Cmd"), Description("ProgramCmd")]
        public string ProgramCmd
        {
            get { return programCmd; }
            set { programCmd = value; }
        }


        [Category("Cmd"), Description("ResetCmd")]
        public string ResetCmd
        {
            get { return resetCmd; }
            set { resetCmd = value; }
        }

        [Category("Cmd"), Description("CmdCommandInterval")]
        public int CmdCommandInterval
        {
            get { return cmdCommandInterval; }
            set { cmdCommandInterval = value; }
        }


        [Category("Cmd"), Description("BinFileFullPath")]
        public string BinFileFullPath
        {
            get { return binFileFullPath; }
            set { binFileFullPath = value; }
        }

    }
}
