/**
 * ==============================================================================
 *
 * ClassName: PermissionHelper
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/3/23 14:37:20
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace HOTINST.COMMON.Controls.VisualUtil
{
	/// <summary>
	/// 
	/// </summary>
	public class PermissionHelper
	{
		private static bool? _unmanagedCodePermission;
		/// <summary>
		/// 
		/// </summary>
		public static bool HasUnmanagedCodePermission
		{
			get
			{
				if(!_unmanagedCodePermission.HasValue)
				{
					_unmanagedCodePermission = HasSecurityPermissionFlag(SecurityPermissionFlag.UnmanagedCode);
				}
				return _unmanagedCodePermission != null && _unmanagedCodePermission.Value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="visual"></param>
		/// <param name="point"></param>
		/// <returns></returns>
		public static Point GetSafePointToScreen(Visual visual, Point point)
		{
			Point result;
			try
			{
				result = visual is HwndHost
					? (PresentationSource.FromVisual(visual) != null ? visual.PointToScreen(point) : point)
					: visual.PointToScreen(point);
			}
			catch
			{
				try
				{
					result = visual != null && PresentationSource.FromVisual(visual) != null ? visual.PointToScreen(point) : point;
				}
				catch
				{
					FrameworkElement element = visual as FrameworkElement;
					result = element != null && element.Parent != null && PresentationSource.FromVisual((Visual)element.Parent) != null ? ((Visual)((FrameworkElement)visual).Parent).PointToScreen(point) : point;
				}
			}
			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="flag"></param>
		/// <returns></returns>
		private static bool? HasSecurityPermissionFlag(SecurityPermissionFlag flag)
		{
			bool? result = true;
			try
			{
				SecurityPermission securityPermission = new SecurityPermission(flag);
				securityPermission.Demand();
			}
			catch(Exception)
			{
				result = false;
			}
			return result;
		}
	}
}