using System.Windows;

namespace HOTINST.COMMON.Controls.Controls.PackIcon
{
    /// <summary>
    /// Icons from Entypo+ Icons Font - <see><cref>http://www.entypo.com</cref></see>.
    /// </summary>
    public class PackIconEntypo : PackIconControl<PackIconEntypoKind>
    {
        static PackIconEntypo()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PackIconEntypo), new FrameworkPropertyMetadata(typeof(PackIconEntypo)));
        }

		/// <summary>
		/// 
		/// </summary>
        public PackIconEntypo() : base(PackIconEntypoDataFactory.Create)
        {

        }
    }
}