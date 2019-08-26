/**
 * ==============================================================================
 *
 * ClassName: EditDoneFocusFixEx
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/7/8 10:44:52
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HOTINST.COMMON.Controls.Controls.Editors;
using HOTINST.COMMON.Controls.VisualUtil;

namespace HOTINST.COMMON.Controls.Attaches
{
	/// <summary>
	/// 编辑完成事件参数
	/// </summary>
	public class EditDoneRoutedEventArgs : RoutedEventArgs
	{
		/// <summary>
		/// 旧文本
		/// </summary>
		public string OldText { get; }
		
		/// <summary>
		/// .ctor
		/// </summary>
		public EditDoneRoutedEventArgs(RoutedEvent routedEvent, object source, string oldText)
			: base(routedEvent, source)
		{
			OldText = oldText;
		}
	}

	/// <summary>
	/// 可编辑树形节点附加属性
	/// </summary>
	public class TreeItemEx
	{
		private static TreeViewItem _selectedItem;

		#region props

		/// <summary>
		/// EditKeyProperty
		/// </summary>
		public static readonly DependencyProperty EditKeyProperty = DependencyProperty.RegisterAttached(
			"EditKey", typeof(Key), typeof(TreeItemEx), new PropertyMetadata(Key.None, HandleEditKeyChanged));
		/// <summary>
		/// 设置进入编辑模式的快捷键
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
		public static void SetEditKey(DependencyObject element, Key value)
		{
			element.SetValue(EditKeyProperty, value);
		}
		/// <summary>
		/// 获取进入编辑模式的快捷键
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static Key GetEditKey(DependencyObject element)
		{
			return (Key)element.GetValue(EditKeyProperty);
		}

		/// <summary>
		/// SelectOnRightButtonDownProperty
		/// </summary>
		public static readonly DependencyProperty SelectOnRightButtonDownProperty = DependencyProperty.RegisterAttached(
			"SelectOnRightButtonDown", typeof(bool), typeof(TreeItemEx), new PropertyMetadata(default(bool), HandleSelectOnRightButtonDownChanged));
		/// <summary>
		/// 设置右键按下时是否选中该节点
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
		public static void SetSelectOnRightButtonDown(DependencyObject element, bool value)
		{
			element.SetValue(SelectOnRightButtonDownProperty, value);
		}
		/// <summary>
		/// 获取右键按下时是否选中该节点
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static bool GetSelectOnRightButtonDown(DependencyObject element)
		{
			return (bool)element.GetValue(SelectOnRightButtonDownProperty);
		}
		
		#endregion

		#region 附加事件

		/// <summary>
		/// 双击事件
		/// </summary>
		public static RoutedEvent DoubleClickEvent = EventManager.RegisterRoutedEvent(
			"DoubleClick", RoutingStrategy.Bubble, typeof(MouseButtonEventHandler), typeof(TreeItemEx));
		/// <summary>
		/// 添加双击事件
		/// </summary>
		/// <param name="d"></param>
		/// <param name="h"></param>
		public static void AddDoubleClickHandler(DependencyObject d, MouseButtonEventHandler h)
		{
			(d as UIElement)?.AddHandler(DoubleClickEvent, h);
		}
		/// <summary>
		/// 移除双击事件
		/// </summary>
		/// <param name="d"></param>
		/// <param name="h"></param>
		public static void RemoveDoubleClickHandler(DependencyObject d, MouseButtonEventHandler h)
		{
			(d as UIElement)?.RemoveHandler(DoubleClickEvent, h);
		}

		/// <summary>
		/// 编辑完成事件
		/// </summary>
		public static RoutedEvent EditDoneEvent = EventManager.RegisterRoutedEvent(
			"EditDone", RoutingStrategy.Direct, typeof(EventHandler<EditDoneRoutedEventArgs>), typeof(TreeItemEx));

		/// <summary>
		/// 添加编辑完成事件
		/// </summary>
		/// <param name="d"></param>
		/// <param name="h"></param>
		public static void AddEditDoneHandler(DependencyObject d, EventHandler<EditDoneRoutedEventArgs> h)
		{
			(d as UIElement)?.AddHandler(EditDoneEvent, h);
		}

		/// <summary>
		/// 移除编辑完成事件
		/// </summary>
		/// <param name="d"></param>
		/// <param name="h"></param>
		public static void RemoveEditDoneHandler(DependencyObject d, EventHandler<EditDoneRoutedEventArgs> h)
		{
			(d as UIElement)?.RemoveHandler(EditDoneEvent, h);
		}

		#endregion

		private static void HandleEditKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TreeView tv = d as TreeView;
			if(tv == null)
			{
				Debug.Fail("Invalid type！");
			}

			tv.RemoveHandler(TreeViewItem.SelectedEvent, new RoutedEventHandler(TreeViewItemOnSelected));
			tv.RemoveHandler(Control.MouseDoubleClickEvent, new MouseButtonEventHandler(TreeViewOnDoubleClick));
			tv.PreviewKeyDown -= OnTreeViewPreviewKeyDown;
			
			if(e.NewValue != null)
			{
				tv.AddHandler(TreeViewItem.SelectedEvent, new RoutedEventHandler(TreeViewItemOnSelected));
				tv.AddHandler(Control.MouseDoubleClickEvent, new MouseButtonEventHandler(TreeViewOnDoubleClick));
				tv.PreviewKeyDown += OnTreeViewPreviewKeyDown;
			}
		}

		private static void TreeViewItemOnSelected(object sender, RoutedEventArgs e)
		{
			_selectedItem = e.OriginalSource as TreeViewItem;
		}
		
		private static void TreeViewOnDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if(sender is TreeView tv)
			{
				TreeViewItem tvi = VisualUtils.GetHitTest<TreeViewItem>(tv, e.GetPosition(tv));
				if(tvi != null && tvi.Equals(_selectedItem))
				{
					_selectedItem?.RaiseEvent(new MouseButtonEventArgs(e.MouseDevice, 0, e.ChangedButton)
					{
						RoutedEvent = DoubleClickEvent,
						Source = _selectedItem
					});
				}
			}
		}

		private static void OnTreeViewPreviewKeyDown(object sender, KeyEventArgs e)
		{
			if(sender is TreeView tv && e.Key.Equals(GetEditKey(tv)))
			{
				EditableTextBlock etb = VisualUtils.FindChild<EditableTextBlock>(_selectedItem);
				if(etb != null && etb.IsEditable)
				{
					etb.IsInEditMode = true;
				}
			}
		}
		
		private static void HandleSelectOnRightButtonDownChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TreeView tv = d as TreeView;
			if(tv == null)
			{
				Debug.Fail($"无效类型[{d.GetType()}]");
			}

			tv.MouseRightButtonDown -= TreeViewOnMouseRightButtonDown;
			tv.PreviewMouseRightButtonUp -= TreeViewOnPreviewMouseRightButtonUp;

			if(true.Equals(e.NewValue))
			{
				tv.MouseRightButtonDown += TreeViewOnMouseRightButtonDown;
				tv.PreviewMouseRightButtonUp += TreeViewOnPreviewMouseRightButtonUp;
			}
		}

		private static void TreeViewOnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			if(VisualUtils.VisualUpSearch<TreeViewItem>(e.OriginalSource as DependencyObject) is TreeViewItem tvi)
			{
				tvi.IsSelected = true;
				tvi.Focus();
			}
		}

		private static void TreeViewOnPreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			if(VisualUtils.VisualUpSearch<TreeViewItem>(e.OriginalSource as DependencyObject) is TreeViewItem tvi)
			{
				tvi.IsSelected = true;
				tvi.Focus();
			}
		}
	}
}