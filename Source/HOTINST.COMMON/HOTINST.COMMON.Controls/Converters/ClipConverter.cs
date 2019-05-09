/**
 * ==============================================================================
 *
 * ClassName: ClipConverter
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/3/23 15:07:39
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
	public class ClipConverter : IMultiValueConverter
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="values"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if(values[0] is double && values[1] is double && values.Length > 0)
			{
				double width = (double)values[0];
				double height = (double)values[1];
				return new Rect(0.0, 0.0, width, height);
			}
			return new Rect(0.0, 0.0, 1.7976931348623157E+308, 1.7976931348623157E+308);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}