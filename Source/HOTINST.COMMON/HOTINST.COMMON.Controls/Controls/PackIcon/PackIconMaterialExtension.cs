using System.Windows.Markup;

namespace HOTINST.COMMON.Controls.Controls.PackIcon
{
    /// <summary>
    /// 
    /// </summary>
    [MarkupExtensionReturnType(typeof(PackIconMaterial))]
    public class MaterialExtension : PackIconExtension<PackIconMaterial, PackIconMaterialKind>
    {
        /// <summary>
        /// 
        /// </summary>
        public MaterialExtension()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kind"></param>
        public MaterialExtension(PackIconMaterialKind kind) : base(kind)
        {
        }
    }
}