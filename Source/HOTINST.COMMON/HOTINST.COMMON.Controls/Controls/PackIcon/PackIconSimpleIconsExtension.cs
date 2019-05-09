using System.Windows.Markup;

namespace HOTINST.COMMON.Controls.Controls.PackIcon
{
    /// <summary>
    /// 
    /// </summary>
    [MarkupExtensionReturnType(typeof(PackIconSimpleIcons))]
    public class SimpleIconsExtension : PackIconExtension<PackIconSimpleIcons, PackIconSimpleIconsKind>
    {
        /// <summary>
        /// 
        /// </summary>
        public SimpleIconsExtension()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kind"></param>
        public SimpleIconsExtension(PackIconSimpleIconsKind kind) : base(kind)
        {
        }
    }
}