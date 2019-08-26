using System.Windows;
using System.Windows.Controls;

#pragma warning disable 1591

namespace HOTINST.COMMON.Controls.Controls.Dragablz
{    
    public class DragablzIcon : Control
    {
        static DragablzIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DragablzIcon), new FrameworkPropertyMetadata(typeof(DragablzIcon)));
        }
    }
}
