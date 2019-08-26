/**
 * ==============================================================================
 *
 * ClassName: blink
 * Description: 
 *
 * Version: 1.0
 * Created: 2018/5/9 11:39:25
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
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HOTINST.COMMON.Controls.Extension.AnimationExtension
{
	/// <summary>
	/// 基于控件的前景色实现闪烁效果
	/// </summary>
	public class Blink
	{
		#region props

		/// <summary>
		/// Color1
		/// </summary>
		public static readonly DependencyProperty Color1Property = DependencyProperty.RegisterAttached(
			"Color1", typeof(Brush), typeof(Blink), new PropertyMetadata(Brushes.WhiteSmoke, PropertyChanged));
		/// <summary>
		/// Color1
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
		public static void SetColor1(DependencyObject element, Brush value)
		{
			element.SetValue(Color1Property, value);
		}
		/// <summary>
		/// Color1
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static Brush GetColor1(DependencyObject element)
		{
			return (Brush)element.GetValue(Color1Property);
		}

		/// <summary>
		/// Color2
		/// </summary>
		public static readonly DependencyProperty Color2Property = DependencyProperty.RegisterAttached(
			"Color2", typeof(Brush), typeof(Blink), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 244, 75, 75)), PropertyChanged));
		/// <summary>
		/// Color2
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
		public static void SetColor2(DependencyObject element, Brush value)
		{
			element.SetValue(Color2Property, value);
		}
		/// <summary>
		/// Color2
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static Brush GetColor2(DependencyObject element)
		{
			return (Brush)element.GetValue(Color2Property);
		}

		/// <summary>
		/// Duration
		/// </summary>
		public static readonly DependencyProperty DurationProperty = DependencyProperty.RegisterAttached(
			"Duration", typeof(Duration), typeof(Blink), new PropertyMetadata(new Duration(TimeSpan.FromMilliseconds(200)), PropertyChanged));
		/// <summary>
		/// Duration
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
		public static void SetDuration(DependencyObject element, Duration value)
		{
			element.SetValue(DurationProperty, value);
		}
		/// <summary>
		/// Duration
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static Duration GetDuration(DependencyObject element)
		{
			return (Duration)element.GetValue(DurationProperty);
		}

		/// <summary>
		/// RepeatBehavior
		/// </summary>
		public static readonly DependencyProperty RepeatBehaviorProperty = DependencyProperty.RegisterAttached(
			"RepeatBehavior", typeof(RepeatBehavior), typeof(Blink), new PropertyMetadata(RepeatBehavior.Forever, PropertyChanged));
		/// <summary>
		/// RepeatBehavior
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
		public static void SetRepeatBehavior(DependencyObject element, RepeatBehavior value)
		{
			element.SetValue(RepeatBehaviorProperty, value);
		}
		/// <summary>
		/// RepeatBehavior
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static RepeatBehavior GetRepeatBehavior(DependencyObject element)
		{
			return (RepeatBehavior)element.GetValue(RepeatBehaviorProperty);
		}

		/// <summary>
		/// Visible
		/// </summary>
		public static readonly DependencyProperty VisibleProperty = DependencyProperty.RegisterAttached(
			"Visible", typeof(bool), typeof(Blink), new PropertyMetadata(false, PropertyChanged));
		/// <summary>
		/// Visible
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
		public static void SetVisible(DependencyObject element, bool value)
		{
			element.SetValue(VisibleProperty, value);
		}
		/// <summary>
		/// Visible
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

		private static void Update(UIElement element)
		{
			if(!GetVisible(element))
			{
				return;
			}

			if(element.Visibility != Visibility.Visible)
			{
				return;
			}

			Duration duration = GetDuration(element);
			RepeatBehavior repeatBehavior = GetRepeatBehavior(element);

			Storyboard sb = new Storyboard();

			ColorAnimation animation = new ColorAnimation
			{
				From = GetColor1(element).ToColor(),
				To = GetColor2(element).ToColor(),
				Duration = duration,
				RepeatBehavior = repeatBehavior
			};
			Storyboard.SetTarget(animation, element);
			Storyboard.SetTargetProperty(animation, new PropertyPath("(Foreground).(SolidColorBrush.Color)"));

			sb.Children.Add(animation);

			sb.Completed += (sender, args) => SetVisible(element, false);

			sb.Begin();
		}
	}
}