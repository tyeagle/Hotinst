/**
 * ==============================================================================
 *
 * ClassName: ProgressBar
 * Description: 
 *
 * Version: 1.0
 * Created: 2018/2/11 11:56:26
 * Compiler: Visual Studio 2017
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using HOTINST.COMMON.Controls.Core;

namespace HOTINST.COMMON.Controls.Controls
{
	/// <summary>
	/// 进度条
	/// </summary>
	public class ProgressBar : Slider
	{
		#region fields

		private FrameworkElement _gridRoot;
		private FrameworkElement _indicator;
		private FrameworkElement _glow;

		#endregion

		#region porps

		/// <summary>
		/// ProgressColorProperty
		/// </summary>
		public static readonly DependencyProperty ProgressColorProperty = DependencyProperty.Register(
			"ProgressColor", typeof(Brush), typeof(ProgressBar), new PropertyMetadata(Brushes.Green));
		/// <summary>
		/// 获取或设置进度颜色
		/// </summary>
		public Brush ProgressColor
		{
			get => (Brush)GetValue(ProgressColorProperty);
			set => SetValue(ProgressColorProperty, value);
		}

		/// <summary>
		/// UnProgressColorProperty
		/// </summary>
		public static readonly DependencyProperty UnProgressColorProperty = DependencyProperty.Register(
			"UnProgressColor", typeof(Brush), typeof(ProgressBar), new PropertyMetadata(SystemColors.ControlBrush));
		/// <summary>
		/// 获取或设置进度剩余部分的颜色
		/// </summary>
		public Brush UnProgressColor
		{
			get => (Brush)GetValue(UnProgressColorProperty);
			set => SetValue(UnProgressColorProperty, value);
		}

		/// <summary>
		/// HintProperty
		/// </summary>
		public static readonly DependencyProperty HintProperty = DependencyProperty.Register(
			"Hint", typeof(string), typeof(ProgressBar), new PropertyMetadata(default(string)));
		/// <summary>
		/// 获取或设置提示信息内容
		/// </summary>
		public string Hint
		{
			get => (string)GetValue(HintProperty);
			set => SetValue(HintProperty, value);
		}

		#endregion

		#region .ctor

		static ProgressBar()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressBar), new FrameworkPropertyMetadata(typeof(ProgressBar)));
		}

		/// <summary>
		/// ctor
		/// </summary>
		public ProgressBar()
		{
			SizeChanged += OnSizeChanged;
			IsVisibleChanged += (s, e) => UpdateAnimation();
		}

		#endregion

		private void OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			SetProgressBarIndicatorLength();
		}

		private void SetProgressBarIndicatorLength()
		{
			if(_indicator != null && _gridRoot != null)
			{
				//_indicator.Width = progress * _gridRoot.ActualWidth / 100;
				double minimum = Minimum;
				double maximum = Maximum;
				_indicator.Width = (maximum <= minimum ? 1.0 : (Value - minimum) / (maximum - minimum)) * _gridRoot.ActualWidth;

				UpdateAnimation();
			}
		}

		private void UpdateAnimation()
		{
			if(_glow == null)
				return;

			if(IsVisible && _glow.Width > 0.0 && _indicator.Width > 0.0)
			{
				double left1 = _indicator.Width + _glow.Width;
				double left2 = -1.0 * _glow.Width;
				TimeSpan timeSpan1 = TimeSpan.FromSeconds((int)(left1 - left2) / 200.0);
				TimeSpan timeSpan2 = TimeSpan.FromSeconds(1.0);
				Thickness margin = _glow.Margin;
				TimeSpan timeSpan3;
				if(DoubleUtil.GreaterThan(margin.Left, left2))
				{
					margin = _glow.Margin;
					if(DoubleUtil.LessThan(margin.Left, left1 - 1.0))
					{
						margin = _glow.Margin;
						double num2 = margin.Left - left2;
						timeSpan3 = TimeSpan.FromSeconds(-1.0 * num2 / 200.0);
						goto label_6;
					}
				}
				timeSpan3 = TimeSpan.Zero;
label_6:
				ThicknessAnimationUsingKeyFrames animationUsingKeyFrames = new ThicknessAnimationUsingKeyFrames
				{
					BeginTime = timeSpan3,
					Duration = new Duration(timeSpan1 + timeSpan2),
					RepeatBehavior = RepeatBehavior.Forever
				};
				animationUsingKeyFrames.KeyFrames.Add(new LinearThicknessKeyFrame(new Thickness(left2, 0.0, 0.0, 0.0), TimeSpan.FromSeconds(0.0)));
				animationUsingKeyFrames.KeyFrames.Add(new LinearThicknessKeyFrame(new Thickness(left1, 0.0, 0.0, 0.0), timeSpan1));
				_glow.BeginAnimation(MarginProperty, animationUsingKeyFrames);
			}
			else
			{
				_glow.BeginAnimation(MarginProperty, null);
			}
		}

		#region Overrides of Slider

		/// <summary>当 <see cref="P:System.Windows.Controls.Primitives.RangeBase.Minimum" /> 属性更改时，更新 <see cref="T:System.Windows.Controls.ProgressBar" /> 的当前位置。</summary>
		/// <param name="oldMinimum">旧值 <see cref="P:System.Windows.Controls.Primitives.RangeBase.Minimum" /> 属性。</param>
		/// <param name="newMinimum">新值 <see cref="P:System.Windows.Controls.Primitives.RangeBase.Minimum" /> 属性。</param>
		protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
		{
			base.OnMinimumChanged(oldMinimum, newMinimum);
			SetProgressBarIndicatorLength();
		}

		/// <summary>当 <see cref="P:System.Windows.Controls.Primitives.RangeBase.Maximum" /> 属性更改时，更新 <see cref="T:System.Windows.Controls.ProgressBar" /> 的当前位置。</summary>
		/// <param name="oldMaximum">旧值 <see cref="P:System.Windows.Controls.Primitives.RangeBase.Maximum" /> 属性。</param>
		/// <param name="newMaximum">新值 <see cref="P:System.Windows.Controls.Primitives.RangeBase.Maximum" /> 属性。</param>
		protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
		{
			base.OnMaximumChanged(oldMaximum, newMaximum);
			SetProgressBarIndicatorLength();
		}

		/// <summary>当 <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> 属性更改时，更新 <see cref="T:System.Windows.Controls.ProgressBar" /> 的当前位置。</summary>
		/// <param name="oldValue">旧值 <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> 属性。</param>
		/// <param name="newValue">新值 <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> 属性。</param>
		protected override void OnValueChanged(double oldValue, double newValue)
		{
			base.OnValueChanged(oldValue, newValue);
			SetProgressBarIndicatorLength();
		}

		/// <summary>生成 <see cref="T:System.Windows.Controls.Slider" /> 控件的可视化树。</summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_gridRoot = GetTemplateChild("Grid_Root") as FrameworkElement;
			_indicator = GetTemplateChild("PART_Indicator") as FrameworkElement;
			_glow = GetTemplateChild("PART_GlowRect") as FrameworkElement;
		}

		#endregion
	}
}