﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using HOTINST.OSGI.Extension;

namespace HOTINST.OSGI.Core
{
    /// <summary>
    /// Bundle表示由内核创建的一个插件
    /// </summary>
    public interface IBundle
    {
        /// <summary>
        /// 启动
        /// </summary>
        void Start();

        /// <summary>
        /// 停止
        /// </summary>
        void Stop();

        /// <summary>
        /// 获取当前Bundle状态
        /// </summary>
        /// <returns>Bundle状态</returns>
        int GetState();

        /// <summary>
        /// 获取当前Bundle版本信息
        /// </summary>
        /// <returns>Bundle版本信息</returns>
        Version GetVersion();
        /// <summary>
        /// 获取当前Bundle的描述信息
        /// </summary>
        /// <returns></returns>
        String GetDescription();

        /// <summary>
        /// 获取当前Bundle符号名称
        /// </summary>
        /// <returns>Bundle符号名称</returns>
        string GetSymbolicName();

        /// <summary>
        /// 获取当前Bundle程序集全名
        /// </summary>
        /// <returns>Bundle程序集全名</returns>
        string GetBundleAssemblyFullName();

        /// <summary>
        /// 获取当前Bundle程序集清单数据
        /// </summary>
        /// <returns>Bundle程序清单数据</returns>
        IDictionary<string, string> GetManifest();

        /// <summary>
        /// 获取当前Bundle上下文对象
        /// </summary>
        /// <returns>Bundle上下文对象</returns>
        IBundleContext GetBundleContext();

        /// <summary>
        /// 获取当前Bundle扩展点
        /// </summary>
        /// <returns>扩展点列表</returns>
        IList<ExtensionPoint> GetExtensionPoints();

        /// <summary>
        /// 获取当前Bundle扩展的扩展数据
        /// </summary>
        /// <returns>扩展数据列表</returns>
        IList<ExtensionData> GetExtensionDatas();

        /// <summary>
        /// 获取当前Bundle目录
        /// </summary>
        /// <returns>Bundle目录</returns>
        string GetBundleDirectoryPath();

        /// <summary>
        /// 获取当前Bundle程序集文件名称
        /// </summary>
        /// <returns>Bundle程序集文件名称</returns>
        string GetBundleAssemblyFileName();

        /// <summary>
        /// 获取模块清单数据
        /// </summary>
        /// <returns>清单数据节点</returns>
        XmlNode GetBundleManifestData();

        /// <summary>
        /// 获取当前Bundle启动级别
        /// </summary>
        /// <returns>Bundle启动级别</returns>
        int GetBundleStartLevel();

        /// <summary>
        /// 指定路径更新当前Bundle
        /// </summary>
        /// <param name="zipFile">更新的Bundle路径</param>
        void Update(string zipFile);

        /// <summary>
        /// 卸载当前Bundle
        /// </summary>
        void UnInstall();
    }
}
