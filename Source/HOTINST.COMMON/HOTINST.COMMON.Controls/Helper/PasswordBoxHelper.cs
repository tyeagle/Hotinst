﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HOTINST.COMMON.Controls.Helper
{
	/// <summary>
	/// 
	/// </summary>
    public class PasswordBoxHelper
    {
		/// <summary>
		/// 
		/// </summary>
        public static readonly DependencyProperty CapsLockIconProperty
            = DependencyProperty.RegisterAttached("CapsLockIcon",
                                                  typeof(object),
                                                  typeof(PasswordBoxHelper),
                                                  new PropertyMetadata("!", ShowCapslockWarningChanged));
		/// <summary>
		/// 
		/// </summary>
        public static readonly DependencyProperty CapsLockWarningToolTipProperty
            = DependencyProperty.RegisterAttached("CapsLockWarningToolTip",
                                                  typeof(object),
                                                  typeof(PasswordBoxHelper),
                                                  new PropertyMetadata("Caps lock is on"));

        /// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
        [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
        public static object GetCapsLockIcon(PasswordBox element)
        {
            return element.GetValue(CapsLockIconProperty);
        }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
        public static void SetCapsLockIcon(PasswordBox element, object value)
        {
            element.SetValue(CapsLockIconProperty, value);
        }

        /// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
        [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
        public static object GetCapsLockWarningToolTip(PasswordBox element)
        {
            return element.GetValue(CapsLockWarningToolTipProperty);
        }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
        public static void SetCapsLockWarningToolTip(PasswordBox element, object value)
        {
            element.SetValue(CapsLockWarningToolTipProperty, value);
        }

        private static void ShowCapslockWarningChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                PasswordBox pb = (PasswordBox)d;

                pb.KeyDown -= RefreshCapslockStatus;
                pb.GotFocus -= RefreshCapslockStatus;
                pb.PreviewGotKeyboardFocus -= RefreshCapslockStatus;
                pb.LostFocus -= HandlePasswordBoxLostFocus;

                if (e.NewValue != null)
                {
                    pb.KeyDown += RefreshCapslockStatus;
                    pb.GotFocus += RefreshCapslockStatus;
                    pb.PreviewGotKeyboardFocus += RefreshCapslockStatus;
                    pb.LostFocus += HandlePasswordBoxLostFocus;
                }
            }
        }

        private static void RefreshCapslockStatus(object sender, RoutedEventArgs e)
        {
            FrameworkElement fe = FindCapsLockIndicator((Control)sender);
            if (fe != null)
            {
                fe.Visibility = Keyboard.IsKeyToggled(Key.CapsLock) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private static void HandlePasswordBoxLostFocus(object sender, RoutedEventArgs e)
        {
            FrameworkElement fe = FindCapsLockIndicator((Control)sender);
            if (fe != null)
            {
                fe.Visibility = Visibility.Collapsed;
            }
        }

        private static FrameworkElement FindCapsLockIndicator(Control pb)
        {
	        if(pb != null)
	        {
				if(pb.Template != null)
				{
					return pb.Template.FindName("PART_CapsLockIndicator", pb) as FrameworkElement;
				}
	        }
			return null;
        }
    }
}