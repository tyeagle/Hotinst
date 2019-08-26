using System;

namespace HOTINST.COMMON.ServiceProcess
{
	/// <summary>
	/// 表示服务状态变化事件参数类。
	/// </summary>
	public class ServiceStateChangedEventArgs : EventArgs
	{
		/// <summary>
		/// 当前状态
		/// </summary>
		public ServiceState State { get; set; }

		/// <summary>
		/// .ctor
		/// </summary>
		/// <param name="state"></param>
		public ServiceStateChangedEventArgs(ServiceState state)
		{
			State = state;
		}
	}
}