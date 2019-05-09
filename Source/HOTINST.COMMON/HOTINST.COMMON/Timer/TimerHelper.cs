/***************************************************************
*	Copyright (C) 2013-2018 Hotinst CO.,Ltd
*	All rights reserved
*	文 件 名:	TimerHelper
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
    /// 定时器Helper类
    /// </summary>
    public static class TimerHelper
    {
        /// <summary>
        /// 创建一个特定种类的定时器实例
        /// </summary>
        /// <param name="timerKind">指定使用的定时器种类</param>
        /// <returns>创建成功返回实例，失败返回null</returns>
        public static ITimer CreateTimer(TimerKind timerKind)
        {
            switch (timerKind)
            {
                case TimerKind.SystemTimer:
                    return new SystemTimer();
                case TimerKind.WinFormTimer:
                    return new WinFormTimer();
                case TimerKind.DispatcherTimer:
                    return new DispatcherTimer();
                case TimerKind.ThreadTimer:
                    return new ThreadTimer();
                case TimerKind.SleepTimer:
                    return new SleepTimer();
                case TimerKind.WaitHandleTimer:
                    return new WaitHandleTimer();
                case TimerKind.StopwatchTimer:
                    return new StopwatchTimer();
                case TimerKind.SocketPollTimer:
                    return new SocketPollTimer();
                case TimerKind.EnvironmentTickCountTimer:
                    return new EnvironmentTickCountTimer();
                case TimerKind.DateTimeTickCountTimer:
                    return new DateTimeTickCountTimer();
                case TimerKind.WinmmTimer:
                    return new WinmmTimer();
                case TimerKind.QueryPerformanceTimer:
                    return new QueryPerformanceTimer();
                default:
                    return null;
            }
        }
    }
}
