using AILinkFactoryAuto.Dut.AtCommand.Property.Converter;
using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class GetLicenseProperties : CommonProperties
    {
        private string portName;

        private int baudRate;

        private bool rts;

        private bool dtr;


        private string atCommand;

        private string atCommandError;

        private string atCommandOk;

        private int atCommandInterval;

        private string[] checkInfo;

        private string globalVariblesKey;

        private string globalVariblesKeyPattern;

        private bool isToUpper;

        private string endLine;

        [Category("SerialPort"), Description("PortName"), TypeConverter(typeof(PortNamesConverter))]
        public string PortName
        {
            get { return portName; }
            set { portName = value; }
        }

        [Category("SerialPort"), Description("BaudRate")]
        public int BaudRate
        {
            get { return baudRate; }
            set { baudRate = value; }
        }

        [Category("SerialPort"), Description("数据终端就绪 (RTS) 信号")]
        public bool Rts
        {
            get { return rts; }
            set { rts = value; }
        }

        [Category("SerialPort"), Description("数据终端就绪 (DTR) 信号")]
        public bool Dtr
        {
            get { return dtr; }
            set { dtr = value; }
        }


        [Category("ATCommand"), Description("ATCommand")]
        public string AtCommand
        {
            get { return atCommand; }
            set { atCommand = value; }
        }

        [Category("ATCommand"), Description("error")]
        public string AtCommandError
        {
            get { return atCommandError; }
            set { atCommandError = value; }
        }

        [Category("ATCommand"), Description("ok")]
        public string AtCommandOk
        {
            get { return atCommandOk; }
            set { atCommandOk = value; }
        }

        [Category("ATCommand"), Description("interval, unit:ms")]
        public int AtCommandInterval
        {
            get
            {
                return atCommandInterval;
            }

            set
            {
                this.atCommandInterval = value;
            }
        }

        [Category("CheckInfo"), Description("check info")]
        public string[] CheckInfo
        {
            get { return checkInfo; }
            set { checkInfo = value; }
        }

        [Category("Response"), Description("ToUpper 接收的数据转大写")]
        public bool IsToUpper
        {
            get { return isToUpper; }
            set { isToUpper = value; }
        }
        [Category("SerialPort"), Description("AT Command 结束符")]
        public string EndLine
        {
            get { return endLine; }
            set { endLine = value; }
        }

        //[Category("ReadInfo"), Description("global value's key")]
        //public string GlobalVariblesKey
        //{
        //    get { return globalVariblesKey; }
        //    set { globalVariblesKey = value; }
        //}

        //[Category("ReadInfo"), Description("global varibles key pattern")]
        //public string GlobalVariblesKeyPattern
        //{
        //    get { return globalVariblesKeyPattern; }
        //    set { globalVariblesKeyPattern = value; }
        //}


    }
}
