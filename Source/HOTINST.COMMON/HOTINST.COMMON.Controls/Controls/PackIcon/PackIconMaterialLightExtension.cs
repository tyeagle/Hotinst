using System.Windows.Markup;

namespace HOTINST.COMMON.Controls.Controls.PackIcon
{
    /// <summary>
    /// 
    /// </summary>
    [MarkupExtensionReturnType(typeof(PackIconMaterialLight))]
    public class MaterialLightExtension : PackIconExtension<PackIconMaterialLight, PackIconMaterialLightKind>
    {
        /// <summary>
        /// 
        /// </summary>
        public MaterialLightExtension()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kind"></param>
        public MaterialLightExtension(PackIconMaterialLightKind kind) : base(kind)
        {
        }
    }
}