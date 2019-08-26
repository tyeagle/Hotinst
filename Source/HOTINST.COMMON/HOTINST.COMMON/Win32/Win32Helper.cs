/********************************************************************
 * 类 名 称：Win32Helper
 * 命名空间：HOTINST.COMMON.Win32API
 * 文 件 名：Win32Helper.cs
 * 创建时间：2016-4-12
 * 作    者：汪锋
 * 说    明：基于Win32API调用编写的一些实用的函数库
 * 待 补 充: 可根据需要继续扩展各种依赖Win32API调用的实用函数
 * 修改时间：
 * 修 改 人：
 *           2016-5-4 汪锋 1.增加键盘CAPS Lock/Num Lock/Scroll Lock
 *                         等的状态获取函数；
 *                         2.增加获取物理内存大小/可用大小，虚拟内
 *                         存大小/可用大小的四个获取函数。
 ********************************************************************/

using System;

namespace HOTINST.COMMON.Win32
{
	/// <summary>
	/// 
	/// </summary>
    public static class Win32Helper
    {
        /// <summary>
        /// 转让控制权
        /// </summary>
        /// <remarks>实现类似C#中的Application.DoEvents()功能</remarks>
        public static void DoEvents()
        {
            MSG msg = new MSG { hwnd = IntPtr.Zero, message = 0, wParam = IntPtr.Zero, lParam = IntPtr.Zero, time = 0, pt_x = 0, pt_y = 0 }; //定义一个MSG类型的变量

            while (Win32API.PeekMessage(ref msg, 0, 0, 0, (uint)PeekMessageFlags.PM_REMOVE)) //获取消息并把该消息从消息队列中移除（防止重复响应）
            {
                Win32API.DispatchMessage(ref msg);  //将消息移交给过程函数
                Win32API.TranslateMessage(ref msg); //翻译消息 在合适的机会产生char消息
            }
        }

        /// <summary>
        /// 延时函数（不卡死调用线程，CPU占用率高）
        /// </summary>
        /// <param name="DelayMillisecond">延时毫秒数</param>
        public static void Delay(int DelayMillisecond)
        {
            int CurrentMillisecond = Win32API.GetTickCount();
            do
            {
                DoEvents();
            } while (Win32API.GetTickCount() < CurrentMillisecond + DelayMillisecond);
        }

        /// <summary>
        /// 延时函数（会卡死调用线程，CPU占用几乎为零，适合单独线程中使用）
        /// </summary>
        /// <param name="DelayMillisecond">延时毫秒数</param>
        public static void DelayS(int DelayMillisecond)
        {
            int CurrentMillisecond = Win32API.GetTickCount();
            do
            {
                Win32API.Sleep(1);
            } while (Win32API.GetTickCount() < CurrentMillisecond + DelayMillisecond);
        }

        /// <summary>
        /// 延时函数（不卡死调用线程，CPU占用率低，适合主线程中使用）
        /// </summary>
        /// <param name="DelayMillisecond">延时毫秒数。不得低于2毫秒</param>
        public static void DelayEx(int DelayMillisecond)
        {
            if (DelayMillisecond < 10)
                return;

            int CurrentMillisecond = Win32API.GetTickCount();
            do
            {
                DoEvents();
                Win32API.Sleep(1);
            } while (Win32API.GetTickCount() < CurrentMillisecond + DelayMillisecond);
        }

        /// <summary>
        /// 当前线程休眠一段时间
        /// </summary>
        /// <param name="Millisecond">指定休眠时间。单位:毫秒</param>
        public static void Sleep(int Millisecond)
        {
            Win32API.Sleep(Millisecond);
        }

        /// <summary>
        /// 获取Caps Lock状态。
        /// </summary>
        /// <returns>true开启，false关闭</returns>
        public static bool CapsLock()
        {
            return (((ushort)Win32API.GetKeyState((int)VirtualKeys.VK_CAPITAL)) & 0xffff) != 0;
        }

        /// <summary>
        /// 获取Num Lock状态。
        /// </summary>
        /// <returns>true开启，false关闭</returns>
        public static bool NumLock()
        {
            return (((ushort)Win32API.GetKeyState((int)VirtualKeys.VK_NUMLOCK)) & 0xffff) != 0;
        }

        /// <summary>
        /// 获取Scroll Lock状态。
        /// </summary>
        /// <returns>true开启，false关闭</returns>
        public static bool ScrollLock()
        {
            return (((ushort)Win32API.GetKeyState((int)VirtualKeys.VK_SCROLL)) & 0xffff) != 0;
        }

        /// <summary>
        /// 获取SHIFT键是否按下状态。
        /// </summary>
        /// <returns>true已按下，false未按下</returns>
        public static bool ShiftKeyDown()
        {
            return ((ushort)Win32API.GetAsyncKeyState((int)VirtualKeys.VK_SHIFT) & 0x8000) == 1 ? true : false;
        }

        /// <summary>
        /// 获取SHIFT键是否弹起状态。
        /// </summary>
        /// <returns>true已弹起，false未弹起</returns>
        public static bool ShiftKeyUp()
        {
            return ((ushort)Win32API.GetAsyncKeyState((int)VirtualKeys.VK_SHIFT) & 0x8000) == 0 ? true : false;
        }

        /// <summary>
        /// 获取CTRL键是否按下状态。
        /// </summary>
        /// <returns>true已按下，false未按下</returns>
        public static bool CtrlKeyDown()
        {
            return ((ushort)Win32API.GetAsyncKeyState((int)VirtualKeys.VK_CONTROL) & 0x8000) == 1 ? true : false;
        }

