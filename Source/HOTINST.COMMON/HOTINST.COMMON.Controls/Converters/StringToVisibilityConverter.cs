/**
 * ==============================================================================
 *
 * ClassName: StringToVisibilityConverter
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/5/18 15:57:41
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
	///     Converts a String into a Visibility enumeration (and back)
	///     The FalseEquivalent can be declared with the "FalseEquivalent" property
	/// </summary>
	[ValueConversion(typeof(string), typeof(Visibility))]
	[MarkupExtensionReturnType(typeof(StringToVisibilityConverter))]
	public class StringToVisibilityConverter : MarkupExtension, IValueConverter
	{
		/// <summary>
		///     Initialize the properties with standard values
		/// </summary>
		public StringToVisibilityConverter()
		{
			FalseEquivalent = Visibility.Collapsed;
			OppositeStringValue = false;
		}

		/// <summary>
		///     FalseEquivalent (default : Visibility.Collapsed => see Constructor)
		/// </summary>
		public Visibility FalseEquivalent { get; set; }

		/// <summary>
		///     Define whether the opposite boolean value is crucial (default : false)
		/// </summary>
		public bool OppositeStringValue { get; set; }

		#region MarkupExtension "overrides"

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceProvider"></param>
		/// <returns></returns>
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return new StringToVisibilityConverter
			{
				FalseEquivalent = FalseEquivalent,
				OppositeStringValue = OppositeStringValue
			};
		}

		#endregion

		#region IValueConverter Members

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
			if((value == null || value is string) && targetType == typeof(Visibility))
			{
				if(OppositeStringValue)
				{
					return string.IsNullOrEmpty((string)value) ? Visibility.Visible : FalseEquivalent;
				}
				return string.IsNullOrEmpty((string)value) ? FalseEquivalent : Visibility.Visible;
			}
			return value;
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
			if(value is Visibility)
			{
				if(OppositeStringValue)
				{
					return ((Visibility)value == Visibility.Visible) ? String.Empty : "visible";
				}
				return ((Visibility)value == Visibility.Visible) ? "visible" : String.Empty;
			}
			return value;
		}

		#endregion
	}
}