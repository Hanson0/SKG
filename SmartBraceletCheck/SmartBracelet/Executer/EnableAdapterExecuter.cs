using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using AILinkFactoryAuto.Task.SmartBracelet.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;

namespace AILinkFactoryAuto.Task.SmartBracelet.Executer
{
    public class EnableAdapterExecuter : IExecuter
    {
        EnableAdapterProperties config;
        private GlobalVaribles configGv;

        List<string> netWorkList;
        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {
            config = properties as EnableAdapterProperties;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;
            //NetWorkList();
            if (NetWorkState(config.NICname))
            {
                if (!EnableNetWork(NetWork(config.NICname)))
                {
                    throw new BaseException(string.Format("开启网卡：{0}，失败", config.NICname));
                }
                log.Info(string.Format("开启网卡：{0}，成功", config.NICname));
            }
            else
            {
                log.Info(string.Format("未找到该网卡：{0}，网卡已启用", config.NICname));
            }
            
            //EnableAdapter(out mac, config.NICname);
        }





        // <summary>
        /// 网卡列表
        /// </summary>
        public void NetWorkList()
        {
            string manage = "SELECT * From Win32_NetworkAdapter";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(manage);
            ManagementObjectCollection collection = searcher.Get();
            netWorkList = new List<string>();

            foreach (ManagementObject obj in collection)
            {
                netWorkList.Add(obj["Name"].ToString());

            }
            //this.cmbNetWork.DataSource = netWorkList;

        }

        /// <summary>
        /// 禁用网卡
        /// </summary>5
        /// <param name="netWorkName">网卡名</param>
        /// <returns></returns>
        public bool DisableNetWork(ManagementObject network)
        {
            try
            {
                network.InvokeMethod("Disable", null);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 启用网卡
        /// </summary>
        /// <param name="netWorkName">网卡名</param>
        /// <returns></returns>
        public bool EnableNetWork(ManagementObject network)
        {
            try
            {
                network.InvokeMethod("Enable", null);
                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// 网卡状态
        /// </summary>
        /// <param name="netWorkName">网卡名</param>
        /// <returns></returns>
        public bool NetWorkState(string netWorkName)
        {
            string netState = "SELECT * From Win32_NetworkAdapter";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(netState);
            ManagementObjectCollection collection = searcher.Get();
            foreach (ManagementObject manage in collection)
            {
                if (manage["Name"].ToString() == netWorkName)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 得到指定网卡
        /// </summary>
        /// <param name="networkname">网卡名字</param>
        /// <returns></returns>
        public ManagementObject NetWork(string networkname)
        {
            string netState = "SELECT * From Win32_NetworkAdapter";

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(netState);
            ManagementObjectCollection collection = searcher.Get();

            foreach (ManagementObject manage in collection)
            {
                if (manage["Name"].ToString() == networkname)
                {
                    return manage;
                }
            }


            return null;
        }

    }
}
