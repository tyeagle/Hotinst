/***************************************************************
*	Copyright (C) 2013-2018 Hotinst CO.,Ltd
*	All rights reserved
*	文 件 名:	TimerKind
*	CLR 版本:	4.0.30319.42000
*
*	作    者:	tanyu
*	创建时间:	2018/6/11 
*	文件描述:			   	
*
***************************************************************/
namespace HOTINST.COMMON.Timer
{
    /// <summary>
    /// 定时器种类枚举
    /// </summary>
    public enum TimerKind
    {
        #region C#自带定时器

        /// <summary>
        /// System.Timers.Timer
        /// </summary>
        SystemTimer,
        /// <summary>
        /// System.Windows.Forms.Timer
        /// </summary>
        WinFormTimer,
        /// <summary>
        /// System.Windows.Threading.Dispatcher
        /// </summary>
        DispatcherTimer,
        /// <summary>
        /// System.Threading.Timer
        /// </summary>
        ThreadTimer,

        #endregion

        #region 利用C#部分超时方法自定义定时器

        /// <summary>
        /// System.Threading.Thread
        /// </summary>
        SleepTimer,
        /// <summary>
        /// System.Threading.WaitHandle
        /// </summary>
        WaitHandleTimer,
        /// <summary>
        /// System.Diagnostics.Stopwatch
        /// </summary>
        StopwatchTimer,
        /// <summary>
        /// System.Net.Sockets.Socket
        /// </summary>
        SocketPollTimer,

        #endregion

        #region 利用经过的时间数自定义定时器

        /// <summary>
        /// System.Environment
        /// </summary>
        EnvironmentTickCountTimer,
        /// <summary>
        /// System.DateTime
        /// </summary>
        DateTimeTickCountTimer,

        #endregion

        #region 利用WindowsAPI自定义定时器

        /// <summary>
        /// 基于Winmm.DLL的多媒体定时器
        /// </summary>
        WinmmTimer,
        /// <summary>
        /// 基于Kernel32.DLL的高精度定时器
        /// </summary>
        QueryPerformanceTimer,

        #endregion
    }
}
