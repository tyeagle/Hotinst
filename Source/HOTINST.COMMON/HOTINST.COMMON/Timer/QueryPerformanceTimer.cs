/***************************************************************
*	Copyright (C) 2013-2018 Hotinst CO.,Ltd
*	All rights reserved
*	文 件 名:	QueryPerformanceTimer
*	CLR 版本:	4.0.30319.42000
*
*	作    者:	tanyu
*	创建时间:	2018/6/11 
*	文件描述:			   	
*
***************************************************************/
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace HOTINST.COMMON.Timer
{
	/// <summary>
	/// QueryPerformance高精度定时器
	/// </summary>
	internal class QueryPerformanceTimer : ITimer
	{
		#region API相关

		[DllImport("kernel32.dll")]
		private static extern bool QueryPerformanceCounter(out long value);

		[DllImport("kernel32.dll")]
		private static extern bool QueryPerformanceFrequency(out long value);

		#endregion

		#region 字段

		private readonly AutoResetEvent _objAutoResetEvent;
		private readonly long _lngFrequency;
		private Thread _objThread;
		private bool _bolIsRunning;
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
		/// 获取或设置间隔时间（单位：毫秒）, 默认100ms
		/// </summary>
		public int Interval { get; set; }

		/// <summary>
		/// 定时模式, 默认重复运行
		/// </summary>
		public Mode TimingMode { get; set; }

		#endregion

		#region 构造

		/// <summary>
		/// 初始化类<see cref="QueryPerformanceTimer"/>的新实例
		/// </summary>
		public QueryPerformanceTimer()
		{
			_objAutoResetEvent = new AutoResetEvent(false);
			_bolIsRunning = false;
			Interval = 100;
			TimingMode = Mode.Repeats;
			_lngFrequency = GetFrequency();
		}

		#endregion

		#region 方法

		#region 私有方法

		private long GetFrequency()
		{
			return !QueryPerformanceFrequency(out long lngFrequency) ? 0L : lngFrequency;
		}

		private long GetTimestamp()
		{
			if(QueryPerformanceCounter(out long lngNum))
			{
				return lngNum;
			}
			throw new Exception("QueryPerformanceCounter() failed!");
		}

		private void Run()
		{
			long lngBegin = GetTimestamp();
			while(_bolIsRunning)
			{
				long lngEnd = GetTimestamp();
				long lngElapsedTime = (long)System.Math.Round((lngEnd - lngBegin) / (double)_lngFrequency * 1000);
				if(lngElapsedTime >= Interval)
				{
					lngBegin = GetTimestamp();

					if(TimingMode == Mode.OnceOnly)
						_bolIsRunning = false;

					new Task(() => Tick?.Invoke(this, new TickEventArgs(_state))).Start();
				}
				else
				{
					if(Interval > 15 && _bolIsRunning)
					{
						Thread.Sleep(1); // 此处是为了降低CPU占用率。由于会受系统的优先级等影响，当定时间隔过小的情况下定时精度会有一定误差，反之越精准
					}
				}
			}
			_objAutoResetEvent.Set();
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
			if(_lngFrequency == 0)
				throw new Exception("本机不支持此高精度定时器!");

			if(_bolIsRunning)
				return;

			_state = state;

			_bolIsRunning = true;
			_objThread = new Thread(Run)
			{
				Name = "QueryPerformanceTimer",
				Priority = ThreadPriority.Highest,
				IsBackground = true
			};
			_objThread.Start();
		}

		/// <summary>
		/// 停止定时器
		/// </summary>
		public void Stop()
		{
			if(_bolIsRunning)
			{
				_bolIsRunning = false;
				//延时500ms等待线程自行退出
				if(_objAutoResetEvent.WaitOne(500) == false)
				{
					//如果延时后线程仍不能自行退出，则强制中止
					if(_objThread.IsAlive)
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