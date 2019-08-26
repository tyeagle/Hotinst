/****************************************************************
 * 类 名 称：WCFServer
 * 命名空间：HOTINST.COMMON.Wcf
 * 文 件 名：WCFServer.cs
 * 创建时间：2016-8-25
 * 作    者：汪锋
 * 说    明：封装WCF服务端通用类
 * 修改历史：
 * 
 * 
 *****************************************************************/

using HOTINST.COMMON.Data;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading;
using System.Xml;

namespace HOTINST.COMMON.Wcf
{
    /// <summary>
    /// WCF服务类
    /// </summary>
    public class WCFServer : IDisposable
    {
        #region 字段

        /// <summary>
        /// WCF的ServiceHost对象
        /// </summary>
        private ServiceHost _objHost;
        private Thread _objThread;
        private Type m_ServiceContract;
        private Type m_ContractImplementClass;
        private int m_OpenTimeout;
        private int m_CloseTimeout;
        private string m_Address;
        private ushort m_Port;
        private string m_Name;
        private string _strNetTcpURI;
        private string _strNamedPipeURI;
        private ushort m_MetadataPort;
        private string _MetadataURI;

        /// <summary>
        /// 当通信对象转换到正在打开状态时发生
        /// </summary>
        public event EventHandler Opening;
        /// <summary>
        /// 当通信对象转换到正在关闭状态时发生
        /// </summary>
        public event EventHandler Closing;
        /// <summary>
        /// 当通信对象转换到出错状态时发生
        /// </summary>
        public event EventHandler Faulted;

        #endregion

        #region 构造与析构

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            CloseWCFService();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public WCFServer()
        {
            _objHost = null;
            m_ServiceContract = null;
            m_ContractImplementClass = null;
            m_Address = "localhost";
            m_Port = 57601;
            m_Name = "CommServer";
            m_MetadataPort = 57600;
            m_OpenTimeout = 0;
            m_CloseTimeout = 0;
            UpdateURI();
        }

        #endregion

        #region 属性

        /// <summary>
        /// 打开WCF服务的超时时间（单位：毫秒。默认值1分钟）
        /// </summary>
        /// <remarks>为零时使用系统默认值</remarks>
        public int OpenTimeout
        {
            get { return m_OpenTimeout; }
            set { m_OpenTimeout = System.Math.Abs(value); }
        }

        /// <summary>
        /// 关闭WCF服务的超时时间（单位：毫秒。默认值1分钟）
        /// </summary>
        /// <remarks>为零时使用系统默认值</remarks>
        private int CloseTimeout
        {
            get { return m_CloseTimeout; }
            set { m_CloseTimeout = System.Math.Abs(value); }
        }

