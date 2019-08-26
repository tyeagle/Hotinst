/**
 * ==============================================================================
 *
 * ClassName: MarkupConverter
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/5/18 15:56:01
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
using System.Windows.Markup;

namespace HOTINST.COMMON.Controls.Converters
{
	/// <summary>
	/// 
	/// </summary>
	[MarkupExtensionReturnType(typeof(IValueConverter))]
	public abstract class MarkupConverter : MarkupExtension, IValueConverter
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceProvider"></param>
		/// <returns></returns>
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return this;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		protected abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		protected abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

		object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			try
			{
				return Convert(value, targetType, parameter, culture);
			}
			catch
			{
				return DependencyProperty.UnsetValue;
			}
		}

		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			try
			{
				return ConvertBack(value, targetType, parameter, culture);
			}
			catch
			{
				return DependencyProperty.UnsetValue;
			}
		}
	}
}