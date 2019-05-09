/**
 * ==============================================================================
 * Classname   : ReverseBooleanConverter
 * Description : 反转 Bool 类型的值转换器。
 *
 * Compiler    : Visual Studio 2013
 * CLR Version : 4.0.30319.42000
 * Created     : 2017/3/16 18:38:31
 *
 * Author  : caixs
 * Company : Hotinst
 *
 * Copyright © Hotinst 2017. All rights reserved.
 * ==============================================================================
 */

using System;
using System.Globalization;
using System.Windows.Data;

namespace HOTINST.COMMON.Controls.Converters
{
	/// <summary>
	/// 反转 Bool 类型的值转换器。
	/// </summary>
	public class ReverseBooleanConverter : IValueConverter
	{
		#region IValueConverter 成员

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
			return !(bool)value;
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
			return !(bool)value;
		}

		#endregion
	}
}