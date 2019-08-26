/**
 * ==============================================================================
 *
 * ClassName: TreeLineMarginConverter
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/11/23 11:50:44
 * Compiler: Visual Studio 2017
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace HOTINST.COMMON.Controls.Converters.Internal
{
	/// <summary>
	/// 
	/// </summary>
	public class ExpandMarginConverter : IValueConverter
	{
		/// <summary>
		/// 
		/// </summary>
		public double TopEx { get; set; } = 6;

		#region Implementation of IValueConverter

		/// <summary>转换值。</summary>
		/// <returns>转换后的值。如果该方法返回 null，则使用有效的 null 值。</returns>
		/// <param name="value">绑定源生成的值。</param>
		/// <param name="targetType">绑定目标属性的类型。</param>
		/// <param name="parameter">要使用的转换器参数。</param>
		/// <param name="culture">要用在转换器中的区域性。</param>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value is Thickness val)
			{
				return new Thickness(val.Left - 2, val.Top + TopEx, val.Right, val.Bottom);
			}

			return value;
		}

		/// <summary>转换值。</summary>
		/// <returns>转换后的值。如果该方法返回 null，则使用有效的 null 值。</returns>
		/// <param name="value">绑定目标生成的值。</param>
		/// <param name="targetType">要转换到的类型。</param>
		/// <param name="parameter">要使用的转换器参数。</param>
		/// <param name="culture">要用在转换器中的区域性。</param>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}

	/// <summary>
	/// 
	/// </summary>
	public class TreeLineMarginConverter : IValueConverter
	{
		/// <summary>
		/// 
		/// </summary>
		public bool IsVerticalLine { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public double TopEx { get; set; } = 12;

		#region Implementation of IValueConverter

		/// <summary>转换值。</summary>
		/// <returns>转换后的值。如果该方法返回 null，则使用有效的 null 值。</returns>
		/// <param name="value">绑定源生成的值。</param>
		/// <param name="targetType">绑定目标属性的类型。</param>
		/// <param name="parameter">要使用的转换器参数。</param>
		/// <param name="culture">要用在转换器中的区域性。</param>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value is Thickness val)
			{
				if(Equals(parameter, "1"))
				{
					return new Thickness(val.Left + 5, val.Top + TopEx, val.Right, val.Bottom);
				}
				return IsVerticalLine
					? new Thickness(val.Left + 5, val.Top, val.Right, val.Bottom)
					: new Thickness(val.Left + 6, val.Top + TopEx, val.Right, val.Bottom);
			}
			return value;
		}

		/// <summary>转换值。</summary>
		/// <returns>转换后的值。如果该方法返回 null，则使用有效的 null 值。</returns>
		/// <param name="value">绑定目标生成的值。</param>
		/// <param name="targetType">要转换到的类型。</param>
		/// <param name="parameter">要使用的转换器参数。</param>
		/// <param name="culture">要用在转换器中的区域性。</param>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}

	/// <summary>
	/// 
	/// </summary>
	public class TreeLineVisibleConverter : IValueConverter
	{
		private static Visibility ConvertTop(TreeViewItem tvi)
		{
			ItemsControl parentItemsControl = ItemsControl.ItemsControlFromItemContainer(tvi);
			TreeView parentTree = null;
			for(ItemsControl itemsControl = parentItemsControl; itemsControl != null; itemsControl = ItemsControl.ItemsControlFromItemContainer(itemsControl))
			{
				parentTree = itemsControl as TreeView;
				if(parentTree != null)
					break;
			}
			TreeViewItem parentItem = parentItemsControl as TreeViewItem;
			if(parentItem == null)
			{
				int? num = parentTree?.ItemContainerGenerator.IndexFromContainer(tvi);
				if(num == 0 || num == parentTree?.Items.Count - 1)
				{
					return Visibility.Collapsed;
				}
			}
			else
			{
				int num = parentItem.ItemContainerGenerator.IndexFromContainer(tvi);
				if(num == parentItem.Items.Count - 1)
				{
					return Visibility.Collapsed;
				}
			}
			return Visibility.Visible;
		}

		private static Visibility ConvertBottom(TreeViewItem tvi)
		{
			ItemsControl parentItemsControl = ItemsControl.ItemsControlFromItemContainer(tvi);
			TreeView parentTree = null;
			for(ItemsControl itemsControl = parentItemsControl; itemsControl != null; itemsControl = ItemsControl.ItemsControlFromItemContainer(itemsControl))
			{
				parentTree = itemsControl as TreeView;
				if(parentTree != null)
					break;
			}
			TreeViewItem parentItem = parentItemsControl as TreeViewItem;
			if(parentItem == null)
			{
				int? num = parentTree?.ItemContainerGenerator.IndexFromContainer(tvi);
				if(num == parentTree?.Items.Count - 1)
				{
					return Visibility.Collapsed;
				}
			}
			else
			{
				int num = parentItem.ItemContainerGenerator.IndexFromContainer(tvi);
				if(num == parentItem.Items.Count - 1)
				{
					return Visibility.Collapsed;
				}
			}
			return Visibility.Visible;
		}

		#region Implementation of IValueConverter

		/// <summary>转换值。</summary>
		/// <returns>转换后的值。如果该方法返回 null，则使用有效的 null 值。</returns>
		/// <param name="value">绑定源生成的值。</param>
		/// <param name="targetType">绑定目标属性的类型。</param>
		/// <param name="parameter">要使用的转换器参数。</param>
		/// <param name="culture">要用在转换器中的区域性。</param>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value is TreeViewItem tvi && parameter is string param)
			{
				if(param == "1")
				{
					return ConvertTop(tvi);
				}
				if(param == "2")
				{
					return ConvertBottom(tvi);
				}
			}
			return Visibility.Visible;
		}

		/// <summary>转换值。</summary>
		/// <returns>转换后的值。如果该方法返回 null，则使用有效的 null 值。</returns>
		/// <param name="value">绑定目标生成的值。</param>
		/// <param name="targetType">要转换到的类型。</param>
		/// <param name="parameter">要使用的转换器参数。</param>
		/// <param name="culture">要用在转换器中的区域性。</param>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}

	/// <summary>
	/// 
	/// </summary>
	public class TreeLineVisibleConverterFix : IValueConverter
	{
		private static Visibility ConvertTop(TreeViewItem tvi)
		{
			ItemsControl parentItemsControl = ItemsControl.ItemsControlFromItemContainer(tvi);
			TreeView parentTree = null;
			for(ItemsControl itemsControl = parentItemsControl; itemsControl != null; itemsControl = ItemsControl.ItemsControlFromItemContainer(itemsControl))
			{
				parentTree = itemsControl as TreeView;
				if(parentTree != null)
					break;
			}
			TreeViewItem parentItem = parentItemsControl as TreeViewItem;
			if(parentItem == null)
			{
				if(parentTree?.Items.Count == 1)
				{
					return Visibility.Collapsed;
				}
				int? num = parentTree?.ItemContainerGenerator.IndexFromContainer(tvi);
				if(num == 0)
				{
					return Visibility.Visible;
				}
			}
			return Visibility.Collapsed;
		}

		private static Visibility ConvertBottom(TreeViewItem tvi)
		{
			ItemsControl parentItemsControl = ItemsControl.ItemsControlFromItemContainer(tvi);
			TreeView parentTree = null;
			for(ItemsControl itemsControl = parentItemsControl; itemsControl != null; itemsControl = ItemsControl.ItemsControlFromItemContainer(itemsControl))
			{
				parentTree = itemsControl as TreeView;
				if(parentTree != null)
					break;
			}
			TreeViewItem parentItem = parentItemsControl as TreeViewItem;
			if(parentItem == null)
			{
				int? num = parentTree?.ItemContainerGenerator.IndexFromContainer(tvi);
				if(num == parentTree?.Items.Count - 1)
				{
					return Visibility.Visible;
				}
			}
			else
			{
				int num = parentItem.ItemContainerGenerator.IndexFromContainer(tvi);
				if(num == parentItem.Items.Count - 1)
				{
					return Visibility.Visible;
				}
			}
			return Visibility.Collapsed;
		}

		#region Implementation of IValueConverter

		/// <summary>转换值。</summary>
		/// <returns>转换后的值。如果该方法返回 null，则使用有效的 null 值。</returns>
		/// <param name="value">绑定源生成的值。</param>
		/// <param name="targetType">绑定目标属性的类型。</param>
		/// <param name="parameter">要使用的转换器参数。</param>
		/// <param name="culture">要用在转换器中的区域性。</param>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value is TreeViewItem tvi && parameter is string param)
			{
				if(param == "1")
				{
					return ConvertTop(tvi);
				}
				if(param == "2")
				{
					return ConvertBottom(tvi);
				}
			}
			return Visibility.Collapsed;
		}

		/// <summary>转换值。</summary>
		/// <returns>转换后的值。如果该方法返回 null，则使用有效的 null 值。</returns>
		/// <param name="value">绑定目标生成的值。</param>
		/// <param name="targetType">要转换到的类型。</param>
		/// <param name="parameter">要使用的转换器参数。</param>
		/// <param name="culture">要用在转换器中的区域性。</param>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}