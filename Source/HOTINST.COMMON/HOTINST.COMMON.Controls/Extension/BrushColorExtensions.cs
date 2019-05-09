/**
 * ==============================================================================
 *
 * ClassName: BrushColorExtension
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/3/29 13:55:23
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System.Windows.Media;

namespace HOTINST.COMMON.Controls.Extension
{
	/// <summary>
	/// 
	/// </summary>
	public static class BrushColorExtensions
	{
		/// <summary>
		/// 将Color转换成Brush
		/// </summary>
		/// <param name="color">要转换的Color</param>
		/// <returns>Brush</returns>
		public static Brush ToBrush(this Color color)
		{
			return new SolidColorBrush(color);
		}

		/// <summary>
		/// 将Brush转换成Color
		/// </summary>
		/// <param name="brush">要转换的Brush</param>
		/// <returns>Color</returns>
		public static Color ToColor(this Brush brush)
		{
			SolidColorBrush scb = (SolidColorBrush)brush;
			return Color.FromRgb(scb.Color.R, scb.Color.G, scb.Color.B);
		}
	}
}