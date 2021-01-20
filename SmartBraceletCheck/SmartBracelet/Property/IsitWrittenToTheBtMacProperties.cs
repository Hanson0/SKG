using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class IsitWrittenToTheBtMacProperties : CommonProperties
    {
        public enum GetBtMacRetType
        {
            ThrowExcption,
            SKIP,
        }

        private string atCommand;

        private string atCommandError;

        private string atCommandOk;

        private int atCommandInterval;

        private string[] checkInfo;

        private string globalVariblesKey;

        private string globalVariblesKeyPattern;

        private bool imesPreCheck;

        private GetBtMacRetType resultType;

        [Category("Result"), Description("ResultType，未读取到BT MAC后，是报错还是忽略")]
        public GetBtMacRetType ResultType
        {
            get { return resultType; }
            set { resultType = value; }
        }

        [Category("iMes"), Description("iMesPreCheck，主号在IMES的预检查,是否开启")]
        public bool ImesPreCheck
        {
            get { return imesPreCheck; }
            set { imesPreCheck = value; }
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

        [Category("ReadInfo"), Description("global value's key")]
        public string GlobalVariblesKey
        {
            get { return globalVariblesKey; }
            set { globalVariblesKey = value; }
        }

        [Category("ReadInfo"), Description("global varibles key pattern")]
        public string GlobalVariblesKeyPattern
        {
            get { return globalVariblesKeyPattern; }
            set { globalVariblesKeyPattern = value; }
        }
    }
}
