/**
 * ==============================================================================
 *
 * ClassName: Fade
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/10/17 11:16:56
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
using System.Windows.Media.Animation;

namespace HOTINST.COMMON.Controls.Extension.AnimationExtension
{
	/// <summary>
	/// 为控件实现渐隐渐现效果
	/// </summary>
	public class Fade
	{
		/// <summary>
		/// 是否可见
		/// </summary>
		public static readonly DependencyProperty VisibleProperty = DependencyProperty.RegisterAttached(
			"Visible", typeof(bool), typeof(Fade), new PropertyMetadata(true, VisibleChanged));
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

		private static void VisibleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			if(obj is UIElement element)
			{
				if((bool)args.NewValue)
				{
					element.Visibility = Visibility.Visible;
				}

				Storyboard sb = new Storyboard();

				DoubleAnimation animation = new DoubleAnimation
				{
					To = (bool)args.NewValue ? 1 : 0,
					Duration = new Duration(TimeSpan.FromMilliseconds(250))
				};
				Storyboard.SetTarget(animation, element);
				Storyboard.SetTargetProperty(animation, new PropertyPath(UIElement.OpacityProperty));
				sb.Children.Add(animation);

				sb.Completed += (sender, e) =>
				{
					if(!GetVisible(element))
					{
						element.Visibility = Visibility.Collapsed;
					}
				};
				sb.Begin();
			}
		}
	}
}