using AILinkFactoryAuto.Core;
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

        public bool Result
        {
            get { return result; }
        }

        public TipAndCheckUartForm()
        {
            InitializeComponent();
        }

        public TipAndCheckUartForm(FormTipAndUartCheckProperties formTipAndUartCheckProperties, ILog log)
        {
            InitializeComponent();

            this.formTipAndUartCheckProperties = formTipAndUartCheckProperties;
            this.log = log;

            //lblInfoPass.Text = "PASS：请按" + userConfirmProperties.KeyPass + "键";
            //lblInfoFail.Text = "FAIL：请按" + userConfirmProperties.KeyFail + "键";
            txtTips.Text = formTipAndUartCheckProperties.Tips;
            countDown = new TimeSpan(formTipAndUartCheckProperties.CountDownTime * 1000 * 10);
        }

        private void TipAndCheckUartForm_Shown(object sender, EventArgs e)
        {
            txtTips.SelectionLength = 0;

            Timer timer = new Timer();
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
                Timer timer = sender as Timer;
                timer.Stop();

                log.Fail("Wait TimeOut, " + countDown.Seconds + "seconds");
                Thread.Sleep(400);

                this.Close();
            }
            else
            {
                lblCountDown.Text = "倒计时：" + remainTime.Seconds + "秒";
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
        //    return base.ProcessCmdKey(ref msg, keyData);
        //}
    }
}
