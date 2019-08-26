using System.Windows.Markup;

namespace HOTINST.COMMON.Controls.Controls.PackIcon
{
    /// <summary>
    /// 
    /// </summary>
    [MarkupExtensionReturnType(typeof(PackIconFontAwesome))]
    public class FontAwesomeExtension : PackIconExtension<PackIconFontAwesome, PackIconFontAwesomeKind>
    {
        /// <summary>
        /// 
        /// </summary>
        public FontAwesomeExtension()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kind"></param>
        public FontAwesomeExtension(PackIconFontAwesomeKind kind) : base(kind)
        {
        }
    }
}