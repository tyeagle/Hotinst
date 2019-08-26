/**
 * ==============================================================================
 * Classname   : FilterableComboBox
 * Description : 下拉列表选择输入框；
 *				 在可编辑时额外提供自动过滤功能。
 *
 * Compiler    : Visual Studio 2013
 * CLR Version : 4.0.30319.42000
 * Created     : 2017/3/14 11:44:01
 *
 * Author  : caixs
 * Company : Hotinst
 *
 * Copyright © Hotinst 2017. All rights reserved.
 * ==============================================================================
 */

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace HOTINST.COMMON.Controls.Controls.Editors
{
	/// <summary>
	/// Represents a selection control with a drop-down list that can be shown or hidden by clicking the arrow on the control.
	/// And additional provides automatic filtering function.
	/// </summary>
	public class FilterableComboBox : ComboBox
	{
		#region 构造器

		static FilterableComboBox()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(FilterableComboBox),
				new FrameworkPropertyMetadata(typeof(FilterableComboBox)));

			EventManager.RegisterClassHandler(typeof(FilterableComboBox), ItemActivedEvent, new RoutedEventHandler(
				(sender, args) =>
				{
					((FilterableComboBox)sender).ShowAllNormal();
				}));
		}

		/// <summary>
		/// .ctor
		/// </summary>
		public FilterableComboBox()
		{
			DropDownOpenOnFocus = false;
		}

		#endregion

		#region 字段

		/// <summary>
		/// 模板中文本框控件的名字
		/// </summary>
		private const string PART_InputBox = "PART_EditableTextBox";
		/// <summary>
		/// 文本输入框
		/// </summary>
		private TextBox _inputBox;

		private bool _inputFlag;
		private bool _setFlag;

		#endregion

		#region 依赖属性

		/// <summary>
		/// NormalBrush Dependency Property
		/// </summary>
		public static readonly DependencyProperty NormalBrushProperty = DependencyProperty.Register(
			"NormalBrush", typeof(Brush), typeof(FilterableComboBox), new PropertyMetadata(default(Brush)));

		/// <summary>
		/// 正常显示的颜色
		/// </summary>
		public Brush NormalBrush
		{
			get { return (Brush)GetValue(NormalBrushProperty); }
			set { SetValue(NormalBrushProperty, value); }
		}

		/// <summary>
		/// FilterBrush Dependency Property
		/// </summary>
		public static readonly DependencyProperty FilterBrushProperty = DependencyProperty.Register(
			"FilterBrush", typeof(Brush), typeof(FilterableComboBox), new PropertyMetadata(default(Brush)));

		/// <summary>
		/// 过滤字符显示的颜色
		/// </summary>
		public Brush FilterBrush
		{
			get { return (Brush)GetValue(FilterBrushProperty); }
			set { SetValue(FilterBrushProperty, value); }
		}

		/// <summary>
		/// HasFilteredItems Dependency Property
		/// </summary>
		public static readonly DependencyProperty HasFilteredItemsProperty = DependencyProperty.Register(
			"HasFilteredItems", typeof(bool), typeof(FilterableComboBox), new PropertyMetadata(default(bool)));

		/// <summary>
		/// 是否有匹配结果项
		/// </summary>
		public bool HasFilteredItems
		{
			get { return (bool)GetValue(HasFilteredItemsProperty); }
			set { SetValue(HasFilteredItemsProperty, value); }
		}

		/// <summary>
		/// 重写父类成员属性 ItemsSource
		/// </summary>
		public new static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
			"ItemsSource", typeof(IEnumerable), typeof(FilterableComboBox), new PropertyMetadata(default(IEnumerable), (o, args) =>
			{
				FilterableComboBox context = o as FilterableComboBox;
				if(context == null)
					return;
				if(args.NewValue == null)
					context.ClearItems();
				else
					context.CreateItems(args.NewValue as IEnumerable);
			}));
		/// <summary>
		/// 获取或设置用于生成 System.Windows.Controls.ItemsControl 的内容的集合。
		/// </summary>
		public new IEnumerable ItemsSource
		{
			get { return (IEnumerable)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		/// <summary>
		/// 重写父类成员属性 DisplayMemberPath
		/// </summary>
		public new static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register(
			"DisplayMemberPath", typeof(string), typeof(FilterableComboBox), new PropertyMetadata(default(string)));
		/// <summary>
		/// 获取或设置源对象上某个值的路径，该值作为对象的可视化表示形式。
		/// </summary>
		public new string DisplayMemberPath
		{
			get { return (string)GetValue(DisplayMemberPathProperty); }
			set { SetValue(DisplayMemberPathProperty, value); }
		}

		/// <summary>
		/// 重写父类成员属性 SelectedItem
		/// </summary>
		public new static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
			"SelectedItem", typeof(object), typeof(FilterableComboBox), new PropertyMetadata(default(object), (o, args) =>
			{
				FilterableComboBox context = o as FilterableComboBox;
				if(context != null)
					context.SetSelectedItem(args.NewValue);
			}));
		/// <summary>
		/// 获取或设置当前选择中的第一项，或者，如果选择为空，则返回 null。
		/// </summary>
		public new object SelectedItem
		{
			get { return GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}

		/// <summary>
		/// 定义依赖属性
		/// </summary>
		public static readonly DependencyProperty DropDownOpenOnFocusProperty = DependencyProperty.Register(
			"DropDownOpenOnFocus", typeof(bool), typeof(FilterableComboBox), new PropertyMetadata(default(bool)));
		/// <summary>
		/// 在获得焦点时是否要打开下拉列表
		/// </summary>
		public bool DropDownOpenOnFocus
		{
			get { return (bool)GetValue(DropDownOpenOnFocusProperty); }
			set { SetValue(DropDownOpenOnFocusProperty, value); }
		}	

		#endregion

		#region 事件

		/// <summary>
		/// ItemActived Routed Event
		/// </summary>
		public static RoutedEvent ItemActivedEvent = EventManager.RegisterRoutedEvent(
			"ItemActived", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FilterableComboBox));

		/// <summary>
		/// Add ItemActived Handler
		/// </summary>
		/// <param name="d"></param>
		/// <param name="handler"></param>
		public static void AddItemActivedHandler(DependencyObject d, RoutedEventHandler handler)
		{
			UIElement u = d as UIElement;
			if(u != null)
				u.AddHandler(ItemActivedEvent, handler);
		}

		/// <summary>
		/// Remove ItemActived Handler
		/// </summary>
		/// <param name="d"></param>
		/// <param name="handler"></param>
		public static void RemoveItemActivedHandler(DependencyObject d, RoutedEventHandler handler)
		{
			UIElement u = d as UIElement;
			if(u != null)
				u.RemoveHandler(ItemActivedEvent, handler);
		}

		#endregion

		#region 重写方法
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>
		protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
		{
			base.OnItemsSourceChanged(oldValue, newValue);

			HasFilteredItems = HasItems;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);

			HasFilteredItems = HasItems;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			base.OnSelectionChanged(e);

			if(e.AddedItems.Count == 0)
				return;

			if(!_setFlag)
			{
				SelectedItem = ((FilterableComboBoxItem)e.AddedItems[0]).Content;
				base.SelectedItem = e.AddedItems[0];
			}

			base.SelectedItem = e.AddedItems[0];

			_inputFlag = false;

			if ( _inputBox != null)
			{
				_inputBox.Text = ((FilterableComboBoxItem)e.AddedItems[0]).Tag.ToString();
				_inputBox.SelectAll();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			_inputFlag = true;

			base.OnPreviewKeyDown(e);

			switch(e.Key)
			{
				case Key.Escape:
				case Key.Enter:
					ShowAllNormal();
					break;
			}
		}

		/// <summary>
		/// 在派生类中重写后，每当应用程序代码或内部进程调用 <see cref="M:System.Windows.FrameworkElement.ApplyTemplate"/>，都将调用此方法。
		/// </summary>
		public override void OnApplyTemplate()
		{
			if(_inputBox != null)
			{
				_inputBox.TextChanged -= InputBox_OnTextChanged;
				_inputBox.GotFocus -= InputBox_OnGotFocus;
				_inputBox = null;
			}
			_inputBox = GetTemplateChild(PART_InputBox) as TextBox;
			if(_inputBox != null)
			{
				_inputBox.TextChanged += InputBox_OnTextChanged;
				_inputBox.GotFocus += InputBox_OnGotFocus;
				_inputBox.Loaded += InputBoxOnLoaded;
			}

			base.OnApplyTemplate();
		}

		#endregion

		#region 私有方法

		private void InputBoxOnLoaded(object sender, RoutedEventArgs e)
		{
			if (base.SelectedItem != null)
			{
				_inputBox.Text = ((FilterableComboBoxItem)base.SelectedItem)?.Tag?.ToString();
			}
		}

		private void ClearItems()
		{
			base.ItemsSource = null;
		}

		private void CreateItems(IEnumerable items)
		{
			List<FilterableComboBoxItem> list = new List<FilterableComboBoxItem>();
			foreach(object item in items)
			{
				FilterableComboBoxItem cbItem = new FilterableComboBoxItem { Content = item };
				if(!string.IsNullOrEmpty(DisplayMemberPath))
				{
					cbItem.SetBinding(TagProperty, new Binding(DisplayMemberPath)
					{
						StringFormat = ItemStringFormat,
						Source = item
					});
				}
				else
				{
					cbItem.Tag = item;
				}
				list.Add(cbItem);
			}

			base.ItemsSource = list;
		}

		private void SetSelectedItem(object newValue)
		{
			_setFlag = true;
			if(newValue == null)
			{
				if (_inputBox != null)
				{
					_inputBox.Text = string.Empty;
				}
				_setFlag = false;
				return;
			}
			foreach(FilterableComboBoxItem item in Items.Cast<FilterableComboBoxItem>().Where(item => item.Content.Equals(newValue)))
			{
				OnSelectionChanged(new SelectionChangedEventArgs(SelectedEvent, new List<FilterableComboBoxItem>(), new List<FilterableComboBoxItem> { item }));
				break;
			}
			_setFlag = false;
		}

		private void InputBox_OnTextChanged(object sender, TextChangedEventArgs args)
		{
			if(!_inputFlag)
				return;
			_inputFlag = false;

			if(args.Changes.Count > 0 && args.Changes.ToList()[0].RemovedLength > 0 && string.IsNullOrEmpty(_inputBox.Text))
			{
				ShowAllNormal();
				SelectedItem = base.SelectedItem = null;
				IsDropDownOpen = true;
				return;
			}

			if(_inputBox.Text.Equals(" ") || string.IsNullOrEmpty(_inputBox.Text) || _inputBox.Text.Equals(Text))
				return;

			if(!IsDropDownOpen)
			{
				IsDropDownOpen = true;
				_inputBox.SelectionStart = _inputBox.Text.Length;
			}

			string text = _inputBox.Text;

			SelectedItem = base.SelectedItem = null;

			UpdateItemsSource(text);
			_inputBox.Text = text;
			_inputBox.SelectionStart = _inputBox.Text.Length;
		}

		private void InputBox_OnGotFocus(object sender, RoutedEventArgs routedEventArgs)
		{
			if(DropDownOpenOnFocus)
				IsDropDownOpen = true;
		}

		private void UpdateItemsSource(string filterText)
		{
			foreach(FilterableComboBoxItem data in Items.OfType<FilterableComboBoxItem>())
			{
				data.FilterTextCore(filterText, NormalBrush, FilterBrush);
			}

			HasFilteredItems = Items.OfType<FilterableComboBoxItem>().Any(i => i.UIVisible);
		}

		private void ShowAllNormal()
		{
			foreach(FilterableComboBoxItem data in Items.OfType<FilterableComboBoxItem>())
			{
				data.ResetTextCore(NormalBrush);
			}

			HasFilteredItems = Items.OfType<FilterableComboBoxItem>().Any(i => i.UIVisible);
		}

		#endregion
	}
}