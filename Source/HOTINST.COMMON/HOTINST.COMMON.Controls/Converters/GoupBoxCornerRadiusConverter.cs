/**
 * ==============================================================================
 *
 * ClassName: GoupBoxCornerRadioConverter
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/3/21 14:25:34
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
using System.Windows;
using System.Windows.Data;

namespace HOTINST.COMMON.Controls.Converters
{
	/// <summary>
	/// GroupBox 边框圆角转换器。
	/// </summary>
	public class GoupBoxCornerRadiusConverter : IValueConverter
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
			CornerRadius corner = (CornerRadius)value;
			bool param = System.Convert.ToBoolean(parameter);
			return param
				? new CornerRadius(corner.TopLeft, corner.TopRight, 0, 0)
				: new CornerRadius(0, 0, corner.BottomLeft, corner.BottomRight);
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