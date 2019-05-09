/**
 * ==============================================================================
 *
 * ClassName: FlyoutContainer
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/3/20 13:23:42
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using HOTINST.COMMON.Controls.Core;

namespace HOTINST.COMMON.Controls.Controls.Flyout
{
	/// <summary>
	/// A container element for hosting <see cref="Flyout"/>s.
	/// </summary>
	[TemplatePart(Name = PART_Content, Type = typeof(ContentPresenter))]
	[TemplatePart(Name = PART_Overlay, Type = typeof(Border))]
	public class FlyoutContainer : ContentControl
	{
		#region fields

		private const string PART_Content = "PART_Content";
		private const string PART_Overlay = "PART_Overlay";

		private ContentPresenter _presenter;
		private Border _overlay;

		private readonly object _openLock = new object();
		private readonly List<Flyout> _openDialogs = new List<Flyout>();

		#endregion

		#region props

		/// <summary>
		/// The dependency property for <see cref="OpenZIndex"/>.
		/// </summary>
		public static readonly DependencyProperty OpenZIndexProperty =
			DependencyProperty.Register("OpenZIndex", typeof(int), typeof(FlyoutContainer), new FrameworkPropertyMetadata(1));

		/// <summary>
		/// Gets or sets the z-index when a flyout is displayed.
		/// </summary>
		/// <value>
		/// The z-index when displaying.
		/// </value>
		public int OpenZIndex
		{
			get { return (int)GetValue(OpenZIndexProperty); }
			set { SetValue(OpenZIndexProperty, value); }
		}

		private static readonly DependencyPropertyKey HasFlyoutOpenPropertyKey = 
			DependencyProperty.RegisterReadOnly("HasFlyoutOpen", typeof(bool), typeof(FlyoutContainer), new FrameworkPropertyMetadata(false));
		/// <summary>
		/// The dependency property for <see cref="HasFlyoutOpen"/>.
		/// </summary>
		public static readonly DependencyProperty HasFlyoutOpenProperty = HasFlyoutOpenPropertyKey.DependencyProperty;

		/// <summary>
		/// Gets a value indicating whether this container has any flyout open.
		/// </summary>
		/// <value>
		///   <c>true</c> if it has flyout open; otherwise, <c>false</c>.
		/// </value>
		public bool HasFlyoutOpen
		{
			get { return (bool)GetValue(HasFlyoutOpenProperty); }
			private set
			{
				bool changed = value != HasFlyoutOpen;

				SetValue(HasFlyoutOpenPropertyKey, value);

				if(changed)
				{
					VisualStateManager.GoToState(this, value ? "IsOpen" : "IsClosed", Animations.ShouldAnimate);
				}
			}
		}

		/// <summary>
		/// The dependency property for <see cref="DisableTarget"/>.
		/// </summary>
		public static readonly DependencyProperty DisableTargetProperty =
			DependencyProperty.Register("DisableTarget", typeof(FrameworkElement), typeof(FlyoutContainer), new FrameworkPropertyMetadata(null));
		
		/// <summary>
		/// Gets or sets the target to disable when flyouts are visible.
		/// </summary>
		/// <value>
		/// The disable target.
		/// </value>
		public FrameworkElement DisableTarget
		{
			get { return (FrameworkElement)GetValue(DisableTargetProperty); }
			set { SetValue(DisableTargetProperty, value); }
		}

		#endregion

		#region .ctor
		static FlyoutContainer()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(FlyoutContainer),
				new FrameworkPropertyMetadata(typeof(FlyoutContainer)));
		}
		
		#endregion

		/// <summary>
		/// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
		/// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			_presenter = GetTemplateChild(PART_Content) as ContentPresenter;
			_overlay = GetTemplateChild(PART_Overlay) as Border;
			VisualStateManager.GoToState(this, HasFlyoutOpen ? "IsOpen" : "IsClosed", Animations.ShouldAnimate);
		}

		/// <summary>
		/// Invoked when an unhandled <see cref="E:System.Windows.UIElement.MouseLeftButtonDown" /> routed event is raised on this element. Implement this method to add class handling for this event.
		/// </summary>
		/// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs" /> that contains the event data. The event data reports that the left mouse button was pressed.</param>
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			var diag = Content as Flyout;
			if(diag != null)
			{
				var hitRes = VisualTreeHelper.HitTest(this, e.GetPosition(this));
				if(Equals(hitRes.VisualHit, _overlay))
				{
					if(diag.OverlayClickBehavior == OverlayClickBehavior.Dismiss)
					{
						diag.DialogResult = false;
					}
					else if(diag.OverlayClickBehavior == OverlayClickBehavior.DragMove)
					{
						Window window = Window.GetWindow(this);
						if(window != null)
							window.DragMove();
					}
				}
			}
			base.OnMouseLeftButtonDown(e);
		}

		internal void Close(Flyout dialog)
		{
			lock(_openLock)
			{
				dialog.OnClosing();
				dialog.Container = null;
				_openDialogs.Remove(dialog);
				ShowMostRecentDialogIfNecessary();
			}
		}

		internal void Show(Flyout dialog)
		{
			if(dialog.Container != null && !Equals(dialog.Container, this))
			{
				throw new ArgumentException(@"This dialog already has a container.", "dialog");
			}

			if(Equals(Content, dialog))
			{ return; }

			lock(_openLock)
			{
				if(dialog.Container != null)
				{
					// already somewhere in this stack
					_openDialogs.Remove(dialog);
				}
				_openDialogs.Add(dialog);
				ShowMostRecentDialogIfNecessary();
			}
		}

		private void ShowMostRecentDialogIfNecessary()
		{
			var next = _openDialogs.LastOrDefault();
			if(next == null)
			{
				HasFlyoutOpen = false;
				Content = null;
				if(_presenter != null)
				{ BindingOperations.ClearAllBindings(_presenter); }
				if(DisableTarget != null)
				{ DisableTarget.IsEnabled = true; }
			}
			else
			{
				next.Container = this;
				if(DisableTarget != null)
				{ DisableTarget.IsEnabled = !next.DisableTarget; }
				if(_presenter != null)
				{
					BindContentAlignment(next);
				}
				Content = next;
				if(Animations.ShouldAnimate)
				{
					DoShowContentAnimation(next);
				}
				HasFlyoutOpen = true;

				var dt = new DispatcherTimer(DispatcherPriority.Send);
				dt.Tick += (s, e) =>
				{
					dt.Stop();
					next.TryFocus();
				};
				dt.Interval = TimeSpan.FromMilliseconds(300);
				dt.Start();

			}
		}

		private void BindContentAlignment(Flyout content)
		{
			var hbind = new Binding(HorizontalAlignmentProperty.Name)
			{
				Source = content,
				NotifyOnSourceUpdated = true
			};
			BindingOperations.SetBinding(_presenter, HorizontalAlignmentProperty, hbind);


			var vbind = new Binding(VerticalAlignmentProperty.Name)
			{
				Source = content,
				NotifyOnSourceUpdated = true
			};
			BindingOperations.SetBinding(_presenter, VerticalAlignmentProperty, vbind);
		}

		private void DoShowContentAnimation(Flyout content)
		{
			var dir = DetermineAniDirection(content);
			Animations.SlideIn(_presenter, dir, Animations.TypicalDuration, 200, Animations.TypicalEasing);
		}

		private static SlideFromDirection DetermineAniDirection(Flyout content)
		{
			if(content != null)
			{
				if(content.VerticalAlignment == VerticalAlignment.Stretch)
				{
					switch(content.HorizontalAlignment)
					{
						case HorizontalAlignment.Left:
							return SlideFromDirection.Left;
						case HorizontalAlignment.Right:
							return SlideFromDirection.Right;
					}
				}
				else if(content.HorizontalAlignment == HorizontalAlignment.Stretch &&
					content.VerticalAlignment == VerticalAlignment.Bottom)
				{
					return SlideFromDirection.Bottom;
				}
			}
			return SlideFromDirection.Top;
		}
	}
}