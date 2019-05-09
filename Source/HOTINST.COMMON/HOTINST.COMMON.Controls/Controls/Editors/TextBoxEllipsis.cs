using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HOTINST.COMMON.Controls.Core;

namespace HOTINST.COMMON.Controls.Controls.Editors
{
	/// <summary>
	/// 
	/// </summary>
	public class TextBoxEllipsis : TextBox
	{
		private string _shortText;

		private EllipsisFormat _ellipsis;

		/// <summary>
		/// FullText1Property
		/// </summary>
		public static readonly DependencyProperty FullTextProperty = DependencyProperty.Register(
			"FullText", typeof(string), typeof(TextBoxEllipsis), new PropertyMetadata(default(string), (o, args) => ((TextBoxEllipsis)o).Text = args.NewValue.ToString()));

		/// <summary>
		/// Get the text associated with the control without ellipsis.
		/// </summary>
		public string FullText
		{
			get => (string)GetValue(FullTextProperty);
			set => SetValue(FullTextProperty, value);
		}

		/// <summary>
		/// TextProperty
		/// </summary>
		public new static readonly DependencyProperty TextProperty = DependencyProperty.Register(
			"Text", typeof(string), typeof(TextBoxEllipsis), new PropertyMetadata(default(string), (o, args) => ((TextBoxEllipsis)o).UpdateText(args.NewValue.ToString())));
		/// <summary>
		/// 设置文本框的文本内容。
		/// </summary>
		public new string Text
		{
			get => (string)GetValue(TextProperty);
			set
			{
				if(value == Text)
				{
					UpdateText(value);
				}
				else
				{
					SetValue(TextProperty, value);
				}
			}
		}

		/// <summary>
		/// Get the text associated with the control truncated if it exceeds the width of the control.
		/// </summary>
		[Browsable(false)]
		public virtual string EllipsisText => _shortText;

		/// <summary>
		/// Indicates whether the text exceeds the witdh of the control.
		/// </summary>
		[Browsable(false)]
		public virtual bool IsEllipsis => FullText != _shortText;

		/// <summary>
		/// 
		/// </summary>
		[Category("Behavior")]
		[Description("Define ellipsis format and alignment when text exceeds the width of the control")]
		public virtual EllipsisFormat AutoEllipsis
		{
			get { return _ellipsis; }
			set
			{
				if(_ellipsis != value)
				{
					_ellipsis = value;
					// ellipsis type changed, recalculate ellipsis text
					Text = FullText;
					OnAutoEllipsisChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[Category("Property Changed")]
		[Description("Event raised when the value of AutoEllipsis property is changed on Control")]
		public event EventHandler AutoEllipsisChanged;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected void OnAutoEllipsisChanged(EventArgs e)
		{
			AutoEllipsisChanged?.Invoke(this, e);
		}
		
		/// <summary>
		/// 初始化类<see cref="TextBoxEllipsis"/>的新实例。
		/// </summary>
		public TextBoxEllipsis()
		{
			SizeChanged += OnSizeChanged;
		}

		private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
		{
			if(!IsFocused) // doesn't apply if textbox has the focus
			{
				Text = FullText;
			}
		}

		private void UpdateText(string value)
		{
			FullText = value;
			_shortText = Ellipsis.Compact(FullText, this, AutoEllipsis);

			ToolTip = string.IsNullOrEmpty(value) ? null : value;
			base.Text = IsFocused ? FullText : _shortText;
		}

		#region Overrides of FrameworkElement

		/// <summary>每当未处理的 <see cref="E:System.Windows.UIElement.GotFocus" /> 事件在其路由中到达此元素时调用。</summary>
		/// <param name="e">包含事件数据的 <see cref="T:System.Windows.RoutedEventArgs" />。</param>
		protected override void OnGotFocus(RoutedEventArgs e)
		{
			base.Text = FullText;
			base.OnGotFocus(e);
		}

		/// <summary>引发 <see cref="E:System.Windows.UIElement.LostFocus" /> 事件（用提供的参数）。</summary>
		/// <param name="e">提供与事件有关的数据。</param>
		protected override void OnLostFocus(RoutedEventArgs e)
		{
			base.OnLostFocus(e);
			Text = base.Text;
		}
		
		/// <summary>在 <see cref="E:System.Windows.UIElement.KeyDown" /> 发生时调用。</summary>
		/// <param name="e">事件数据。</param>
		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			if(e.Key == Key.Enter)
			{
				Text = base.Text;
			}
			base.OnPreviewKeyDown(e);
		}
		
		#endregion
	}
}