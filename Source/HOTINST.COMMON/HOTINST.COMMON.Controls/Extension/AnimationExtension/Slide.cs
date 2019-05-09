/**
 * ==============================================================================
 *
 * ClassName: Slide
 * Description: 
 *
 * Version: 1.0
 * Created: 2018/3/6 14:37:17
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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HOTINST.COMMON.Controls.Extension.AnimationExtension
{
	/// <summary>
	/// 为控件实现淡入淡出效果
	/// </summary>
	public class Slide
	{
		/// <summary>
		/// 起始点
		/// </summary>
		public static readonly DependencyProperty StartProperty = DependencyProperty.RegisterAttached(
			"Start", typeof(Point), typeof(Slide), new PropertyMetadata(new Point(100, 0)));
		/// <summary>
		/// 起始点
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
		public static void SetStart(DependencyObject element, Point value)
		{
			element.SetValue(StartProperty, value);
		}
		/// <summary>
		/// 起始点
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static Point GetStart(DependencyObject element)
		{
			return (Point)element.GetValue(StartProperty);
		}

		/// <summary>
		/// 结束点
		/// </summary>
		public static readonly DependencyProperty EndProperty = DependencyProperty.RegisterAttached(
			"End", typeof(Point), typeof(Slide), new PropertyMetadata(new Point(0, 0)));
		/// <summary>
		/// 结束点
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
		public static void SetEnd(DependencyObject element, Point value)
		{
			element.SetValue(EndProperty, value);
		}
		/// <summary>
		/// 结束点
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static Point GetEnd(DependencyObject element)
		{
			return (Point)element.GetValue(EndProperty);
		}

		/// <summary>
		/// 持续时间
		/// </summary>
		public static readonly DependencyProperty DurationProperty = DependencyProperty.RegisterAttached(
			"Duration", typeof(int), typeof(Slide), new PropertyMetadata(250));
		/// <summary>
		/// 持续时间
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
		public static void SetDuration(DependencyObject element, int value)
		{
			element.SetValue(DurationProperty, value);
		}
		/// <summary>
		/// 持续时间
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static int GetDuration(DependencyObject element)
		{
			return (int)element.GetValue(DurationProperty);
		}

		/// <summary>
		/// 是否可见
		/// </summary>
		public static readonly DependencyProperty VisibleProperty = DependencyProperty.RegisterAttached(
			"Visible", typeof(bool), typeof(Slide), new PropertyMetadata(true, VisibleChanged));

		/// <summary>
		/// 是否可见
		/// </summary>
		public static void SetVisible(DependencyObject element, bool value)
		{
			element.SetValue(VisibleProperty, value);
		}

		/// <summary>
		/// 是否可见
		/// </summary>
		public static bool GetVisible(DependencyObject element)
		{
			return (bool)element.GetValue(VisibleProperty);
		}

		/// <summary>
		/// Completed
		/// </summary>
		public static RoutedEvent SlideInCompletedEvent = EventManager.RegisterRoutedEvent(
			"SlideInCompleted", RoutingStrategy.Direct, typeof(EventHandler), typeof(Slide));

		/// <summary>
		/// Completed
		/// </summary>
		/// <param name="d"></param>
		/// <param name="h"></param>
		public static void AddSlideInCompletedHandler(DependencyObject d, EventHandler h)
		{
			(d as UIElement)?.AddHandler(SlideInCompletedEvent, h);
		}

		/// <summary>
		/// Completed
		/// </summary>
		/// <param name="d"></param>
		/// <param name="h"></param>
		public static void RemoveSlideInCompletedHandler(DependencyObject d, EventHandler h)
		{
			(d as UIElement)?.RemoveHandler(SlideInCompletedEvent, h);
		}
		
		/// <summary>
		/// Completed
		/// </summary>
		public static RoutedEvent SlideOutCompletedEvent = EventManager.RegisterRoutedEvent(
			"SlideOutCompleted", RoutingStrategy.Direct, typeof(EventHandler), typeof(Slide));

		/// <summary>
		/// Completed
		/// </summary>
		/// <param name="d"></param>
		/// <param name="h"></param>
		public static void AddSlideOutCompletedHandler(DependencyObject d, EventHandler h)
		{
			(d as UIElement)?.AddHandler(SlideOutCompletedEvent, h);
		}

		/// <summary>
		/// Completed
		/// </summary>
		/// <param name="d"></param>
		/// <param name="h"></param>
		public static void RemoveSlideOutCompletedHandler(DependencyObject d, EventHandler h)
		{
			(d as UIElement)?.RemoveHandler(SlideOutCompletedEvent, h);
		}
		
		private static void VisibleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			if(obj is UIElement element)
			{
				if((bool)args.NewValue)
				{
					element.Visibility = Visibility.Visible;
				}

				int duration = GetDuration(element);

				Storyboard sb = new Storyboard();

				DoubleAnimation animation1 = new DoubleAnimation
				{
					To = (bool)args.NewValue ? 1 : 0,
					Duration = new Duration(TimeSpan.FromMilliseconds(duration))
				};
				Storyboard.SetTarget(animation1, element);
				Storyboard.SetTargetProperty(animation1, new PropertyPath(UIElement.OpacityProperty));
				sb.Children.Add(animation1);

				if(Equals(element.RenderTransform, Transform.Identity))
				{
					element.RenderTransform = new TranslateTransform();
				}
				DoubleAnimation animation2 = new DoubleAnimation
				{
					To = (bool)args.NewValue ? GetEnd(element).X : GetStart(element).X,
					Duration = new Duration(TimeSpan.FromMilliseconds(duration))
				};

				DoubleAnimation animation3 = new DoubleAnimation
				{
					To = (bool)args.NewValue ? GetEnd(element).Y : GetStart(element).Y,
					Duration = new Duration(TimeSpan.FromMilliseconds(duration))
				};

				TranslateTransform translate = (TranslateTransform)element.RenderTransform;

				translate.BeginAnimation(TranslateTransform.XProperty, animation2);
				translate.BeginAnimation(TranslateTransform.YProperty, animation3);

				sb.Completed += (sender, e) =>
				{
					if(!GetVisible(element))
					{
						element.Visibility = Visibility.Collapsed;
						element.RaiseEvent(new RoutedEventArgs(SlideOutCompletedEvent, element));
					}
					else
					{
						if(element.Focusable)
						{
							element.Focus();
						}
						element.RaiseEvent(new RoutedEventArgs(SlideInCompletedEvent, element));
					}
				};
				sb.Begin();
			}
		}
	}
}