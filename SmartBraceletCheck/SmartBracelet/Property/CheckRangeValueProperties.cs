using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class CheckRangeValueProperties : CommonProperties
    {
        private string testValueName;
        private double maxValue;
        private double minValue;
        private string globalVarible;
        
        [Category("Item"), Description("测试值名")]
        public string TestValueName
        {
            get { return testValueName; }
            set { testValueName = value; }
        }

        [Category("Range"), Description("标准范围,最小值")]
        public double MinValue
        {
            get { return minValue; }
            set { minValue = value; }
        }


        [Category("Range"), Description("标准范围,最大值")]
        public double MaxValue
        {
            get { return maxValue; } 
            set { maxValue = value; }
        }

        [Category("Item"), Description("变量名")]
        public string GlobalVarible
        {
            get { return globalVarible; }
            set { globalVarible = value; }
        }

    }
}
