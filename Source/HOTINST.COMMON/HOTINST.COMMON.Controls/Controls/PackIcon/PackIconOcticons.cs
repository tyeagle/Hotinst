using System.Windows;

namespace HOTINST.COMMON.Controls.Controls.PackIcon
{
    /// <summary>
    /// Icons from GitHub Octicons - <see><cref>https://octicons.github.com</cref></see>
    /// </summary>
    public class PackIconOcticons : PackIconControl<PackIconOcticonsKind>
    {
        static PackIconOcticons()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PackIconOcticons), new FrameworkPropertyMetadata(typeof(PackIconOcticons)));
        }

        /// <summary>
        /// 
        /// </summary>
        public PackIconOcticons() : base(PackIconOcticonsDataFactory.Create)
        {

        }
    }
}