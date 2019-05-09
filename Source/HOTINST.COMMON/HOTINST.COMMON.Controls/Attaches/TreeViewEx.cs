/**
 * ==============================================================================
 *
 * ClassName: TreeViewEx
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/11/23 16:01:12
 * Compiler: Visual Studio 2017
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using HOTINST.COMMON.Controls.Controls.Editors;
using HOTINST.COMMON.Controls.Helper;
using HOTINST.COMMON.Controls.VisualUtil;

namespace HOTINST.COMMON.Controls.Attaches
{
	/// <summary>
	/// 对 <see cref="TreeView"/> 的附加属性
	/// </summary>
	public class TreeViewEx
	{
		private static TreeView _tv;

		#region porps

		/// <summary>
		/// ShowLineProperty
		/// </summary>
		public static readonly DependencyProperty ShowLineProperty = DependencyProperty.RegisterAttached(
			"ShowLine", typeof(bool), typeof(TreeViewEx), new PropertyMetadata(default(bool), ShowLinePropertyChanged));

		private static void ShowLinePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
		{
			if(o is TreeView tv)
			{
				bool val = Convert.ToBoolean(args.NewValue);
				tv.TargetUpdated -= TvOnTargetUpdated;
				if(val)
				{
					tv.TargetUpdated += TvOnTargetUpdated;
				}

				tv.Tag = val;

				_tv = tv;
			}
		}

		private static void TvOnTargetUpdated(object sender, DataTransferEventArgs args)
		{
			if(args.Property == ItemsControl.ItemsSourceProperty)
			{
				ICollectionView cv = CollectionViewSource.GetDefaultView(((TreeView)sender).Items);
				cv.CollectionChanged += CvOnCollectionChanged;

				if(!UIServices.IsInDesignMode())
				{
					foreach(INode node in cv.SourceCollection)
					{
						AddEvent(node);
					}
				}
			}
		}

		private static void CvOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			if(args.OldItems != null)
			{
				foreach(object oldItem in args.OldItems)
				{
					if(oldItem is INode node)
					{
						RemoveEvent(node);
						Update(node);
					}
				}
			}
			if(args.NewItems != null)
			{
				foreach(object newItem in args.NewItems)
				{
					if(newItem is INode node)
					{
						AddEvent(node);
						Update(node);
					}
				}
			}
		}

		private static void RemoveEvent(INode node)
		{
			node.Children.CollectionChanged -= CvOnCollectionChanged;
			foreach(INode child in node.Children)
			{
				RemoveEvent(child);
			}
		}

		private static void AddEvent(INode node)
		{
			node.Children.CollectionChanged += CvOnCollectionChanged;
			foreach(INode child in node.Children)
			{
				AddEvent(child);
			}
		}

		private static void Update(INode node)
		{
			if(node?.Parent == null)
			{
				_tv.SetCurrentValue(Control.TemplateProperty, null);
				_tv.InvalidateProperty(Control.TemplateProperty);
				return;
			}

			if(node.Parent.Children.Count == 1)
			{
				node.Parent.Children[0].TreeViewItem?.SetCurrentValue(Control.TemplateProperty, null);
				node.Parent.Children[0].TreeViewItem?.InvalidateProperty(Control.TemplateProperty);
			}

			if(node.Parent.Children.Count >= 2)
			{
				int id = node.Parent.Children.IndexOf(node);
				if(id >= 1)
				{
					node.Parent.Children[id - 1].TreeViewItem?.SetCurrentValue(Control.TemplateProperty, null);
					node.Parent.Children[id - 1].TreeViewItem?.InvalidateProperty(Control.TemplateProperty);
				}
				else if(id == -1)
				{
					node.Parent.Children[node.Parent.Children.Count - 1].TreeViewItem?.SetCurrentValue(Control.TemplateProperty, null);
					node.Parent.Children[node.Parent.Children.Count - 1].TreeViewItem?.InvalidateProperty(Control.TemplateProperty);
				}
			}
		}

		/// <summary>
		/// 设置是否要显示网格线的值。
		/// 占用 <see cref="TreeView"/> 的 <c>Tag</c> 属性。
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
		public static void SetShowLine(DependencyObject element, bool value)
		{
			element.SetValue(ShowLineProperty, value);
		}
		/// <summary>
		/// 获取是否显示网格线的值
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static bool GetShowLine(DependencyObject element)
		{
			return (bool)element.GetValue(ShowLineProperty);
		}

		#endregion
	}
}