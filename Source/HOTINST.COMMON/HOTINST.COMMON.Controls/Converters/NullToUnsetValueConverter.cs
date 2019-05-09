/**
 * ==============================================================================
 *
 * ClassName: NullToUnsetValueConverter
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/5/18 15:56:50
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

namespace HOTINST.COMMON.Controls.Converters
{
	/// <summary>
	/// 
	/// </summary>
	public class NullToUnsetValueConverter : MarkupConverter
	{
		private static NullToUnsetValueConverter _instance;

		// Explicit static constructor to tell C# compiler
		// not to mark type as beforefieldinit
		static NullToUnsetValueConverter()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		public NullToUnsetValueConverter()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceProvider"></param>
		/// <returns></returns>
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return _instance ?? (_instance = new NullToUnsetValueConverter());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		protected override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value ?? DependencyProperty.UnsetValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		protected override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}