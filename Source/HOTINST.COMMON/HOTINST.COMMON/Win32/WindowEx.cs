/**
 * ==============================================================================
 *
 * ClassName: WindowEx
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/3/20 16:38:15
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;

namespace HOTINST.COMMON.Win32
{
	/// <summary>
	/// Window 扩展类
	/// </summary>
	public static class WindowEx
	{
		#region 隐藏窗口的图标

		/// <summary>
		/// 隐藏窗口的图标
		/// </summary>
		/// <param name="window"></param>
		public static void HideIcon(this Window window)
		{
			window.SourceInitialized += (sender, args) =>
			{
				IntPtr hwnd = new WindowInteropHelper(window).Handle;

				// Change the extended window style to not show a window icon
				int extendedStyle = Win32API.GetWindowLong(hwnd, (int)SetWindowLongOffsets.GWL_EXSTYLE);
				extendedStyle &= (int)WindowExStyles.WS_EX_DLGMODALFRAME;
				Win32API.SetWindowLong(hwnd, (int)SetWindowLongOffsets.GWL_EXSTYLE, extendedStyle);
				// Update the window's non-client area to reflect the changes
				Win32API.SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, (int)SetWindowPosFlags.SWP_NOMOVE | (int)SetWindowPosFlags.SWP_NOSIZE | (int)SetWindowPosFlags.SWP_NOZORDER | (int)SetWindowPosFlags.SWP_FRAMECHANGED);
			};
		}

		/// <summary>
		/// 禁用并隐藏最小化和最大化按钮
		/// </summary>
		/// <param name="window"></param>
		/// <param name="removeMenuItem"></param>
		public static void DisableAndHideMinMaxButtonAndIcon(this Window window, bool removeMenuItem = true)
		{
			if(window == null)
				return;

			window.SourceInitialized += (sender, args) =>
			{
				IntPtr handle = new WindowInteropHelper(window).Handle;

				// Change the extended window style to not show a window icon
				int extendedStyle = Win32API.GetWindowLong(handle, (int)SetWindowLongOffsets.GWL_EXSTYLE);
				extendedStyle |= (int)WindowExStyles.WS_EX_DLGMODALFRAME;

				int nStyle = Win32API.GetWindowLong(handle, (int)SetWindowLongOffsets.GWL_STYLE);
				nStyle &= ~((int)WindowStyles.WS_MINIMIZEBOX | (int)WindowStyles.WS_MAXIMIZEBOX);

				Win32API.SetWindowLong(handle, (int)SetWindowLongOffsets.GWL_EXSTYLE, extendedStyle);
				Win32API.SetWindowLong(handle, (int)SetWindowLongOffsets.GWL_STYLE, nStyle);

				if(removeMenuItem)
				{
					IntPtr hmenu = Win32API.GetSystemMenu(handle, 0);

					//remove the menu item
					Win32API.RemoveMenu(hmenu, 0, (int)(MenuFlag.MF_DISABLED | MenuFlag.MF_BYPOSITION));
					Win32API.RemoveMenu(hmenu, 1, (int)(MenuFlag.MF_DISABLED | MenuFlag.MF_BYPOSITION));
					Win32API.RemoveMenu(hmenu, 2, (int)(MenuFlag.MF_DISABLED | MenuFlag.MF_BYPOSITION));
					Win32API.RemoveMenu(hmenu, 1, (int)(MenuFlag.MF_DISABLED | MenuFlag.MF_BYPOSITION));
					//Redraw the menu bar
					Win32API.DrawMenuBar(handle);
				}
				window.ResizeMode = ResizeMode.NoResize;
			};
		}

		#endregion

		#region 禁用并隐藏最小化和最大化按钮

		private static Action _helpAction;

		/// <summary>
		/// 禁用并隐藏最小化和最大化按钮
		/// </summary>
		/// <param name="window"></param>
		/// <param name="removeMenuItem"></param>
		public static void DisableAndHideMinMaxButton(this Window window, bool removeMenuItem = true)
		{
			if(window == null)
				return;
			
			IntPtr handle = new WindowInteropHelper(window).Handle;
			int nStyle = Win32API.GetWindowLong(handle, (int)SetWindowLongOffsets.GWL_STYLE);

			nStyle &= ~((int)WindowStyles.WS_MINIMIZEBOX | (int)WindowStyles.WS_MAXIMIZEBOX);

			Win32API.SetWindowLong(handle, (int)SetWindowLongOffsets.GWL_STYLE, nStyle);
			Win32API.SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0, (int)(SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOMOVE | SetWindowPosFlags.SWP_FRAMECHANGED));

			if(removeMenuItem)
			{
				IntPtr hmenu = Win32API.GetSystemMenu(handle, 0);

				//remove the menu item
				Win32API.RemoveMenu(hmenu, 3, (int)(MenuFlag.MF_DISABLED | MenuFlag.MF_BYPOSITION));
				Win32API.RemoveMenu(hmenu, 3, (int)(MenuFlag.MF_DISABLED | MenuFlag.MF_BYPOSITION));
				//Redraw the menu bar
				Win32API.DrawMenuBar(handle);
			}
		}

		/// <summary>
		/// 显示帮助按钮(只有在隐藏最小化和最大化按钮后才生效)
		/// </summary>
		/// <param name="window"></param>
		/// <param name="action"></param>
		public static void ShowHelpButton(this Window window, Action action)
		{
			IntPtr handle = new WindowInteropHelper(window).Handle;

			int styles = Win32API.GetWindowLong(handle, (int)SetWindowLongOffsets.GWL_EXSTYLE);

			styles |= (int)WindowExStyles.WS_EX_CONTEXTHELP;

			Win32API.SetWindowLong(handle, (int)SetWindowLongOffsets.GWL_EXSTYLE, styles);
			Win32API.SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0, (int)(SetWindowPosFlags.SWP_NOMOVE | SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOZORDER | SetWindowPosFlags.SWP_FRAMECHANGED));

			HwndSource hwndSource = (HwndSource)PresentationSource.FromVisual(window);
			if(hwndSource != null)
			{
				_helpAction = action;
				hwndSource.AddHook(HelpHook);
			}
		}

		private static IntPtr HelpHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if(msg == (int)WindowMessage.WM_SYSCOMMAND && ((int)wParam & 0xFFF0) == (int)SystemCommand.SC_CONTEXTHELP)
			{
				_helpAction?.Invoke();
				handled = true;
			}
			return IntPtr.Zero;
		}

		#endregion

		#region 设置任意位置拖动效果

		private static int _topMargin;
		
		public static void EnableMouseDrag(this Window window, int topMargin)
		{
			_topMargin = topMargin;
			window.MouseLeftButtonDown += (s, e) =>
			{
				Window win = s as Window;
				if(win == null)
					return;

				if(_topMargin != -1)
				{
					Point point = e.GetPosition(win);
					if(point.Y > _topMargin)
						return;
				}

				IntPtr mainWindowPtr = new WindowInteropHelper(win).Handle;
				Win32API.ReleaseCapture();
				Win32API.SendMessage(mainWindowPtr, (int)WindowMessage.WM_NCLBUTTONDOWN, (int)HitTest.HT_CAPTION, 0);
			};
		}

		#endregion

		#region 设置窗口毛玻璃效果

		#region win7

		[StructLayout(LayoutKind.Sequential)]
		internal struct Margins
		{
			public int cxLeftWidth;
			public int cxRightWidth;
			public int cyTopHeight;
			public int cyBottomHeight;
		}

		[DllImport("DwmApi.dll")]
		internal static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref Margins pMarInset);

		[DllImport("dwmapi.dll", PreserveSig = false)]
		internal static extern bool DwmIsCompositionEnabled();

		internal static void EnableBlurWin7(Window window)
		{
			try
			{
				if(!DwmIsCompositionEnabled())
					return;

				HwndSource mainWindowSrc = HwndSource.FromHwnd(new WindowInteropHelper(window).Handle);
				if(mainWindowSrc?.CompositionTarget != null)
				{
					// Set the background to transparent from both the WPF and Win32 perspectives  
					window.Background = Brushes.Transparent;
					mainWindowSrc.CompositionTarget.BackgroundColor = Colors.Transparent;
					
					Margins margins = new Margins
					{
						cxLeftWidth = -1,
						cxRightWidth = -1,
						cyTopHeight = -1,
						cyBottomHeight = -1
					};

					int hr = DwmExtendFrameIntoClientArea(mainWindowSrc.Handle, ref margins);
					if(hr < 0)
					{
						throw new Exception("DwmExtendFrameIntoClientArea Failed");
					}
				}
			}
			catch(DllNotFoundException ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		#endregion

		#region win10

		internal enum AccentState
		{
			ACCENT_DISABLED = 1,
			ACCENT_ENABLE_GRADIENT = 0,
			ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
			ACCENT_ENABLE_BLURBEHIND = 3,
			ACCENT_INVALID_STATE = 4
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct AccentPolicy
		{
			public AccentState AccentState;
			public int AccentFlags;
			public int GradientColor;
			public int AnimationId;
		}

		internal enum WindowCompositionAttribute
		{
			// ...
			WCA_ACCENT_POLICY = 19

			// ...
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct WindowCompositionAttributeData
		{
			public WindowCompositionAttribute Attribute;
			public IntPtr Data;
			public int SizeOfData;
		}

		[DllImport("user32.dll")]
		internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

		internal static void EnableBlurWin10(Window window)
		{
			AccentPolicy accent = new AccentPolicy { AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND };

			int accentStructSize = Marshal.SizeOf(accent);

			IntPtr accentPtr = Marshal.AllocHGlobal(accentStructSize);
			Marshal.StructureToPtr(accent, accentPtr, false);

			WindowCompositionAttributeData data = new WindowCompositionAttributeData
			{
				Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
				SizeOfData = accentStructSize,
				Data = accentPtr
			};

			SetWindowCompositionAttribute(new WindowInteropHelper(window).Handle, ref data);

			Marshal.FreeHGlobal(accentPtr);
		}

		#endregion

		private static void AeroGlassWin7(Window window, bool enableDrag)
		{
			window.AllowsTransparency = false;

			window.SourceInitialized += (sender, args) =>
			{
				EnableBlurWin7(window);
				if(enableDrag)
				{
					window.EnableMouseDrag(-1);
				}
			};
		}

		private static void AeroGlassWin10(Window window, bool enableDrag)
		{
			window.BorderThickness = new Thickness(1);
			window.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 31, 138, 223));
			window.Background = new SolidColorBrush(Color.FromArgb(2, 0, 0, 0));
			window.WindowStyle = WindowStyle.None;
			window.AllowsTransparency = true;

			window.SourceInitialized += (sender, args) =>
			{
				EnableBlurWin10(window);
				if(enableDrag)
				{
					window.EnableMouseDrag(-1);
				}
			};
		}

		/// <summary>
		/// 使窗口具有毛玻璃透明效果(win7、win10)
		/// 必须在初始化窗口(InitializeComponent)之前调用。
		/// </summary>
		/// <param name="window"></param>
		/// <param name="enableDrag"></param>
		public static void AeroGlass(this Window window, bool enableDrag = true)
		{
			Version currentVersion = Environment.OSVersion.Version;
			Version win10Version = new Version("6.2");

			if(currentVersion >= win10Version)
			{
				AeroGlassWin10(window, enableDrag);
			}
			else
			{
				AeroGlassWin7(window, enableDrag);
			}
		}

		#endregion

		#region 设置窗口在前显示

		/// <summary>
		/// 设置窗口在前显示
		/// </summary>
		/// <param name="win">要在前面显示的窗口</param>
		public static void SetForegroundWindow(this Window win)
		{
			Win32API.SetForegroundWindow(new WindowInteropHelper(win).Handle);
		}

		#endregion

		#region 设置窗口屏幕居中

		/// <summary>
		/// 设置窗口屏幕居中
		/// </summary>
		/// <param name="window">要设置的窗口</param>
		/// <param name="horizontalOffset">水平偏移</param>
		/// <param name="verticalOffset">垂直偏移</param>
		public static void PostitionWindowOnScreen(this Window window, double horizontalOffset = 0, double verticalOffset = 0)
		{
			Screen screen = Screen.FromHandle(new WindowInteropHelper(window).Handle);
			window.Left = screen.Bounds.X + (screen.Bounds.Width - window.ActualWidth) / 2 + horizontalOffset;
			window.Top = screen.Bounds.Y + (screen.Bounds.Height - window.ActualHeight) / 2 + verticalOffset;
		}

		#endregion
	}
}