using System.Windows.Controls;

#pragma warning disable 1591

namespace HOTINST.COMMON.Controls.Controls.Dragablz
{
    public class HorizontalOrganiser : StackOrganiser
    {
        public HorizontalOrganiser() : base(Orientation.Horizontal)
        { }

        public HorizontalOrganiser(double itemOffset) : base(Orientation.Horizontal, itemOffset)
        { }
    }    
}