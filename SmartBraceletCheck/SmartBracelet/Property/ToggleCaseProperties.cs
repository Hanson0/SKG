using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class ToggleCaseProperties : CommonProperties
    {
        private string preGlobalVaribles;
        private string afterGlobalVaribles;
        
        private CaseEnum toggleCase;

        public enum CaseEnum
        {
            ToUper,
            ToLower
        }

        [Category("PreGlobalVaribles"), Description("将要转换的变量")]
        public string PreGlobalVaribles { get { return preGlobalVaribles; } set { preGlobalVaribles = value; } }



        [Category("ToggleCase"), Description("转换为大写/小写")]
        public CaseEnum ToggleCase { get { return toggleCase; } set { toggleCase = value; } }


        [Category("AfterGlobalVaribles"), Description("转换后存放的变量")]
        public string AfterGlobalVaribles { get { return afterGlobalVaribles; } set { afterGlobalVaribles = value; } }


    }
}
