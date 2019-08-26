/***************************************************************
*	Copyright (C) 2013-2018 Hotinst CO.,Ltd
*	All rights reserved
*	文 件 名:	ITimer
*	CLR 版本:	4.0.30319.42000
*
*	作    者:	tanyu
*	创建时间:	2018/6/11 16:48:23
*	文件描述:			   	
*
***************************************************************/
using System;

namespace HOTINST.COMMON.Timer
{
    /// <summary>
    /// 定时器公用接口
    /// </summary>
    public interface ITimer
    {
        /// <summary>
        /// 获取或设置间隔时间（单位：毫秒）
        /// </summary>
        int Interval { get; set; }

        /// <summary>
        /// 定时模式
        /// </summary>
        Mode TimingMode { get; set; }

        /// <summary>
        /// 定时事件
        /// </summary>
        event EventHandler<TickEventArgs> Tick;
        
        /// <summary>
        /// 启动定时器
        /// </summary>
        void Start();

		/// <summary>
		/// 启动定时器
		/// </summary>
		/// <param name="state">事件参数</param>
	    void Start(object state);

		/// <summary>
		/// 停止定时器
		/// </summary>
		void Stop();
    }
}
