/**
 * ==============================================================================
 *
 * ClassName: TreeViewBehavior
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/9/27 15:02:52
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace HOTINST.COMMON.Controls.Behavior
{
	/// <summary>
	/// 提供树形控件选择项依赖属性, 以便于绑定
	/// </summary>
	public class TreeViewBehavior : Behavior<TreeView>
	{
		#region SelectedItem Property

		/// <summary>
		/// 
		/// </summary>
		public object SelectedItem
		{
			get => GetValue(SelectedItemProperty);
			set => SetValue(SelectedItemProperty, value);
		}
		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty SelectedItemProperty =
			DependencyProperty.Register("SelectedItem", typeof(object), typeof(TreeViewBehavior), new UIPropertyMetadata(null, OnSelectedItemChanged));

		private static void OnSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			TreeViewItem item = e.NewValue as TreeViewItem;
			item?.SetValue(TreeViewItem.IsSelectedProperty, true);
		}

		#endregion
	
		/// <summary>
		/// 
		/// </summary>
		protected override void OnAttached()
		{
			base.OnAttached();

			AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnDetaching()
		{
			base.OnDetaching();

			if(AssociatedObject != null)
			{
				AssociatedObject.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
			}
		}
		
		private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			SelectedItem = e.NewValue;
		}
	}
}