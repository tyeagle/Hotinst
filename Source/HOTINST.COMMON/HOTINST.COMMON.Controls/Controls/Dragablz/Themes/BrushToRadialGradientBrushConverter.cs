using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace HOTINST.COMMON.Controls.Controls.Dragablz.Themes
{
	/// <summary>
	/// 
	/// </summary>
    public class BrushToRadialGradientBrushConverter : IValueConverter
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
            var solidColorBrush = value as SolidColorBrush;
            if (solidColorBrush == null) return Binding.DoNothing;

            return new RadialGradientBrush(solidColorBrush.Color, Colors.Transparent)
            {
                Center = new Point(.5, .5),
                GradientOrigin = new Point(.5, .5),
                RadiusX = .5,
                RadiusY = .5,
                Opacity = .39
            };
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