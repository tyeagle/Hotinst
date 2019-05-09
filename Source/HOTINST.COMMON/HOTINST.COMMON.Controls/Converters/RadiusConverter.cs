/**
 * ==============================================================================
 *
 * ClassName: RadiusConverter
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/3/23 15:07:12
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
	/// 
	/// </summary>
	public class RadiusConverter : IValueConverter
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
			if(value is CornerRadius)
			{
				return ((CornerRadius)value).TopLeft;
			}
			return null;
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
			throw new NotImplementedException();
		}
	}
}