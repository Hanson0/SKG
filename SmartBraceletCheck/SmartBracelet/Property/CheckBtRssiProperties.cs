using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class CheckBtRssiProperties : CommonProperties
    {
        private int maxValue;
        private int minValue;


        [Category("RSSI"), Description("RSSIMaxValue")]
        public int MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }
        [Category("RSSI"), Description("RSSIMinValue")]
        public int MinValue
        {
            get { return minValue; }
            set { minValue = value; }
        }

    }

}
