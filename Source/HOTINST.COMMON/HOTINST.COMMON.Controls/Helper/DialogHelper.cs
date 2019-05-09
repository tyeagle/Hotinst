/**
 * ==============================================================================
 *
 * ClassName: DialogHelper
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/10/25 16:10:14
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
using System.Windows;
using System.Windows.Interop;
using HOTINST.COMMON.Win32;

namespace HOTINST.COMMON.Controls.Helper
{
	/// <summary>
	/// 
	/// </summary>
	public static class DialogHelper
	{
		private static readonly IList<ResourceDictionary> _resources = new List<ResourceDictionary>();

		private static Window GetWindowFromHwnd(IntPtr hwnd)
		{
			return (Window)HwndSource.FromHwnd(hwnd)?.RootVisual;
		}

		private static Window GetTopWindow()
		{
			try
			{
				IntPtr hwnd = Win32API.GetForegroundWindow();
				
				return GetWindowFromHwnd(hwnd);
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// 向弹出对话框窗体添加资源
		/// </summary>
		/// <param name="resource"></param>
		public static void AddMergedResource(ResourceDictionary resource)
		{
			if(_resources.Contains(resource))
			{
				_resources.Remove(resource);
			}
			_resources.Add(resource);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="win"></param>
		public static void ShowDialog(Window win)
		{
			foreach(ResourceDictionary resource in _resources)
			{
				win.Resources.MergedDictionaries.Add(resource);
			}
			win.Owner = GetTopWindow();
			win.ShowDialog();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="win"></param>
		public static void Show(Window win)
		{
			foreach(ResourceDictionary resource in _resources)
			{
				win.Resources.MergedDictionaries.Add(resource);
			}
			win.Show();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dlg"></param>
		public static void Show(Type dlg)
		{
			if(!typeof(Window).IsAssignableFrom(dlg))
				throw new ArgumentException("dlg必须是Window");
			
			Window window = (Window)Activator.CreateInstance(dlg);
			ShowDialog(window);
		}
	}
}