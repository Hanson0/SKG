using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using AILinkFactoryAuto.Task.SmartBracelet.Property;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AILinkFactoryAuto.Task.SmartBracelet.Executer
{
    public class ScanBindBtExecuter : IExecuter
    {
        ScanBindBtProperties config;
        private GlobalVaribles configGv;
        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {
            config = properties as ScanBindBtProperties;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;
            string DevBt_MAC = configGv.Get(config.MacGlobalVaribles);
            //int ret = -1;
            //string returnString = "";




            ////取存储的关键字为MoudleBtMac的value值
            //string moudleBtMac = configGv.Get("MoudleBtMac");
            //string btMacFromImes = configGv.Get("MAC_BT"); ;//configGv.Get("MAC_BT");

            //if (moudleBtMac != btMacFromImes)
            //{
            //    log.Fail(string.Format("BT MAC核验：读出BT MAC：{0}，与预写入MAC：{1} 不一致 FAIL\r\n", moudleBtMac, btMacFromImes));
            //    throw new BaseException(string.Format("BT MAC核验：读出BT MAC：{0}，与预写入MAC：{1} 不一致 \r\n", moudleBtMac, btMacFromImes));
            //}
            ////string moudleBtMac = configGv.Get("");
            //log.Info(string.Format("BT MAC核验：读出BT MAC：{0}，与预写入MAC：{1} 一致，PASS \r\n", moudleBtMac, btMacFromImes));


            BluetoothRadio radio = BluetoothRadio.PrimaryRadio;//获取当前PC的蓝牙适配器  
            //CheckForIllegalCrossThreadCalls = false;//不检查跨线程调用  
            radio.Mode = RadioMode.Discoverable;
            if (radio == null)//检查该电脑蓝牙是否可用  
            {
                throw new BaseException("本机电脑蓝牙不可用！");
            }


            BluetoothClient Blueclient = new BluetoothClient();

            BluetoothDeviceInfo[] devices = Blueclient.DiscoverDevices();
            bool hasScanded = false;
            BluetoothAddress btMac=null ;
            string btName = "";
            foreach (BluetoothDeviceInfo item in devices)
            {
                log.Info(string.Format("设备名:{0}\t\t地址:{1}", item.DeviceName, item.DeviceAddress));
                if (item.DeviceAddress.ToString() == DevBt_MAC)//"1895526CCD7B"
                {
                    hasScanded = true;
                    btMac = item.DeviceAddress;
                    btName = item.DeviceName;
                }
            }
            //Dictionary<string, BluetoothAddress> deviceAddresses = new Dictionary<string, BluetoothAddress>();
            //BluetoothRadio BuleRadio = BluetoothRadio.PrimaryRadio;
            //BuleRadio.Mode = RadioMode.Connectable;
            //BluetoothDeviceInfo[] Devices = Blueclient.DiscoverDevices();
            ////lsbDevices.Items.Clear();
            //deviceAddresses.Clear();
            //foreach (BluetoothDeviceInfo device in Devices)
            //{
            //    //循环将所有搜索到的设备添加进来。
            //    //lsbDevices.Items.Add(device.DeviceName);
            //    deviceAddresses[device.DeviceName] = device.DeviceAddress;
            //}
            if (!hasScanded)
            {
                throw new BaseException(string.Format("未扫描到指定MAC的蓝牙信号 \r\n"));
            }
            if (btMac==null)
            {
                throw new BaseException(string.Format("btMac为空，未扫描到指定MAC的蓝牙 \r\n"));
            }
            log.Info(string.Format("检测蓝牙RF信号PASS，MAC:{0}", btMac));


            //Guid mGUID2 = Guid.Parse("00001101-0000-1000-8000-00805F9B34FB");
            //Guid mGUID = Guid.Parse("0000fff4-0000-1000-8000-00805F9B34FB");//蓝牙串口服务的uuiid
            //配对：
            //Blueclient.SetPin(btMac, mGUID2);
            //Blueclient.SetPin(btMac, mGUID);

            //Blueclient.Connect(btMac, mGUID);
            if (config.ConnectionPairing)
            {
                Blueclient.Connect(btMac, BluetoothService.Handsfree);

                log.Info(string.Format("配对连接测试成功,{0}", btName));
                //Blueclient.Connect(btMac, mGUID2);
                //Blueclient.
                //Thread.Sleep(10000);
            }
            Blueclient.Close();


        }
    }
}
