using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Dut.AtCommand;
using AILinkFactoryAuto.Task.SmartBracelet.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace AILinkFactoryAuto.Task.Common
{

    partial class TipAndCheckUartForm : Form
    {
        private DateTime start;
        private TimeSpan countDown;

        private bool result = false;

        private FormTipAndUartCheckProperties formTipAndUartCheckProperties;

        private ILog log;

        private ComDut comDut;
        Timer timer;
        private List<byte> buffer = new List<byte>(4096);       //uart 接受数据buffer 未做任何处理

        public bool Result
        {
            get { return result; }
        }

        public TipAndCheckUartForm()
        {
            InitializeComponent();
        }

        public TipAndCheckUartForm(FormTipAndUartCheckProperties formTipAndUartCheckProperties, ILog log,ComDut comDut)
        {
            InitializeComponent();

            this.formTipAndUartCheckProperties = formTipAndUartCheckProperties;
            this.log = log;

            //lblInfoPass.Text = "PASS：请按" + userConfirmProperties.KeyPass + "键";
            //lblInfoFail.Text = "FAIL：请按" + userConfirmProperties.KeyFail + "键";
            txtTips.Text = formTipAndUartCheckProperties.Tips;
            countDown = new TimeSpan(formTipAndUartCheckProperties.CountDownTime * 1000 * 10);

            this.comDut = comDut;

        }

        private void TipAndCheckUartForm_Shown(object sender, EventArgs e)
        {
            txtTips.SelectionLength = 0;

            timer = new Timer();
            timer.Enabled = true;
            timer.Interval = 100;
            timer.Tick += Timer_Tick;

            start = DateTime.Now;
            lblCountDown.Text = "倒计时：" + countDown.Seconds + "秒";
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan remainTime = countDown - (DateTime.Now - start);
            if (remainTime.Ticks <= 0)
            {
                //Timer timer = sender as Timer;
                timer.Stop();
                log.Info(string.Format("未返回正确状态关键字：{0}", this.formTipAndUartCheckProperties.AtCommandOk));

                log.Fail("Wait TimeOut, " + countDown.Seconds + "seconds");  
                //Thread.Sleep(100);

                this.Close();
            }
            else
            {
                lblCountDown.Text = "倒计时：" + remainTime.Seconds + "秒";

                if (formTipAndUartCheckProperties.UartDataType == FormTipAndUartCheckProperties.EnumUartDataType.String)
                {
                    string response = comDut.ReadExisting();
                    log.Info("Response：" + response);

                    if (!string.IsNullOrEmpty(this.formTipAndUartCheckProperties.AtCommandOk))
                    {
                        if (response.Contains(this.formTipAndUartCheckProperties.AtCommandOk))
                        {
                            result = true;
                            timer.Stop();
                            //throw new BaseException(string.Format("not contain OK=[{0}]", this.formTipAndUartCheckProperties.AtCommandOk));
                            log.Pass(string.Format("包含关键字：{0}", this.formTipAndUartCheckProperties.AtCommandOk));
                            //Thread.Sleep(500);

                            this.Close();
                        }
                    }
                }
                else if (formTipAndUartCheckProperties.UartDataType == FormTipAndUartCheckProperties.EnumUartDataType.Hex)
                {
                    int n = comDut.BytesToRead;
                    //if (n>0)
                    //{
                    //    byte[] buf = new byte[n];
                    //    comDut.Read(buf, 0, n);

                    //    UartDisplay(buf, "R");

                    //}
                    log.Info(string.Format("接收的长度:{0}", n));
                    if (n != 0)
                    {
                        byte[] buf = new byte[n];
                        comDut.Read(buf, 0, n);
                        UartDisplay(buf, "R");
                        buffer.Clear();
                        buffer.AddRange(buf);

                    }
                    if (buffer.Count > 9)
                    {
                        if (buffer[0] == 0xA5)
                        {
                            //throw new BaseException(string.Format("Head Error"));
                            //if (&& buf[1] == 0xC3))
                            //{

                            //}
                            if (buffer[1] == 0xC3)
                            {
                                byte high = buffer[2];
                                byte low = buffer[3];
                                int iTotalLength = (high & 0xFF) << 8 | low;

                                if (buffer.Count < iTotalLength) //数据区尚未接收完整
                                {
                                    //继续循环执行
                                    log.Info(string.Format("数据尚未接收完整,已接收:{0},总长度:{1}", buffer.Count, iTotalLength));
                                    return;
                                }
                                byte[] oneFrameBytes = new byte[iTotalLength];
                                buffer.CopyTo(0, oneFrameBytes, 0, iTotalLength);
                                buffer.RemoveRange(0, iTotalLength);

                                UartDisplay(oneFrameBytes, "R");

                                #region //开始校验-命令字
                                if (!(oneFrameBytes[6] == 0xA0 && oneFrameBytes[7] == 0x41))
                                {
                                    log.Info(string.Format("返回的命令字错误"));
                                    //继续循环执行
                                    return;

                                }
                                log.Info("收到DUT状态事件上报");
                                //异或校验 1字节
                                byte[] bufOutOxr = new byte[oneFrameBytes.Length - 1];
                                Array.Copy(oneFrameBytes, 0, bufOutOxr, 0, oneFrameBytes.Length - 1);
                                byte xor = ByteToXOR(bufOutOxr);

                                if (oneFrameBytes[iTotalLength - 1] != xor)
                                {
                                    log.Info(string.Format("核验RX的校验位：错误,返回校验位:{0:X2},计算校验位:{1:X2}", oneFrameBytes[iTotalLength - 1], xor));
                                    //继续循环执行
                                    return;
                                }
                                log.Info(string.Format("核验RX的校验位：正确,返回校验位:{0:X2},计算校验位:{1:X2}", oneFrameBytes[iTotalLength - 1], xor));


                                #region //再显示 按键事件，USB插入检查，电池状态，佩戴检测等状态
                                //数据内容
                                byte[] dataArry = new byte[iTotalLength - 9];//config.DataLength
                                Array.Copy(oneFrameBytes, 8, dataArry, 0, iTotalLength - 9);//config.DataLength
                                //按键事件
                                switch (dataArry[0])
                                {
                                    case 0x00:
                                        log.Info("无按键");
                                        break;
                                    case 0x01:
                                        log.Info("电源键按下");
                                        break;
                                    case 0x02:
                                        log.Info("加热键被按下");
                                        break;
                                    case 0x03:
                                        log.Info("+键被按下");
                                        break;
                                    case 0x04:
                                        log.Info("-键被按下");
                                        break;

                                    default:
                                        break;
                                }
                                //USB插入事件
                                switch (dataArry[1])
                                {
                                    case 0x00:
                                        log.Info("无状态改变");
                                        break;
                                    case 0x01:
                                        log.Info("USB 未插入");
                                        break;
                                    case 0x02:
                                        log.Info("USB 已插入");
                                        break;

                                    default:
                                        break;
                                }
                                //电池状态
                                switch (dataArry[2])
                                {
                                    case 0x00:
                                        log.Info("无状态改变");
                                        break;
                                    case 0x01:
                                        log.Info("未充电");
                                        break;
                                    case 0x02:
                                        log.Info("充电中");
                                        break;
                                    case 0x03:
                                        log.Info("充满状态");
                                        break;
                                    case 0x04:
                                        log.Info("电量不足");
                                        break;

                                    default:
                                        break;
                                }
                                //佩戴检测状态
                                switch (dataArry[3])
                                {
                                    case 0x00:
                                        log.Info("无状态改变");
                                        break;
                                    case 0x01:
                                        log.Info("未检测到佩戴");
                                        break;
                                    case 0x02:
                                        log.Info("检测到有佩戴");
                                        break;

                                    default:
                                        break;
                                }
                                #endregion

                                if (formTipAndUartCheckProperties.CheckKeyEvent != FormTipAndUartCheckProperties.EnumCheckKeyEvent.不检查)
                                {
                                    switch (formTipAndUartCheckProperties.CheckKeyEvent)
                                    {
                                        case FormTipAndUartCheckProperties.EnumCheckKeyEvent.无按键:
                                            if (dataArry[0] != 0x00)
                                            {
                                                return;
                                            }
                                            break;
                                        case FormTipAndUartCheckProperties.EnumCheckKeyEvent.电源键按下:
                                            if (dataArry[0] != 0x01)
                                            {
                                                return;
                                            }
                                            break;

                                        case FormTipAndUartCheckProperties.EnumCheckKeyEvent.加热键被按下:
                                            if (dataArry[0] != 0x02)
                                            {
                                                return;
                                            }
                                            break;

                                        case FormTipAndUartCheckProperties.EnumCheckKeyEvent.加键被按下:
                                            if (dataArry[0] != 0x03)
                                            {
                                                return;
                                            }
                                            break;
                                        case FormTipAndUartCheckProperties.EnumCheckKeyEvent.减键被按下:
                                            if (dataArry[0] != 0x04)
                                            {
                                                return;
                                            }
                                            break;

                                        default:
                                            break;
                                    }
                                    log.Pass(string.Format("PASS,检测到正确按键上报事件：{0}", formTipAndUartCheckProperties.CheckKeyEvent));

                                }
                                if (formTipAndUartCheckProperties.CheckUsbInsert != FormTipAndUartCheckProperties.EnumCheckUsbInsertEvent.不检查)
                                {
                                    switch (formTipAndUartCheckProperties.CheckUsbInsert)
                                    {
                                        case FormTipAndUartCheckProperties.EnumCheckUsbInsertEvent.无状态改变:
                                            if (dataArry[1] != 0x00)
                                            {
                                                return;
                                            }
                                            break;
                                        case FormTipAndUartCheckProperties.EnumCheckUsbInsertEvent.USB未插入:
                                            if (dataArry[1] != 0x01)
                                            {
                                                return;
                                            }
                                            break;

                                        case FormTipAndUartCheckProperties.EnumCheckUsbInsertEvent.USB已插入:
                                            if (dataArry[1] != 0x02)
                                            {
                                                return;
                                            }
                                            break;

                                        default:
                                            break;
                                    }
                                    log.Pass(string.Format("PASS,检测到正确USB状态事件：{0}", formTipAndUartCheckProperties.CheckUsbInsert));
                                }
                                if (formTipAndUartCheckProperties.CheckBatteryStatus != FormTipAndUartCheckProperties.EnumCheckBatteryStatus.不检查)
                                {
                                    switch (formTipAndUartCheckProperties.CheckBatteryStatus)
                                    {
                                        case FormTipAndUartCheckProperties.EnumCheckBatteryStatus.无状态改变:
                                            if (dataArry[2] != 0x00)
                                            {
                                                return;
                                            }
                                            break;
                                        case FormTipAndUartCheckProperties.EnumCheckBatteryStatus.未充电:
                                            if (dataArry[2] != 0x01)
                                            {
                                                return;
                                            }
                                            break;

                                        case FormTipAndUartCheckProperties.EnumCheckBatteryStatus.充电中:
                                            if (dataArry[2] != 0x02)
                                            {
                                                return;
                                            }
                                            break;

                                        case FormTipAndUartCheckProperties.EnumCheckBatteryStatus.充满状态:
                                            if (dataArry[2] != 0x03)
                                            {
                                                return;
                                            }
                                            break;
                                        case FormTipAndUartCheckProperties.EnumCheckBatteryStatus.电量不足:
                                            if (dataArry[2] != 0x04)
                                            {
                                                return;
                                            }
                                            break;

                                        default:
                                            break;
                                    }
                                    log.Pass(string.Format("PASS,检测到正确电池状态上报事件：{0}", formTipAndUartCheckProperties.CheckBatteryStatus));

                                }
                                if (formTipAndUartCheckProperties.CheckWearingStatus != FormTipAndUartCheckProperties.EnumCheckWearingStatus.不检查)
                                {
                                    switch (formTipAndUartCheckProperties.CheckWearingStatus)
                                    {
                                        case FormTipAndUartCheckProperties.EnumCheckWearingStatus.无状态改变:
                                            if (dataArry[3] != 0x00)
                                            {
                                                return;
                                            }
                                            break;
                                        case FormTipAndUartCheckProperties.EnumCheckWearingStatus.未检测到佩戴:
                                            if (dataArry[3] != 0x01)
                                            {
                                                return;
                                            }
                                            break;

                                        case FormTipAndUartCheckProperties.EnumCheckWearingStatus.检测到有佩戴:
                                            if (dataArry[3] != 0x02)
                                            {
                                                return;
                                            }
                                            break;

                                        default:
                                            break;
                                    }
                                    log.Pass(string.Format("PASS,检测到正确的佩戴状态事件：{0}", formTipAndUartCheckProperties.CheckWearingStatus));
                                }

                                //if (formTipAndUartCheckProperties.== 检查是否是充满状态事件)
                                //{
                                //    if (dataArry[2] != 0x03)
                                //    {
                                //        return;
                                //    }
                                //}
                                //跳出循环执行
                                result = true;
                                timer.Stop();
                                //throw new BaseException(string.Format("not contain OK=[{0}]", this.formTipAndUartCheckProperties.AtCommandOk));
                                //log.Pass(string.Format("PASS,正确状态上报事件：{0}", this.formTipAndUartCheckProperties.AtCommandOk));
                                //Thread.Sleep(500);

                                this.Close();

                                #endregion
                            }
                            else
                            {
                                log.Info(string.Format("头部不是0xA5,0xC3，buffer.RemoveAt：{0:X2},{1:X2}", buffer[0], buffer[1]));
                                buffer.RemoveRange(0, 2);
                                ////继续循环执行
                                //return false;

                            }

                        }
                        else
                        {
                            log.Info(string.Format("头部不是0xA5，buffer.RemoveAt：{0:X2}", buffer[0]));
                            buffer.RemoveAt(0);
                        }
                    }

                    #region //字符串的检查关键字的方式
                    //string response = byteToHexStr(buf);

                    #region //先检查格式
                    //if (buf.Length < 9)
                    //{
                    //    return;
                    //    //throw new BaseException(string.Format("Length Not enough"));
                    //}
                    ////包头
                    //if (!(buf[0] == 0xA5 && buf[1] == 0xC3))
                    //{
                    //    return;

                    //    //throw new BaseException(string.Format("Head Error"));
                    //}
                    ////长度 2字节
                    //byte high = buf[2];
                    //byte low = buf[3];
                    //int iTotalLength = (high & 0xFF) << 8 | low;

                    //if (iTotalLength != n)
                    //{
                    //    return;
                    //    //throw new BaseException(string.Format("Length Error,长度位：{0},实际长度:{1}", iTotalLength, n));
                    //}

                    ////命令字
                    //if (!(buf[6] == 0xA0 && buf[7] == 0x41))
                    //{
                    //    return;
                    //    //throw new BaseException(string.Format("CMD Type Error"));
                    //}


                    ////异或校验 1字节
                    //byte[] bufOutOxr = new byte[buf.Length - 1];
                    //Array.Copy(buf, 0, bufOutOxr, 0, buf.Length - 1);
                    //byte xor = ByteToXOR(bufOutOxr);

                    //if (buf[n - 1] != xor)
                    //{
                    //    return;
                    //    //throw new BaseException(string.Format("校验不正确,返回校验位:{0},计算校验位:{1}", buf[n - 1], xor));
                    //}
                    //log.Info(string.Format("校验位核验正确,返回校验位:{0:X2},计算校验位:{1:X2}", buf[n - 1], xor));
                    #endregion

                    #region //再显示 按键事件，USB插入检查，电池状态，佩戴检测等状态
                    ////数据内容
                    //byte[] dataArry = new byte[iTotalLength - 9];//config.DataLength
                    //Array.Copy(buf, 8, dataArry, 0, iTotalLength - 9);//config.DataLength
                    ////按键事件
                    //switch (dataArry[0])
                    //{
                    //    case 0x00:
                    //        log.Info("无按键");
                    //        break;
                    //    case 0x01:
                    //        log.Info("电源键按下");
                    //        break;
                    //    case 0x02:
                    //        log.Info("加热键被按下");
                    //        break;
                    //    case 0x03:
                    //        log.Info("+键被按下");
                    //        break;
                    //    case 0x04:
                    //        log.Info("-键被按下");
                    //        break;

                    //    default:
                    //        break;
                    //}
                    ////USB插入事件
                    //switch (dataArry[1])
                    //{
                    //    case 0x00:
                    //        log.Info("无状态改变");
                    //        break;
                    //    case 0x01:
                    //        log.Info("USB 未插入");
                    //        break;
                    //    case 0x02:
                    //        log.Info("USB 已插入");
                    //        break;

                    //    default:
                    //        break;
                    //}
                    ////电池状态
                    //switch (dataArry[2])
                    //{
                    //    case 0x00:
                    //        log.Info("无状态改变");
                    //        break;
                    //    case 0x01:
                    //        log.Info("未充电");
                    //        break;
                    //    case 0x02:
                    //        log.Info("充电中");
                    //        break;
                    //    case 0x03:
                    //        log.Info("充满状态");
                    //        break;
                    //    case 0x04:
                    //        log.Info("电量不足");
                    //        break;

                    //    default:
                    //        break;
                    //}
                    ////佩戴检测状态
                    //switch (dataArry[3])
                    //{
                    //    case 0x00:
                    //        log.Info("无状态改变");
                    //        break;
                    //    case 0x01:
                    //        log.Info("未检测到佩戴");
                    //        break;
                    //    case 0x02:
                    //        log.Info("检测到有佩戴");
                    //        break;

                    //    default:
                    //        break;
                    //}
                    #endregion

                    //if (!string.IsNullOrEmpty(this.formTipAndUartCheckProperties.AtCommandOk))
                    //{
                    //    if (response.Contains(this.formTipAndUartCheckProperties.AtCommandOk))
                    //    {
                    //        result = true;
                    //        timer.Stop();
                    //        //throw new BaseException(string.Format("not contain OK=[{0}]", this.formTipAndUartCheckProperties.AtCommandOk));
                    //        log.Pass(string.Format("PASS,返回正确状态关键字：{0}", this.formTipAndUartCheckProperties.AtCommandOk));
                    //        //Thread.Sleep(500);

                    //        this.Close();
                    //    }
                    //}
                    #endregion
                }




            }
        }
        /// <summary>
        /// 对同一数组内数据进行异或
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        private byte ByteToXOR(byte[] arr)
        {
            byte xor = 0;

            for (int i = 0; i < arr.Length; i++)
            {
                xor ^= arr[i];

            }
            //Console.WriteLine("0x{0:x}", xor);
            //Console.ReadKey();
            return xor;
        }

        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        public void UartDisplay(byte[] by, string type)
        {
            string log = "";
            for (int i = 0; i < by.Length; i++)
            {
                log += by[i].ToString("X2") + " ";
            }
            if (type == "R")
            {
                this.log.Info("Rx:" + log);
            }
            else if (type == "T")
            {
                this.log.Info("Tx:" + log);
            }
            else
            {
                this.log.Info(log);
            }

        }

        //protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        //{
        //    if (keyData == userConfirmProperties.KeyFail)
        //    {
        //        log.Fail("User Press " + userConfirmProperties.KeyFail);
        //        this.Close();
        //    }
        //    else if (keyData == userConfirmProperties.KeyPass)
        //    {
        //        result = true;
        //        log.Pass("User Press " + userConfirmProperties.KeyPass);
        //        this.Close();
        //    }
        //return base.ProcessCmdKey(ref msg, keyData);
        //}
    }
}
