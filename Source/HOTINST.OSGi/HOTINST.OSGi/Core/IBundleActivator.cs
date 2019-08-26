using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HOTINST.OSGI.Core
{
    /// <summary>
    /// Bundle激活器,每一个插件需要且只应该实现一个该接口的public对象，框架内核在加载插件的时候会自动创建该对象并
    /// 在合适的时机调用Start,Stoop方法
    /// </summary>
    public interface IBundleActivator
    {
        /// <summary>
        /// 激活器启动
        /// </summary>
        /// <param name="context"></param>
        void Start(IBundleContext context);

        /// <summary>
        /// 激活器停止
        /// </summary>
        /// <param name="context"></param>
        void Stop(IBundleContext context);

    }
}
