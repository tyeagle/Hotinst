/**
 * ==============================================================================
 *
 * ClassName: GetTranslucentBrushConverter
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/3/21 14:12:13
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace HOTINST.COMMON.Controls.Converters
{
	/// <summary>
	/// 获取颜色的半透明画刷的转换器。
	/// </summary>
	public class GetTranslucentBrushConverter : IValueConverter
	{
		/// <summary>
		/// 要使用的透明度值
		/// </summary>
		public double Opacity { get; set; } = 0.5;

		/// <summary>
		/// 根据设置的透明度返回该画刷
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			SolidColorBrush brush = value as SolidColorBrush;
			return brush != null
				? new SolidColorBrush(Color.FromArgb((byte)(brush.Color.A * Opacity), brush.Color.R, brush.Color.G, brush.Color.B))
				: value;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException("this should not be called");
		}
	}
}