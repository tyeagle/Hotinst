using System;

namespace HOTINST.COMMON.Controls.Controls.Dragablz.Referenceless
{
    internal interface ICancelable : IDisposable
    {
        bool IsDisposed { get; }
    }
}
