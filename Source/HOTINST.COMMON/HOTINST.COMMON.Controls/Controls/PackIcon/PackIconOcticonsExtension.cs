using System.Windows.Markup;

namespace HOTINST.COMMON.Controls.Controls.PackIcon
{
    /// <summary>
    /// 
    /// </summary>
    [MarkupExtensionReturnType(typeof(PackIconOcticons))]
    public class OcticonsExtension : PackIconExtension<PackIconOcticons, PackIconOcticonsKind>
    {
        /// <summary>
        /// 
        /// </summary>
        public OcticonsExtension()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kind"></param>
        public OcticonsExtension(PackIconOcticonsKind kind) : base(kind)
        {
        }
    }
}