/****************************************************************
* 类 名 称：ServiceHelper
* 命名空间：HOTINST.COMMON.ServiceProcess
* 文 件 名：ServiceHelper.cs
* 创建时间：2017-3-10
* 作    者：蔡先松
* 说    明：简易Windows服务处理类。提供服务状态变化通知事件。
* 修改时间：
* 修 改 人：
*****************************************************************/

using System;
using System.Linq;
using System.ServiceProcess;

namespace HOTINST.COMMON.ServiceProcess
{
	/// <summary>
	/// 提供Windows服务的简单处理的类
	/// </summary>
	public static class ServiceHelper
	{
		/// <summary>
		/// 服务名
		/// </summary>
		public static string ServiceName { get; set; }
		/// <summary>
		/// 服务状态变化事件
		/// </summary>
		public static event EventHandler<ServiceStateChangedEventArgs> ServiceStateChanged;

		private static void RaiseStateChanged(ServiceState state)
		{
			if(ServiceStateChanged != null)
				ServiceStateChanged(ServiceName, new ServiceStateChangedEventArgs(state));
		}

		/// <summary>
		/// 初始化服务
		/// </summary>
		/// <param name="svrName">服务名称</param>
		public static void Initialize(string svrName)
		{
			ServiceName = svrName;

			ServiceController svr = GetServiceController();
			if(svr == null)
			{
				RaiseStateChanged(ServiceState.Error);
				return;
			}
			RaiseStateChanged(IsStart() ? ServiceState.Running : ServiceState.Stopped);
		}

		/// <summary>
		/// 获取windows服务的控制器
		/// </summary>
		/// <returns>成功返回windows服务的控制器；失败返回null</returns>
		public static ServiceController GetServiceController()
		{
			try
			{
				return ServiceController.GetServices().FirstOrDefault(svr => svr.ServiceName == ServiceName);
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// 启动服务
		/// </summary>
		public static bool StartService()
		{
			try
			{
				ServiceController svr = GetServiceController();
				svr.Start();
				svr.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 10));

				RaiseStateChanged(ServiceState.Running);
				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// 停止服务
		/// </summary>
		public static bool StopService()
		{
			try
			{
				ServiceController svr = GetServiceController();
				svr.Stop();
				svr.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 10));

				RaiseStateChanged(ServiceState.Stopped);
				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// 暂停服务
		/// </summary>
		/// <returns></returns>
		public static bool PauseService()
		{
			try
			{
				ServiceController svr = GetServiceController();
				svr.Pause();
				svr.WaitForStatus(ServiceControllerStatus.Paused, new TimeSpan(0, 0, 10));

				RaiseStateChanged(ServiceState.Paused);
				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// 恢复运行
		/// </summary>
		/// <returns></returns>
		public static bool ResumeService()
		{
			try
			{
				ServiceController svr = GetServiceController();
				svr.Continue();
				svr.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 10));

				RaiseStateChanged(ServiceState.Running);
				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// 判断服务是否已暂停
		/// </summary>
		public static bool IsPaused()
		{
			try
			{
				ServiceController svr = GetServiceController();
				if((svr.Status == ServiceControllerStatus.Paused) ||
				   (svr.Status == ServiceControllerStatus.PausePending))
				{
					return true;
				}
			}
			catch
			{
				return false;
			}

			return false;
		}

		/// <summary>
		/// 判断服务是否已启动
		/// </summary>
		public static bool IsStart()
		{
			try
			{
				ServiceController svr = GetServiceController();
				if((svr.Status == ServiceControllerStatus.Stopped) ||
				   (svr.Status == ServiceControllerStatus.StopPending))
				{
					return false;
				}
			}
			catch
			{
				return false;
			}

			return true;
		}
	}
}