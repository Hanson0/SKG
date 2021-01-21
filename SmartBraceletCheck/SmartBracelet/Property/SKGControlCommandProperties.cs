using AILinkFactoryAuto.Dut.AtCommand.Property.Converter;
using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class SKGControlCommandProperties : CommonProperties
    {
        private string head;
        private string reservedWord;
        private string commandWord;
        private EnumPowerOnOffSetting powerOnOffSetting;
        private EnumLedModeSetting ledModeSetting;
        private EnumEmsTestSwitch emsTestSwitch;

        private int emsPWSetting;
        private int emsFreqSetting;
        private int emsAmplitudeSetting;

        private EnumHeatingGearControl heatingGearControl;
        private EnumVoiceControl voiceControl;
        private EnumWritePcbaFinishFlag writePcbaFinishFlag;
        private EnumWholeMachineFinishFlag wholeMachineFinishFlag;
        private EnumBtTestOnOffSetting btTestOnOffSetting;
        private EnumMotorControl motorControl;
        private EnumAginTestOnOffSetting aginTestOnOffSetting;
        private int aginTestTime;

        private EnumVibrationControl vibrationControl1;
        private EnumVibrationControl vibrationControl2;
        private EnumVibrationControl vibrationControl3;
        private EnumVibrationControl vibrationControl4;
        private EnumVibrationControl vibrationControl5;
        private EnumVibrationControl vibrationControl6;
        private EnumVibrationControl vibrationControl7;
        private EnumVibrationControl vibrationControl8;
        private EnumVibrationControl vibrationControl9;
        private EnumVibrationControl vibrationControl10;
        private EnumVibrationControl vibrationControl11;
        private EnumVibrationControl vibrationControl12;
        private EnumVibrationControl vibrationControl13;
        private EnumVibrationControl vibrationControl14;
        private EnumVibrationControl vibrationControl15;
        private EnumVibrationControl vibrationControl16;

        private EnumVibrationControl redLightControl640nm;
        
        private string portName;

        private string atCommand;

        private string atCommandError;

        private string atCommandOk;

        private int atCommandInterval;

        private string[] checkInfo;

        private string[] globalVariblesKey;

        private string[] globalVariblesKeyPattern;

        private int dataLength;

        private EnumCommandType commandType;

        public enum EnumCommandType
        {
            查询指令,
            控制指令,
            下发授权
        }

        public enum EnumPowerOnOffSetting
        {
            开机=1,
            关机=2,
            跳转到bootloader_升级预留 = 3
        }

        public enum EnumLedModeSetting
        {
            红或橙色LED开=1,
            蓝色LED开=2,
            LED全关=3,
            绿色LED开=4,
        }

        public enum EnumEmsTestSwitch
        {
            开启正反交替脉冲_2电极产品=1,
            关闭=2,
            正反交替脉冲A_B=3,
            正反交替脉冲C_D=4,
            正反交替脉冲E_F=5,
            正反交替脉冲G_F=6,
        }

        public enum EnumHeatingGearControl
        {
            开启42度加热或加热中档 = 1,
            关闭加热 = 2,
            加热低档 = 3,
            加热高档 = 4,
        }

        public enum EnumVoiceControl
        {
            不输出语音 = 0,
            输出一段语音_输出蜂鸣器响声 = 1,
        }
        public enum EnumWritePcbaFinishFlag
        {
            PCBA测试未完成_标志不写入flash = 0,
            PCBA测试完成_标志写入flash = 1,
        }
        public enum EnumWholeMachineFinishFlag
        {
            整机测试未完成_标志不写入flash = 0,
            整机测试完成_标志写入flash = 1,
        }

        public enum EnumBtTestOnOffSetting
        {
            开启蓝牙测试 = 1,
            关闭蓝牙测试 = 2,
        }

        public enum EnumMotorControl
        {
            电机输出一段震动 = 1,
            关闭电机 = 2,
            开启1档力度 = 3,
            开启2档力度 = 4,
            开启3档力度 = 5,
            开启4档力度 = 6,
        }

        public enum EnumAginTestOnOffSetting
        {
            不开启老化测试 = 0,
            开启老化测试 = 1,
        }

        public enum EnumVibrationControl
        {
            开 = 0,
            关 = 1,
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

        [Category("DataContent"), Description("开关机设置")]
        public EnumPowerOnOffSetting PowerOnOffSetting
        {
            get { return powerOnOffSetting; }
            set { powerOnOffSetting = value; }
        }

        [Category("DataContent"), Description("LED模式设置")]
        public EnumLedModeSetting LedModeSetting
        {
            get { return ledModeSetting; }
            set { ledModeSetting = value; }
        }

        [Category("EMS"), Description("EMS 测试开关")]
        public EnumEmsTestSwitch EmsTestSwitch
        {
            get { return emsTestSwitch; }
            set { emsTestSwitch = value; }
        }

        [Category("EMS"), Description("EMS脉宽设置（单位 uS）")]
        public int EmsPWSetting
        {
            get { return emsPWSetting; }
            set { emsPWSetting = value; }
        }
        [Category("EMS"), Description("EMS调制频率设置（单位 Hz）")]
        public int EmsFreqSetting
        {
            get { return emsFreqSetting; }
            set { emsFreqSetting = value; }
        }
        [Category("EMS"), Description("EMS幅度（峰峰值）设置（单位 mV）")]
        public int EmsAmplitudeSetting
        {
            get { return emsAmplitudeSetting; }
            set { emsAmplitudeSetting = value; }
        }

        [Category("DataContent"), Description("加热档位控制")]
        public EnumHeatingGearControl HeatingGearControl
        {
            get { return heatingGearControl; }
            set { heatingGearControl = value; }
        }

        [Category("DataContent"), Description("语音/蜂鸣器控制")]
        public EnumVoiceControl VoiceControl
        {
            get { return voiceControl; }
            set { voiceControl = value; }
        }

        [Category("DataContent"), Description("PCBA 测试完成标志")]
        public EnumWritePcbaFinishFlag WritePcbaFinishFlag
        {
            get { return writePcbaFinishFlag; }
            set { writePcbaFinishFlag = value; }
        }

        [Category("DataContent"), Description("整机测试完成标志")]
        public EnumWholeMachineFinishFlag WholeMachineFinishFlag
        {
            get { return wholeMachineFinishFlag; }
            set { wholeMachineFinishFlag = value; }
        }

        [Category("DataContent"), Description("蓝牙模块测试")]
        public EnumBtTestOnOffSetting BtTestOnOffSetting
        {
            get { return btTestOnOffSetting; }
            set { btTestOnOffSetting = value; }
        }

        [Category("DataContent"), Description("电机控制,多电机产品可以根据测试需求依次振动")]
        public EnumMotorControl MotorControl
        {
            get { return motorControl; }
            set { motorControl = value; }
        }

        [Category("DataContent"), Description("老化测试开关")]
        public EnumAginTestOnOffSetting AginTestOnOffSetting
        {
            get { return aginTestOnOffSetting; }
            set { aginTestOnOffSetting = value; }
        }

        [Category("DataContent"), Description("老化时间设置（单位 10 分钟）,在到达指定老化时长后,固件在FLASH里写完成老化标志")]
        public int AginTestTime
        {
            get { return aginTestTime; }
            set { aginTestTime = value; }
        }
        #region 多路振动电机控制
        [Category("DataContent"), Description("第1通道振动电机控制")]
        public EnumVibrationControl VibrationControl1
        {
            get { return vibrationControl1; }
            set { vibrationControl1 = value; }
        }

        [Category("DataContent"), Description("第2通道振动电机控制")]
        public EnumVibrationControl VibrationControl2
        {
            get { return vibrationControl2; }
            set { vibrationControl2 = value; }
        }
        [Category("DataContent"), Description("第3通道振动电机控制")]
        public EnumVibrationControl VibrationControl3
        {
            get { return vibrationControl3; }
            set { vibrationControl3 = value; }
        }
        [Category("DataContent"), Description("第4通道振动电机控制")]
        public EnumVibrationControl VibrationControl4
        {
            get { return vibrationControl4; }
            set { vibrationControl4 = value; }
        }
        [Category("DataContent"), Description("第5通道振动电机控制")]
        public EnumVibrationControl VibrationControl5
        {
            get { return vibrationControl5; }
            set { vibrationControl5 = value; }
        }
        [Category("DataContent"), Description("第6通道振动电机控制")]
        public EnumVibrationControl VibrationControl6
        {
            get { return vibrationControl6; }
            set { vibrationControl6 = value; }
        }
        [Category("DataContent"), Description("第7通道振动电机控制")]
        public EnumVibrationControl VibrationControl7
        {
            get { return vibrationControl7; }
            set { vibrationControl7 = value; }
        }
        [Category("DataContent"), Description("第8通道振动电机控制")]
        public EnumVibrationControl VibrationControl8
        {
            get { return vibrationControl8; }
            set { vibrationControl8 = value; }
        }
        [Category("DataContent"), Description("第9通道振动电机控制")]
        public EnumVibrationControl VibrationControl9
        {
            get { return vibrationControl9; }
            set { vibrationControl9 = value; }
        }
        [Category("DataContent"), Description("第10通道振动电机控制")]
        public EnumVibrationControl VibrationControl10
        {
            get { return vibrationControl10; }
            set { vibrationControl10 = value; }
        }
        [Category("DataContent"), Description("第11通道振动电机控制")]
        public EnumVibrationControl VibrationControl11
        {
            get { return vibrationControl11; }
            set { vibrationControl11 = value; }
        }
        [Category("DataContent"), Description("第12通道振动电机控制")]
        public EnumVibrationControl VibrationControl12
        {
            get { return vibrationControl12; }
            set { vibrationControl12 = value; }
        }
        [Category("DataContent"), Description("第13通道振动电机控制")]
        public EnumVibrationControl VibrationControl13
        {
            get { return vibrationControl13; }
            set { vibrationControl13 = value; }
        }
        [Category("DataContent"), Description("第14通道振动电机控制")]
        public EnumVibrationControl VibrationControl14
        {
            get { return vibrationControl14; }
            set { vibrationControl14 = value; }
        }
        [Category("DataContent"), Description("第15通道振动电机控制")]
        public EnumVibrationControl VibrationControl15
        {
            get { return vibrationControl15; }
            set { vibrationControl15 = value; }
        }
        [Category("DataContent"), Description("第16通道振动电机控制")]
        public EnumVibrationControl VibrationControl16
        {
            get { return vibrationControl16; }
            set { vibrationControl16 = value; }
        }

        #endregion

        [Category("DataContent"), Description("640nm 红光控制,G7 新增功能")]
        public EnumVibrationControl RedLightControl640nm
        {
            get { return redLightControl640nm; }
            set { redLightControl640nm = value; }
        }

        [Category("DataLength"), Description("数据部分长度")]
        public int DataLength
        {
            get { return dataLength; }
            set { dataLength = value; }
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
