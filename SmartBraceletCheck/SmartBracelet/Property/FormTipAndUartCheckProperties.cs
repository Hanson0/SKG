using AILinkFactoryAuto.Dut.AtCommand.Property.Converter;
using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class FormTipAndUartCheckProperties : CommonProperties
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


        private int countDownTime = 30000;

        private string tips;
        private EnumUartDataType uartDataType;

        
        private EnumCheckKeyEvent checkKeyEvent;
        private EnumCheckUsbInsertEvent checkUsbInsert;
        private EnumCheckBatteryStatus checkBatteryStatus;
        private EnumCheckWearingStatus checkWearingStatus;
        

        public enum EnumCheckKeyEvent
        {
            不检查,
            无按键,
            电源键按下,
            加热键被按下,
            加键被按下,
            减键被按下,
        }

        
        public enum EnumCheckUsbInsertEvent
        {
            不检查,
            无状态改变,
            USB未插入,
            USB已插入,
        }
        public enum EnumCheckBatteryStatus
        {
            不检查,
            无状态改变,
            未充电,
            充电中,
            充满状态,
            电量不足,
        }

        public enum EnumCheckWearingStatus
        {
            不检查,
            无状态改变,
            未检测到佩戴,
            检测到有佩戴,
        }
        
        public enum EnumUartDataType
        {
            String,
            Hex,
        }

        //private Keys keyPass;

        //private Keys keyFail;

        //public UserConfirmProperties()
        //{
        //    this.RetryCount = 0;
        //}



        [Category("StatusEventReportCheck"), Description("检查按键状态事件")]
        public EnumCheckKeyEvent CheckKeyEvent
        {
            get { return checkKeyEvent; }
            set { checkKeyEvent = value; }
        }

        [Category("StatusEventReportCheck"), Description("检查USB插入状态")]
        public EnumCheckUsbInsertEvent CheckUsbInsert
        {
            get { return checkUsbInsert; }
            set { checkUsbInsert = value; }
        }

        [Category("StatusEventReportCheck"), Description("检查电池状态")]
        public EnumCheckBatteryStatus CheckBatteryStatus
        {
            get { return checkBatteryStatus; }
            set { checkBatteryStatus = value; }
        }

        [Category("StatusEventReportCheck"), Description("检查佩戴状态")]
        public EnumCheckWearingStatus CheckWearingStatus
        {
            get { return checkWearingStatus; }
            set { checkWearingStatus = value; }
        }

        [Category("Common"), Description("count down time, unit:ms")]
        public int CountDownTime
        {
            get { return countDownTime; }
            set { countDownTime = value; }
        }

        [Category("Common"), Description("串口数据类型")]
        public EnumUartDataType UartDataType
        {
            get { return uartDataType; }
            set { uartDataType = value; }
        }


        [Category("Common"), Description("tips")]
        public string Tips
        {
            get { return tips; }
            set { tips = value; }
        }




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
