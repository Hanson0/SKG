using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Dut.AtCommand.Executer;
using AILinkFactoryAuto.Dut.AtCommand.Property;
using AILinkFactoryAuto.Task;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using AILinkFactoryAuto.Task.SmartBracelet.Executer;
using AILinkFactoryAuto.Task.SmartBracelet.Property;
using AILinkFactoryAuto.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static AILinkFactoryAuto.Dut.AtCommand.Property.AtCommandProperties;

namespace AILinkFactoryAuto.GenJts.SmartBraceletJts
{
    public partial class CreatJtsForm : Form, IGenJts
    {
        public string ProjectPath { get; set; }
        public CreatJtsForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ParseLabelProperties properties = new ParseLabelProperties()
            {
                MacLocation = 0,
                //ImeiLocation = 1,
                SnLocation = 1,
            };
            TaskItemManager taskItemManager = new TaskItemManager(properties);
            taskItemManager.MesPreCheckProperties.EnableCheckMac = false;
            taskItemManager.TaskItemParseLabel.Enable = true;
            //生成log 
            taskItemManager.DeinitProperties.LogType = LogType.SN;



            if (false)
            {
                TaskItem readBinItem = new TaskItem();
                readBinItem.Enable = true;
                readBinItem.Item = "检测DUT上电";//Read MAP From File
                readBinItem.CommonProperties = new CmdFindDeviceProperties()
                {
                    ToolFolder = "adb",

                    Command = "adb devices",
                    CommandInterval=800,//800,

                    CommandError = "error",

                    CommandOk = "\tdevice\r\n\r\n",// "wifi init success",//+NOTICE:SCANFINISH  upload param init
                    //CheckInfo = new string[] { "Starting device recovery", "Erasing flash", "Finished writing images" },//mac: B4C9B9A4A6E5

                    Timeout = 30000,
                    RetryCount = 0,
                };
                readBinItem.Executer = new CmdFindDeviceExecuter();
                taskItemManager.Put(readBinItem);
            }
            ////开始录音
            //if (true)
            //{
            //    TaskItem readBinItem = new TaskItem();
            //    readBinItem.Enable = true;
            //    readBinItem.Item = "MIC测试-开始录音";//Read MAP From File
            //    readBinItem.CommonProperties = new CmdProperties()
            //    {
            //        IsMicorsoftInit = false,

            //        ToolFolder = "adb",

            //        //录音加存储命令：Command = "adb shell arecord -D \"hw:0,0\" -r 48000 -c 2 -d {0} -f \"S16_LE\" /tmp/record.wav",
            //        //Command = "adb shell arecord -Dhw:0,3 -r 16000 -f S16_LE -d 7 -c 8 /tmp/test.raw",
            //        Command = "adb shell arecord -D \"hw:0,3\" -r 16000 -c 8 -d 3 -f \"S16_LE\" /tmp/test.wav",

            //        CommandDelayTime = 4000,
            //        CommandError = "error",

            //        CommandOk = "Recording WAVE",
            //        //CheckInfo = new string[] { "Starting device recovery", "Erasing flash", "Finished writing images" },//mac: B4C9B9A4A6E5

            //        Timeout = 30000,
            //        RetryCount = 0,
            //    };
            //    readBinItem.Executer = new CmdExecuter();
            //    taskItemManager.Put(readBinItem);
            //}
            //////断开录音
            ////if (true)
            ////{
            ////    TaskItem readBinItem = new TaskItem();
            ////    readBinItem.Enable = true;
            ////    readBinItem.Item = "MIC测试-断开录音";//Read MAP From File
            ////    readBinItem.CommonProperties = new CmdProperties()
            ////    {
            ////        IsMicorsoftInit = false,

            ////        ToolFolder = "adb",

            ////        Command = "[ctrl+c]",
            ////        CommandError = "error",

            ////        CommandOk = "Device recovered successfully",// "wifi init success",//+NOTICE:SCANFINISH  upload param init
            ////        CheckInfo = new string[] { "Starting device recovery", "Erasing flash", "Finished writing images" },//mac: B4C9B9A4A6E5

            ////        Timeout = 60000,
            ////        RetryCount = 0,
            ////    };
            ////    readBinItem.Executer = new CmdExecuter();
            ////    taskItemManager.Put(readBinItem);
            ////}
            ////存储录音
            //if (true)
            //{
            //    TaskItem readBinItem = new TaskItem();
            //    readBinItem.Enable = true;
            //    readBinItem.Item = "MIC测试-存储录音";//Read MAP From File
            //    readBinItem.CommonProperties = new CmdProperties()
            //    {
            //        IsMicorsoftInit = false,

            //        ToolFolder = "adb",

            //        Command = @"adb pull /tmp/test.raw C:\you\path",

            //        CommandDelayTime = 1500,

            //        CommandError = "error",

            //        CommandOk = "1 file pulled",// "wifi init success",//+NOTICE:SCANFINISH  upload param init
            //        CheckInfo = new string[] { "100%", "Erasing flash" },//mac: B4C9B9A4A6E5

            //        Timeout = 10000,
            //        RetryCount = 0,
            //    };
            //    readBinItem.Executer = new CmdExecuter();
            //    taskItemManager.Put(readBinItem);
            //}

            //MIC测试-灵敏度_失真检测
            if (false)
            {
                TaskItem readBinItem = new TaskItem();
                readBinItem.Enable = true;
                readBinItem.Item = "MIC测试-灵敏度_失真检测";//Read MAP From File
                readBinItem.CommonProperties = new CmdSplThdCheckProperties()
                {
                    IsMicorsoftInit = false,

                    ToolFolder = "adb",

                    Command = "adb shell SA -l0 -F1000~1000 -L32767 -d4",
                    CommandDelayTime = 5000,

                    CommandError = "error",

                    CommandOk = "channel5",// "wifi init success",//+NOTICE:SCANFINISH  upload param init
                                           //CheckInfo = new string[] { "Starting device recovery", "Erasing flash", "Finished writing images" },//mac: B4C9B9A4A6E5

                    CheckFreqValue = 984,
                    SplMaxOffset=6,//dB
                    ThdMaxOffset=10,//%

                    Timeout = 30000,
                    RetryCount = 0,
                };
                readBinItem.Executer = new CmdSplThdCheckExecuter();
                taskItemManager.Put(readBinItem);
            }

            #region
            ////播放高频
            //if (true)
            //{
            //    TaskItem readBinItem = new TaskItem();
            //    readBinItem.Enable = true;
            //    readBinItem.Item = "喇叭测试-播放高频";//Read MAP From File
            //    readBinItem.CommonProperties = new CmdProperties()
            //    {
            //        IsMicorsoftInit = false,

            //        //ToolFolderPath = @"C:\Program Files (x86)\Microsoft Azure Sphere SDK",
            //        ToolFolder = "adb",

            //        Command = "adb shell aplay /usr/share/test/high.wav",
            //        CommandDelayTime = 4000,

            //        CommandError = "error",

            //        CommandOk = "Playing WAVE",// "wifi init success",//+NOTICE:SCANFINISH  upload param init
            //        //CheckInfo = new string[] { "Starting device recovery", "Erasing flash", "Finished writing images" },//mac: B4C9B9A4A6E5
            //        //GlobalVariblesKeyPattern = new string[] { "Device ID: ([0-9A-F]{128})" },//, "([0-9A-Fa-f:]{18})" 
            //        //GlobalVariblesKey = new string[] { "{DeviceId}" },//, "{DevBt_MAC}" 

            //        Timeout = 15000,
            //        RetryCount = 0,
            //    };
            //    readBinItem.Executer = new CmdExecuter();
            //    taskItemManager.Put(readBinItem);
            //}
            ////高频判断
            //if (true)
            //{
            //    UserConfirmProperties userConfirmProperties = new UserConfirmProperties()
            //    {
            //        Tips = "喇叭检测-高频判断",
            //        CountDownTime = 15000,
            //        KeyPass = Keys.Space,
            //        KeyFail = Keys.F,

            //    };
            //    taskItemManager.AppendUserConfirm(userConfirmProperties);
            //}

            ////播放低频
            //if (true)
            //{
            //    TaskItem readBinItem = new TaskItem();
            //    readBinItem.Enable = true;
            //    readBinItem.Item = "喇叭测试-播放低频";//Read MAP From File
            //    readBinItem.CommonProperties = new CmdProperties()
            //    {
            //        IsMicorsoftInit = false,

            //        //ToolFolderPath = @"C:\Program Files (x86)\Microsoft Azure Sphere SDK",
            //        ToolFolder = "adb",
            //        CommandDelayTime = 4000,
            //        Command = "adb shell aplay /usr/share/test/low.wav",
            //        CommandError = "error",

            //        CommandOk = "Playing WAVE",// "wifi init success",//+NOTICE:SCANFINISH  upload param init
            //        //CheckInfo = new string[] { "Starting device recovery", "Erasing flash", "Finished writing images" },//mac: B4C9B9A4A6E5
            //        //GlobalVariblesKeyPattern = new string[] { "Device ID: ([0-9A-F]{128})" },//, "([0-9A-Fa-f:]{18})" 
            //        //GlobalVariblesKey = new string[] { "{DeviceId}" },//, "{DevBt_MAC}" 

            //        Timeout = 60000,
            //        RetryCount = 0,
            //    };
            //    readBinItem.Executer = new CmdExecuter();
            //    taskItemManager.Put(readBinItem);
            //}
            #endregion

            //功放板测试-
            //打开WIFI UART
            if (true)
            {
                OpenPhoneProperties configOpenWifiUart = new OpenPhoneProperties();
                configOpenWifiUart.PortName = "COM4";
                configOpenWifiUart.BaudRate = 115200;
                configOpenWifiUart.Dtr = true;
                configOpenWifiUart.Rts = true;
                //configOpenWifiUart.EndLine = "\\r\\n";
                configOpenWifiUart.Timeout = 10 * 1000;
                configOpenWifiUart.RetryCount = 0;
                configOpenWifiUart.AtType = AtType.Manual;
                configOpenWifiUart.SleepTimeAfterFindDut = 100;
                TaskItem openWifiUartItem = new TaskItem();
                openWifiUartItem.Enable = true;
                openWifiUartItem.Item = "打开串口";//Open Wifi Uart
                openWifiUartItem.CommonProperties = configOpenWifiUart;
                openWifiUartItem.Executer = new OpenPhoneExecutor();
                taskItemManager.Put(openWifiUartItem);
            }
            //发现设备
            if (false)
            {
                TaskItem findDevice = new TaskItem();
                findDevice.Enable = true;
                findDevice.Item = "检测产品上电中...";//Find Device
                findDevice.CommonProperties = new FindDeviceProperties()
                {
                    PortName = "COM4",
                    TestPowerOnAT = "",
                    AtCommandInterval = 100,
                    EndLine = "",//\r\n
                    AtCommandOk = "[97F] Default BB Swing=30",// "wifi init success",//+NOTICE:SCANFINISH  upload param init
                    //CheckInfo = new string[] { "FW VER: 1.0.5" },//mac: B4C9B9A4A6E5
                    //GlobalVariblesKeyPattern = new string[] { "MAC = ([0-9A-F]{12})", "BT MAC = ([0-9A-F]{12})" },//, "([0-9A-Fa-f:]{18})" 
                    //GlobalVariblesKey = new string[] { "{DevWifi_MAC}","{DevBt_MAC}" },//, "{DevBt_MAC}" 

                    Timeout = 30 * 1000,
                    RetryCount = 0,
                    SleepTimeBefore = 0,
                };
                findDevice.Executer = new FindDeviceExecuter();
                taskItemManager.Put(findDevice);
            }
            //发送指令
            if (true)
            {
                TaskItem findDevice = new TaskItem();
                findDevice.Enable = true;
                findDevice.Item = "发送查询指令";//Find Device
                findDevice.CommonProperties = new SKGProperties()
                {
                    PortName = "COM4",
                    AtCommand = "A5C3001100000037FFFFFFFFFFFFFFFF40",
                    AtCommandInterval = 500,
                    CommandType= SKGProperties.EnumCommandType.查询指令,
                    //CommandType= EnumCommandType
                    //EndLine = "",//\r\n
                    //AtCommandOk = "[97F] Default BB Swing=30",// "wifi init success",//+NOTICE:SCANFINISH  upload param init
                    //CheckInfo = new string[] { "FW VER: 1.0.5" },//mac: B4C9B9A4A6E5
                    //GlobalVariblesKeyPattern = new string[] { "MAC = ([0-9A-F]{12})", "BT MAC = ([0-9A-F]{12})" },//, "([0-9A-Fa-f:]{18})" 
                    GlobalVariblesKey = new string[] { "{RetQueryData}"},//, "{DevBt_MAC}" 
                    DataLength = 32,
                    //Timeout = 30 * 1000,
                    RetryCount = 0,
                    SleepTimeBefore = 0,
                };
                findDevice.Executer = new SKGExecuter();
                taskItemManager.Put(findDevice);
            }
            //查询内容检查
            if (true)
            {
                TaskItem findDevice = new TaskItem();
                findDevice.Enable = true;
                findDevice.Item = "查询内容检查";//Find Device
                findDevice.CommonProperties = new SkgQueryCheckProperties()
                {

                    GlobalVariblesKey= "RetQueryData",
                    FirmwareName= "K4-2T-",
                    FirewareVersion="1010",
                    SoftwareVersion="1011",
                    VolMaxValue=3000,//mv
                    VolMinValue=500,
                    Ntc1MaxValue=900,//0.1摄氏度
                    Ntc1MinValue=10,

                    MotorSpeedMaxValue=1000,//电机转速
                    MotorSpeedMinValue=100,

                    Ntc2MaxValue = 900,//0.1摄氏度
                    Ntc2MinValue = 10,

                    Ntc3MaxValue = 900,//0.1摄氏度
                    Ntc3MinValue = 10,

                    //Timeout = 30 * 1000,
                    RetryCount = 0,
                    SleepTimeBefore = 0,
                };
                findDevice.Executer = new SkgQueryCheckExecuter();
                taskItemManager.Put(findDevice);
            }
            //控制命令要自己来计算校验位
            //参数应该是开启电机1档,2档,3档;LED灯颜色
            //EMS脉宽设置命令后，通过采集串口发命令获取电流值（自动化:大部分每改变一个状态，都要去检测是否OK）


            //发送指令
            if (true)
            {
                TaskItem findDevice = new TaskItem();
                findDevice.Enable = true;
                findDevice.Item = "发送控制指令";//Find Device
                findDevice.CommonProperties = new SKGControlCommandProperties()
                {
                    PortName = "COM4",
                    Head= "A5C3",
                    DataLength = 20,
                    ReservedWord = "0000",
                    CommandWord="0040",
                    PowerOnOffSetting= SKGControlCommandProperties.EnumPowerOnOffSetting.开机,
                    LedModeSetting= SKGControlCommandProperties.EnumLedModeSetting.红或橙色LED开,
                    EmsTestSwitch= SKGControlCommandProperties.EnumEmsTestSwitch.开启正反交替脉冲_2电极产品,
                    EmsPWSetting= 1000,
                    EmsFreqSetting=1000,
                    EmsAmplitudeSetting=1000,
                    HeatingGearControl= SKGControlCommandProperties.EnumHeatingGearControl.开启42度加热或加热中档,
                    VoiceControl= SKGControlCommandProperties.EnumVoiceControl.输出一段语音_输出蜂鸣器响声,
                    WritePcbaFinishFlag= SKGControlCommandProperties.EnumWritePcbaFinishFlag.PCBA测试未完成_标志不写入flash,
                    WholeMachineFinishFlag= SKGControlCommandProperties.EnumWholeMachineFinishFlag.整机测试未完成_标志不写入flash,
                    BtTestOnOffSetting= SKGControlCommandProperties.EnumBtTestOnOffSetting.开启蓝牙测试,
                    MotorControl= SKGControlCommandProperties.EnumMotorControl.开启1档力度,
                    AginTestOnOffSetting= SKGControlCommandProperties.EnumAginTestOnOffSetting.不开启老化测试,
                    AginTestTime= 30,//30分钟
                    VibrationControl1= SKGControlCommandProperties.EnumVibrationControl.开,
                    VibrationControl2 = SKGControlCommandProperties.EnumVibrationControl.开,
                    VibrationControl3 = SKGControlCommandProperties.EnumVibrationControl.开,
                    VibrationControl4 = SKGControlCommandProperties.EnumVibrationControl.开,
                    VibrationControl5 = SKGControlCommandProperties.EnumVibrationControl.开,
                    VibrationControl6 = SKGControlCommandProperties.EnumVibrationControl.开,
                    VibrationControl7 = SKGControlCommandProperties.EnumVibrationControl.开,
                    VibrationControl8 = SKGControlCommandProperties.EnumVibrationControl.开,
                    VibrationControl9 = SKGControlCommandProperties.EnumVibrationControl.开,
                    VibrationControl10 = SKGControlCommandProperties.EnumVibrationControl.开,
                    VibrationControl11 = SKGControlCommandProperties.EnumVibrationControl.开,
                    VibrationControl12 = SKGControlCommandProperties.EnumVibrationControl.开,
                    VibrationControl13 = SKGControlCommandProperties.EnumVibrationControl.开,
                    VibrationControl14 = SKGControlCommandProperties.EnumVibrationControl.开,
                    VibrationControl15 = SKGControlCommandProperties.EnumVibrationControl.开,
                    VibrationControl16 = SKGControlCommandProperties.EnumVibrationControl.开,

                    RedLightControl640nm= SKGControlCommandProperties.EnumVibrationControl.开,


                    //AtCommand = "A5C3001D0000004001010103E803E800640101000001030003FFFF015E",
                    AtCommandInterval = 500,
                    CommandType = SKGControlCommandProperties.EnumCommandType.控制指令,
                    //CommandType= EnumCommandType
                    //EndLine = "",//\r\n
                    //AtCommandOk = "[97F] Default BB Swing=30",// "wifi init success",//+NOTICE:SCANFINISH  upload param init
                    //CheckInfo = new string[] { "FW VER: 1.0.5" },//mac: B4C9B9A4A6E5
                    //GlobalVariblesKeyPattern = new string[] { "MAC = ([0-9A-F]{12})", "BT MAC = ([0-9A-F]{12})" },//, "([0-9A-Fa-f:]{18})" 
                    GlobalVariblesKey = new string[] { "{RetControlData}" },//, "{DevBt_MAC}" 

                    //Timeout = 30 * 1000,
                    RetryCount = 0,
                    SleepTimeBefore = 0,
                };
                findDevice.Executer = new SKGControlCommandExecuter();
                taskItemManager.Put(findDevice);
            }

