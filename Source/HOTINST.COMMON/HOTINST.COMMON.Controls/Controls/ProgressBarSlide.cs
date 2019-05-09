/**
 * ==============================================================================
 *
 * ClassName: ProgressBarSlide
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/9/30 11:00:56
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
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HOTINST.COMMON.Controls.Controls
{
	/// <summary>
	/// 
	/// </summary>
	public class ProgressBarSlide : RangeBase
	{
		#region fields

		private Grid _gridRoot;
		private Border _indicator;

		private Storyboard _sb;

		#endregion

		#region props

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty ShowValueProperty = DependencyProperty.Register(
			"ShowValue", typeof(bool), typeof(ProgressBarSlide), new PropertyMetadata(default(bool)));
		/// <summary>
		/// 获取或设置是否显示值
		/// </summary>
		public bool ShowValue
		{
			get => (bool)GetValue(ShowValueProperty);
			set => SetValue(ShowValueProperty, value);
		}

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty ValueAlignmentProperty = DependencyProperty.Register(
			"ValueAlignment", typeof(HorizontalAlignment), typeof(ProgressBarSlide), new PropertyMetadata(HorizontalAlignment.Center));
		/// <summary>
		/// 获取或设置显示的对齐方式
		/// </summary>
		public HorizontalAlignment ValueAlignment
		{
			get => (HorizontalAlignment)GetValue(ValueAlignmentProperty);
			set => SetValue(ValueAlignmentProperty, value);
		}

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty ValueDisplayFormatProperty = DependencyProperty.Register(
			"ValueDisplayFormat", typeof(string), typeof(ProgressBarSlide), new PropertyMetadata("{}{0}"));
		/// <summary>
		/// 获取或设置值格式化字符串
		/// </summary>
		public string ValueDisplayFormat
		{
			get => (string)GetValue(ValueDisplayFormatProperty);
			set => SetValue(ValueDisplayFormatProperty, value);
		}

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty ValueForegroundProperty = DependencyProperty.Register(
			"ValueForeground", typeof(Brush), typeof(ProgressBarSlide), new PropertyMetadata(Brushes.Black));
		/// <summary>
		/// 获取或设置显示值的前景色
		/// </summary>
		public Brush ValueForeground
		{
			get => (Brush)GetValue(ValueForegroundProperty);
			set => SetValue(ValueForegroundProperty, value);
		}

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty UnTreatedColorProperty = DependencyProperty.Register(
			"UnTreatedColor", typeof(Brush), typeof(ProgressBarSlide), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(232, 232, 232))));
		/// <summary>
		/// 获取或设置没有进度的背景色
		/// </summary>
		public Brush UnTreatedColor
		{
			get => (Brush)GetValue(UnTreatedColorProperty);
			set => SetValue(UnTreatedColorProperty, value);
		}

		/// <summary>
		/// 定义依赖属性 StateProperty
		/// </summary>
		public static readonly DependencyProperty StateProperty = DependencyProperty.Register(
			"State", typeof(State), typeof(ProgressBarSlide), new PropertyMetadata(default(State), (o, e) =>
			{
				ProgressBarSlide context = o as ProgressBarSlide;
				context?.StateChanged((State)e.NewValue);
			}));

		/// <summary>
		/// 进度条状态
		/// </summary>
		public State State
		{
			get => (State)GetValue(StateProperty);
			set => SetValue(StateProperty, value);
		}

		/// <summary>
		/// 定义依赖属性 ProgressBrushProperty
		/// </summary>
		public static readonly DependencyProperty ProgressBrushProperty = DependencyProperty.Register(
			"ProgressBrush", typeof(Brush), typeof(ProgressBarSlide), new PropertyMetadata(default(Brush)));

		/// <summary>
		/// 当前进度条颜色
		/// </summary>
		public Brush ProgressBrush
		{
			get => (Brush)GetValue(ProgressBrushProperty);
			private set => SetValue(ProgressBrushProperty, value);
		}

		/// <summary>
		/// 定义依赖属性 BrushNormalProperty
		/// </summary>
		public static readonly DependencyProperty BrushNormalProperty = DependencyProperty.Register(
			"BrushNormal", typeof(Brush), typeof(ProgressBarSlide), new PropertyMetadata(default(Brush), (o, e) =>
			{
				ProgressBarSlide context = o as ProgressBarSlide;
				context?.StateChanged(context.State);
			}));

		/// <summary>
		/// 获取或设置正常进度状态的颜色画刷
		/// </summary>
		public Brush BrushNormal
		{
			get => (Brush)GetValue(BrushNormalProperty);
			set => SetValue(BrushNormalProperty, value);
		}

		/// <summary>
		/// 定义依赖属性 BrushPauseProperty
		/// </summary>
		public static readonly DependencyProperty BrushPauseProperty = DependencyProperty.Register(
			"BrushPause", typeof(Brush), typeof(ProgressBarSlide), new PropertyMetadata(default(Brush), (o, e) =>
			{
				ProgressBarSlide context = o as ProgressBarSlide;
				context?.StateChanged(context.State);
			}));

		/// <summary>
		/// 获取或设置暂停进度状态的颜色画刷
		/// </summary>
		public Brush BrushPause
		{
			get => (Brush)GetValue(BrushPauseProperty);
			set => SetValue(BrushPauseProperty, value);
		}

		/// <summary>
		/// 定义依赖属性 BrushErrorProperty
		/// </summary>
		public static readonly DependencyProperty BrushErrorProperty = DependencyProperty.Register(
			"BrushError", typeof(Brush), typeof(ProgressBarSlide), new PropertyMetadata(default(Brush), (o, e) =>
			{
				ProgressBarSlide context = o as ProgressBarSlide;
				context?.StateChanged(context.State);
			}));

		/// <summary>
		/// 获取或设置错误进度状态的颜色画刷
		/// </summary>
		public Brush BrushError
		{
			get => (Brush)GetValue(BrushErrorProperty);
			set => SetValue(BrushErrorProperty, value);
		}

		#endregion

		#region .ctor

		static ProgressBarSlide()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressBarSlide), new FrameworkPropertyMetadata(typeof(ProgressBarSlide)));
		}

		/// <summary>
		/// 
		/// </summary>
		public ProgressBarSlide()
		{

		}

		#endregion

		private void StateChanged(State newValue)
		{
			switch(newValue)
			{
				case State.Normal:
					ProgressBrush = BrushNormal;
					break;
				case State.Pause:
					ProgressBrush = BrushPause;
					break;
				case State.Error:
					ProgressBrush = BrushError;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(newValue), newValue, @"不支持的状态");
			}
		}

		private void GridRootOnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			if(e.WidthChanged)
			{
				SetIndicatorLength(false);
			}
		}

		private void SetIndicatorLength(bool animate)
		{
			if(_sb == null)
			{
				return;
			}

			double totaleWidth = Maximum - Minimum;
			double progress = Maximum - Value;
			double delta = progress / totaleWidth;

			double actualWidth = _gridRoot.ActualWidth * delta;
			DoubleAnimation animation = _sb.Children[0] as DoubleAnimation;
			if(animation == null)
				return;
			animation.To = actualWidth;

			if(animate)
			{
				double delta2 = System.Math.Abs(_indicator.ActualWidth - actualWidth);
				double percent = delta2 / _gridRoot.ActualWidth;

				animation.Duration = new Duration(TimeSpan.FromMilliseconds(500 * percent));
			}
			else
			{
				animation.Duration = new Duration(TimeSpan.Zero);
			}
			_sb.Begin();
		}

		#region Overrides of FrameworkElement

		/// <summary>在派生类中重写后，每当应用程序代码或内部进程调用 <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />，都将调用此方法。</summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_gridRoot = GetTemplateChild("Part_GridRoot") as Grid;
			_indicator = GetTemplateChild("Part_Indicator") as Border;

			if(_gridRoot != null)
			{
				_gridRoot.SizeChanged += GridRootOnSizeChanged;
			}
			_sb = _indicator?.FindResource("sbProgress") as Storyboard;
		}

		#endregion

		#region Overrides of RangeBase

		/// <summary>引发 <see cref="E:System.Windows.Controls.Primitives.RangeBase.ValueChanged" /> 路由事件。</summary>
		/// <param name="oldValue">
		/// <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> 属性的旧值。</param>
		/// <param name="newValue">
		/// <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> 属性的新值。</param>
		protected override void OnValueChanged(double oldValue, double newValue)
		{
			base.OnValueChanged(oldValue, newValue);
			SetIndicatorLength(true);
		}

		/// <summary>在 <see cref="P:System.Windows.Controls.Primitives.RangeBase.Minimum" /> 属性更改时调用。</summary>
		/// <param name="oldMinimum">
		/// <see cref="P:System.Windows.Controls.Primitives.RangeBase.Minimum" /> 属性的旧值。</param>
		/// <param name="newMinimum">
		/// <see cref="P:System.Windows.Controls.Primitives.RangeBase.Minimum" /> 属性的新值。</param>
		protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
		{
			base.OnMinimumChanged(oldMinimum, newMinimum);
			SetIndicatorLength(true);
		}

		/// <summary>在 <see cref="P:System.Windows.Controls.Primitives.RangeBase.Maximum" /> 属性更改时调用。</summary>
		/// <param name="oldMaximum">
		/// <see cref="P:System.Windows.Controls.Primitives.RangeBase.Maximum" /> 属性的旧值。</param>
		/// <param name="newMaximum">
		/// <see cref="P:System.Windows.Controls.Primitives.RangeBase.Maximum" /> 属性的新值。</param>
		protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
		{
			base.OnMaximumChanged(oldMaximum, newMaximum);
			SetIndicatorLength(true);
		}

		#endregion
	}

	internal class HeightConverter : IValueConverter
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
				return val + 2;
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
			return null;
		}

		#endregion
	}
}