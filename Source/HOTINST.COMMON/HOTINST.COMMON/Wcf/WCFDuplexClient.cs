/**
 * ==============================================================================
 *
 * ClassName: WCFDuplexClient
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/4/6 18:47:51
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using HOTINST.COMMON.Data;
using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Xml;

namespace HOTINST.COMMON.Wcf
{
	/// <summary>
	/// 表示用于wcf双工通信的客户端
	/// </summary>
	/// <typeparam name="TContract"></typeparam>
	/// <typeparam name="TCallback"></typeparam>
	public class WCFDuplexClient<TContract, TCallback> : IDisposable
	{
		#region fields

		private DuplexChannelFactory<TContract> _channelFactory;
		private TContract _service;
		private readonly TCallback _callback;

		private string _address;
		private ushort _port;
		private string _name;
		private string _strURI;

		#endregion

		#region props

		/// <summary>
		/// 获取或设置在传输引发异常之前可用于打开连接的时间间隔（单位：毫秒。默认值1分钟）
		/// </summary>
		/// <remarks>为零时使用系统默认值</remarks>
		public uint OpenTimeout { get; set; }

		/// <summary>
		/// 获取连接对当前状态
		/// </summary>
		public CommunicationState State
		{
			get
			{
				if(_channelFactory == null)
					return CommunicationState.None;

				switch(_channelFactory.State)
				{
					case System.ServiceModel.CommunicationState.Created:
						return CommunicationState.Created;
					case System.ServiceModel.CommunicationState.Opening:
						return CommunicationState.Opening;
					case System.ServiceModel.CommunicationState.Opened:
						return CommunicationState.Opened;
					case System.ServiceModel.CommunicationState.Closing:
						return CommunicationState.Closing;
					case System.ServiceModel.CommunicationState.Closed:
						return CommunicationState.Closed;
					case System.ServiceModel.CommunicationState.Faulted:
						return CommunicationState.Faulted;
					default:
						return CommunicationState.None;
				}
			}
		}

		/// <summary>
		/// IP地址
		/// </summary>
		/// <remarks>注：命名管道固定为locahhost</remarks>
		public string Address
		{
			get { return _address; }
			set
			{
				if(string.IsNullOrWhiteSpace(value))
					return;

				if(string.Equals(value.ToUpper(), "LOCALHOST") || string.Equals(value.ToUpper(), "LOCAL"))
				{
					_address = "localhost";
					UpdateURI();
					return;
				}

				value = value.Replace(" ", "");

				if(StringFormat.IsIPAddressString(value))
				{
					_address = value;
					UpdateURI();
				}
			}
		}

		/// <summary>
		/// 通讯端口
		/// </summary>
		/// <remarks>不要与MetadataPort相同。注：此项对命名管道无效</remarks>
		public ushort Port
		{
			get { return _port; }
			set
			{
				_port = value;
				UpdateURI();
			}
		}

		/// <summary>
		/// 识别名
		/// </summary>
		public string Name
		{
			get { return _name; }
			set
			{
				if(string.IsNullOrWhiteSpace(value))
					return;

				_name = value;
				UpdateURI();
			}
		}

		#endregion

		#region .ctor

		/// <summary>
		/// 初始化 <see cref="WCFDuplexClient{TContract,TCallback}"/> 类的新实例。
		/// </summary>
		/// <param name="cb">回调实例</param>
		public WCFDuplexClient(TCallback cb)
		{
			if(cb == null)
				throw new ArgumentNullException("cb", "回调实例不能为空");

			_service = default(TContract);
			_callback = cb;
			_address = "localhost";
			_port = 0;
			_name = "ServiceName";
			UpdateURI();
		}

		#endregion

		/// <summary>
		/// 更新URI
		/// </summary>
		private void UpdateURI()
		{
			_strURI = string.Format(@"net.tcp://{0}:{1}/{2}", _address, _port, _name);
		}
		
		/// <summary>
		/// 连接ICD服务子系统的WCF服务
		/// </summary>
		/// <returns></returns>
		public bool ConnectWCFService()
		{
			try
			{
				if(_channelFactory != null && _channelFactory.State == System.ServiceModel.CommunicationState.Opened)
					return true;

				NetTcpBinding tcpBinding = new NetTcpBinding(SecurityMode.None)
				{
					MaxReceivedMessageSize = int.MaxValue,
					MaxBufferPoolSize = int.MaxValue,
					ReaderQuotas = XmlDictionaryReaderQuotas.Max
				};
				//设置超时
				if(OpenTimeout != 0)
					tcpBinding.OpenTimeout = TimeSpan.FromMilliseconds(Convert.ToDouble(OpenTimeout));

				_channelFactory = new DuplexChannelFactory<TContract>(new InstanceContext(_callback), tcpBinding, _strURI);
				_service = _channelFactory.CreateChannel();
				
				return true;
			}
			catch(Exception ex)
			{
				if(_channelFactory != null)
				{
					_channelFactory.Abort();
					_channelFactory = null;

				}

				throw new Exception("连接服务器失败：" + ex.Message);
			}
		}

		/// <summary>
		/// 断开与ICD服务子系统的WCF服务连接
		/// </summary>
		public void DisconnectWCFService()
		{
			try
			{
				if(_channelFactory != null && _channelFactory.State != System.ServiceModel.CommunicationState.Closed)
					_channelFactory.Close();

				_channelFactory = null;
				_service = default(TContract);
			}
			catch(CommunicationObjectFaultedException)
			{
				// ignored
			}
			catch(Exception ex)
			{
				Debug.WriteLine("断开服务器异常：" + ex.Message);
			}
			finally
			{
				if(_channelFactory != null)
				{
					_channelFactory.Abort();
					_channelFactory = null;
				}
			}
		}

		/// <summary>
		/// 调用契约方法（自动连接）, 连接失败将抛出异常
		/// </summary>
		/// <param name="action">方法</param>
		public void UsingService(Action<TContract> action)
		{
			if(ConnectWCFService())
				action(_service);
			else
				throw new Exception("服务器未在线");
		}

		/// <summary>
		/// 调用契约方法（自动连接）, 连接失败将抛出异常
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="action"></param>
		/// <returns></returns>
		public TResult UsingService<TResult>(Func<TContract, TResult> action)
		{
			if(!ConnectWCFService())
				throw new Exception("服务器未在线");

			return action(_service);
		}

		/// <summary>
		/// 资源释放
		/// </summary>
		public void Dispose()
		{
			DisconnectWCFService();
		}
	}
}