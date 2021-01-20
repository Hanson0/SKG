using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;

namespace AILinkFactoryAuto.Dut.AtCommand.Property.Converter
{
    class PortNamesConverter : StringConverter
    {
        //true enable,false disable
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(SerialPort.GetPortNames()); //编辑下拉框中的items
        }

        //true: disable text editting.    false: enable text editting;
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}
