using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class SkgQueryCheckProperties : CommonProperties
    {
        //private bool checkPcba_id;
        //private bool checkSn;
        private EnumCheckNumberInFlash checkNumberInFlash;
        private EnumIfCheckFinishTest ifCheckFinishTest;

        private int volMaxValue;
        private int volMinValue;

        private int ntc1MaxValue;
        private int ntc1MinValue;

        private int ntc2MaxValue;
        private int ntc2MinValue;

        private int ntc3MaxValue;
        private int ntc3MinValue;


        private int motorSpeedMaxValue;
        private int motorSpeedMinValue;
        
        private string globalVariblesKey;
        private string firewareVersion;
        private string softwareVersion;
        private string firmwareName;

        public enum EnumIfCheckFinishTest
        {
            不检查,
            检查是否完成过PCBA测试,
            检查是否完成过整机测试,
            检查是否完成过老化测试,
        }


        public enum EnumCheckNumberInFlash
        {
            不检查,
            检查是否写入过SN,
            检查是否写入过PCBA_ID,
            检查是否写入过蓝牙广播信息,
        }

        //[Category("NumberCheck"), Description("是否检查写入的PCBA-ID")]
        //public bool CheckPcba_id
        //{
        //    get { return checkPcba_id; }
        //    set { checkPcba_id = value; }
        //}

        //[Category("NumberCheck"), Description("是否检查写入的SN")]
        //public bool CheckSn
        //{
        //    get { return checkSn; }
        //    set { checkSn = value; }
        //}
        
        [Category("CheckIfFinishTest"), Description("检查是否已完成过测试")]
        public EnumIfCheckFinishTest IfCheckFinishTest
        {
            get { return ifCheckFinishTest; }
            set { ifCheckFinishTest = value; }
        }

        [Category("CheckIfWriteNumberInFlash"), Description("检查是否向Flash写入过号（SN/PCBA-ID/蓝牙广播信息）")]
        public EnumCheckNumberInFlash CheckNumberInFlash
        {
            get { return checkNumberInFlash; }
            set { checkNumberInFlash = value; }
        }
        

        [Category("Vol"), Description("标准电压最大值,mV")]
        public int VolMaxValue
        {
            get { return volMaxValue; }
            set { volMaxValue = value; }
        }

        [Category("Vol"), Description("标准电压最小值,mV")]
        public int VolMinValue
        {
            get { return volMinValue; }
            set { volMinValue = value; }
        }

        [Category("Ntc1"), Description("标准NTC1温度最大值,单位:0.1摄氏度")]
        public int Ntc1MaxValue
        {
            get { return ntc1MaxValue; }
            set { ntc1MaxValue = value; }
        }

        [Category("Ntc1"), Description("标准NTC1温度最小值,单位:0.1摄氏度")]
        public int Ntc1MinValue
        {
            get { return ntc1MinValue; }
            set { ntc1MinValue = value; }
        }

        [Category("Ntc2"), Description("标准NTC2温度最大值,单位:0.1摄氏度")]
        public int Ntc2MaxValue
        {
            get { return ntc2MaxValue; }
            set { ntc2MaxValue = value; }
        }

        [Category("Ntc2"), Description("标准NTC2温度最小值,单位:0.1摄氏度")]
        public int Ntc2MinValue
        {
            get { return ntc2MinValue; }
            set { ntc2MinValue = value; }
        }

        [Category("Ntc3"), Description("标准NTC3温度最大值,单位:0.1摄氏度")]
        public int Ntc3MaxValue
        {
            get { return ntc3MaxValue; }
            set { ntc3MaxValue = value; }
        }

        [Category("Ntc3"), Description("标准NTC3温度最小值,单位:0.1摄氏度")]
        public int Ntc3MinValue
        {
            get { return ntc3MinValue; }
            set { ntc3MinValue = value; }
        }


        [Category("MotorSpeed"), Description("标准电机转速最大值")]
        public int MotorSpeedMaxValue
        {
            get { return motorSpeedMaxValue; }
            set { motorSpeedMaxValue = value; }
        }

        [Category("MotorSpeed"), Description("标准电机转速最小值")]
        public int MotorSpeedMinValue
        {
            get { return motorSpeedMinValue; }
            set { motorSpeedMinValue = value; }
        }


        [Category("ReadInfo"), Description("global value's key")]
        public string GlobalVariblesKey
        {
            get { return globalVariblesKey; }
            set { globalVariblesKey = value; }
        }

        [Category("Info"), Description("硬件版本号")]
        public string FirewareVersion
        {
            get { return firewareVersion; }
            set { firewareVersion = value; }
        }

        [Category("Info"), Description("软件版本号")]
        public string SoftwareVersion
        {
            get { return softwareVersion; }
            set { softwareVersion = value; }
        }
        [Category("Info"), Description("固件名称")]
        public string FirmwareName
        {
            get { return firmwareName; }
            set { firmwareName = value; }
        }
        
    }
}
