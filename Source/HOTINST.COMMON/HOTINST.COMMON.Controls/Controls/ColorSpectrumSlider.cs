/**
 * ==============================================================================
 *
 * ClassName: ColorSpectrumSlider
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/3/30 10:40:48
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using HOTINST.COMMON.Controls.Core;

namespace HOTINST.COMMON.Controls.Controls
{
	/// <summary>
	/// ColorSpectrumSlider
	/// </summary>
	[TemplatePart(Name = PART_SpectrumDisplay, Type = typeof(Rectangle))]
	public class ColorSpectrumSlider : Slider
	{
		#region fields

		private const string PART_SpectrumDisplay = "PART_SpectrumDisplay";

		private Rectangle _spectrumDisplay;
		private LinearGradientBrush _pickerBrush;

		#endregion

		#region props

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register(
			"SelectedColor", typeof(Color), typeof(ColorSpectrumSlider), new PropertyMetadata(default(Color)));
		/// <summary>
		/// 
		/// </summary>
		public Color SelectedColor
		{
			get { return (Color)GetValue(SelectedColorProperty); }
			set { SetValue(SelectedColorProperty, value); }
		}

		#endregion

		#region .ctor

		/// <summary>
		/// 
		/// </summary>
		static ColorSpectrumSlider()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorSpectrumSlider), new FrameworkPropertyMetadata(typeof(ColorSpectrumSlider)));
		}

		/// <summary>
		/// 
		/// </summary>
		public ColorSpectrumSlider()
		{

		}

		#endregion

		#region Base Class Overrides

		/// <summary>
		/// 
		/// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_spectrumDisplay = (Rectangle)GetTemplateChild(PART_SpectrumDisplay);
			CreateSpectrum();
			OnValueChanged(double.NaN, Value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>
		protected override void OnValueChanged(double oldValue, double newValue)
		{
			base.OnValueChanged(oldValue, newValue);

			Color color = ColorUtilities.ConvertHsvToRgb(360 - newValue, 1, 1);
			SelectedColor = color;
		}

		#endregion //Base Class Overrides

		#region Methods

		private void CreateSpectrum()
		{
			_pickerBrush = new LinearGradientBrush();
			_pickerBrush.StartPoint = new Point(0.5, 0);
			_pickerBrush.EndPoint = new Point(0.5, 1);
			_pickerBrush.ColorInterpolationMode = ColorInterpolationMode.SRgbLinearInterpolation;

			List<Color> colorsList = ColorUtilities.GenerateHsvSpectrum();

			double stopIncrement = (double)1 / colorsList.Count;

			int i;
			for(i = 0; i < colorsList.Count; i++)
			{
				_pickerBrush.GradientStops.Add(new GradientStop(colorsList[i], i * stopIncrement));
			}

			_pickerBrush.GradientStops[i - 1].Offset = 1.0;
			_spectrumDisplay.Fill = _pickerBrush;
		}

		#endregion //Methods
	}
}