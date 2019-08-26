/**
 * ==============================================================================
 *
 * ClassName: MergableTemplateColumn
 * Description: 
 *
 * Version: 1.0
 * Created: 2018/2/28 9:12:06
 * Compiler: Visual Studio 2017
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace HOTINST.COMMON.Controls.Controls.Editors
{
	/// <summary>
	/// 可合并显示单元格的模板列
	/// </summary>
	public class MergableTemplateColumn : DataGridTemplateColumn
	{
		private static readonly BrushConverter BackgroundBrushConverter = new BrushConverter();

		private readonly Dictionary<int, Canvas> _mergedCells = new Dictionary<int, Canvas>();

		/// <summary>
		/// 单元格合并确认
		/// </summary>
		public event MergeCellConfirmEventHandler MergeCellConfirmEvent;

		/// <summary>
		/// Using a DependencyProperty as the backing store for MergeColumn.  This enables animation, styling, binding, etc...
		/// </summary>
		public static readonly DependencyProperty MergeColumnProperty = DependencyProperty.Register(
			"MergeColumn", typeof(string), typeof(MergableTemplateColumn), new PropertyMetadata(string.Empty));
		/// <summary>
		/// 合并列列名（属性名）
		/// </summary>
		public string MergeColumn
		{
			get => (string)GetValue(MergeColumnProperty);
			set => SetValue(MergeColumnProperty, value);
		}

		/// <summary>
		/// ctor
		/// </summary>
		public MergableTemplateColumn()
		{
			IsReadOnly = true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cell"></param>
		/// <param name="dataItem"></param>
		/// <returns></returns>
		protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
		{
			DataGridRow row = cell.FindAncestor<DataGridRow>();
			int rowIndex = row.GetIndex();
			return _mergedCells.TryGetValue(rowIndex, out Canvas canvas) ? canvas : base.GenerateEditingElement(cell, dataItem);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cell"></param>
		/// <param name="dataItem"></param>
		/// <returns></returns>
		protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
		{
			MergeableCellInfo cellInfo = new MergeableCellInfo
			{
				Cell = cell,
				Column = this,
				DataGridOwner = DataGridOwner,
				Row = cell.FindAncestor<DataGridRow>()
			};
			int rowIndex = cellInfo.Row.GetIndex();
			//CanUserAddRows为true时（默认），增行忽略合并处理
			if(CollectionView.NewItemPlaceholder == dataItem)
			{
				return base.GenerateElement(cell, dataItem);
			}

			FrameworkElement content = base.GenerateElement(cell, dataItem);
			Grid grid = new Grid();
			// ReSharper disable once AssignNullToNotNullAttribute
			grid.Children.Add(content);

			Canvas canvas = new Canvas();
			canvas.Children.Add(grid);
			canvas.VerticalAlignment = VerticalAlignment.Top;
			//DataGridCell的默认模板外层是一个Thinkness为1的Border
			canvas.Margin = new Thickness(-1, -1, -1, 0);
			canvas.SetBinding(Panel.BackgroundProperty, new Binding("Background") { Source = cellInfo.Cell, Converter = BackgroundBrushConverter });

			grid.SetBinding(FrameworkElement.WidthProperty, new Binding("ActualWidth") { Source = canvas });
			grid.SetBinding(FrameworkElement.HeightProperty, new Binding("ActualHeight") { Source = canvas });

			_mergedCells[rowIndex] = canvas;
			UpdateMergedCell(cellInfo);
			cell.LayoutUpdated += (ss, ee) => UpdateMergedCell(cellInfo);
			return canvas;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cellInfo"></param>
		public void UpdateMergedCell(MergeableCellInfo cellInfo)
		{
			DataGridRow row = cellInfo.Row;
			int rowIndex = row.GetIndex();
			if(rowIndex < 0)
			{
				_mergedCells.Remove(rowIndex);
				return;
			}
			int minRowIndex = rowIndex;
			int maxRowIndex = rowIndex;
			double height = cellInfo.Cell.ActualHeight;
			for(int i = rowIndex - 1; i >= 0; i--)
			{
				//向上搜索可合并单元格
				DataGridRow mergeToRow = cellInfo.DataGridOwner.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
				if(IsMergable(row, mergeToRow))
				{
					height += mergeToRow.ActualHeight;
					minRowIndex = i;
				}
				else
				{
					break;
				}
			}
			for(int i = rowIndex + 1; i < cellInfo.DataGridOwner.Items.Count; i++)
			{
				//向下搜索可合并单元格
				DataGridRow mergeToRow = cellInfo.DataGridOwner.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
				if(IsMergable(row, mergeToRow))
				{
					height += mergeToRow.ActualHeight;
					maxRowIndex = i;
				}
				else
				{
					break;
				}
			}
			height = height > 0 ? height - 1 : 0;
			//行的Panel.ZIndex默认为零，所以默认行的遮盖方式由行的添加顺序决定
			//所以，如果想以行索引小的单元格遮盖索引大的，则需要通过改变所有行的Panel.ZIndex实现
			Panel.SetZIndex(row, cellInfo.DataGridOwner.Items.Count - rowIndex);
			Canvas mergedCell = _mergedCells[minRowIndex];
			mergedCell.Visibility = Visibility.Visible;
			mergedCell.Height = height;
			//隐藏组中的其他单元格的 Canvas
			for(int i = minRowIndex + 1; i <= maxRowIndex; i++)
			{
				_mergedCells[i].Visibility = Visibility.Collapsed;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="row1"></param>
		/// <param name="row2"></param>
		/// <returns></returns>
		public bool IsMergable(DataGridRow row1, DataGridRow row2)
		{
			if(row1 == null || row2 == null)
				return false;
			if(row1.Item == row2.Item)
				return true;
			if(row1.Item == null || row2.Item == null)
				return false;
			Type type = row1.Item.GetType();
			if(type != row2.Item.GetType())
				return false;

			string propertyName = MergeColumn;
			if(!string.IsNullOrEmpty(propertyName))
			{
				PropertyInfo p = type.GetProperty(propertyName, BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
				if(p != null)
				{
					return Equals(p.GetValue(row1.Item, null), p.GetValue(row2.Item, null));
				}
			}
			MergeCellConfirmEventArgs args = new MergeCellConfirmEventArgs(DataGridOwner, this, row1, row2);
			MergeCellConfirmEvent?.Invoke(this, args);
			return args.Mergable;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void MergeCellConfirmEventHandler(object sender, MergeCellConfirmEventArgs e);

	/// <summary>
	/// 
	/// </summary>
	public class MergeCellConfirmEventArgs : EventArgs
	{
		/// <summary>
		/// 
		/// </summary>
		public DataGrid DataGridOwner { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public DataGridTemplateColumn Column { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public DataGridRow Row1 { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public DataGridRow Row2 { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public bool Mergable { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dataGrid"></param>
		/// <param name="column"></param>
		/// <param name="row1"></param>
		/// <param name="row2"></param>
		public MergeCellConfirmEventArgs(DataGrid dataGrid, DataGridTemplateColumn column, DataGridRow row1, DataGridRow row2)
		{
			DataGridOwner = dataGrid;
			Column = column;
			Row1 = row1;
			Row2 = row2;
			Mergable = false;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public struct MergeableCellInfo
	{
		/// <summary>
		/// 
		/// </summary>
		public DataGrid DataGridOwner;
		/// <summary>
		/// 
		/// </summary>
		public DataGridTemplateColumn Column;
		/// <summary>
		/// 
		/// </summary>
		public DataGridRow Row;
		/// <summary>
		/// 
		/// </summary>
		public DataGridCell Cell;
	}

	/// <summary>
	/// 
	/// </summary>
	public class BrushConverter : IValueConverter
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Brush brush = value as Brush;
			if(brush == null || Equals(brush, Brushes.Transparent))
			{
				return Brushes.White;
			}
			return value;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	internal static class VisualTreeHelperEx
	{
		public static T FindDescendant<T>(this DependencyObject obj) where T : DependencyObject
		{
			int count = VisualTreeHelper.GetChildrenCount(obj);
			for(int i = 0; i < count; i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(obj, i);
				if(child is T)
				{
					return (T)child;
				}
			}
			for(int i = 0; i < count; i++)
			{
				T child = FindDescendant<T>(VisualTreeHelper.GetChild(obj, i));
				if(child != null)
				{
					return child;
				}
			}
			return null;
		}
		
		public static T FindAncestor<T>(this DependencyObject obj) where T : DependencyObject
		{
			DependencyObject p = obj;
			while(!(p == null || p is T))
			{
				p = VisualTreeHelper.GetParent(p);
			}
			return p as T;
		}
	}
}