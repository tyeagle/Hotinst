#pragma warning disable 1591

using System.Windows;

namespace HOTINST.COMMON.Controls.Controls.PackIcon
{
    /// <summary>
    /// Icons from the FontAwesome Icons project, <see><cref>http://fontawesome.io</cref></see>.
    /// </summary>
    public class PackIconFontAwesome : PackIconControl<PackIconFontAwesomeKind>
    {
        static PackIconFontAwesome()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PackIconFontAwesome), new FrameworkPropertyMetadata(typeof(PackIconFontAwesome)));
        }

        public PackIconFontAwesome() : base(PackIconFontAwesomeDataFactory.Create)
        {

        }
    }
}