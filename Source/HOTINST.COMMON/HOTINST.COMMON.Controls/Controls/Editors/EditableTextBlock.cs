/**
 * ==============================================================================
 *
 * ClassName: EditableTextBlock
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/7/7 16:11:12
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
using System.Windows.Input;
using System.Windows.Media;
using HOTINST.COMMON.Controls.Attaches;
using HOTINST.COMMON.Controls.VisualUtil;

namespace HOTINST.COMMON.Controls.Controls.Editors
{
	/// <summary>
	/// 可编辑的文本显示控件
	/// </summary>
	[TemplatePart(Type = typeof(TextBlock), Name = TEXTBLOCK_DISPLAYTEXT_NAME)]
	[TemplatePart(Type = typeof(TextBox), Name = TEXTBOX_EDITTEXT_NAME)]
	public class EditableTextBlock : ContentControl
	{
		#region Constants

		private const string TEXTBLOCK_DISPLAYTEXT_NAME = "PART_TbDisplayText";
		private const string TEXTBOX_EDITTEXT_NAME = "PART_TbEditText";

		#endregion
		
		#region Member Variables

		// We keep the old text when we go into editmode
		// in case the user aborts with the escape key

		private TextBox _tbEdit;

		private bool _esc;

		#endregion Member Variables

		#region Properties

		/// <summary>
		/// 
		/// </summary>
		public string Text
		{
			get => (string)GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(EditableTextBlock), new PropertyMetadata(string.Empty));

		/// <summary>
		/// 
		/// </summary>
		public bool IsEditable
		{
			get => (bool)GetValue(IsEditableProperty);
			set => SetValue(IsEditableProperty, value);
		}

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty IsEditableProperty = DependencyProperty.Register("IsEditable", typeof(bool), typeof(EditableTextBlock), new PropertyMetadata(true));

		/// <summary>
		/// 
		/// </summary>
		public bool IsInEditMode
		{
			get => IsEditable && (bool)GetValue(IsInEditModeProperty);
			set
			{
				if(IsEditable)
				{
					SetValue(IsInEditModeProperty, value);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty IsInEditModeProperty = DependencyProperty.Register("IsInEditMode", typeof(bool), typeof(EditableTextBlock), new PropertyMetadata(false, (o, args) =>
		{
			EditableTextBlock context = o as EditableTextBlock;
			context?.IsInEditModeChanged((bool)args.NewValue);
		}));

		/// <summary>
		/// 
		/// </summary>
		public string TextFormat
		{
			get => (string)GetValue(TextFormatProperty);
			set => SetValue(TextFormatProperty, string.IsNullOrEmpty(value) ? "{0}" : value);
		}

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty TextFormatProperty = DependencyProperty.Register("TextFormat", typeof(string), typeof(EditableTextBlock), new PropertyMetadata("{0}"));

		/// <summary>
		/// 
		/// </summary>
		public string FormattedText => string.Format(string.IsNullOrEmpty(TextFormat) ? "{0}" : TextFormat, Text);

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty TextExProperty = DependencyProperty.Register("TextEx", typeof(string), typeof(EditableTextBlock), new PropertyMetadata(default(string)));
		/// <summary>
		/// 
		/// </summary>
		public string TextEx
		{
			get => (string)GetValue(TextExProperty);
			set => SetValue(TextExProperty, value);
		}

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty TextExBrushProperty = DependencyProperty.Register("TextExBrush", typeof(Brush), typeof(EditableTextBlock), new PropertyMetadata(Brushes.Black));
		/// <summary>
		/// 
		/// </summary>
		public Brush TextExBrush
		{
			get => (Brush)GetValue(TextExBrushProperty);
			set => SetValue(TextExBrushProperty, value);
		}

		/// <summary>
		/// 获取编辑前的文本
		/// </summary>
		public string OldText { get; private set; }

		#endregion Properties
		
		#region Constructor

		static EditableTextBlock()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(EditableTextBlock), new FrameworkPropertyMetadata(typeof(EditableTextBlock)));
		}

		/// <summary>
		/// 
		/// </summary>
		public EditableTextBlock()
		{
			
		}

		#endregion

		#region Overrides of FrameworkElement

		/// <summary>在派生类中重写后，每当应用程序代码或内部进程调用 <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />，都将调用此方法。</summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_tbEdit = GetTemplateChild(TEXTBOX_EDITTEXT_NAME) as TextBox;
			if(_tbEdit != null)
			{
				_tbEdit.Loaded += TextBox_Loaded;
				_tbEdit.LostFocus += TextBox_LostFocus;
				_tbEdit.KeyDown += TextBox_KeyDown;
				_tbEdit.MouseMove += (sender, args) => args.Handled = true;
			}
			
			TreeViewItem tvi = VisualUtils.FindVisualParent<TreeViewItem>(this);
			if(tvi != null && tvi.IsSelected)
			{
				tvi.Focus();
			}
		}
		
		#endregion

		#region Event Handlers

		// Invoked when we enter edit mode.
		private void TextBox_Loaded(object sender, RoutedEventArgs e)
		{
			TextBox txt = sender as TextBox;

			// Give the TextBox input focus
			txt.Focus();

			txt.SelectAll();
		}

		// Invoked when we exit edit mode.
		private void TextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			IsInEditMode = false;
		}
		
		// Invoked when the user edits the annotation.
		private void TextBox_KeyDown(object sender, KeyEventArgs e)
		{
			_esc = false;
			if (e.Key == Key.Enter)
			{
				Text = _tbEdit.Text;
				IsInEditMode = false;
				e.Handled = true;
			}
			else if(e.Key == Key.Escape)
			{
				_esc = true;
				IsInEditMode = false;
				Text = OldText;
				e.Handled = true;
			}
		}

		private void IsInEditModeChanged(bool argsNewValue)
		{
			if(argsNewValue)
			{
				OldText = Text;
			}
			else
			{
				if(!_esc)
				{
					EditDoneRoutedEventArgs args = new EditDoneRoutedEventArgs(TreeItemEx.EditDoneEvent, VisualUtils.FindVisualParent<TreeViewItem>(this), OldText);
					VisualUtils.FindVisualParent<TreeView>(this)?.RaiseEvent(args);

					_esc = false;

					// 如果已被处理, 恢复文本
					if(args.Handled)
					{
						Text = OldText;
					}
					// 排序
					if(VisualUtils.VisualUpSearch<TreeViewItem>(this) is TreeViewItem tvi && tvi.DataContext is INode node && node.Parent is ICustomSort sorter)
					{
						sorter.Sort();
					}
				}

				VisualUtils.FindVisualParent<TreeViewItem>(this)?.Focus();
			}
		}

		#endregion Event Handlers
	}
}