using System;
using System.Management;

namespace HOTINST.COMMON.Computer
{
    public static partial class MyComputer
    {
        /// <summary>
        /// 存储设备静态类
        /// </summary>
        public static class Storage
        {
            /// <summary>
            /// 获取本机硬盘序列号
            /// </summary>
            /// <returns>获取成功返回硬盘序列号，获取失败返回空字符串</returns>
            public static string GetHardDiskSerialNumber()
            {
                try
                {
                    ManagementClass objManagementClass = new ManagementClass("Win32_DiskDrive");
                    ManagementObjectCollection objManagementObjectList = objManagementClass.GetInstances();

                    string strHardDiskSerialNumber = string.Empty;
                    foreach (ManagementObject objManagementObject in objManagementObjectList)
                        strHardDiskSerialNumber = objManagementObject.Properties["Model"].Value.ToString();

                    return strHardDiskSerialNumber;
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
