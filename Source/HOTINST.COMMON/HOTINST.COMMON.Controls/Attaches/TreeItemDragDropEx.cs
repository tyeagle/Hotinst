/**
 * ==============================================================================
 *
 * ClassName: TreeItemDragDropEx
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/7/8 11:45:44
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using HOTINST.COMMON.Controls.Controls.Editors;
using HOTINST.COMMON.Controls.Helper;
using HOTINST.COMMON.Controls.VisualUtil;

namespace HOTINST.COMMON.Controls.Attaches
{
	/// <summary>
	/// 拖放完成事件参数
	/// </summary>
	public class DragDoneRoutedEventArgs : RoutedEventArgs
	{
		/// <summary>
		/// 拖放的最终效果
		/// </summary>
		public DragDropEffects Action { get; }
		/// <summary>
		/// 拖动的项
		/// </summary>
		public TreeViewItem DraggedItem { get; }
		/// <summary>
		/// 放置的项
		/// </summary>
		public TreeViewItem DroppedItem { get; }

		/// <summary>
		/// .ctor
		/// </summary>
		/// <param name="routedEvent"></param>
		/// <param name="source"></param>
		/// <param name="action"></param>
		/// <param name="draggedItem"></param>
		/// <param name="droppedItem"></param>
		public DragDoneRoutedEventArgs(RoutedEvent routedEvent, object source, DragDropEffects action, TreeViewItem draggedItem, TreeViewItem droppedItem)
			: base(routedEvent, source)
		{
			Action = action;
			DraggedItem = draggedItem;
			DroppedItem = droppedItem;
		}
	}

	/// <summary>
	/// 树形节点拖放扩展
	/// </summary>
	public class TreeItemDragDropEx
	{
		#region fields

		private static DateTime _lastTime1 = DateTime.MinValue;
		private const double MaxTolerance = 40;

		private static DateTime _lastTime2 = DateTime.MinValue;

		private static bool _canDrag = true;
		private static bool _isDragging;
		private static Point _startPoint;
		
		private static AdornerLayer _adornerLayerDragging;

		private static TreeViewItem _draggingItem;
		private static TreeViewItem _dragOverItem;

		#endregion

		#region props

		/// <summary>
		/// DragDropEnableProperty
		/// </summary>
		public static readonly DependencyProperty DragDropEnableProperty = DependencyProperty.RegisterAttached(
			"DragDropEnable", typeof(bool), typeof(TreeItemDragDropEx), new PropertyMetadata(default(bool), HandleDragDropEnableChanged));
		/// <summary>
		/// 设置是否启用拖放
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
		public static void SetDragDropEnable(DependencyObject element, bool value)
		{
			element.SetValue(DragDropEnableProperty, value);
		}
		/// <summary>
		/// 获取是否启用拖放
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static bool GetDragDropEnable(DependencyObject element)
		{
			return (bool)element.GetValue(DragDropEnableProperty);
		}

		/// <summary>
		/// ScrollOnDragDropProperty
		/// </summary>
		public static readonly DependencyProperty ScrollOnDragDropProperty = DependencyProperty.RegisterAttached(
			"ScrollOnDragDrop", typeof(bool), typeof(TreeItemDragDropEx), new PropertyMetadata(default(bool), HandleScrollOnDragDropChanged));

		/// <summary>
		/// 设置拖动到上下边界时是否自动滚动
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
		public static void SetScrollOnDragDrop(DependencyObject element, bool value)
		{
			element.SetValue(ScrollOnDragDropProperty, value);
		}

		/// <summary>
		/// 获取拖动到上下边界时是否自动滚动
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static bool GetScrollOnDragDrop(DependencyObject element)
		{
			return (bool)element.GetValue(ScrollOnDragDropProperty);
		}

		#endregion

		#region 附加事件

		/// <summary>
		/// 拖放完成事件
		/// </summary>
		public static RoutedEvent DragDoneEvent = EventManager.RegisterRoutedEvent(
			"DragDone", RoutingStrategy.Bubble, typeof(EventHandler<DragDoneRoutedEventArgs>), typeof(TreeItemDragDropEx));
		/// <summary>
		/// 添加拖放完成事件
		/// </summary>
		/// <param name="d"></param>
		/// <param name="h"></param>
		public static void AddDragDoneHandler(DependencyObject d, EventHandler<DragDoneRoutedEventArgs> h)
		{
			(d as UIElement)?.AddHandler(DragDoneEvent, h);
		}
		/// <summary>
		/// 移除拖放完成事件
		/// </summary>
		/// <param name="d"></param>
		/// <param name="h"></param>
		public static void RemoveDragDoneHandler(DependencyObject d, EventHandler<DragDoneRoutedEventArgs> h)
		{
			(d as UIElement)?.RemoveHandler(DragDoneEvent, h);
		}

		#endregion

		private static void HandleDragDropEnableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TreeView tv = d as TreeView;
			if(tv == null)
			{
				Debug.Fail($"无效类型[{d.GetType()}]");
			}
			
			UnSubscribeEvents(tv);

			if(true.Equals(e.NewValue))
			{
				SubscribeEvents(tv);
			}
		}

		private static void SubscribeEvents(UIElement element)
		{
			element.PreviewMouseLeftButtonUp += TreeViewOnPreviewMouseLeftButtonUp;
			element.PreviewMouseLeftButtonDown += TreeViewOnPreviewMouseLeftButtonDown;
			element.PreviewMouseRightButtonDown += TreeViewOnPreviewMouseRightButtonDown;
			element.MouseMove += TreeViewOnMouseMove;

			element.DragOver += TreeViewOnDragOver;
			element.DragLeave += TreeViewOnDragLeave;
			element.Drop += TreeViewOnDrop;
		}

		private static void UnSubscribeEvents(UIElement element)
		{
			element.PreviewMouseLeftButtonUp -= TreeViewOnPreviewMouseLeftButtonUp;
			element.PreviewMouseLeftButtonDown -= TreeViewOnPreviewMouseLeftButtonDown;
			element.PreviewMouseRightButtonDown -= TreeViewOnPreviewMouseRightButtonDown;
			element.MouseMove -= TreeViewOnMouseMove;
			
			element.DragOver -= TreeViewOnDragOver;
			element.DragLeave += TreeViewOnDragLeave;
			element.Drop -= TreeViewOnDrop;
		}

		private static void TreeViewOnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			_draggingItem = null;
		}

		private static void TreeViewOnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			_draggingItem = null;
		}

		private static void TreeViewOnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if(sender is TreeView tv)
			{
				TreeViewItem tvi = VisualUtils.GetHitTest<TreeViewItem>(tv, e.GetPosition(tv));
				if(tvi != null && tvi.DataContext is TreeNodeItemBase node && node.AllowDrag && node.Equals(tv.SelectedItem))
				{
					_draggingItem = tvi;
					_startPoint = e.GetPosition(tvi);
				}
				else
				{
					_draggingItem = null;
				}
			}
		}

		private static void TreeViewOnMouseMove(object sender, MouseEventArgs e)
		{
			if(e.LeftButton != MouseButtonState.Pressed || _isDragging || _draggingItem == null || _startPoint.X <= 0 || _startPoint.Y <= 0)
			{
				return;
			}

			if(sender is TreeView tv)
			{
				TreeViewItem tvi = VisualUtils.GetHitTest<TreeViewItem>(tv, e.GetPosition(tv));
				if(tvi != null && tvi.DataContext is TreeNodeItemBase node && node.AllowDrag && node.Equals(tv.SelectedItem))
				{
					Point position = e.GetPosition(tvi);
					if(System.Math.Abs(position.X - _startPoint.X) > SystemParameters.MinimumHorizontalDragDistance || System.Math.Abs(position.Y - _startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
					{
						StartDrag();
					}
				}
			}
		}

		private static void TreeViewOnDragOver(object sender, DragEventArgs e)
		{
			if(sender is TreeView tv)
			{
				UnSetDraggingOverItemStyle(_dragOverItem);

				_dragOverItem = null;

				TreeViewItem tvi = VisualUtils.GetHitTest<TreeViewItem>(tv, e.GetPosition(tv));
				if(!ConfirmAccept(tvi, e))
				{
					return;
				}
				
				SetDraggingOverItemStyle(tvi);

				_dragOverItem = tvi;
			}			
		}

		private static bool ConfirmAccept(TreeViewItem dragoverItem, DragEventArgs e)
		{
			if(!(_draggingItem?.DataContext is TreeNodeItemBase))
			{
				e.Effects = DragDropEffects.None;
				e.Handled = true;
				return false;
			}
			if(!(dragoverItem?.DataContext is TreeNodeItemBase))
			{
				e.Effects = DragDropEffects.None;
				e.Handled = true;
				return false;
			}

			if(dragoverItem.Equals(_draggingItem))
			{
				e.Effects = DragDropEffects.None;
				e.Handled = true;
				return false;
			}

			IConfirmDropable overItem = (IConfirmDropable)dragoverItem.DataContext;
			if(!overItem.CanDrop((TreeNodeItemBase)_draggingItem.DataContext))
			{
				e.Effects = DragDropEffects.None;
				e.Handled = true;
				return false;
			}

			return true;
		}

		private static void TreeViewOnDragLeave(object sender, DragEventArgs e)
		{
			UnSetDraggingOverItemStyle(_dragOverItem, true);
		}

		private static void TreeViewOnDrop(object sender, DragEventArgs e)
		{
			e.Effects = e.KeyStates.HasFlag(DragDropKeyStates.ControlKey) ? DragDropEffects.Copy : DragDropEffects.Move;
		}

		private static void StartDrag()
		{
			if(!_canDrag || _draggingItem == null)
			{
				return;
			}

			_canDrag = false;
			_isDragging = true;

			UIElement element;
			if(_draggingItem.IsExpanded)
			{
				element = VisualUtils.FindChild<StackPanel>(_draggingItem);
			}
			else
			{
				element = VisualUtils.FindChild<Border>(_draggingItem);
			}
			if(element == null)
			{
				return;
			}
			DraggingAdorner adorner = new DraggingAdorner(element);
			_adornerLayerDragging = AdornerLayer.GetAdornerLayer(VisualUtils.FindVisualParent<Grid>(_draggingItem, "RootContent"));
			_adornerLayerDragging.Add(adorner);

			DragDrop.AddPreviewQueryContinueDragHandler(_draggingItem, OnQueryContinueDrag);

			DragDropEffects resultEffects = DragDrop.DoDragDrop(_draggingItem, _draggingItem, DragDropEffects.Move | DragDropEffects.Copy);
			
			DragDrop.RemovePreviewQueryContinueDragHandler(_draggingItem, OnQueryContinueDrag);

			_isDragging = false;
			_canDrag = true;

			_adornerLayerDragging.Remove(adorner);
			_adornerLayerDragging = null;

			DragFinished(VisualUtils.FindVisualParent<TreeView>(_draggingItem), resultEffects);
		}
		
		private static void OnQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
		{
			_adornerLayerDragging.Update();
		}
		
		private static void DragFinished(TreeView treeview, DragDropEffects effect)
		{
			Mouse.Capture(null);

			UnSetDraggingOverItemStyle(_dragOverItem);

			if(effect == DragDropEffects.Copy || effect == DragDropEffects.Move)
			{
				if(treeview != null && _draggingItem != null && _dragOverItem != null)
				{
					if(!RaiseEventDone(treeview, effect))
					{
						// 移动拖动的节点到新的位置
						HandleDragNode(treeview, effect == DragDropEffects.Copy, _draggingItem, _dragOverItem);
					}
				}
			}

			if(_dragOverItem != null)
			{
				_dragOverItem.IsExpanded = true;
			}

			_draggingItem = null;
			_dragOverItem = null;
		}

		private static void HandleDragNode(TreeView treeview, bool isCopy, TreeViewItem draggedItem, TreeViewItem droppedItem)
		{
			ObservableCollection<INode> items = treeview.ItemsSource as ObservableCollection<INode>;
			if(items == null)
			{
				return;
			}
			object dc = draggedItem.DataContext;
			// 节点处理
			if(draggedItem.DataContext is INode draggedNode && droppedItem.DataContext is INode droppedNode)
			{
				if(!isCopy)
				{
					// 移除拖动的节点
					if(draggedNode.Parent == null)
					{
						items.Remove(draggedNode);
					}
					else
					{
						draggedNode.RemoveSelf();
					}
					// 添加新的节点
					droppedNode.AppendChild(draggedNode);
				}
				else
				{
					// 添加新的节点
					droppedNode.AppendChild(draggedNode.Clone());
				}
			}

			// 排序
			if(droppedItem.DataContext is ICustomSort sortNode)
			{
				sortNode.Sort();
			}

			TreeViewHelper.FindTreeViewItem(treeview, dc)?.Focus();
		}

		private static bool RaiseEventDone(UIElement element, DragDropEffects effect)
		{
			DragDoneRoutedEventArgs args = new DragDoneRoutedEventArgs(DragDoneEvent, element, effect, _draggingItem, _dragOverItem);
			element.RaiseEvent(args);
			return args.Handled;
		}

		private static void SetDraggingOverItemStyle(TreeViewItem item)
		{
			if(item != null)
			{
				item.BorderBrush = Brushes.OrangeRed;

				TimeSpan span = DateTime.UtcNow - _lastTime2;
				if(span.Milliseconds < 500)
					return;
				_lastTime2 = DateTime.UtcNow;

				if(!item.IsExpanded)
				{
					item.IsExpanded = true;
				}
			}
		}
		
		private static void UnSetDraggingOverItemStyle(TreeViewItem item, bool reset = false)
		{
			if(reset)
			{
				_lastTime2 = DateTime.UtcNow;
			}
			if(item != null)
			{
				item.BorderBrush = Brushes.Transparent;
			}
		}

		private static void HandleScrollOnDragDropChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement container = d as FrameworkElement;

			if(container == null)
			{
				Debug.Fail("Invalid type!");
			}

			container.AllowDrop = true;

			container.PreviewDragOver -= OnContainerPreviewDragOver;

			if(true.Equals(e.NewValue))
			{
				container.PreviewDragOver += OnContainerPreviewDragOver;
			}
		}

		private static void OnContainerPreviewDragOver(object sender, DragEventArgs e)
		{
			FrameworkElement container = sender as FrameworkElement;
			if(container == null)
				return;

			// determine Item-wise or content scrolling (by pixel)
			bool itemwise = (bool)container.GetValue(ScrollViewer.CanContentScrollProperty);

			// record time and execute only so often - store time singular static
			// this does not restrict to scroll at two places at a time (how would that go anyways)
			// but only syncs them in time.... that's fair enough; (300ms for ListBox, 20ms for Content)
			TimeSpan span = DateTime.UtcNow - _lastTime1;
			if(span.Milliseconds < (itemwise ? 300 : 20))
				return;

			_lastTime1 = DateTime.UtcNow;

			// digg out the scrollviewer in question
			ScrollViewer scrollViewer = VisualUtils.FindVisualParent<ScrollViewer>(container);
			if(scrollViewer == null)
				return;

			//==============//////////// actual begin ================
			// base Tolerance on ActualHeight and make sensitive area relative but at max a constant size
			double actualHeight = scrollViewer.ActualHeight;
			// try max 25% of height (4 sml ctrl) and limit to max so the regions don't become too 
			// big but also the sensitive regions never overlap
			double tolerance = System.Math.Min(MaxTolerance, actualHeight * 0.25);
			double verticalPos = e.GetPosition(scrollViewer).Y;
			// for list box go as fast as maximum 3 (leave some room to hit->0.35 more) for content jump 30;
			double offset = itemwise ? 3.35 : 30d;

			if(verticalPos < tolerance) // Top of visible list? 
			{
				// accelerate offset * 0..1
				offset = offset * ((tolerance - verticalPos) / tolerance);
				//Scroll up.
				scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - offset);
			}
			else if(verticalPos > actualHeight - tolerance) //Bottom of visible list? 
			{
				// accelerate offset * 0..1
				offset = offset * ((tolerance - (actualHeight - verticalPos)) / tolerance);
				//Scroll down.
				scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + offset);
			}
		}
	}
}