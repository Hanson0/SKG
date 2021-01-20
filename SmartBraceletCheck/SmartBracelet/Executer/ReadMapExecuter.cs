using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Core.HttpClient;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using AILinkFactoryAuto.Task.SmartBracelet.Property;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AILinkFactoryAuto.Task.SmartBracelet.Executer
{
    public class ReadMapExecuter : IExecuter
    {
        ReadMapProperties config;
        private GlobalVaribles configGv;
        public void Run(IProperties properties, GlobalDic<string, object> globalDic)//virtual
        {
            config = properties as ReadMapProperties;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;
            //int ret = -1;
            string binFileString = "";
            //查询MAC对应的bin文件
            //第一种方法
            string mac = configGv.Get(GlobalVaribles.MAC);
            try
            {
                var files = Directory.GetFiles(config.BinFolderPath, "*.bin");
                foreach (var file in files)
                {
                    if (file.Contains(mac))
                    {
                        binFileString = ReadBinery(file);
                        break;
                    }
                }
                configGv.Add("BinFileString", binFileString);
                if (string.IsNullOrEmpty(binFileString))
                {
                    throw new Exception(string.Format("读取License文件失败,未读到该MAC：{0}的binFile", mac));
                }

                #region 从服务器download文件下来 http://10.42.1.4:9999/midea_hi3861_license/847C9B7E6287.bin
                //HttpTools.Download("http://10.42.1.4:9999/midea_hi3861_license/847C9B7E6.bin", "E:/testDownLoadBin/847C9B7E6287.bin");

                #endregion


            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("读取License文件失败,异常：{0}", ex.Message));
            }
            log.Info(string.Format("读取License文件成功：\r\n{0}", binFileString));

            //try
            //{
            //    binFileString = Read(config.MapFilePath);
            //}
            //catch (Exception ex)
            //{

            //    throw new Exception(string.Format("读取License文件失败,异常：{0}", ex.Message));
            //}

            //log.Info(string.Format("读取License文件成功：\r\n{0}", binFileString));
            ////更新MAP内存中的MAC地址
            //string[] splits = { "\r\n" };
            //string[] mapFileStringLines = mapFileString.Split(splits, StringSplitOptions.RemoveEmptyEntries);

            ////IMES获取的MAC***************
            //configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;
            //string macFromImes = configGv.Get("MAC");
            //string reverseMacAddSpace = AddSpaceEachTwoBtye(macFromImes);
            //string mapFileStringMacUpdate = "";
            //for (int i = 0; i < mapFileStringLines.Length; i++)
            //{
            //    if (i == 17)
            //    {
            //        //前面固定部分+反转并添加空格的MAC
            //        mapFileStringLines[i] = mapFileStringLines[i].Substring(0, 25) + reverseMacAddSpace;
            //    }
            //    mapFileStringMacUpdate += mapFileStringLines[i] + "\r\n";
            //}

            ////存为byte数组应该会好些，这样可以定位地址，当MAC的偏移地址发生变化时,只需改地址值
            //log.Info(string.Format("更新MAP文件中的MAC成功：\r\n{0}", mapFileStringMacUpdate));
            //GlobalVaribles configGv1 = globalDic.Get<GlobalVaribles>();
            //configGv1.Add("MapFileStringMacUpdate", mapFileStringMacUpdate);

        }
        /// <summary>
        /// 字符串每两个字节添加空格
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static string AddSpaceEachTwoBtye(string strValue)
        {
            for (int i = 4; i < strValue.Length; i = i + 4)
            {
                strValue = strValue.Insert(i, " ");
                i++;
            }
            return strValue;
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
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string byteToHexStr(byte[] bytes)
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
        /// <summary>
        /// 字节数组高低字节互换
        /// </summary>
        /// <param name="bytesValue"></param>
        /// <returns></returns>
        public static byte[] bytesConvert(byte[] bytesValue)
        {
            byte[] data2 = new byte[bytesValue.Length];
            for (int i = 0; i < bytesValue.Length; i += 2)
            {
                data2[i] = bytesValue[i + 1];
                data2[i + 1] = bytesValue[i];
            }
            return data2;
        }

        /// <summary>
        /// 字符串高低位字节互换
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static string strConvert(string strValue)
        {
            int intLength = strValue.Length;
            string res = string.Empty;
            for (int i = 0; i < intLength / 2; i++)
            {
                res += strValue.Substring(intLength - 2 * (i + 1), 2);
            }
            return res;
        }

        public string Read(string path)
        {
            try
            {
                StreamReader sr = new StreamReader(path, Encoding.Default);
                String line;
                String fileContent = "";
                while ((line = sr.ReadLine()) != null)
                {
                    fileContent += line + "\r\n";
                }
                sr.Dispose();
                sr.Close();
                return fileContent;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public string ReadBinery(string path)
        {
            try
            {
                //FileStream stream = new FileStream(path, FileMode.Open);
                //byte[] data = new byte[stream.Length];   //stream.Length 是字节类型的长度
                //num = stream.Read(data, 0, (int)(stream.Length));   //读取数据到data数组
                //stream.Flush();
                //stream.Close();
                ////serialPort1.Write(data, 0, num); //如果有串口的话，这个就可以打印


                //FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                //UInt32 imageSize = (UInt32)fs.Length;
                //BinaryReader br = new BinaryReader(fs);

                //br.ToString();
                //string str = br.ReadString();
                //int a = br.BaseStream.ReadByte();


                FileStream inStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                long nBytesToRead = inStream.Length;
                byte[] buffer = new byte[nBytesToRead];
                int m = inStream.Read(buffer, 0, buffer.Length);
                inStream.Close();
                string a = "";
                //显示到richtextbox1 控件中，并且用  隔开
                //richTextBox1.Clear();
                for (int i = 0; i < buffer.Length; i++)
                {
                    a += buffer[i].ToString("X2") + " ";
                    if (((i+1)/16)*16==i+1)
                    {
                        a += "\r\n";
                    }
                }

                a = a.ToLower();

                return a;
                //return br.ToString();
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// 将二进制转成字符串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string jiema(string s)
        {
            System.Text.RegularExpressions.CaptureCollection cs =
                System.Text.RegularExpressions.Regex.Match(s, @"([01]{8})+").Groups[1].Captures;
            byte[] data = new byte[cs.Count];
            for (int i = 0; i < cs.Count; i++)
            {
                data[i] = Convert.ToByte(cs[i].Value, 2);
            }
            return Encoding.Unicode.GetString(data, 0, data.Length);
        }

    }
}
