using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using AILinkFactoryAuto.Task.SmartBracelet.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Executer
{
    public class SkgAuthorizeCheckExecuter : IExecuter
    {
        private string pattern;

        private GlobalVaribles configGv;


        private ILog log;
        private List<byte> buffer = new List<byte>(4096);       //uart 接受数据buffer 未做任何处理

        //public SKGExecuter()
        //{
        //    this.pattern = GlobalVaribles.PATTERN;
        //}




        public void Run(IProperties properties, GlobalDic<string, object> globalDic)
        {
            SkgAuthorizeCheckProperties config = properties as SkgAuthorizeCheckProperties;

            //解析是否收到正确的包,包头，命令，校验位
            //起始标志 数据长度 保留字     命令字     数据内容    异或校验码 
            //0XA5C3    9+N     0x0000      0x0037      Data        异或 
            //2 字节  2 字节    2 字节      2 字节      N 字节     1 字节
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;

            byte[] dataArry = (byte[])configGv.GetObject(config.GlobalVariblesKey);

            //判断授权是否成功
            //MCUID是否为00

            #region
            //string fireWareVersion = config.FirewareVersion;
            //string softwareVersion = config.SoftwareVersion;
            //string firmwareName = config.FirmwareName;
            //#region
            ////是否完成过PCBA 0x01：已完成过 其他值：未完成过
            //if (true)
            //{
            //    if (dataArry[0] == 0x01)
            //    {
            //        log.Info("完成过PCBA测试");
            //    }
            //    else
            //    {
            //        log.Info("未完成过PCBA测试");
            //    }
            //}

            ////查询是否完成过整机测试  0x01：已完成过 其他值：未完成过
            //if (true)//check Whole machine test
            //{
            //    if (dataArry[1] == 0x01)
            //    {
            //        log.Info("完成过整机测试");
            //    }
            //    else
            //    {
            //        log.Info("未完成过整机测试");
            //    }
            //}

            ////查询是否完成过老化测试  0x01：已完成过 其他值：未完成过
            //if (true)//checkAgeing
            //{
            //    if (dataArry[2] == 0x01)
            //    {
            //        log.Info("完成过老化测试");
            //    }
            //    else
            //    {
            //        log.Info("未完成过老化测试");
            //    }
            //}


            ////硬件版本号（高位） 0x00 硬件版本号（高位）
            //if (true)
            //{
            //    byte[] dataFirmwareVersion = new byte[2];
            //    Array.Copy(dataArry, 3, dataFirmwareVersion, 0, 2);
            //    string strFirmwareVersion = System.Text.Encoding.ASCII.GetString(dataFirmwareVersion);
            //    //string strFirmwareVersion = byteToHexStr(dataFirmwareVersion);
            //    if (strFirmwareVersion != fireWareVersion)
            //    {
            //        throw new BaseException(string.Format("硬件版本号:{0},与设置版本号:{1}不一致", strFirmwareVersion, fireWareVersion));
            //    }
            //    log.Info(string.Format("硬件版本号:{0},与设置版本号:{1}一致", strFirmwareVersion, fireWareVersion));
            //}


            //软件版本号（高位） 0x00 软件版本号 6 
            //软件版本号 0x01 软件版本号 7 
            //软件版本号 0x02 软件版本号 8 
            //软件版本号（低位） 0x03 软件版本号（低位）
            //if (true)
            //{
            //    byte[] dataSoftwareVersion = new byte[4];
            //    Array.Copy(dataArry, 5, dataSoftwareVersion, 0, 4);
            //    string strSoftwareVersion = System.Text.Encoding.ASCII.GetString(dataSoftwareVersion);
            //    //string strSoftwareVersion = byteToHexStr(dataSoftwareVersion);
            //    if (strSoftwareVersion != softwareVersion)
            //    {
            //        throw new BaseException(string.Format("软件版本号:{0},与设置版本号:{1}不一致", strSoftwareVersion, softwareVersion));
            //    }
            //    log.Info(string.Format("软件版本号:{0},与设置版本号:{1}一致", strSoftwareVersion, softwareVersion));
            //}

            #endregion
            //MCU ID
            byte[] dataMcuId = new byte[12];
            Array.Copy(dataArry, 26, dataMcuId, 0, 12);
            //ASCII码的方式
            //string strDataMcuId = System.Text.Encoding.ASCII.GetString(dataMcuId);
            //MCU ID：1F39A109高位在前1F,39的直接16进制拼接方式
            string strDataMcuId = byteToHexStr(dataMcuId);
            log.Info(string.Format("MCU ID:{0}", strDataMcuId));
            if (strDataMcuId.Contains("00000000000000000000"))
            {
                throw new BaseException(string.Format("授权失败,数据为00的MCUID+"));
            }
            //BLE MAC
            byte[] dataBleMac = new byte[6];
            Array.Copy(dataArry, 38, dataBleMac, 0, 12);
            //ASCII码的方式
            //string strDataMcuId = System.Text.Encoding.ASCII.GetString(dataMcuId);
            //BLE MAC：1F39A109,低位在前0x09,0xA1的直接16进制拼接方式
            string strDataBleMac = byteToHexStrReverse(dataBleMac);
            log.Info(string.Format("BLE MAC:{0}", strDataBleMac));
            log.Info(string.Format("授权成功"));
            #region
            ////电池电压
            ////长度 2字节
            //if (true)
            //{
            //    byte high = dataArry[27];
            //    byte low = dataArry[28];
            //    int vol = (high & 0xFF) << 8 | low;
            //    CheckRange("电压值(mV)：", vol, config.VolMaxValue, config.VolMinValue);
            //}

            ////NTC温度
            ////长度 2字节
            //if (true)
            //{
            //    byte high = dataArry[29];
            //    if (high == 0x80)
            //    {
            //        throw new BaseException(string.Format("NTC1 开路或短路,NTC温度输出 0x8000"));
            //    }

            //    byte low = dataArry[30];
            //    int ntc = (high & 0xFF) << 8 | low;
            //    CheckRange("NTC1温度值(单位0.1摄氏度)：", ntc, config.Ntc1MaxValue, config.Ntc1MinValue);
            //}

            ////PCBA-ID
            //if (true)
            //{
            //    if (dataArry[31] > 0x18)
            //    {
            //        throw new BaseException(string.Format("PCBID 有效长度:{0}不在0~24的合理范围内，未授权过", dataArry[31]));
            //    }
            //    byte[] dataPcbaId = new byte[24];
            //    Array.Copy(dataArry, 32, dataPcbaId, 0, 24);
            //    //ASCII码的方式
            //    //string strDataMcuId = System.Text.Encoding.ASCII.GetString(dataMcuId);
            //    //MCU ID：1F39A109高位在前1F,39的直接16进制拼接方式
            //    string strDataPcbaId = byteToHexStr(dataPcbaId);
            //    log.Info(string.Format("PCBA-ID:{0}", strDataPcbaId));
            //}

            ////SN
            //if (true)
            //{
            //    if (dataArry[31] > 0x18)
            //    {
            //        throw new BaseException(string.Format("SN 有效长度:{0}不在0~24的合理范围内，未授权过", dataArry[31]));
            //    }

            //    byte[] dataSn = new byte[24];
            //    Array.Copy(dataArry, 32, dataSn, 0, 24);
            //    //ASCII码的方式
            //    //string strDataMcuId = System.Text.Encoding.ASCII.GetString(dataMcuId);
            //    //MCU ID：1F39A109高位在前1F,39的直接16进制拼接方式
            //    //不足的，需要判断补0?
            //    string strDataSn = byteToHexStr(dataSn);
            //    log.Info(string.Format("SN:{0}", strDataSn));
            //}
            ////固件名称
            //if (true)
            //{
            //    byte[] dataFirmwareName = new byte[8];
            //    Array.Copy(dataArry, 81, dataFirmwareName, 0, 8);
            //    string strDataFirmwareName = System.Text.Encoding.ASCII.GetString(dataFirmwareName);
            //    //string strSoftwareVersion = byteToHexStr(dataSoftwareVersion);
            //    if (strDataFirmwareName != firmwareName)
            //    {
            //        throw new BaseException(string.Format("固件名称:{0},与设置固件名称:{1}不一致", strDataFirmwareName, firmwareName));
            //    }
            //    log.Info(string.Format("固件名称:{0},与设置固件名称:{1}一致", strDataFirmwareName, firmwareName));
            //}
            ////电机转速
            //if (true)
            //{
            //    byte high = dataArry[89];
            //    byte low = dataArry[90];
            //    int motorSpeed = (high & 0xFF) << 8 | low;
            //    CheckRange("电机转速：", motorSpeed, config.MotorSpeedMaxValue, config.MotorSpeedMinValue);
            //}

            ////NTC2温度
            //if (true)
            //{
            //    byte high = dataArry[91];
            //    if (high == 0x80)
            //    {
            //        throw new BaseException(string.Format("NTC2 开路或短路,NTC温度输出 0x8000"));
            //    }

            //    byte low = dataArry[92];
            //    int ntc = (high & 0xFF) << 8 | low;
            //    CheckRange("NTC2温度值(单位0.1摄氏度)：", ntc, config.Ntc2MaxValue, config.Ntc2MinValue);
            //}

            ////NTC3温度
            //if (true)
            //{
            //    byte high = dataArry[93];
            //    if (high == 0x80)
            //    {
            //        throw new BaseException(string.Format("NTC3 开路或短路,NTC温度输出 0x8000"));
            //    }

            //    byte low = dataArry[94];
            //    int ntc = (high & 0xFF) << 8 | low;
            //    CheckRange("NTC3温度值(单位0.1摄氏度)：", ntc, config.Ntc3MaxValue, config.Ntc3MinValue);
            //}

            #endregion


        }


        private void CheckRange(string TestValueName, int intMoudleBtRssi, int MaxValue, int MinValue)
        {
            if (intMoudleBtRssi > MaxValue)
            {
                //log.Fail(string.Format("{0}：{1}，大于设定最大值：{2} FAIL \r\n", TestValueName, intMoudleBtRssi, .MaxValue));
                throw new BaseException(string.Format("{0}：{1}，大于设定最大值：{2} FAIL \r\n", TestValueName, intMoudleBtRssi, MaxValue));
            }
            else if (intMoudleBtRssi < MinValue)
            {
                //log.Fail(string.Format("{0}：{1}，小于设定最小值：{2} FAIL \r\n", TestValueName,intMoudleBtRssi, MinValue));
                throw new BaseException(string.Format("{0}：{1}，小于设定最小值：{2} FAIL \r\n", TestValueName, intMoudleBtRssi, MinValue));
            }

            log.Info(string.Format("{0}：{1}，在设定范围：{2} ~ {3}之间 PASS \r\n", TestValueName, intMoudleBtRssi, MinValue, MaxValue));

        }
        /// <summary>
        /// 字符串转16进制字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        private static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// <summary>
        /// 字节数组转16进制字符串并反转
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string byteToHexStrReverse(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = bytes.Length - 1; i >= 0; i--)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        public void UartDisplay(byte[] by, string type)
        {
            string log = "";
            for (int i = 0; i < by.Length; i++)
            {
                log += by[i].ToString("X2") + " ";
            }
            if (type == "R")
            {
                this.log.Info("Rx:" + log);
            }
            else if (type == "T")
            {
                this.log.Info("Tx:" + log);
            }
            else
            {
                this.log.Info(log);
            }

        }
        /// <summary>
        /// 对同一数组内数据进行异或
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        private byte ByteToXOR(byte[] arr)
        {
            byte xor = 0;

            for (int i = 0; i < arr.Length; i++)
            {
                xor ^= arr[i];

            }
            //Console.WriteLine("0x{0:x}", xor);
            //Console.ReadKey();
            return xor;
        }

    }
}
