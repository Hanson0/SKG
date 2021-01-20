using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Executer
{
    public class CheckWifiMapExecuter : IExecuter
    {
        //FindDeviceProperties config;
        private GlobalVaribles configGv;
        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {
            //config = properties as FindDeviceProperties;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;
            //String mapFileString = globalDic[typeof(String).ToString()] as String;

            String mapFileString = configGv.Get("MapFileStringMacUpdate");

            //int ret = -1;
            //string returnString = "";

            //取存储的关键字为MoudleBtMac的value值
            string moudleWifiMap = configGv.Get("MoudleWifiMap");
            moudleWifiMap = moudleWifiMap.Replace("0x", "").Replace(" ", "");

            //for (int j = 4; j < moudleWifiMap.Length; j = j + 4)
            //{
            //    moudleWifiMap = moudleWifiMap.Insert(j, " ");
            //    j = j + 1;
            //}

            for (int i = 32; i < moudleWifiMap.Length; i = i + 32)
            {
                //if (moudleWifiMap[i])
                //{

                //}
                moudleWifiMap = moudleWifiMap.Insert(i, "\r\n");
                i = i + 2;

            }

            string[] splits = { "\r\n" };
            string[] moudleWifiMapLines = moudleWifiMap.Split(splits, StringSplitOptions.RemoveEmptyEntries);

            string tempMoudleWifiMap = "";
            for (int i = 0; i < moudleWifiMapLines.Length; i++)
            {
                for (int j = 4; j < moudleWifiMapLines[i].Length; j = j + 4)
                {
                    if (j != 39)
                    {
                        moudleWifiMapLines[i] = moudleWifiMapLines[i].Insert(j, " ");
                        j = j + 1;
                        //拼接完整
                    }
                }
                tempMoudleWifiMap += moudleWifiMapLines[i] + "\r\n";
            }

            if (tempMoudleWifiMap != mapFileString)
            {
                log.Fail(string.Format("WIFI MAP核验：\r\n读出模组WIFI MAP：\r\n{0}\r\n文件 WIFI MAP：\r\n{1}\r\n不一致 \r\n", tempMoudleWifiMap, mapFileString));
                throw new Exception(string.Format("WIFI MAP核验：不一致 \r\n"));
            }
            //string moudleBtMac = configGv.Get("");
            log.Pass(string.Format("WIFI MAP核验：\r\n读出模组WIFI MAP：\r\n{0}\r\n文件 WIFI MAP：\r\n{1}\r\n一致 \r\n", tempMoudleWifiMap, mapFileString));

        }
    }

}

