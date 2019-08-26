using HOTINST.COMMON.Win32;

namespace HOTINST.COMMON.Computer
{
    public static partial class MyComputer
    {
        /// <summary>
        /// 内存静态类
        /// </summary>
        public static class Memory
        {
            /// <summary>
            /// 获取物理内存大小 
            /// </summary>
            /// <returns>返回物理内存大小</returns>
            public static string TotalPhys()
            {
                return Win32Helper.TotalPhys();
            }

            /// <summary>
            /// 获取可使用的物理内存
            /// </summary>
            /// <returns>返回可使用的物理内存大小</returns>
            public static string AvailablePhys()
            {
                return Win32Helper.AvailablePhys();
            }

            /// <summary>
            /// 获取虚拟内存大小
            /// </summary>
            /// <returns>返回虚似内存大小</returns>
            public static string TotalVirtual()
            {
                return Win32Helper.TotalVirtual();
            }

            /// <summary>
            /// 获取可使用的虚拟内存大小
            /// </summary>
            /// <returns>返回可使用的虚拟内存大小</returns>
            public static string AvailableVirtual()
            {
                return Win32Helper.AvailableVirtual();
            }
        }
    }
}
