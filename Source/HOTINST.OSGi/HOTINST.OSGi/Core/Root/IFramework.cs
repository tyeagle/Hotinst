﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HOTINST.OSGI.Event;
using HOTINST.OSGI.Listener;
using HOTINST.OSGI.Service;

namespace HOTINST.OSGI.Core.Root
{
    /// <summary>
    /// 框架内核，内核也作为一个Bundle，用来启动其他Bundle组件
    /// </summary>
    public interface IFramework : IBundle
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void Init();

        /// <summary>
        /// 获取所有Bundle模块
        /// </summary>
        /// <returns>所有已装载Bundle列表</returns>
        IList<IBundle> GetBundles();
    }
}

