using System.Windows.Markup;

namespace HOTINST.COMMON.Controls.Controls.PackIcon
{
	/// <summary>
	/// 
	/// </summary>
    [MarkupExtensionReturnType(typeof(PackIconEntypo))]
    public class EntypoExtension : PackIconExtension<PackIconEntypo, PackIconEntypoKind>
    {
		/// <summary>
		/// 
		/// </summary>
        public EntypoExtension()
        {
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="kind"></param>
        public EntypoExtension(PackIconEntypoKind kind) : base(kind)
        {
        }
    }
}