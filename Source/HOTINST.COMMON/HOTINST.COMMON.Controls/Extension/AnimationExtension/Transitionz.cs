/**
 * ==============================================================================
 *
 * ClassName: Transitionz
 * Description: 为动画提供扩展标记类；
 *				提供对 Opacity、Margin、Translate 属性的动画
 *
 * Version: 1.0
 * Created: 2017/3/29 10:14:03
 * Compiler: Visual Studio 2015
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
using System.Windows.Threading;

namespace HOTINST.COMMON.Controls.Extension.AnimationExtension
{
	/// <summary>
	/// 动画扩展
	/// </summary>
	public class Transitionz
	{
		#region Opacity

		/// <summary>
		/// 定义附加属性 OpacityProperty
		/// </summary>
		public static readonly DependencyProperty OpacityProperty = DependencyProperty.RegisterAttached(
			"Opacity", typeof(IOpacityParams), typeof(Transitionz), new PropertyMetadata(default(IOpacityParams), OnOpacityChanged));
		/// <summary>
		/// 设置附加属性 Opacity 的值
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
		public static void SetOpacity(DependencyObject element, IOpacityParams value)
		{
			element.SetValue(OpacityProperty, value);
		}
		/// <summary>
		/// 获取附加属性 Opacity 的值
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static IOpacityParams GetOpacity(DependencyObject element)
		{
			return (IOpacityParams)element.GetValue(OpacityProperty);
		}

		private static void OnOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			IOpacityParams transtionParams = e.NewValue as IOpacityParams;
			FrameworkElement target = d as FrameworkElement;
			if(transtionParams == null || target == null)
				return;

			target.Opacity = transtionParams.From;
			RoutedEventHandler onLoaded = null;
			onLoaded = (_, __) => target.Dispatcher.BeginInvoke(new Action(() =>
			{
				target.Loaded -= onLoaded;

				DoubleAnimation animation = new DoubleAnimation
				{
					From = transtionParams.From,
					To = transtionParams.To,
					FillBehavior = transtionParams.FillBehavior,
					BeginTime = TimeSpan.FromMilliseconds(transtionParams.BeginTime),
					Duration = new Duration(TimeSpan.FromMilliseconds(transtionParams.Duration)),
					EasingFunction = transtionParams.Ease,
				};
				Storyboard storyboard = new Storyboard();

				storyboard.Children.Add(animation);
				Storyboard.SetTarget(animation, target);
				Storyboard.SetTargetProperty(animation, new PropertyPath(UIElement.OpacityProperty));
				storyboard.Begin();
			}), DispatcherPriority.Background);

			if(target.IsLoaded)
				onLoaded(null, null);
			else
				target.Loaded += onLoaded;
		}

		#endregion

		#region Margin

		/// <summary>
		/// 定义附加属性 MarginProperty
		/// </summary>
		public static readonly DependencyProperty MarginProperty = DependencyProperty.RegisterAttached(
			"Margin", typeof(MarginParamsExtension), typeof(Transitionz), new PropertyMetadata(default(MarginParamsExtension), OnMarginChanged));
		/// <summary>
		/// 设置附加属性 Margin 的值
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
		public static void SetMargin(DependencyObject element, MarginParamsExtension value)
		{
			element.SetValue(MarginProperty, value);
		}
		/// <summary>
		/// 获取附加属性 Margin 的值
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static MarginParamsExtension GetMargin(DependencyObject element)
		{
			return (MarginParamsExtension)element.GetValue(MarginProperty);
		}

		private static void OnMarginChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MarginParamsExtension transtionParams = e.NewValue as MarginParamsExtension;
			FrameworkElement target = d as FrameworkElement;
			if(transtionParams == null || target == null)
				return;

			target.Margin = transtionParams.From;
			RoutedEventHandler onLoaded = null;
			onLoaded = (_, __) => target.Dispatcher.BeginInvoke(new Action(() =>
			{
				target.Loaded -= onLoaded;

				ThicknessAnimation animation = new ThicknessAnimation
				{
					From = transtionParams.From,
					To = transtionParams.To,
					FillBehavior = transtionParams.FillBehavior,
					BeginTime = TimeSpan.FromMilliseconds(transtionParams.BeginTime),
					Duration = new Duration(TimeSpan.FromMilliseconds(transtionParams.Duration)),
					EasingFunction = transtionParams.Ease
				};
				Storyboard storyboard = new Storyboard();

				storyboard.Children.Add(animation);
				Storyboard.SetTarget(animation, target);
				Storyboard.SetTargetProperty(animation, new PropertyPath(FrameworkElement.MarginProperty));
				storyboard.Begin();
			}), DispatcherPriority.Background);

			if(target.IsLoaded)
				onLoaded(null, null);
			else
				target.Loaded += onLoaded;
		}

		#endregion

		#region Translate

		/// <summary>
		/// 定义附加属性 TranslateProperty
		/// </summary>
		public static readonly DependencyProperty TranslateProperty = DependencyProperty.RegisterAttached(
			"Translate", typeof(ITranslateParams), typeof(Transitionz), new PropertyMetadata(default(ITranslateParams), OnTranslateChanged));
		/// <summary>
		/// 设置附加属性 Translate 的值
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
		public static void SetTranslate(DependencyObject element, ITranslateParams value)
		{
			element.SetValue(TranslateProperty, value);
		}
		/// <summary>
		/// 获取附加属性 Translate 的值
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static ITranslateParams GetTranslate(DependencyObject element)
		{
			return (ITranslateParams)element.GetValue(TranslateProperty);
		}

		private static void OnTranslateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ITranslateParams transtionParams = e.NewValue as ITranslateParams;
			FrameworkElement target = d as FrameworkElement;
			if(transtionParams == null || target == null)
				return;

			TranslateTransform translateTransform = new TranslateTransform(transtionParams.From.X, transtionParams.From.Y);
			target.RenderTransform = translateTransform;

			RoutedEventHandler onLoaded = null;
			onLoaded = (_, __) => target.Dispatcher.BeginInvoke(new Action(() =>
			{
				target.Loaded -= onLoaded;

				DoubleAnimation x = new DoubleAnimation
				{
					From = transtionParams.From.X,
					To = transtionParams.To.X,
					FillBehavior = transtionParams.FillBehavior,
					BeginTime = TimeSpan.FromMilliseconds(transtionParams.BeginTime),
					Duration = new Duration(TimeSpan.FromMilliseconds(transtionParams.Duration)),
					EasingFunction = transtionParams.Ease
				};
				
				translateTransform.BeginAnimation(TranslateTransform.XProperty, x);
			}), DispatcherPriority.Background);

			if(target.IsLoaded)
				onLoaded(null, null);
			else
				target.Loaded += onLoaded;
		}

		#endregion
	}
}