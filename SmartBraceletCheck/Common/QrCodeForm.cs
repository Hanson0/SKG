using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ThoughtWorks.QRCode.Codec;

namespace AILinkFactoryAuto.Task.Common
{
    public partial class QrCodeForm : Form
    {
        public QrCodeForm(string strQRCode, int showTime)
        {
            InitializeComponent();
            pictureBox1.BackgroundImage = QRCode(strQRCode);
            timer1.Interval = showTime;
            timer1.Enabled = true;
        }

        #region 生成二维码
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private Bitmap QRCode(string enCodeString)
        {
            System.Drawing.Bitmap bt;
            //string enCodeString = "http://www.baidu.com";

            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;//编码方式(注意：BYTE能支持中文，ALPHA_NUMERIC扫描出来的都是数字)
            qrCodeEncoder.QRCodeScale = 4;//大小(值越大生成的二维码图片像素越高)
            qrCodeEncoder.QRCodeVersion = 0;//版本(注意：设置为0主要是防止编码的字符串太长时发生错误)
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;//错误效验、错误更正(有4个等级)
            qrCodeEncoder.QRCodeBackgroundColor = Color.Yellow;//背景色
            qrCodeEncoder.QRCodeForegroundColor = Color.Green;//前景色

            bt = qrCodeEncoder.Encode(enCodeString, Encoding.UTF8);

            //string filename = "code";
            //string file_path = AppDomain.CurrentDomain.BaseDirectory + "QRCode\\";
            //string codeUrl = file_path + filename + ".jpg";

            ////根据文件名称，自动建立对应目录
            //if (!System.IO.Directory.Exists(file_path))
            //    System.IO.Directory.CreateDirectory(file_path);

            //bt.Save(codeUrl);//保存图片
            //return codeUrl;
            return bt;

        }
        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Close();
        }

    }



}
