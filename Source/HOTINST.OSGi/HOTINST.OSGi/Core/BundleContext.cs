﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HOTINST.OSGI.Core.Root;
using HOTINST.OSGI.Event;
using HOTINST.OSGI.Listener;
using HOTINST.OSGI.Service;

namespace HOTINST.OSGI.Core
{

    /// <summary>
    /// 表示模块上下文，暴露了内核的操作
    /// </summary>
    class BundleContext : IBundleContext, IContextFireEvent
    {
        #region Property & Field
        /// <summary>
        /// 框架内核
        /// </summary>
        private IFramework framework;
        /// <summary>
        /// 上下文关联的当前模块
        /// </summary>
        private IBundle bundle;
        /// <summary>
        /// 当前模块的Bundle监听器列表
        /// </summary>
        private IList<IBundleListener> bundleListenerList = new List<IBundleListener>();
        /// <summary>
        /// 当前模块的Extension监听器列表
        /// </summary>
        private IList<IExtensionListener> extensionListenerList = new List<IExtensionListener>();
        /// <summary>
        /// 当前模块的服务监听器列表
        /// </summary>
        private IList<IServiceListener> serviceListenerList = new List<IServiceListener>();
        /// <summary>
        /// 当前模块注册的服务引用
        /// </summary>
        private IList<IServiceReference> serviceReferenceList = new List<IServiceReference>();
        /// <summary>
        /// 当前模块引用的其他服务
        /// </summary>
        private IList<IServiceReference> bundleUsingServiceReferenceList = new List<IServiceReference>();
        #endregion

        #region Constructor

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="framework">框架内核</param>
        /// <param name="bundle">模块</param>
        public BundleContext(IFramework framework, IBundle bundle)
        {
            this.framework = framework;
            this.bundle = bundle;
        }

        #endregion

        #region Method


        #region Listener
        /// <summary>
        /// 添加一个Bundle监听器实例
        /// </summary>
        /// <param name="listener">Bundle监听器实例</param>
        public void AddBundleListener(IBundleListener listener)
        {
            ((IFrameworkListener)framework).AddBundleListener(listener);
            bundleListenerList.Add(listener);
        }

        /// <summary>
        /// 移除一个Bundle监听器实例
        /// </summary>
        /// <param name="listener">Bundle监听器实例</param>
        public void RemoveBundleListener(IBundleListener listener)
        {
            bundleListenerList.Remove(listener);
            ((IFrameworkListener)framework).RemoveBundleListener(listener);
        }

        /// <summary>
        /// 添加一个Extension监听器实例
        /// </summary>
        /// <param name="listener">Extension监听器实例</param>
        public void AddExtensionListener(IExtensionListener listener)
        {
            ((IFrameworkListener)framework).AddExtensionListener(listener);
            extensionListenerList.Add(listener);
        }

        /// <summary>
        /// 移除一个Extension监听器实例
        /// </summary>
        /// <param name="listener">Extension监听器实例</param>
        public void RemoveExtensionListener(IExtensionListener listener)
        {
            extensionListenerList.Remove(listener);
            ((IFrameworkListener)framework).RemoveExtensionListener(listener);
        }


        /// <summary>
        /// 添加一个服务监听器实例
        /// </summary>
        /// <param name="listener">服务监听器实例</param>
        public void AddServiceListener(IServiceListener listener)
        {
            ((IFrameworkListener)framework).AddServiceListener(listener);
            serviceListenerList.Add(listener);
        }

        /// <summary>
        /// 移除一个服务监听器
        /// </summary>
        /// <param name="listener">服务监听器实例</param>
        public void RemoveServiceListener(IServiceListener listener)
        {
            serviceListenerList.Remove(listener);
            ((IFrameworkListener)framework).RemoveServiceListener(listener);
        }

        #endregion

        #region Service

        /// <summary>
        /// 实现了多个服务接口的服务对象注册
        /// </summary>
        /// <param name="contracts">服务约束数组</param>
        /// <param name="service">服务对象</param>
        /// <param name="properties">服务属性，在查找服务时用于过滤服务</param>
        /// <returns>服务注册信息</returns>
        public IServiceRegistration RegisterService(string[] contracts, object service, IDictionary<string, object> properties = null)
        {
            IServiceRegistration sr = ((IFrameworkService)framework).RegisterService(this, contracts, service, properties);
            serviceReferenceList.Add(sr.GetReference());

            //触发服变更事件
            foreach (var bundleObj in this.framework.GetBundles())
            {
                var fireContext = (IContextFireEvent)bundleObj.GetBundleContext();
                fireContext.FireServiceChanged(this.bundle, new ServiceEventArgs(ServiceEventArgs.REGISTERED, contracts, sr.GetReference()));
            }

            return sr;
        }

        /// <summary>
        /// 实现了一个服务接口的服务对象注册
        /// </summary>
        /// <param name="contract">服务约束</param>
        /// <param name="service">服务对象</param>
        /// <param name="properties">服务属性</param>
        /// <returns>服务注册信息</returns>
        public IServiceRegistration RegisterService(string contract, object service, IDictionary<string, object> properties = null)
        {
            return RegisterService(new string[] { contract }, service, properties);
        }


