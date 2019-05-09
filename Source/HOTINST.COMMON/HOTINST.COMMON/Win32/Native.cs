/**
 * ==============================================================================
 *
 * ClassName: Native
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/3/17 13:24:48
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
using System.Windows.Interop;

namespace HOTINST.COMMON.Win32
{
	public static class Native
	{
		public const uint GW_HWNDNEXT = 2;

		public static POINT GetRawCursorPos()
		{
			POINT lpPoint;
			Win32API.GetCursorPos(out lpPoint);
			return lpPoint;
		}

		public static Point GetCursorPos()
		{
			POINT lpPoint;
			Win32API.GetCursorPos(out lpPoint);
			return lpPoint;
		}

		public static Point ToWpf(this Point pixelPoint)
		{
			var desktop = Win32API.GetDC(IntPtr.Zero);
			var dpi = Win32API.GetDeviceCaps(desktop, 88);
			Win32API.ReleaseDC(IntPtr.Zero, desktop);

			var physicalUnitSize = 96d / dpi;
			var wpfPoint = new Point(physicalUnitSize * pixelPoint.X, physicalUnitSize * pixelPoint.Y);

			return wpfPoint;
		}

		public static IEnumerable<Window> SortWindowsTopToBottom(IEnumerable<Window> windows)
		{
			var windowsByHandle = windows.Select(window =>
			{
				var hwndSource = PresentationSource.FromVisual(window) as HwndSource;
				var handle = hwndSource != null ? hwndSource.Handle : IntPtr.Zero;
				return new { window, handle };
			}).Where(x => x.handle != IntPtr.Zero)
				.ToDictionary(x => x.handle, x => x.window);

			for(var hWnd = Win32API.GetTopWindow(IntPtr.Zero); hWnd != IntPtr.Zero; hWnd = Win32API.GetWindow(hWnd, GW_HWNDNEXT))
				if(windowsByHandle.ContainsKey((hWnd)))
					yield return windowsByHandle[hWnd];
		}
	}
}