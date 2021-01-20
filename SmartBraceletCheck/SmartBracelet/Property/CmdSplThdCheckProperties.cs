using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class CmdSplThdCheckProperties : CmdProperties
    {
        private int checkFreqValue;
        private float splMaxOffset;
        private float thdMaxOffset;


        [Category("FreqValue"), Description("检查的频率点,单位:HZ")]
        public int CheckFreqValue
        {
            get { return checkFreqValue; }
            set { checkFreqValue = value; }
        }

        [Category("SplMaxOffset"), Description("MIC通道间灵敏度最大差值,单位:HZ")]
        public float SplMaxOffset
        {
            get { return splMaxOffset; }
            set { splMaxOffset = value; }
        }

        [Category("ThdMaxOffset"), Description("MIC通道间谐波失真最大差值，单位:%")]
        public float ThdMaxOffset
        {
            get { return thdMaxOffset; }
            set { thdMaxOffset = value; }
        }

    }
}
