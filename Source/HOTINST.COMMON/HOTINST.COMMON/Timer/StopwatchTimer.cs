/***************************************************************
*	Copyright (C) 2013-2018 Hotinst CO.,Ltd
*	All rights reserved
*	文 件 名:	StopwatchTimer
*	CLR 版本:	4.0.30319.42000
*
*	作    者:	tanyu
*	创建时间:	2018/6/11 
*	文件描述:			   	
*
***************************************************************/
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace HOTINST.COMMON.Timer
{
    /// <summary>
    /// Stopwatch定时器
    /// </summary>
    internal class StopwatchTimer : ITimer
    {
        #region 字段

        private Stopwatch _objStopwatch;
        private Thread _objThread;
        private AutoResetEvent _objAutoResetEvent;
        private bool _bolIsRunning;
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
        /// 初始化StopwatchTimer实例
        /// </summary>
        public StopwatchTimer()
        {
            _objStopwatch = new Stopwatch();
            _objAutoResetEvent = new AutoResetEvent(false);
            _bolIsRunning = false;
            m_intInterval = 100;
            m_enuTimingMode = Mode.Repeats;
        }

        #endregion

        #region 方法

        private void Run()
        {
            long lngElapsedTime = 0;
            _objStopwatch.Start();
            while (_bolIsRunning)
            {
                lngElapsedTime = (long)System.Math.Round((_objStopwatch.ElapsedTicks / (decimal)Stopwatch.Frequency) * 1000);
                if (lngElapsedTime >= m_intInterval)
                {
                    _objStopwatch.Restart();

                    if (m_enuTimingMode == Mode.OnceOnly)
                        _bolIsRunning = false;

                    new Task(() =>
                    {
                        if (Tick != null)
                            Tick(this, null);
                    }).Start();
                }
                else
                {
                    Thread.Sleep(1);
                }
            }
            _objStopwatch.Stop();
            _objAutoResetEvent.Set();
        }

		#region 私有方法

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
            if (_bolIsRunning)
                return;

	        _state = state;

			_bolIsRunning = true;
            _objThread = new Thread(Run) { IsBackground = true };
            _objThread.Start();
        }

        /// <summary>
        /// 停止定时器
        /// </summary>
        public void Stop()
        {
            if (!_bolIsRunning)
            {
                _bolIsRunning = false;
                //延时等待线程自行退出
                if (_objAutoResetEvent.WaitOne(500) == false)
                {
                    //如果延时后线程仍不能自行退出，则强制中止
                    if (_objThread.IsAlive)
                        _objThread.Abort();
                }
                //释放线程对象
                _objThread = null;
            }
        }

        #endregion

        #endregion
    }
}
