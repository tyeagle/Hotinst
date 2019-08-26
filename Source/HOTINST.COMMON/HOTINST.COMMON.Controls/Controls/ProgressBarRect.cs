/**
 * ==============================================================================
 *
 * ClassName: ProgressBarRect
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/4/14 11:05:49
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
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HOTINST.COMMON.Controls.Controls
{
	/// <summary>
	/// A <see cref="ProgressBarRect"/> that show progress.
	/// </summary>
	public class ProgressBarRect : Control
	{
		#region fields

		private const string PART_UntreatedBorder = "PART_UntreatedBorder";
		private const string PART_Pin = "PART_Pin";

		private Border _untreatedBorder;
		private Grid _pin;

		private Storyboard _sb;
		private Storyboard _sbPin;

		#endregion

		#region props

		#region dependency props
		/// <summary>
		/// 重写父类依赖属性 WidthProperty
		/// </summary>
		public new static readonly DependencyProperty WidthProperty = DependencyProperty.Register(
			"Width", typeof(double), typeof(ProgressBarRect), new PropertyMetadata(default(double), (o, e) =>
			{
				ProgressBarRect context = o as ProgressBarRect;
				if(context?.Tag != null)
				{
					((WidthAndBorder)context.Tag).Width = (double)e.NewValue;
				}
			}));
		/// <summary>
		/// 获取或设置宽度
		/// </summary>
		public new double Width
		{
			get { return (double)GetValue(WidthProperty); }
			set { SetValue(WidthProperty, value); }
		}

		/// <summary>
		/// 重写父类依赖属性 HeightProperty
		/// </summary>
		public new static readonly DependencyProperty HeightProperty = DependencyProperty.Register(
			"Height", typeof(double), typeof(ProgressBarRect), new PropertyMetadata(default(double)));
		/// <summary>
		/// 获取或设置高度
		/// </summary>
		public new double Height
		{
			get { return (double)GetValue(HeightProperty); }
			set { SetValue(HeightProperty, value); }
		}

		/// <summary>
		/// 重写父类依赖属性 BorderThicknessProperty
		/// </summary>
		public new static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register(
			"BorderThickness", typeof(Thickness), typeof(ProgressBarRect), new PropertyMetadata(default(Thickness), (o, e) =>
			{
				ProgressBarRect context = o as ProgressBarRect;
				if(context != null && context.Tag != null)
				{
					((WidthAndBorder)context.Tag).LeftBorder = ((Thickness)e.NewValue).Left;
					((WidthAndBorder)context.Tag).RightBorder = ((Thickness)e.NewValue).Right;
				}
			}));
		/// <summary>
		/// 获取或设置边框粗细
		/// </summary>
		public new Thickness BorderThickness
		{
			get { return (Thickness)GetValue(BorderThicknessProperty); }
			set { SetValue(BorderThicknessProperty, value); }
		}
		
		/// <summary>
		/// 定义依赖属性 StateProperty
		/// </summary>
		public static readonly DependencyProperty StateProperty = DependencyProperty.Register(
			"State", typeof(State), typeof(ProgressBarRect), new PropertyMetadata(default(State), (o, e) =>
			{
				ProgressBarRect context = o as ProgressBarRect;
				if(context != null)
					context.StateChanged((State)e.NewValue);
			}));

		/// <summary>
		/// 进度条状态
		/// </summary>
		public State State
		{
			get { return (State)GetValue(StateProperty); }
			set { SetValue(StateProperty, value); }
		}

		/// <summary>
		/// 定义依赖属性 ValueProperty
		/// </summary>
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
			"Value", typeof(double), typeof(ProgressBarRect), new PropertyMetadata(0.0, (o, e) =>
			{
				ProgressBarRect context = o as ProgressBarRect;
				context?.ValueChanged((double)e.NewValue);
			}));
		/// <summary>
		/// 当前进度值
		/// </summary>
		public double Value
		{
			get { return (double)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		/// <summary>
		/// 定义依赖属性 IsIndeterminate
		/// </summary>
		public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register(
			"IsIndeterminate", typeof(bool), typeof(ProgressBarRect), new PropertyMetadata(false, (o, e) =>
			{
				ProgressBarRect context = o as ProgressBarRect;
				if(context != null)
					context.Value = 100;
			}));
		/// <summary>
		/// 是否是不确定状态
		/// </summary>
		public bool IsIndeterminate
		{
			get { return (bool)GetValue(IsIndeterminateProperty); }
			set { SetValue(IsIndeterminateProperty, value); }
		}

		/// <summary>
		/// 定义依赖属性 OrientationProperty
		/// </summary>
		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
			"Orientation", typeof(Orientation), typeof(ProgressBarRect), new PropertyMetadata(default(Orientation)));
		/// <summary>
		/// 获取或设置进度条的方向: 水平或垂直
		/// </summary>
		public Orientation Orientation
		{
			get { return (Orientation)GetValue(OrientationProperty); }
			set { SetValue(OrientationProperty, value); }
		}

		/// <summary>
		/// 定义依赖属性 ProgressBrushProperty
		/// </summary>
		public static readonly DependencyProperty ProgressBrushProperty = DependencyProperty.Register(
			"ProgressBrush", typeof(Brush), typeof(ProgressBarRect), new PropertyMetadata(default(Brush)));
		/// <summary>
		/// 当前进度条颜色
		/// </summary>
		public Brush ProgressBrush
		{
			get { return (Brush)GetValue(ProgressBrushProperty); }
			private set { SetValue(ProgressBrushProperty, value); }
		}

		/// <summary>
		/// 定义依赖属性 BrushNormalProperty
		/// </summary>
		public static readonly DependencyProperty BrushNormalProperty = DependencyProperty.Register(
			"BrushNormal", typeof(Brush), typeof(ProgressBarRect), new PropertyMetadata(default(Brush), (o, e) =>
			{
				ProgressBarRect context = o as ProgressBarRect;
				if(context != null)
					context.StateChanged(context.State);
			}));
		/// <summary>
		/// 获取或设置正常进度状态的颜色画刷
		/// </summary>
		public Brush BrushNormal
		{
			get { return (Brush)GetValue(BrushNormalProperty); }
			set { SetValue(BrushNormalProperty, value); }
		}

		/// <summary>
		/// 定义依赖属性 BrushPauseProperty
		/// </summary>
		public static readonly DependencyProperty BrushPauseProperty = DependencyProperty.Register(
			"BrushPause", typeof(Brush), typeof(ProgressBarRect), new PropertyMetadata(default(Brush), (o, e) =>
			{
				ProgressBarRect context = o as ProgressBarRect;
				if(context != null)
					context.StateChanged(context.State);
			}));
		/// <summary>
		/// 获取或设置暂停进度状态的颜色画刷
		/// </summary>
		public Brush BrushPause
		{
			get { return (Brush)GetValue(BrushPauseProperty); }
			set { SetValue(BrushPauseProperty, value); }
		}

		/// <summary>
		/// 定义依赖属性 BrushErrorProperty
		/// </summary>
		public static readonly DependencyProperty BrushErrorProperty = DependencyProperty.Register(
			"BrushError", typeof(Brush), typeof(ProgressBarRect), new PropertyMetadata(default(Brush), (o, e) =>
			{
				ProgressBarRect context = o as ProgressBarRect;
				if(context != null)
					context.StateChanged(context.State);
			}));
		/// <summary>
		/// 获取或设置错误进度状态的颜色画刷
		/// </summary>
		public Brush BrushError
		{
			get { return (Brush)GetValue(BrushErrorProperty); }
			set { SetValue(BrushErrorProperty, value); }
		}

		/// <summary>
		/// 定义依赖属性 UntreatedBackgroundProperty
		/// </summary>
		public static readonly DependencyProperty UntreatedBackgroundProperty = DependencyProperty.Register(
			"UntreatedBackground", typeof(Brush), typeof(ProgressBarRect), new PropertyMetadata(default(Brush)));
		/// <summary>
		/// 获取或设置进度条右部分的背景画刷
		/// </summary>
		public Brush UntreatedBackground
		{
			get { return (Brush)GetValue(UntreatedBackgroundProperty); }
			set { SetValue(UntreatedBackgroundProperty, value); }
		}

		/// <summary>
		/// 定义依赖属性 IsShowPinProperty
		/// </summary>
		public static readonly DependencyProperty IsShowPinProperty = DependencyProperty.Register(
			"IsShowPin", typeof(bool), typeof(ProgressBarRect), new PropertyMetadata(default(bool)));
		/// <summary>
		/// 获取或设置是否显示进度值气泡
		/// </summary>
		public bool IsShowPin
		{
			get { return (bool)GetValue(IsShowPinProperty); }
			set { SetValue(IsShowPinProperty, value); }
		}
		#endregion

		#endregion

		#region .ctor

		static ProgressBarRect()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressBarRect), new FrameworkPropertyMetadata(typeof(ProgressBarRect)));
		}

		/// <summary>
		/// 初始化 <see cref="ProgressBarRect"/> 类的新实例。
		/// </summary>
		public ProgressBarRect()
		{
			Tag = new WidthAndBorder
			{
				Width = ActualWidth,
				LeftBorder = BorderThickness.Left,
				RightBorder = BorderThickness.Right
			};
		}

		#endregion
		
		/// <summary>
		/// override OnApplyTemplate
		/// </summary>
		public override void OnApplyTemplate()
		{
			_untreatedBorder = GetTemplateChild(PART_UntreatedBorder) as Border;
			if(_untreatedBorder != null)
				_sb = _untreatedBorder.FindResource("sbProgress") as Storyboard;

			_pin = GetTemplateChild(PART_Pin) as Grid;
			if(_pin != null)
				_sbPin = _pin.FindResource("sbPin") as Storyboard;
			
			base.OnApplyTemplate();
		}

		private void StateChanged(State newValue)
		{
			switch(newValue)
			{
				case State.Normal: ProgressBrush = BrushNormal; break;
				case State.Pause:  ProgressBrush = BrushPause;  break;
				case State.Error:  ProgressBrush = BrushError;  break;
				default:
					throw new ArgumentOutOfRangeException("newValue", newValue, @"不支持的状态");
			}
		}

		private void ValueChanged(double newValue)
		{
			WidthAndBorder stu = Tag as WidthAndBorder;
			if(stu == null)
				return;

			double width = stu.Width;
			double leftb = stu.LeftBorder;
			double right = stu.RightBorder;
			newValue = IsIndeterminate ? 100 : newValue;
			Tag = new WidthAndBorder
			{
				Value = newValue,
				Width = width,
				LeftBorder = leftb,
				RightBorder = right
			};

			if(_untreatedBorder == null || _sb == null)
				return;

			if(newValue < 0)   newValue = 0;
			if(newValue > 100) newValue = 100;

			double actualWidth = (width - leftb - right) * (1 - newValue * 0.01);

			DoubleAnimation animation = _sb.Children[0] as DoubleAnimation;
			if(animation == null)
				return;
			animation.To = actualWidth;

			ThicknessAnimation animationPin = _sbPin.Children[0] as ThicknessAnimation;
			if(animationPin == null)
				return;
			double rightmargin = actualWidth - 13;
			if(newValue < 5)
			{
				rightmargin = width - leftb - right - _pin.ActualWidth;
			}
			animationPin.To = new Thickness(0, 0, rightmargin, 2);

			_sb.Begin();
			_sbPin.Begin();
		}
	}

	/// <summary>
	/// 进度条状态
	/// </summary>
	public enum State
	{
		/// <summary>
		/// 正常
		/// </summary>
		Normal,

		/// <summary>
		/// 暂停
		/// </summary>
		Pause,

		/// <summary>
		/// 错误
		/// </summary>
		Error
	}

	internal class ProgressBorderWidthConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value != null)
				return (double)value + 30;
			return 0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	internal class ProgressValueConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			WidthAndBorder wab = value as WidthAndBorder;
			if(wab == null)
				return null;

			double progressValue = wab.Value;
			if(progressValue <= 0)
				return wab.Width;
			if(progressValue >= 100)
				return 0;

			double awidth = wab.Width - wab.LeftBorder - wab.RightBorder;

			return awidth * (1 - progressValue * 0.01);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	internal class WidthAndBorder
	{
		public double Value { get; set; }
		public double Width { get; set; }
		public double LeftBorder { get; set; }
		public double RightBorder { get; set; }
	}
}