            //打开WIFI UART
            if (true)
            {
                OpenPhoneProperties configOpenWifiUart = new OpenPhoneProperties();
                configOpenWifiUart.PortName = "COM5";
                configOpenWifiUart.BaudRate = 115200;
                configOpenWifiUart.Dtr = true;
                configOpenWifiUart.Rts = true;
                //configOpenWifiUart.EndLine = "\\r\\n";
                configOpenWifiUart.Timeout = 10 * 1000;
                configOpenWifiUart.RetryCount = 0;
                configOpenWifiUart.AtType = AtType.Manual;
                configOpenWifiUart.SleepTimeAfterFindDut = 100;
                TaskItem openWifiUartItem = new TaskItem();
                openWifiUartItem.Enable = true;
                openWifiUartItem.Item = "打开检测串口";//Open Wifi Uart
                openWifiUartItem.CommonProperties = configOpenWifiUart;
                openWifiUartItem.Executer = new OpenPhoneExecutor();
                taskItemManager.Put(openWifiUartItem);
            }
            if (true)
            {
                TaskItem checkBtVersionItem = new TaskItem();
                checkBtVersionItem.Enable = true;
                checkBtVersionItem.Item = "AT-获取低压通道1";//Write BT MAC
                AtCommandProperties checkBtVersionProperties = new AtCommandProperties();
                checkBtVersionProperties.PortName = "COM5";
                checkBtVersionProperties.AtCommand = "AT+LVL1?";

                checkBtVersionProperties.AtCommandOk = "OK";
                checkBtVersionProperties.AtCommandError = "UNKNOWN";

                checkBtVersionProperties.GlobalVariblesKeyPattern = new string[] { "LVL1=([.0-9]{5})" };//, "([0-9A-Fa-f:]{18})" 
                checkBtVersionProperties.GlobalVariblesKey = new string[] { "{LVL1}" };//, "{DevBt_MAC}" 

                checkBtVersionProperties.RetryCount = 3;
                checkBtVersionProperties.AtCommandInterval = 800;
                checkBtVersionProperties.SleepTimeBefore = 100;
                checkBtVersionProperties.Timeout = 8000;

                checkBtVersionItem.CommonProperties = checkBtVersionProperties;
                checkBtVersionItem.Executer = new AtCommandExecuter();
                taskItemManager.Put(checkBtVersionItem);
            }
            //低压通道1检查
            if (true)
            {
                TaskItem checkNbVersion = new TaskItem();
                checkNbVersion.Enable = true;
                checkNbVersion.Item = "检查低压通道1";//Check NB IMEI
                checkNbVersion.CommonProperties = new CheckRangeValueProperties()
                {
                    MaxValue = 3.0,
                    MinValue = 1.0,

                    TestValueName = "低压通道1电压值",
                    GlobalVarible = "LVL1",


                    Timeout = 3 * 1000,
                    RetryCount = 0,
                };
                checkNbVersion.Executer = new CheckRangeValueExecuter();
                taskItemManager.Put(checkNbVersion);
            }



            #region 低压通道
            //if (true)
            //{
            //    TaskItem checkBtVersionItem = new TaskItem();
            //    checkBtVersionItem.Enable = true;
            //    checkBtVersionItem.Item = "AT-获取低压通道1";//Write BT MAC
            //    AtCommandProperties checkBtVersionProperties = new AtCommandProperties();
            //    checkBtVersionProperties.PortName = "COM4";
            //    checkBtVersionProperties.AtCommand = "AT+LVL1?";

            //    checkBtVersionProperties.AtCommandOk = "OK";
            //    checkBtVersionProperties.AtCommandError = "UNKNOWN";

            //    checkBtVersionProperties.GlobalVariblesKeyPattern = new string[] { "LVL1=([.0-9]{5})"};//, "([0-9A-Fa-f:]{18})" 
            //    checkBtVersionProperties.GlobalVariblesKey = new string[] {"{LVL1}"};//, "{DevBt_MAC}" 

            //    checkBtVersionProperties.RetryCount = 3;
            //    checkBtVersionProperties.AtCommandInterval = 800;
            //    checkBtVersionProperties.SleepTimeBefore = 100;
            //    checkBtVersionProperties.Timeout = 8000;

            //    checkBtVersionItem.CommonProperties = checkBtVersionProperties;
            //    checkBtVersionItem.Executer = new AtCommandExecuter();
            //    taskItemManager.Put(checkBtVersionItem);
            //}
            ////低压通道1检查
            //if (true)
            //{
            //    TaskItem checkNbVersion = new TaskItem();
            //    checkNbVersion.Enable = true;
            //    checkNbVersion.Item = "检查低压通道1";//Check NB IMEI
            //    checkNbVersion.CommonProperties = new CheckRangeValueProperties()
            //    {
            //        MaxValue = 3.0,
            //        MinValue = 1.0,

            //        TestValueName = "低压通道1电压值",
            //        GlobalVarible = "LVL1",


            //        Timeout = 3 * 1000,
            //        RetryCount = 0,
            //    };
            //    checkNbVersion.Executer = new CheckRangeValueExecuter();
            //    taskItemManager.Put(checkNbVersion);
            //}

            //if (true)
            //{
            //    TaskItem checkBtVersionItem = new TaskItem();
            //    checkBtVersionItem.Enable = true;
            //    checkBtVersionItem.Item = "AT-获取低压通道2";//Write BT MAC
            //    AtCommandProperties checkBtVersionProperties = new AtCommandProperties();
            //    checkBtVersionProperties.PortName = "COM4";
            //    //checkBtVersionProperties.CommandType = EnumCommandType.Hex;
            //    checkBtVersionProperties.AtCommand = "AT+LVL2?";

            //    checkBtVersionProperties.AtCommandOk = "OK";
            //    checkBtVersionProperties.AtCommandError = "UNKNOWN";

            //    //checkBtVersionProperties.AtCommandError = "SCAN_TIMEOUT";
            //    //checkBtVersionProperties.CheckInfo = new string[] { "{}" };
            //    checkBtVersionProperties.GlobalVariblesKeyPattern = new string[] { "LVL2=([.0-9]{5})" };//, "([0-9A-Fa-f:]{18})" 
            //    checkBtVersionProperties.GlobalVariblesKey = new string[] { "{LVL2}" };//, "{DevBt_MAC}" 

            //    checkBtVersionProperties.RetryCount = 3;
            //    checkBtVersionProperties.AtCommandInterval = 800;
            //    checkBtVersionProperties.SleepTimeBefore = 100;
            //    checkBtVersionProperties.Timeout = 8000;

            //    checkBtVersionItem.CommonProperties = checkBtVersionProperties;
            //    checkBtVersionItem.Executer = new AtCommandExecuter();
            //    taskItemManager.Put(checkBtVersionItem);
            //}
            ////低压通道1检查
            //if (true)
            //{
            //    TaskItem checkNbVersion = new TaskItem();
            //    checkNbVersion.Enable = true;
            //    checkNbVersion.Item = "检查低压通道2";//Check NB IMEI
            //    checkNbVersion.CommonProperties = new CheckRangeValueProperties()
            //    {
            //        MaxValue = 3.0,
            //        MinValue = 2.0,

            //        TestValueName = "低压通道2电压值",
            //        GlobalVarible = "LVL2",


            //        Timeout = 3 * 1000,
            //        RetryCount = 0,
            //    };
            //    checkNbVersion.Executer = new CheckRangeValueExecuter();
            //    taskItemManager.Put(checkNbVersion);
            //}

            //if (true)
            //{
            //    TaskItem checkBtVersionItem = new TaskItem();
            //    checkBtVersionItem.Enable = true;
            //    checkBtVersionItem.Item = "AT-获取低压通道3";//Write BT MAC
            //    AtCommandProperties checkBtVersionProperties = new AtCommandProperties();
            //    checkBtVersionProperties.PortName = "COM4";
            //    //checkBtVersionProperties.CommandType = EnumCommandType.Hex;
            //    checkBtVersionProperties.AtCommand = "AT+LVL3?";

            //    checkBtVersionProperties.AtCommandOk = "OK";
            //    checkBtVersionProperties.AtCommandError = "UNKNOWN";

            //    //checkBtVersionProperties.AtCommandError = "SCAN_TIMEOUT";
            //    //checkBtVersionProperties.CheckInfo = new string[] { "{}" };
            //    checkBtVersionProperties.GlobalVariblesKeyPattern = new string[] { "LVL3=([.0-9]{5})" };//, "([0-9A-Fa-f:]{18})" 
            //    checkBtVersionProperties.GlobalVariblesKey = new string[] { "{LVL3}" };//, "{DevBt_MAC}" 

            //    checkBtVersionProperties.RetryCount = 3;
            //    checkBtVersionProperties.AtCommandInterval = 800;
            //    checkBtVersionProperties.SleepTimeBefore = 100;
            //    checkBtVersionProperties.Timeout = 8000;

            //    checkBtVersionItem.CommonProperties = checkBtVersionProperties;
            //    checkBtVersionItem.Executer = new AtCommandExecuter();
            //    taskItemManager.Put(checkBtVersionItem);
            //}
            ////低压通道1检查
            //if (true)
            //{
            //    TaskItem checkNbVersion = new TaskItem();
            //    checkNbVersion.Enable = true;
            //    checkNbVersion.Item = "检查低压通道3";//Check NB IMEI
            //    checkNbVersion.CommonProperties = new CheckRangeValueProperties()
            //    {
            //        MaxValue = 5.0,
            //        MinValue = 2.0,

            //        TestValueName = "低压通道3电压值",
            //        GlobalVarible = "LVL3",


            //        Timeout = 3 * 1000,
            //        RetryCount = 0,
            //    };
            //    checkNbVersion.Executer = new CheckRangeValueExecuter();
            //    taskItemManager.Put(checkNbVersion);
            //}
            #endregion
            #region 高压通道
            if (false)
            {
                TaskItem checkBtVersionItem = new TaskItem();
                checkBtVersionItem.Enable = true;
                checkBtVersionItem.Item = "AT-获取高压通道1";//Write BT MAC
                AtCommandProperties checkBtVersionProperties = new AtCommandProperties();
                checkBtVersionProperties.PortName = "COM4";
                checkBtVersionProperties.AtCommand = "AT+HVL1?";

                checkBtVersionProperties.AtCommandOk = "OK";
                checkBtVersionProperties.AtCommandError = "UNKNOWN";
                checkBtVersionProperties.GlobalVariblesKeyPattern = new string[] { "HVL1=([.0-9]{5})" };//, "([0-9A-Fa-f:]{18})" 
                checkBtVersionProperties.GlobalVariblesKey = new string[] { "{HVL1}" };//, "{DevBt_MAC}" 

                checkBtVersionProperties.RetryCount = 3;
                checkBtVersionProperties.AtCommandInterval = 800;
                checkBtVersionProperties.SleepTimeBefore = 100;
                checkBtVersionProperties.Timeout = 8000;

                checkBtVersionItem.CommonProperties = checkBtVersionProperties;
                checkBtVersionItem.Executer = new AtCommandExecuter();
                taskItemManager.Put(checkBtVersionItem);
            }
            //低压通道1检查
            if (false)
            {
                TaskItem checkNbVersion = new TaskItem();
                checkNbVersion.Enable = true;
                checkNbVersion.Item = "检查高压通道1";//Check NB IMEI
                checkNbVersion.CommonProperties = new CheckRangeValueProperties()
                {
                    MaxValue = 3.0,
                    MinValue = 1.0,

                    TestValueName = "高压通道1电压值",
                    GlobalVarible = "HVL1",


                    Timeout = 3 * 1000,
                    RetryCount = 0,
                };
                checkNbVersion.Executer = new CheckRangeValueExecuter();
                taskItemManager.Put(checkNbVersion);
            }

            if (false)
            {
                TaskItem checkBtVersionItem = new TaskItem();
                checkBtVersionItem.Enable = true;
                checkBtVersionItem.Item = "AT-获取高压通道2";//Write BT MAC
                AtCommandProperties checkBtVersionProperties = new AtCommandProperties();
                checkBtVersionProperties.PortName = "COM4";
                //checkBtVersionProperties.CommandType = EnumCommandType.Hex;
                checkBtVersionProperties.AtCommand = "AT+HVL2?";
                checkBtVersionProperties.AtCommandError = "UNKNOWN";

                checkBtVersionProperties.AtCommandOk = "OK";
                //checkBtVersionProperties.AtCommandError = "SCAN_TIMEOUT";
                //checkBtVersionProperties.CheckInfo = new string[] { "{}" };
                checkBtVersionProperties.GlobalVariblesKeyPattern = new string[] { "HVL2=([.0-9]{5})" };//, "([0-9A-Fa-f:]{18})" 
                checkBtVersionProperties.GlobalVariblesKey = new string[] { "{HVL2}" };//, "{DevBt_MAC}" 

                checkBtVersionProperties.RetryCount = 3;
                checkBtVersionProperties.AtCommandInterval = 800;
                checkBtVersionProperties.SleepTimeBefore = 100;
                checkBtVersionProperties.Timeout = 8000;

                checkBtVersionItem.CommonProperties = checkBtVersionProperties;
                checkBtVersionItem.Executer = new AtCommandExecuter();
                taskItemManager.Put(checkBtVersionItem);
            }
            //低压通道1检查
            if (false)
            {
                TaskItem checkNbVersion = new TaskItem();
                checkNbVersion.Enable = true;
                checkNbVersion.Item = "检查高压通道2";//Check NB IMEI
                checkNbVersion.CommonProperties = new CheckRangeValueProperties()
                {
                    MaxValue = 3.0,
                    MinValue = 2.0,

                    TestValueName = "高压通道2电压值",
                    GlobalVarible = "HVL2",


                    Timeout = 3 * 1000,
                    RetryCount = 0,
                };
                checkNbVersion.Executer = new CheckRangeValueExecuter();
                taskItemManager.Put(checkNbVersion);
            }

            if (false)
            {
                TaskItem checkBtVersionItem = new TaskItem();
                checkBtVersionItem.Enable = true;
                checkBtVersionItem.Item = "AT-获取·高压通道3";//Write BT MAC
                AtCommandProperties checkBtVersionProperties = new AtCommandProperties();
                checkBtVersionProperties.PortName = "COM4";
                //checkBtVersionProperties.CommandType = EnumCommandType.Hex;
                checkBtVersionProperties.AtCommand = "AT+HVL3?";

                checkBtVersionProperties.AtCommandOk = "OK";
                checkBtVersionProperties.AtCommandError = "UNKNOWN";

                //checkBtVersionProperties.AtCommandError = "SCAN_TIMEOUT";
                //checkBtVersionProperties.CheckInfo = new string[] { "{}" };
                checkBtVersionProperties.GlobalVariblesKeyPattern = new string[] { "HVL3=([.0-9]{5})" };//, "([0-9A-Fa-f:]{18})" 
                checkBtVersionProperties.GlobalVariblesKey = new string[] { "{HVL3}" };//, "{DevBt_MAC}" 

                checkBtVersionProperties.RetryCount = 3;
                checkBtVersionProperties.AtCommandInterval = 800;
                checkBtVersionProperties.SleepTimeBefore = 100;
                checkBtVersionProperties.Timeout = 8000;

                checkBtVersionItem.CommonProperties = checkBtVersionProperties;
                checkBtVersionItem.Executer = new AtCommandExecuter();
                taskItemManager.Put(checkBtVersionItem);
            }
            //低压通道1检查
            if (false)
            {
                TaskItem checkNbVersion = new TaskItem();
                checkNbVersion.Enable = true;
                checkNbVersion.Item = "检查高压通道3";//Check NB IMEI
                checkNbVersion.CommonProperties = new CheckRangeValueProperties()
                {
                    MaxValue = 5.0,
                    MinValue = 2.0,

                    TestValueName = "高压通道3电压值",
                    GlobalVarible = "HVL3",


                    Timeout = 3 * 1000,
                    RetryCount = 0,
                };
                checkNbVersion.Executer = new CheckRangeValueExecuter();
                taskItemManager.Put(checkNbVersion);
            }
            #endregion

