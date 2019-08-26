/**
 * ==============================================================================
 *
 * ClassName: TcpClientWithTimeout
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/4/13 10:22:14
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;

namespace HOTINST.COMMON.Wcf
{
	/// <summary>
	/// 表示用来设置一个带连接超时功能的类
	/// </summary>
	public class TcpClientWithTimeout
	{
		#region fields

		/// <summary>
		/// 
		/// </summary>
		protected string _hostname;
		/// <summary>
		/// 
		/// </summary>
		protected int _port;
		/// <summary>
		/// 毫秒
		/// </summary>
		protected int _timeout;
		/// <summary>
		/// 
		/// </summary>
		protected TcpClient _connection;
		/// <summary>
		/// 
		/// </summary>
		protected bool _connected;
		/// <summary>
		/// 
		/// </summary>
		protected Exception _exception;

		private Thread _thread;

		#endregion

		#region props

		#endregion

		#region .ctor

		/// <summary>
		/// 初始化 <see cref="TcpClientWithTimeout"/> 类的新实例。
		/// </summary>
		public TcpClientWithTimeout()
		{

		}

		/// <summary>
		/// 初始化 <see cref="TcpClientWithTimeout"/> 类的新实例。
		/// </summary>
		/// <param name="hostname"></param>
		/// <param name="port"></param>
		/// <param name="timeout"></param>
		public TcpClientWithTimeout(string hostname, int port, int timeout = 200)
		{
			_hostname = hostname;
			_port = port;
			_timeout = timeout;
		}

		#endregion

		/// <summary>
		/// 在指定的超时时间内发起连接
		/// </summary>
		/// <returns>连接成功返回TcpClient对象, 否则抛出异常</returns>
		public TcpClient Connect()
		{
			try
			{
				// kick off the thread that tries to connect
				_connected = false;
				_exception = null;

				_thread = new Thread(BeginConnect) { IsBackground = true };
				_thread.Start();

				if(_thread.Join(_timeout))
				{
					if(_connected)
						return _connection;
					else
					{
						if(_exception != null)
							throw _exception;
						return _connection;
					}
				}
				else
				{
					_thread.Abort();
					throw new TimeoutException(string.Format("TcpClient connection to {0}:{1} timed out", _hostname, _port));
				}
			}
			finally
			{
				if(_thread.IsAlive)
				{
					_thread.Abort();
				}
			}
		}

		/// <summary>
		/// 启动连接
		/// </summary>
		protected void BeginConnect()
		{
			try
			{
				_connection = new TcpClient(_hostname, _port);
				// 标记成功，返回调用者
				_connected = true;
			}
			catch(Exception ex)
			{
				// 标记失败
				_exception = ex;
			}
		}
	}
}