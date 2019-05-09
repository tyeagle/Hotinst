using HOTINST.COMMON.Data;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace HOTINST.COMMON.Computer
{
    public static partial class MyComputer
    {
        /// <summary>
        /// 网络静态类
        /// </summary>
        public static class Network
        {
            /// <summary>
            /// 获取本地计算机的NetBIOS名称
            /// </summary>
            /// <returns>返回NetBIOS名称</returns>
            public static string NetBIOSName()
            {
                return Environment.MachineName;
            }

            /// <summary>
            /// 指示是否有任何可用的网络连接
            /// </summary>
            /// <returns>有可用的网络返回true,无可用的网络返回false</returns>
            public static bool IsAvailable()
            {
                return NetworkInterface.GetIsNetworkAvailable();
            }

            /// <summary>
            /// 获取指定域名的IP地址
            /// </summary>
            /// <param name="DomainName">域名（主机名）</param>
            /// <returns>返回IP地址。传入参数为空字符串或获取异常时返回空字符串。</returns>
            public static string GetIPByDomainName(string DomainName)
            {
                DomainName = DomainName.Trim();
                if (string.IsNullOrWhiteSpace(DomainName))
                    return string.Empty;

                try
                {
                    IPHostEntry host = Dns.GetHostEntry(DomainName);
                    return host.AddressList.GetValue(0).ToString();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.Message);
                    return string.Empty;
                }
            }

            /// <summary>
            /// 获取指定IP的域名（主机名）
            /// </summary>
            /// <param name="IPAddressString">指定IP地址</param>
            /// <returns>获取成功返回主机名，获取失败返回空字符串</returns>
            public static string GetDomainNameByIP(string IPAddressString)
            {
                IPAddressString = IPAddressString.Trim();
                if (IPAddressString.Length < 1)
                    return string.Empty;

                if (!StringFormat.IsIPAddressString(IPAddressString))
                    return string.Empty;

                try
                {
                    return Dns.GetHostEntry(IPAddressString).HostName;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.Message);
                    return string.Empty;
                }
            }

            /// <summary>
            /// Ping指定的IP或域名（主机名）
            /// </summary>
            /// <param name="IPAddressOrDomainName">IP地址或域名（主机名）</param>
            /// <param name="DelayTime">网络延时（传出参数）。用以返回Ping包往返行程的时间</param>
            /// <returns>Ping成功返回true。参数错误、不支持、Ping异常及Ping失败等均返回false</returns>
            public static bool NetPing(string IPAddressOrDomainName, out long DelayTime)
            {
                DelayTime = 0;

                IPAddressOrDomainName = IPAddressOrDomainName.ToUpper().Trim();
                if (IPAddressOrDomainName.Trim().Length < 1)
                    return false;

                if (IPAddressOrDomainName == "(LOCAL)" || IPAddressOrDomainName == "LOCALHOST" || IPAddressOrDomainName == "." || IPAddressOrDomainName == "LOCAL")
                    IPAddressOrDomainName = "127.0.0.1";

                try
                {
                    Ping objPing = new Ping();
                    PingReply objPingReply = objPing.Send(IPAddressOrDomainName);
                    if (objPingReply.Status == IPStatus.Success)
                    {
                        DelayTime = objPingReply.RoundtripTime;
                        return true;
                    }
                    
                    return false;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.Message);
                    return false;
                }
            }

            /// <summary>
            /// 获取本机所有IPV4地址
            /// </summary>
            /// <returns>返回含有IPV4地址的字符串数组</returns>
            public static string[] GetAllIpv4Adress()
            {
                List<string> strIPAddressList = new List<string>();

                IPAddress[] objIPAddressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
                foreach (IPAddress objIPAddress in objIPAddressList)
                {
                    //根据AddressFamily判断是否为ipv4
                    if (objIPAddress.AddressFamily == AddressFamily.InterNetwork)
                        strIPAddressList.Add(objIPAddress.ToString());
                }
                return strIPAddressList.ToArray();
            }

            /// <summary>
            /// 获取本机所有网卡的MAC地址
            /// </summary>
            /// <returns>返回含有MAC地址的字符串数组</returns>
            public static string[] GetAllMacAddress()
            {
                List<string> strNetworkInterfaceMacList = new List<string>();
                NetworkInterface[] objNetworkInterfaceList = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface objNetworkInterface in objNetworkInterfaceList)
                    strNetworkInterfaceMacList.Add(objNetworkInterface.GetPhysicalAddress().ToString());
                return strNetworkInterfaceMacList.ToArray();
            }

            /// <summary>
            /// 获取本机所有网卡标识符
            /// </summary>
            /// <returns>返回含有网卡名称的字符串数组</returns>
            public static string[] GetAllNetworkInterfaceId()
            {
                List<string> strNetworkInterfaceIdList=new List<string>();
                NetworkInterface[] objNetworkInterfaceList = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface objNetworkInterface in objNetworkInterfaceList)
                    strNetworkInterfaceIdList.Add(objNetworkInterface.Id);
                return strNetworkInterfaceIdList.ToArray();
            }

            /// <summary>
            /// 获取网卡标识符指定网卡的网卡名称
            /// </summary>
            /// <param name="NetworkInterfaceId">网卡标识符</param>
            /// <returns>存在网卡标识符指定的网卡则返回网卡名称。其它返回空字符串</returns>
            public static string GetNetworkInterfaceNameById(string NetworkInterfaceId)
            {
                if (string.IsNullOrWhiteSpace(NetworkInterfaceId))
                    return string.Empty;

                NetworkInterface[] objNetworkInterfaceList = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface objNetworkInterface in objNetworkInterfaceList)
                {
                    if (objNetworkInterface.Id == NetworkInterfaceId)
                        return objNetworkInterface.Name;
                }
                return string.Empty;
            }

            /// <summary>
            /// 获取网卡标识符指定网卡的MAC地址
            /// </summary>
            /// <param name="NetworkInterfaceId">网卡标识符</param>
            /// <returns>存在网卡标识符指定的网卡则返回其MAC地址。其它返回空字符串</returns>
            public static string GetMacAddressById(string NetworkInterfaceId)
            {
                if (string.IsNullOrWhiteSpace(NetworkInterfaceId))
                    return string.Empty;

                NetworkInterface[] objNetworkInterfaceList = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface objNetworkInterface in objNetworkInterfaceList)
                {
                    if (objNetworkInterface.Id == NetworkInterfaceId)
                        return objNetworkInterface.GetPhysicalAddress().ToString();
                }
                return string.Empty;
            }

            /// <summary>
            /// 获取网卡标识符指定网卡的IPV4地址
            /// </summary>
            /// <param name="NetworkInterfaceId">网卡标识符</param>
            /// <returns>存在网卡标识符指定的网卡且获取到IPV4地址时返回IPV4地址。其它返回空字符串</returns>
            public static string GetIPV4AddressById(string NetworkInterfaceId)
            {
                if (string.IsNullOrWhiteSpace(NetworkInterfaceId))
                    return string.Empty;

                NetworkInterface[] objNetworkInterfaceList = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface objNetworkInterface in objNetworkInterfaceList)
                {
                    if (objNetworkInterface.Id == NetworkInterfaceId)
                    {
                        UnicastIPAddressInformationCollection objUnicastIPAddressInformationCollectionList = objNetworkInterface.GetIPProperties().UnicastAddresses;
                        foreach (UnicastIPAddressInformation objUnicastIPAddressInformation in objUnicastIPAddressInformationCollectionList)
                        {
                            if (objUnicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                                return objUnicastIPAddressInformation.Address.ToString();
                        }
                    }
                }
                return string.Empty;
            }

            /// <summary>
            /// 获取本地计算机的主机名
            /// </summary>
            /// <returns>返回主机名</returns>
            public static string GetHostName()
            {
                try
                {
                    return Dns.GetHostName();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.Message);
                    return string.Empty;
                }
            }
        }
    }
}