            #region 电流通道
            if (false)
            {
                TaskItem checkBtVersionItem = new TaskItem();
                checkBtVersionItem.Enable = true;
                checkBtVersionItem.Item = "AT指令-电流通道1";//Write BT MAC
                AtCommandProperties checkBtVersionProperties = new AtCommandProperties();
                checkBtVersionProperties.PortName = "COM4";
                checkBtVersionProperties.AtCommand = "AT+CRT?";

                checkBtVersionProperties.AtCommandOk = "OK";
                checkBtVersionProperties.AtCommandError = "UNKNOWN";
                checkBtVersionProperties.GlobalVariblesKeyPattern = new string[] { "CRT1=([.0-9]{5})" };//, "([0-9A-Fa-f:]{18})" 
                checkBtVersionProperties.GlobalVariblesKey = new string[] { "{CRT1}" };//, "{DevBt_MAC}" 

                checkBtVersionProperties.RetryCount = 3;
                checkBtVersionProperties.AtCommandInterval = 800;
                checkBtVersionProperties.SleepTimeBefore = 100;
                checkBtVersionProperties.Timeout = 8000;

                checkBtVersionItem.CommonProperties = checkBtVersionProperties;
                checkBtVersionItem.Executer = new AtCommandExecuter();
                taskItemManager.Put(checkBtVersionItem);
            }
            //低压通道1检查
            if (false)
            {
                TaskItem checkNbVersion = new TaskItem();
                checkNbVersion.Enable = true;
                checkNbVersion.Item = "检查电流通道1";//Check NB IMEI
                checkNbVersion.CommonProperties = new CheckRangeValueProperties()
                {
                    MaxValue = 3.0,
                    MinValue = 1.0,

                    TestValueName = "电流通道1电流值",
                    GlobalVarible = "CRT1",


                    Timeout = 3 * 1000,
                    RetryCount = 0,
                };
                checkNbVersion.Executer = new CheckRangeValueExecuter();
                taskItemManager.Put(checkNbVersion);
            }

            if (false)
            {
                TaskItem checkBtVersionItem = new TaskItem();
                checkBtVersionItem.Enable = true;
                checkBtVersionItem.Item = "AT指令-电流通道2";//Write BT MAC
                AtCommandProperties checkBtVersionProperties = new AtCommandProperties();
                checkBtVersionProperties.PortName = "COM4";
                //checkBtVersionProperties.CommandType = EnumCommandType.Hex;
                checkBtVersionProperties.AtCommand = "AT+CRT2?";

                checkBtVersionProperties.AtCommandOk = "OK";
                checkBtVersionProperties.AtCommandError = "UNKNOWN";
                //checkBtVersionProperties.CheckInfo = new string[] { "{}" };
                checkBtVersionProperties.GlobalVariblesKeyPattern = new string[] { "CRT2=([.0-9]{5})" };//, "([0-9A-Fa-f:]{18})" 
                checkBtVersionProperties.GlobalVariblesKey = new string[] { "{CRT2}" };//, "{DevBt_MAC}" 

                checkBtVersionProperties.RetryCount = 3;
                checkBtVersionProperties.AtCommandInterval = 800;
                checkBtVersionProperties.SleepTimeBefore = 100;
                checkBtVersionProperties.Timeout = 8000;

                checkBtVersionItem.CommonProperties = checkBtVersionProperties;
                checkBtVersionItem.Executer = new AtCommandExecuter();
                taskItemManager.Put(checkBtVersionItem);
            }
            //低压通道1检查
            if (false)
            {
                TaskItem checkNbVersion = new TaskItem();
                checkNbVersion.Enable = true;
                checkNbVersion.Item = "检查电流通道2";//Check NB IMEI
                checkNbVersion.CommonProperties = new CheckRangeValueProperties()
                {
                    MaxValue = 3.0,
                    MinValue = 2.0,

                    TestValueName = "电流通道2电流值",
                    GlobalVarible = "CRT2",


                    Timeout = 3 * 1000,
                    RetryCount = 0,
                };
                checkNbVersion.Executer = new CheckRangeValueExecuter();
                taskItemManager.Put(checkNbVersion);
            }

            #endregion


            //if (true)
            //{
            //    TaskItem checkBtVersionItem = new TaskItem();
            //    checkBtVersionItem.Enable = true;
            //    checkBtVersionItem.Item = "发送AT指令获取MAC";//Write BT MAC
            //    AtCommandProperties checkBtVersionProperties = new AtCommandProperties();
            //    checkBtVersionProperties.PortName = "COM4";
            //    //checkBtVersionProperties.CommandType = EnumCommandType.Hex;
            //    checkBtVersionProperties.AtCommand = "ifconfig";

            //    checkBtVersionProperties.AtCommandOk = "UP BROADCAST";
            //    //checkBtVersionProperties.AtCommandError = "SCAN_TIMEOUT";
            //    //checkBtVersionProperties.CheckInfo = new string[] { "{DevWifi_MAC}" };
            //    checkBtVersionProperties.GlobalVariblesKeyPattern = new string[] { "br0       Link encap:Ethernet  HWaddr ([:0-9A-F]{17})" };//, "([0-9A-Fa-f:]{18})" 
            //    checkBtVersionProperties.GlobalVariblesKey = new string[] { "{DevWifi_MAC}" };//, "{DevBt_MAC}" 

            //    checkBtVersionProperties.RetryCount = 2;
            //    checkBtVersionProperties.AtCommandInterval = 1000;
            //    checkBtVersionProperties.SleepTimeBefore = 100;
            //    checkBtVersionProperties.Timeout = 8000;

            //    checkBtVersionItem.CommonProperties = checkBtVersionProperties;
            //    checkBtVersionItem.Executer = new AtCommandExecuter();
            //    taskItemManager.Put(checkBtVersionItem);
            //}

            //
            if (true)
            {
                UserConfirmProperties userConfirmProperties = new UserConfirmProperties()
                {
                    Tips = "被测板卡是否有完成声音发出？",
                    CountDownTime = 30000,
                    KeyPass = Keys.Space,
                    KeyFail = Keys.F,

                };
                taskItemManager.AppendUserConfirm(userConfirmProperties);
            }

            //关闭AT UART
            if (true)
            {
                ClosePhoneProperties closeWifiUartItem = new ClosePhoneProperties();
                closeWifiUartItem.PortName = "COM4";
                closeWifiUartItem.BaudRate = 115200;
                closeWifiUartItem.Dtr = true;
                closeWifiUartItem.Rts = true;
                closeWifiUartItem.EndLine = "\\r\\n";
                closeWifiUartItem.Timeout = 10 * 1000;
                closeWifiUartItem.RetryCount = 0;
                closeWifiUartItem.AtType = AtType.Manual;

                TaskItem closeAtUartItem = new TaskItem();

                closeAtUartItem.Enable = true;
                //closeAtUartItem = ExecuteCondition.ALWAYS;

                closeAtUartItem.Item = "关闭串口";//Open Wifi Uart
                closeAtUartItem.CommonProperties = closeWifiUartItem;
                closeAtUartItem.CommonProperties.ExecuteCondition = ExecuteCondition.ALWAYS;
                closeAtUartItem.Executer = new ClosePhoneExecuter();
                taskItemManager.Put(closeAtUartItem);
            }

            if (true)
            {
                ClosePhoneProperties closeWifiUartItem = new ClosePhoneProperties();
                closeWifiUartItem.PortName = "COM5";
                closeWifiUartItem.BaudRate = 115200;
                closeWifiUartItem.Dtr = true;
                closeWifiUartItem.Rts = true;
                closeWifiUartItem.EndLine = "\\r\\n";
                closeWifiUartItem.Timeout = 10 * 1000;
                closeWifiUartItem.RetryCount = 0;
                closeWifiUartItem.AtType = AtType.Manual;

                TaskItem closeAtUartItem = new TaskItem();

                closeAtUartItem.Enable = true;
                //closeAtUartItem = ExecuteCondition.ALWAYS;

                closeAtUartItem.Item = "关闭检测串口";//Open Wifi Uart
                closeAtUartItem.CommonProperties = closeWifiUartItem;
                closeAtUartItem.CommonProperties.ExecuteCondition = ExecuteCondition.ALWAYS;
                closeAtUartItem.Executer = new ClosePhoneExecuter();
                taskItemManager.Put(closeAtUartItem);
            }

            #region

            ////读取产品MAC号
            //if (true)
            //{
            //    TaskItem readBinItem = new TaskItem();
            //    readBinItem.Enable = true;
            //    readBinItem.Item = "读取产品MAC号";//Read MAP From File
            //    readBinItem.CommonProperties = new CmdReadMacProperties()
            //    {
            //        ToolFolder= "ESP_DOWNLOAD_TOOL_v1.1.0.1",
            //        GetMacCmd= "esp_cmm_download_tool_v1.1.0.1.exe 1,0",
            //        Pattern= "ESP_MAC: ([:0-9A-Z]{17})",

            //        DeviceMac = "DevWifi_MAC",

            //        RetryCount = 0,
            //    };
            //    readBinItem.Executer = new CmdReadMacExecuter();
            //    taskItemManager.Put(readBinItem);
            //}

            ////再次扫码
            //if (true)
            //{
            //    //ScanCodeProperties scanCodeProperties = new ScanCodeProperties
            //    //{
            //    //    DutName = "DUT1",
            //    //    //Order=1,
            //    //};
            //    //taskItemManager.AppendScanBarcode(scanCodeProperties);

            //    TaskItem readBinItem = new TaskItem();
            //    readBinItem.Enable = true;
            //    readBinItem.Item = "扫描第二个标签";//Read MAP From File
            //    readBinItem.CommonProperties = new ReScanLabelProperties()
            //    {
            //        RetryCount = 0,
            //    };
            //    readBinItem.Executer = new ReScanLabelExecuter();
            //    taskItemManager.Put(readBinItem);
            //}
            ////检查再次扫码MAC一致性性
            //if (true)
            //{
            //    TaskItem readBinItem = new TaskItem();
            //    readBinItem.Enable = true;
            //    readBinItem.Item = "检查两个标签一致性";//Read MAP From File
            //    readBinItem.CommonProperties = new CheckReScanLabelsProperties()
            //    {
            //        RetryCount = 0,
            //    };
            //    readBinItem.Executer = new CheckReScanLabelsExecuter();
            //    taskItemManager.Put(readBinItem);
            //}
            //if (true)
            //{
            //    //iMes预检查 当无标签时，使用
            //    MesPreCheckProperties mesPreCheckProperties = new MesPreCheckProperties()
            //    {
            //        EnableCheckMac = true,
            //        EnableCheckImei = false,
            //        EnableCheckSn = false,
            //    };
            //    taskItemManager.AppendNewMesPreCheck(mesPreCheckProperties);
            //}

            ////MAC一致性检查
            //if (true)
            //{
            //    CheckMacConsistencyProperties checkMacConsistencyProperties = new CheckMacConsistencyProperties()
            //    {
            //        DeviceMac = "{DevWifi_MAC}",

            //        RetryCount = 0,
            //    };
            //    taskItemManager.AppendCheckMacConsistency(checkMacConsistencyProperties);

            //}
            ////启动网卡——以管理员运行
            //if (true)
            //{
            //    TaskItem findDevice = new TaskItem();
            //    findDevice.Enable = true;
            //    findDevice.Item = "启动网卡";//Find Device
            //    findDevice.CommonProperties = new EnableAdapterProperties()
            //    {
            //        NICname = "Intel(R) Dual Band Wireless-AC 8265",

            //        Timeout = 10 * 1000,
            //        RetryCount = 0,
            //        SleepTimeBefore = 100,
            //        SleepTimeAfter = 2000,//扫码后，等待WIFI刷新的时间
            //    };
            //    findDevice.Executer = new EnableAdapterExecuter();
            //    taskItemManager.Put(findDevice);
            //}

            ////连接WIFI热点
            //if (true)
            //{
            //    TaskItem checkNbVersion = new TaskItem();
            //    checkNbVersion.Enable = true;
            //    checkNbVersion.Item = "连接WIFI热点，同时按下按键";//Check NB IMEI
            //    checkNbVersion.CommonProperties = new ConnectSsiProperties()
            //    {
            //        SSID = "BZL_{0}",
            //        Key = "",
            //        //Timeout = 3 * 1000,
            //        SleepTimeBefore=1000,
            //        RetryInterval=1000,
            //        RetryCount = 1,
            //    };
            //    checkNbVersion.Executer = new ConnectSsiExecuter();
            //    taskItemManager.Put(checkNbVersion);
            //}

            ////socket连接
            //if (true)
            //{
            //    TaskItem checkNbVersion = new TaskItem();
            //    checkNbVersion.Enable = true;
            //    checkNbVersion.Item = "Socket连接";//Check NB IMEI
            //    checkNbVersion.CommonProperties = new SocketConnectProperties()
            //    {
            //        Ip = "192.168.43.1",
            //        Port = "9999",

            //        SleepTimeBefore=2000,
            //        RetryInterval=2000,
            //        RetryCount = 2,
            //    };
            //    checkNbVersion.Executer = new SocketConnectExecuter();
            //    taskItemManager.Put(checkNbVersion);
            //}
            ////发送命令
            //if (true)
            //{
            //    TaskItem checkNbVersion = new TaskItem();
            //    checkNbVersion.Enable = true;
            //    checkNbVersion.Item = "发送Socket指令";//Check NB IMEI
            //    checkNbVersion.CommonProperties = new SocketCommandProperties()
            //    {
            //        SocketCommand = "NI+MAC?",
            //        SocketCommandInterval = 2000,
            //        SocketCommandError = "ERROR",
            //        SocketCommandOk = "ERROR",//"OK｝"
            //        CheckInfo = new string[] { "{MAC}" },//mac: B4C9B9A4A6E5
            //        //GlobalVariblesKeyPattern = new string[] { "mac: ([0-9A-F]{12})" },//, "([0-9A-Fa-f:]{18})" 
            //        //GlobalVariblesKey = new string[] { "{DevWifi_MAC}" },//, "{DevBt_MAC}" 

            //        Timeout = 10 * 1000,
            //        RetryCount = 2,
            //        RetryInterval = 200,

            //        SleepTimeBefore = 0,
            //    };
            //    checkNbVersion.Executer = new SocketCommandExecuter();
            //    taskItemManager.Put(checkNbVersion);
            //}

            ////断开Socket
            //if (true)
            //{
            //    TaskItem checkNbVersion = new TaskItem();
            //    checkNbVersion.Enable = true;
            //    checkNbVersion.Item = "断开Socket连接";//Check NB IMEI
            //    checkNbVersion.CommonProperties = new SocketDisConnectProperties()
            //    {
            //        ExecuteCondition = ExecuteCondition.ALWAYS,
            //        RetryCount = 0,
            //    };
            //    checkNbVersion.Executer = new SocketDisConnectExecuter();
            //    taskItemManager.Put(checkNbVersion);
            //}


            ////关闭连接WIFI热点
            //if (true)
            //{
            //    TaskItem checkNbVersion = new TaskItem();
            //    checkNbVersion.Enable = true;
            //    checkNbVersion.Item = "断开连接WIFI热点";//Check NB IMEI
            //    checkNbVersion.CommonProperties = new DisConnectSsiProperties()
            //    {
            //        ExecuteCondition = ExecuteCondition.ALWAYS,

            //        RetryCount = 0,
            //    };
            //    checkNbVersion.Executer = new DisConnectSsiExecuter();
            //    taskItemManager.Put(checkNbVersion);
            //}

            ////禁用网卡
            //if (true)
            //{
            //    TaskItem findDevice = new TaskItem();
            //    findDevice.Enable = true;
            //    findDevice.Item = "禁用网卡";//Find Device
            //    findDevice.CommonProperties = new DisableAdapterProperties()
            //    {
            //        NICname = "Intel(R) Dual Band Wireless-AC 8265",
            //        ExecuteCondition = ExecuteCondition.ALWAYS,

            //        Timeout = 10 * 1000,
            //        RetryCount = 0,
            //        SleepTimeBefore = 100,
            //    };
            //    findDevice.Executer = new DisableAdapterExecuter();
            //    taskItemManager.Put(findDevice);
            //}



            ////打开WIFI UART
            //if (true)
            //{
            //    OpenPhoneProperties configOpenWifiUart = new OpenPhoneProperties();
            //    configOpenWifiUart.PortName = "COM4";
            //    configOpenWifiUart.BaudRate = 38400;
            //    configOpenWifiUart.Dtr = true;
            //    configOpenWifiUart.Rts = true;
            //    configOpenWifiUart.EndLine = "\\r\\n";
            //    configOpenWifiUart.Timeout = 10 * 1000;
            //    configOpenWifiUart.RetryCount = 0;
            //    configOpenWifiUart.AtType = AtType.Manual;
            //    configOpenWifiUart.SleepTimeAfterFindDut = 100;
            //    TaskItem openWifiUartItem = new TaskItem();
            //    openWifiUartItem.Enable = true;
            //    openWifiUartItem.Item = "打开Log口";//Open Wifi Uart
            //    openWifiUartItem.CommonProperties = configOpenWifiUart;
            //    openWifiUartItem.Executer = new OpenPhoneExecutor();
            //    taskItemManager.Put(openWifiUartItem);
            //}
            //发现设备
            //if (true)
            //{
            //    TaskItem findDevice = new TaskItem();
            //    findDevice.Enable = true;
            //    findDevice.Item = "检测产品上电中...";//Find Device
            //    findDevice.CommonProperties = new FindDeviceProperties()
            //    {
            //        PortName = "COM4",
            //        TestPowerOnAT = "",
            //        AtCommandInterval = 100,
            //        EndLine = "",//\r\n
            //        AtCommandOk = "[97F] Default BB Swing=30",// "wifi init success",//+NOTICE:SCANFINISH  upload param init
            //        //CheckInfo = new string[] { "FW VER: 1.0.5" },//mac: B4C9B9A4A6E5
            //        //GlobalVariblesKeyPattern = new string[] { "MAC = ([0-9A-F]{12})", "BT MAC = ([0-9A-F]{12})" },//, "([0-9A-Fa-f:]{18})" 
            //        //GlobalVariblesKey = new string[] { "{DevWifi_MAC}","{DevBt_MAC}" },//, "{DevBt_MAC}" 