        /// <summary>
        /// 获取CTRL键是否弹起状态。
        /// </summary>
        /// <returns>true已弹起，false未弹起</returns>
        public static bool CtrlKeyUp()
        {
            return ((ushort)Win32API.GetAsyncKeyState((int)VirtualKeys.VK_CONTROL) & 0x8000) == 0 ? true : false;
        }

        /// <summary>
        /// 获取ALT键是否按下状态。
        /// </summary>
        /// <returns>true已按下，false未按下</returns>
        public static bool AltKeyDown()
        {
            return ((ushort)Win32API.GetAsyncKeyState((int)VirtualKeys.VK_MENU) & 0x8000) == 1 ? true : false;
        }

        /// <summary>
        /// 获取ALT键是否弹起状态。
        /// </summary>
        /// <returns>true已弹起，false未弹起</returns>
        public static bool AltKeyUp()
        {
            return ((ushort)Win32API.GetAsyncKeyState((int)VirtualKeys.VK_MENU) & 0x8000) == 0 ? true : false;
        }

        /// <summary>
        /// 获取物理内存大小 
        /// </summary>
        /// <returns>返回物理内存大小</returns>
        public static string TotalPhys()
        {
            MEMORY_INFO meminfo = new MEMORY_INFO();
            Win32API.GlobalMemoryStatus(ref meminfo);
            return (meminfo.dwTotalPhys / 1024 / 1024).ToString() + "MB";
        }

        /// <summary>
        /// 获取可使用的物理内存
        /// </summary>
        /// <returns>返回可使用的物理内存大小</returns>
        public static string AvailablePhys()
        {
            MEMORY_INFO meminfo = new MEMORY_INFO();
            Win32API.GlobalMemoryStatus(ref meminfo);
            return (meminfo.dwAvailPhys / 1024 / 1024).ToString() + "MB";
        }

        /// <summary>
        /// 获取虚拟内存大小
        /// </summary>
        /// <returns>返回虚似内存大小</returns>
        public static string TotalVirtual()
        {
            MEMORY_INFO meminfo = new MEMORY_INFO();
            Win32API.GlobalMemoryStatus(ref meminfo);
            return (meminfo.dwTotalVirtual / 1024 / 1024).ToString() + "MB";
        }

        /// <summary>
        /// 获取可使用的虚拟内存大小
        /// </summary>
        /// <returns>返回可使用的虚拟内存大小</returns>
        public static string AvailableVirtual()
        {
            MEMORY_INFO meminfo = new MEMORY_INFO();
            Win32API.GlobalMemoryStatus(ref meminfo);
            return (meminfo.dwAvailVirtual / 1024 / 1024).ToString() + "MB";
        }
        
        /// <summary>
        /// 获取硬盘序列号
        /// </summary>
        /// <param name="DriveChar">硬盘盘符[c|d|e|....]</param>
        /// <returns>返回十六进制形式的硬盘序列号字符串</returns>
        public static string GetDiskSerialNumber(string DriveChar)
        {
            const int MAX_FILENAME_LEN = 256;
            int intSerialNumber = 0;
            Win32API.GetVolumeInformation(DriveChar + @":\", null, MAX_FILENAME_LEN, ref intSerialNumber, 0, 0, null, MAX_FILENAME_LEN);
            return intSerialNumber.ToString("X");
        }

        /// <summary>
        /// 设置本地系统时间
        /// </summary>
        /// <param name="NewDateTime">待设置的新时间。格式为"yyyy-MM-dd hh:mm:ss"</param>
        /// <returns>设置成功返回true，设置失败返回false</returns>
        /// <remarks>在vista、win7或更高版本的系统中，调用此功能的应用程
        /// 序需要有管理员权限（以管理员身份运行）才能成功执行此功能</remarks>
        public static bool SetLocalTime(string NewDateTime)
        {
            if (string.IsNullOrWhiteSpace(NewDateTime))
                return false;
            DateTime struDateTime;
            if (!DateTime.TryParse(NewDateTime, out struDateTime))
                return false;

            string strSetDateTime = struDateTime.ToString("yyyy-MM-dd hh:mm:ss");
            SystemTime struSystemTime = new SystemTime();
            struSystemTime.wYear = Convert.ToUInt16(strSetDateTime.Substring(0, 4));
            struSystemTime.wMonth = Convert.ToUInt16(strSetDateTime.Substring(5, 2));
            struSystemTime.wDay = Convert.ToUInt16(strSetDateTime.Substring(8, 2));
            struSystemTime.wHour = Convert.ToUInt16(strSetDateTime.Substring(11, 2));
            struSystemTime.wMinute = Convert.ToUInt16(strSetDateTime.Substring(14, 2));
            struSystemTime.wSecond = Convert.ToUInt16(strSetDateTime.Substring(17, 2));

            try
            {
                return Win32API.SetLocalTime(ref struSystemTime);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
                return false;
            }

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
            SystemTime struSystemTime = new SystemTime();
            struSystemTime.wYear = Convert.ToUInt16(NewDateTime.Year);
            struSystemTime.wMonth = Convert.ToUInt16(NewDateTime.Month);
            struSystemTime.wDay = Convert.ToUInt16(NewDateTime.Day);
            struSystemTime.wHour = Convert.ToUInt16(NewDateTime.Hour);
            struSystemTime.wMinute = Convert.ToUInt16(NewDateTime.Minute);
            struSystemTime.wSecond = Convert.ToUInt16(NewDateTime.Second);

            try
            {
                return Win32API.SetLocalTime(ref struSystemTime);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
                return false;
            }
        }
    }
}
