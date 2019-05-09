using System;
using System.Management;
using HOTINST.COMMON.Win32;

namespace HOTINST.COMMON.Computer
{
    public static partial class MyComputer
    {
        /// <summary>
        /// 操作系统静态类
        /// </summary>
        public static class OS
        {
            /// <summary>
            /// 获取当前操作系统平台类型
            /// </summary>
            /// <returns>返回平台标识符</returns>
            /// <remarks>
            /// MacOSX为Macintosh系统；
            /// Unix为Unix系统；
            /// Win32NT为Windows NT或较新的系统；
            /// Win32S为Windows 32位子集的系统；
            /// Win32Windows为Windows 95或Windows 98；
            /// WinCE为Windows CE系统；
            /// Xbox为Xbox 360平台。
            /// </remarks>
            public static string OSPlatform()
            {
                switch (Environment.OSVersion.Platform)
                {
                    case PlatformID.MacOSX:
                        return "MacOSX";
                    case PlatformID.Unix:
                        return "Unix";
                    case PlatformID.Win32NT:
                        return "Win32NT";
                    case PlatformID.Win32S:
                        return "Win32S";
                    case PlatformID.Win32Windows:
                        return "Win32Windows";
                    case PlatformID.WinCE:
                        return "WinCE";
                    case PlatformID.Xbox:
                        return "Xbox";
                    default:
                        return "Unknown";
                }
            }

            /// <summary>
            /// 获取当前操作系统的版本号
            /// </summary>
            /// <returns>返回操作系统版本号</returns>
            public static string OSVersion()
            {
                Version OSVer = Environment.OSVersion.Version;
                return string.Format("{0}.{1}.{2}内部版本{3}", OSVer.Major, OSVer.Minor, OSVer.Revision, OSVer.Build);
            }

            /// <summary>
            /// 获取当前操作系统的Service Pack版本
            /// </summary>
            /// <returns>返回Service Pack版本</returns>
            public static string OSServicePack()
            {
                return Environment.OSVersion.ServicePack;
            }

            /// <summary>
            /// 获取当前操作系统的版本相关信息
            /// </summary>
            /// <returns>返回平台标识符、版本和当前安装在操作系统上的Service Pack的连接字符串</returns>
            public static string OSAllVersionInfo()
            {
                return Environment.OSVersion.VersionString;
            }

            /// <summary>
            /// 确定当前操作系统是否为64位操作系统
            /// </summary>
            /// <returns>64位操作系统返回true,否则返回false</returns>
            public static bool Is64BitOS()
            {
                return Environment.Is64BitOperatingSystem;
            }

            /// <summary>
            /// 获取当前操作系统目录的完全限定路径
            /// </summary>
            /// <returns>返回系统目录路径字符串</returns>
            public static string SystemDirectory()
            {
                return Environment.SystemDirectory;
            }

            /// <summary>
            /// 获取当前操作系统页面文件的内存量
            /// </summary>
            /// <returns>返回内存量</returns>
            public static string SystemPageSize()
            {
                return Environment.SystemPageSize.ToString();
            }

            /// <summary>
            /// 设置本地系统时间
            /// </summary>
            /// <param name="NewDateTime">待设置的新时间</param>
            /// <returns>设置成功返回true，设置失败返回false</returns>
            /// <remarks>在vista、win7或更高版本的系统中，调用此功能的应用程
            /// 序需要有管理员权限（以管理员身份运行）才能成功执行此功能</remarks>
            public static bool SetLocalTime(DateTime NewDateTime)
            {
                return Win32Helper.SetLocalTime(NewDateTime);
            }

            /// <summary> 
            /// 获取操作系统当前登录用户的用户名
            /// </summary> 
            /// <returns>获取成功返回用户名，获取失败返回空字符串</returns> 
            public static string GetUserName()
            {
                try
                {
                    ManagementClass objManagementClass = new ManagementClass("Win32_ComputerSystem");
                    ManagementObjectCollection objManagementObjectList = objManagementClass.GetInstances();

                    string strUserName = string.Empty;
                    foreach (ManagementObject objManagementObject in objManagementObjectList)
                        strUserName = objManagementObject["UserName"].ToString();

                    return strUserName;
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
