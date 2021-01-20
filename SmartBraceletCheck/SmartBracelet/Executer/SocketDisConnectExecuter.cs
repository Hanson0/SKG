using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using AILinkFactoryAuto.Task.SmartBracelet.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Executer
{
    public class SocketDisConnectExecuter : IExecuter
    {
        SocketDisConnectProperties config;
        private GlobalVaribles configGv;
        //private List<WIFISSID> ssids=new List<WIFISSID>();
        //private wifiSo wifiso;
        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {
            config = properties as SocketDisConnectProperties;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;

            try
            {
                Socket objSocket = configGv.GetObject("ObjSocket") as Socket;
                if (objSocket != null)
                {
                    if (objSocket.Connected)
                    {
                        //objSocket.Disconnect(true);
                        objSocket.Close();
                        if (objSocket.Connected)
                        {
                            throw new BaseException(string.Format("断开连接失败，状态仍然为连接状态"));
                        }
                        log.Info("断开Soket连接成功");
                    }
                    else
                    {
                        throw new BaseException(string.Format("未连接Soket服务器，无法断开连接1"));
                    }
                }
                else
                {
                    throw new BaseException(string.Format("未连接Soket服务器，objSocket为空，无法断开连接"));
                }

            }
            catch (Exception ex)
            {
                throw new Exception("断开Soket连接出错，" + ex.Message);
            }

        }
    }
}
