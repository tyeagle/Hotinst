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
    /// 表示模块上下文，每一个模块有一个自己的BundleContex,模块初始化时由框架传递给用户模块的激活器，
    ///  IBundleContext暴露了内核的操作,用户模块使用该接口访问内核框架的各种功能
    /// </summary>
    public interface IBundleContext
    {

        /// <summary>
        /// 获取当前上下文关联的Bundle模块
        /// </summary>
        /// <returns>Bundle模块</returns>
        IBundle GetBundle();

        /// <summary>
        /// 根据序号获取已装载的指定Bundle
        /// </summary>
        /// <param name="index">序号</param>
        /// <returns>Bundle模块</returns>
        IBundle GetBundle(int index);

        /// <summary>
        /// 根据程序集名称获取已装载的指定Bundle
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <returns>Bundle模块</returns>
        IBundle GetBundle(string assemblyName);

        /// <summary>
        /// 获取所有Bundle模块
        /// </summary>
        /// <returns>Bundle模块列表</returns>
        IList<IBundle> GetBundles();

        /// <summary>
        /// 安装指定路径下的Bundle模块
        /// </summary>
        /// <param name="location">Bundle文件全路径</param>
        /// <returns>已安装的Bundle实例</returns>
        IBundle InstallBundle(string location);

        /// <summary>
        /// 添加一个Bundle监听器实例
        /// </summary>
        /// <param name="listener">Bundle监听器实例</param>
        void AddBundleListener(IBundleListener listener);

        /// <summary>
        /// 移除一个Bundle监听器实例
        /// </summary>
        /// <param name="listener">Bundle监听器实例</param>
        void RemoveBundleListener(IBundleListener listener);

        /// <summary>
        /// 添加一个Extension监听器实例
        /// </summary>
        /// <param name="listener">Extension监听器实例</param>
        void AddExtensionListener(IExtensionListener listener);

        /// <summary>
        /// 移除一个Extension监听器实例
        /// </summary>
        /// <param name="listener">Extension监听器实例</param>
        void RemoveExtensionListener(IExtensionListener listener);

        /// <summary>
        /// 添加一个服务监听器实例
        /// </summary>
        /// <param name="listener">服务监听器实例</param>
        void AddServiceListener(IServiceListener listener);

        /// <summary>
        /// 移除一个服务监听器
        /// </summary>
        /// <param name="listener">服务监听器实例</param>
        void RemoveServiceListener(IServiceListener listener);

        /// <summary>
        /// 实现了一个服务接口的服务对象注册
        /// </summary>
        /// <param name="contract">服务约束</param>
        /// <param name="service">服务对象</param>
        /// <param name="properties">服务属性</param>
        /// <returns>服务注册信息</returns>
        IServiceRegistration RegisterService(string contract, object service, IDictionary<string, object> properties = null);

        /// <summary>
        /// 实现了多个服务接口的服务对象注册
        /// </summary>
        /// <param name="contracts">服务约束数组</param>
        /// <param name="service">服务对象</param>
        /// <param name="properties">服务属性</param>
        /// <returns>服务注册信息</returns>
        IServiceRegistration RegisterService(string[] contracts, object service, IDictionary<string, object> properties = null);

        /// <summary>
        /// 以泛型的方式注册服务对象
        /// </summary>
        /// <typeparam name="T">服务约束类型</typeparam>
        /// <param name="service">服务对象</param>
        /// <param name="properties">服务属性</param>
        /// <returns>服务注册信息</returns>
        IServiceRegistration RegisterService<T>(object service, IDictionary<string, object> properties = null);

        /// <summary>
        /// 取消注册公开的服务对象
        /// </summary>
        /// <param name="serviceReference">服务引用</param>
        void UnRegisterService(IServiceReference serviceReference);

        /// <summary>
        /// 根据服务约束获取服务(根据注册顺序获取第一个服务引用)
        /// </summary>
        /// <param name="contract">服务约束</param>
        /// <returns>服务引用</returns>
        IServiceReference GetServiceReference(string contract);

        /// <summary>
        /// 根据服务约束获取服务引用列表
        /// </summary>
        /// <param name="contract">服务约束</param>
        /// <returns>服务引用列表</returns>
        IList<IServiceReference> GetServiceReferences(string contract);


        /// <summary>
        /// 根据服务约束及服务属性获取服务引用列表
        /// </summary>
        /// <param name="contract">服务约束</param>
        /// <param name="properties">服务属性</param>
        /// <returns>服务引用列表</returns>
        IList<IServiceReference> GetServiceReferences(string contract, IDictionary<string, object> properties);

        /// <summary>
        /// 以泛型方式根据服务约束类型获取服务
        /// </summary>
        /// <typeparam name="T">服务约束类型</typeparam>
        /// <returns>服务引用</returns>
        IServiceReference GetServiceReference<T>();

        /// <summary>
        /// 以泛型方式根据服务约束类型获取服务引用列表
        /// </summary>
        /// <typeparam name="T">服务约束类型</typeparam>
        /// <returns>服务引用列表</returns>
        IList<IServiceReference> GetServiceReferences<T>();

        /// <summary>
        /// 以泛型方式根据服务约束类型获取服务引用列表
        /// </summary>
        /// <typeparam name="T">服务约束类型</typeparam>
        /// <param name="properties">服务属性</param>
        /// <returns>服务引用列表</returns>
        IList<IServiceReference> GetServiceReferences<T>(IDictionary<string, object> properties);

        /// <summary>
        /// 根据服务引用获取对应的服务实例
        /// </summary>
        /// <param name="reference">服务引用</param>
        /// <returns>服务对象</returns>
        object GetService(IServiceReference reference);

        /// <summary>
        /// 获取服务实例(强类型版本)
        /// </summary>
        /// <returns>服务对象</returns>
        T GetService<T>() where T : class;
        /// <summary>
        /// 获取服务实例( 强类型),
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="properties"></param>
        /// <returns></returns>
        T GetService<T>(IDictionary<string, object> properties) where T : class;

        /// <summary>
        /// 根据服务引用获取对应的服务实例(强类型版本)
        /// </summary>
        /// <param name="reference">服务引用</param>
        /// <returns>服务对象</returns>
        T GetService<T>(IServiceReference reference) where T : class;

        /// <summary>
        /// 获取正在使用指定服务的所有Bundle模块
        /// </summary>
        /// <param name="reference">服务引用</param>
        /// <returns>正在使用服务的Bundle列表</returns>
        IList<IBundle> GetUsingBundles(IServiceReference reference);

        /// <summary>
        /// 取消调用指定服务引用的服务实例
        /// </summary>
        /// <param name="reference">服务引用</param>
        /// <returns>是否成功</returns>
        bool UnGetService(IServiceReference reference);

        /// <summary>
        /// 扩展点变更事件
        /// </summary>
        event EventHandler<ExtensionEventArgs> ExtensionChanged;

        /// <summary>
        /// 服务变更事件
        /// </summary>
        event EventHandler<ServiceEventArgs> ServiceChanged;
    }
}
