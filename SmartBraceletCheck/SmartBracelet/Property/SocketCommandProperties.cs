﻿using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class SocketCommandProperties : CommonProperties
    {
        //private string portName;

        private string socketCommand;

        private string socketCommandError;

        private string socketCommandOk;

        private int socketCommandInterval;

        private string[] checkInfo;

        private string[] globalVariblesKey;

        private string[] globalVariblesKeyPattern;

        public EnumCommandType commandType;

        public enum EnumCommandType
        {
            String,
            Hex
        }

        //[Category("SerialPort"), Description("PortName"), TypeConverter(typeof(PortNamesConverter))]
        //public string PortName
        //{
        //    get { return portName; }
        //    set { portName = value; }
        //}

        [Category("Command"), Description("Command")]
        public string SocketCommand
        {
            get { return socketCommand; }
            set { socketCommand = value; }
        }

        [Category("ATCommand"), Description("error")]
        public string SocketCommandError
        {
            get { return socketCommandError; }
            set { socketCommandError = value; }
        }

        [Category("ATCommand"), Description("ok")]
        public string SocketCommandOk
        {
            get { return socketCommandOk; }
            set { socketCommandOk = value; }
        }

        [Category("ATCommand"), Description("interval, unit:ms")]
        public int SocketCommandInterval
        {
            get
            {
                return socketCommandInterval;
            }

            set
            {
                this.socketCommandInterval = value;
            }
        }

        [Category("CheckInfo"), Description("check info")]
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

        [Category("CommandType"), Description("串口命令类型")]
        public EnumCommandType CommandType
        {
            get { return commandType; }
            set { commandType = value; }
        }

    }
}
