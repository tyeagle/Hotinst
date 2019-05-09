using System;
using System.Management;

namespace HOTINST.COMMON.Computer
{
    public static partial class MyComputer
    {
        /// <summary>
        /// CPU静态类
        /// </summary>
        public static class CPU
        {
            /// <summary>
            /// 获取当前计算机处理器个数
            /// </summary>
            /// <returns>返回处理器个数</returns>
            public static int ProcessorCount()
            {
                return Environment.ProcessorCount;
            }

            /// <summary>
            /// 判断当前计算机CPU是否为64位
            /// </summary>
            /// <returns>是64位返回true，不是返回false</returns>
            public static bool Is64BitProcess()
            {
                return Environment.Is64BitProcess;
            }

            /// <summary>
            /// 获取CPU序列号
            /// </summary>
            /// <returns>获取成功返回CPU序列号字符串，获取失败返回空字符串</returns>
            public static string GetCpuSerialNumber()
            {
                try
                {
                    ManagementClass objManagementClass = new ManagementClass("Win32_Processor");
                    ManagementObjectCollection objManagementObjectList = objManagementClass.GetInstances();

                    string strCpuSerialNumber = string.Empty;
                    foreach (ManagementObject objManagementObject in objManagementObjectList)
                        strCpuSerialNumber = objManagementObject.Properties["ProcessorId"].Value.ToString();

                    return strCpuSerialNumber;
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
