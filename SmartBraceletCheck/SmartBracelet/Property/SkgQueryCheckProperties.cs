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

        [Category("VolMaxValue"), Description("标准电压最大值,mV")]
        public int VolMaxValue
        {
            get { return volMaxValue; }
            set { volMaxValue = value; }
        }

        [Category("VolMinValue"), Description("标准电压最小值,mV")]
        public int VolMinValue
        {
            get { return volMinValue; }
            set { volMinValue = value; }
        }

        [Category("Ntc1MaxValue"), Description("标准NTC1温度最大值,单位:0.1摄氏度")]
        public int Ntc1MaxValue
        {
            get { return ntc1MaxValue; }
            set { ntc1MaxValue = value; }
        }

        [Category("Ntc1MinValue"), Description("标准NTC1温度最小值,单位:0.1摄氏度")]
        public int Ntc1MinValue
        {
            get { return ntc1MinValue; }
            set { ntc1MinValue = value; }
        }

        [Category("Ntc2MaxValue"), Description("标准NTC2温度最大值,单位:0.1摄氏度")]
        public int Ntc2MaxValue
        {
            get { return ntc2MaxValue; }
            set { ntc2MaxValue = value; }
        }

        [Category("Ntc2MinValue"), Description("标准NTC2温度最小值,单位:0.1摄氏度")]
        public int Ntc2MinValue
        {
            get { return ntc2MinValue; }
            set { ntc2MinValue = value; }
        }

        [Category("Ntc3MaxValue"), Description("标准NTC3温度最大值,单位:0.1摄氏度")]
        public int Ntc3MaxValue
        {
            get { return ntc3MaxValue; }
            set { ntc3MaxValue = value; }
        }

        [Category("Ntc3MinValue"), Description("标准NTC3温度最小值,单位:0.1摄氏度")]
        public int Ntc3MinValue
        {
            get { return ntc3MinValue; }
            set { ntc3MinValue = value; }
        }


        [Category("MotorSpeedMaxValue"), Description("标准电机转速最大值")]
        public int MotorSpeedMaxValue
        {
            get { return motorSpeedMaxValue; }
            set { motorSpeedMaxValue = value; }
        }

        [Category("MotorSpeedMinValue"), Description("标准电机转速最小值")]
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

        [Category("FirewareVersion"), Description("硬件版本号")]
        public string FirewareVersion
        {
            get { return firewareVersion; }
            set { firewareVersion = value; }
        }

        [Category("SoftwareVersion"), Description("软件版本号")]
        public string SoftwareVersion
        {
            get { return softwareVersion; }
            set { softwareVersion = value; }
        }
        [Category("FirmwareName"), Description("固件名称")]
        public string FirmwareName
        {
            get { return firmwareName; }
            set { firmwareName = value; }
        }
        
    }
}
