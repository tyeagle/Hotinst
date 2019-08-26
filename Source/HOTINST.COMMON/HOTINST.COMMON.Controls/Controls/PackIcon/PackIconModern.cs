using System.Windows;

namespace HOTINST.COMMON.Controls.Controls.PackIcon
{
    /// <summary>
    /// Icons from the Modern UI Icons project, <see><cref>http://modernuiicons.com</cref></see>.
    /// </summary>
    public class PackIconModern : PackIconControl<PackIconModernKind>
    {
        static PackIconModern()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PackIconModern), new FrameworkPropertyMetadata(typeof(PackIconModern)));
        }

        /// <summary>
        /// 
        /// </summary>
        public PackIconModern() : base(PackIconModernDataFactory.Create)
        {

        }
    }
}