using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class CmdFindDeviceProperties : CommonProperties
    {
        private string command;

        private string commandError;
        private string commandOk;
        private string[] checkInfo;
        private string[] globalVariblesKey;

        private string[] globalVariblesKeyPattern;

        private string toolFolder;
        //private bool isMicorsoftInit;
        private int commandInterval;


        //private string key;

        [Category("Cmd"), Description("CMD命令")]
        public string Command
        {
            get { return command; }
            set { command = value; }
        }


        [Category("Cmd"), Description("CMD命令间隔")]
        public int CommandInterval
        {
            get { return commandInterval; }
            set { commandInterval = value; }
        }

        //[Category("Regex"), Description("解析MAC的正则")]
        //public string Pattern
        //{
        //    get { return pattern; }
        //    set { pattern = value; }
        //}

        [Category("Cmd"), Description("工具所在文件夹名")]
        public string ToolFolder
        {
            get { return toolFolder; }
            set { toolFolder = value; }
        }

        //[Category("Cmd"), Description("是否发送InitializeCommandPrompt命令")]
        //public bool IsMicorsoftInit
        //{
        //    get { return isMicorsoftInit; }
        //    set { isMicorsoftInit = value; }
        //}

        [Category("CmdCheck"), Description("错误标志")]
        public string CommandError
        {
            get { return commandError; }
            set { commandError = value; }
        }
        [Category("CmdCheck"), Description("检测上电标志")]
        public string CommandOk
        {
            get { return commandOk; }
            set { commandOk = value; }
        }


        [Category("CmdCheck"), Description("CheckInfo")]
        public string[] CheckInfo
        {
            get { return checkInfo; }
            set { checkInfo = value; }
        }

        [Category("ReadInfo"), Description("global value's key")]
        public string[] GlobalVariblesKey
        {
            get { return globalVariblesKey; }
            set { globalVariblesKey = value; }
        }

        [Category("ReadInfo"), Description("global varibles key pattern")]
        public string[] GlobalVariblesKeyPattern
        {
            get { return globalVariblesKeyPattern; }
            set { globalVariblesKeyPattern = value; }
        }

    }
}
