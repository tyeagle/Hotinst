using System.Windows;

namespace HOTINST.COMMON.Controls.Controls.PackIcon
{
    /// <summary>
    /// Icons from the Material Design Icons project, <see><cref>https://materialdesignicons.com/</cref></see>.
    /// </summary>
    public class PackIconMaterial : PackIconControl<PackIconMaterialKind>
    {
        static PackIconMaterial()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PackIconMaterial), new FrameworkPropertyMetadata(typeof(PackIconMaterial)));
        }

        /// <summary>
        /// 
        /// </summary>
        public PackIconMaterial() : base(PackIconMaterialDataFactory.Create)
        {
            
        }
    }
}