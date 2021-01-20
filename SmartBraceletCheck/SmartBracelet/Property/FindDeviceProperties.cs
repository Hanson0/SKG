using AILinkFactoryAuto.Dut.AtCommand.Property.Converter;
using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class FindDeviceProperties : CommonProperties
    {
        private string portName;
        private string testPowerOnAT;
        private int atCommandInterval;
        private string endLine;
        private string atCommandOk;
        private string[] checkInfo;
        private string[] globalVariblesKey;
        private string[] globalVariblesKeyPattern;
        private string atCommandError;


        [Category("SerialPort"), Description("PortName"), TypeConverter(typeof(PortNamesConverter))]
        public string PortName
        {
            get { return portName; }
            set { portName = value; }
        }

        [Category("AT"), Description("TestPowerOnAT")]
        public string TestPowerOnAT
        {
            get { return testPowerOnAT; }
            set { testPowerOnAT = value; }
        }
        [Category("AT"), Description("AtCommandInterval")]
        public int AtCommandInterval
        {
            get { return atCommandInterval; }
            set { atCommandInterval = value; }
        }
        [Category("AT"), Description("EndLine")]
        public string EndLine
        {
            get { return endLine; }
            set { endLine = value; }
        }

        [Category("AT"), Description("AtCommandOk")]
        public string AtCommandOk
        {
            get { return atCommandOk; }
            set { atCommandOk = value; }
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

        [Category("CheckInfo"), Description("check info")]
        public string[] CheckInfo
        {
            get { return checkInfo; }
            set { checkInfo = value; }
        }

        [Category("ATCommand"), Description("error")]
        public string AtCommandError
        {
            get { return atCommandError; }
            set { atCommandError = value; }
        }


    }
}