        /// <summary>
        /// 以泛型的方式注册服务对象
        /// </summary>
        /// <typeparam name="T">服务约束类型</typeparam>
        /// <param name="service">服务对象</param>
        /// <param name="properties">服务属性</param>
        /// <returns>服务注册信息</returns>
        public IServiceRegistration RegisterService<T>(object service, IDictionary<string, object> properties = null)
        {
            var contract = typeof(T).FullName;

            return RegisterService(contract, service, properties);
        }

        /// <summary>
        /// 取消注册公开的服务对象
        /// </summary>
        /// <param name="serviceReference">服务引用</param>
        public void UnRegisterService(IServiceReference serviceReference)
        {
            if (!serviceReferenceList.Contains(serviceReference)) return;

            //触发服变更事件
            foreach (var bundleObj in this.framework.GetBundles())
            {
                var fireContext = (IContextFireEvent)bundleObj.GetBundleContext();
                fireContext.FireServiceChanged(this.bundle, new ServiceEventArgs(ServiceEventArgs.UNREGISTERING, serviceReference.GetSercieContracts(), serviceReference));
            }

            ((IFrameworkService)framework).UnRegisterService(serviceReference);

            serviceReferenceList.Remove(serviceReference);
        }

        /// <summary>
        /// 以泛型方式根据服务约束类型获取服务
        /// </summary>
        /// <typeparam name="T">服务约束类型</typeparam>
        /// <returns>服务引用</returns>
        public IServiceReference GetServiceReference<T>()
        {
            var contract = typeof(T).FullName;
            return GetServiceReference(contract);
        }

        /// <summary>
        /// 根据服务约束获取服务(根据注册顺序获取第一个服务引用)
        /// </summary>
        /// <param name="contract">服务约束</param>
        /// <returns>服务引用</returns>
        public IServiceReference GetServiceReference(string contract)
        {
            var sr = ((IFrameworkService)framework).GetServiceReference(contract);
            if (sr != null && !bundleUsingServiceReferenceList.Contains(sr))
                bundleUsingServiceReferenceList.Add(sr);
            return sr;
        }

        /// <summary>
        /// 以泛型方式根据服务约束类型获取服务引用列表
        /// </summary>
        /// <typeparam name="T">服务约束类型</typeparam>
        /// <returns>服务引用列表</returns>
        public IList<IServiceReference> GetServiceReferences<T>()
        {
            var contract = typeof(T).FullName;
            return GetServiceReferences(contract);
        }

        /// <summary>
        /// 以泛型方式根据服务约束类型获取服务引用列表
        /// </summary>
        /// <typeparam name="T">服务约束类型</typeparam>
        /// <returns>服务引用列表</returns>
        public IList<IServiceReference> GetServiceReferences<T>(IDictionary<string, object> properties)
        {
            var contract = typeof(T).FullName;
            return GetServiceReferences(contract, properties);
        }

        /// <summary>
        /// 根据服务约束获取服务引用列表
        /// </summary>
        /// <param name="contract">服务约束</param>
        /// <returns>服务引用列表</returns>
        public IList<IServiceReference> GetServiceReferences(string contract)
        {
            var sres = ((IFrameworkService)framework).GetServiceReferences(contract);
            foreach (var sr in sres)
            {
                if (!bundleUsingServiceReferenceList.Contains(sr))
                    bundleUsingServiceReferenceList.Add(sr);
            }
            return sres;
        }


        /// <summary>
        /// 根据服务约束及服务属性获取服务引用列表
        /// </summary>
        /// <param name="contract">服务约束</param>
        /// <param name="properties">服务属性</param>
        /// <returns>服务引用列表</returns>
        public IList<IServiceReference> GetServiceReferences(string contract, IDictionary<string, object> properties)
        {
            var sres = ((IFrameworkService)framework).GetServiceReferences(contract, properties);
            foreach (var sr in sres)
            {
                if (!bundleUsingServiceReferenceList.Contains(sr))
                    bundleUsingServiceReferenceList.Add(sr);
            }
            return sres;
        }

        /// <summary>
        /// 获取正在使用指定服务的所有Bundle模块
        /// </summary>
        /// <param name="reference">服务引用</param>
        /// <returns>正在使用服务的Bundle列表</returns>
        public IList<IBundle> GetUsingBundles(IServiceReference reference)
        {
            return ((IFrameworkService)framework).GetUsingBundles(reference);
        }


        /// <summary>
        /// 根据服务引用获取对应的服务实例
        /// </summary>
        /// <param name="reference">服务引用</param>
        /// <returns>服务对象</returns>
        public object GetService(IServiceReference reference)
        {
            return ((IFrameworkService)framework).GetService(reference, GetBundle());
        }


        /// <summary>
        /// 根据服务引用获取对应的服务实例(强类型版本)
        /// </summary>
        /// <param name="reference">服务引用</param>
        /// <returns>服务对象</returns>
        public T GetService<T>(IServiceReference reference) where T : class
        {
            return ((IFrameworkService)framework).GetService(reference, GetBundle()) as T;
        }

