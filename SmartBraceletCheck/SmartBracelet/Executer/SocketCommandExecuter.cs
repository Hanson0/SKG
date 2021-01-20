using AILinkFactoryAuto.Core;
using AILinkFactoryAuto.Dut.AtCommand;
using AILinkFactoryAuto.Task.Executer;
using AILinkFactoryAuto.Task.Property;
using AILinkFactoryAuto.Task.SmartBracelet.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace AILinkFactoryAuto.Task.SmartBracelet.Executer
{
    public class SocketCommandExecuter : IExecuter
    {
        private string pattern;

        private GlobalVaribles configGv;

        public SocketCommandExecuter()
        {
            this.pattern = GlobalVaribles.PATTERN;
        }

        private string PreTranslateAtCommand(string atCommand)
        {
            string newAtCommand = atCommand;
            Match match = Regex.Match(atCommand, @"{([0-9a-zA-Z_]+)}");
            for (int i = 1; i < match.Groups.Count; i++)
            {
                string key = match.Groups[i].ToString();
                string value = configGv.Get(key);

                if (string.IsNullOrEmpty(value))
                {
                    throw new BaseException("null value in GlobalVaribles");
                }

                newAtCommand = newAtCommand.Replace("{" + key + "}", value);
            }

            return newAtCommand;
        }

        private string PreTranslateCheckInfo(string checkInfo)
        {
            string newCheckInfo = checkInfo;
            Match match = Regex.Match(checkInfo, pattern);
            for (int i = 1; i < match.Groups.Count; i++)
            {
                string key = match.Groups[i].ToString();
                newCheckInfo = configGv.Get(key);
            }

            return newCheckInfo;
        }

        private string Parse_r_n(string value)
        {
            value = value.Replace("\\r", "\r");
            value = value.Replace("\\R", "\r");
            value = value.Replace("\\n", "\n");
            value = value.Replace("\\N", "\n");
            return value;
        }

        public void Run(IProperties properties, GlobalDic<string, object> globalDic)
        {
            SocketCommandProperties config = properties as SocketCommandProperties;
            ILog log = globalDic[typeof(ILog).ToString()] as ILog;
            configGv = globalDic[typeof(GlobalVaribles).ToString()] as GlobalVaribles;

            Socket objSocket = configGv.GetObject("ObjSocket") as Socket;

            if (objSocket == null)
            {
                throw new BaseException(string.Format("objSocket is null"));
            }

            if (config.CommandType == SocketCommandProperties.EnumCommandType.String)
            {
                string atCommand = PreTranslateAtCommand(config.SocketCommand);
                log.Info("SocketCommand:\r\n" + atCommand);
                byte[] bytesCommand = Encoding.ASCII.GetBytes(atCommand);
                objSocket.Send(bytesCommand, bytesCommand.Length, SocketFlags.None);
                Thread.Sleep(config.SocketCommandInterval);

                byte[] data = new byte[objSocket.ReceiveBufferSize];
                int len = objSocket.Receive(data);
                string response = Encoding.ASCII.GetString(data, 0, len); 

                log.Info("Socket Response:\r\n" + response);

                if (!string.IsNullOrEmpty(config.SocketCommandError))
                {
                    if (response.Contains(config.SocketCommandError))
                    {
                        throw new BaseException(string.Format("contain error:[{0}]", config.SocketCommandError));
                    }
                }

                if (!string.IsNullOrEmpty(config.SocketCommandOk))
                {
                    if (!response.Contains(config.SocketCommandOk))
                    {
                        throw new BaseException(string.Format("not contain ok:[{0}]",config.SocketCommandOk));
                    }
                    log.Info(string.Format("contain ok:[{0}]", config.SocketCommandOk));
                }

                if (config.CheckInfo != null)
                {
                    for (int i = 0; i < config.CheckInfo.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(config.CheckInfo[i]))
                        {
                            string checkInfo = PreTranslateCheckInfo(config.CheckInfo[i]);
                            if (!response.Contains(checkInfo))
                            {
                                throw new BaseException(string.Format("not contain check info:[{0}]", checkInfo));
                            }
                            log.Info(string.Format("contain check info:[{0}]", checkInfo));
                        }
                    }
                }

                if (config.GlobalVariblesKey != null && config.GlobalVariblesKeyPattern != null)
                {
                    if (config.GlobalVariblesKey.Length != config.GlobalVariblesKeyPattern.Length)
                    {
                        throw new BaseException("请保持GlobalVariblesKey与GlobalVariblesKeyPattern成对存在");
                    }
                    for (int i = 0; i < config.GlobalVariblesKey.Length; i++)
                    {
                        string pattern = Parse_r_n(config.GlobalVariblesKeyPattern[i]);
                        Match matchValue = Regex.Match(response, pattern);
                        if (!matchValue.Success)
                        {
                            throw new BaseException("read info value fail");
                        }

                        string value = matchValue.Groups[1].ToString();

                        Match matchKey = Regex.Match(config.GlobalVariblesKey[i], this.pattern);
                        if (!matchKey.Success)
                        {
                            throw new BaseException("read info key fail");
                        }

                        string key = matchKey.Groups[1].ToString();

                        configGv.Add(key, value);
                    }
                }

            }
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
    }
}
