using System.Windows;

#pragma warning disable 1591

namespace HOTINST.COMMON.Controls.Controls.Dragablz
{
    public interface INewTabHost<out TElement> where TElement : UIElement
    {
        TElement Container { get; }
        TabablzControl TabablzControl { get; }
    }
}