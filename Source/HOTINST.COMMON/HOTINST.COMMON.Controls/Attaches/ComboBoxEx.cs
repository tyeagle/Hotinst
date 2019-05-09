/**
 * ==============================================================================
 *
 * ClassName: ComboBoxEx
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/8/10 11:28:20
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using HOTINST.COMMON.Controls.Net4._0.Helper;
using HOTINST.COMMON.Controls.Net4._0.VisualUtil;

namespace HOTINST.COMMON.Controls.Net4._0.Attaches
{
	public class TextData
	{
		public string Text { get; }
		public TextBlock TextOwner { get; }

		public TextData(string text, TextBlock tb)
		{
			Text = text;
			TextOwner = tb;
		}
	}

	/// <summary>
	/// 下拉框附加属性类
	/// </summary>
	public class ComboBoxEx
	{
		#region fields

		private static ComboBox _cb;

		private static readonly Dictionary<ComboBoxItem, TextData> _items = new Dictionary<ComboBoxItem, TextData>();

		private static bool _flag;

		#endregion

		#region props

		/// <summary>
		/// 定义附加属性 AutoFilter
		/// </summary>
		public static readonly DependencyProperty AutoFilterProperty = DependencyProperty.RegisterAttached(
			"AutoFilter", typeof(bool), typeof(ComboBoxEx), new PropertyMetadata(default(bool), AutoFilterChanged));
		/// <summary>
		/// 设置是否允许自动过滤
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
		[AttachedPropertyBrowsableForType(typeof(ComboBox))]
		public static void SetAutoFilter(DependencyObject element, bool value)
		{
			element.SetValue(AutoFilterProperty, value);
		}
		/// <summary>
		/// 获取是否允许自动过滤
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		[AttachedPropertyBrowsableForType(typeof(ComboBox))]
		public static bool GetAutoFilter(DependencyObject element)
		{
			return (bool)element.GetValue(AutoFilterProperty);
		}

		#endregion
		
		private static void AutoFilterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if(UIServices.IsInDesignMode())
			{
				return;
			}

			ComboBox cb = d as ComboBox;
			if(cb == null)
			{
				Debug.Fail("Invalid type！available type is \"System.Windows.Controls.ComboBox\"");
			}

			_cb = null;
			cb.SelectionChanged -= ComboBoxOnSelectionChanged;
			cb.Loaded -= ComboBoxOnLoaded;
			if((bool)e.NewValue)
			{
				_cb = cb;
				cb.Loaded += ComboBoxOnLoaded;
				cb.SelectionChanged += ComboBoxOnSelectionChanged;
			}
		}

		private static void ComboBoxOnLoaded(object sender, EventArgs e)
		{
			ComboBox cb = (ComboBox)sender;

			INotifyCollectionChanged collection = cb.ItemsSource as INotifyCollectionChanged;
			if(collection == null)
			{
				Debug.Fail("仅支持实现了\"System.Collections.Specialized.INotifyCollectionChanged\"接口的数据源。");
			}

			collection.CollectionChanged += ComboBoxOnCollectionChanged;

			bool isDropDownOpen = cb.IsDropDownOpen;
			cb.IsDropDownOpen = true;
			cb.IsDropDownOpen = isDropDownOpen;

			foreach(ComboBoxItem cbi in (from object existItem in cb.ItemsSource select _cb.ItemContainerGenerator.ContainerFromItem(existItem)).OfType<ComboBoxItem>())
			{
				cbi.Loaded += ComboBoxItemOnLoaded;
			}

			TextBox tb = VisualUtils.FindChild<TextBox>(cb, "PART_EditableTextBox");
			tb.GotFocus += InputBoxOnGotFocus;
			tb.TextChanged += InputBoxOnTextChanged;
		}

		private static void ComboBoxOnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			_flag = true;
		}

		private static void ComboBoxOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if(e.OldItems != null)
			{
				foreach(ComboBoxItem cbi in (from object oldItem in e.OldItems select _cb.ItemContainerGenerator.ContainerFromItem(oldItem)).OfType<ComboBoxItem>())
				{
					cbi.Loaded -= ComboBoxItemOnLoaded;
				}
			}
			if(e.NewItems != null)
			{
				foreach(ComboBoxItem cbi in (from object newItem in e.NewItems select _cb.ItemContainerGenerator.ContainerFromItem(newItem)).OfType<ComboBoxItem>())
				{
					cbi.Loaded += ComboBoxItemOnLoaded;
				}
			}

		}

		private static void ComboBoxItemOnLoaded(object sender, RoutedEventArgs e)
		{
			if(sender is ComboBoxItem cbi)
			{
				if(!_items.TryGetValue(cbi, out TextData _))
				{
					TextBlock tb = VisualUtils.FindChild<TextBlock>(cbi);
					_items.Add(cbi, new TextData(tb.Text, tb));
				}
			}
		}

		private static void InputBoxOnGotFocus(object sender, RoutedEventArgs e)
		{
			if(!_cb.IsDropDownOpen)
			{
				_cb.IsDropDownOpen = true;
			}
		}

		private static void InputBoxOnTextChanged(object sender, TextChangedEventArgs e)
		{
			if(_flag)
			{
				_flag = false;
				ShowAllNormal();
				return;
			}
			if(!_cb.IsDropDownOpen)
			{
				_cb.IsDropDownOpen = true;
			}

			Task.Factory.StartNew(Filter, ((TextBox)sender).Text);
		}
		
		private static void Filter(object key)
		{
			string strKey = key.ToString();
			foreach(KeyValuePair<ComboBoxItem, TextData> kvp in _items)
			{
				kvp.Value.TextOwner.Dispatcher.BeginInvoke((Action)delegate
				{
					Tuple<IList<Inline>, bool> result = FilterTextCore(kvp.Value.Text, strKey, Brushes.Black, Brushes.Red);
					if(result.Item2)
					{
						// 匹配成功替换ComboBoxItem里面的TextBlock里面的Inline对象
						kvp.Value.TextOwner.Inlines.Clear();
						kvp.Value.TextOwner.UpdateLayout();
						kvp.Value.TextOwner.Inlines.AddRange(result.Item1);
						kvp.Key.Visibility = Visibility.Visible;
					}
					else
					{
						kvp.Key.Visibility = Visibility.Collapsed;
					}
				});
			}
		}

		private static Tuple<IList<Inline>, bool> FilterTextCore(string source, string strFilter, Brush normal, Brush filter)
		{
			if(string.IsNullOrEmpty(source))
				return null;

			if(string.IsNullOrEmpty(strFilter))
			{
				return new Tuple<IList<Inline>, bool>(new List<Inline>
				{
					new Run(source) { Foreground = normal }
				}, true);
			}

			List<Inline> inlines = new List<Inline>();

			int foundPos = -1;
			int startPos = 0;
			do
			{
				foundPos = source.IndexOf(strFilter, startPos, StringComparison.OrdinalIgnoreCase);
				if(foundPos > -1)
				{
					if(foundPos == 0)
					{
						inlines.Add(new Run(strFilter) { Foreground = filter });
					}
					else if(foundPos == source.Length - 1)
					{
						inlines.Add(new Run(source.Substring(startPos, foundPos - startPos)) { Foreground = normal });
						inlines.Add(new Run(strFilter) { Foreground = filter });
					}
					else
					{
						inlines.Add(new Run(source.Substring(startPos, foundPos - startPos)) { Foreground = normal });
						inlines.Add(new Run(strFilter) { Foreground = filter });
					}
					startPos = foundPos + strFilter.Length;
				}
				else
				{
					inlines.Add(new Run(source.Substring(startPos)) { Foreground = normal });
				}
			}
			while(foundPos > -1 && startPos < source.Length);

			return new Tuple<IList<Inline>, bool>(inlines, source.IndexOf(strFilter, 0, StringComparison.OrdinalIgnoreCase) != -1);
		}

		private static void ShowAllNormal()
		{
			Filter(string.Empty);
		}
	}
}