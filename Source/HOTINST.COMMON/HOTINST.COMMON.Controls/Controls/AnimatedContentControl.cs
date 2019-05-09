/**
 * ==============================================================================
 *
 * ClassName: AnimatedContentControl
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/3/20 12:49:39
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using HOTINST.COMMON.Controls.Core;

namespace HOTINST.COMMON.Controls.Controls
{
	/// <summary>
	/// A <see cref="ContentControl" /> that animates content in.
	/// </summary>
	[TemplatePart(Name = PART_Content, Type = typeof(ContentPresenter))]
	public class AnimatedContentControl : ContentControl
	{
		private const string PART_Content = "PART_MainContent";

		#region props

		/// <summary>
		/// The DependencyProperty for <see cref="SlideFromDirection"/>.
		/// </summary>
		public static readonly DependencyProperty SlideFromDirectionProperty =
			DependencyProperty.Register("SlideFromDirection", typeof(SlideFromDirection), typeof(AnimatedContentControl), new FrameworkPropertyMetadata(SlideFromDirection.Right));

		/// <summary>
		/// Gets or sets the slide from animation direction.
		/// </summary>
		/// <value>
		/// The slide from direction.
		/// </value>
		public SlideFromDirection SlideFromDirection
		{
			get { return (SlideFromDirection)GetValue(SlideFromDirectionProperty); }
			set { SetValue(SlideFromDirectionProperty, value); }
		}

		#endregion

		#region .ctor

		static AnimatedContentControl()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimatedContentControl), new FrameworkPropertyMetadata(typeof(AnimatedContentControl)));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AnimatedContentControl"/> class.
		/// </summary>
		public AnimatedContentControl()
		{
			if(!DesignerProperties.GetIsInDesignMode(this))
			{
				Loaded += (s, e) => AnimateIn();
			}
		}

		#endregion

		/// <summary>
		/// This gets called when the content we're displaying has changed
		/// </summary>
		/// <param name="oldContent">The content that was previously displayed</param>
		/// <param name="newContent">The new content that is displayed</param>
		protected override void OnContentChanged(object oldContent, object newContent)
		{
			AnimateIn();

			base.OnContentChanged(oldContent, newContent);
		}

		/// <summary>
		/// Animates the content in.
		/// </summary>
		public void AnimateIn()
		{
			if(Animations.ShouldAnimate)
			{
				Animations.FadeIn(this, Animations.TypicalDuration);
				Animations.SlideIn(this, SlideFromDirection, TimeSpan.FromMilliseconds((double)AnimationSpeed.Slow));
			}
		}
	}
}