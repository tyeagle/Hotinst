/**
 * ==============================================================================
 *
 * ClassName: TreeViewHelper
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/7/18 12:47:10
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Windows.Controls;

namespace HOTINST.COMMON.Controls.Helper
{
	/// <summary>
	/// TreeViewHelper
	/// </summary>
	public static class TreeViewHelper
	{
		/// <summary>
		/// 展开或折叠TreeView的所有节点
		/// </summary>
		/// <param name="control">TreeView控件</param>
		/// <param name="expandNode">true:展开 false:收缩</param>
		public static void SetNodeExpandedState(ItemsControl control, bool expandNode)
		{
			try
			{
				if(control != null)
				{
					foreach(object item in control.Items)
					{
						if(control.ItemContainerGenerator.ContainerFromItem(item) is TreeViewItem treeItem && treeItem.HasItems)
						{
							treeItem.IsExpanded = expandNode;

							if(treeItem.ItemContainerGenerator.Status != System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
							{
								treeItem.UpdateLayout();
							}

							SetNodeExpandedState(treeItem, expandNode);
						}
					}
				}
			}
			catch(Exception)
			{
				// ignored
			}
		}

		/// <summary>
		/// 根据节点内容查找节点对象
		/// </summary>
		/// <param name="item"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static TreeViewItem FindTreeViewItem(ItemsControl item, object data)
		{
			TreeViewItem findItem = null;
			bool itemIsExpand = false;
			if(item is TreeViewItem tviCurrent)
			{
				itemIsExpand = tviCurrent.IsExpanded;
				if(!tviCurrent.IsExpanded)
				{
					//如果这个TreeViewItem未展开过，则不能通过ItemContainerGenerator来获得TreeViewItem
					tviCurrent.SetValue(TreeViewItem.IsExpandedProperty, true);
					//必须使用UpdaeLayour才能获取到TreeViewItem
					tviCurrent.UpdateLayout();
				}
			}
			for(int i = 0; i < item.Items.Count; i++)
			{
				TreeViewItem tvItem = (TreeViewItem)item.ItemContainerGenerator.ContainerFromIndex(i);
				if(tvItem == null)
					continue;
				object itemData = item.Items[i];
				if(itemData == data)
				{
					findItem = tvItem;
					break;
				}
				if(tvItem.Items.Count > 0)
				{
					findItem = FindTreeViewItem(tvItem, data);
					if(findItem != null)
						break;
				}
			}
			if(findItem == null)
			{
				if(item is TreeViewItem current)
				{
					current.SetValue(TreeViewItem.IsExpandedProperty, itemIsExpand);
					current.UpdateLayout();
				}
			}
			return findItem;
		}
	}
}