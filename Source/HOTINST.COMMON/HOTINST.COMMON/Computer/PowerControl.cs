using System.Diagnostics;
using HOTINST.COMMON.Win32;

namespace HOTINST.COMMON.Computer
{
	/// <summary>
	/// 
	/// </summary>
    public static class PowerControl
    {
		#region Publice

        /// <summary>
        /// 关机
        /// </summary>
        public static void ShutDown()
        {
            Process.Start("shutdown", "/s /t 0");
        }

        /// <summary>
        /// 重启
        /// </summary>
        public static void ReStart()
        {
            Process.Start("shutdown", "/r /t 0");
        }

        /// <summary>
        /// 注销
        /// </summary>
        public static bool LoginOut()
        {
            return Win32API.ExitWindowsEx(0, 0);
        }

        /// <summary>
        /// 锁定用户
        /// </summary>
        public static void LockCurentWorkStation()
        {
			Win32API.LockWorkStation();
        }

        /// <summary>
        /// 休眠
        /// </summary>
        public static bool Hibernates()
        {
            return Win32API.SetSuspendState(true, true, true);
        }

        /// <summary>
        /// 睡眠
        /// </summary>
        public static bool Suspended()
        {
            return Win32API.SetSuspendState(false, true, true);
        }
		
		#endregion
    }
}