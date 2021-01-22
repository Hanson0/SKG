using AILinkFactoryAuto.Dut.AtCommand.Property.Converter;
using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class SKGAuthorizeCommandProperties : CommonProperties
    {
        private string head;
        private string reservedWord;
        private string commandWord;
        private EnumAuthorizeEvent authorizeEvent;
        //private string sn_PCBA_IDVariable;
        private string bleBroadcastName;
        private string deviceType;
        private string reservedWord1;
        private string reservedWord2;


        //private EnumPowerOnOffSetting powerOnOffSetting;
        //private EnumLedModeSetting ledModeSetting;
        //private EnumEmsTestSwitch emsTestSwitch;

        //private int emsPWSetting;
        //private int emsFreqSetting;
        //private int emsAmplitudeSetting;

        //private EnumHeatingGearControl heatingGearControl;
        //private EnumVoiceControl voiceControl;
        //private EnumWritePcbaFinishFlag writePcbaFinishFlag;
        //private EnumWholeMachineFinishFlag wholeMachineFinishFlag;
        //private EnumBtTestOnOffSetting btTestOnOffSetting;
        //private EnumMotorControl motorControl;
        //private EnumAginTestOnOffSetting aginTestOnOffSetting;
        //private int aginTestTime;

        //private EnumVibrationControl vibrationControl1;
        //private EnumVibrationControl vibrationControl2;
        //private EnumVibrationControl vibrationControl3;
        //private EnumVibrationControl vibrationControl4;
        //private EnumVibrationControl vibrationControl5;
        //private EnumVibrationControl vibrationControl6;
        //private EnumVibrationControl vibrationControl7;
        //private EnumVibrationControl vibrationControl8;
        //private EnumVibrationControl vibrationControl9;
        //private EnumVibrationControl vibrationControl10;
        //private EnumVibrationControl vibrationControl11;
        //private EnumVibrationControl vibrationControl12;
        //private EnumVibrationControl vibrationControl13;
        //private EnumVibrationControl vibrationControl14;
        //private EnumVibrationControl vibrationControl15;
        //private EnumVibrationControl vibrationControl16;

        //private EnumVibrationControl redLightControl640nm;

        private string portName;

        private string atCommand;

        private string atCommandError;

        private string atCommandOk;

        private int atCommandInterval;

        private string[] checkInfo;

        private string[] globalVariblesKey;

        private string[] globalVariblesKeyPattern;


        private int dataLength;

        private int respDataLength;

        private EnumCommandType commandType;

        public enum EnumCommandType
        {
            查询指令,
            控制指令,
            下发授权
        }

        public enum EnumAuthorizeEvent
        {
            SN_授权 = 1,
            PCBA_ID授权 = 2,
            解锁SWD = 3,
            授权写入蓝牙广播报信息=4,
        }





        [Category("Head"), Description("命令起始标志")]
        public string Head
        {
            get { return head; }
            set { head = value; }
        }

        [Category("ReservedWord"), Description("保留字")]
        public string ReservedWord
        {
            get { return reservedWord; }
            set { reservedWord = value; }
        }

        [Category("CommandWord"), Description("命令字")]
        public string CommandWord
        {
            get { return commandWord; }
            set { commandWord = value; }
        }

        [Category("AuthorizeContent"), Description("授权事件")]
        public EnumAuthorizeEvent AuthorizeEvent
        {
            get { return authorizeEvent; }
            set { authorizeEvent = value; }
        }

        //[Category("Sn/PcbaAuthorize"), Description("存放SN/PCBAID号的变量")]
        //public string Sn_PCBA_IDVariable
        //{
        //    get { return sn_PCBA_IDVariable; }
        //    set { sn_PCBA_IDVariable = value; }
        //}

        [Category("BleBroadcastAuthorize"), Description("BLE广播名,最长17个字符")]
        public string BleBroadcastName
        {
            get { return bleBroadcastName; }
            set { bleBroadcastName = value; }
        }

        
        [Category("BleBroadcastAuthorize"), Description("设备类型,5个字节")]
        public string DeviceType
        {
            get { return deviceType; }
            set { deviceType = value; }
        }

        [Category("BleBroadcastAuthorize"), Description("保留字1")]
        public string ReservedWord1
        {
            get { return reservedWord1; }
            set { reservedWord1 = value; }
        }
        [Category("BleBroadcastAuthorize"), Description("保留字2")]
        public string ReservedWord2
        {
            get { return reservedWord2; }
            set { reservedWord2 = value; }
        }

        [Category("DataLength"), Description("数据部分长度")]
        public int DataLength
        {
            get { return dataLength; }
            set { dataLength = value; }
        }

        [Category("DataLength"), Description("回复数据的数据部分长度")]
        public int RespDataLength
        {
            get { return respDataLength; }
            set { respDataLength = value; }
        }


        [Category("SerialPort"), Description("PortName"), TypeConverter(typeof(PortNamesConverter))]
        public string PortName
        {
            get { return portName; }
            set { portName = value; }
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

        [Category("CommandType"), Description("命令类型")]
        public EnumCommandType CommandType
        {
            get { return commandType; }
            set { commandType = value; }
        }

    }
}
