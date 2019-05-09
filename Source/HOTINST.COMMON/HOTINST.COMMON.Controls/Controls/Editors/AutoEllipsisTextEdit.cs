using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;

namespace HOTINST.COMMON.Controls.Controls.Editors
{
	/// <summary>
	/// 
	/// </summary>
	public class AutoEllipsisTextEdit : ComboBox
	{
		#region fields

		private Button _btnRegular;

		#endregion

		#region props

		/// <summary>
		/// NoItemsHintProperty
		/// </summary>
		public static readonly DependencyProperty NoItemsHintProperty = DependencyProperty.Register(
			"NoItemsHint", typeof(string), typeof(AutoEllipsisTextEdit), new PropertyMetadata("没有数据。"));
		/// <summary>
		/// NoItemsHint
		/// </summary>
		public string NoItemsHint
		{
			get => (string)GetValue(NoItemsHintProperty);
			set => SetValue(NoItemsHintProperty, value);
		}

		/// <summary>
		/// FullTextProperty
		/// </summary>
		public static readonly DependencyProperty FullTextProperty = DependencyProperty.Register(
			"FullText", typeof(string), typeof(AutoEllipsisTextEdit), new PropertyMetadata(default(string)));
		/// <summary>
		/// FullText
		/// </summary>
		public string FullText
		{
			get => (string)GetValue(FullTextProperty);
			set => SetValue(FullTextProperty, value);
		}

		/// <summary>
		/// CmdRegularProperty
		/// </summary>
		public static readonly DependencyProperty CmdRegularProperty = DependencyProperty.Register(
			"CmdRegular", typeof(ICommand), typeof(AutoEllipsisTextEdit), new PropertyMetadata(ApplicationCommands.Open));
		/// <summary>
		/// 获取或设置Regular按钮的命令。
		/// </summary>
		public ICommand CmdRegular
		{
			get => (ICommand)GetValue(CmdRegularProperty);
			set => SetValue(CmdRegularProperty, value);
		}

		/// <summary>
		/// BtnRegularTooltipProperty
		/// </summary>
		public static readonly DependencyProperty BtnRegularTooltipProperty = DependencyProperty.Register(
			"BtnRegularTooltip", typeof(string), typeof(AutoEllipsisTextEdit), new PropertyMetadata("浏览 ..."));
		/// <summary>
		/// BtnRegularTooltip
		/// </summary>
		public string BtnRegularTooltip
		{
			get => (string)GetValue(BtnRegularTooltipProperty);
			set => SetValue(BtnRegularTooltipProperty, value);
		}

		/// <summary>
		/// MaxRecentItemCountProperty
		/// </summary>
		public static readonly DependencyProperty MaxRecentItemCountProperty = DependencyProperty.Register(
			"MaxRecentItemCount", typeof(int), typeof(AutoEllipsisTextEdit), new PropertyMetadata(5));
		/// <summary>
		/// MaxRecentItemCount
		/// </summary>
		public int MaxRecentItemCount
		{
			get => (int)GetValue(MaxRecentItemCountProperty);
			set => SetValue(MaxRecentItemCountProperty, value);
		}

		#endregion

		#region ctor

		/// <summary>
		/// Initializes a new instance of the <see cref="LoadingIndicator"/> class.
		/// </summary>
		public AutoEllipsisTextEdit()
		{
			
		}

		static AutoEllipsisTextEdit()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoEllipsisTextEdit), new FrameworkPropertyMetadata(typeof(AutoEllipsisTextEdit)));
		}

		#endregion

		#region Overrides of ComboBox

		/// <summary>通过引发 <see cref="E:System.Windows.Controls.Primitives.Selector.SelectionChanged" /> 事件来对 <see cref="T:System.Windows.Controls.ComboBox" /> 选择更改进行响应。</summary>
		/// <param name="e">为 <see cref="T:System.Windows.Controls.SelectionChangedEventArgs" /> 提供数据。</param>
		protected override void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			base.OnSelectionChanged(e);

			if(e.AddedItems.Count > 0)
			{
				FullText = e.AddedItems[0].ToString();
			}
		}
		
		/// <summary>调用 <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" /> 时进行调用。</summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_btnRegular = GetTemplateChild("BtnRegular") as Button;
			if(_btnRegular != null)
			{
				_btnRegular.Click += (sender, args) => IsDropDownOpen = false;

				_btnRegular.CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, OpenExecuted, CanOpenExecute));
			}
		}

		private void CanOpenExecute(object o, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void OpenExecuted(object o, ExecutedRoutedEventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog
			{
				Title = "选择文件...",
				DefaultExt = ".xlsx",
				Filter = "Excel文件 (*.xlsx)|*.xlsx|所有文件 (*.*)|*.*"
			};
			if(dlg.ShowDialog() == true)
			{
				UpdateRecentItems(dlg.FileName);
				SelectFirstItem();
			}
		}

		#endregion

		/// <summary>
		/// 更新最近记录
		/// </summary>
		/// <param name="fileName"></param>
		public void UpdateRecentItems(string fileName)
		{
			if(!HasItems)
			{
				Items.Add(fileName);
				return;
			}

			if(Items.Contains(fileName))
			{
				Items.Remove(fileName);
			}

			Items.Insert(0, fileName);
		}

		private void SelectFirstItem()
		{
			if(Items.Count > 0)
			{
				SelectedItem = Items[0];
			}
		}
	}
}