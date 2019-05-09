/**
 * ==============================================================================
 *
 * ClassName: ServiceState
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/4/13 9:38:27
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Net.Sockets;
using HOTINST.COMMON.DataCheck;

namespace HOTINST.COMMON.Wcf
{
	/// <summary>
	/// 判断远程计算机的端口打开状态
	/// </summary>
	public class ServiceState
	{
		#region .ctor

		/// <summary>
		/// 初始化 <see cref="ServiceState"/> 类的新实例。
		/// </summary>
		public ServiceState()
		{

		}

		#endregion

		/// <summary>
		/// 检测端口是否打开
		/// </summary>
		/// <returns>true: 已开启;	false: 未开启</returns>
		public bool IsOpen(string ip, ushort port)
		{
			if(string.IsNullOrEmpty(ip))
				throw new ArgumentNullException("ip");

			if(!ip.ToLower().Equals("localhost"))
			{
				if(!DataCheckHelper.IsIPAddressString(ip))
					throw new ArgumentException("IP 地址无效！", "ip");
			}

			try
			{
				TcpClient connection = new TcpClientWithTimeout(ip, port).Connect();
				connection.Close();
				return true;
			}
			catch(Exception)
			{
				return false;
			}
		}
	}
}