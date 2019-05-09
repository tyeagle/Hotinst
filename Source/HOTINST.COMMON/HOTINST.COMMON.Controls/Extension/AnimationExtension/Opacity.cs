/**
 * ==============================================================================
 *
 * ClassName: Opacity
 * Description: 
 *
 * Version: 1.0
 * Created: 2018/5/14 10:07:09
 * Compiler: Visual Studio 2017
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace HOTINST.COMMON.Controls.Extension.AnimationExtension
{
	/// <summary>
	/// 基于控件的透明度实现闪烁效果
	/// </summary>
	public class Opacity
	{
		#region props

		/// <summary>
		/// StartProperty
		/// </summary>
		public static readonly DependencyProperty StartProperty = DependencyProperty.RegisterAttached(
			"Start", typeof(double), typeof(Opacity), new PropertyMetadata(0d, PropertyChanged));
		/// <summary>
		/// 透明度起始值
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
		public static void SetStart(DependencyObject element, double value)
		{
			element.SetValue(StartProperty, value);
		}
		/// <summary>
		/// 透明度起始值
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static double GetStart(DependencyObject element)
		{
			return (double)element.GetValue(StartProperty);
		}

		/// <summary>
		/// EndProperty
		/// </summary>
		public static readonly DependencyProperty EndProperty = DependencyProperty.RegisterAttached(
			"End", typeof(double), typeof(Opacity), new PropertyMetadata(1d, PropertyChanged));
		/// <summary>
		/// 透明度结束值
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
		public static void SetEnd(DependencyObject element, double value)
		{
			element.SetValue(EndProperty, value);
		}
		/// <summary>
		/// 透明度结束值
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static double GetEnd(DependencyObject element)
		{
			return (double)element.GetValue(EndProperty);
		}

		/// <summary>
		/// DurationProperty
		/// </summary>
		public static readonly DependencyProperty DurationProperty = DependencyProperty.RegisterAttached(
			"Duration", typeof(Duration), typeof(Opacity), new PropertyMetadata(new Duration(TimeSpan.FromMilliseconds(200)), PropertyChanged));
		/// <summary>
		/// 动画持续时间
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
		public static void SetDuration(DependencyObject element, Duration value)
		{
			element.SetValue(DurationProperty, value);
		}
		/// <summary>
		/// 动画持续时间
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static Duration GetDuration(DependencyObject element)
		{
			return (Duration)element.GetValue(DurationProperty);
		}

		/// <summary>
		/// RepeatBehaviorProperty
		/// </summary>
		public static readonly DependencyProperty RepeatBehaviorProperty = DependencyProperty.RegisterAttached(
			"RepeatBehavior", typeof(RepeatBehavior), typeof(Opacity), new PropertyMetadata(RepeatBehavior.Forever, PropertyChanged));
		/// <summary>
		/// 动画重复行为
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
		public static void SetRepeatBehavior(DependencyObject element, RepeatBehavior value)
		{
			element.SetValue(RepeatBehaviorProperty, value);
		}
		/// <summary>
		/// 动画重复行为
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static RepeatBehavior GetRepeatBehavior(DependencyObject element)
		{
			return (RepeatBehavior)element.GetValue(RepeatBehaviorProperty);
		}

		/// <summary>
		/// VisibleProperty
		/// </summary>
		public static readonly DependencyProperty VisibleProperty = DependencyProperty.RegisterAttached(
			"Visible", typeof(bool), typeof(Opacity), new PropertyMetadata(default(bool), PropertyChanged));
		/// <summary>
		/// 是否启用动画
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
		public static void SetVisible(DependencyObject element, bool value)
		{
			element.SetValue(VisibleProperty, value);
		}
		/// <summary>
		/// 是否启用动画
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static bool GetVisible(DependencyObject element)
		{
			return (bool)element.GetValue(VisibleProperty);
		}

		#endregion

		private static void PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if(d is UIElement element)
			{
				Update(element);
			}
		}

		private static readonly IDictionary<int, Storyboard> _sb = new Dictionary<int, Storyboard>();
		private static void Update(UIElement element)
		{
			int hash = element.GetHashCode();

			Stop(hash);

			if(GetVisible(element) && element.Visibility == Visibility.Visible)
			{
				Start(hash, element);
			}
		}

		private static void Stop(int hash)
		{
			if(_sb.TryGetValue(hash, out Storyboard sb))
			{
				sb.Stop();
				sb = null;
				_sb.Remove(hash);
			}
		}

		private static void Start(int hash, UIElement element)
		{
			double start = GetStart(element);
			double end = GetEnd(element);
			Duration duration = GetDuration(element);
			RepeatBehavior repeatBehavior = GetRepeatBehavior(element);

			Storyboard sb = new Storyboard();

			DoubleAnimation animation = new DoubleAnimation
			{
				From = start,
				To = end,
				Duration = duration,
				RepeatBehavior = repeatBehavior,
				AutoReverse = true
			};
			Storyboard.SetTarget(animation, element);
			Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));

			sb.Children.Add(animation);

			sb.Completed += (s, args) => SetVisible(element, false);

			sb.Begin();

			_sb.Add(hash, sb);
		}
	}
}