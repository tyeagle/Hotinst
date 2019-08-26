/**
 * ==============================================================================
 *
 * ClassName: Animations
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/3/20 12:55:13
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HOTINST.COMMON.Controls.Core
{
	/// <summary>
	/// Quick utility for doing simple ad-hoc animations.
	/// </summary>
	public class Animations
	{
		#region props

		/// <summary>
		/// Gets a flag indicating whether animation should be used.
		/// This does not stop controls from using animations in this class.
		/// </summary>
		/// <value>
		///   <c>true</c> if the app should animate; otherwise, <c>false</c>.
		/// </value>
		public static bool ShouldAnimate
		{
			get { return !SystemParameters.IsRemoteSession || (RenderCapability.Tier >> 16) > 0; }
		}

		/// <summary>
		/// Gets the a typical animation duration.
		/// </summary>
		public static TimeSpan TypicalDuration { get; private set; }

		/// <summary>
		/// Gets the typical easing function.
		/// </summary>
		/// <value>
		/// The typical easing.
		/// </value>
		public static QuarticEase TypicalEasing { get; private set; }

		/// <summary>
		/// Gets the typical slide offset value. Default is 15.
		/// </summary>
		/// <value>
		/// The typical slide offset.
		/// </value>
		public static double TypicalSlideOffset { get { return 15; } }

		#endregion

		#region .ctor

		static Animations()
		{
			TypicalDuration = TimeSpan.FromMilliseconds((double)AnimationSpeed.Normal);
			TypicalEasing = new QuarticEase { EasingMode = EasingMode.EaseOut };
			TypicalEasing.Freeze();
		}

		#endregion

		#region methods

		/// <summary>
		/// Slides the element in with translate transform.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="direction">The direction.</param>
		/// <param name="duration">The duration.</param>
		public static void SlideIn(UIElement element, SlideFromDirection direction, TimeSpan duration)
		{
			SlideIn(element, direction, duration, TypicalSlideOffset, TypicalEasing);
		}

		/// <summary>
		/// Slides the element in with translate transform.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="direction">The direction.</param>
		/// <param name="duration">The duration.</param>
		/// <param name="startOffset">The start offset.</param>
		public static void SlideIn(UIElement element, SlideFromDirection direction, TimeSpan duration, double startOffset)
		{
			SlideIn(element, direction, duration, startOffset, TypicalEasing);
		}

		/// <summary>
		/// Slides the element in with translate transform
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="direction">The direction.</param>
		/// <param name="duration">The duration.</param>
		/// <param name="startOffset">The start offset.</param>
		/// <param name="easing">The easing.</param>
		/// <param name="completedCallback">The completed callback.</param>
		public static void SlideIn(UIElement element, SlideFromDirection direction, TimeSpan duration, double startOffset, IEasingFunction easing, Action completedCallback = null)
		{
			if(element == null)
				return;

			DoubleAnimation da = new DoubleAnimation
			{
				Duration = duration,
				EasingFunction = easing,
				To = 0
			};
			if(completedCallback != null)
				da.Completed += (s, e) => completedCallback();

			TranslateTransform transform = FindOrCreateRenderXform(element);
			switch(direction)
			{
				case SlideFromDirection.Top:
					da.From = -startOffset;
					transform.BeginAnimation(TranslateTransform.YProperty, da);
					break;
				case SlideFromDirection.Left:
					da.From = -startOffset;
					transform.BeginAnimation(TranslateTransform.XProperty, da);
					break;
				case SlideFromDirection.Right:
					da.From = startOffset;
					transform.BeginAnimation(TranslateTransform.XProperty, da);
					break;
				case SlideFromDirection.Bottom:
					da.From = startOffset;
					transform.BeginAnimation(TranslateTransform.YProperty, da);
					break;
				default:
					throw new ArgumentException(@"不支持的参数值", "direction");
			}
		}

		private static TranslateTransform FindOrCreateRenderXform(UIElement element)
		{
			TranslateTransform transform = null;

			if(element.RenderTransform != null)
			{
				TransformGroup grp = element.RenderTransform as TransformGroup;
				if(grp == null)
				{
					transform = element.RenderTransform as TranslateTransform;
				}
				else
				{
					Transform hit = grp.Children.FirstOrDefault(t => t is TranslateTransform);
					transform = (TranslateTransform)hit;
				}
			}

			if(transform == null)
			{
				transform = new TranslateTransform();
				// probably shouldn't replace existing transform but anyway
				element.RenderTransform = transform;
			}
			return transform;
		}

		/// <summary>
		/// Animates the element's opacity from 0 to 1.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="duration">The duration.</param>
		/// <param name="completedCallback">The completed callback.</param>
		public static void FadeIn(UIElement element, TimeSpan duration, Action completedCallback = null)
		{
			if(element != null)
			{
				DoubleAnimation da = new DoubleAnimation
				{
					From = 0,
					To = 1,
					Duration = duration
				};
				if(completedCallback != null)
					da.Completed += (s, e) => completedCallback();

				element.BeginAnimation(UIElement.OpacityProperty, da, HandoffBehavior.SnapshotAndReplace);
			}
		}

		/// <summary>
		/// Animates the element's opacity to 1.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="duration">The duration.</param>
		/// <param name="completedCallback">The completed callback.</param>
		public static void FadeOut(UIElement element, TimeSpan duration, Action completedCallback = null)
		{
			if(element != null)
			{
				DoubleAnimation da = new DoubleAnimation
				{
					To = 0,
					Duration = duration
				};
				if(completedCallback != null)
					da.Completed += (s, e) => completedCallback();

				element.BeginAnimation(UIElement.OpacityProperty, da, HandoffBehavior.SnapshotAndReplace);
			}
		}

		#endregion
	}
}