            //        Timeout = 30 * 1000,
            //        RetryCount = 0,
            //        SleepTimeBefore = 0,
            //    };
            //    findDevice.Executer = new FindDeviceExecuter();
            //    taskItemManager.Put(findDevice);
            //}
            //if (true)
            //{
            //    TaskItem checkBtVersionItem = new TaskItem();
            //    checkBtVersionItem.Enable = true;
            //    checkBtVersionItem.Item = "发送AT指令登录";//Write BT MAC
            //    AtCommandProperties checkBtVersionProperties = new AtCommandProperties();
            //    checkBtVersionProperties.PortName = "COM4";
            //    //checkBtVersionProperties.CommandType = EnumCommandType.Hex;
            //    checkBtVersionProperties.AtCommand = "root";

            //    checkBtVersionProperties.AtCommandOk = "BLE/MESH GATEWAY";
            //    //checkBtVersionProperties.AtCommandError = "SCAN_TIMEOUT";
            //    //checkBtVersionProperties.CheckInfo = new string[] { "{DevWifi_MAC}" };
            //    //checkBtVersionProperties.GlobalVariblesKeyPattern = new string[] { "vbat_dat=([0-9]*)" , "rssi_dat=([-0-9]{4})" };//, "([0-9A-Fa-f:]{18})" 
            //    //checkBtVersionProperties.GlobalVariblesKey = new string[] { "{Electricity_Quantity}", "{RSSI}"};//, "{DevBt_MAC}" 

            //    checkBtVersionProperties.RetryCount = 8;
            //    checkBtVersionProperties.AtCommandInterval = 2000;
            //    checkBtVersionProperties.SleepTimeBefore = 100;
            //    checkBtVersionProperties.Timeout = 20000;

            //    checkBtVersionItem.CommonProperties = checkBtVersionProperties;
            //    checkBtVersionItem.Executer = new AtCommandExecuter();
            //    taskItemManager.Put(checkBtVersionItem);
            //}
            //if (true)
            //{
            //    TaskItem checkBtVersionItem = new TaskItem();
            //    checkBtVersionItem.Enable = true;
            //    checkBtVersionItem.Item = "发送AT指令获取MAC";//Write BT MAC
            //    AtCommandProperties checkBtVersionProperties = new AtCommandProperties();
            //    checkBtVersionProperties.PortName = "COM4";
            //    //checkBtVersionProperties.CommandType = EnumCommandType.Hex;
            //    checkBtVersionProperties.AtCommand = "ifconfig";

            //    checkBtVersionProperties.AtCommandOk = "UP BROADCAST";
            //    //checkBtVersionProperties.AtCommandError = "SCAN_TIMEOUT";
            //    //checkBtVersionProperties.CheckInfo = new string[] { "{DevWifi_MAC}" };
            //    checkBtVersionProperties.GlobalVariblesKeyPattern = new string[] { "br0       Link encap:Ethernet  HWaddr ([:0-9A-F]{17})" };//, "([0-9A-Fa-f:]{18})" 
            //    checkBtVersionProperties.GlobalVariblesKey = new string[] { "{DevWifi_MAC}"};//, "{DevBt_MAC}" 

            //    checkBtVersionProperties.RetryCount = 2;
            //    checkBtVersionProperties.AtCommandInterval = 1000;
            //    checkBtVersionProperties.SleepTimeBefore = 100;
            //    checkBtVersionProperties.Timeout = 8000;

            //    checkBtVersionItem.CommonProperties = checkBtVersionProperties;
            //    checkBtVersionItem.Executer = new AtCommandExecuter();
            //    taskItemManager.Put(checkBtVersionItem);
            //}
            ////MAC一致性检查
            //if (true)
            //{
            //    CheckMacConsistencyProperties checkMacConsistencyProperties = new CheckMacConsistencyProperties()
            //    {
            //        DeviceMac = "{DevWifi_MAC}",

            //        RetryCount = 0,
            //    };
            //    taskItemManager.AppendCheckMacConsistency(checkMacConsistencyProperties);
            //}
            ////检查信号强度
            //if (true)
            //{
            //    TaskItem checkNbVersion = new TaskItem();
            //    checkNbVersion.Enable = true;
            //    checkNbVersion.Item = "检查信号强度";//Check NB IMEI
            //    checkNbVersion.CommonProperties = new CheckBtRssiProperties()
            //    {
            //        MaxValue = 0,
            //        MinValue = -80,

            //        Timeout = 3 * 1000,
            //        RetryCount = 0,
            //    };
            //    checkNbVersion.Executer = new CheckBtRssiExecuter();
            //    taskItemManager.Put(checkNbVersion);
            //}
            ////检查电量
            //if (true)
            //{
            //    TaskItem checkNbVersion = new TaskItem();
            //    checkNbVersion.Enable = true;
            //    checkNbVersion.Item = "检查电量";//Check NB IMEI
            //    checkNbVersion.CommonProperties = new CheckRangeValueProperties()
            //    {
            //        MaxValue = 100,
            //        MinValue = 10,

            //        TestValueName="电量值",
            //        GlobalVarible = "Electricity_Quantity",


            //        Timeout = 3 * 1000,
            //        RetryCount = 0,
            //    };
            //    checkNbVersion.Executer = new CheckRangeValueExecuter();
            //    taskItemManager.Put(checkNbVersion);
            //}
            //声音检查
            if (true)
            {
                //UserConfirmProperties userConfirmProperties = new UserConfirmProperties()
                //{
                //    Tips = "喇叭检查",
                //    CountDownTime = 15000,
                //    KeyPass = Keys.Space,
                //    KeyFail = Keys.F,

                //    //EnableCheckMac = true,
                //    //EnableCheckImei = false,
                //    //EnableCheckSn = false,
                //};
                //taskItemManager.AppendUserConfirm(userConfirmProperties);
                //if (true)
                //{
                //    TaskItem findDevice = new TaskItem();
                //    findDevice.Enable = true;
                //    findDevice.Item = "喇叭检查";//Find Device
                //    findDevice.CommonProperties = new FindDeviceProperties()
                //    {
                //        PortName = "COM4",
                //        TestPowerOnAT = "",
                //        AtCommandInterval = 100,
                //        EndLine = "",//\r\n
                //        AtCommandOk = "SPEAKER TEST",// "wifi init success",//+NOTICE:SCANFINISH  upload param init
                //                                                  //CheckInfo = new string[] { "FW VER: 1.0.5" },//mac: B4C9B9A4A6E5
                //                                                  //GlobalVariblesKeyPattern = new string[] { "MAC = ([0-9A-F]{12})", "BT MAC = ([0-9A-F]{12})" },//, "([0-9A-Fa-f:]{18})" 
                //                                                  //GlobalVariblesKey = new string[] { "{DevWifi_MAC}","{DevBt_MAC}" },//, "{DevBt_MAC}" 
                //        AtCommandError= "SPEAKER TEST FAIL",
                //        CheckInfo = new string[] { "SPEAKER TEST PASS" },

                //        Timeout = 15 * 1000,
                //        RetryCount = 0,
                //        SleepTimeBefore = 0,
                //    };
                //    findDevice.Executer = new FindDeviceExecuter();
                //    taskItemManager.Put(findDevice);
                //}


                //TaskItem checkNbVersion = new TaskItem();
                //checkNbVersion.Enable = true;
                //checkNbVersion.Item = "声音检查";//Check NB IMEI
                //checkNbVersion.CommonProperties = new CheckBtRssiProperties()
                //{
                //    MaxValue = 0,
                //    MinValue = -80,

                //    Timeout = 3 * 1000,
                //    RetryCount = 0,
                //};
                //checkNbVersion.Executer = new CheckBtRssiExecuter();
                //taskItemManager.Put(checkNbVersion);
            }
            #endregion

            //LED灯检查
            if (true)
            {
                //UserConfirmProperties userConfirmProperties = new UserConfirmProperties()
                //{
                //    Tips = "触摸检查",
                //    CountDownTime = 15000,
                //    KeyPass = Keys.Space,
                //    KeyFail = Keys.F,

                //    //EnableCheckMac = true,
                //    //EnableCheckImei = false,
                //    //EnableCheckSn = false,
                //};
                //taskItemManager.AppendUserConfirm(userConfirmProperties);

                //if (true)
                //{
                //    TaskItem findDevice = new TaskItem();
                //    findDevice.Enable = true;
                //    findDevice.Item = "触摸检查";
                //    findDevice.CommonProperties = new FindDeviceProperties()
                //    {
                //        PortName = "COM4",
                //        TestPowerOnAT = "",
                //        AtCommandInterval = 100,
                //        EndLine = "",//\r\n
                //        AtCommandOk = "TOUCH TEST",
                //        AtCommandError = "TOUCH TEST FAIL",
                //        CheckInfo = new string[] { "TOUCH TEST PASS" },
                //        Timeout = 15 * 1000,
                //        RetryCount = 0,
                //        SleepTimeBefore = 0,
                //    };
                //    findDevice.Executer = new FindDeviceExecuter();
                //    taskItemManager.Put(findDevice);
                //}

            }
            //LED灯检查
            if (true)
            {
                //if (true)
                //{
                //    TaskItem findDevice = new TaskItem();
                //    findDevice.Enable = true;
                //    findDevice.Item = "光感检查";
                //    findDevice.CommonProperties = new FindDeviceProperties()
                //    {
                //        PortName = "COM4",
                //        TestPowerOnAT = "",
                //        AtCommandInterval = 100,
                //        EndLine = "",//\r\n
                //        AtCommandOk = "LIGHT SENSOR TEST",
                //        AtCommandError = "LIGHT SENSOR TEST FAIL",
                //        CheckInfo = new string[] { "LIGHT SENSOR TEST PASS" },
                //        Timeout = 15 * 1000,
                //        RetryCount = 0,
                //        SleepTimeBefore = 0,
                //    };
                //    findDevice.Executer = new FindDeviceExecuter();
                //    taskItemManager.Put(findDevice);
                //}

                //UserConfirmProperties userConfirmProperties = new UserConfirmProperties()
                //{
                //    Tips = "光感检查",
                //    CountDownTime = 15000,
                //    KeyPass = Keys.Space,
                //    KeyFail = Keys.F,

                //    //EnableCheckMac = true,
                //    //EnableCheckImei = false,
                //    //EnableCheckSn = false,
                //};
                //taskItemManager.AppendUserConfirm(userConfirmProperties);
            }

            //设置devMac为MAC
            if (true)
            {
                //////LOG MAC CHANGE TO BT MAC
                //TaskItem logMacChangeToBtMacItem = new TaskItem();
                //logMacChangeToBtMacItem.Enable = true;
                //logMacChangeToBtMacItem.Item = "LOG名MAC为产品内部WIFI MAC";//LOG MAC CHANGE TO BT MAC
                //logMacChangeToBtMacItem.CommonProperties = new LogMacChangeToThisMacProperties()
                //{
                //    GlobalVarible = "DevWifi_MAC",

                //    RetryCount = 0,
                //    ExecuteCondition = ExecuteCondition.ALWAYS,
                //};
                //logMacChangeToBtMacItem.Executer = new LogMacChangeToThisMacExecuter();
                //taskItemManager.Put(logMacChangeToBtMacItem);
            }


            if (true)
            {
                ////iMes预检查 当无标签时，使用
                //MesPreCheckProperties mesPreCheckProperties = new MesPreCheckProperties()
                //{
                //    EnableCheckMac = true,
                //    EnableCheckImei = false,
                //    EnableCheckSn = false,
                //};
                //taskItemManager.AppendNewMesPreCheck(mesPreCheckProperties);
            }

            //if (true)
            //{
            //    TaskItem checkBtVersionItem = new TaskItem();
            //    checkBtVersionItem.Enable = false;
            //    checkBtVersionItem.Item = "发送AT指令检查";//Write BT MAC
            //    AtCommandProperties checkBtVersionProperties = new AtCommandProperties();
            //    checkBtVersionProperties.PortName = "COM4";
            //    checkBtVersionProperties.AtCommand = "AT+VERSION";

            //    checkBtVersionProperties.AtCommandOk = "OK";
            //    checkBtVersionProperties.AtCommandError = "ERROR";
            //    checkBtVersionProperties.CheckInfo = new string[] { "AI_TEST_8762CMF_SDK1.1.0_V1.1" };
            //    //checkBtVersionProperties.GlobalVariblesKeyPattern = new string[] { "EFUSEMAC:([:0-9A-F]{17})" };//, "([0-9A-Fa-f:]{18})" 
            //    //checkBtVersionProperties.GlobalVariblesKey = new string[] { "{DevWifi_MAC}" };//, "{DevBt_MAC}" 

            //    checkBtVersionProperties.AtCommandInterval = 500;
            //    checkBtVersionProperties.SleepTimeBefore = 100;

            //    checkBtVersionItem.CommonProperties = checkBtVersionProperties;
            //    checkBtVersionItem.Executer = new AtCommandExecuter();
            //    taskItemManager.Put(checkBtVersionItem);
            //}


            //MAC一致性检查
            if (true)
            {
                //CheckMacConsistencyProperties checkMacConsistencyProperties = new CheckMacConsistencyProperties()
                //{
                //    DeviceMac = "{DevWifi_MAC}",

                //    RetryCount = 0,
                //};
                //taskItemManager.AppendCheckMacConsistency(checkMacConsistencyProperties);

            }
            //WIIF-BT MAC加一关系检查
            if (true)
            {
                //CheckPlusOneRelationProperties checkPlusOneRelationProperties = new CheckPlusOneRelationProperties()
                //{
                //    DevWifiMac = "{DevWifi_MAC}",
                //    DevBtMac = "{DevBt_MAC}",

                //    RetryCount=0,
                //};
                //taskItemManager.AppendCheckPlusOneRelation(checkPlusOneRelationProperties);
            }


            //自动识别MP模式并切入用户模式
            if (true)
            {
                //TaskItem checkBtVersionItem = new TaskItem();
                //checkBtVersionItem.Enable = false;
                //checkBtVersionItem.Item = "自动识别MP模式并切入用户模式";//Write BT MAC
                //AutoToUserModeProperties checkBtVersionProperties = new AutoToUserModeProperties();
                //checkBtVersionProperties.PortName = "COM4";
                //checkBtVersionProperties.AtCommand = "ATSC";

                //checkBtVersionProperties.AtCommandOk = "OK";
                //checkBtVersionProperties.AtCommandInterval = 800;
                //checkBtVersionProperties.SleepTimeBefore = 0;
                //checkBtVersionProperties.RetryCount = 3;

                //checkBtVersionItem.CommonProperties = checkBtVersionProperties;
                //checkBtVersionItem.Executer = new AutoToUserModeExecuter();
                //taskItemManager.Put(checkBtVersionItem);
            }

            //切入用户模式
            if (true)
            {
                //TaskItem checkBtVersionItem = new TaskItem();
                //checkBtVersionItem.Enable = false;
                //checkBtVersionItem.Item = "切入用户模式";//Write BT MAC
                //AtCommandProperties checkBtVersionProperties = new AtCommandProperties();
                //checkBtVersionProperties.PortName = "COM4";
                ////checkBtVersionProperties.CommandType = EnumCommandType.Hex;
                //checkBtVersionProperties.AtCommand = "ATSC";

                //checkBtVersionProperties.AtCommandOk = "OK";
                ////checkBtVersionProperties.AtCommandError = "ERROR";
                ////checkBtVersionProperties.CheckInfo = new string[] { "{MAC}" };
                //checkBtVersionProperties.AtCommandInterval = 800;
                //checkBtVersionProperties.SleepTimeBefore = 0;
                //checkBtVersionProperties.RetryCount = 3;

                //checkBtVersionItem.CommonProperties = checkBtVersionProperties;
                //checkBtVersionItem.Executer = new AtCommandExecuter();
                //taskItemManager.Put(checkBtVersionItem);
            }
            //再次扫码
            if (true)
            {
                ////ScanCodeProperties scanCodeProperties = new ScanCodeProperties
                ////{
                ////    DutName = "DUT1",
                ////    //Order=1,
                ////};
                ////taskItemManager.AppendScanBarcode(scanCodeProperties);

                //TaskItem readBinItem = new TaskItem();
                //readBinItem.Enable = true;
                //readBinItem.Item = "再次扫描标签";//Read MAP From File
                //readBinItem.CommonProperties = new ReScanLabelProperties()
                //{
                //    RetryCount=0,
                //};
                //readBinItem.Executer = new ReScanLabelExecuter();
                //taskItemManager.Put(readBinItem);
            }
            //检查再次扫码MAC一致性性
            if (true)
            {
                //TaskItem readBinItem = new TaskItem();
                //readBinItem.Enable = true;
                //readBinItem.Item = "检查再次扫码的标签一致性";//Read MAP From File
                //readBinItem.CommonProperties = new CheckReScanLabelsProperties()
                //{
                //    RetryCount=0,
                //};
                //readBinItem.Executer = new CheckReScanLabelsExecuter();
                //taskItemManager.Put(readBinItem);
            }

            //发现设备
            if (true)
            {
                //TaskItem findDevice = new TaskItem();
                //findDevice.Enable = true;
                //findDevice.Item = "请再次上电检测用户模式下的产品上电和开机信息";//Find Device
                //findDevice.CommonProperties = new FindDeviceProperties()
                //{
                //    PortName = "COM4",
                //    TestPowerOnAT = "",
                //    AtCommandInterval = 100,
                //    EndLine = "",//\r\n
                //    AtCommandOk = "BT Reset ok",//+NOTICE:SCANFINISH  upload param init
                //    CheckInfo = new string[] { "software version : V1.01", "build time: 2020-8-29-13:52:03" },//mac: B4C9B9A4A6E5
                //    GlobalVariblesKeyPattern = new string[] { "mac: ([0-9A-F]{12})"},//, "([0-9A-Fa-f:]{18})" 
                //    GlobalVariblesKey = new string[] { "{DevWifi_MAC}"},//, "{DevBt_MAC}" 

                //    Timeout = 10 * 1000,
                //    RetryCount = 0,
                //    SleepTimeBefore = 0,
                //};
                //findDevice.Executer = new FindDeviceExecuter();
                //taskItemManager.Put(findDevice);
            }





