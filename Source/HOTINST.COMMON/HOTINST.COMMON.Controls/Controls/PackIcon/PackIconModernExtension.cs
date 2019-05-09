using System.Windows.Markup;

namespace HOTINST.COMMON.Controls.Controls.PackIcon
{
    /// <summary>
    /// 
    /// </summary>
    [MarkupExtensionReturnType(typeof(PackIconModern))]
    public class ModernExtension : PackIconExtension<PackIconModern, PackIconModernKind>
    {
        /// <summary>
        /// 
        /// </summary>
        public ModernExtension()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kind"></param>
        public ModernExtension(PackIconModernKind kind) : base(kind)
        {
        }
    }
}