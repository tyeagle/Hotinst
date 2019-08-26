using System;
using System.Collections.Generic;
using System.Text;

using HOTINST.OSGI.Event;

namespace HOTINST.OSGI.Listener
{

    /// <summary>
    /// 服务状态监听器
    /// </summary>
    public interface IServiceListener
    {
        /// <summary>
        /// 服务状态变更
        /// </summary>
        /// <param name="serviceEvent">服务事件参数</param>
        void ServiceChanged(ServiceEventArgs serviceEvent);
    }
}