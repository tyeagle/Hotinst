using System.Windows;

namespace HOTINST.COMMON.Controls.Controls.PackIcon
{
    /// <summary>
    /// Icons from the Material Design Icons project, <see><cref>https://materialdesignicons.com/</cref></see>.
    /// </summary>
    public class PackIconMaterialLight : PackIconControl<PackIconMaterialLightKind>
    {
        static PackIconMaterialLight()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PackIconMaterialLight), new FrameworkPropertyMetadata(typeof(PackIconMaterialLight)));
        }

        /// <summary>
        /// 
        /// </summary>
        public PackIconMaterialLight() : base(PackIconMaterialLightDataFactory.Create)
        {

        }
    }
}