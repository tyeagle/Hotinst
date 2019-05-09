﻿/***************************************************************
*	Copyright (C) 2013-2018 Hotinst CO.,Ltd
*	All rights reserved
*	文 件 名:	WinFormTimer
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
    /// WinForm定时器
    /// </summary>
    internal class WinFormTimer : ITimer
    {
        #region 字段

        private System.Windows.Forms.Timer _objTimer;
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
            get { return _objTimer.Interval; }
            set { _objTimer.Interval = value; }
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
        /// 初始化WinFormTimer实例
        /// </summary>
        public WinFormTimer()
        {
            m_enuTimingMode = Mode.Repeats;
            _objTimer = new System.Windows.Forms.Timer() { Interval = 100 };
            _objTimer.Tick += Timer_Tick;
        }

        #endregion

        #region 方法

        #region 私有方法

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (m_enuTimingMode == Mode.OnceOnly)
                _objTimer.Stop();
            if (Tick != null)
                Tick(this, new TickEventArgs(_state));
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

			_objTimer.Start();
        }

        /// <summary>
        /// 停止定时器
        /// </summary>
        public void Stop()
        {
            _objTimer.Stop();
        }

        #endregion

        #endregion
    }
}