            //一致性检查
            if (true)
            {
                //TaskItem checkWifiMac = new TaskItem();
                //checkWifiMac.Enable = true;
                //checkWifiMac.Item = "WIFI MAC内部与标签一致性检查";//Check BT MAC
                //checkWifiMac.CommonProperties = new CheckWifiMacProperties()
                //{
                //    Timeout = 3 * 1000,
                //    RetryCount = 0,
                //};
                //checkWifiMac.Executer = new CheckWifiMacExecuter();
                //taskItemManager.Put(checkWifiMac);
            }


            //SSID检查
            //进入station模式
            if (true)
            {
                //TaskItem checkBtVersionItem = new TaskItem();
                //checkBtVersionItem.Enable = false;
                //checkBtVersionItem.Item = "进入Station模式";//Write BT MAC
                //AtCommandProperties checkBtVersionProperties = new AtCommandProperties();
                //checkBtVersionProperties.PortName = "COM4";
                ////checkBtVersionProperties.CommandType = EnumCommandType.Hex;
                //checkBtVersionProperties.AtCommand = "AT+STARTSTA";

                //checkBtVersionProperties.AtCommandOk = "OK";
                ////checkBtVersionProperties.AtCommandError = "ERROR";
                ////checkBtVersionProperties.CheckInfo = new string[] { "{MAC}" };
                //checkBtVersionProperties.AtCommandInterval = 800;
                //checkBtVersionProperties.SleepTimeBefore = 500;
                //checkBtVersionProperties.RetryCount = 3;

                //checkBtVersionItem.CommonProperties = checkBtVersionProperties;
                //checkBtVersionItem.Executer = new AtCommandExecuter();
                //taskItemManager.Put(checkBtVersionItem);
            }
            //搜索AP
            if (true)
            {
                //TaskItem checkBtVersionItem = new TaskItem();
                //checkBtVersionItem.Enable = false;
                //checkBtVersionItem.Item = "搜索AP";//Write BT MAC
                //AtCommandProperties checkBtVersionProperties = new AtCommandProperties();
                //checkBtVersionProperties.PortName = "COM4";
                ////checkBtVersionProperties.CommandType = EnumCommandType.Hex;
                //checkBtVersionProperties.AtCommand = "AT+SCAN";

                //checkBtVersionProperties.AtCommandOk = "OK";
                ////checkBtVersionProperties.AtCommandError = "ERROR";
                ////checkBtVersionProperties.CheckInfo = new string[] { "{MAC}" };
                //checkBtVersionProperties.AtCommandInterval = 800;
                //checkBtVersionProperties.SleepTimeBefore = 0;
                //checkBtVersionProperties.RetryCount = 3;

                //checkBtVersionItem.CommonProperties = checkBtVersionProperties;
                //checkBtVersionItem.Executer = new AtCommandExecuter();
                //taskItemManager.Put(checkBtVersionItem);
            }

            //显示搜索结果
            if (true)
            {
                //TaskItem checkBtVersionItem = new TaskItem();
                //checkBtVersionItem.Enable = false;
                //checkBtVersionItem.Item = "获取搜索结果的信号强度";//Write BT MAC
                //AtCommandProperties checkBtVersionProperties = new AtCommandProperties();
                //checkBtVersionProperties.PortName = "COM4";
                ////checkBtVersionProperties.CommandType = EnumCommandType.Hex;
                //checkBtVersionProperties.AtCommand = "AT+SCANRESULT";

                //checkBtVersionProperties.AtCommandOk = "OK";
                ////checkBtVersionProperties.AtCommandError = "ERROR";
                ////checkBtVersionProperties.CheckInfo = new string[] { "{MAC}" };
                //checkBtVersionProperties.GlobalVariblesKey = new string[] { "{RSSI}" };
                //checkBtVersionProperties.GlobalVariblesKeyPattern = new string[] { "TP-Link_D540,[0-9a-z:]{17},[0-9]{1,2},([-0-9]{1,3}),[0-9]{1,2}" };


                //checkBtVersionProperties.AtCommandInterval = 800;
                //checkBtVersionProperties.SleepTimeBefore = 1000;
                //checkBtVersionProperties.RetryCount = 3;

                //checkBtVersionItem.CommonProperties = checkBtVersionProperties;
                //checkBtVersionItem.Executer = new AtCommandExecuter();
                //taskItemManager.Put(checkBtVersionItem);
            }

            //检查 RSSI
            if (true)
            {
                //TaskItem checkBtRssiItem = new TaskItem();
                //checkBtRssiItem.Enable = true;
                //checkBtRssiItem.Item = "检查信号强度";//Check BT RSSI
                //checkBtRssiItem.CommonProperties = new CheckBtRssiProperties()
                //{
                //    MaxValue = -10,
                //    MinValue = -90,
                //    Timeout = 2 * 1000,
                //    RetryCount = 0,
                //};
                //checkBtRssiItem.Executer = new CheckBtRssiExecuter();
                //taskItemManager.Put(checkBtRssiItem);
            }

            ////读取License文件至内存
            //if (true)
            //{
            //    TaskItem readBinItem = new TaskItem();
            //    readBinItem.Enable = true;
            //    readBinItem.Item = "读取License文件";//Read MAP From File
            //    readBinItem.CommonProperties = new ReadMapProperties()
            //    {
            //        BinFolderPath = "./bin",
            //        RetryCount = 0,
            //    };
            //    readBinItem.Executer = new ReadMapExecuter();
            //    taskItemManager.Put(readBinItem);
            //}

            ////发AT+LIC?命令
            //if (true)
            //{
            //    TaskItem checkBtVersionItem = new TaskItem();
            //    checkBtVersionItem.Enable = false;
            //    checkBtVersionItem.Item = "发AT命令，检查回复信息";//Write BT MAC
            //    AtCommandProperties checkBtVersionProperties = new AtCommandProperties();
            //    checkBtVersionProperties.PortName = "COM3";
            //    checkBtVersionProperties.CommandType = EnumCommandType.Hex;
            //    checkBtVersionProperties.AtCommand = "A6 A6 20 05 71";

            //    checkBtVersionProperties.AtCommandOk = "DONE";
            //    //checkBtVersionProperties.AtCommandError = "ERROR";
            //    checkBtVersionProperties.CheckInfo = new string[] { "{MAC}" };
            //    checkBtVersionProperties.AtCommandInterval = 1000;
            //    checkBtVersionProperties.SleepTimeBefore = 800;

            //    checkBtVersionItem.CommonProperties = checkBtVersionProperties;
            //    checkBtVersionItem.Executer = new AtCommandExecuter();
            //    taskItemManager.Put(checkBtVersionItem);
            //}
            //关闭AT UART
            if (true)
            {
                //ClosePhoneProperties closeWifiUartItem = new ClosePhoneProperties();
                //closeWifiUartItem.PortName = "COM4";
                //closeWifiUartItem.BaudRate = 38400;
                //closeWifiUartItem.Dtr = true;
                //closeWifiUartItem.Rts = true;
                //closeWifiUartItem.EndLine = "\\r\\n";
                //closeWifiUartItem.Timeout = 10 * 1000;
                //closeWifiUartItem.RetryCount = 0;
                //closeWifiUartItem.AtType = AtType.Manual;

                //TaskItem closeAtUartItem = new TaskItem();

                //closeAtUartItem.Enable = true;
                ////closeAtUartItem = ExecuteCondition.ALWAYS;

                //closeAtUartItem.Item = "关闭Log口";//Open Wifi Uart
                //closeAtUartItem.CommonProperties = closeWifiUartItem;
                //closeAtUartItem.CommonProperties.ExecuteCondition = ExecuteCondition.ALWAYS;
                //closeAtUartItem.Executer = new ClosePhoneExecuter();
                //taskItemManager.Put(closeAtUartItem);
            }
            ////打开WIFI UART
            //if (true)
            //{
            //    OpenPhoneProperties configOpenWifiUart = new OpenPhoneProperties();
            //    configOpenWifiUart.PortName = "COM5";
            //    configOpenWifiUart.BaudRate = 115200;
            //    configOpenWifiUart.Dtr = true;
            //    configOpenWifiUart.Rts = true;
            //    configOpenWifiUart.EndLine = "\\r\\n";
            //    configOpenWifiUart.Timeout = 10 * 1000;
            //    configOpenWifiUart.RetryCount = 0;
            //    configOpenWifiUart.AtType = AtType.Manual;
            //    configOpenWifiUart.SleepTimeAfterFindDut = 100;
            //    TaskItem openWifiUartItem = new TaskItem();
            //    openWifiUartItem.Enable = true;
            //    openWifiUartItem.Item = "打开扫描通信口";//Open Wifi Uart
            //    openWifiUartItem.CommonProperties = configOpenWifiUart;
            //    openWifiUartItem.Executer = new OpenPhoneExecutor();
            //    taskItemManager.Put(openWifiUartItem);
            //}

            //if (true)
            //{
            //    TaskItem checkBtVersionItem = new TaskItem();
            //    checkBtVersionItem.Enable = false;
            //    checkBtVersionItem.Item = "获取信号强度";//Write BT MAC
            //    AtCommandProperties checkBtVersionProperties = new AtCommandProperties();
            //    checkBtVersionProperties.PortName = "COM5";
            //    checkBtVersionProperties.AtCommand = "AT+INQ={DevWifi_MAC}";

            //    checkBtVersionProperties.AtCommandOk = "OK";
            //    checkBtVersionProperties.AtCommandError = "TIMEOUT";
            //    //checkBtVersionProperties.CheckInfo = new string[] { "AI_TEST_8762CMF_SDK1.1.0_V1.1" };
            //    checkBtVersionProperties.GlobalVariblesKeyPattern = new string[] { "RSSI=([-0-9]{4})" };//, "([0-9A-Fa-f:]{18})" 
            //    checkBtVersionProperties.GlobalVariblesKey = new string[] { "{RSSI}" };//, "{DevBt_MAC}" 

            //    checkBtVersionProperties.AtCommandInterval = 4000;
            //    checkBtVersionProperties.SleepTimeBefore = 100;
            //    checkBtVersionProperties.RetryCount = 1;

            //    checkBtVersionItem.CommonProperties = checkBtVersionProperties;
            //    checkBtVersionItem.Executer = new AtCommandExecuter();
            //    taskItemManager.Put(checkBtVersionItem);
            //}
            ////检查信号强度
            //if (true)
            //{
            //    TaskItem checkNbVersion = new TaskItem();
            //    checkNbVersion.Enable = true;
            //    checkNbVersion.Item = "检查信号强度";//Check NB IMEI
            //    checkNbVersion.CommonProperties = new CheckBtRssiProperties()
            //    {
            //        MaxValue = 0,
            //        MinValue = -80,

            //        Timeout = 3 * 1000,
            //        RetryCount = 0,
            //    };
            //    checkNbVersion.Executer = new CheckBtRssiExecuter();
            //    taskItemManager.Put(checkNbVersion);
            //}



            ////关闭AT UART
            //if (true)
            //{
            //    ClosePhoneProperties closeWifiUartItem = new ClosePhoneProperties();
            //    closeWifiUartItem.PortName = "COM5";
            //    closeWifiUartItem.BaudRate = 115200;
            //    closeWifiUartItem.Dtr = true;
            //    closeWifiUartItem.Rts = true;
            //    closeWifiUartItem.EndLine = "\\r\\n";
            //    closeWifiUartItem.Timeout = 10 * 1000;
            //    closeWifiUartItem.RetryCount = 0;
            //    closeWifiUartItem.AtType = AtType.Manual;

            //    TaskItem closeAtUartItem = new TaskItem();

            //    closeAtUartItem.Enable = true;
            //    //closeAtUartItem = ExecuteCondition.ALWAYS;

            //    closeAtUartItem.Item = "关闭扫描通信口";//Open Wifi Uart
            //    closeAtUartItem.CommonProperties = closeWifiUartItem;
            //    closeAtUartItem.CommonProperties.ExecuteCondition = ExecuteCondition.ALWAYS;
            //    closeAtUartItem.Executer = new ClosePhoneExecuter();
            //    taskItemManager.Put(closeAtUartItem);
            //}


            //打开WIFI UART
            if (true)
            {
                //OpenPhoneProperties configOpenWifiUart = new OpenPhoneProperties();
                //configOpenWifiUart.PortName = "COM5";
                //configOpenWifiUart.BaudRate = 19200;
                //configOpenWifiUart.Dtr = true;
                //configOpenWifiUart.Rts = true;

                ////configOpenWifiUart.Parity = System.IO.Ports.Parity.Even;

                ////configOpenWifiUart.EndLine = "\\r\\n";
                //configOpenWifiUart.Timeout = 10 * 1000;
                //configOpenWifiUart.RetryCount = 0;
                //configOpenWifiUart.AtType = AtType.Manual;
                //configOpenWifiUart.SleepTimeAfterFindDut = 100;
                //TaskItem openWifiUartItem = new TaskItem();
                //openWifiUartItem.Enable = false;
                //openWifiUartItem.Item = "打开Hex通讯口";//Open Wifi Uart
                //openWifiUartItem.CommonProperties = configOpenWifiUart;
                //openWifiUartItem.Executer = new OpenPhoneExecutor();
                //taskItemManager.Put(openWifiUartItem);

            }

            //检查通讯口hex上电
            if (true)
            {
                //TaskItem GetNumItem = new TaskItem();
                //GetNumItem.Enable = true;
                //GetNumItem.Item = "自动检测产品上电-通信串口";//Check NB IMEI
                //GetNumItem.CommonProperties = new CheckHexUartPowerOnProperties()
                //{
                //    PortName = "COM5",
                //    //AtCommand = "7e7e0b9481160000000001020039",
                //    //SleepTimeBefore = 10,
                //    AtCommandInterval = 1 * 1000,

                //    Timeout = 10 * 1000,
                //    RetryCount = 0,

                //};
                //GetNumItem.Executer = new CheckHexUartPowerOnExecuter();
                //taskItemManager.Put(GetNumItem);
            }


            //检查通讯口hex
            if (true)
            {
                //TaskItem GetNumItem = new TaskItem();
                //GetNumItem.Enable = true;
                //GetNumItem.Item = "获取产品信息从通信串口";//Check NB IMEI
                //GetNumItem.CommonProperties = new CheckHexUartProperties()
                //{
                //    PortName = "COM5",
                //    AtCommand = "A6A6AA05FB",
                //    SleepTimeBefore = 10,
                //    AtCommandInterval = 1 * 1000,

                //    //CheckInfo=new string[] {"DONE","ZNKT20016" },

                //    Timeout = 5 * 1000,
                //    RetryCount = 3,
                //};
                //GetNumItem.Executer = new CheckHexUartExecuter();
                //taskItemManager.Put(GetNumItem);
            }
            //检查标签MAC一致性
            if (true)
            {
                //CheckMacConsistencyProperties checkMacConsistencyProperties = new CheckMacConsistencyProperties()
                //{
                //    DeviceMac = "{DevWifi_MAC}",

                //    RetryCount = 0,
                //};
                //taskItemManager.AppendCheckMacConsistency(checkMacConsistencyProperties);

            }


            ////设置devMac为MAC
            //if (true)
            //{
            //    ////LOG MAC CHANGE TO BT MAC
            //    TaskItem logMacChangeToBtMacItem = new TaskItem();
            //    logMacChangeToBtMacItem.Enable = true;
            //    logMacChangeToBtMacItem.Item = "LOG MAC为产品内部WIFI MAC";//LOG MAC CHANGE TO BT MAC
            //    logMacChangeToBtMacItem.CommonProperties = new LogMacChangeToThisMacProperties()
            //    {
            //        GlobalVarible = "DevWifi_MAC",

            //        RetryCount = 0,
            //        ExecuteCondition = ExecuteCondition.ALWAYS,
            //    };
            //    logMacChangeToBtMacItem.Executer = new LogMacChangeToThisMacExecuter();
            //    taskItemManager.Put(logMacChangeToBtMacItem);
            //}

            if (true)
            {
                ////iMes预检查 当无标签时，使用
                //MesPreCheckProperties mesPreCheckProperties = new MesPreCheckProperties()
                //{
                //    EnableCheckMac = true,
                //    EnableCheckImei = false,
                //    EnableCheckSn = false,
                //};
                //taskItemManager.AppendNewMesPreCheck(mesPreCheckProperties);
            }
            //检查信号强度
            if (true)
            {
                //TaskItem checkNbVersion = new TaskItem();
                //checkNbVersion.Enable = true;
                //checkNbVersion.Item = "检查信号强度";//Check NB IMEI
                //checkNbVersion.CommonProperties = new CheckBtRssiProperties()
                //{
                //    MaxValue = 0,
                //    MinValue = -80,

                //    Timeout = 3 * 1000,
                //    RetryCount = 0,
                //};
                //checkNbVersion.Executer = new CheckBtRssiExecuter();
                //taskItemManager.Put(checkNbVersion);
            }


            //检查NB版本号
            if (true)
            {
                //TaskItem checkNbVersion = new TaskItem();
                //checkNbVersion.Enable = true;
                //checkNbVersion.Item = "检查软件版本号";//Check NB IMEI
                //checkNbVersion.CommonProperties = new CheckNbVersionProperties()
                //{
                //    Version = "0.0.3",
                //    GlobalVarible= "VERSION",

                //    Timeout = 5 * 1000,
                //    RetryCount = 0,
                //};
                //checkNbVersion.Executer = new CheckNbVersionExecuter();
                //taskItemManager.Put(checkNbVersion);
            }
            if (true)
            {
                TaskItem checkNbVersion = new TaskItem();
                //checkNbVersion.Enable = true;
                //checkNbVersion.Item = "检查软件编号";//Check NB IMEI
                //checkNbVersion.CommonProperties = new CheckNbVersionProperties()
                //{
                //    Version = "ZNKT20016",
                //    GlobalVarible = "SoftNumber",

                //    Timeout = 5 * 1000,
                //    RetryCount = 0,
                //};
                //checkNbVersion.Executer = new CheckNbVersionExecuter();
                //taskItemManager.Put(checkNbVersion);
            }