        /// <summary>
        /// IP地址
        /// </summary>
        /// <remarks>注：命名管道固定为locahhost</remarks>
        public string Address
        {
            get { return m_Address; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    return;
                if (string.Equals(value.ToUpper(), "LOCALHOST") || string.Equals(value.ToUpper(), "LOCAL"))
                {
                    m_Address = "localhost";
                    UpdateURI();
                    return;
                }
                if (StringFormat.IsIPAddressString(value))
                {
                    m_Address = value;
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
            get { return m_Port; }
            set 
            {
                if (m_Port == m_MetadataPort)
                    return;
                m_Port = value;
                UpdateURI();
            }
        }

        /// <summary>
        /// 识别名
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set 
            {
                if (string.IsNullOrWhiteSpace(value))
                    return;
                m_Name = value;
                UpdateURI();
            }
        }

        /// <summary>
        /// 发布元数据的端口
        /// </summary>
        /// <remarks>不要与Port相同</remarks>
        public ushort MetadataPort
        {
            get { return m_Port; }
            set
            {
                if (m_Port == m_MetadataPort)
                    return;
                m_MetadataPort = value;
                UpdateURI();
            }
        }

        /// <summary>
        /// 服务契约的接口类
        /// </summary>
        /// <example>typeof(Interface)</example>
        public Type ServiceContract
        {
            get { return m_ServiceContract; }
            set { m_ServiceContract = value; }
        }

        /// <summary>
        /// 服务契约的实现类
        /// </summary>
        /// <example>typeof(Class)</example>
        public Type ContractImplementClass
        {
            get { return m_ContractImplementClass; }
            set { m_ContractImplementClass = value; }
        }

        /// <summary>
        /// 更新URI
        /// </summary>
        private void UpdateURI()
        {
            _MetadataURI = string.Format(@"http://{0}:{1}/{2}", m_Address, m_MetadataPort.ToString().Trim(), m_Name);
            _strNetTcpURI = string.Format(@"net.tcp://{0}:{1}/{2}", m_Address, m_Port.ToString().Trim(), m_Name);
            _strNamedPipeURI = string.Format(@"net.pipe://localhost/{0}", m_Name);
        }

        #endregion

        #region 方法

        /// <summary>
        /// 检查WCF服务是否处于打开状态
        /// </summary>
        /// <returns>打开返回true，非打开返回false</returns>
        public bool IsOpen()
        {
            if (_objHost == null)
                return false;
            if (_objHost.State == System.ServiceModel.CommunicationState.Closed)
                return false;
            return true;
        }

        /// <summary>
        /// 启动WCF服务
        /// </summary>
        /// <returns></returns>
        public bool StartWCFService()
        {
            try
            {
                if (ServiceContract == null || ContractImplementClass == null)
                    return false;

                CloseWCFService();
                //创建服务
                _objHost = new ServiceHost(m_ContractImplementClass);
                _objHost.AddServiceEndpoint(m_ServiceContract,
                    new NetTcpBinding(SecurityMode.None)
                    {
                        MaxReceivedMessageSize = int.MaxValue,
                        MaxBufferPoolSize = int.MaxValue,
                        ReaderQuotas = XmlDictionaryReaderQuotas.Max
                    }, _strNetTcpURI);
                _objHost.AddServiceEndpoint(m_ServiceContract,
                    new NetNamedPipeBinding(NetNamedPipeSecurityMode.None)
                    {
                        MaxReceivedMessageSize = int.MaxValue,
                        MaxBufferPoolSize = int.MaxValue,
                        ReaderQuotas = XmlDictionaryReaderQuotas.Max
                    }, _strNamedPipeURI);
                //发布元数据
                ServiceMetadataBehavior objMetadataBehavior = new ServiceMetadataBehavior() { HttpGetEnabled = true, HttpGetUrl = new Uri(_MetadataURI) };
                _objHost.Description.Behaviors.Add(objMetadataBehavior);
                //参数设置
                //ServiceDebugBehavior objServiceDebugBehavior = new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true };
                //_objHost.Description.Behaviors.Add(objServiceDebugBehavior);
                foreach(IServiceBehavior objBehavior in _objHost.Description.Behaviors)
                {
                    if (objBehavior.GetType().Equals(typeof(ServiceDebugBehavior)))
                    {
                        ((ServiceDebugBehavior)objBehavior).IncludeExceptionDetailInFaults = true;
                    }
                }
                //设置超时
                if (OpenTimeout != 0)
                    _objHost.OpenTimeout = TimeSpan.FromMilliseconds(Convert.ToDouble(m_OpenTimeout));
                if (CloseTimeout != 0)
                    _objHost.CloseTimeout = TimeSpan.FromMilliseconds(Convert.ToDouble(m_CloseTimeout));
                //绑定服务的相关事件
                _objHost.Opening += _objHost_Opening;
                _objHost.Closing += _objHost_Closing;
                _objHost.Faulted += _objHost_Faulted;
                //以独立线程来启动运行服务
                _objThread = new Thread(_objHost.Open) { IsBackground = true };
                _objThread.Start();

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 关闭WCF服务
        /// </summary>
        public void CloseWCFService()
        {
            try
            {
                if (_objHost != null && _objHost.State != System.ServiceModel.CommunicationState.Closed)
                {
                    _objHost.Close();
                    _objHost.Opening -= _objHost_Opening;
                    _objHost.Closing -= _objHost_Closing;
                    _objHost.Faulted -= _objHost_Faulted;
                    _objHost = null;
                    //如果线程仍未退出，则强制中止
                    if (_objThread.IsAlive)     //if (_objThread.ThreadState != (ThreadState.Background | ThreadState.Stopped))
                        _objThread.Abort();
                }
            }
            catch (Exception ex)
            {
                if (_objHost != null)
                    _objHost.Abort();
                System.Diagnostics.Debug.Print(ex.Message);
            }
        }

        #region 触发事件

        private void _objHost_Faulted(object sender, EventArgs e)
        {
            if (Faulted != null)
                Faulted(sender, e);
        }

        private void _objHost_Closing(object sender, EventArgs e)
        {
            if (Closing != null)
                Closing(sender, e);
        }

        private void _objHost_Opening(object sender, EventArgs e)
        {
            if (Opening != null)
                Opening(sender, e);
        }

        #endregion

        #endregion
    }
}
