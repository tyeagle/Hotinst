/**
 * ==============================================================================
 *
 * ClassName: ValidationPopup
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/5/18 15:59:21
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
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using HOTINST.COMMON.Controls.Win32;

namespace HOTINST.COMMON.Controls.Controls
{
	/// <summary>
	/// This custom popup is used by the validation error template.
	/// It provides some additional nice features:
	///     - repositioning if host-window size or location changed
	///     - repositioning if host-window gets maximized and vice versa
	///     - it's only topmost if the host-window is activated
	/// </summary>
	public class ValidationPopup : Popup
	{
		private Window _hostWindow;

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty CloseOnMouseLeftButtonDownProperty = DependencyProperty.Register(
			"CloseOnMouseLeftButtonDown", typeof(bool), typeof(ValidationPopup), new PropertyMetadata(true));
		
		/// <summary>
		/// 
		/// </summary>
		public ValidationPopup()
		{
			Loaded += CustomValidationPopup_Loaded;
			Opened += CustomValidationPopup_Opened;
		}

		/// <summary>
		/// Gets/sets if the popup can be closed by left mouse button down.
		/// </summary>
		public bool CloseOnMouseLeftButtonDown
		{
			get { return (bool)GetValue(CloseOnMouseLeftButtonDownProperty); }
			set { SetValue(CloseOnMouseLeftButtonDownProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if(CloseOnMouseLeftButtonDown)
			{
				SetCurrentValue(IsOpenProperty, false);
			}
		}

		private void CustomValidationPopup_Loaded(object sender, RoutedEventArgs e)
		{
			var target = PlacementTarget as FrameworkElement;
			if(target == null)
			{
				return;
			}

			_hostWindow = Window.GetWindow(target);
			if(_hostWindow == null)
			{
				return;
			}

			_hostWindow.LocationChanged -= hostWindow_SizeOrLocationChanged;
			_hostWindow.LocationChanged += hostWindow_SizeOrLocationChanged;
			_hostWindow.SizeChanged -= hostWindow_SizeOrLocationChanged;
			_hostWindow.SizeChanged += hostWindow_SizeOrLocationChanged;
			target.SizeChanged -= hostWindow_SizeOrLocationChanged;
			target.SizeChanged += hostWindow_SizeOrLocationChanged;
			_hostWindow.StateChanged -= hostWindow_StateChanged;
			_hostWindow.StateChanged += hostWindow_StateChanged;
			_hostWindow.Activated -= hostWindow_Activated;
			_hostWindow.Activated += hostWindow_Activated;
			_hostWindow.Deactivated -= hostWindow_Deactivated;
			_hostWindow.Deactivated += hostWindow_Deactivated;

			Unloaded -= CustomValidationPopup_Unloaded;
			Unloaded += CustomValidationPopup_Unloaded;
		}

		private void CustomValidationPopup_Opened(object sender, EventArgs e)
		{
			SetTopmostState(true);
		}

		private void hostWindow_Activated(object sender, EventArgs e)
		{
			SetTopmostState(true);
		}

		private void hostWindow_Deactivated(object sender, EventArgs e)
		{
			SetTopmostState(false);
		}

		private void CustomValidationPopup_Unloaded(object sender, RoutedEventArgs e)
		{
			var target = PlacementTarget as FrameworkElement;
			if(target != null)
			{
				target.SizeChanged -= hostWindow_SizeOrLocationChanged;
			}
			if(_hostWindow != null)
			{
				_hostWindow.LocationChanged -= hostWindow_SizeOrLocationChanged;
				_hostWindow.SizeChanged -= hostWindow_SizeOrLocationChanged;
				_hostWindow.StateChanged -= hostWindow_StateChanged;
				_hostWindow.Activated -= hostWindow_Activated;
				_hostWindow.Deactivated -= hostWindow_Deactivated;
			}
			Unloaded -= CustomValidationPopup_Unloaded;
			Opened -= CustomValidationPopup_Opened;
			_hostWindow = null;
		}

		private void hostWindow_StateChanged(object sender, EventArgs e)
		{
			if(_hostWindow != null && _hostWindow.WindowState != WindowState.Minimized)
			{
				var target = PlacementTarget as FrameworkElement;
				var holder = target != null ? target.DataContext as AdornedElementPlaceholder : null;
				if(holder != null && holder.AdornedElement != null)
				{
					PopupAnimation = PopupAnimation.None;
					IsOpen = false;
					var errorTemplate = holder.AdornedElement.GetValue(Validation.ErrorTemplateProperty);
					holder.AdornedElement.SetValue(Validation.ErrorTemplateProperty, null);
					holder.AdornedElement.SetValue(Validation.ErrorTemplateProperty, errorTemplate);
				}
			}
		}

		private void hostWindow_SizeOrLocationChanged(object sender, EventArgs e)
		{
			var offset = HorizontalOffset;
			// "bump" the offset to cause the popup to reposition itself on its own
			HorizontalOffset = offset + 1;
			HorizontalOffset = offset;
		}

		private bool? appliedTopMost;

		private void SetTopmostState(bool isTop)
		{
			// Don抰 apply state if it抯 the same as incoming state
			if(appliedTopMost.HasValue && appliedTopMost == isTop)
			{
				return;
			}

			if(Child == null)
			{
				return;
			}

			var hwndSource = (PresentationSource.FromVisual(Child)) as HwndSource;

			if(hwndSource == null)
			{
				return;
			}
			var hwnd = hwndSource.Handle;

			RECT rect;
			if(!UnsafeNativeMethods.GetWindowRect(hwnd, out rect))
			{
				return;
			}
			//Debug.WriteLine("setting z-order " + isTop);

			var left = rect.left;
			var top = rect.top;
			var width = rect.Width;
			var height = rect.Height;
			if(isTop)
			{
				UnsafeNativeMethods.SetWindowPos(hwnd, Constants.HWND_TOPMOST, left, top, width, height, Constants.TOPMOST_FLAGS);
			}
			else
			{
				// Z-Order would only get refreshed/reflected if clicking the
				// the titlebar (as opposed to other parts of the external
				// window) unless I first set the popup to HWND_BOTTOM
				// then HWND_TOP before HWND_NOTOPMOST
				UnsafeNativeMethods.SetWindowPos(hwnd, Constants.HWND_BOTTOM, left, top, width, height, Constants.TOPMOST_FLAGS);
				UnsafeNativeMethods.SetWindowPos(hwnd, Constants.HWND_TOP, left, top, width, height, Constants.TOPMOST_FLAGS);
				UnsafeNativeMethods.SetWindowPos(hwnd, Constants.HWND_NOTOPMOST, left, top, width, height, Constants.TOPMOST_FLAGS);
			}

			appliedTopMost = isTop;
		}
	}
}