            if (true)
            {
                //TaskItem checkNbVersion = new TaskItem();
                //checkNbVersion.Enable = true;
                //checkNbVersion.Item = "检查联网检查结果";//Check NB IMEI
                //checkNbVersion.CommonProperties = new CheckNbVersionProperties()
                //{
                //    Version = "DONE",
                //    GlobalVarible = "RESULT",

                //    Timeout = 5 * 1000,
                //    RetryCount = 0,
                //};
                //checkNbVersion.Executer = new CheckNbVersionExecuter();
                //taskItemManager.Put(checkNbVersion);
            }



            //WIIF-BT MAC加一关系检查
            if (true)
            {
                //CheckPlusOneRelationProperties checkPlusOneRelationProperties = new CheckPlusOneRelationProperties()
                //{
                //    DevWifiMac = "{DevWifi_MAC}",
                //    DevBtMac = "{DevBt_MAC}",

                //    RetryCount = 0,
                //};
                //taskItemManager.AppendCheckPlusOneRelation(checkPlusOneRelationProperties);
            }



            //if (true)
            //{
            //    TaskItem checkWifiBtMac = new TaskItem();
            //    checkWifiBtMac.Enable = true;
            //    checkWifiBtMac.Item = "WIFI与BT MAC核验";//Check BT MAC
            //    checkWifiBtMac.CommonProperties = new CheckWifiBtMacProperties()
            //    {
            //        Timeout = 3 * 1000,
            //        RetryCount = 0,
            //    };
            //    checkWifiBtMac.Executer = new CheckWifiBtMacExecuter();
            //    taskItemManager.Put(checkWifiBtMac);
            //}

            //uart2接收license
            //if (true)
            //{
            //    TaskItem checkBtVersionItem = new TaskItem();
            //    checkBtVersionItem.Enable = true;
            //    checkBtVersionItem.Item = "接收并检查License";//Write BT MAC
            //    AtCommandProperties checkBtVersionProperties = new AtCommandProperties();
            //    checkBtVersionProperties.PortName = "COM25";
            //    checkBtVersionProperties.AtCommand = "";

            //    //checkBtVersionProperties.AtCommandOk = "OK";
            //    //checkBtVersionProperties.AtCommandError = "ERROR";
            //    checkBtVersionProperties.AtCommandInterval = 2000;
            //    //checkBtVersionProperties.SleepTimeBefore = 800;
            //    checkBtVersionProperties.CheckInfo = new string[] { "{BinFileString}" };


            //    checkBtVersionItem.CommonProperties = checkBtVersionProperties;
            //    checkBtVersionItem.Executer = new AtCommandExecuter();
            //    taskItemManager.Put(checkBtVersionItem);
            //}

            if (true)
            {
                //ClosePhoneProperties closeWifiUartItem = new ClosePhoneProperties();
                //closeWifiUartItem.PortName = "COM5";
                //TaskItem closeAtUartItem = new TaskItem();

                //closeAtUartItem.Enable = true;
                //closeAtUartItem.Item = "关闭通讯口";//Open Wifi Uart
                //closeAtUartItem.CommonProperties = closeWifiUartItem;
                //closeAtUartItem.CommonProperties.ExecuteCondition = ExecuteCondition.ALWAYS;

                //closeAtUartItem.Executer = new ClosePhoneExecuter();

                //taskItemManager.Put(closeAtUartItem);
            }


            ////打开BT
            //OpenPhoneProperties configOpenPhone = new OpenPhoneProperties();
            //configOpenPhone.PortName = "COM17";
            //configOpenPhone.BaudRate = 2400;
            //configOpenPhone.Dtr = true;
            //configOpenPhone.Rts = true;
            //configOpenPhone.EndLine = "\\r\\n";
            //configOpenPhone.Timeout = 10 * 1000;
            //configOpenPhone.RetryCount = 0;
            //configOpenPhone.AtType = AtType.Manual;
            //configOpenPhone.SleepTimeAfterFindDut = 1000;
            //TaskItem openPhoneItem = new TaskItem();
            //openPhoneItem.Enable = true;
            //openPhoneItem.Item = "打开BT串口";//Open BT Uart
            //openPhoneItem.CommonProperties = configOpenPhone;
            //openPhoneItem.Executer = new OpenPhoneExecutor();
            //taskItemManager.Put(openPhoneItem);

            ////读BT MAC，若读到则放到 MAC_BT中即可，后续fetch sn将自动+1，而不是取。若未读到，则info一下即可
            //if (true)
            //{

            //    TaskItem isitWrittenMac = new TaskItem();
            //    isitWrittenMac.Enable = true;
            //    isitWrittenMac.Item = "BT MAC预读";//Check BT MAC
            //    isitWrittenMac.CommonProperties = new IsitWrittenToTheBtMacProperties()
            //    {
            //        ResultType = AILinkFactoryAuto.Task.SmartBracelet.Property.IsitWrittenToTheBtMacProperties.GetBtMacRetType.ThrowExcption,
            //        ImesPreCheck = true,

            //        Timeout = 3 * 1000,
            //        RetryCount = 0,

            //        AtCommand = "AT+GETBLEMAC",
            //        AtCommandError = "ERROR",

            //        GlobalVariblesKeyPattern = "BLE MAC:([0-9A-F]{12})",
            //        GlobalVariblesKey = "{MAC_BT}",

            //        AtCommandInterval = 1000,
            //        SleepTimeBefore = 1500,
            //    };

            //    isitWrittenMac.Executer = new IsitWrittenToTheBtMacExecuter();
            //    taskItemManager.Put(isitWrittenMac);
            //}


            ////写BT 固件
            //TaskItem writeBtBin = new TaskItem();
            //writeBtBin.Enable = true;
            //writeBtBin.Item = "写BT固件";//Write BT Bin
            //writeBtBin.CommonProperties = new WriteBtBinProperties()
            //{
            //    EraseCmd = "nrfjprog.exe --eraseall -f NRF52",
            //    ProgramCmd = "nrfjprog.exe --program {0} --verify -f NRF52",
            //    BinFileFullPath = " ./bin/whole.hex",
            //    ResetCmd = "nrfjprog.exe --reset -f NRF52",
            //    CmdCommandInterval = 500,

            //    Timeout = 10 * 1000,
            //    RetryCount = 0,
            //    SleepTimeBefore = 100,
            //};
            //writeBtBin.Executer = new WriteBtBinExecuter();
            //taskItemManager.Put(writeBtBin);

            ////BT版本号检查
            //if (true)
            //{
            //    TaskItem checkBtVersionItem = new TaskItem();
            //    checkBtVersionItem.Enable = true;
            //    checkBtVersionItem.Item = "BLE固件版本号检查";//Write BT MAC
            //    AtCommandProperties checkBtVersionProperties = new AtCommandProperties();
            //    checkBtVersionProperties.AtCommand = "AT+GETINFO";

            //    checkBtVersionProperties.AtCommandOk = "GET INFO OK";
            //    checkBtVersionProperties.AtCommandError = "ERROR";
            //    checkBtVersionProperties.CheckInfo = new string[] { "AI_WRIST_V1.15", "Apr  9 2020 18:58:47" };

            //    checkBtVersionProperties.AtCommandInterval = 1000;
            //    checkBtVersionProperties.SleepTimeBefore = 800;

            //    checkBtVersionItem.CommonProperties = checkBtVersionProperties;
            //    checkBtVersionItem.Executer = new AtCommandExecuter();
            //    taskItemManager.Put(checkBtVersionItem);
            //}

            //////读BT MAC，判断是否要取号
            ////if (true)
            ////{
            ////    //读BT MAC
            ////    TaskItem getBTMacItem = new TaskItem();
            ////    getBTMacItem.Enable = true;
            ////    getBTMacItem.Item = "读BT MAC";//GET BT MAC
            ////    AtCommandProperties configGetBtMac = new AtCommandProperties();
            ////    configGetBtMac.AtCommand = "AT+GETBLEMAC";

            ////    configGetBtMac.AtCommandOk = "OK";
            ////    configGetBtMac.AtCommandError = "ERROR";

            ////    configGetBtMac.GlobalVariblesKeyPattern = "BLE MAC:([0-9A-F]{12})";//KBLE MAC:112233445566OK
            ////    configGetBtMac.GlobalVariblesKey = "{MoudleBtMac}";

            ////    configGetBtMac.AtCommandInterval = 800;
            ////    configGetBtMac.SleepTimeBefore = 800;
            ////    getBTMacItem.CommonProperties = configGetBtMac;
            ////    getBTMacItem.Executer = new AtCommandExecuter();
            ////    taskItemManager.Put(getBTMacItem);
            ////}

            ////判断是否要取号,不取则+1赋值,并通知不从imes取号
            ////if (true)
            ////{
            ////    TaskItem isFetchMac = new TaskItem();
            ////    isFetchMac.Enable = true;
            ////    isFetchMac.Item = "取MAC号方式自动判断";//Check BT MAC
            ////    isFetchMac.CommonProperties = new AccessToMacProperties()
            ////    {
            ////        Timeout = 3 * 1000,
            ////        RetryCount = 0,

            ////        AtCommand = "AT+GETBLEMAC",
            ////        AtCommandError = "ERROR",

            ////        GlobalVariblesKeyPattern = "BLE MAC:([0-9A-F]{12})",
            ////        GlobalVariblesKey = "{MoudleBtMac}",

            ////        AtCommandInterval = 800,
            ////        SleepTimeBefore = 800,
            ////    };

            ////    isFetchMac.Executer = new AccessToMacExecuter();
            ////    taskItemManager.Put(isFetchMac);
            ////}

            ////从IMES取wifi MAC，bt mac会自动放入全局变量
            //if (true)
            //{
            //    MesFetchSnProperties mesFetchSnProperties = new MesFetchSnProperties();
            //    mesFetchSnProperties.RetryCount = 0;
            //    mesFetchSnProperties.IsNeedInc1 = true;
            //    taskItemManager.AppendMesFetchSn(mesFetchSnProperties);

            //}


            ////写BT MAC
            //TaskItem writeBTMacItem = new TaskItem();
            //writeBTMacItem.Enable = true;
            //writeBTMacItem.Item = "写BT MAC";//Write BT MAC
            //AtCommandProperties configWriteBtMac = new AtCommandProperties();
            //configWriteBtMac.AtCommand = "AT+SETBLEMAC={MAC_BT}";

            //configWriteBtMac.AtCommandOk = "reboot system";
            //configWriteBtMac.AtCommandError = "ERROR";
            //configWriteBtMac.AtCommandInterval = 800;
            //configWriteBtMac.SleepTimeBefore = 3000;
            //writeBTMacItem.CommonProperties = configWriteBtMac;
            //writeBTMacItem.Executer = new AtCommandExecuter();
            //taskItemManager.Put(writeBTMacItem);

            ////读BT MAC
            //if (true)
            //{
            //    TaskItem getBTMacItem = new TaskItem();
            //    getBTMacItem.Enable = true;
            //    getBTMacItem.Item = "读BT MAC";//GET BT MAC
            //    AtCommandProperties configGetBtMac = new AtCommandProperties();
            //    configGetBtMac.AtCommand = "AT+GETBLEMAC";

            //    configGetBtMac.AtCommandOk = "OK";
            //    configGetBtMac.AtCommandError = "ERROR";

            //    configGetBtMac.GlobalVariblesKeyPattern = "BLE MAC:([0-9A-F]{12})";//KBLE MAC:112233445566OK
            //    configGetBtMac.GlobalVariblesKey = "{MoudleBtMac}";

            //    configGetBtMac.AtCommandInterval = 800;
            //    getBTMacItem.CommonProperties = configGetBtMac;
            //    getBTMacItem.Executer = new AtCommandExecuter();
            //    taskItemManager.Put(getBTMacItem);
            //}
            ////生成BT MAC二维码
            //if (true)
            //{
            //    TaskItem showBtMacQrCodeItem = new TaskItem();
            //    showBtMacQrCodeItem.Enable = true;
            //    showBtMacQrCodeItem.Item = "展示BT MAC二维码";//Check BT RSSI
            //    showBtMacQrCodeItem.CommonProperties = new ShowBtMacQrCodeProperties()
            //    {
            //        RetryCount = 3,
            //        StrQrcode = "http://www.baidu.com",
            //        ShowTime = 8000,
            //    };
            //    showBtMacQrCodeItem.Executer = new ShowBtMacQrCodeExecuter();
            //    taskItemManager.Put(showBtMacQrCodeItem);
            //}

            ////校验BT MAC
            ////************待补充：来自IMES的BtMac应存在全局变量中*************
            //TaskItem checkBtMac = new TaskItem();
            //checkBtMac.Enable = true;
            //checkBtMac.Item = "校验BT MAC";//Check BT MAC
            //checkBtMac.CommonProperties = new CheckBtMacProperties()
            //{
            //    Timeout = 3 * 1000,
            //    RetryCount = 0,
            //};
            //checkBtMac.Executer = new CheckBtMacExecuter();
            //taskItemManager.Put(checkBtMac);

            ////设置BT FACMODE
            //TaskItem setBtFacModeItem = new TaskItem();
            //setBtFacModeItem.Enable = true;
            //setBtFacModeItem.Item = "设置BT 进入工厂模式";//Set Bt FactoryMode
            //AtCommandProperties configSetBtFacMode = new AtCommandProperties();
            //configSetBtFacMode.AtCommand = "AT+SETFACMODE=1";

            //configSetBtFacMode.AtCommandOk = "OK";
            //configSetBtFacMode.AtCommandError = "ERROR";
            //configSetBtFacMode.AtCommandInterval = 800;
            //configSetBtFacMode.SleepTimeAfter = 7000;
            //setBtFacModeItem.CommonProperties = configSetBtFacMode;
            //setBtFacModeItem.Executer = new AtCommandExecuter();
            //taskItemManager.Put(setBtFacModeItem);
            ////BT串口发WIFI 软断电电指令
            //TaskItem wifiPowerOff = new TaskItem();
            //wifiPowerOff.Enable = true;
            //wifiPowerOff.Item = "Wifi 断电";//Wifi PowerOff
            //AtCommandProperties configWifiPowerOff = new AtCommandProperties();
            //configWifiPowerOff.AtCommand = "AT+SETGPSWIFIPWR=0";

            //configWifiPowerOff.AtCommandOk = "OK";
            //configWifiPowerOff.AtCommandError = "ERROR";
            //configWifiPowerOff.CheckInfo = new string[] { "wifi power off" };
            //configWifiPowerOff.AtCommandInterval = 3000;
            ////configWifiPowerOff.SleepTimeBefore = 6000;
            //configWifiPowerOff.Timeout = 12000;
            //configWifiPowerOff.RetryCount = 3;
            //wifiPowerOff.CommonProperties = configWifiPowerOff;
            //wifiPowerOff.Executer = new AtCommandExecuter();
            //taskItemManager.Put(wifiPowerOff);

            ////BT串口发WIFI 软上电指令
            //TaskItem wifiPowerOn = new TaskItem();
            //wifiPowerOn.Enable = true;
            //wifiPowerOn.Item = "Wifi 上电";//Wifi PowerOn
            //AtCommandProperties configWifiPowerOn = new AtCommandProperties();
            //configWifiPowerOn.AtCommand = "AT+SETGPSWIFIPWR=1";

            //configWifiPowerOn.AtCommandOk = "OK";
            //configWifiPowerOn.AtCommandError = "ERROR";
            //configWifiPowerOn.CheckInfo = new string[] { "wifi power on" };
            //configWifiPowerOn.AtCommandInterval = 2000;
            //configWifiPowerOn.Timeout = 3000;
            //configWifiPowerOff.RetryCount = 3;
            //wifiPowerOn.CommonProperties = configWifiPowerOn;
            //wifiPowerOn.Executer = new AtCommandExecuter();
            //taskItemManager.Put(wifiPowerOn);

            ////BT发检查BT广播命令
            //TaskItem btBroadcastItem = new TaskItem();
            //btBroadcastItem.Enable = true;
            //btBroadcastItem.Item = "BT 开始广播";//BT Broadcast
            //AtCommandProperties configBtBroadcast = new AtCommandProperties();
            //configBtBroadcast.AtCommand = "AT+OPENBLEADV";

            //configBtBroadcast.AtCommandOk = "OK";
            //configBtBroadcast.AtCommandError = "ERROR";
            //configBtBroadcast.AtCommandInterval = 600;
            //configBtBroadcast.Timeout = 2000;
            //configBtBroadcast.RetryCount = 3;
            //btBroadcastItem.CommonProperties = configBtBroadcast;
            //btBroadcastItem.Executer = new AtCommandExecuter();
            //taskItemManager.Put(btBroadcastItem);

            ////关闭BT UART
            //TaskItem closePhoneItem = new TaskItem();
            //closePhoneItem.Enable = true;
            //closePhoneItem.Item = "关闭BT 串口";//Close BT Uart
            //closePhoneItem.Executer = new ClosePhoneExecutor();
            //closePhoneItem.CommonProperties = new CommonProperties()
            //{
            //    ExecuteCondition = ExecuteCondition.ALWAYS,
            //    RetryCount = 0,
            //};
            //taskItemManager.Put(closePhoneItem);

