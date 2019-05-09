/**
 * ==============================================================================
 *
 * ClassName: DevTreeListViewEx
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/10/18 10:14:56
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
using DevExpress.Xpf.Grid.TreeList;

namespace HOTINST.COMMON.Controls.Net4._0.Attaches
{
	/// <summary>
	/// 对Dev控件包的TreeListView扩展附加属性
	/// </summary>
	public class DevTreeListViewEx
	{
		/// <summary>
		/// 鼠标左键双击树形控件节点的时候切换该节点的展开状态
		/// </summary>
		public static readonly DependencyProperty ChangeNodeExpandStateOnDoubleClickProperty = DependencyProperty.RegisterAttached(
			"ChangeNodeExpandStateOnDoubleClick", typeof(bool), typeof(DevTreeListViewEx), new PropertyMetadata(default(bool), ChangeNodeExpandStateOnDoubleClickChanged));
		/// <summary>
		/// 鼠标左键双击树形控件节点的时候切换该节点的展开状态
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
		public static void SetChangeNodeExpandStateOnDoubleClick(DependencyObject element, bool value)
		{
			element.SetValue(ChangeNodeExpandStateOnDoubleClickProperty, value);
		}
		/// <summary>
		/// 鼠标左键双击树形控件节点的时候切换该节点的展开状态
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static bool GetChangeNodeExpandStateOnDoubleClick(DependencyObject element)
		{
			return (bool)element.GetValue(ChangeNodeExpandStateOnDoubleClickProperty);
		}
		
		private static void ChangeNodeExpandStateOnDoubleClickChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			if(obj is TreeListView view)
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
			if(e.Source is TreeListView view)
			{
				if(e.HitInfo is TreeListViewHitInfo hitInfo && !hitInfo.InNodeExpandButton && !hitInfo.InNodeIndent)
				{
					view.ChangeNodeExpanded(e.HitInfo.RowHandle, !view.GetNodeByRowHandle(e.HitInfo.RowHandle).IsExpanded);
				}
			}
		}
	}
}