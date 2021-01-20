using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class CheckTempProperties : CommonProperties
    {
        private float maxValue;
        private float minValue;


        [Category("Check Temp"), Description("TempMaxValue")]
        public float MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }
        [Category("Check Temp"), Description("TempMinValue")]
        public float MinValue
        {
            get { return minValue; }
            set { minValue = value; }
        }

    }
}
