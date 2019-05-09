/**
 * ==============================================================================
 *
 * ClassName: IntegerTextBox
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/3/23 14:45:34
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using HOTINST.COMMON.Controls.Core;
using HOTINST.COMMON.Controls.Core.Editors;

#pragma warning disable 1591

namespace HOTINST.COMMON.Controls.Controls.Editors
{
	public class IntegerTextBox : EditorBase, IDisposable
	{
		internal long? OldValue;
		internal long? mValue;
		internal bool? mValueChanged = new bool?(true);
		internal bool mIsLoaded;
		internal int count = 1;
		internal string checktext = "";
		private ScrollViewer PART_ContentHost;
		private bool _validatingrResult;
		public static readonly DependencyProperty GroupSeperatorEnabledProperty;
		public static readonly DependencyProperty NumberGroupSizesProperty;
		public static readonly DependencyProperty ValueProperty;
		public static readonly DependencyProperty MinValueProperty;
		public static readonly DependencyProperty MaxValueProperty;
		public static readonly DependencyProperty NumberGroupSeparatorProperty;
		public static readonly DependencyProperty ScrollIntervalProperty;
		internal double percentage;
		public static readonly DependencyProperty NullValueProperty;
		public static readonly DependencyProperty ValueValidationProperty;
		public static readonly DependencyProperty InvalidValueBehaviorProperty;
		public static readonly DependencyProperty ValidationValueProperty;
		public static readonly DependencyProperty ValidationCompletedProperty;
		public event PropertyChangedCallback ValidationCompletedChanged;
		public event PropertyChangedCallback InvalidValueBehaviorChanged;
		public event StringValidationCompletedEventHandler ValueValidationCompleted;
		public event PropertyChangedCallback ValidationValueChanged;
		public event CancelEventHandler Validating;
		public event EventHandler Validated;
		public event PropertyChangedCallback MinValueChanged;
		public event PropertyChangedCallback ValueChanged;
		public event PropertyChangedCallback MaxValueChanged;
		public event PropertyChangedCallback NumberGroupSizesChanged;
		public event PropertyChangedCallback NumberGroupSeparatorChanged;
		public ICommand pastecommand
		{
			get;
			private set;
		}
		public ICommand copycommand
		{
			get;
			private set;
		}
		public ICommand cutcommand
		{
			get;
			private set;
		}
		public long? Value
		{
			get
			{
				return (long?)base.GetValue(IntegerTextBox.ValueProperty);
			}
			set
			{
				base.SetValue(IntegerTextBox.ValueProperty, value);
			}
		}
		public long MinValue
		{
			get
			{
				return (long)base.GetValue(IntegerTextBox.MinValueProperty);
			}
			set
			{
				base.SetValue(IntegerTextBox.MinValueProperty, value);
			}
		}
		public long MaxValue
		{
			get
			{
				return (long)base.GetValue(IntegerTextBox.MaxValueProperty);
			}
			set
			{
				base.SetValue(IntegerTextBox.MaxValueProperty, value);
			}
		}
		public string NumberGroupSeparator
		{
			get
			{
				return (string)base.GetValue(IntegerTextBox.NumberGroupSeparatorProperty);
			}
			set
			{
				base.SetValue(IntegerTextBox.NumberGroupSeparatorProperty, value);
			}
		}
		public bool GroupSeperatorEnabled
		{
			get
			{
				return (bool)base.GetValue(IntegerTextBox.GroupSeperatorEnabledProperty);
			}
			set
			{
				base.SetValue(IntegerTextBox.GroupSeperatorEnabledProperty, value);
			}
		}
		public Int32Collection NumberGroupSizes
		{
			get
			{
				return (Int32Collection)base.GetValue(IntegerTextBox.NumberGroupSizesProperty);
			}
			set
			{
				base.SetValue(IntegerTextBox.NumberGroupSizesProperty, value);
			}
		}
		public double ProgressFactor
		{
			get
			{
				return this.percentage / base.ActualWidth;
			}
		}
		public int ScrollInterval
		{
			get
			{
				return (int)base.GetValue(IntegerTextBox.ScrollIntervalProperty);
			}
			set
			{
				base.SetValue(IntegerTextBox.ScrollIntervalProperty, value);
			}
		}
		public long? NullValue
		{
			get
			{
				return (long?)base.GetValue(IntegerTextBox.NullValueProperty);
			}
			set
			{
				base.SetValue(IntegerTextBox.NullValueProperty, value);
			}
		}
		public StringValidation ValueValidation
		{
			get
			{
				return (StringValidation)base.GetValue(IntegerTextBox.ValueValidationProperty);
			}
			set
			{
				base.SetValue(IntegerTextBox.ValueValidationProperty, value);
			}
		}
		public InvalidInputBehavior InvalidValueBehavior
		{
			get
			{
				return (InvalidInputBehavior)base.GetValue(IntegerTextBox.InvalidValueBehaviorProperty);
			}
			set
			{
				base.SetValue(IntegerTextBox.InvalidValueBehaviorProperty, value);
			}
		}
		public string ValidationValue
		{
			get
			{
				return (string)base.GetValue(IntegerTextBox.ValidationValueProperty);
			}
			set
			{
				base.SetValue(IntegerTextBox.ValidationValueProperty, value);
			}
		}
		public bool ValidationCompleted
		{
			get
			{
				return (bool)base.GetValue(IntegerTextBox.ValidationCompletedProperty);
			}
			set
			{
				base.SetValue(IntegerTextBox.ValidationCompletedProperty, value);
			}
		}
		static IntegerTextBox()
		{
			IntegerTextBox.GroupSeperatorEnabledProperty = DependencyProperty.Register("GroupSeperatorEnabled", typeof(bool), typeof(IntegerTextBox), new PropertyMetadata(true, new PropertyChangedCallback(IntegerTextBox.OnNumberGroupSeparatorChanged)));
			IntegerTextBox.NumberGroupSizesProperty = DependencyProperty.Register("NumberGroupSizes", typeof(Int32Collection), typeof(IntegerTextBox), new PropertyMetadata(new Int32Collection(), new PropertyChangedCallback(IntegerTextBox.OnNumberGroupSizesChanged)));
			IntegerTextBox.ValueProperty = DependencyProperty.Register("Value", typeof(long?), typeof(IntegerTextBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(IntegerTextBox.OnValueChanged), new CoerceValueCallback(IntegerTextBox.CoerceValue), false, UpdateSourceTrigger.LostFocus));
			IntegerTextBox.MinValueProperty = DependencyProperty.Register("MinValue", typeof(long), typeof(IntegerTextBox), new PropertyMetadata(-9223372036854775808L, new PropertyChangedCallback(IntegerTextBox.OnMinValueChanged)));
			IntegerTextBox.MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(long), typeof(IntegerTextBox), new PropertyMetadata(9223372036854775807L, new PropertyChangedCallback(IntegerTextBox.OnMaxValueChanged)));
			IntegerTextBox.NumberGroupSeparatorProperty = DependencyProperty.Register("NumberGroupSeparator", typeof(string), typeof(IntegerTextBox), new PropertyMetadata(string.Empty, new PropertyChangedCallback(IntegerTextBox.OnNumberGroupSeparatorChanged), new CoerceValueCallback(IntegerTextBox.CoerceNumberGroupSeperator)));
			IntegerTextBox.ScrollIntervalProperty = DependencyProperty.Register("ScrollInterval", typeof(int), typeof(IntegerTextBox), new PropertyMetadata(1));
			IntegerTextBox.NullValueProperty = DependencyProperty.Register("NullValue", typeof(long?), typeof(IntegerTextBox), new PropertyMetadata(null, new PropertyChangedCallback(IntegerTextBox.OnNullValueChanged)));
			IntegerTextBox.ValueValidationProperty = DependencyProperty.Register("ValueValidation", typeof(StringValidation), typeof(IntegerTextBox), new PropertyMetadata(StringValidation.OnLostFocus));
			IntegerTextBox.InvalidValueBehaviorProperty = DependencyProperty.Register("InvalidValueBehavior", typeof(InvalidInputBehavior), typeof(IntegerTextBox), new PropertyMetadata(InvalidInputBehavior.None, new PropertyChangedCallback(IntegerTextBox.OnInvalidValueBehaviorChanged)));
			IntegerTextBox.ValidationValueProperty = DependencyProperty.Register("ValidationValue", typeof(string), typeof(IntegerTextBox), new PropertyMetadata(string.Empty, new PropertyChangedCallback(IntegerTextBox.OnValidationValueChanged)));
			IntegerTextBox.ValidationCompletedProperty = DependencyProperty.Register("ValidationCompleted", typeof(bool), typeof(IntegerTextBox), new PropertyMetadata(false, new PropertyChangedCallback(IntegerTextBox.OnValidationCompletedPropertyChanged)));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(IntegerTextBox), new FrameworkPropertyMetadata(typeof(IntegerTextBox)));
		}
		public IntegerTextBox()
		{
			this.pastecommand = new DelegateCommand(new Action<object>(this._pastecommand), new Predicate<object>(this.Canpaste));
			this.copycommand = new DelegateCommand(new Action<object>(this._copycommand), new Predicate<object>(this.Canpaste));
			this.cutcommand = new DelegateCommand(new Action<object>(this._cutcommand), new Predicate<object>(this.Canpaste));
			base.RemoveHandler(CommandManager.PreviewExecutedEvent, new ExecutedRoutedEventHandler(this.CommandExecuted));
			base.AddHandler(CommandManager.PreviewExecutedEvent, new ExecutedRoutedEventHandler(this.CommandExecuted), true);
			base.Loaded -= new RoutedEventHandler(this.IntegerTextbox_Loaded);
			base.Loaded += new RoutedEventHandler(this.IntegerTextbox_Loaded);
			base.Unloaded += new RoutedEventHandler(this.IntegerTextBox_Unloaded);
		}
		private void IntegerTextBox_Unloaded(object sender, RoutedEventArgs e)
		{
			base.Unloaded -= new RoutedEventHandler(this.IntegerTextBox_Unloaded);
			base.RemoveHandler(CommandManager.PreviewExecutedEvent, new ExecutedRoutedEventHandler(this.CommandExecuted));
		}
		public void Dispose()
		{
			base.Unloaded -= new RoutedEventHandler(this.IntegerTextBox_Unloaded);
			base.RemoveHandler(CommandManager.PreviewExecutedEvent, new ExecutedRoutedEventHandler(this.CommandExecuted));
		}
		void IDisposable.Dispose()
		{
			this.Dispose();
		}
		private void _pastecommand(object parameter)
		{
			this.Paste();
		}
		private void _copycommand(object parameter)
		{
			this.copy();
		}
		private void _cutcommand(object parameter)
		{
			this.cut();
		}
		private bool Canpaste(object parameter)
		{
			return true;
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.PART_ContentHost = (base.GetTemplateChild("PART_ContentHost") as ScrollViewer);
		}
		private void CommandExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			if(e.Command == ApplicationCommands.Paste)
			{
				this.Paste();
				e.Handled = true;
			}
			if(e.Command == ApplicationCommands.Cut)
			{
				this.cut();
				e.Handled = true;
			}
		}
		private void copy()
		{
			try
			{
				Clipboard.SetText(base.SelectedText);
			}
			catch(SecurityException)
			{
			}
			catch(COMException)
			{
			}
		}
		private new void Paste()
		{
			if(!base.IsReadOnly)
			{
				try
				{
					string text = Clipboard.GetText();
					if(this.NumberGroupSeparator != "." && text.Contains("."))
					{
						int startIndex = text.IndexOf(".");
						text = text.Remove(startIndex);
					}
					string text2 = text;
					int selectionStart = base.SelectionStart;
					base.GetType().ToString();
					NumberFormatInfo numberFormat = this.GetCulture().NumberFormat;
					double num = 0.0;
					if(!base.UseNullOption && this.Value.HasValue)
					{
						num = (double)this.Value.Value;
					}
					int num2 = 0;
					if(base.SelectedText.Length == base.Text.Length)
					{
						if(text2[0] == '-')
						{
							num2 = 1;
						}
						else
						{
							if(base.Culture.Name == "ar-SA" && text2[text2.Length - 1] == '-')
							{
								num2 = 1;
							}
						}
					}
					for(int i = 0; i < text2.Length; i++)
					{
						if(!char.IsDigit(text2[i]))
						{
							text2 = text2.Remove(i, 1);
							i--;
						}
					}
					if(base.SelectionLength > 0)
					{
						if(text2 == string.Empty)
						{
							if(this.OldValue.HasValue)
							{
								this.SetValue(new bool?(false), this.OldValue);
								base.Text = this.OldValue.Value.ToString("N", numberFormat);
							}
						}
						else
						{
							base.Text = base.Text.Replace(base.SelectedText, text2);
						}
					}
					else
					{
						base.Text = base.Text.Insert(base.SelectionStart, text2);
					}
					if(num2 == 1)
					{
						base.Text = "-" + base.Text;
					}
					if(numberFormat.NumberDecimalSeparator != string.Empty && numberFormat.NumberDecimalSeparator != CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
					{
						base.Text = base.Text.Replace(numberFormat.NumberDecimalSeparator, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
					}
					if(numberFormat.NumberGroupSeparator != string.Empty && numberFormat.NumberGroupSeparator != CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator)
					{
						base.Text = base.Text.Replace(numberFormat.NumberGroupSeparator, CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator);
					}
					double num3;
					double.TryParse(base.Text, out num3);
					if((num3 > (double)this.MaxValue && base.MaxValidation == MaxValidation.OnKeyPress) || (num3 < (double)this.MinValue && base.MinValidation == MinValidation.OnKeyPress))
					{
						if(num3 > (double)this.MaxValue && base.MaxValidation == MaxValidation.OnKeyPress)
						{
							if(base.MaxValueOnExceedMaxDigit)
							{
								num3 = (double)this.MaxValue;
								base.CaretIndex = selectionStart;
								this.SetValue(new bool?(false), new long?((long)num3));
								base.Text = num3.ToString("N", numberFormat);
							}
							else
							{
								this.SetValue(new bool?(false), new long?((long)num));
								double num4 = num;
								base.Text = num4.ToString("N", numberFormat);
							}
						}
						if(num3 < (double)this.MinValue && base.MinValidation == MinValidation.OnKeyPress)
						{
							if(base.MinValueOnExceedMinDigit)
							{
								num3 = (double)this.MinValue;
								base.CaretIndex = selectionStart;
								this.SetValue(new bool?(false), new long?((long)num3));
								base.Text = num3.ToString("N", numberFormat);
							}
							else
							{
								this.SetValue(new bool?(false), new long?((long)num));
								double num5 = num;
								base.Text = num5.ToString("N", numberFormat);
							}
						}
					}
					else
					{
						this.SetValue(new bool?(false), new long?((long)num3));
						if(num2 == 1)
						{
							base.CaretIndex = selectionStart + num2 + text2.Length;
						}
						else
						{
							base.CaretIndex = selectionStart + text2.Length;
						}
						base.Text = num3.ToString("N", numberFormat);
					}
				}
				catch(SecurityException)
				{
				}
				catch(COMException)
				{
				}
			}
		}
		protected override void OnMouseWheel(MouseWheelEventArgs e)
		{
			base.OnMouseWheel(e);
			if(e.Delta > 0)
			{
				IntegerValueHandler.integerValueHandler.HandleUpKey(this);
				return;
			}
			if(e.Delta < 0)
			{
				IntegerValueHandler.integerValueHandler.HandleDownKey(this);
			}
		}
		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			e.Handled = IntegerValueHandler.integerValueHandler.HandleKeyDown(this, e);
			if(e.Key == Key.Z && !base.IsUndoEnabled)
			{
				e.Handled = true;
			}
			base.OnPreviewKeyDown(e);
		}
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if(ModifierKeys.Control == Keyboard.Modifiers)
			{
				if(e.Key == Key.V)
				{
					this.Paste();
					e.Handled = true;
				}
				if(e.Key == Key.C)
				{
					this.copy();
				}
				if(e.Key == Key.X)
				{
					this.cut();
					e.Handled = true;
				}
			}
			else
			{
				if(e.Key == Key.Return)
				{
					if(base.EnterToMoveNext)
					{
						FocusNavigationDirection focusNavigationDirection = FocusNavigationDirection.Next;
						TraversalRequest request = new TraversalRequest(focusNavigationDirection);
						UIElement uIElement = Keyboard.FocusedElement as UIElement;
						if(uIElement != null)
						{
							uIElement.MoveFocus(request);
							e.Handled = true;
						}
					}
				}
				else
				{
					e.Handled = IntegerValueHandler.integerValueHandler.HandleKeyDown(this, e);
				}
			}
			base.OnKeyDown(e);
		}
		private void cut()
		{
			if(base.SelectionLength > 0)
			{
				try
				{
					Clipboard.SetText(base.SelectedText);
					this.count = 1;
					IntegerValueHandler.integerValueHandler.HandleDeleteKey(this);
				}
				catch(SecurityException)
				{
				}
				catch(COMException)
				{
				}
			}
		}
		protected override void OnTextInput(TextCompositionEventArgs e)
		{
			int caretIndex = base.CaretIndex;
			if(base.CaretIndex > 0 && InputLanguageManager.Current.CurrentInputLanguage.DisplayName.Contains("Chinese"))
			{
				base.Text = base.Text.Remove(base.CaretIndex - e.Text.Length, e.Text.Length);
			}
			base.CaretIndex = caretIndex;
			e.Handled = IntegerValueHandler.integerValueHandler.MatchWithMask(this, e.Text);
			base.OnTextInput(e);
		}
		internal override void OnCultureChanged()
		{
			base.OnCultureChanged();
			if(this.mIsLoaded)
			{
				this.FormatText();
			}
		}
		internal override void OnNumberFormatChanged()
		{
			base.OnNumberFormatChanged();
			if(this.mIsLoaded)
			{
				this.FormatText();
			}
		}
		protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			if(this._validatingrResult)
			{
				e.Handled = true;
			}
			base.OnPreviewLostKeyboardFocus(e);
		}
		protected override void OnLostFocus(RoutedEventArgs e)
		{
			if(!this.OnValidating(new CancelEventArgs(false)))
			{
				string message = "";
				bool flag = true;
				flag = (this.ValidationValue == this.Value.ToString());
				string messageBoxText = flag ? "String validation succeeded" : "String validation failed";
				if(!flag)
				{
					if(this.InvalidValueBehavior == InvalidInputBehavior.DisplayErrorMessage)
					{
						MessageBox.Show(messageBoxText, "Invalid value", MessageBoxButton.OK);
						this.OnValueValidationCompleted(new StringValidationEventArgs(flag, message, this.ValidationValue));
						this.OnValidated(EventArgs.Empty);
					}
					else
					{
						if(this.InvalidValueBehavior == InvalidInputBehavior.ResetValue)
						{
							this.OnValueValidationCompleted(new StringValidationEventArgs(flag, message, this.ValidationValue));
							this.OnValidated(EventArgs.Empty);
							base.SetCurrentValue(IntegerTextBox.ValueProperty, null);
						}
						else
						{
							if(this.InvalidValueBehavior == InvalidInputBehavior.None)
							{
								this.OnValueValidationCompleted(new StringValidationEventArgs(flag, message, this.ValidationValue));
								this.OnValidated(EventArgs.Empty);
							}
						}
					}
				}
			}
			if(base.EnableFocusColors && this.PART_ContentHost != null)
			{
				this.PART_ContentHost.Background = base.Background;
			}
			long? value = this.Value;
			if(value.HasValue)
			{
				long? num = value;
				long maxValue = this.MaxValue;
				if(num.GetValueOrDefault() > maxValue && num.HasValue)
				{
					value = new long?(this.MaxValue);
				}
				else
				{
					long? num2 = this.mValue;
					long minValue = this.MinValue;
					if(num2.GetValueOrDefault() < minValue && num2.HasValue)
					{
						value = new long?(this.MinValue);
					}
				}
				long? num3 = value;
				long? value2 = this.Value;
				if(num3.GetValueOrDefault() != value2.GetValueOrDefault() || num3.HasValue != value2.HasValue)
				{
					this.Value = value;
				}
			}
			base.OnLostFocus(e);
			if(!string.IsNullOrEmpty(base.MaskedText) && base.UseNullOption && base.IsNull)
			{
				base.MaskedText = string.Empty;
				this.checktext = "";
				this.minusPressed = false;
			}
		}
		protected override void OnContextMenuOpening(ContextMenuEventArgs e)
		{
			if(base.IsReadOnly && base.SelectionLength <= 0)
			{
				e.Handled = true;
				base.OnContextMenuOpening(e);
				return;
			}
			base.OnContextMenuOpening(e);
		}
		protected override void OnGotFocus(RoutedEventArgs e)
		{
			if(base.EnableFocusColors && this.PART_ContentHost != null)
			{
				this.PART_ContentHost.Background = base.FocusedBackground;
			}
			base.OnGotFocus(e);
		}
		public override void OnUseNullOptionChanged(DependencyPropertyChangedEventArgs args)
		{
			if(!(bool)args.NewValue && base.IsNull)
			{
				base.InvalidateProperty(IntegerTextBox.ValueProperty);
				base.IsNull = false;
			}
		}
		internal void FormatText()
		{
			if(this.Value.HasValue)
			{
				NumberFormatInfo numberFormat = this.GetCulture().NumberFormat;
				base.MaskedText = this.mValue.Value.ToString("N", numberFormat);
				return;
			}
			base.MaskedText = "";
		}
		internal void SetValue(bool? IsReload, long? _Value)
		{
			bool? flag = IsReload;
			if(!flag.GetValueOrDefault() && flag.HasValue)
			{
				this.mValueChanged = new bool?(false);
				this.Value = _Value;
				this.mValueChanged = new bool?(true);
				return;
			}
			bool? flag2 = IsReload;
			if(flag2.GetValueOrDefault() && flag2.HasValue)
			{
				int caretIndex = base.CaretIndex;
				this.Value = _Value;
				base.CaretIndex = caretIndex;
			}
		}
		internal long? ValidateValue(long? Val)
		{
			if(Val.HasValue)
			{
				long? num = Val;
				long maxValue = this.MaxValue;
				if(num.GetValueOrDefault() > maxValue && num.HasValue)
				{
					Val = new long?(this.MaxValue);
				}
				else
				{
					long? num2 = this.mValue;
					long minValue = this.MinValue;
					if(num2.GetValueOrDefault() < minValue && num2.HasValue)
					{
						Val = new long?(this.MinValue);
					}
				}
			}
			return Val;
		}
		internal CultureInfo GetCulture()
		{
			CultureInfo cultureInfo;
			if(base.Culture != null && base.Culture != CultureInfo.InvariantCulture)
			{
				cultureInfo = (base.Culture.Clone() as CultureInfo);
			}
			else
			{
				cultureInfo = (CultureInfo.CurrentCulture.Clone() as CultureInfo);
			}
			if(base.NumberFormat != null)
			{
				cultureInfo.NumberFormat = base.NumberFormat;
			}
			cultureInfo.NumberFormat.NumberDecimalDigits = 0;
			if(!this.GroupSeperatorEnabled)
			{
				cultureInfo.NumberFormat.NumberGroupSeparator = string.Empty;
			}
			if(this.GroupSeperatorEnabled && !this.NumberGroupSeparator.Equals(string.Empty))
			{
				cultureInfo.NumberFormat.NumberGroupSeparator = this.NumberGroupSeparator;
			}
			int num = this.NumberGroupSizes.Count;
			if(num > 0)
			{
				int[] array = new int[num];
				for(int i = 0; i < num; i++)
				{
					array[i] = this.NumberGroupSizes[i];
				}
				cultureInfo.NumberFormat.NumberGroupSizes = array;
			}
			return cultureInfo;
		}
		private static object CoerceValue(DependencyObject d, object baseValue)
		{
			IntegerTextBox integerTextBox = (IntegerTextBox)d;
			if(baseValue != null)
			{
				long? num = (long?)baseValue;
				bool? flag = integerTextBox.mValueChanged;
				if(flag.GetValueOrDefault() && flag.HasValue)
				{
					long? num2 = num;
					long maxValue = integerTextBox.MaxValue;
					if(num2.GetValueOrDefault() > maxValue && num2.HasValue && !integerTextBox.IsFocused && integerTextBox.MaxValidation != MaxValidation.OnLostFocus)
					{
						num = new long?(integerTextBox.MaxValue);
					}
					long? num3 = num;
					long minValue = integerTextBox.MinValue;
					if(num3.GetValueOrDefault() < minValue && num3.HasValue && !integerTextBox.IsFocused && integerTextBox.MinValidation != MinValidation.OnLostFocus)
					{
						num = new long?(integerTextBox.MinValue);
					}
				}
				if(num.HasValue)
				{
					EditorBase arg_DB_0 = integerTextBox;
					long? num4 = num;
					arg_DB_0.IsNegative = (num4.GetValueOrDefault() < 0L && num4.HasValue);
					EditorBase arg_FF_0 = integerTextBox;
					long? num5 = num;
					arg_FF_0.IsZero = (num5.GetValueOrDefault() == 0L && num5.HasValue);
					integerTextBox.IsNull = false;
				}
				return num;
			}
			if(integerTextBox.UseNullOption)
			{
				integerTextBox.IsNull = true;
				integerTextBox.IsNegative = false;
				integerTextBox.IsZero = false;
				return integerTextBox.NullValue;
			}
			long num6 = 0L;
			bool? flag2 = integerTextBox.mValueChanged;
			if(flag2.GetValueOrDefault() && flag2.HasValue)
			{
				if(num6 > integerTextBox.MaxValue)
				{
					num6 = integerTextBox.MaxValue;
				}
				if(num6 < integerTextBox.MinValue)
				{
					num6 = integerTextBox.MinValue;
				}
			}
			integerTextBox.IsNegative = (num6 < 0L);
			integerTextBox.IsZero = (num6 == 0L);
			integerTextBox.IsNull = false;
			return num6;
		}
		private static object CoerceMinValue(DependencyObject d, object baseValue)
		{
			IntegerTextBox integerTextBox = (IntegerTextBox)d;
			if(integerTextBox.MinValue > integerTextBox.MaxValue)
			{
				baseValue = integerTextBox.MaxValue;
			}
			return baseValue;
		}
		private static object CoerceMaxValue(DependencyObject d, object baseValue)
		{
			IntegerTextBox integerTextBox = (IntegerTextBox)d;
			if(integerTextBox.MinValue > integerTextBox.MaxValue)
			{
				return integerTextBox.MinValue;
			}
			return baseValue;
		}
		private void IntegerTextbox_Loaded(object sender, RoutedEventArgs e)
		{
			this.mIsLoaded = true;
			object obj = IntegerTextBox.CoerceValue(this, this.Value);
			long? num = (long?)obj;
			if(base.IsNull)
			{
				base.WatermarkVisibility = Visibility.Visible;
			}
			long? num2 = num;
			long? value = this.Value;
			if(num2.GetValueOrDefault() != value.GetValueOrDefault() || num2.HasValue != value.HasValue)
			{
				this.Value = num;
				return;
			}
			this.mValue = this.Value;
			this.FormatText();
		}
		public static void OnNumberGroupSizesChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			IntegerTextBox integerTextBox = (IntegerTextBox)obj;
			if(integerTextBox != null && integerTextBox != null)
			{
				integerTextBox.OnNumberGroupSizesChanged(args);
			}
		}
		protected void OnNumberGroupSizesChanged(DependencyPropertyChangedEventArgs args)
		{
			if(this.NumberGroupSizesChanged != null)
			{
				this.NumberGroupSizesChanged(this, args);
			}
			if(this.mIsLoaded)
			{
				this.FormatText();
			}
		}
		public static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			if((IntegerTextBox)obj != null)
			{
				((IntegerTextBox)obj).OnValueChanged(args);
			}
		}
		protected void OnValueChanged(DependencyPropertyChangedEventArgs args)
		{
			if(this.Value.HasValue)
			{
				long? value = this.Value;
				base.IsNegative = (value.GetValueOrDefault() < 0L && value.HasValue);
				long? value2 = this.Value;
				base.IsZero = (value2.GetValueOrDefault() == 0L && value2.HasValue);
				base.IsNull = false;
			}
			else
			{
				if(base.UseNullOption)
				{
					base.IsNull = true;
					base.IsNegative = false;
					base.IsZero = false;
				}
			}
			this.OldValue = (long?)args.OldValue;
			this.mValue = this.Value;
			if(this.Value.HasValue)
			{
				base.WatermarkVisibility = Visibility.Collapsed;
			}
			if(this.ValueChanged != null)
			{
				this.ValueChanged(this, args);
			}
			long? value3 = this.Value;
			long minValue = this.MinValue;
			if(value3.GetValueOrDefault() > minValue && value3.HasValue && base.MinValidation == MinValidation.OnKeyPress)
			{
				this.checktext = "";
			}
			if(this.mIsLoaded)
			{
				bool? flag = this.mValueChanged;
				if(flag.GetValueOrDefault() && flag.HasValue)
				{
					this.FormatText();
				}
			}
		}
		public static void OnMinValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			if((IntegerTextBox)obj != null)
			{
				((IntegerTextBox)obj).OnMinValueChanged(args);
			}
		}
		protected void OnMinValueChanged(DependencyPropertyChangedEventArgs args)
		{
			if(this.MinValueChanged != null)
			{
				this.MinValueChanged(this, args);
			}
			if(this.MaxValue < this.MinValue)
			{
				this.MaxValue = this.MinValue;
			}
			if(base.UseNullOption && this.Value.HasValue && this.MinValue != 0L)
			{
				long? value = this.Value;
				long minValue = this.MinValue;
				if(value.GetValueOrDefault() < minValue && value.HasValue)
				{
					this.Value = new long?(this.MinValue);
				}
			}
			long? value2 = this.Value;
			long? num = this.ValidateValue(this.Value);
			if(value2.GetValueOrDefault() != num.GetValueOrDefault() || value2.HasValue != num.HasValue)
			{
				this.Value = this.ValidateValue(this.Value);
			}
			if(this.Value.HasValue)
			{
				long? value3 = this.Value;
				double num2 = base.ActualWidth / (double)this.MaxValue;
				this.percentage = (value3.HasValue ? new double?((double)value3.GetValueOrDefault() * num2) : null).Value;
			}
		}
		public static void OnMaxValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			if((IntegerTextBox)obj != null)
			{
				((IntegerTextBox)obj).OnMaxValueChanged(args);
			}
		}
		protected void OnMaxValueChanged(DependencyPropertyChangedEventArgs args)
		{
			if(this.MaxValueChanged != null)
			{
				this.MaxValueChanged(this, args);
			}
			if(this.MinValue > this.MaxValue)
			{
				this.MinValue = this.MaxValue;
			}
			long? value = this.Value;
			long? num = this.ValidateValue(this.Value);
			if(value.GetValueOrDefault() != num.GetValueOrDefault() || value.HasValue != num.HasValue)
			{
				this.Value = this.ValidateValue(this.Value);
			}
		}
		private static void OnNumberGroupSeparatorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			if((IntegerTextBox)obj != null)
			{
				((IntegerTextBox)obj).OnNumberGroupSeparatorChanged(args);
			}
		}
		private static object CoerceNumberGroupSeperator(DependencyObject d, object baseValue)
		{
			IntegerTextBox integerTextBox = (IntegerTextBox)d;
			NumberFormatInfo numberFormat = integerTextBox.GetCulture().NumberFormat;
			if(!baseValue.Equals(string.Empty) && !char.IsLetterOrDigit(baseValue.ToString(), 0))
			{
				return baseValue;
			}
			if(baseValue.Equals(string.Empty))
			{
				return baseValue;
			}
			return numberFormat.NumberGroupSeparator;
		}
		protected virtual void OnNumberGroupSeparatorChanged(DependencyPropertyChangedEventArgs e)
		{
			if(this.NumberGroupSeparatorChanged != null)
			{
				this.NumberGroupSeparatorChanged(this, e);
			}
			if(this.mIsLoaded)
			{
				this.FormatText();
			}
		}
		public static void OnNullValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			if((IntegerTextBox)obj != null)
			{
				((IntegerTextBox)obj).OnNullValueChanged(args);
			}
		}
		protected void OnNullValueChanged(DependencyPropertyChangedEventArgs args)
		{
		}
		internal void OnValueValidationCompleted(StringValidationEventArgs e)
		{
			if(this.ValueValidationCompleted != null)
			{
				this.ValueValidationCompleted(this, e);
			}
		}
		internal void OnValidated(EventArgs e)
		{
			if(this.Validated != null)
			{
				this.Validated(this, e);
			}
		}
		internal bool OnValidating(CancelEventArgs e)
		{
			if(this.Validating != null)
			{
				this.Validating(this, e);
				this._validatingrResult = e.Cancel;
				return e.Cancel;
			}
			return false;
		}
		public static void OnInvalidValueBehaviorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			IntegerTextBox integerTextBox = (IntegerTextBox)obj;
			if(integerTextBox != null)
			{
				integerTextBox.OnInvalidValueBehaviorChanged(args);
			}
		}
		protected void OnInvalidValueBehaviorChanged(DependencyPropertyChangedEventArgs args)
		{
			if(this.InvalidValueBehaviorChanged != null)
			{
				this.InvalidValueBehaviorChanged(this, args);
			}
		}
		public static void OnValidationValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			IntegerTextBox integerTextBox = (IntegerTextBox)obj;
			if(integerTextBox != null)
			{
				integerTextBox.OnValidationValueChanged(args);
			}
		}
		protected void OnValidationValueChanged(DependencyPropertyChangedEventArgs args)
		{
			if(this.ValidationValueChanged != null)
			{
				this.ValidationValueChanged(this, args);
			}
		}
		public static void OnValidationCompletedPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			IntegerTextBox integerTextBox = (IntegerTextBox)obj;
			if(integerTextBox != null)
			{
				integerTextBox.OnValidationCompletedPropertyChanged(args);
			}
		}
		protected void OnValidationCompletedPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			if(this.ValidationCompletedChanged != null)
			{
				this.ValidationCompletedChanged(this, args);
			}
		}
	}
}