        /// <summary>
        /// 获取对应的服务实例(强类型版本)
        /// </summary>
        /// <returns>服务对象</returns>
        public T GetService<T>() where T : class
        {
            return ((IFrameworkService)framework).GetService(GetServiceReference<T>(), GetBundle()) as T;
        }
        /// <summary>
        /// 获取服务实例( 强类型),
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="properties"></param>
        /// <returns></returns>
        public T GetService<T>(IDictionary<string, object> properties) where T : class
        {
            return ((IFrameworkService)framework).GetService(GetServiceReferences<T>(properties).FirstOrDefault(), GetBundle()) as T;
        }

        /// <summary>
        /// 取消调用指定服务引用的服务实例
        /// </summary>
        /// <param name="reference">服务引用</param>
        /// <returns>是否成功</returns>
        public bool UnGetService(IServiceReference reference)
        {
            if (reference != null && bundleUsingServiceReferenceList.Contains(reference))
                bundleUsingServiceReferenceList.Remove(reference);
            return ((IFrameworkService)framework).UnGetService(reference, GetBundle());
        }

        #endregion

        #region Other
        /// <summary>
        /// 安装指定路径下的Bundle模块
        /// </summary>
        /// <param name="location">Bundle文件全路径</param>
        /// <returns>已安装的Bundle实例</returns>
        public IBundle InstallBundle(string location)
        {
            return ((IFrameworkInstaller)framework).Install(location);
        }


        /// <summary>
        /// 获取所有Bundle模块
        /// </summary>
        /// <returns>Bundle模块列表</returns>
        public IList<IBundle> GetBundles()
        {
            return framework.GetBundles();
        }

        /// <summary>
        /// 获取当前上下文关联的Bundle模块
        /// </summary>
        /// <returns>Bundle模块</returns>
        public IBundle GetBundle()
        {
            return this.bundle;
        }

        /// <summary>
        /// 根据序号获取已装载的指定Bundle
        /// </summary>
        /// <param name="index">序号</param>
        /// <returns>Bundle模块</returns>
        public IBundle GetBundle(int index)
        {
            if (index > 0 && index < framework.GetBundles().Count)
                return framework.GetBundles()[index];
            return framework.GetBundles()[0];
        }

        /// <summary>
        /// 根据程序集名称获取已装载的指定Bundle
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <returns>Bundle模块</returns>
        public IBundle GetBundle(string assemblyName)
        {
            return framework.GetBundles().FirstOrDefault(bundle => bundle.GetSymbolicName().Equals(assemblyName));
        }


        /// <summary>
        /// 停止
        /// </summary>
        internal void Stop()
        {
            //移除服务监听器
            foreach (IServiceListener listener in serviceListenerList)
            {
                ((IFrameworkListener)framework).RemoveServiceListener(listener);
            }
            serviceListenerList.Clear();
            //移除Bundle监听器
            foreach (IBundleListener listener in bundleListenerList)
            {
                ((IFrameworkListener)framework).RemoveBundleListener(listener);
            }
            bundleListenerList.Clear();
            //移除Extension监听器
            foreach (IExtensionListener listener in extensionListenerList)
            {
                ((IFrameworkListener)framework).RemoveExtensionListener(listener);
            }
            extensionListenerList.Clear();
            //移除已注册的服务
            foreach (IServiceReference reference in serviceReferenceList)
            {
                ((IFrameworkService)framework).UnRegisterService(reference);
            }
            serviceReferenceList.Clear();
            //取消使用正在使用的服务
            IServiceReference[] bundleUsingServiceReferences = new IServiceReference[bundleUsingServiceReferenceList.Count];
            bundleUsingServiceReferenceList.CopyTo(bundleUsingServiceReferences, 0);
            foreach (IServiceReference reference in bundleUsingServiceReferences)
            {
                UnGetService(reference);
            }
            bundleUsingServiceReferenceList.Clear();
        }
        #endregion

        #endregion

        #region Event

        /// <summary>
        /// 扩展变更事件
        /// </summary>
        public event EventHandler<ExtensionEventArgs> ExtensionChanged;

        /// <summary>
        /// 服务变更事件
        /// </summary>
        public event EventHandler<ServiceEventArgs> ServiceChanged;


        /// <summary>
        /// 触发扩展变更事件
        /// </summary>
        /// <param name="bundle">引发的模块</param>
        /// <param name="extensionEventArgs">扩展事件参数</param>
        public void FireExtensionChanged(IBundle bundle, ExtensionEventArgs extensionEventArgs)
        {
            if (this.ExtensionChanged != null)
                this.ExtensionChanged(bundle, extensionEventArgs);
        }


        /// <summary>
        /// 触发服务变更事件
        /// </summary>
        /// <param name="bundle">引发的模块</param>
        /// <param name="serviceEventArgs">服务事件参数</param>
        public void FireServiceChanged(IBundle bundle, ServiceEventArgs serviceEventArgs)
        {
            if (this.ServiceChanged != null)
                this.ServiceChanged(bundle, serviceEventArgs);
        }      

        #endregion

    }
}
