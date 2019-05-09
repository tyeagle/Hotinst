using System;

namespace HOTINST.COMMON.ServiceProcess
{
	/// <summary>
	/// ��ʾ����״̬�仯�¼������ࡣ
	/// </summary>
	public class ServiceStateChangedEventArgs : EventArgs
	{
		/// <summary>
		/// ��ǰ״̬
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