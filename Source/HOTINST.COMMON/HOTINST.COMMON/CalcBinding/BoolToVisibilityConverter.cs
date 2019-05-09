using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HOTINST.COMMON.CalcBinding
{
    /// <summary>
    /// Common BoolToVisibility converter with FalseToVisibility parameter
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
		/// <summary>
		/// 
		/// </summary>
        public BoolToVisibilityConverter()
        {
            FalseToVisibility = FalseToVisibility.Collapsed;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="falseToVisibility"></param>
        public BoolToVisibilityConverter(FalseToVisibility falseToVisibility)
        {
            FalseToVisibility = falseToVisibility;
        }

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
            if ((bool)value)
                return Visibility.Visible;

            return (FalseToVisibility == FalseToVisibility.Collapsed) ? Visibility.Collapsed : Visibility.Hidden;
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
            return (Visibility)value == Visibility.Visible;
        }

		/// <summary>
		/// 
		/// </summary>
        public FalseToVisibility FalseToVisibility { get; set; }
    }
}
