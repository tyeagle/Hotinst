/**
 * ==============================================================================
 *
 * ClassName: DevTableViewEx
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/10/18 10:14:39
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System.Windows;
using System.Windows.Input;
using DevExpress.Xpf.Grid;

namespace HOTINST.COMMON.Controls.Net4._0.Attaches
{
	/// <summary>
	/// 对Dev控件包的TableView扩展附加属性(主从表)
	/// </summary>
	public class DevTableViewEx
	{
		/// <summary>
		/// 鼠标左键双击表格行的时候切换该行的展开状态
		/// </summary>
		public static readonly DependencyProperty ChangeRowExpandStateOnDoubleClickProperty = DependencyProperty.RegisterAttached(
			"ChangeRowExpandStateOnDoubleClick", typeof(bool), typeof(DevTableViewEx), new PropertyMetadata(default(bool), ChangeRowExpandStateOnDoubleClickChanged));
		/// <summary>
		/// 鼠标左键双击表格行的时候切换该行的展开状态
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
		public static void SetChangeRowExpandStateOnDoubleClick(DependencyObject element, bool value)
		{
			element.SetValue(ChangeRowExpandStateOnDoubleClickProperty, value);
		}
		/// <summary>
		/// 鼠标左键双击表格行的时候切换该行的展开状态
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static bool GetChangeRowExpandStateOnDoubleClick(DependencyObject element)
		{
			return (bool)element.GetValue(ChangeRowExpandStateOnDoubleClickProperty);
		}

		private static void ChangeRowExpandStateOnDoubleClickChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			if(obj is TableView view)
			{
				view.RowDoubleClick -= ViewOnRowDoubleClick;
				if((bool)e.NewValue)
				{
					view.RowDoubleClick += ViewOnRowDoubleClick;
				}
			}
		}

		private static void ViewOnRowDoubleClick(object sender, RowDoubleClickEventArgs e)
		{
			if(e.ChangedButton != MouseButton.Left || !e.HitInfo.InRow)
			{
				return;
			}
			if(e.Source is TableView view)
			{
				view.Grid.SetMasterRowExpanded(e.HitInfo.RowHandle, !view.Grid.IsMasterRowExpanded(e.HitInfo.RowHandle));
			}
		}
	}
}