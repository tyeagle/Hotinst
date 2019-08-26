using System;
using System.Management;
using System.Net.NetworkInformation;
using Microsoft.Win32;

namespace HOTINST.COMMON.License
{
	/// <summary>
	/// 
	/// </summary>
    public class ComputerInfo
    {
		/// <summary>
		/// 获取计算机标识
		/// </summary>
		/// <returns></returns>
        public string GetComputerIdentify()
        {
            string info = string.Empty;
            string cpu = GetCPUInfo();
            //string disk = GetHardWareInfo("Win32_DiskDrive", "Model");
            string baseBoard = GetHardWareInfo("Win32_BaseBoard", "SerialNumber");
            string bios = GetHardWareInfo("Win32_BIOS", "SerialNumber");
            //string mac = GetMACInfo();

            info = string.Concat(cpu, baseBoard, bios);
            return info;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        public static string GetCPUInfo()
        {
            string info = string.Empty;
            info = GetHardWareInfo("Win32_Processor", "ProcessorId");
            return info;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        public static string GetMACInfo()
        {
            string result = string.Empty;
            try
            {
                ManagementClass managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection mn = managementClass.GetInstances();

                foreach (ManagementObject m in mn)
                {
                    if ((bool) m["IPEnabled"] == true)
                    {
                        result += m.Properties["MacAddress"].Value.ToString();
                    }
                }
            }
            catch (Exception)
            {
                //这里写异常的处理  
            }
            return result;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="typePath"></param>
		/// <param name="key"></param>
		/// <returns></returns>
        public static string GetHardWareInfo(string typePath, string key)
        {
            string result = string.Empty;
            try
            {
                ManagementClass managementClass = new ManagementClass(typePath);
                ManagementObjectCollection mn = managementClass.GetInstances();

                foreach (ManagementObject m in mn)
                {
                    result = m.Properties[key].Value.ToString();
                }
                
            }
            catch (Exception)
            {
                //这里写异常的处理  
            }
            return result;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        public static string GetMacAddressByNetworkInformation()
        {
            string key = "SYSTEM\\CurrentControlSet\\Control\\Network\\{4D36E972-E325-11CE-BFC1-08002BE10318}\\";
            string macAddress = string.Empty;
            try
            {
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface adapter in nics)
                {
                    if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet
                        && adapter.GetPhysicalAddress().ToString().Length != 0)
                    {
                        string fRegistryKey = key + adapter.Id + "\\Connection";
                        RegistryKey rk = Registry.LocalMachine.OpenSubKey(fRegistryKey, false);
                        if (rk != null)
                        {
                            string fPnpInstanceID = rk.GetValue("PnpInstanceID", "").ToString();
                            //int fMediaSubType = Convert.ToInt32(rk.GetValue("MediaSubType", 0));
                            if (fPnpInstanceID.Length > 3 &&
                                fPnpInstanceID.Substring(0, 3) == "PCI")
                            {
                                macAddress = adapter.GetPhysicalAddress().ToString();
                                for (int i = 1; i < 6; i++)
                                {
                                    macAddress = macAddress.Insert(3 * i - 1, ":");
                                }
                                break;
                            }
                        }

                    }
                }
            }
            catch (Exception)
            {
                //这里写异常的处理  
            }
            return macAddress;
        }


    }
}
