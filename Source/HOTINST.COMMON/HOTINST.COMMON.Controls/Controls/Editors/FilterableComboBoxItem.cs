/**
 * ==============================================================================
 * Classname   : FilterableComboBoxItem
 * Description : 可选择的下拉列表项，并可使用不同颜色显示过滤字符。
 *
 * Compiler    : Visual Studio 2013
 * CLR Version : 4.0.30319.42000
 * Created     : 2017/3/14 11:52:29
 *
 * Author  : caixs
 * Company : Hotinst
 *
 * Copyright © Hotinst 2017. All rights reserved.
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace HOTINST.COMMON.Controls.Controls.Editors
{
	/// <summary>
	/// Implements a selectable item inside a HOTINST.COMMON.Controls.Net4._5.Controls.FilterableComboBox.
	/// And can use different colors to display filter characters.
	/// </summary>
	public class FilterableComboBoxItem : ComboBoxItem
	{
		#region 构造器

		static FilterableComboBoxItem()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(FilterableComboBoxItem),
				new FrameworkPropertyMetadata(typeof(FilterableComboBoxItem)));
		}

		/// <summary>
		/// .ctor
		/// </summary>
		public FilterableComboBoxItem()
		{
			UIVisible = true;
		}

		#endregion

		#region 字段

		/// <summary>
		/// 模版中显示项的元素名
		/// </summary>
		public const string PART_TextPresenter = "PART_TextPresenter";
		private TextBlock _textPresenter;

		#endregion

		#region 依赖属性

		/// <summary>
		/// UIVisible Dependency Property
		/// </summary>
		public static readonly DependencyProperty UIVisibleProperty = DependencyProperty.Register(
			"UIVisible", typeof(bool), typeof(FilterableComboBoxItem), new PropertyMetadata(true));

		/// <summary>
		/// 是否可见
		/// </summary>
		public bool UIVisible
		{
			get { return (bool)GetValue(UIVisibleProperty); }
			set { SetValue(UIVisibleProperty, value); }
		}

		#endregion

		#region 私有方法

		/// <summary>
		/// 过滤字符颜色
		/// </summary>
		/// <param name="strFilter"></param>
		/// <param name="normal"></param>
		/// <param name="filter"></param>
		public void FilterTextCore(string strFilter, Brush normal, Brush filter)
		{
			UIVisible = false;

			if(_textPresenter == null || string.IsNullOrEmpty(strFilter) || !_textPresenter.Text.Contains(strFilter) || string.IsNullOrEmpty(_textPresenter.Text))
				return;

			List<Inline> inlines = new List<Inline>();

			int foundPos = -1;
			int startPos = 0;
			do
			{
				foundPos = _textPresenter.Text.IndexOf(strFilter, startPos, StringComparison.OrdinalIgnoreCase);
				if(foundPos > -1)
				{
					if(foundPos == 0)
					{
						inlines.Add(new Run(strFilter) { Foreground = filter });
					}
					else if(foundPos == _textPresenter.Text.Length - 1)
					{
						inlines.Add(new Run(_textPresenter.Text.Substring(startPos, foundPos - startPos)) { Foreground = normal });
						inlines.Add(new Run(strFilter) { Foreground = filter });
					}
					else
					{
						inlines.Add(new Run(_textPresenter.Text.Substring(startPos, foundPos - startPos)) { Foreground = normal });
						inlines.Add(new Run(strFilter) { Foreground = filter });
					}
					startPos = foundPos + strFilter.Length;
				}
				else
					inlines.Add(new Run(_textPresenter.Text.Substring(startPos)) { Foreground = normal });
			} while(foundPos > -1 && startPos < _textPresenter.Text.Length);

			_textPresenter.Inlines.Clear();
			_textPresenter.Inlines.AddRange(inlines);

			UIVisible = true;
		}

		/// <summary>
		/// 重置字符颜色
		/// </summary>
		/// <param name="normal"></param>
		public void ResetTextCore(Brush normal)
		{
			UIVisible = true;
			if(_textPresenter == null || string.IsNullOrEmpty(_textPresenter.Text))
				return;

			foreach(Inline inline in _textPresenter.Inlines)
			{
				inline.Foreground = normal;
			}
		}

		#endregion

		#region 重写方法

		/// <summary>
		/// 在派生类中重写后，每当应用程序代码或内部进程调用 <see cref="M:System.Windows.FrameworkElement.ApplyTemplate"/>，都将调用此方法。
		/// </summary>
		public override void OnApplyTemplate()
		{
			_textPresenter = GetTemplateChild(PART_TextPresenter) as TextBlock;

			base.OnApplyTemplate();
		}

		/// <summary>
		/// 响应 <see cref="E:System.Windows.UIElement.MouseLeftButtonUp"/> 事件。
		/// </summary>
		/// <param name="e">为 <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> 提供数据。</param>
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonUp(e);

			RaiseEvent(new RoutedEventArgs(FilterableComboBox.ItemActivedEvent, this));
		}
		
		#endregion
	}
}