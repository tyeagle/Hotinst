using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HOTINST.COMMON.Controls.Converters
{
	/// <summary>
	/// 
	/// </summary>
    public class EqualityToVisibilityConverter : IValueConverter
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
            return Equals(value, parameter)
                ? Visibility.Visible
                : Visibility.Collapsed;
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
            return Binding.DoNothing;
        }
    }
}
