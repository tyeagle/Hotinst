/****************************************************************
 * 类 名 称：WCFClient
 * 命名空间：HOTINST.COMMON.Wcf
 * 文 件 名：WCFClient.cs
 * 创建时间：2016-8-25
 * 作    者：汪锋
 * 说    明：封装WCF客户端通用类
 * 修改历史：
 *           2016-8-29 汪锋 新增UsingService方法，用于更便捷的
 *           方法调用。
 *           2017-2-21 汪锋 修改UsingService方法，当调用方法出
 *           现异常时也能保证DisconnectWCFService被调用。
 * 
 *****************************************************************/

using HOTINST.COMMON.Data;
using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Xml;

namespace HOTINST.COMMON.Wcf
{
	/// <summary>
	/// 表示用于wcf双工通信的客户端
	/// </summary>
	/// <typeparam name="TContract"></typeparam>
	public class WCFClient<TContract> : IDisposable
	{
		#region fields

		private ChannelFactory<TContract> _channelFactory;
		private TContract _service;
		private NetTcpBinding _netTcpBinding;

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
		/// 初始化 <see cref="WCFClient{TContract}"/> 类的新实例。
		/// </summary>
		public WCFClient()
		{
			_service = default(TContract);
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
			_strURI = $@"net.tcp://{_address}:{_port}/{_name}";
		}
		
		/// <summary>
		/// 连接服务系统的WCF服务
		/// </summary>
		/// <returns></returns>
		public bool ConnectWCFService()
		{
			try
			{
				if(_channelFactory != null && _channelFactory.State == System.ServiceModel.CommunicationState.Opened)
					return true;

				if(_netTcpBinding == null)
				{
					_netTcpBinding = new NetTcpBinding(SecurityMode.None)
					{
						MaxReceivedMessageSize = int.MaxValue,
						MaxBufferPoolSize = int.MaxValue,
						ReaderQuotas = XmlDictionaryReaderQuotas.Max
					};
				}
				
				//设置超时
				if(OpenTimeout != 0)
					_netTcpBinding.OpenTimeout = TimeSpan.FromMilliseconds(Convert.ToDouble(OpenTimeout));

				_channelFactory = new ChannelFactory<TContract>(_netTcpBinding, _strURI);

				//add by lisheng to avoid error on Xp Sytem
				foreach(OperationDescription op in _channelFactory.Endpoint.Contract.Operations)
				{
					DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<DataContractSerializerOperationBehavior>() as DataContractSerializerOperationBehavior;
					if(dataContractBehavior != null)
					{
						dataContractBehavior.MaxItemsInObjectGraph = int.MaxValue;
					}
				}

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
				
				Debug.WriteLine($"连接服务器[{_strURI}]失败：{ex.Message}");

				return false;
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
				Debug.WriteLine($"断开服务器[{_strURI}]异常：{ex.Message}");
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
			if(!ConnectWCFService())
				throw new Exception($"连接服务器[{_strURI}]失败");

			action(_service);
		}

		/// <summary>
		/// 调用契约方法（自动连接）, 连接失败将抛出异常
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="func"></param>
		/// <returns></returns>
		public TResult UsingService<TResult>(Func<TContract, TResult> func)
		{
			if(!ConnectWCFService())
				throw new Exception($"连接服务器[{_strURI}]失败");

			return func(_service);
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