﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace HOTINST.COMMON.Controls.Converters.Internal
{
	/// <summary>
	/// 
	/// </summary>
	public class DropShdwWidthConverter : IValueConverter
	{
		#region Implementation of IValueConverter

		/// <summary>转换值。</summary>
		/// <returns>转换后的值。如果该方法返回 null，则使用有效的 null 值。</returns>
		/// <param name="value">绑定源生成的值。</param>
		/// <param name="targetType">绑定目标属性的类型。</param>
		/// <param name="parameter">要使用的转换器参数。</param>
		/// <param name="culture">要用在转换器中的区域性。</param>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value is double val && !double.IsNaN(val))
			{
				return val + 5;
			}
			return value;
		}

		/// <summary>转换值。</summary>
		/// <returns>转换后的值。如果该方法返回 null，则使用有效的 null 值。</returns>
		/// <param name="value">绑定目标生成的值。</param>
		/// <param name="targetType">要转换到的类型。</param>
		/// <param name="parameter">要使用的转换器参数。</param>
		/// <param name="culture">要用在转换器中的区域性。</param>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}