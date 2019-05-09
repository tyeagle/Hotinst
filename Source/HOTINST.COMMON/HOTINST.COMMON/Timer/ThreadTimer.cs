/***************************************************************
*	Copyright (C) 2013-2018 Hotinst CO.,Ltd
*	All rights reserved
*	文 件 名:	ThreadTimer
*	CLR 版本:	4.0.30319.42000
*
*	作    者:	tanyu
*	创建时间:	2018/6/11 
*	文件描述:			   	
*
***************************************************************/
using System;

namespace HOTINST.COMMON.Timer
{
    /// <summary>
    /// Thread定时器
    /// </summary>
    internal class ThreadTimer : ITimer
    {
        #region 字段

        private System.Threading.Timer _objTimer;
        private int m_intInterval;
        private Mode m_enuTimingMode;
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
        /// 初始化ThreadTimer实例
        /// </summary>
        public ThreadTimer()
        {
            m_intInterval = 100;
            m_enuTimingMode = Mode.Repeats;
        }

        #endregion

        #region 方法

        #region 私有方法

        private void CallbackFunction(object sender)
        {
            if(m_enuTimingMode==Mode.OnceOnly)
                Stop();
            if (Tick != null)
                Tick(sender, null);
        }

		#endregion

		#region 公开方法

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

			if(m_enuTimingMode == Mode.Repeats)
                _objTimer = new System.Threading.Timer(new System.Threading.TimerCallback(CallbackFunction), this, m_intInterval, m_intInterval);
            else
                _objTimer = new System.Threading.Timer(new System.Threading.TimerCallback(CallbackFunction), this, m_intInterval, System.Threading.Timeout.Infinite);
        }

        /// <summary>
        /// 停止定时器
        /// </summary>
        public void Stop()
        {
            if (_objTimer != null)
            {
                _objTimer.Dispose();
            }
        }

        #endregion

        #endregion
    }
}
