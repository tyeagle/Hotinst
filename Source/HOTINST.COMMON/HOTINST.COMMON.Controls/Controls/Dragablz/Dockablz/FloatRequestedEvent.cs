using System.Windows;

#pragma warning disable 1591

namespace HOTINST.COMMON.Controls.Controls.Dragablz.Dockablz
{
    public delegate void FloatRequestedEventHandler(object sender, FloatRequestedEventArgs e);

    public class FloatRequestedEventArgs : DragablzItemEventArgs
    {
        public FloatRequestedEventArgs(RoutedEvent routedEvent, object source, DragablzItem dragablzItem) 
            : base(routedEvent, source, dragablzItem)
        { }

        public FloatRequestedEventArgs(RoutedEvent routedEvent, DragablzItem dragablzItem) 
            : base(routedEvent, dragablzItem)
        { }
    }
}
