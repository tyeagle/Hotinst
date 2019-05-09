/**
 * ==============================================================================
 *
 * ClassName: DoubleTextBox
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/3/23 14:21:36
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using HOTINST.COMMON.Controls.Core;
using HOTINST.COMMON.Controls.Core.Editors;
using HOTINST.COMMON.Controls.Extension;
using HOTINST.COMMON.Controls.VisualUtil;

#pragma warning disable 1591

namespace HOTINST.COMMON.Controls.Controls.Editors
{
	public class StringValidationEventArgs : EventArgs
	{
		private bool m_bCancel;
		private bool m_bIsValidInput = true;
		private string m_message = string.Empty;
		private string m_validationString = string.Empty;
		public bool Cancel
		{
			get
			{
				return this.m_bCancel;
			}
			set
			{
				if(this.m_bCancel != value)
				{
					this.m_bCancel = value;
				}
			}
		}
		public bool IsValidInput
		{
			get
			{
				return this.m_bIsValidInput;
			}
		}
		public string Message
		{
			get
			{
				return this.m_message;
			}
		}
		public string ValidationString
		{
			get
			{
				return this.m_validationString;
			}
		}
		public StringValidationEventArgs(bool bCancel, bool bIsValidInput, string message, string validationString)
		{
			this.m_bCancel = bCancel;
			this.m_bIsValidInput = bIsValidInput;
			this.m_message = message;
			this.m_validationString = validationString;
		}
		public StringValidationEventArgs(bool bIsValidInput, string message, string validationString) : this(false, bIsValidInput, message, validationString)
		{
		}
		public StringValidationEventArgs()
		{
		}
	}

	public class ValueChangingEventArgs : CancelEventArgs
	{
		public object NewValue { get; set; }
		public object OldValue { get; set; }
	}

	public delegate void StringValidationCompletedEventHandler(object sender, StringValidationEventArgs e);

	public class DoubleTextBox : EditorBase, IDisposable
	{
		public delegate void ValueChangingEventHandler(object sender, ValueChangingEventArgs e);
		internal double? OldValue;
		internal double? mValue;
		internal string utext;
		internal int uval;
		internal bool? mValueChanged = new bool?(true);
		internal bool mIsLoaded;
		internal int count = 1;
		internal bool negativeFlag;
		internal string checktext = "";
		internal int numberDecimalDigits;
		internal string lostfocusmasktext;
		internal bool isUpDownDoubleTextBox;
		private ScrollViewer PART_ContentHost;
		private bool _validatingrResult;
		public static readonly DependencyProperty NumberGroupSizesProperty;
		public static readonly DependencyProperty ValueProperty;
		public static readonly DependencyProperty MinimumNumberDecimalDigitsProperty;
		public static readonly DependencyProperty MaximumNumberDecimalDigitsProperty;
		public static readonly DependencyProperty MinValueProperty;
		public static readonly DependencyProperty MaxValueProperty;
		public static readonly DependencyProperty NumberGroupSeparatorProperty;
		public static readonly DependencyProperty NumberDecimalDigitsProperty;
		public static readonly DependencyProperty NumberDecimalSeparatorProperty;
		public static readonly DependencyProperty GroupSeperatorEnabledProperty;
		internal static readonly DependencyProperty IsExceedDecimalDigitsProperty;
		public static readonly DependencyProperty ScrollIntervalProperty;
		public static readonly DependencyProperty StepProperty;
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
		public event DoubleTextBox.ValueChangingEventHandler ValueChanging;
		public event PropertyChangedCallback MaxValueChanged;
		public event PropertyChangedCallback NumberDecimalDigitsChanged;
		public event PropertyChangedCallback NumberDecimalSeparatorChanged;
		public event PropertyChangedCallback NumberGroupSizesChanged;
		public event PropertyChangedCallback NumberGroupSeparatorChanged;
		internal event PropertyChangedCallback IsExceedDecimalDigitsChanged;
		public event PropertyChangedCallback MinimumNumberDecimalDigitsChanged;
		public event PropertyChangedCallback MaximumNumberDecimalDigitsChanged;
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
		public double? Value
		{
			get
			{
				return (double?)base.GetValue(DoubleTextBox.ValueProperty);
			}
			set
			{
				base.SetValue(DoubleTextBox.ValueProperty, value);
			}
		}
		public double MinValue
		{
			get
			{
				return (double)base.GetValue(DoubleTextBox.MinValueProperty);
			}
			set
			{
				base.SetValue(DoubleTextBox.MinValueProperty, value);
			}
		}
		public double MaxValue
		{
			get
			{
				return (double)base.GetValue(DoubleTextBox.MaxValueProperty);
			}
			set
			{
				base.SetValue(DoubleTextBox.MaxValueProperty, value);
			}
		}
		public string NumberGroupSeparator
		{
			get
			{
				return (string)base.GetValue(DoubleTextBox.NumberGroupSeparatorProperty);
			}
			set
			{
				base.SetValue(DoubleTextBox.NumberGroupSeparatorProperty, value);
			}
		}
		public Int32Collection NumberGroupSizes
		{
			get
			{
				return (Int32Collection)base.GetValue(DoubleTextBox.NumberGroupSizesProperty);
			}
			set
			{
				base.SetValue(DoubleTextBox.NumberGroupSizesProperty, value);
			}
		}
		public int MinimumNumberDecimalDigits
		{
			get
			{
				return (int)base.GetValue(DoubleTextBox.MinimumNumberDecimalDigitsProperty);
			}
			set
			{
				base.SetValue(DoubleTextBox.MinimumNumberDecimalDigitsProperty, value);
			}
		}
		public int MaximumNumberDecimalDigits
		{
			get
			{
				return (int)base.GetValue(DoubleTextBox.MaximumNumberDecimalDigitsProperty);
			}
			set
			{
				base.SetValue(DoubleTextBox.MaximumNumberDecimalDigitsProperty, value);
			}
		}
		public int NumberDecimalDigits
		{
			get
			{
				return (int)base.GetValue(DoubleTextBox.NumberDecimalDigitsProperty);
			}
			set
			{
				base.SetValue(DoubleTextBox.NumberDecimalDigitsProperty, value);
			}
		}
		public string NumberDecimalSeparator
		{
			get
			{
				return (string)base.GetValue(DoubleTextBox.NumberDecimalSeparatorProperty);
			}
			set
			{
				base.SetValue(DoubleTextBox.NumberDecimalSeparatorProperty, value);
			}
		}
		public bool GroupSeperatorEnabled
		{
			get
			{
				return (bool)base.GetValue(DoubleTextBox.GroupSeperatorEnabledProperty);
			}
			set
			{
				base.SetValue(DoubleTextBox.GroupSeperatorEnabledProperty, value);
			}
		}
		internal bool IsExceedDecimalDigits
		{
			get
			{
				return (bool)base.GetValue(DoubleTextBox.IsExceedDecimalDigitsProperty);
			}
			set
			{
				base.SetValue(DoubleTextBox.IsExceedDecimalDigitsProperty, value);
			}
		}
		public double ScrollInterval
		{
			get
			{
				return (double)base.GetValue(DoubleTextBox.ScrollIntervalProperty);
			}
			set
			{
				base.SetValue(DoubleTextBox.ScrollIntervalProperty, value);
			}
		}
		public double Step
		{
			get
			{
				return (double)base.GetValue(DoubleTextBox.StepProperty);
			}
			set
			{
				base.SetValue(DoubleTextBox.StepProperty, value);
			}
		}
		public double? NullValue
		{
			get
			{
				return (double?)base.GetValue(DoubleTextBox.NullValueProperty);
			}
			set
			{
				base.SetValue(DoubleTextBox.NullValueProperty, value);
			}
		}
		public StringValidation ValueValidation
		{
			get
			{
				return (StringValidation)base.GetValue(DoubleTextBox.ValueValidationProperty);
			}
			set
			{
				base.SetValue(DoubleTextBox.ValueValidationProperty, value);
			}
		}
		public InvalidInputBehavior InvalidValueBehavior
		{
			get
			{
				return (InvalidInputBehavior)base.GetValue(DoubleTextBox.InvalidValueBehaviorProperty);
			}
			set
			{
				base.SetValue(DoubleTextBox.InvalidValueBehaviorProperty, value);
			}
		}
		public string ValidationValue
		{
			get
			{
				return (string)base.GetValue(DoubleTextBox.ValidationValueProperty);
			}
			set
			{
				base.SetValue(DoubleTextBox.ValidationValueProperty, value);
			}
		}
		public bool ValidationCompleted
		{
			get
			{
				return (bool)base.GetValue(DoubleTextBox.ValidationCompletedProperty);
			}
			set
			{
				base.SetValue(DoubleTextBox.ValidationCompletedProperty, value);
			}
		}
		static DoubleTextBox()
		{
			DoubleTextBox.NumberGroupSizesProperty = DependencyProperty.Register("NumberGroupSizes", typeof(Int32Collection), typeof(DoubleTextBox), new PropertyMetadata(new Int32Collection(), new PropertyChangedCallback(DoubleTextBox.OnNumberGroupSizesChanged)));
			DoubleTextBox.ValueProperty = DependencyProperty.Register("Value", typeof(double?), typeof(DoubleTextBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(DoubleTextBox.OnValueChanged), new CoerceValueCallback(DoubleTextBox.CoerceValue), false, UpdateSourceTrigger.LostFocus));
			DoubleTextBox.MinimumNumberDecimalDigitsProperty = DependencyProperty.Register("MinimumNumberDecimalDigits", typeof(int), typeof(DoubleTextBox), new PropertyMetadata(-1, new PropertyChangedCallback(DoubleTextBox.OnMinimumNumberDecimalDigitsChanged)));
			DoubleTextBox.MaximumNumberDecimalDigitsProperty = DependencyProperty.Register("MaximumNumberDecimalDigits", typeof(int), typeof(DoubleTextBox), new PropertyMetadata(-1, new PropertyChangedCallback(DoubleTextBox.OnMaximumNumberDecimalDigitsChanged)));
			DoubleTextBox.MinValueProperty = DependencyProperty.Register("MinValue", typeof(double), typeof(DoubleTextBox), new PropertyMetadata(-1.7976931348623157E+308, new PropertyChangedCallback(DoubleTextBox.OnMinValueChanged)));
			DoubleTextBox.MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(double), typeof(DoubleTextBox), new PropertyMetadata(1.7976931348623157E+308, new PropertyChangedCallback(DoubleTextBox.OnMaxValueChanged)));
			DoubleTextBox.NumberGroupSeparatorProperty = DependencyProperty.Register("NumberGroupSeparator", typeof(string), typeof(DoubleTextBox), new PropertyMetadata(string.Empty, new PropertyChangedCallback(DoubleTextBox.OnNumberGroupSeparatorChanged), new CoerceValueCallback(DoubleTextBox.CoerceNumberGroupSeperator)));
			DoubleTextBox.NumberDecimalDigitsProperty = DependencyProperty.Register("NumberDecimalDigits", typeof(int), typeof(DoubleTextBox), new PropertyMetadata(-1, new PropertyChangedCallback(DoubleTextBox.OnNumberDecimalDigitsChanged)));
			DoubleTextBox.NumberDecimalSeparatorProperty = DependencyProperty.Register("NumberDecimalSeparator", typeof(string), typeof(DoubleTextBox), new PropertyMetadata(string.Empty, new PropertyChangedCallback(DoubleTextBox.OnNumberDecimalSeparatorChanged)));
			DoubleTextBox.GroupSeperatorEnabledProperty = DependencyProperty.Register("GroupSeperatorEnabled", typeof(bool), typeof(DoubleTextBox), new PropertyMetadata(true, new PropertyChangedCallback(DoubleTextBox.OnNumberGroupSeparatorChanged)));
			DoubleTextBox.IsExceedDecimalDigitsProperty = DependencyProperty.Register("IsExceedDecimalDigits", typeof(bool), typeof(DoubleTextBox), new PropertyMetadata(false, new PropertyChangedCallback(DoubleTextBox.OnIsExceedDecimalDigits)));
			DoubleTextBox.ScrollIntervalProperty = DependencyProperty.Register("ScrollInterval", typeof(double), typeof(DoubleTextBox), new PropertyMetadata(1.0));
			DoubleTextBox.StepProperty = DependencyProperty.Register("Step", typeof(double), typeof(DoubleTextBox), new PropertyMetadata(1.0));
			DoubleTextBox.NullValueProperty = DependencyProperty.Register("NullValue", typeof(double?), typeof(DoubleTextBox), new PropertyMetadata(null, new PropertyChangedCallback(DoubleTextBox.OnNullValueChanged)));
			DoubleTextBox.ValueValidationProperty = DependencyProperty.Register("ValueValidation", typeof(StringValidation), typeof(DoubleTextBox), new PropertyMetadata(StringValidation.OnLostFocus));
			DoubleTextBox.InvalidValueBehaviorProperty = DependencyProperty.Register("InvalidValueBehavior", typeof(InvalidInputBehavior), typeof(DoubleTextBox), new PropertyMetadata(InvalidInputBehavior.None, new PropertyChangedCallback(DoubleTextBox.OnInvalidValueBehaviorChanged)));
			DoubleTextBox.ValidationValueProperty = DependencyProperty.Register("ValidationValue", typeof(string), typeof(DoubleTextBox), new PropertyMetadata(string.Empty, new PropertyChangedCallback(DoubleTextBox.OnValidationValueChanged)));
			DoubleTextBox.ValidationCompletedProperty = DependencyProperty.Register("ValidationCompleted", typeof(bool), typeof(DoubleTextBox), new PropertyMetadata(false, new PropertyChangedCallback(DoubleTextBox.OnValidationCompletedPropertyChanged)));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DoubleTextBox), new FrameworkPropertyMetadata(typeof(DoubleTextBox)));
		}
		public DoubleTextBox()
		{
			this.pastecommand = new DelegateCommand(new Action<object>(this._pastecommand), new Predicate<object>(this.Canpaste));
			this.copycommand = new DelegateCommand(new Action<object>(this._copycommand), new Predicate<object>(this.Canpaste));
			this.cutcommand = new DelegateCommand(new Action<object>(this._cutcommand), new Predicate<object>(this.Canpaste));
			base.RemoveHandler(CommandManager.PreviewExecutedEvent, new ExecutedRoutedEventHandler(this.CommandExecuted));
			base.AddHandler(CommandManager.PreviewExecutedEvent, new ExecutedRoutedEventHandler(this.CommandExecuted), true);
			base.Loaded -= new RoutedEventHandler(this.DoubleTextbox_Loaded);
			base.Loaded += new RoutedEventHandler(this.DoubleTextbox_Loaded);
			base.TextChanged -= new TextChangedEventHandler(this.DoubleTextBox_TextChanged);
			base.TextChanged += new TextChangedEventHandler(this.DoubleTextBox_TextChanged);
			base.Unloaded += new RoutedEventHandler(this.DoubleTextBox_Unloaded);
		}
		private void DoubleTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if(InputLanguageManager.Current.CurrentInputLanguage.DisplayName.Contains("Chinese"))
			{
				if(this.allowchange && base.Culture.NumberFormat.NumberGroupSeparator == this.NumberGroupSeparator && base.Culture.NumberFormat.NumberDecimalSeparator == this.NumberDecimalSeparator)
				{
					if(base.MaskedText == "-")
					{
						double? value = this.Value;
						if(value.GetValueOrDefault() == 0.0 && value.HasValue)
						{
							return;
						}
					}
					double? num = null;
					num = base.Text.ConvertToDoubleNull(base.Culture, base.NumberFormat);
					double value2;
					if(!num.HasValue && double.TryParse(base.Text, NumberStyles.Any, this.GetCulture().NumberFormat, out value2))
					{
						num = new double?(value2);
					}
					double? num2 = num;
					double? value3 = this.Value;
					if(num2.GetValueOrDefault() != value3.GetValueOrDefault() || num2.HasValue != value3.HasValue)
					{
						base.SetValue(DoubleTextBox.ValueProperty, num);
						return;
					}
				}
			}
			else
			{
				bool flag = true;
				NumberFormatInfo numberFormat = this.GetCulture().NumberFormat;
				if(!this.isUpDownDoubleTextBox)
				{
					flag = (base.Culture.NumberFormat.NumberDecimalSeparator == numberFormat.NumberDecimalSeparator && base.Culture.NumberFormat.NumberGroupSeparator == numberFormat.NumberGroupSeparator);
				}
				else
				{
					if(this.isUpDownDoubleTextBox)
					{
						double? value4 = this.Value;
						if(value4.GetValueOrDefault() == 1.7976931348623157E+308 && value4.HasValue)
						{
							flag = (base.Culture.NumberFormat.NumberDecimalSeparator == numberFormat.NumberDecimalSeparator && base.Culture.NumberFormat.NumberGroupSeparator == numberFormat.NumberGroupSeparator);
						}
					}
				}
				if(flag)
				{
					if(base.MaskedText == "-")
					{
						double? value5 = this.Value;
						if(value5.GetValueOrDefault() == 0.0 && value5.HasValue)
						{
							return;
						}
					}
					double? num3 = null;
					num3 = base.Text.ConvertToDoubleNull(base.Culture, base.NumberFormat);
					double value6;
					if(!num3.HasValue && double.TryParse(base.Text, NumberStyles.Any, this.GetCulture().NumberFormat, out value6))
					{
						num3 = new double?(value6);
					}
					double? num4 = num3;
					double? value7 = this.Value;
					if(num4.GetValueOrDefault() != value7.GetValueOrDefault() || num4.HasValue != value7.HasValue)
					{
						base.SetValue(DoubleTextBox.ValueProperty, num3);
					}
				}
			}
		}
		private void DoubleTextBox_Unloaded(object sender, RoutedEventArgs e)
		{
			base.TextChanged -= new TextChangedEventHandler(this.DoubleTextBox_TextChanged);
			base.Unloaded -= new RoutedEventHandler(this.DoubleTextBox_Unloaded);
		}
		public void Dispose()
		{
			base.Unloaded -= new RoutedEventHandler(this.DoubleTextBox_Unloaded);
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
			if(this.IsExceedDecimalDigits && this.NumberDecimalDigits < 0)
			{
				throw new InvalidOperationException("NumberDecimalDigits must be a postive value.current value -1");
			}
			this.PART_ContentHost = (base.GetTemplateChild("PART_ContentHost") as ScrollViewer);
		}
		protected override void OnMouseWheel(MouseWheelEventArgs e)
		{
			base.OnMouseWheel(e);
			if(base.IsScrollingOnCircle)
			{
				if(e.Delta > 0)
				{
					DoubleValueHandler.DoubleValHandler.HandleUpKey(this);
					return;
				}
				if(e.Delta < 0)
				{
					DoubleValueHandler.DoubleValHandler.HandleDownKey(this);
				}
			}
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
		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			e.Handled = DoubleValueHandler.DoubleValHandler.HandleKeyDown(this, e);
			if(e.Key == Key.Z)
			{
				if(!base.IsUndoEnabled)
				{
					e.Handled = true;
				}
				else
				{
					if(this.uval <= 1 && base.MaskedText == this.utext)
					{
						e.Handled = true;
					}
					this.uval--;
				}
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
					else
					{
						if(!base.EnterToMoveNext)
						{
							Window window = VisualUtils.FindAncestor(this, typeof(Window)) as Window;
							if(window != null)
							{
								Button button = DoubleTextBox.FindChild(window, typeof(Button)) as Button;
								if(button != null)
								{
									e.Handled = false;
								}
								else
								{
									e.Handled = true;
								}
							}
							else
							{
								Window.GetWindow(this);
								Visual visual = VisualUtils.FindRootVisual(this);
								if(visual is Popup || visual.GetType().Name.Contains("Popup"))
								{
									Button button2 = DoubleTextBox.FindChild(visual, typeof(Button)) as Button;
									if(button2 != null)
									{
										e.Handled = false;
									}
									else
									{
										e.Handled = true;
									}
								}
							}
						}
						else
						{
							e.Handled = true;
						}
					}
				}
				else
				{
					e.Handled = DoubleValueHandler.DoubleValHandler.HandleKeyDown(this, e);
				}
			}
			base.OnKeyDown(e);
		}
		public static Visual FindChild(Visual startingFrom, Type typeDescendant)
		{
			Visual visual = null;
			int childrenCount = VisualTreeHelper.GetChildrenCount(startingFrom);
			for(int i = 0; i < childrenCount; i++)
			{
				Visual visual2 = VisualTreeHelper.GetChild(startingFrom, i) as Visual;
				if(typeDescendant.IsInstanceOfType(visual2))
				{
					Button button = visual2 as Button;
					if(button != null && button.IsDefault)
					{
						return button;
					}
				}
				if(visual2 != null)
				{
					visual = DoubleTextBox.FindChild(visual2, typeDescendant);
					if(visual != null)
					{
						break;
					}
				}
			}
			return visual;
		}
		private void cut()
		{
			try
			{
				if(base.SelectionLength > 0)
				{
					Clipboard.SetText(base.SelectedText);
					this.count = 1;
					DoubleValueHandler.DoubleValHandler.HandleDeleteKey(this);
				}
			}
			catch(COMException)
			{
			}
		}
		protected override void OnTextInput(TextCompositionEventArgs e)
		{
			if(InputLanguageManager.Current.CurrentInputLanguage.DisplayName.Contains("Chinese"))
			{
				Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
				{
					int caretIndex = this.CaretIndex;
					if(this.CaretIndex > 0)
					{
						this.MaskedText = this.MaskedText.Remove(this.CaretIndex - e.Text.Length, e.Text.Length);
					}
					if(caretIndex - e.Text.Length >= 0)
					{
						this.CaretIndex = caretIndex - e.Text.Length;
					}
					e.Handled = DoubleValueHandler.DoubleValHandler.MatchWithMask(this, e.Text);
					this.allowchange = false;
				}));
			}
			else
			{
				e.Handled = DoubleValueHandler.DoubleValHandler.MatchWithMask(this, e.Text);
			}
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
							base.SetCurrentValue(DoubleTextBox.ValueProperty, null);
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
				else
				{
					this.OnValidated(EventArgs.Empty);
				}
			}
			if(base.EnableFocusColors && this.PART_ContentHost != null)
			{
				this.PART_ContentHost.Background = base.Background;
			}
			double? value = this.Value;
			if(this.mIsLoaded)
			{
				if(this.ValidationValue == this.Value.ToString())
				{
					this.ValidationCompleted = true;
				}
				else
				{
					this.ValidationCompleted = false;
				}
			}
			if(value.HasValue)
			{
				double? num = value;
				double maxValue = this.MaxValue;
				if(num.GetValueOrDefault() > maxValue && num.HasValue)
				{
					value = new double?(this.MaxValue);
				}
				else
				{
					double? num2 = this.mValue;
					double minValue = this.MinValue;
					if(num2.GetValueOrDefault() < minValue && num2.HasValue)
					{
						value = new double?(this.MinValue);
					}
				}
				double? num3 = value;
				double? value2 = this.Value;
				if((num3.GetValueOrDefault() != value2.GetValueOrDefault() || num3.HasValue != value2.HasValue) && !this.TriggerValueChangingEvent(new ValueChangingEventArgs
				{
					OldValue = this.Value,
					NewValue = value
				}))
				{
					this.Value = value;
				}
			}
			if(base.MaskedText.Length >= 15 && base.MaxValidation == MaxValidation.OnLostFocus)
			{
				this.lostfocusmasktext = base.MaskedText;
				NumberFormatInfo numberFormat = this.GetCulture().NumberFormat;
				double value3 = double.Parse(base.MaskedText);
				base.MaskedText = value3.ToString("N", numberFormat);
				this.Value = new double?(value3);
			}
			if(base.Text == "-")
			{
				double? value4 = this.Value;
				if(value4.GetValueOrDefault() == 0.0 && value4.HasValue)
				{
					this.minusPressed = false;
					this.SetValue(new bool?(true), new double?(0.0));
				}
			}
			base.OnLostFocus(e);
			this.checktext = "";
		}
		protected override void OnGotFocus(RoutedEventArgs e)
		{
			if(base.EnableFocusColors && this.PART_ContentHost != null)
			{
				this.PART_ContentHost.Background = base.FocusedBackground;
			}
			try
			{
				if(base.MaskedText.Length >= 15 && base.MaxValidation == MaxValidation.OnLostFocus)
				{
					base.MaskedText = this.lostfocusmasktext;
				}
			}
			catch
			{
			}
			base.OnGotFocus(e);
		}
		internal void FormatText()
		{
			if(this.Value.HasValue && !double.IsNaN(this.Value.Value))
			{
				double? num = this.mValue;
				if(num.GetValueOrDefault() != 0.0 || !num.HasValue || this.Value.HasValue)
				{
					NumberFormatInfo numberFormat = this.GetCulture().NumberFormat;
					if(base.UseNullOption)
					{
						double? value = this.Value;
						if(value.GetValueOrDefault() == 0.0 && value.HasValue && base.MaskedText == "-")
						{
							return;
						}
					}
					base.MaskedText = this.mValue.Value.ToString("N", numberFormat);
					return;
				}
				if(base.UseNullOption)
				{
					this.SetValue(new bool?(true), null);
					return;
				}
				this.SetValue(new bool?(true), new double?(0.0));
				NumberFormatInfo numberFormat2 = this.GetCulture().NumberFormat;
				base.MaskedText = this.mValue.Value.ToString("N", numberFormat2);
				return;
			}
			else
			{
				base.MaskedText = "";
			}
		}
		internal bool SetValue(bool? IsReload, double? _Value)
		{
			NumberFormatInfo numberFormat = this.GetCulture().NumberFormat;
			if(base.IsUndoEnabled)
			{
				this.uval++;
			}
			bool? flag = IsReload;
			if(!flag.GetValueOrDefault() && flag.HasValue)
			{
				this.mValueChanged = new bool?(false);
				if(!this.TriggerValueChangingEvent(new ValueChangingEventArgs
				{
					OldValue = this.Value,
					NewValue = _Value
				}))
				{
					this.Value = _Value;
					if(this.Value.HasValue)
					{
						double? value = this.Value;
						if((value.GetValueOrDefault() != 0.0 || !value.HasValue) && !base.MaskedText.Contains("-"))
						{
							base.MaskedText = _Value.Value.ToString("N", numberFormat);
						}
					}
					else
					{
						if(!this.Value.HasValue && base.UseNullOption)
						{
							base.MaskedText = "";
						}
					}
					this.mValueChanged = new bool?(true);
					return true;
				}
				if(this.Value.HasValue)
				{
					base.MaskedText = this.Value.Value.ToString("N", numberFormat);
				}
				return true;
			}
			else
			{
				bool? flag2 = IsReload;
				if(!flag2.GetValueOrDefault() || !flag2.HasValue)
				{
					return false;
				}
				if(!this.TriggerValueChangingEvent(new ValueChangingEventArgs
				{
					OldValue = this.Value,
					NewValue = _Value
				}))
				{
					int caretIndex = base.CaretIndex;
					this.Value = _Value;
					base.CaretIndex = caretIndex;
					return true;
				}
				if(this.Value.HasValue)
				{
					base.MaskedText = this.Value.Value.ToString("N", numberFormat);
				}
				return true;
			}
		}
		internal bool TriggerValueChangingEvent(ValueChangingEventArgs args)
		{
			if(this.ValueChanging != null)
			{
				this.ValueChanging(this, args);
				return args.Cancel;
			}
			return false;
		}
		internal double? ValidateValue(double? Val)
		{
			if(Val.HasValue)
			{
				double? num = Val;
				double maxValue = this.MaxValue;
				if(num.GetValueOrDefault() > maxValue && num.HasValue)
				{
					Val = new double?(this.MaxValue);
				}
				else
				{
					double? num2 = this.mValue;
					double minValue = this.MinValue;
					if(num2.GetValueOrDefault() < minValue && num2.HasValue)
					{
						Val = new double?(this.MinValue);
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
			if(!this.GroupSeperatorEnabled)
			{
				cultureInfo.NumberFormat.NumberGroupSeparator = string.Empty;
			}
			if(this.GroupSeperatorEnabled && !this.NumberGroupSeparator.Equals(string.Empty) && cultureInfo.NumberFormat.NumberGroupSeparator != this.NumberGroupSeparator)
			{
				cultureInfo.NumberFormat.NumberGroupSeparator = this.NumberGroupSeparator;
			}
			if(this.NumberDecimalDigits >= 0 && cultureInfo.NumberFormat.NumberDecimalDigits != this.NumberDecimalDigits)
			{
				cultureInfo.NumberFormat.NumberDecimalDigits = this.NumberDecimalDigits;
			}
			if(!this.NumberDecimalSeparator.Equals(string.Empty) && cultureInfo.NumberFormat.NumberDecimalSeparator != this.NumberDecimalSeparator)
			{
				cultureInfo.NumberFormat.NumberDecimalSeparator = this.NumberDecimalSeparator;
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
			DoubleTextBox doubleTextBox = (DoubleTextBox)d;
			if(baseValue != null)
			{
				double? num = (double?)baseValue;
				bool? flag = doubleTextBox.mValueChanged;
				if(flag.GetValueOrDefault() && flag.HasValue)
				{
					double? num2 = num;
					double maxValue = doubleTextBox.MaxValue;
					if(num2.GetValueOrDefault() > maxValue && num2.HasValue && doubleTextBox.MaxValidation == MaxValidation.OnKeyPress)
					{
						num = new double?(doubleTextBox.MaxValue);
					}
					else
					{
						double? num3 = num;
						double minValue = doubleTextBox.MinValue;
						if(num3.GetValueOrDefault() < minValue && num3.HasValue && doubleTextBox.MinValidation == MinValidation.OnKeyPress)
						{
							num = new double?(doubleTextBox.MinValue);
						}
					}
				}
				if(num.HasValue)
				{
					EditorBase arg_D3_0 = doubleTextBox;
					double? num4 = num;
					arg_D3_0.IsNegative = (num4.GetValueOrDefault() < 0.0 && num4.HasValue);
					EditorBase arg_FF_0 = doubleTextBox;
					double? num5 = num;
					arg_FF_0.IsZero = (num5.GetValueOrDefault() == 0.0 && num5.HasValue);
					doubleTextBox.IsNull = false;
				}
				if(doubleTextBox.NumberDecimalDigits > 0 || doubleTextBox.MaximumNumberDecimalDigits > 0 || doubleTextBox.numberDecimalDigits > 0)
				{
					NumberFormatInfo numberFormat = doubleTextBox.GetCulture().NumberFormat;
					if(baseValue.ToString().Contains(numberFormat.NumberDecimalSeparator))
					{
						int num6 = DoubleTextBox.CountDecimalDigits(baseValue.ToString(), d);
						int num7 = DoubleTextBox.CountDecimalDigits(doubleTextBox.MaskedText, d);
						if(!string.IsNullOrEmpty(doubleTextBox.MaskedText) && doubleTextBox.MaskedText != "DoubleTextBox")
						{
							DoubleValueHandler.DoubleValHandler.CanUpdate = true;
							if((num6 != 0 && baseValue.ToString()[baseValue.ToString().Length - 1] != '0' && doubleTextBox.MaskedText[doubleTextBox.MaskedText.Length - 1] != '0') || num6 > num7)
							{
								if(doubleTextBox.MaximumNumberDecimalDigits == -1 && doubleTextBox.MinimumNumberDecimalDigits == -1)
								{
									num6 = doubleTextBox.numberDecimalDigits;
								}
								else
								{
									if(doubleTextBox.NumberDecimalDigits > 0 && doubleTextBox.MaximumNumberDecimalDigits >= 0 && doubleTextBox.MinimumNumberDecimalDigits == -1)
									{
										if(num6 >= doubleTextBox.MaximumNumberDecimalDigits)
										{
											num6 = doubleTextBox.MaximumNumberDecimalDigits;
										}
										if(num6 <= doubleTextBox.numberDecimalDigits)
										{
											num6 = doubleTextBox.numberDecimalDigits;
										}
									}
									else
									{
										if(doubleTextBox.NumberDecimalDigits > 0 && doubleTextBox.MinimumNumberDecimalDigits >= 0 && doubleTextBox.MaximumNumberDecimalDigits == -1)
										{
											if(num6 >= doubleTextBox.numberDecimalDigits)
											{
												num6 = doubleTextBox.numberDecimalDigits;
											}
											if(num6 <= doubleTextBox.MinimumNumberDecimalDigits)
											{
												num6 = doubleTextBox.MinimumNumberDecimalDigits;
											}
										}
										else
										{
											if(doubleTextBox.MinimumNumberDecimalDigits >= 0 && doubleTextBox.MaximumNumberDecimalDigits >= 0 && doubleTextBox.NumberDecimalDigits > 0)
											{
												if(num6 >= doubleTextBox.MaximumNumberDecimalDigits)
												{
													num6 = doubleTextBox.MaximumNumberDecimalDigits;
												}
												if(num6 <= doubleTextBox.MinimumNumberDecimalDigits)
												{
													num6 = doubleTextBox.MinimumNumberDecimalDigits;
												}
											}
										}
									}
								}
								doubleTextBox.NumberDecimalDigits = num6;
							}
							if(num6 == 0 && doubleTextBox.MinimumNumberDecimalDigits >= 0)
							{
								doubleTextBox.NumberDecimalDigits = 0;
							}
							DoubleValueHandler.DoubleValHandler.CanUpdate = false;
						}
						else
						{
							if((doubleTextBox.MaskedText == "" || doubleTextBox.MaskedText == "DoubleTextBox") && baseValue != null && doubleTextBox.MaximumNumberDecimalDigits > doubleTextBox.NumberDecimalDigits)
							{
								DoubleValueHandler.DoubleValHandler.CanUpdate = true;
								int num8 = DoubleTextBox.CountDecimalDigits(baseValue.ToString(), d);
								if(num8 >= doubleTextBox.MaximumNumberDecimalDigits)
								{
									doubleTextBox.NumberDecimalDigits = doubleTextBox.MaximumNumberDecimalDigits;
								}
								else
								{
									if(num8 <= doubleTextBox.MinimumNumberDecimalDigits)
									{
										doubleTextBox.NumberDecimalDigits = doubleTextBox.MinimumNumberDecimalDigits;
									}
									else
									{
										if(doubleTextBox.NumberDecimalDigits != doubleTextBox.numberDecimalDigits)
										{
											doubleTextBox.NumberDecimalDigits = num8;
										}
									}
								}
								DoubleValueHandler.DoubleValHandler.CanUpdate = false;
							}
						}
					}
					else
					{
						if(doubleTextBox.MaskedText.Contains(numberFormat.NumberDecimalSeparator))
						{
							DoubleValueHandler.DoubleValHandler.CanUpdate = true;
							int num9 = DoubleTextBox.CountDecimalDigits(doubleTextBox.MaskedText, d);
							double num10;
							if(double.TryParse(doubleTextBox.MaskedText, out num10))
							{
								double num11 = Convert.ToDouble(doubleTextBox.MaskedText, numberFormat);
								double? num12 = num;
								if(num11 > num12.GetValueOrDefault() && num12.HasValue)
								{
									if(doubleTextBox.MinimumNumberDecimalDigits >= 0)
									{
										doubleTextBox.NumberDecimalDigits = doubleTextBox.MinimumNumberDecimalDigits;
										goto IL_521;
									}
									doubleTextBox.NumberDecimalDigits = doubleTextBox.numberDecimalDigits;
									goto IL_521;
								}
							}
							if(num9 >= 0)
							{
								if(doubleTextBox.MaximumNumberDecimalDigits == -1 && doubleTextBox.MinimumNumberDecimalDigits == -1)
								{
									num9 = doubleTextBox.numberDecimalDigits;
								}
								else
								{
									if(doubleTextBox.NumberDecimalDigits > 0 && doubleTextBox.MaximumNumberDecimalDigits >= 0 && doubleTextBox.MinimumNumberDecimalDigits == -1)
									{
										if(num9 >= doubleTextBox.MaximumNumberDecimalDigits)
										{
											num9 = doubleTextBox.MaximumNumberDecimalDigits;
										}
										if(num9 <= doubleTextBox.numberDecimalDigits)
										{
											num9 = doubleTextBox.numberDecimalDigits;
										}
									}
									else
									{
										if(doubleTextBox.NumberDecimalDigits > 0 && doubleTextBox.MinimumNumberDecimalDigits >= 0 && doubleTextBox.MaximumNumberDecimalDigits == -1)
										{
											if(num9 >= doubleTextBox.numberDecimalDigits)
											{
												num9 = doubleTextBox.numberDecimalDigits;
											}
											if(num9 <= doubleTextBox.MinimumNumberDecimalDigits)
											{
												num9 = doubleTextBox.MinimumNumberDecimalDigits;
											}
										}
										else
										{
											if(doubleTextBox.MinimumNumberDecimalDigits >= 0 && doubleTextBox.MaximumNumberDecimalDigits >= 0 && doubleTextBox.NumberDecimalDigits > 0)
											{
												if(num9 >= doubleTextBox.MaximumNumberDecimalDigits)
												{
													num9 = doubleTextBox.MaximumNumberDecimalDigits;
												}
												if(num9 <= doubleTextBox.MinimumNumberDecimalDigits)
												{
													num9 = doubleTextBox.MinimumNumberDecimalDigits;
												}
											}
										}
									}
								}
								doubleTextBox.NumberDecimalDigits = num9;
							}
							IL_521:
							DoubleValueHandler.DoubleValHandler.CanUpdate = false;
						}
					}
					double value = num.Value;
					return System.Math.Round(value, doubleTextBox.NumberDecimalDigits);
				}
				return num;
			}
			else
			{
				if(doubleTextBox.UseNullOption)
				{
					doubleTextBox.IsNull = true;
					doubleTextBox.IsNegative = false;
					doubleTextBox.IsZero = false;
					return doubleTextBox.NullValue;
				}
				double num13 = 0.0;
				bool? flag2 = doubleTextBox.mValueChanged;
				if(flag2.GetValueOrDefault() && flag2.HasValue)
				{
					if(num13 > doubleTextBox.MaxValue)
					{
						num13 = doubleTextBox.MaxValue;
					}
					if(num13 < doubleTextBox.MinValue)
					{
						num13 = doubleTextBox.MinValue;
					}
				}
				doubleTextBox.IsNegative = (num13 < 0.0);
				doubleTextBox.IsZero = (num13 == 0.0);
				doubleTextBox.IsNull = false;
				if(doubleTextBox.NumberDecimalDigits > 0)
				{
					double value2 = num13;
					return System.Math.Round(value2, doubleTextBox.numberDecimalDigits);
				}
				return num13;
			}
		}
		private static int CountDecimalDigits(string p, DependencyObject d)
		{
			if(!string.IsNullOrEmpty(p) && d is DoubleTextBox)
			{
				int num = 0;
				DoubleTextBox doubleTextBox = (DoubleTextBox)d;
				NumberFormatInfo numberFormat = doubleTextBox.GetCulture().NumberFormat;
				for(int i = p.Length - 1; i >= 0; i--)
				{
					if(numberFormat != null)
					{
						if(p[i].ToString() == numberFormat.NumberDecimalSeparator || p.Length.ToString() == doubleTextBox.NumberDecimalSeparator)
						{
							break;
						}
						num++;
					}
				}
				return num;
			}
			return 0;
		}
		private static object CoerceMinValue(DependencyObject d, object baseValue)
		{
			DoubleTextBox doubleTextBox = (DoubleTextBox)d;
			if(doubleTextBox.MinValue > doubleTextBox.MaxValue)
			{
				return doubleTextBox.MaxValue;
			}
			return baseValue;
		}
		private static object CoerceMaxValue(DependencyObject d, object baseValue)
		{
			DoubleTextBox doubleTextBox = (DoubleTextBox)d;
			if(doubleTextBox.MinValue > doubleTextBox.MaxValue)
			{
				return doubleTextBox.MinValue;
			}
			return baseValue;
		}
		private void copy()
		{
			try
			{
				Clipboard.SetText(base.SelectedText);
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
					double num = 0.0;
					if(!base.UseNullOption && this.Value.HasValue)
					{
						num = this.Value.Value;
					}
					string text = Clipboard.GetText();
					int selectionStart = base.SelectionStart;
					NumberFormatInfo numberFormat = this.GetCulture().NumberFormat;
					string text2 = string.Empty;
					string text3 = string.Empty;
					int num2 = 0;
					int num3 = 0;
					for(int i = 0; i < text.Length; i++)
					{
						if(numberFormat != null)
						{
							if(numberFormat.NumberDecimalSeparator != null)
							{
								if(char.IsDigit(text[i]) && i == num2)
								{
									num2 = i + 1;
									text2 += text[i];
								}
								else
								{
									if(text[i].ToString() == numberFormat.NumberDecimalSeparator)
									{
										num2 = i;
									}
									else
									{
										if(char.IsDigit(text[i]))
										{
											text3 += text[i];
										}
										else
										{
											if(i <= num2)
											{
												num2 = i + 1;
											}
										}
									}
								}
							}
						}
						else
						{
							if(char.IsDigit(text[i]) && i == num2)
							{
								num2 = i + 1;
								text2 += text[i];
							}
						}
					}
					if(base.SelectionLength > 0)
					{
						if(text == string.Empty)
						{
							if(this.OldValue.HasValue)
							{
								this.SetValue(new bool?(false), this.OldValue);
								base.Text = this.OldValue.Value.ToString("N", numberFormat);
							}
						}
						else
						{
							if(base.SelectedText.Length == base.Text.Length)
							{
								if(text[0] == '-')
								{
									num3 = 1;
								}
								else
								{
									if(base.Culture.Name == "ar-SA" && text[text.Length - 1] == '-')
									{
										num3 = 1;
									}
								}
							}
							if(base.SelectedText.Length == base.Text.Length || base.Text.Contains(base.SelectedText) || base.SelectedText.Contains(numberFormat.NumberDecimalSeparator))
							{
								base.Text = base.Text.Replace(base.SelectedText, text2);
								if(numberFormat != null && numberFormat.NumberDecimalSeparator != null)
								{
									if(text3 != string.Empty && !base.Text.Contains(numberFormat.NumberDecimalSeparator))
									{
										base.Text = base.Text + numberFormat.NumberDecimalSeparator + text3;
									}
									else
									{
										if(base.Text.Contains(numberFormat.NumberDecimalSeparator))
										{
											for(int j = 0; j < base.Text.Length; j++)
											{
												if(base.Text[j].ToString() == numberFormat.NumberDecimalSeparator)
												{
													num2 = j;
												}
											}
											if(num2 < base.Text.Length)
											{
												base.Text = base.Text.Insert(num2 + 1, text3);
											}
										}
									}
								}
								if(num3 == 1)
								{
									base.Text = "-" + base.Text;
								}
							}
						}
					}
					else
					{
						double? value = this.Value;
						if(value.GetValueOrDefault() == 0.0 && value.HasValue)
						{
							base.Text = text;
						}
						else
						{
							if(base.SelectionStart == base.Text.Length && base.Text.Length != 0)
							{
								return;
							}
							if(base.Text.Length == 0)
							{
								if(text[0] == '-')
								{
									num3 = 1;
								}
								else
								{
									if(base.Culture.Name == "ar-SA" && text[text.Length - 1] == '-')
									{
										num3 = 1;
									}
								}
							}
							base.Text = base.Text.Insert(base.SelectionStart, text2);
							if(numberFormat != null && numberFormat.NumberDecimalSeparator != null)
							{
								if(text3 != string.Empty && !base.Text.Contains(numberFormat.NumberDecimalSeparator))
								{
									base.Text = base.Text + numberFormat.NumberDecimalSeparator + text3;
								}
								else
								{
									if(base.Text.Contains(numberFormat.NumberDecimalSeparator))
									{
										for(int k = 0; k < base.Text.Length; k++)
										{
											if(base.Text[k].ToString() == numberFormat.NumberDecimalSeparator)
											{
												num2 = k;
											}
										}
										if(num2 < base.Text.Length)
										{
											base.Text = base.Text.Insert(num2 + 1, text3);
										}
									}
								}
							}
							if(num3 == 1)
							{
								base.Text = "-" + base.Text;
							}
						}
					}
					if(base.Text.Length >= 15 && base.MaxValidation == MaxValidation.OnLostFocus)
					{
						try
						{
							base.MaskedText = decimal.Parse(base.Text).ToString("N", numberFormat);
						}
						catch
						{
						}
					}
					else
					{
						if(this.IsExceedDecimalDigits)
						{
							for(int l = base.Text.Length - 1; l >= 0; l--)
							{
								if(numberFormat != null)
								{
									if(base.Text[l].ToString() == numberFormat.NumberDecimalSeparator || base.Text.Length.ToString() == this.NumberDecimalSeparator)
									{
										break;
									}
									this.count++;
								}
							}
							if(this.count >= this.MinimumNumberDecimalDigits && this.count < this.MaximumNumberDecimalDigits)
							{
								if(this.MaximumNumberDecimalDigits > 0)
								{
									if(this.MinimumNumberDecimalDigits > 0)
									{
										DoubleValueHandler.DoubleValHandler.CanUpdate = true;
										this.NumberDecimalDigits = this.count - 1;
										DoubleValueHandler.DoubleValHandler.AllowChange = true;
										DoubleValueHandler.DoubleValHandler.CanUpdate = false;
									}
									else
									{
										if(this.count <= this.numberDecimalDigits)
										{
											this.NumberDecimalDigits = this.numberDecimalDigits;
										}
										else
										{
											if(this.count <= this.MaximumNumberDecimalDigits)
											{
												DoubleValueHandler.DoubleValHandler.CanUpdate = true;
												this.NumberDecimalDigits = this.count;
												DoubleValueHandler.DoubleValHandler.AllowChange = true;
												DoubleValueHandler.DoubleValHandler.CanUpdate = false;
											}
										}
									}
								}
							}
							else
							{
								if(this.MaximumNumberDecimalDigits > 0)
								{
									DoubleValueHandler.DoubleValHandler.CanUpdate = true;
									this.NumberDecimalDigits = this.MaximumNumberDecimalDigits;
									DoubleValueHandler.DoubleValHandler.AllowChange = false;
									DoubleValueHandler.DoubleValHandler.CanUpdate = false;
								}
							}
							numberFormat = this.GetCulture().NumberFormat;
						}
						bool flag = false;
						bool flag2 = false;
						if(numberFormat.NumberDecimalSeparator != string.Empty && numberFormat.NumberDecimalSeparator != CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
						{
							base.Text = base.Text.Replace(numberFormat.NumberDecimalSeparator, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
							flag = true;
						}
						if(numberFormat.NumberGroupSeparator != string.Empty && numberFormat.NumberGroupSeparator != CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator)
						{
							base.Text = base.Text.Replace(numberFormat.NumberGroupSeparator, CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator);
							flag2 = true;
						}
						double num4;
						double.TryParse(base.Text, out num4);
						if((num4 > this.MaxValue && base.MaxValidation == MaxValidation.OnKeyPress) || (num4 < this.MinValue && base.MinValidation == MinValidation.OnKeyPress))
						{
							if(num4 > this.MaxValue && base.MaxValidation == MaxValidation.OnKeyPress)
							{
								if(base.MaxValueOnExceedMaxDigit)
								{
									num4 = this.MaxValue;
									this.SetValue(new bool?(false), new double?(num4));
									if(flag || flag2)
									{
										base.MaskedText = num4.ToString("N", numberFormat);
									}
									base.Text = num4.ToString("N", numberFormat);
									base.CaretIndex = selectionStart;
								}
								else
								{
									if(num == 0.0 && base.UseNullOption)
									{
										this.Value = null;
										base.Text = null;
									}
									else
									{
										if(!double.IsNaN(num))
										{
											this.SetValue(new bool?(false), new double?(num));
											double num5 = num;
											base.Text = num5.ToString("N", numberFormat);
										}
										else
										{
											this.Value = new double?(double.NaN);
											base.Text = "";
										}
									}
								}
							}
							else
							{
								if(num4 < this.MinValue && base.MinValidation == MinValidation.OnKeyPress)
								{
									if(base.MinValueOnExceedMinDigit)
									{
										num4 = this.MinValue;
										this.SetValue(new bool?(false), new double?(num4));
										if(flag || flag2)
										{
											base.MaskedText = num4.ToString("N", numberFormat);
										}
										base.Text = num4.ToString("N", numberFormat);
										base.CaretIndex = selectionStart;
									}
									else
									{
										if(num == 0.0 && base.UseNullOption)
										{
											this.Value = null;
											base.Text = null;
										}
										else
										{
											if(!double.IsNaN(num))
											{
												this.SetValue(new bool?(false), new double?(num));
												double num6 = num;
												base.Text = num6.ToString("N", numberFormat);
											}
											else
											{
												this.Value = new double?(double.NaN);
												base.Text = "";
											}
										}
									}
								}
							}
						}
						else
						{
							if(!double.IsNaN(num4))
							{
								this.SetValue(new bool?(false), new double?(num4));
								if(flag || flag2)
								{
									base.MaskedText = num4.ToString("N", numberFormat);
								}
								if(base.Culture.Name == "vi-VN" && this.NumberGroupSeparator == string.Empty)
								{
									base.Text = num4.ToString("N", numberFormat).Replace(".", "");
								}
								else
								{
									base.Text = num4.ToString("N", numberFormat);
								}
								int caretIndex = 0;
								if(base.Culture.Name != "ar-SA")
								{
									caretIndex = selectionStart + text.Length;
								}
								base.CaretIndex = caretIndex;
							}
						}
						flag = false;
						flag2 = false;
						num3 = 0;
					}
				}
				catch(COMException)
				{
				}
			}
		}
		private void DoubleTextbox_Loaded(object sender, RoutedEventArgs e)
		{
			base.TextChanged -= new TextChangedEventHandler(this.DoubleTextBox_TextChanged);
			base.TextChanged += new TextChangedEventHandler(this.DoubleTextBox_TextChanged);
			this.mIsLoaded = true;
			object obj = DoubleTextBox.CoerceValue(this, this.Value);
			double? num = (double?)obj;
			if(base.IsNull)
			{
				base.WatermarkVisibility = Visibility.Visible;
			}
			double? num2 = num;
			double? value = this.Value;
			if(num2.GetValueOrDefault() != value.GetValueOrDefault() || num2.HasValue != value.HasValue)
			{
				if(!this.TriggerValueChangingEvent(new ValueChangingEventArgs
				{
					OldValue = this.Value,
					NewValue = num
				}))
				{
					if(base.UseNullOption)
					{
						if(num.HasValue)
						{
							this.Value = num;
						}
					}
					else
					{
						this.Value = num;
					}
				}
			}
			else
			{
				this.FormatText();
			}
			if(base.TextSelectionOnFocus && base.IsFocused)
			{
				e.Handled = true;
				base.Focus();
				base.SelectAll();
			}
			if(base.IsUndoEnabled)
			{
				this.utext = base.MaskedText;
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
		public static void OnIsExceedDecimalDigits(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			DoubleTextBox doubleTextBox = (DoubleTextBox)obj;
			if(doubleTextBox != null)
			{
				doubleTextBox.OnIsExceedDecimalDigitsChanged(args);
			}
		}
		protected void OnIsExceedDecimalDigitsChanged(DependencyPropertyChangedEventArgs args)
		{
			if(this.IsExceedDecimalDigitsChanged != null)
			{
				this.IsExceedDecimalDigitsChanged(this, args);
			}
		}
		public static void OnMaximumNumberDecimalDigitsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			DoubleTextBox doubleTextBox = (DoubleTextBox)obj;
			if(doubleTextBox != null)
			{
				doubleTextBox.OnMaximumNumberDecimalDigitsChanged(args);
			}
		}
		protected void OnMaximumNumberDecimalDigitsChanged(DependencyPropertyChangedEventArgs args)
		{
			if(this.MaximumNumberDecimalDigitsChanged != null)
			{
				this.MaximumNumberDecimalDigitsChanged(this, args);
			}
			if(this.MaximumNumberDecimalDigits > -1)
			{
				this.IsExceedDecimalDigits = true;
			}
			if(this.MaximumNumberDecimalDigits < this.NumberDecimalDigits && this.MaximumNumberDecimalDigits >= 0 && this.NumberDecimalDigits >= 0)
			{
				throw new InvalidOperationException("MaximumNumberDecimalDigits should not be lesser than NumberDecimalDigits");
			}
			if(this.MinimumNumberDecimalDigits >= 0 && this.MaximumNumberDecimalDigits >= 0)
			{
				if(this.MaximumNumberDecimalDigits < this.NumberDecimalDigits || this.MaximumNumberDecimalDigits < this.MinimumNumberDecimalDigits)
				{
					throw new InvalidOperationException("MaximumNumberDecimalDigits should not be lesser than NumberDecimalDigits or MinimumNumberDecimalDigits");
				}
				if(this.MinimumNumberDecimalDigits > this.MaximumNumberDecimalDigits)
				{
					throw new InvalidOperationException("MinimumNumberDecimalDigits should not be greater than NumberDecimalDigits or MaximumNumberDecimalDigits");
				}
			}
		}
		public static void OnMinimumNumberDecimalDigitsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			DoubleTextBox doubleTextBox = (DoubleTextBox)obj;
			if(doubleTextBox != null)
			{
				doubleTextBox.OnMinimumNumberDecimalDigitsChanged(args);
			}
		}
		protected void OnMinimumNumberDecimalDigitsChanged(DependencyPropertyChangedEventArgs args)
		{
			if(this.MinimumNumberDecimalDigitsChanged != null)
			{
				this.MinimumNumberDecimalDigitsChanged(this, args);
			}
			if(this.MinimumNumberDecimalDigits > -1)
			{
				this.IsExceedDecimalDigits = true;
			}
			if(this.MinimumNumberDecimalDigits > this.NumberDecimalDigits && this.MinimumNumberDecimalDigits >= 0 && this.NumberDecimalDigits >= 0)
			{
				throw new InvalidOperationException("MinimumNumberDecimalDigits should not be greater than NumberDecimalDigits or MaximumNumberDecimalDigits");
			}
			if(this.MinimumNumberDecimalDigits >= 0 && this.MaximumNumberDecimalDigits >= 0)
			{
				if(this.MaximumNumberDecimalDigits < this.NumberDecimalDigits || this.MaximumNumberDecimalDigits < this.MinimumNumberDecimalDigits)
				{
					throw new InvalidOperationException("MaximumNumberDecimalDigits should not be lesser than NumberDecimalDigits or MinimumNumberDecimalDigits");
				}
				if((this.MinimumNumberDecimalDigits > this.NumberDecimalDigits && this.NumberDecimalDigits >= 0) || this.MinimumNumberDecimalDigits > this.MaximumNumberDecimalDigits)
				{
					throw new InvalidOperationException("MinimumNumberDecimalDigits should not be greater than NumberDecimalDigits or MaximumNumberDecimalDigits");
				}
			}
		}
		public static void OnNumberGroupSizesChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			DoubleTextBox doubleTextBox = (DoubleTextBox)obj;
			if(doubleTextBox != null)
			{
				doubleTextBox.OnNumberGroupSizesChanged(args);
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
		public static void OnNumberDecimalSeparatorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			DoubleTextBox doubleTextBox = (DoubleTextBox)obj;
			if(doubleTextBox != null)
			{
				doubleTextBox.OnNumberDecimalSeparatorChanged(args);
			}
		}
		protected void OnNumberDecimalSeparatorChanged(DependencyPropertyChangedEventArgs args)
		{
			if(this.NumberDecimalSeparatorChanged != null)
			{
				this.NumberDecimalSeparatorChanged(this, args);
			}
			if(this.mIsLoaded)
			{
				this.FormatText();
			}
		}
		public static void OnNumberDecimalDigitsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			DoubleTextBox doubleTextBox = (DoubleTextBox)obj;
			if(doubleTextBox != null)
			{
				doubleTextBox.OnNumberDecimalDigitsChanged(args);
			}
		}
		protected void OnNumberDecimalDigitsChanged(DependencyPropertyChangedEventArgs args)
		{
			if(this.NumberDecimalDigits < 0 && !this.mIsLoaded)
			{
				throw new InvalidOperationException("NumberDecimalDigits must be greater than zero");
			}
			if(this.NumberDecimalDigits < 0 && this.mIsLoaded)
			{
				this.NumberDecimalDigits = 0;
			}
			if(this.NumberDecimalDigits < this.MinimumNumberDecimalDigits && this.NumberDecimalDigits >= 0 && this.MinimumNumberDecimalDigits >= 0)
			{
				throw new InvalidOperationException("NumberDecimalDigits should not be lesser than MinimumNumberDecimalDigits");
			}
			if(this.MaximumNumberDecimalDigits >= 0)
			{
				if(this.MaximumNumberDecimalDigits < this.NumberDecimalDigits && this.NumberDecimalDigits >= 0)
				{
					throw new InvalidOperationException("MaximumNumberDecimalDigits should not be lesser than NumberDecimalDigits");
				}
				if(this.MaximumNumberDecimalDigits < this.MinimumNumberDecimalDigits && this.MinimumNumberDecimalDigits >= 0)
				{
					throw new InvalidOperationException("MaximumNumberDecimalDigits should not be lesser than NumberDecimalDigits");
				}
			}
			if(this.NumberDecimalDigitsChanged != null)
			{
				this.NumberDecimalDigitsChanged(this, args);
			}
			if(!DoubleValueHandler.DoubleValHandler.CanUpdate)
			{
				this.numberDecimalDigits = this.NumberDecimalDigits;
			}
			if(this.mIsLoaded)
			{
				this.FormatText();
			}
		}
		public static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			if((DoubleTextBox)obj != null)
			{
				((DoubleTextBox)obj).OnValueChanged(args);
			}
		}
		public override void OnUseNullOptionChanged(DependencyPropertyChangedEventArgs args)
		{
			if(!(bool)args.NewValue && base.IsNull)
			{
				this.Value = new double?(this.MinValue);
				base.IsNull = false;
			}
			base.OnUseNullOptionChanged(args);
		}
		protected void OnValueChanged(DependencyPropertyChangedEventArgs args)
		{
			if(this.Value.HasValue && !double.IsNaN(this.Value.Value))
			{
				double? value = this.Value;
				base.IsNegative = (value.GetValueOrDefault() < 0.0 && value.HasValue);
				double? value2 = this.Value;
				base.IsZero = (value2.GetValueOrDefault() == 0.0 && value2.HasValue);
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
			this.OldValue = (double?)args.OldValue;
			this.mValue = this.Value;
			if(this.Value.HasValue)
			{
				base.WatermarkVisibility = Visibility.Collapsed;
			}
			if(this.ValueChanged != null)
			{
				this.ValueChanged(this, args);
			}
			double? value3 = this.Value;
			double minValue = this.MinValue;
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
			if((DoubleTextBox)obj != null)
			{
				((DoubleTextBox)obj).OnMinValueChanged(args);
			}
		}
		protected void OnMinValueChanged(DependencyPropertyChangedEventArgs args)
		{
			if(this.MinValueChanged != null)
			{
				this.MinValueChanged(this, args);
			}
			if(base.UseNullOption && this.Value.HasValue && !double.IsNaN(this.Value.Value) && this.MinValue != 0.0)
			{
				double? value = this.Value;
				double minValue = this.MinValue;
				if(value.GetValueOrDefault() < minValue && value.HasValue)
				{
					this.Value = new double?(this.MinValue);
				}
			}
			double? value2 = this.Value;
			double? num = this.ValidateValue(this.Value);
			if(value2.GetValueOrDefault() != num.GetValueOrDefault() || value2.HasValue != num.HasValue)
			{
				double? num2 = this.ValidateValue(this.Value);
				if(!this.TriggerValueChangingEvent(new ValueChangingEventArgs
				{
					OldValue = this.Value,
					NewValue = num2
				}))
				{
					this.Value = num2;
				}
			}
			if(BindingOperations.GetBindingExpression(this, DoubleTextBox.ValueProperty) != null)
			{
				BindingOperations.GetBindingExpression(this, DoubleTextBox.ValueProperty).UpdateTarget();
			}
		}
		public static void OnMaxValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			if((DoubleTextBox)obj != null)
			{
				((DoubleTextBox)obj).OnMaxValueChanged(args);
			}
		}
		protected void OnMaxValueChanged(DependencyPropertyChangedEventArgs args)
		{
			if(this.MaxValueChanged != null)
			{
				this.MaxValueChanged(this, args);
			}
			double? value = this.Value;
			double? num = this.ValidateValue(this.Value);
			if(value.GetValueOrDefault() != num.GetValueOrDefault() || value.HasValue != num.HasValue)
			{
				double? num2 = this.ValidateValue(this.Value);
				if(!this.TriggerValueChangingEvent(new ValueChangingEventArgs
				{
					OldValue = this.Value,
					NewValue = num2
				}))
				{
					this.Value = num2;
				}
			}
			if(BindingOperations.GetBindingExpression(this, DoubleTextBox.ValueProperty) != null)
			{
				BindingOperations.GetBindingExpression(this, DoubleTextBox.ValueProperty).UpdateTarget();
			}
		}
		private static void OnNumberGroupSeparatorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			if((DoubleTextBox)obj != null)
			{
				((DoubleTextBox)obj).OnNumberGroupSeparatorChanged(args);
			}
		}
		private static object CoerceNumberGroupSeperator(DependencyObject d, object baseValue)
		{
			DoubleTextBox doubleTextBox = (DoubleTextBox)d;
			NumberFormatInfo numberFormat = doubleTextBox.GetCulture().NumberFormat;
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
			if((DoubleTextBox)obj != null)
			{
				((DoubleTextBox)obj).OnNullValueChanged(args);
			}
		}
		protected void OnNullValueChanged(DependencyPropertyChangedEventArgs args)
		{
		}
		public static void OnInvalidValueBehaviorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			DoubleTextBox doubleTextBox = (DoubleTextBox)obj;
			if(doubleTextBox != null)
			{
				doubleTextBox.OnInvalidValueBehaviorChanged(args);
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
			DoubleTextBox doubleTextBox = (DoubleTextBox)obj;
			if(doubleTextBox != null)
			{
				doubleTextBox.OnValidationValueChanged(args);
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
			DoubleTextBox doubleTextBox = (DoubleTextBox)obj;
			if(doubleTextBox != null)
			{
				doubleTextBox.OnValidationCompletedPropertyChanged(args);
			}
		}
		protected void OnValidationCompletedPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			if(this.ValidationCompletedChanged != null)
			{
				this.ValidationCompletedChanged(this, args);
			}
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
	}
}