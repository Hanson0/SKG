using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Property
{
    public class SocketConnectProperties : CommonProperties
    {
        private string ip;
        private string port;


        [Category("SOCKET")]
        [Description("Ip")]
        public string Ip
        {
            get { return ip; }
            set { ip = value; }
        }


        [Category("SOCKET")]
        [Description("Port")]
        public string Port
        {
            get { return port; }
            set { port = value; }
        }

    }
}
