using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using AILinkFactoryAuto.Task.SmartBracelet.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Executer
{
    public class SocketConnectExecuter : IExecuter
    {
        SocketConnectProperties config;
        private GlobalVaribles configGv;
        //private List<WIFISSID> ssids=new List<WIFISSID>();
        //private wifiSo wifiso;
        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {
            config = properties as SocketConnectProperties;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;

            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Parse(config.Ip);
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 2500);
                socket.Connect(ip, Convert.ToInt16(config.Port));
                if (!socket.Connected)
                {
                    throw new Exception(string.Format("Socke连接服务器失败,{0}:{1}\r\n连接状态为false", config.Ip, config.Port));
                }
                log.Info(string.Format("Socke连接服务器连接成功,{0}:{1}", config.Ip, config.Port));
                configGv.Add("ObjSocket", socket);

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Socke连接服务器失败,{0}:{1}\r\n详情:{2}", config.Ip, config.Port, ex.Message));
            }

        }
    }
}
