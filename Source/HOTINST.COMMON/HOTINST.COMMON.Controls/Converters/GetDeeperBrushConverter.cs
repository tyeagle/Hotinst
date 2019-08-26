/**
 * ==============================================================================
 *
 * ClassName: GetDeeperBrushConverter
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/3/22 16:59:33
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
	/// 获取较深颜色画刷的转换器
	/// </summary>
	public class GetDeeperBrushConverter : IValueConverter
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			SolidColorBrush brush = value as SolidColorBrush;
			return brush != null ? new SolidColorBrush(Color.FromArgb(brush.Color.A, brush.Color.R, (byte)(brush.Color.G / 2), brush.Color.B)) : value;
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