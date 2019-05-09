/**
 * ==============================================================================
 *
 * ClassName: UIServices
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/7/7 13:44:15
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
using System.Windows.Input;
using System.Windows.Threading;

namespace HOTINST.COMMON.Controls.Helper
{
	/// <summary>
	/// Contains helper methods for UI, so far just one for showing a waitcursor
	/// </summary>
	public static class UIServices
	{
		#region 获取是否处于设计时模式

		/// <summary>
		/// 获取当前是否处于设计时模式
		/// </summary>
		/// <returns></returns>
		public static bool IsInDesignMode()
		{
			return (bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue;
		}

		#endregion

		#region 设置控件在Disabled状态下也显示ToolTip

		/// <summary>
		/// 设置<see cref="ToolTipService.ShowOnDisabledProperty"/>为True
		/// </summary>
		public static void ShowToolTipOnDisabled()
		{
			ToolTipService.ShowOnDisabledProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(true));
		}

		#endregion

		#region Sets the busystate as busy

		/// <summary>
		/// A value indicating whether the UI is currently busy
		/// </summary>
		private static bool IsBusy;

		/// <summary>
		/// Sets the busystate as busy.
		/// </summary>
		public static void SetBusyState()
		{
			SetBusyState(true);
		}

		/// <summary>
		/// Sets the busystate to busy or not busy.
		/// </summary>
		/// <param name="busy">if set to <c>true</c> the application is now busy.</param>
		private static void SetBusyState(bool busy)
		{
			if(busy != IsBusy)
			{
				IsBusy = busy;
				Mouse.OverrideCursor = busy ? Cursors.Wait : null;

				if(IsBusy)
				{
					new DispatcherTimer(TimeSpan.FromSeconds(0), DispatcherPriority.ApplicationIdle, Tick, Application.Current.Dispatcher);
				}
			}
		}

		/// <summary>
		/// Handles the Tick event of the dispatcherTimer control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private static void Tick(object sender, EventArgs e)
		{
			if(sender is DispatcherTimer dispatcherTimer)
			{
				SetBusyState(false);
				dispatcherTimer.Stop();
			}
		}

		#endregion
	}
}