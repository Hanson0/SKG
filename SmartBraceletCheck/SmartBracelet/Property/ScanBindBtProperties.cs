﻿using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class ScanBindBtProperties : CommonProperties
    {
        private string macGlobalVaribles;

        private bool connectionPairing;

        //private string atCommand;

        //private string atCommandError;

        //private string atCommandOk;

        //private int atCommandInterval;

        //private string[] checkInfo;

        //private string globalVariblesKey;

        //private string globalVariblesKeyPattern;



        [Category("MacGlobalVaribles"), Description("将要扫描MAC为该变量的蓝牙信号")]
        public string MacGlobalVaribles
        {
            get { return macGlobalVaribles; }
            set { macGlobalVaribles = value; }
        }

        [Category("ConnectionPairing"), Description("是否进行连接配对的手动确认操作")]
        public bool ConnectionPairing
        {
            get { return connectionPairing; }
            set { connectionPairing = value; }
        }



        //[Category("ATCommand"), Description("error")]
        //public string AtCommandError
        //{
        //    get { return atCommandError; }
        //    set { atCommandError = value; }
        //}

        //[Category("ATCommand"), Description("ok")]
        //public string AtCommandOk
        //{
        //    get { return atCommandOk; }
        //    set { atCommandOk = value; }
        //}

        //[Category("ATCommand"), Description("interval, unit:ms")]
        //public int AtCommandInterval
        //{
        //    get
        //    {
        //        return atCommandInterval;
        //    }

        //    set
        //    {
        //        this.atCommandInterval = value;
        //    }
        //}

        //[Category("CheckInfo"), Description("check info")]
        //public string[] CheckInfo
        //{
        //    get { return checkInfo; }
        //    set { checkInfo = value; }
        //}

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
