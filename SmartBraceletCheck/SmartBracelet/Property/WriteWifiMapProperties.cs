using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{

    public class WriteWifiMapProperties : CommonProperties
    {
        private string writeMapAtCommand;

        private string address;

        //private string mapValue16Bytes;
        private int atCommandInterval;


        private string atCommandOk;
        private string atCommandError;


        //private string[] checkInfos;

        //private string resetCmd;

        //private int cmdCommandInterval;
        //private string binFileFullPath;




        [Category("Cmd"), Description("WriteMapAtCommand")]
        public string WriteMapAtCommand
        {
            get { return writeMapAtCommand; }
            set { writeMapAtCommand = value; }
        }
        [Category("Address"), Description("MapAddress")]
        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        //[Category("MapValue"), Description("MapValue16Bytes")]
        //public string MapValue16Bytes
        //{
        //    get { return mapValue16Bytes; }
        //    set { mapValue16Bytes = value; }
        //}
        [Category("Cmd"), Description("AtCommandWaitRespTime")]
        public int AtCommandInterval
        {
            get { return atCommandInterval; }
            set { atCommandInterval = value; }
        }

        //[Category("CheckInfo"), Description("Check Infos")]
        //public string [] CheckInfos
        //{
        //    get { return checkInfos; }
        //    set { checkInfos = value; }
        //}

        [Category("RespCheck"), Description("AtCommandOk")]
        public string AtCommandOk
        {
            get { return atCommandOk; }
            set { atCommandOk = value; }
        }

        [Category("RespCheck"), Description("AtCommandError")]
        public string AtCommandError
        {
            get { return atCommandError; }
            set { atCommandError = value; }
        }

    }

}
