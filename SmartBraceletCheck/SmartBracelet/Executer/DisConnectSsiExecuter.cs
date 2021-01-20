using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using AILinkFactoryAuto.Task.SmartBracelet.Property;
using SimpleWifi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Executer
{
    public class DisConnectSsiExecuter : IExecuter
    {
        DisConnectSsiProperties config;
        private GlobalVaribles configGv;
        //private List<WIFISSID> ssids=new List<WIFISSID>();
        //private wifiSo wifiso;
        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {
            config = properties as DisConnectSsiProperties;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;

            try
            {
                Wifi objWifi = configGv.GetObject("ObjWifi") as Wifi;
                if (objWifi != null)
                {
                    if (objWifi.ConnectionStatus == WifiStatus.Connected)
                    {
                        objWifi.Disconnect();
                        log.Info("断开WIFI连接成功");
                    }
                    else
                    {
                        throw new BaseException(string.Format("未连接WIFI热点，无法断开连接1"));
                    }
                }
                else
                {
                    throw new BaseException(string.Format("未连接WIFI热点，objWifi为空，无法断开连接"));
                }
                //if (!hasSsid)
                //{
                //    throw new BaseException(string.Format("未扫描到SSID:{0}", ssid));

                //}

            }
            catch (Exception ex)
            {
                throw new Exception("断开热点出错，" + ex.Message);
            }

        }
    }
}
