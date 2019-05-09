using System;
using System.Collections.Generic;
using System.Text;

using HOTINST.OSGI.Event;

namespace HOTINST.OSGI.Listener
{

    /// <summary>
    /// Bundle状态监听器
    /// </summary>
    public interface IBundleListener
    {
        /// <summary>
        /// Bundle状态变更
        /// </summary>
        /// <param name="e">Bundle事件参数</param>
        void BundleChanged(BundleEventArgs e);
    }
}
