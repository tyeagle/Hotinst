/***************************************************************
*	Copyright (C) 2013-2018 Hotinst CO.,Ltd
*	All rights reserved
*	文 件 名:	WinmmTimer
*	CLR 版本:	4.0.30319.42000
*
*	作    者:	tanyu
*	创建时间:	2018/6/11 
*	文件描述:			   	
*
***************************************************************/
using System;
using System.Runtime.InteropServices;

namespace HOTINST.COMMON.Timer
{
    /// <summary>
    /// 多媒体定时器
    /// </summary>
    internal class WinmmTimer : ITimer
    {
        #region API相关

        #region API声明

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uDelay">延迟时间（单位:毫秒）</param>
        /// <param name="uResolution">时间精度（单位:毫秒，默认值为1毫秒）</param>
        /// <param name="lpTimeProc">用于定时回调的用户自定义函数</param>
        /// <param name="dwUser">回调时要传回的用户自定义数据</param>
        /// <param name="fuEvent">定时器的事件类型，TIME_ONESHOT表示执行一次；TIME_PERIODIC：周期性执行。</param>
        /// <returns></returns>
        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        private static extern uint timeSetEvent(uint uDelay, uint uResolution, CallbackFunction lpTimeProc, UIntPtr dwUser, uint fuEvent);

        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        private static extern uint timeKillEvent(uint uTimerID);

        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        private static extern uint timeGetTime();

        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        private static extern uint timeBeginPeriod(uint uPeriod);

        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        private static extern uint timeEndPeriod(uint uPeriod);

        #endregion

        #region 其它有关定义

        private delegate void CallbackFunction(uint uTimerId, uint uMsg, UIntPtr dwUser, UIntPtr dw1, UIntPtr dw2);

        #endregion

        #endregion

        #region 字段

        private int m_intInterval;
        private Mode m_enuTimingMode;
        private uint _intTimerHandle;
        private CallbackFunction _objCallbackFunction;
        private object _state;

        #endregion

        #region 事件

        /// <summary>
        /// 定时事件
        /// </summary>
        public event EventHandler<TickEventArgs> Tick;

        #endregion

        #region 属性

        /// <summary>
        /// 获取或设置间隔时间（单位：毫秒）
        /// </summary>
        public int Interval
        {
            get { return m_intInterval; }
            set { m_intInterval = value; }
        }

        /// <summary>
        /// 定时模式
        /// </summary>
        public Mode TimingMode
        {
            get { return m_enuTimingMode; }
            set { m_enuTimingMode = value; }
        }

        #endregion

        #region 构造

        /// <summary>
        /// 初始化WinmmTimer实例
        /// </summary>
        public WinmmTimer()
        {
            m_intInterval = 100;
            m_enuTimingMode = Mode.Repeats;
            _intTimerHandle = 0;
            _objCallbackFunction = new CallbackFunction(TimerCallback);
        }

        #endregion

        #region 方法

        #region 私有方法

        private void TimerCallback(uint uTimerId, uint uMsg, UIntPtr dwUser, UIntPtr dw1, UIntPtr dw2)
        {
            if (m_enuTimingMode == Mode.OnceOnly)
                Stop();
            if (Tick != null)
                Tick(this, new TickEventArgs(_state));
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 启动定时器
        /// </summary>
        public void Start()
        {
            Start(null);
        }

        /// <summary>
        /// 启动定时器
        /// </summary>
        public void Start(object state)
        {
	        _state = state;
			//如果Timer句柄已经存在，则释放
			Stop();
            //设置定时器参数
            _intTimerHandle = timeSetEvent((uint)m_intInterval, 1, _objCallbackFunction, UIntPtr.Zero, (uint)m_enuTimingMode);
        }

        /// <summary>
        /// 停止定时器
        /// </summary>
        public void Stop()
        {
            if (_intTimerHandle != 0)
            {
                timeKillEvent(_intTimerHandle);
                _intTimerHandle = 0;
            }
        }

        #endregion

        #endregion
    }
}
