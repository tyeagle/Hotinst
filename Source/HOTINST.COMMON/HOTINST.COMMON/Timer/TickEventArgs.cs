/***************************************************************
*	Copyright (C) 2013-2018 Hotinst CO.,Ltd
*	All rights reserved
*	文 件 名:	TickEventArgs
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
	/// 定时器事件参数
	/// </summary>
	public class TickEventArgs : EventArgs
	{
		#region fields

		#endregion

		#region props

		/// <summary>
		/// 获取用户自定义定时器事件参数
		/// </summary>
		public object State { get; }

		#endregion

		#region .ctor

		/// <summary>
		/// ctor
		/// </summary>
		public TickEventArgs(object state)
		{
			State = state;
		}

		#endregion
	}
}