using System.Windows;

namespace HOTINST.COMMON.Controls.Controls.PackIcon
{
    /// <summary>
    /// All SVG icons for popular brands, maintained by Dan Leech <see><cref>https://twitter.com/bathtype</cref></see>.
    /// Contributions, corrections and requests can be made on GitHub <see><cref>https://github.com/danleech/simple-icons</cref></see>.
    /// </summary>
    public class PackIconSimpleIcons : PackIconControl<PackIconSimpleIconsKind>
    {
        static PackIconSimpleIcons()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PackIconSimpleIcons), new FrameworkPropertyMetadata(typeof(PackIconSimpleIcons)));
        }

        /// <summary>
        /// 
        /// </summary>
        public PackIconSimpleIcons() : base(PackIconSimpleIconsDataFactory.Create)
        {

        }
    }
}