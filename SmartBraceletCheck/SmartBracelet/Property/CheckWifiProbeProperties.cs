using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class CheckWifiProbeProperties : CommonProperties
    {
        private int maxValue;
        private int minValue;


        [Category("Check WifiProbe"), Description("WifiProbeMaxValue")]
        public int MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }
        [Category("Check WifiProbe"), Description("WifiProbeMinValue")]
        public int MinValue
        {
            get { return minValue; }
            set { minValue = value; }
        }

    }

}