            //#region BT扫描工具
            ////打开蓝牙扫描工具 UART
            //OpenPhoneProperties configOpentToolUart = new OpenPhoneProperties();
            //configOpentToolUart.PortName = "COM21";
            //configOpentToolUart.BaudRate = 115200;
            //configOpentToolUart.Dtr = true;
            //configOpentToolUart.Rts = true;
            //configOpentToolUart.EndLine = "\\r\\n";
            //configOpentToolUart.Timeout = 10 * 1000;
            //configOpentToolUart.RetryCount = 0;
            //configOpentToolUart.AtType = AtType.Manual;
            //configOpentToolUart.SleepTimeAfterFindDut = 1000;
            //TaskItem configOpentToolUartTtem = new TaskItem();
            //configOpentToolUartTtem.Enable = true;
            //configOpentToolUartTtem.Item = "打开蓝牙扫描工具的串口";//Open Scan Tool Uart
            //configOpentToolUartTtem.CommonProperties = configOpentToolUart;
            //configOpentToolUartTtem.Executer = new OpenPhoneExecutor();
            //taskItemManager.Put(configOpentToolUartTtem);

            ////发送开始扫描命令并获取BT RSSI
            //TaskItem getBtRssiItem = new TaskItem();
            //getBtRssiItem.Enable = true;
            //getBtRssiItem.Item = "开始扫描BT广播包并获取BT RSSI";//Satrt Scan And Get BT RSSI
            //GetBtRssiAtCommandProperties configGetBtRssiPro = new GetBtRssiAtCommandProperties();//GetBtRssiAtCommandProperties
            //configGetBtRssiPro.AtCommand = "AT+INQ";

            //configGetBtRssiPro.AtCommandOk = "OK";
            //configGetBtRssiPro.AtCommandError = "ERROR";
            //configGetBtRssiPro.CheckInfo = new string[] { "INQS" };
            ////********************************************正则表达式中存在变量的情况：该产品的蓝牙MAC后的值*****************************************
            //configGetBtRssiPro.GlobalVariblesKeyPattern = ":RSSI:([0-9-]{3,4})[:]";//"BLE MAC->([0-9A-F:]{17})";
            //configGetBtRssiPro.GlobalVariblesKey = "{MoudleBtRssi}";

            //configGetBtRssiPro.AtCommandInterval = 7000;
            //configGetBtRssiPro.Timeout = 7000;
            //configGetBtRssiPro.RetryCount = 2;
            //getBtRssiItem.CommonProperties = configGetBtRssiPro;
            //getBtRssiItem.Executer = new GetBtRssiAtCommandExecuter();//GetBtRssiAtCommandExecuter
            //taskItemManager.Put(getBtRssiItem);

            ////检查BT RSSI
            //if (true)
            //{
            //    TaskItem checkBtRssiItem = new TaskItem();
            //    checkBtRssiItem.Enable = true;
            //    checkBtRssiItem.Item = "检查BT RSSI";//Check BT RSSI
            //    checkBtRssiItem.CommonProperties = new CheckBtRssiProperties()
            //    {
            //        MaxValue = -10,
            //        MinValue = -90,
            //        Timeout = 2 * 1000,
            //        RetryCount = 0,
            //    };
            //    checkBtRssiItem.Executer = new CheckBtRssiExecuter();
            //    taskItemManager.Put(checkBtRssiItem);
            //}

            ////停止扫描
            //TaskItem stopScanBtRssiItem = new TaskItem();
            //stopScanBtRssiItem.Enable = true;
            //stopScanBtRssiItem.Item = "停止扫描";//Stop Scan BT RSSI
            //AtCommandProperties configStopScanBtRssiPro = new AtCommandProperties();
            //configStopScanBtRssiPro.AtCommand = "AT+SINQ";

            //configStopScanBtRssiPro.AtCommandOk = "INQE";
            //configStopScanBtRssiPro.AtCommandError = "ERROR";
            //configStopScanBtRssiPro.ExecuteCondition = ExecuteCondition.ALWAYS;

            //configStopScanBtRssiPro.AtCommandInterval = 800;
            //configStopScanBtRssiPro.Timeout = 2000;
            //configStopScanBtRssiPro.RetryCount = 0;
            //stopScanBtRssiItem.CommonProperties = configStopScanBtRssiPro;
            //stopScanBtRssiItem.Executer = new AtCommandExecuter();
            //taskItemManager.Put(stopScanBtRssiItem);

            ////关闭蓝牙扫描工具 UART
            //TaskItem closeScanToolItem = new TaskItem();
            //closeScanToolItem.Enable = true;
            //closeScanToolItem.Item = "关闭BT扫描工具串口";//Close Scan Tool Uart
            //closeScanToolItem.Executer = new ClosePhoneExecutor();
            //closeScanToolItem.CommonProperties = new CommonProperties()
            //{
            //    ExecuteCondition = ExecuteCondition.ALWAYS,
            //    RetryCount = 0,
            //};
            //taskItemManager.Put(closeScanToolItem);
            //#endregion

            ////打开WIFI UART
            ////OpenPhoneProperties configOpenWifiUart = new OpenPhoneProperties();
            ////configOpenWifiUart.PortName = "COM18";
            ////configOpenWifiUart.BaudRate = 115200;
            ////configOpenWifiUart.Dtr = true;
            ////configOpenWifiUart.Rts = true;
            ////configOpenWifiUart.EndLine = "\\r\\n";
            ////configOpenWifiUart.Timeout = 10 * 1000;
            ////configOpenWifiUart.RetryCount = 0;
            ////configOpenWifiUart.AtType = AtType.Manual;
            ////configOpenWifiUart.SleepTimeAfterFindDut = 1000;
            ////TaskItem openWifiUartItem = new TaskItem();
            ////openWifiUartItem.Enable = true;
            ////openWifiUartItem.Item = "Open Wifi Uart";
            ////openWifiUartItem.CommonProperties = configOpenWifiUart;
            ////openWifiUartItem.Executer = new OpenPhoneExecutor();
            //taskItemManager.Put(openWifiUartItem);


            ////读取WIFI MAP文件至内存********并更新MAC至内存*************
            ////if (true)
            ////{
            ////    TaskItem readMapItem = new TaskItem();
            ////    readMapItem.Enable = true;
            ////    readMapItem.Item = "读取WIFI MAP文件并更新MAC至内存";//Read MAP From File
            ////    readMapItem.CommonProperties = new ReadMapProperties()
            ////    {
            ////        MapFilePath = "./Map/8710BX.map",
            ////        Timeout = 2 * 1000,
            ////        RetryCount = 0,
            ////    };
            ////    readMapItem.Executer = new ReadMapExecuter();
            ////    taskItemManager.Put(readMapItem);
            ////}


            ////写WIFI MAP  使用comdut自定义来写
            //TaskItem writeWifiMapItem = new TaskItem();
            //writeWifiMapItem.Enable = true;
            //writeWifiMapItem.Item = "写WIFI MAP";//
            //writeWifiMapItem.CommonProperties = new WriteWifiMapProperties()
            //{
            //    //取MAC文件一行一行的写（16个字节）
            //    WriteMapAtCommand = "iwpriv config_set wmap,{0},{1}",
            //    Address = "0x00",
            //    AtCommandInterval = 50,
            //    AtCommandOk = "[MEM] After do cmd",
            //    AtCommandError = "unknown command",

            //    Timeout = 20 * 1000,//5
            //    RetryCount = 0,
            //    RetryInterval = 50,
            //};
            //writeWifiMapItem.Executer = new WriteWifiMapExecuter();
            //taskItemManager.Put(writeWifiMapItem);




            ////写WIFI MAC
            ////TaskItem writeWifiMacItem = new TaskItem();
            ////writeWifiMacItem.Enable = true;
            ////writeWifiMacItem.Item = "Write WIFI MAC";
            ////AtCommandProperties configWriteWifiMac = new AtCommandProperties();
            ////configWriteWifiMac.AtCommand = "iwpriv config_set wmap,0x0a,{MAC}";

            ////configWriteWifiMac.AtCommandOk = "Private Message: 0x";
            ////configWriteWifiMac.AtCommandError = "unknown command";
            ////configWriteWifiMac.AtCommandInterval = 800;
            ////writeWifiMacItem.CommonProperties = configWriteWifiMac;
            ////writeWifiMacItem.Executer = new AtCommandExecuter();
            ////taskItemManager.Put(writeWifiMacItem);

            //////读并校验WIFI MAC


            ////读并WIFI MAP
            //TaskItem getWifiMoudleMapItem = new TaskItem();
            //getWifiMoudleMapItem.Enable = true;
            //getWifiMoudleMapItem.Item = "读产品内部WIFI Map";//Get Wifi Moudle Map
            //AtCommandProperties configGetWifiMoudleMap = new AtCommandProperties();
            //configGetWifiMoudleMap.AtCommand = "iwpriv config_get rmap,00,512";

            //configGetWifiMoudleMap.AtCommandOk = "Private Message: 0x";
            //configGetWifiMoudleMap.AtCommandError = "unknown command";

            //configGetWifiMoudleMap.GlobalVariblesKeyPattern = "Private Message: ([0-9A-Fx ]{2560})";//"BLE MAC->([0-9A-F:]{17})";
            //configGetWifiMoudleMap.GlobalVariblesKey = "{MoudleWifiMap}";

            //configGetWifiMoudleMap.AtCommandInterval = 800;
            //configGetWifiMoudleMap.RetryCount = 5;
            //getWifiMoudleMapItem.CommonProperties = configGetWifiMoudleMap;
            //getWifiMoudleMapItem.Executer = new AtCommandExecuter();
            //taskItemManager.Put(getWifiMoudleMapItem);


            ////校验WIFI MAP
            //TaskItem checkWifiMapItem = new TaskItem();
            //checkWifiMapItem.Enable = true;
            //checkWifiMapItem.Item = "校验WIFI Map";//Check Wifi Map
            //checkWifiMapItem.CommonProperties = new CheckWifiMapProperties()
            //{
            //    Timeout = 3 * 1000,
            //    RetryCount = 0,
            //};
            //checkWifiMapItem.Executer = new CheckWifiMapExecuter();
            //taskItemManager.Put(checkWifiMapItem);



            ////WIFI退出MP模式，进入用户模式
            //TaskItem wifiIntoUserMode = new TaskItem();
            //wifiIntoUserMode.Enable = true;
            //wifiIntoUserMode.Item = "WIFI进入用户模式";//Wifi Into User Mode
            //AtCommandProperties configWifiIntoUserProperties = new AtCommandProperties();
            //configWifiIntoUserProperties.AtCommand = "ATSC";
            ////****************************进入用户模式是否回复OK？***********************
            //configWifiIntoUserProperties.AtCommandOk = "Enter Image 2 ";
            //configWifiIntoUserProperties.AtCommandError = "ERROR";
            ////configWifiIntoUserProperties.CheckInfo = new string[] { "[ATSC]:", "AT_SYSTEM_CLEAR_OTA_SIGNATURE" };
            //configWifiIntoUserProperties.AtCommandInterval = 4000;
            //wifiIntoUserMode.CommonProperties = configWifiIntoUserProperties;
            //wifiIntoUserMode.Executer = new AtCommandExecuter();
            //taskItemManager.Put(wifiIntoUserMode);


            ////检查WIFI版本号 关键字
            //TaskItem getWifiVersionItem = new TaskItem();
            //getWifiVersionItem.Enable = true;
            //getWifiVersionItem.Item = "检查WIFI版本号";//Get Wifi Version
            //AtCommandProperties configGetWifiVersionProperties = new AtCommandProperties();
            //configGetWifiVersionProperties.AtCommand = "ATS?";
            //configGetWifiVersionProperties.AtCommandOk = "[ATS?]: SW VERSION:";
            //configGetWifiVersionProperties.AtCommandError = "unknown command";
            //configGetWifiVersionProperties.CheckInfo = new string[] { "SW VERSION: v.3.4" };
            ////configGetWifiVersionProperties.GlobalVariblesKeyPattern = "SW VERSION: ([v0-9.]{5})";//"BLE MAC->([0-9A-F:]{17})";
            ////configGetWifiVersionProperties.GlobalVariblesKey = "{MoudleWifiVersion}";


            //configGetWifiVersionProperties.AtCommandInterval = 800;
            //configGetWifiVersionProperties.RetryCount = 3;
            //getWifiVersionItem.CommonProperties = configGetWifiVersionProperties;
            //getWifiVersionItem.Executer = new AtCommandExecuter();
            //taskItemManager.Put(getWifiVersionItem);




            ////关闭WIFI UART
            ////TaskItem closeWifiUartItem = new TaskItem();
            ////closeWifiUartItem.Enable = true;
            ////closeWifiUartItem.Item = "Close WIFI Uart";
            ////closeWifiUartItem.Executer = new ClosePhoneExecutor();
            ////closeWifiUartItem.CommonProperties = new CommonProperties()
            ////{
            ////    ExecuteCondition = ExecuteCondition.ALWAYS,
            ////    RetryCount = 0,
            ////};
            //taskItemManager.Put(closeWifiUartItem);




            ////打开BT UART->版本号检查->信号强度检查->心率检查
            ////OpenPhoneProperties configOpenBtUart = new OpenPhoneProperties();
            ////configOpenBtUart.PortName = "COM17";
            ////configOpenBtUart.BaudRate = 2400;
            ////configOpenBtUart.Dtr = true;
            ////configOpenBtUart.Rts = true;
            ////configOpenBtUart.EndLine = "\\r\\n";
            ////configOpenBtUart.Timeout = 10 * 1000;
            ////configOpenBtUart.RetryCount = 0;
            ////configOpenBtUart.AtType = AtType.Manual;
            ////configOpenBtUart.SleepTimeAfterFindDut = 1000;
            ////TaskItem openBtUartItem = new TaskItem();
            ////openBtUartItem.Enable = true;
            ////openBtUartItem.Item = "Open BT Uart";
            ////openBtUartItem.CommonProperties = configOpenBtUart;
            ////openBtUartItem.Executer = new OpenPhoneExecutor();
            ////taskItemManager.Put(openBtUartItem);
            //taskItemManager.Put(openPhoneItem);

            ////NB 软件版本号检查 AT+CGMR 关键字
            //TaskItem nbSoftVersionCheck = new TaskItem();
            //nbSoftVersionCheck.Enable = true;
            //nbSoftVersionCheck.Item = "检查NB软件版本号";//NB Soft Version Check
            //AtCommandProperties configNbSoftVersionCheck = new AtCommandProperties();
            //configNbSoftVersionCheck.AtCommand = "AT+CGMR";

            //configNbSoftVersionCheck.AtCommandOk = "OK";
            //configNbSoftVersionCheck.AtCommandError = "ERROR";
            //configNbSoftVersionCheck.CheckInfo = new string[] { "SOTFWAREVER:AI_LITEOS_NB15_B300SP5_V1.9_20200229" };
            //configNbSoftVersionCheck.AtCommandInterval = 1500;
            //configNbSoftVersionCheck.SleepTimeBefore = 500;
            //nbSoftVersionCheck.CommonProperties = configNbSoftVersionCheck;
            //nbSoftVersionCheck.Executer = new AtCommandExecuter();
            //taskItemManager.Put(nbSoftVersionCheck);

            //# region NB IMEI号等
            ////在IMEI等号之前先发AT+CFUN=1
            //TaskItem nbAtCfun = new TaskItem();
            //nbAtCfun.Enable = true;
            //nbAtCfun.Item = "设置BT CFUN为1";//AT CFUN
            //AtCommandProperties confignbAtCfun = new AtCommandProperties();
            //confignbAtCfun.AtCommand = "AT+CFUN=1";

            //confignbAtCfun.AtCommandOk = "OK";
            //confignbAtCfun.AtCommandError = "ERROR";
            //confignbAtCfun.AtCommandInterval = 4000;
            //confignbAtCfun.RetryCount = 3;
            //confignbAtCfun.SleepTimeBefore = 200;
            //nbAtCfun.CommonProperties = confignbAtCfun;
            //nbAtCfun.Executer = new AtCommandExecuter();
            //taskItemManager.Put(nbAtCfun);

            ////获取IMEI
            //TaskItem getNbImeiItem = new TaskItem();
            //getNbImeiItem.Enable = true;
            //getNbImeiItem.Item = "获取NB IMEI";//Get NB IMEI
            //AtCommandProperties configGetNbImeiItem = new AtCommandProperties();
            //configGetNbImeiItem.AtCommand = "AT+CGSN=1";
            //configGetNbImeiItem.AtCommandOk = "OK";
            //configGetNbImeiItem.AtCommandError = "ERROR";

            //configGetNbImeiItem.GlobalVariblesKeyPattern = "CGSN:([0-9]{15})";//"Private Message: ([0-9A-Fx ]{2560})";//"BLE MAC->([0-9A-F:]{17})";
            //configGetNbImeiItem.GlobalVariblesKey = "{NBIMEI}";

            //configGetNbImeiItem.AtCommandInterval = 800;
            //configGetNbImeiItem.RetryCount = 3;
            //getNbImeiItem.CommonProperties = configGetNbImeiItem;
            //getNbImeiItem.Executer = new AtCommandExecuter();
            //taskItemManager.Put(getNbImeiItem);

            ////检查IMEI标签与内部一致性
            //TaskItem checkNbImei = new TaskItem();
            //checkNbImei.Enable = true;
            //checkNbImei.Item = "检查NB IMEI内部与标签一致性";//Check NB IMEI
            //checkNbImei.CommonProperties = new CheckNbImeiProperties()
            //{
            //    Timeout = 2 * 1000,
            //    RetryCount = 0,
            //};
            //checkNbImei.Executer = new CheckNbImeiExecuter();
            //taskItemManager.Put(checkNbImei);


            ////打印标签的IMEI 设置为NB IMEI
            //TaskItem printLabelImeiChangetToNbImeiItem = new TaskItem();
            //printLabelImeiChangetToNbImeiItem.Enable = true;
            //printLabelImeiChangetToNbImeiItem.Item = "打印标签IMEI设置为NB IMEI";//LOG MAC CHANGE TO BT MAC
            //printLabelImeiChangetToNbImeiItem.CommonProperties = new PrintLabelImeiChangetToNbImeiProperties()
            //{
            //    RetryCount = 0,
            //};
            //printLabelImeiChangetToNbImeiItem.Executer = new PrintLabelImeiChangetToNbImeiExecuter();
            //taskItemManager.Put(printLabelImeiChangetToNbImeiItem);

            ////
            //TaskItem getNbImsiItem = new TaskItem();
            //getNbImsiItem.Enable = true;
            //getNbImsiItem.Item = "获取NB IMSI";//Get NB IMSI
            //AtCommandProperties configGetNbImsiItem = new AtCommandProperties();
            //configGetNbImsiItem.AtCommand = "AT+CIMI";
            //configGetNbImsiItem.AtCommandOk = "OK";
            //configGetNbImsiItem.AtCommandError = "ERROR";

            //configGetNbImsiItem.GlobalVariblesKeyPattern = "\r\n([0-9]{15})";
            //configGetNbImsiItem.GlobalVariblesKey = "{IMSI}";

            //configGetNbImsiItem.AtCommandInterval = 800;
            //configGetNbImsiItem.RetryCount = 3;
            //getNbImsiItem.CommonProperties = configGetNbImsiItem;
            //getNbImsiItem.Executer = new AtCommandExecuter();
            //taskItemManager.Put(getNbImsiItem);

            ////获取ICCID
            //TaskItem getNbIccidItem = new TaskItem();
            //getNbIccidItem.Enable = true;
            //getNbIccidItem.Item = "获取NB ICCID";//Get NB ICCID
            //AtCommandProperties configGetNbIccidItem = new AtCommandProperties();
            //configGetNbIccidItem.AtCommand = "AT+NCCID";

            //configGetNbIccidItem.AtCommandOk = "OK";
            //configGetNbIccidItem.AtCommandError = "ERROR";

            //configGetNbIccidItem.GlobalVariblesKeyPattern = "NCCID:([0-9]{20})";
            //configGetNbIccidItem.GlobalVariblesKey = "{ICCID}";

            //configGetNbIccidItem.AtCommandInterval = 800;
            //configGetNbIccidItem.RetryCount = 3;
            //getNbIccidItem.CommonProperties = configGetNbIccidItem;
            //getNbIccidItem.Executer = new AtCommandExecuter();
            //taskItemManager.Put(getNbIccidItem);
            //#endregion



            //#region BT/NB 功能检查
            ////附网，AT+CGATT=1
            ////*********************返回什么为PASS????  : OK************************
            //TaskItem nbClingNetItem = new TaskItem();
            //nbClingNetItem.Enable = true;
            //nbClingNetItem.Item = "NB 附网";//NB Cling Net
            //AtCommandProperties configNbClingNetProperties = new AtCommandProperties();
            //configNbClingNetProperties.AtCommand = "AT+CGATT=1";

            //configNbClingNetProperties.AtCommandOk = "OK";
            //configNbClingNetProperties.AtCommandError = "ERROR";
            ////configNbNetCheckProperties.CheckInfo = new string[] { "...1" };
            //configNbClingNetProperties.AtCommandInterval = 2000;
            //configNbClingNetProperties.SleepTimeBefore = 100;
            //nbClingNetItem.CommonProperties = configNbClingNetProperties;
            //nbClingNetItem.Executer = new AtCommandExecuter();
            //taskItemManager.Put(nbClingNetItem);

            ////附网检查 AT+CGATT?  检查是否为1
            ////*********************返回什么为PASS????  :1 ************************
            //TaskItem nbNetCheckItem = new TaskItem();
            //nbNetCheckItem.Enable = true;
            //nbNetCheckItem.Item = "NB附网检查";//B Net CheckN
            //AtCommandProperties configNbNetCheckProperties = new AtCommandProperties();
            //configNbNetCheckProperties.AtCommand = "AT+CGATT?";

            //configNbNetCheckProperties.AtCommandOk = "OK";
            //configNbNetCheckProperties.AtCommandError = "ERROR";
            //configNbNetCheckProperties.CheckInfo = new string[] { "CGATT:1" };
            //configNbNetCheckProperties.AtCommandInterval = 5000;
            //configNbNetCheckProperties.SleepTimeBefore = 100;
            //nbNetCheckItem.CommonProperties = configNbNetCheckProperties;
            //nbNetCheckItem.Executer = new AtCommandExecuter();
            //taskItemManager.Put(nbNetCheckItem);



            ////信号强度CSQ获取
            //TaskItem getCsqItem = new TaskItem();
            //getCsqItem.Enable = true;
            //getCsqItem.Item = "获取NB信号强度";//Get NB CSQ
            //AtCommandProperties configGetCsqProperties = new AtCommandProperties();
            //configGetCsqProperties.AtCommand = "AT+CSQ";
            //configGetCsqProperties.AtCommandOk = "OK";
            //configGetCsqProperties.AtCommandError = "ERROR";

            //configGetCsqProperties.GlobalVariblesKeyPattern = "CSQ:([0-9]{1,2})[,]";
            //configGetCsqProperties.GlobalVariblesKey = "{NBCSQ}";

            //configGetCsqProperties.AtCommandInterval = 1000;
            //configGetCsqProperties.RetryCount = 3;
            //getCsqItem.CommonProperties = configGetCsqProperties;
            //getCsqItem.Executer = new AtCommandExecuter();
            //taskItemManager.Put(getCsqItem);

            ////检查信号强度
            //TaskItem checkCsqItem = new TaskItem();
            //checkCsqItem.Enable = true;
            //checkCsqItem.Item = "检查NB信号强度";//Check Csq
            //checkCsqItem.CommonProperties = new CheckCsqProperties()
            //{
            //    MaxValue = 100,
            //    MinValue = 1,
            //    Timeout = 2 * 1000,
            //    RetryCount = 0,
            //};
            //checkCsqItem.Executer = new CheckCsqExecuter();
            //taskItemManager.Put(checkCsqItem);


            ////获取WIFI探针值
            //TaskItem getWifiProbeItem = new TaskItem();
            //getWifiProbeItem.Enable = true;
            //getWifiProbeItem.Item = "获取WIFI探针-信号强度值";//Get WIFI PROBE
            //AtCommandProperties configGetWifiProbeProperties = new AtCommandProperties();
            //configGetWifiProbeProperties.AtCommand = "AT+AIGPSWIFI=WIFI,2";
            //configGetWifiProbeProperties.AtCommandOk = "OK";
            //configGetWifiProbeProperties.AtCommandError = "ERROR";

            //configGetWifiProbeProperties.GlobalVariblesKeyPattern = "7405a58fa8cb:([0-9-]{1,3}),";

            ////configCheckWifiProbeProperties.GlobalVariblesKeyPattern = "[0-9,:]]*]";
            //configGetWifiProbeProperties.GlobalVariblesKey = "{NBWIFIPROBE}";

            //configGetWifiProbeProperties.AtCommandInterval = 5000;
            //configGetWifiProbeProperties.RetryCount = 3;
            //getWifiProbeItem.CommonProperties = configGetWifiProbeProperties;
            //getWifiProbeItem.Executer = new AtCommandExecuter();
            //taskItemManager.Put(getWifiProbeItem);

            ////WIFI探针检查
            //TaskItem checkWifiProbeItem = new TaskItem();
            //checkWifiProbeItem.Enable = true;
            //checkWifiProbeItem.Item = "WIFI探针-信号强度检查";
            //checkWifiProbeItem.CommonProperties = new CheckWifiProbeProperties()
            //{
            //    MaxValue = -10,
            //    MinValue = -90,
            //    Timeout = 2 * 1000,
            //    RetryCount = 0,
            //};
            //checkWifiProbeItem.Executer = new CheckWifiProbeExecuter();
            //taskItemManager.Put(checkWifiProbeItem);



            ////计步器检查  
            //TaskItem checkPedometer = new TaskItem();
            //checkPedometer.Enable = true;
            //checkPedometer.Item = "计步器检查";
            //AtCommandProperties configCheckPedometer = new AtCommandProperties();
            //configCheckPedometer.AtCommand = "AT+PEDOTEST";//AT+PEDOTEST

            //configCheckPedometer.AtCommandOk = "OK";
            //configCheckPedometer.AtCommandError = "ERROR";
            //configCheckPedometer.AtCommandInterval = 1000;
            //checkPedometer.CommonProperties = configCheckPedometer;
            //checkPedometer.Executer = new AtCommandExecuter();
            //taskItemManager.Put(checkPedometer);


            ////温度传感器值获取
            //TaskItem getTemp = new TaskItem();
            //getTemp.Enable = true;
            //getTemp.Item = "温度传感器值获取";//Get Temp
            //AtCommandProperties configGetTemp = new AtCommandProperties();
            //configGetTemp.AtCommand = "AT+TEMPTEST";//

            //configGetTemp.AtCommandOk = "OK";
            //configGetTemp.AtCommandError = "ERROR";
            //configGetTemp.GlobalVariblesKeyPattern = "TEMP:([0-9.]*)[\r\n]";//TEMP:1519.7\r\n
            //configGetTemp.GlobalVariblesKey = "{NBTemp}";
            //configGetTemp.AtCommandInterval = 2000;
            //getTemp.CommonProperties = configGetTemp;
            //getTemp.Executer = new AtCommandExecuter();
            //taskItemManager.Put(getTemp);

            ////温度传感器检查
            //TaskItem checkTempItem = new TaskItem();
            //checkTempItem.Enable = true;
            //checkTempItem.Item = "温度传感器检查";//Check Temp
            //checkTempItem.CommonProperties = new CheckTempProperties()
            //{
            //    MaxValue = 5000,
            //    MinValue = 5,
            //    Timeout = 2 * 1000,
            //    RetryCount = 0,
            //};
            //checkTempItem.Executer = new CheckTempExecuter();
            //taskItemManager.Put(checkTempItem);

            ////射频获取
            //TaskItem taskItem10 = new TaskItem();
            //taskItem10.Enable = true;
            //taskItem10.Item = "射频获取";//Get Signal Power
            //AtCommandProperties config10 = new AtCommandProperties();
            //config10.AtCommand = "AT+NUESTATS";
            //config10.AtCommandOk = "OK";
            //config10.AtCommandError = "ERROR";
            //config10.AtCommandInterval = 1000;
            //config10.GlobalVariblesKey = "{SIGNAL_POWER}";
            //config10.GlobalVariblesKeyPattern = "Signal power\\:([0-9\\-]+)\\r\\n";
            //taskItem10.CommonProperties = config10;
            //taskItem10.Executer = new AtCommandExecuter();
            //taskItemManager.Put(taskItem10);

            ////射频对比
            //TaskItem comparePower = new TaskItem();
            //comparePower.Enable = true;
            //comparePower.Item = "射频检查";//Compare Signal Power
            //ConditionProperties configComparePower = new ConditionProperties()
            //{
            //    RetryCount = 0
            //};
            //configComparePower.ConditionItems = new ConditionItem[]{
            //    new ConditionItem()
            //    {
            //        Key = "{SIGNAL_POWER}",
            //        Condition = Condition.GREATER_EQUAL,
            //        Value = "-1000"
            //    }
            //};
            //comparePower.CommonProperties = configComparePower;
            //comparePower.Executer = new ConditionExecuter();
            //taskItemManager.Put(comparePower);
            ////AT+HRMTEST

            ////BT 退出工厂模式
            //TaskItem btOutFactoryModeItem = new TaskItem();
            //btOutFactoryModeItem.Enable = true;
            //btOutFactoryModeItem.Item = "BT 退出工厂模式";//Get Signal Power
            //AtCommandProperties configBtOutFactoryMode = new AtCommandProperties();
            //configBtOutFactoryMode.AtCommand = "AT+SETFACMODE=0";
            //configBtOutFactoryMode.AtCommandOk = "OK";
            //configBtOutFactoryMode.AtCommandError = "ERROR";
            //configBtOutFactoryMode.AtCommandInterval = 1000;
            //btOutFactoryModeItem.CommonProperties = configBtOutFactoryMode;
            //btOutFactoryModeItem.Executer = new AtCommandExecuter();
            //taskItemManager.Put(btOutFactoryModeItem);


            ////进入出厂状态
            //TaskItem btInDefaultModeItem = new TaskItem();
            //btInDefaultModeItem.Enable = true;
            //btInDefaultModeItem.Item = "进入出厂状态";//Get Signal Power
            //AtCommandProperties configInDefaultMode = new AtCommandProperties();
            //configInDefaultMode.AtCommand = "AT+FACOUT";
            //configInDefaultMode.AtCommandOk = "OK";
            //configInDefaultMode.AtCommandError = "ERROR";
            //configInDefaultMode.AtCommandInterval = 1000;
            //btInDefaultModeItem.CommonProperties = configInDefaultMode;
            //btInDefaultModeItem.Executer = new AtCommandExecuter();
            //taskItemManager.Put(btInDefaultModeItem);

            ////出厂状态检查
            //if (true)
            //{
            //    TaskItem taskItem = new TaskItem();
            //    taskItem.Enable = true;
            //    taskItem.Item = "出厂状态检查";//Write BT MAC
            //    AtCommandProperties atCommandProperties = new AtCommandProperties();
            //    atCommandProperties.AtCommand = "AT+GETINFO";

            //    atCommandProperties.AtCommandOk = "GET INFO OK";
            //    atCommandProperties.AtCommandError = "ERROR";
            //    atCommandProperties.CheckInfo = new string[] { "FACOUT:1" };

            //    atCommandProperties.AtCommandInterval = 1000;

            //    taskItem.CommonProperties = atCommandProperties;
            //    taskItem.Executer = new AtCommandExecuter();
            //    taskItemManager.Put(taskItem);
            //}


            //#endregion


            ////关闭BT UART
            ////TaskItem closeBTUartItem = new TaskItem();
            ////closeBTUartItem.Enable = true;
            ////closeBTUartItem.Item = "Close BT Uart";
            ////closeBTUartItem.Executer = new ClosePhoneExecutor();
            ////closeBTUartItem.CommonProperties = new CommonProperties()
            ////{
            ////    ExecuteCondition = ExecuteCondition.ALWAYS,
            ////    RetryCount = 0,
            ////};
            ////taskItemManager.Put(closeBTUartItem);
            //taskItemManager.Put(closePhoneItem);


            ////LOG MAC CHANGE TO BT MAC
            //TaskItem logMacChangeToBtMacItem = new TaskItem();
            //logMacChangeToBtMacItem.Enable = true;
            //logMacChangeToBtMacItem.Item = "LOG MAC为BT MAC";//LOG MAC CHANGE TO BT MAC
            //logMacChangeToBtMacItem.CommonProperties = new LogMacChangeToBtMacProperties()
            //{
            //    RetryCount = 0,
            //    ExecuteCondition = ExecuteCondition.ALWAYS,
            //};
            //logMacChangeToBtMacItem.Executer = new LogMacChangeToBtMacExecuter();
            //taskItemManager.Put(logMacChangeToBtMacItem);


            ////TaskItem writeImeiItem = new TaskItem();
            ////writeImeiItem.Enable = true;
            ////writeImeiItem.Item = "Write Imei";
            ////AtCommandProperties configWriteBtMac = new AtCommandProperties();
            ////configWriteBtMac.AtCommand = "AT^IMEI={IMEI}";
            ////configWriteBtMac.AtCommandOk = "OK";
            ////configWriteBtMac.AtCommandError = "ERROR";
            ////configWriteBtMac.AtCommandInterval = 100;
            ////writeImeiItem.CommonProperties = configWriteBtMac;
            ////writeImeiItem.Executer = new AtCommandExecuter();
            ////taskItemManager.Put(writeImeiItem);


            //标签打印
            if (true)
            {
                //PrintLabelProperties printLabelProperties = new PrintLabelProperties()
                //{
                //    DataFile = @"F:\DataFile.txt",
                //    DataHead = "MAC",
                //    TempalteFile = @"F:\ZNTK20016.btw",


                //    RetryCount = 0,
                //    Timeout = 8000,
                //    SleepTimeAfter = 5000,
                //};
                //taskItemManager.AppendPrintLabel(printLabelProperties);
            }

            //回扫标签-与MAC比较
            if (true)
            {
                ////再次扫码
                //TaskItem readBinItem = new TaskItem();
                //readBinItem.Enable = true;
                //readBinItem.Item = "回扫标签";//Read MAP From File
                //readBinItem.CommonProperties = new ReScanLabelProperties()
                //{
                //    RetryCount = 0,
                //};
                //readBinItem.Executer = new ReScanLabelExecuter();
                //taskItemManager.Put(readBinItem);
            }
            if (true)
            {
                ////比较标签
                //TaskItem readBinItem = new TaskItem();
                //readBinItem.Enable = true;
                //readBinItem.Item = "标签核验";//Read MAP From File
                //readBinItem.CommonProperties = new LabelVerifyProperties()
                //{
                //    FixedPart= "DML9032425CH",  

                //    RetryCount = 0,
                //};
                //readBinItem.Executer = new LabelVerifyExecuter();
                //taskItemManager.Put(readBinItem);
            }


            //StringBuilder jtsFileName = new StringBuilder();
            //jtsFileName.Append("Hi3861Check");

            string jtsFileName = "WIFI-AP6255"; 
            taskItemManager.Save2Jts(this, 1, jtsFileName);

            //jtsFileName.Append(".jts");
            //JtsUtils.SaveJsonFile(jtsFileName.ToString(), taskItemList);

            Box.ShowInfoOk(jtsFileName + "\r\n成功");


            //OpenPhoneExecutor
            //OpenPhoneProperties

            //    AtCommandExecuter
            //    AtCommandProperties

            //    ClosePhoneExecutor
            }
        }
    }
