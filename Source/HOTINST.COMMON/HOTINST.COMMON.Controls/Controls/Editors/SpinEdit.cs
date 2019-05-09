/**
 * ==============================================================================
 *
 * ClassName: SpinEdit
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/3/23 14:09:32
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using HOTINST.COMMON.Controls.Core.Editors;

#pragma warning disable 1591

namespace HOTINST.COMMON.Controls.Controls.Editors
{
	public class SpinEdit : Control
	{
		public delegate void ValueChangingEventHandler(object sender, ValueChangingEventArgs e);
		public static RoutedCommand m_downValue;
		public static RoutedCommand m_upValue;
		private Border border;
		private DoubleTextBox t1;
		private DoubleTextBox secondBlock;
		private DoubleTextBox textbox;
		private TextBox Nulltextbox;
		private RepeatButton Upbutton;
		private RepeatButton Downbutton;
		private double? m_value;
		private double? m_oldvalue;
		private double? m_exvalue;
		public static readonly DependencyProperty IsScrollingOnCircleProperty;
		public static readonly DependencyProperty IsReadOnlyProperty;
		public static readonly DependencyProperty NumberDecimalDigitsProperty;
		public static readonly DependencyProperty GroupSeperatorEnabledProperty;
		public new static readonly DependencyProperty IsFocusedProperty;
		public static readonly DependencyProperty NullValueTextProperty;
		public static readonly DependencyProperty AnimationShiftProperty;
		public static readonly DependencyProperty IsValueNegativeProperty;
		public static readonly DependencyProperty CursorBackgroundProperty;
		public static readonly DependencyProperty CursorBorderBrushProperty;
		public static readonly DependencyProperty CursorWidthProperty;
		public static readonly DependencyProperty CursorBorderThicknessProperty;
		public static readonly DependencyProperty CursorTemplateProperty;
		public static readonly DependencyProperty CursorVisibleProperty;
		public static readonly DependencyProperty CursorPositionProperty;
		public static readonly DependencyProperty SelectionBrushProperty;
		public static readonly DependencyProperty EnableRangeAdornerProperty;
		public static readonly DependencyProperty RangeAdornerBackgroundProperty;
		public static readonly DependencyProperty EnableExtendedScrollingProperty;
		public static readonly DependencyProperty EnableTouchProperty;
		public static readonly DependencyProperty SpinEditForegroundProperty;
		public static readonly DependencyProperty SpinEditBackgroundProperty;
		public static readonly DependencyProperty SpinEditBorderBrushProperty;
		public static readonly DependencyProperty TextAlignmentProperty;
		public static readonly DependencyProperty ApplyZeroColorProperty;
		public static readonly DependencyProperty EnableNegativeColorsProperty;
		public static readonly DependencyProperty CultureProperty;
		public static readonly DependencyProperty EnableFocusedColorsProperty;
		public static readonly DependencyProperty FocusedBackgroundProperty;
		public static readonly DependencyProperty IsSpinEditFocusedProperty;
		public static readonly DependencyProperty FocusedForegroundProperty;
		public static readonly DependencyProperty FocusedBorderBrushProperty;
		public static readonly DependencyProperty NegativeBackgroundProperty;
		public static readonly DependencyProperty NegativeBorderBrushProperty;
		public static readonly DependencyProperty AllowEditProperty;
		public static readonly DependencyProperty MinValidationProperty;
		public static readonly DependencyProperty MaxValidationProperty;
		public static readonly DependencyProperty MinValueOnExceedMinDigitProperty;
		public static readonly DependencyProperty MaxValueOnExceedMaxDigitProperty;
		public static readonly DependencyProperty NegativeForegroundProperty;
		public static readonly DependencyProperty ZeroColorProperty;
		public static readonly DependencyProperty UseNullOptionProperty;
		public static readonly DependencyProperty NumberFormatInfoProperty;
		public static readonly DependencyProperty CornerRadiusProperty;
		public static readonly DependencyProperty NullValueProperty;
		public static readonly DependencyProperty ValueProperty;
		public static readonly DependencyProperty MinValueProperty;
		public static readonly DependencyProperty MaxValueProperty;
		public static readonly DependencyProperty StepProperty;
		public static readonly DependencyProperty AnimationSpeedProperty;
		public static readonly DependencyProperty ShowButtonProperty;
		private bool nagativevaluechanged;
		public event PropertyChangedCallback AllowEditChanged;
		public event PropertyChangedCallback StepChanged;
		public event PropertyChangedCallback UseNullOptionChanged;
		public event PropertyChangedCallback ValueChanged;
		public event SpinEdit.ValueChangingEventHandler ValueChanging;
		public event PropertyChangedCallback MinValueChanged;
		public event PropertyChangedCallback IsScrollingOnCircleChanged;
		public event PropertyChangedCallback MaxValueChanged;
		public event PropertyChangedCallback NumberFormatInfoChanged;
		public event PropertyChangedCallback ZeroColorChanged;
		public event PropertyChangedCallback NegativeForegroundChanged;
		public event PropertyChangedCallback MinValidationChanged;
		public event PropertyChangedCallback MaxValidationChanged;
		[Obsolete("Event will not help due to internal arhitecture changes")]
		public event PropertyChangedCallback CursorTemplateChanged;
		[Obsolete("Event will not help due to internal arhitecture changes")]
		public event PropertyChangedCallback IsValueNegativeChanged;
		public event PropertyChangedCallback NullValueTextChanged;
		public event PropertyChangedCallback FocusedBackgroundChanged;
		public event PropertyChangedCallback FocusedForegroundChanged;
		public event PropertyChangedCallback FocusedBorderBrushChanged;
		[Obsolete("Property will not help due to internal arhitecture changes")]
		internal double AnimationShift
		{
			get
			{
				return (double)base.GetValue(SpinEdit.AnimationShiftProperty);
			}
			set
			{
				base.SetValue(SpinEdit.AnimationShiftProperty, value);
			}
		}
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public bool IsValueNegative
		{
			get
			{
				return (bool)base.GetValue(SpinEdit.IsValueNegativeProperty);
			}
			set
			{
				base.SetValue(SpinEdit.IsValueNegativeProperty, value);
			}
		}
		[Obsolete("Property will not help due to internal arhitecture changes")]
		public Brush CursorBackground
		{
			get
			{
				return (Brush)base.GetValue(SpinEdit.CursorBackgroundProperty);
			}
			set
			{
				base.SetValue(SpinEdit.CursorBackgroundProperty, value);
			}
		}
		public bool IsScrollingOnCircle
		{
			get
			{
				return (bool)base.GetValue(SpinEdit.IsScrollingOnCircleProperty);
			}
			set
			{
				base.SetValue(SpinEdit.IsScrollingOnCircleProperty, value);
			}
		}
		public bool IsReadOnly
		{
			get
			{
				return (bool)base.GetValue(SpinEdit.IsReadOnlyProperty);
			}
			set
			{
				base.SetValue(SpinEdit.IsReadOnlyProperty, value);
			}
		}
		public int NumberDecimalDigits
		{
			get
			{
				return (int)base.GetValue(SpinEdit.NumberDecimalDigitsProperty);
			}
			set
			{
				base.SetValue(SpinEdit.NumberDecimalDigitsProperty, value);
			}
		}
		[Obsolete("Property will not help due to internal arhitecture changes")]
		public Brush CursorBorderBrush
		{
			get
			{
				return (Brush)base.GetValue(SpinEdit.CursorBorderBrushProperty);
			}
			set
			{
				base.SetValue(SpinEdit.CursorBorderBrushProperty, value);
			}
		}
		[Obsolete("Property will not help due to internal arhitecture changes")]
		public double CursorWidth
		{
			get
			{
				return (double)base.GetValue(SpinEdit.CursorWidthProperty);
			}
			set
			{
				base.SetValue(SpinEdit.CursorWidthProperty, value);
			}
		}
		[Obsolete("Property will not help due to internal arhitecture changes")]
		public Thickness CursorBorderThickness
		{
			get
			{
				return (Thickness)base.GetValue(SpinEdit.CursorBorderThicknessProperty);
			}
			set
			{
				base.SetValue(SpinEdit.CursorBorderThicknessProperty, value);
			}
		}
		[Obsolete("Property will not help due to internal arhitecture changes")]
		public ControlTemplate CursorTemplate
		{
			get
			{
				return (ControlTemplate)base.GetValue(SpinEdit.CursorTemplateProperty);
			}
			set
			{
				base.SetValue(SpinEdit.CursorTemplateProperty, value);
			}
		}
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false), Obsolete("Property will not help due to internal arhitecture changes")]
		public bool CursorVisible
		{
			get
			{
				return (bool)base.GetValue(SpinEdit.CursorVisibleProperty);
			}
			set
			{
				base.SetValue(SpinEdit.CursorVisibleProperty, value);
			}
		}
		[Obsolete("Property will not help due to internal arhitecture changes")]
		internal Thickness CursorPosition
		{
			get
			{
				return (Thickness)base.GetValue(SpinEdit.CursorPositionProperty);
			}
			set
			{
				base.SetValue(SpinEdit.CursorPositionProperty, value);
			}
		}
		[Obsolete("Property will not help due to internal arhitecture changes")]
		public Brush SelectionBrush
		{
			get
			{
				return (Brush)base.GetValue(SpinEdit.SelectionBrushProperty);
			}
			set
			{
				base.SetValue(SpinEdit.SelectionBrushProperty, value);
			}
		}
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public Brush SpinEditForeground
		{
			get
			{
				return (Brush)base.GetValue(SpinEdit.SpinEditForegroundProperty);
			}
			set
			{
				base.SetValue(SpinEdit.SpinEditForegroundProperty, value);
			}
		}
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public Brush SpinEditBackground
		{
			get
			{
				return (Brush)base.GetValue(SpinEdit.SpinEditBackgroundProperty);
			}
			set
			{
				base.SetValue(SpinEdit.SpinEditBackgroundProperty, value);
			}
		}
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public Brush SpinEditBorderBrush
		{
			get
			{
				return (Brush)base.GetValue(SpinEdit.SpinEditBorderBrushProperty);
			}
			set
			{
				base.SetValue(SpinEdit.SpinEditBorderBrushProperty, value);
			}
		}
		public TextAlignment TextAlignment
		{
			get
			{
				return (TextAlignment)base.GetValue(SpinEdit.TextAlignmentProperty);
			}
			set
			{
				base.SetValue(SpinEdit.TextAlignmentProperty, value);
			}
		}
		public bool ApplyZeroColor
		{
			get
			{
				return (bool)base.GetValue(SpinEdit.ApplyZeroColorProperty);
			}
			set
			{
				base.SetValue(SpinEdit.ApplyZeroColorProperty, value);
			}
		}
		public bool EnableNegativeColors
		{
			get
			{
				return (bool)base.GetValue(SpinEdit.EnableNegativeColorsProperty);
			}
			set
			{
				base.SetValue(SpinEdit.EnableNegativeColorsProperty, value);
			}
		}
		public CultureInfo Culture
		{
			get
			{
				return (CultureInfo)base.GetValue(SpinEdit.CultureProperty);
			}
			set
			{
				base.SetValue(SpinEdit.CultureProperty, value);
			}
		}
		public bool EnableFocusedColors
		{
			get
			{
				return (bool)base.GetValue(SpinEdit.EnableFocusedColorsProperty);
			}
			set
			{
				base.SetValue(SpinEdit.EnableFocusedColorsProperty, value);
			}
		}
		public Brush FocusedBackground
		{
			get
			{
				return (Brush)base.GetValue(SpinEdit.FocusedBackgroundProperty);
			}
			set
			{
				base.SetValue(SpinEdit.FocusedBackgroundProperty, value);
			}
		}
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public bool IsSpinEditFocused
		{
			get
			{
				return (bool)base.GetValue(SpinEdit.IsSpinEditFocusedProperty);
			}
			set
			{
				base.SetValue(SpinEdit.IsSpinEditFocusedProperty, value);
			}
		}
		public Brush FocusedForeground
		{
			get
			{
				return (Brush)base.GetValue(SpinEdit.FocusedForegroundProperty);
			}
			set
			{
				base.SetValue(SpinEdit.FocusedForegroundProperty, value);
			}
		}
		public Brush FocusedBorderBrush
		{
			get
			{
				return (Brush)base.GetValue(SpinEdit.FocusedBorderBrushProperty);
			}
			set
			{
				base.SetValue(SpinEdit.FocusedBorderBrushProperty, value);
			}
		}
		public Brush NegativeBackground
		{
			get
			{
				return (Brush)base.GetValue(SpinEdit.NegativeBackgroundProperty);
			}
			set
			{
				base.SetValue(SpinEdit.NegativeBackgroundProperty, value);
			}
		}
		public Brush NegativeBorderBrush
		{
			get
			{
				return (Brush)base.GetValue(SpinEdit.NegativeBorderBrushProperty);
			}
			set
			{
				base.SetValue(SpinEdit.NegativeBorderBrushProperty, value);
			}
		}
		public bool AllowEdit
		{
			get
			{
				return (bool)base.GetValue(SpinEdit.AllowEditProperty);
			}
			set
			{
				base.SetValue(SpinEdit.AllowEditProperty, value);
			}
		}
		public MinValidation MinValidation
		{
			get
			{
				return (MinValidation)base.GetValue(SpinEdit.MinValidationProperty);
			}
			set
			{
				base.SetValue(SpinEdit.MinValidationProperty, value);
			}
		}
		public MaxValidation MaxValidation
		{
			get
			{
				return (MaxValidation)base.GetValue(SpinEdit.MaxValidationProperty);
			}
			set
			{
				base.SetValue(SpinEdit.MaxValidationProperty, value);
			}
		}
		public bool MinValueOnExceedMinDigit
		{
			get
			{
				return (bool)base.GetValue(SpinEdit.MinValueOnExceedMinDigitProperty);
			}
			set
			{
				base.SetValue(SpinEdit.MinValueOnExceedMinDigitProperty, value);
			}
		}
		public bool MaxValueOnExceedMaxDigit
		{
			get
			{
				return (bool)base.GetValue(SpinEdit.MaxValueOnExceedMaxDigitProperty);
			}
			set
			{
				base.SetValue(SpinEdit.MaxValueOnExceedMaxDigitProperty, value);
			}
		}
		public Brush NegativeForeground
		{
			get
			{
				return (Brush)base.GetValue(SpinEdit.NegativeForegroundProperty);
			}
			set
			{
				base.SetValue(SpinEdit.NegativeForegroundProperty, value);
			}
		}
		public Brush ZeroColor
		{
			get
			{
				return (Brush)base.GetValue(SpinEdit.ZeroColorProperty);
			}
			set
			{
				base.SetValue(SpinEdit.ZeroColorProperty, value);
			}
		}
		public bool UseNullOption
		{
			get
			{
				return (bool)base.GetValue(SpinEdit.UseNullOptionProperty);
			}
			set
			{
				base.SetValue(SpinEdit.UseNullOptionProperty, value);
			}
		}
		public NumberFormatInfo NumberFormatInfo
		{
			get
			{
				return (NumberFormatInfo)base.GetValue(SpinEdit.NumberFormatInfoProperty);
			}
			set
			{
				base.SetValue(SpinEdit.NumberFormatInfoProperty, value);
			}
		}
		public CornerRadius CornerRadius
		{
			get
			{
				return (CornerRadius)base.GetValue(SpinEdit.CornerRadiusProperty);
			}
			set
			{
				base.SetValue(SpinEdit.CornerRadiusProperty, value);
			}
		}
		public double? NullValue
		{
			get
			{
				return (double?)base.GetValue(SpinEdit.NullValueProperty);
			}
			set
			{
				base.SetValue(SpinEdit.NullValueProperty, value);
			}
		}
		public double? Value
		{
			get
			{
				return (double?)base.GetValue(SpinEdit.ValueProperty);
			}
			set
			{
				base.SetValue(SpinEdit.ValueProperty, value);
			}
		}
		public double MinValue
		{
			get
			{
				return (double)base.GetValue(SpinEdit.MinValueProperty);
			}
			set
			{
				base.SetValue(SpinEdit.MinValueProperty, value);
			}
		}
		public double MaxValue
		{
			get
			{
				return (double)base.GetValue(SpinEdit.MaxValueProperty);
			}
			set
			{
				base.SetValue(SpinEdit.MaxValueProperty, value);
			}
		}
		public double Step
		{
			get
			{
				return (double)base.GetValue(SpinEdit.StepProperty);
			}
			set
			{
				base.SetValue(SpinEdit.StepProperty, value);
			}
		}
		public double AnimationSpeed
		{
			get
			{
				return (double)base.GetValue(SpinEdit.AnimationSpeedProperty);
			}
			set
			{
				base.SetValue(SpinEdit.AnimationSpeedProperty, value);
			}
		}
		public bool ShowButton
		{
			get
			{
				return (bool)base.GetValue(SpinEdit.ShowButtonProperty);
			}
			set
			{
				base.SetValue(SpinEdit.ShowButtonProperty, value);
			}
		}
		public new bool IsKeyboardFocused
		{
			get
			{
				return (this.textbox != null && this.textbox.IsKeyboardFocused) || (this.Upbutton != null && this.Upbutton.IsKeyboardFocused) || (this.Downbutton != null && this.Downbutton.IsKeyboardFocused);
			}
		}
		public bool GroupSeperatorEnabled
		{
			get
			{
				return (bool)base.GetValue(SpinEdit.GroupSeperatorEnabledProperty);
			}
			set
			{
				base.SetValue(SpinEdit.GroupSeperatorEnabledProperty, value);
			}
		}
		public new bool IsFocused
		{
			get
			{
				return (bool)base.GetValue(SpinEdit.IsFocusedProperty);
			}
			internal set
			{
				base.SetValue(SpinEdit.IsFocusedProperty, value);
			}
		}
		public string NullValueText
		{
			get
			{
				return (string)base.GetValue(SpinEdit.NullValueTextProperty);
			}
			set
			{
				base.SetValue(SpinEdit.NullValueTextProperty, value);
			}
		}
		public bool EnableRangeAdorner
		{
			get
			{
				return (bool)base.GetValue(SpinEdit.EnableRangeAdornerProperty);
			}
			set
			{
				base.SetValue(SpinEdit.EnableRangeAdornerProperty, value);
			}
		}
		public Brush RangeAdornerBackground
		{
			get
			{
				return (Brush)base.GetValue(SpinEdit.RangeAdornerBackgroundProperty);
			}
			set
			{
				base.SetValue(SpinEdit.RangeAdornerBackgroundProperty, value);
			}
		}
		public bool EnableExtendedScrolling
		{
			get
			{
				return (bool)base.GetValue(SpinEdit.EnableExtendedScrollingProperty);
			}
			set
			{
				base.SetValue(SpinEdit.EnableExtendedScrollingProperty, value);
			}
		}
		public bool EnableTouch
		{
			get
			{
				return (bool)base.GetValue(SpinEdit.EnableTouchProperty);
			}
			set
			{
				base.SetValue(SpinEdit.EnableTouchProperty, value);
			}
		}
		static SpinEdit()
		{
			SpinEdit.IsScrollingOnCircleProperty = DependencyProperty.Register("IsScrollingOnCircle", typeof(bool), typeof(SpinEdit), new PropertyMetadata(true, new PropertyChangedCallback(SpinEdit.OnIsScrollingOnClicleChanged)));
			SpinEdit.IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(SpinEdit), new PropertyMetadata(false));
			SpinEdit.NumberDecimalDigitsProperty = DependencyProperty.Register("NumberDecimalDigits", typeof(int), typeof(SpinEdit), new UIPropertyMetadata(-1));
			SpinEdit.GroupSeperatorEnabledProperty = DependencyProperty.Register("GroupSeperatorEnabled", typeof(bool), typeof(SpinEdit), new UIPropertyMetadata(false));
			SpinEdit.IsFocusedProperty = DependencyProperty.Register("IsFocused", typeof(bool), typeof(SpinEdit), new UIPropertyMetadata(false));
			SpinEdit.NullValueTextProperty = DependencyProperty.Register("NullValueText", typeof(string), typeof(SpinEdit), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(SpinEdit.OnNullValueTextChanged)));
			SpinEdit.AnimationShiftProperty = DependencyProperty.Register("AnimationShift", typeof(double), typeof(SpinEdit), new UIPropertyMetadata(0.0));
			SpinEdit.IsValueNegativeProperty = DependencyProperty.Register("IsValueNegative", typeof(bool), typeof(SpinEdit), new PropertyMetadata(false, new PropertyChangedCallback(SpinEdit.OnIsValueNegativeChanged)));
			SpinEdit.CursorBackgroundProperty = DependencyProperty.Register("CursorBackground", typeof(Brush), typeof(SpinEdit), new PropertyMetadata(null));
			SpinEdit.CursorBorderBrushProperty = DependencyProperty.Register("CursorBorderBrush", typeof(Brush), typeof(SpinEdit), new PropertyMetadata(null));
			SpinEdit.CursorWidthProperty = DependencyProperty.Register("CursorWidth", typeof(double), typeof(SpinEdit), new PropertyMetadata(null));
			SpinEdit.CursorBorderThicknessProperty = DependencyProperty.Register("CursorBorderThickness", typeof(Thickness), typeof(SpinEdit), new PropertyMetadata(null));
			SpinEdit.CursorTemplateProperty = DependencyProperty.Register("CursorTemplate", typeof(ControlTemplate), typeof(SpinEdit), new PropertyMetadata(null, new PropertyChangedCallback(SpinEdit.OnCursorTemplateChanged)));
			SpinEdit.CursorVisibleProperty = DependencyProperty.Register("CursorVisible", typeof(bool), typeof(SpinEdit), new PropertyMetadata(null));
			SpinEdit.CursorPositionProperty = DependencyProperty.Register("CursorPosition", typeof(Thickness), typeof(SpinEdit), new PropertyMetadata(null));
			SpinEdit.SelectionBrushProperty = DependencyProperty.Register("SelectionBrush", typeof(Brush), typeof(SpinEdit), new PropertyMetadata(null));
			SpinEdit.EnableRangeAdornerProperty = DependencyProperty.Register("EnableRangeAdorner", typeof(bool), typeof(SpinEdit), new PropertyMetadata(false));
			SpinEdit.RangeAdornerBackgroundProperty = DependencyProperty.Register("RangeAdornerBackground", typeof(Brush), typeof(SpinEdit), new PropertyMetadata(Brushes.LightGray));
			SpinEdit.EnableExtendedScrollingProperty = DependencyProperty.Register("EnableExtendedScrolling", typeof(bool), typeof(SpinEdit), new PropertyMetadata(false));
			SpinEdit.EnableTouchProperty = DependencyProperty.Register("EnableTouch", typeof(bool), typeof(SpinEdit), new PropertyMetadata(false));
			SpinEdit.SpinEditForegroundProperty = DependencyProperty.Register("SpinEditForeground", typeof(Brush), typeof(SpinEdit), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));
			SpinEdit.SpinEditBackgroundProperty = DependencyProperty.Register("SpinEditBackground", typeof(Brush), typeof(SpinEdit), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));
			SpinEdit.SpinEditBorderBrushProperty = DependencyProperty.Register("SpinEditBorderBrush", typeof(Brush), typeof(SpinEdit), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));
			SpinEdit.TextAlignmentProperty = DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(SpinEdit), new PropertyMetadata(TextAlignment.Center));
			SpinEdit.ApplyZeroColorProperty = DependencyProperty.Register("ApplyZeroColor", typeof(bool), typeof(SpinEdit), new PropertyMetadata(true));
			SpinEdit.EnableNegativeColorsProperty = DependencyProperty.Register("EnableNegativeColors", typeof(bool), typeof(SpinEdit), new PropertyMetadata(true));
			SpinEdit.CultureProperty = DependencyProperty.Register("Culture", typeof(CultureInfo), typeof(SpinEdit), new PropertyMetadata(CultureInfo.CurrentCulture));
			SpinEdit.EnableFocusedColorsProperty = DependencyProperty.Register("EnableFocusedColors", typeof(bool), typeof(SpinEdit), new PropertyMetadata(false));
			SpinEdit.FocusedBackgroundProperty = DependencyProperty.Register("FocusedBackground", typeof(Brush), typeof(SpinEdit), new PropertyMetadata(Brushes.White, new PropertyChangedCallback(SpinEdit.OnFocusedBackgroundChanged)));
			SpinEdit.IsSpinEditFocusedProperty = DependencyProperty.Register("IsSpinEditFocused", typeof(bool), typeof(SpinEdit));
			SpinEdit.FocusedForegroundProperty = DependencyProperty.Register("FocusedForeground", typeof(Brush), typeof(SpinEdit), new PropertyMetadata(Brushes.Black, new PropertyChangedCallback(SpinEdit.OnFocusedForegroundChanged)));
			SpinEdit.FocusedBorderBrushProperty = DependencyProperty.Register("FocusedBorderBrush", typeof(Brush), typeof(SpinEdit), new PropertyMetadata(Brushes.Black, new PropertyChangedCallback(SpinEdit.OnFocusedBorderBrushChanged)));
			SpinEdit.NegativeBackgroundProperty = DependencyProperty.Register("NegativeBackground", typeof(Brush), typeof(SpinEdit), new PropertyMetadata(null));
			SpinEdit.NegativeBorderBrushProperty = DependencyProperty.Register("NegativeBorderBrush", typeof(Brush), typeof(SpinEdit), new PropertyMetadata(null));
			SpinEdit.AllowEditProperty = DependencyProperty.Register("AllowEdit", typeof(bool), typeof(SpinEdit), new PropertyMetadata(true, new PropertyChangedCallback(SpinEdit.OnAllowEditChanged)));
			SpinEdit.MinValidationProperty = DependencyProperty.Register("MinValidation", typeof(MinValidation), typeof(SpinEdit), new PropertyMetadata(MinValidation.OnKeyPress, new PropertyChangedCallback(SpinEdit.OnMinValidationChanged)));
			SpinEdit.MaxValidationProperty = DependencyProperty.Register("MaxValidation", typeof(MaxValidation), typeof(SpinEdit), new PropertyMetadata(MaxValidation.OnKeyPress, new PropertyChangedCallback(SpinEdit.OnMaxValidationChanged)));
			SpinEdit.MinValueOnExceedMinDigitProperty = DependencyProperty.Register("MinValueOnExceedMinDigit", typeof(bool), typeof(SpinEdit), new PropertyMetadata(true));
			SpinEdit.MaxValueOnExceedMaxDigitProperty = DependencyProperty.Register("MaxValueOnExceedMaxDigit", typeof(bool), typeof(SpinEdit), new PropertyMetadata(true));
			SpinEdit.NegativeForegroundProperty = DependencyProperty.Register("NegativeForeground", typeof(Brush), typeof(SpinEdit), new PropertyMetadata(new SolidColorBrush(Colors.Red), new PropertyChangedCallback(SpinEdit.OnNegativeForegroundChanged)));
			SpinEdit.ZeroColorProperty = DependencyProperty.Register("ZeroColor", typeof(Brush), typeof(SpinEdit), new PropertyMetadata(new SolidColorBrush(Colors.Green), new PropertyChangedCallback(SpinEdit.OnZeroColorChanged)));
			SpinEdit.UseNullOptionProperty = DependencyProperty.Register("UseNullOption", typeof(bool), typeof(SpinEdit), new PropertyMetadata(false, new PropertyChangedCallback(SpinEdit.OnUseNullOptionChanged)));
			SpinEdit.NumberFormatInfoProperty = DependencyProperty.Register("NumberFormatInfo", typeof(NumberFormatInfo), typeof(SpinEdit), new PropertyMetadata(null, new PropertyChangedCallback(SpinEdit.OnNumberFormatInfoChanged)));
			SpinEdit.CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(SpinEdit), new PropertyMetadata(null));
			SpinEdit.NullValueProperty = DependencyProperty.Register("NullValue", typeof(double?), typeof(SpinEdit), new PropertyMetadata(null));
			SpinEdit.ValueProperty = DependencyProperty.Register("Value", typeof(double?), typeof(SpinEdit), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(SpinEdit.OnValueChanged), new CoerceValueCallback(SpinEdit.CoerceValue)));
			SpinEdit.MinValueProperty = DependencyProperty.Register("MinValue", typeof(double), typeof(SpinEdit), new PropertyMetadata(-1.7976931348623157E+308, new PropertyChangedCallback(SpinEdit.OnMinValueChanged)));
			SpinEdit.MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(double), typeof(SpinEdit), new PropertyMetadata(1.7976931348623157E+308, new PropertyChangedCallback(SpinEdit.OnMaxValueChanged)));
			SpinEdit.StepProperty = DependencyProperty.Register("Step", typeof(double), typeof(SpinEdit), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(SpinEdit.OnStepChanged)));
			SpinEdit.AnimationSpeedProperty = DependencyProperty.Register("AnimationSpeed", typeof(double), typeof(SpinEdit), new PropertyMetadata(0.1));
			SpinEdit.ShowButtonProperty = DependencyProperty.Register("ShowButton", typeof(bool), typeof(SpinEdit), new PropertyMetadata(true, OnShowButtonChanged));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(SpinEdit), new FrameworkPropertyMetadata(typeof(SpinEdit)));
			SpinEdit.m_downValue = new RoutedCommand();
			SpinEdit.m_upValue = new RoutedCommand();
		}

		public SpinEdit()
		{
			base.DefaultStyleKey = typeof(SpinEdit);
			CommandBinding commandBinding = new CommandBinding(SpinEdit.m_downValue);
			commandBinding.Executed += new ExecutedRoutedEventHandler(this.ChangeDownValue);
			CommandBinding commandBinding2 = new CommandBinding(SpinEdit.m_upValue);
			commandBinding2.Executed += new ExecutedRoutedEventHandler(this.ChangeUpValue);
			base.CommandBindings.Add(commandBinding);
			base.CommandBindings.Add(commandBinding2);
			base.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(this.SpinEdit_LostKeyboardFocus);
		}
		private void SpinEdit_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			if(this.MaxValidation == MaxValidation.OnLostFocus)
			{
				NumberFormatInfo numberFormat = this.textbox.GetCulture().NumberFormat;
				double num;
				if(this.textbox != null && double.TryParse(this.textbox.MaskedText, out num))
				{
					double? value = this.Value;
					double maxValue = this.MaxValue;
					if(value.GetValueOrDefault() <= maxValue || !value.HasValue)
					{
						double? value2 = this.textbox.Value;
						double maxValue2 = this.MaxValue;
						if(value2.GetValueOrDefault() <= maxValue2 || !value2.HasValue)
						{
							double? value3 = this.Value;
							double num2 = double.Parse(this.textbox.MaskedText, numberFormat);
							if(value3.GetValueOrDefault() >= num2 || !value3.HasValue)
							{
								goto IL_12C;
							}
						}
					}
					this.Value = new double?(this.MaxValue);
					if(this.textbox != null)
					{
						this.textbox.Value = this.Value;
						NumberFormatInfo numberFormat2 = this.textbox.GetCulture().NumberFormat;
						this.textbox.MaskedText = this.Value.Value.ToString("N", numberFormat2);
					}
				}
			}
			IL_12C:
			double num3;
			if(this.MinValidation == MinValidation.OnLostFocus && this.textbox != null && double.TryParse(this.textbox.MaskedText, out num3))
			{
				double? value4 = this.Value;
				double minValue = this.MinValue;
				if(value4.GetValueOrDefault() >= minValue || !value4.HasValue)
				{
					double? value5 = this.textbox.Value;
					double minValue2 = this.MinValue;
					if(value5.GetValueOrDefault() >= minValue2 || !value5.HasValue)
					{
						double? value6 = this.Value;
						double num4 = double.Parse(this.textbox.MaskedText);
						if(value6.GetValueOrDefault() <= num4 || !value6.HasValue)
						{
							return;
						}
					}
				}
				this.Value = new double?(this.MinValue);
				if(this.textbox != null)
				{
					this.textbox.Value = this.Value;
					NumberFormatInfo numberFormat3 = this.textbox.GetCulture().NumberFormat;
					this.textbox.MaskedText = this.Value.Value.ToString("N", numberFormat3);
				}
			}
		}
		private static void OnFocusedBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SpinEdit SpinEdit = (SpinEdit)d;
			SpinEdit.OnFocusedBackgroundChanged(e);
		}
		private static void OnFocusedForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SpinEdit SpinEdit = (SpinEdit)d;
			SpinEdit.OnFocusedForegroundChanged(e);
		}
		private static void OnFocusedBorderBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SpinEdit SpinEdit = (SpinEdit)d;
			SpinEdit.OnFocusedBorderBrushChanged(e);
		}
		private static void OnIsValueNegativeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SpinEdit SpinEdit = (SpinEdit)d;
			SpinEdit.OnIsValueNegativeChanged(e);
		}
		private static void OnNullValueTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SpinEdit SpinEdit = (SpinEdit)d;
			SpinEdit.OnNullValueTextChanged(e);
		}
		private static void OnCursorTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SpinEdit SpinEdit = (SpinEdit)d;
			SpinEdit.OnCursorTemplateChanged(e);
		}
		private static void OnAllowEditChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SpinEdit SpinEdit = (SpinEdit)d;
			SpinEdit.OnAllowEditChanged(e);
		}
		private static void OnNegativeForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SpinEdit SpinEdit = (SpinEdit)d;
			SpinEdit.OnNegativeForegroundChanged(e);
		}
		private static void OnMinValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SpinEdit SpinEdit = (SpinEdit)d;
			SpinEdit.OnMinValueChanged(e);
		}
		private static void OnMaxValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SpinEdit SpinEdit = (SpinEdit)d;
			if(SpinEdit != null)
			{
				SpinEdit.OnMaxValueChanged(e);
			}
		}
		private static void OnNumberFormatInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SpinEdit SpinEdit = (SpinEdit)d;
			if(SpinEdit != null)
			{
				SpinEdit.OnNumberFormatInfoChanged(e);
			}
		}
		private static void OnZeroColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SpinEdit SpinEdit = (SpinEdit)d;
			if(SpinEdit != null)
			{
				SpinEdit.OnZeroColorChanged(e);
			}
		}
		private static void OnIsScrollingOnClicleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SpinEdit SpinEdit = (SpinEdit)d;
			if(SpinEdit != null)
			{
				SpinEdit.OnIsScrollingOnClicleChanged(e);
			}
		}
		private static void OnMaxValidationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SpinEdit SpinEdit = (SpinEdit)d;
			if(SpinEdit != null)
			{
				SpinEdit.OnMaxValidationChanged(e);
			}
		}
		private static void OnMinValidationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SpinEdit SpinEdit = (SpinEdit)d;
			if(SpinEdit != null)
			{
				SpinEdit.OnMinValidationChanged(e);
			}
		}
		private static void OnUseNullOptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SpinEdit SpinEdit = (SpinEdit)d;
			if(SpinEdit != null)
			{
				SpinEdit.OnUseNullOptionChanged(e);
			}
		}
		private static void OnStepChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SpinEdit SpinEdit = (SpinEdit)d;
			if(SpinEdit != null)
			{
				SpinEdit.OnStepChanged(e);
			}
		}
		private static void OnShowButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SpinEdit SpinEdit = (SpinEdit)d;
			if(SpinEdit != null)
			{
				SpinEdit.OnShowButtonChanged(e);
			}
		}
		private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SpinEdit SpinEdit = (SpinEdit)d;
			if(SpinEdit != null)
			{
				SpinEdit.OnValueChanged(e);
			}
		}
		protected virtual void OnFocusedBackgroundChanged(DependencyPropertyChangedEventArgs e)
		{
			if(this.FocusedBackgroundChanged != null && e.OldValue.ToString() != e.NewValue.ToString())
			{
				this.FocusedBackgroundChanged(this, e);
			}
		}
		protected virtual void OnFocusedForegroundChanged(DependencyPropertyChangedEventArgs e)
		{
			if(this.FocusedForegroundChanged != null && e.OldValue.ToString() != e.NewValue.ToString())
			{
				this.FocusedForegroundChanged(this, e);
			}
		}
		protected virtual void OnFocusedBorderBrushChanged(DependencyPropertyChangedEventArgs e)
		{
			if(this.FocusedBorderBrushChanged != null && e.OldValue.ToString() != e.NewValue.ToString())
			{
				this.FocusedBorderBrushChanged(this, e);
			}
		}
		protected virtual void OnCursorTemplateChanged(DependencyPropertyChangedEventArgs e)
		{
			if(this.CursorTemplateChanged != null)
			{
				this.CursorTemplateChanged(this, e);
			}
		}
		protected virtual void OnIsValueNegativeChanged(DependencyPropertyChangedEventArgs e)
		{
			if(this.IsValueNegativeChanged != null)
			{
				this.IsValueNegativeChanged(this, e);
			}
		}
		protected virtual void OnNullValueTextChanged(DependencyPropertyChangedEventArgs e)
		{
			if(this.NullValueTextChanged != null)
			{
				this.NullValueTextChanged(this, e);
			}
		}
		private static object CoerceValue(DependencyObject d, object baseValue)
		{
			SpinEdit SpinEdit = (SpinEdit)d;
			if(baseValue != null && SpinEdit != null)
			{
				SpinEdit.nagativevaluechanged = false;
				double? num = (double?)baseValue;
				if(SpinEdit.NumberDecimalDigits > 0)
				{
					double value = num.Value;
					return System.Math.Round(value, SpinEdit.NumberDecimalDigits);
				}
				double? num2 = num;
				double minValue = SpinEdit.MinValue;
				if(num2.GetValueOrDefault() < minValue && num2.HasValue && SpinEdit.MinValidation == MinValidation.OnKeyPress)
				{
					SpinEdit.nagativevaluechanged = true;
					SpinEdit.m_oldvalue = num;
					return SpinEdit.MinValue;
				}
				double? num3 = num;
				double maxValue = SpinEdit.MaxValue;
				if(num3.GetValueOrDefault() > maxValue && num3.HasValue && SpinEdit.MaxValidation == MaxValidation.OnKeyPress)
				{
					return SpinEdit.MaxValue;
				}
				return num;
			}
			else
			{
				if(SpinEdit != null && SpinEdit.UseNullOption)
				{
					return SpinEdit.NullValue;
				}
				return baseValue;
			}
		}
		protected virtual void OnAllowEditChanged(DependencyPropertyChangedEventArgs e)
		{
			this.setAllowEditProperty();
			if(this.AllowEditChanged != null)
			{
				this.AllowEditChanged(this, e);
			}
		}
		protected virtual void OnNegativeForegroundChanged(DependencyPropertyChangedEventArgs e)
		{
			if(this.textbox != null)
			{
				double? value = this.Value;
				if(value.GetValueOrDefault() < 0.0 && value.HasValue)
				{
					this.textbox.Foreground = this.NegativeForeground;
				}
				this.textbox.NegativeForeground = this.NegativeForeground;
			}
			if(this.NegativeForegroundChanged != null)
			{
				this.NegativeForegroundChanged(this, e);
			}
		}
		private void OnIsScrollingOnClicleChanged(DependencyPropertyChangedEventArgs e)
		{
			if(this.IsScrollingOnCircleChanged != null)
			{
				this.IsScrollingOnCircleChanged(this, e);
			}
		}
		protected virtual void OnMinValueChanged(DependencyPropertyChangedEventArgs e)
		{
			if(this.MinValueChanged != null)
			{
				this.MinValueChanged(this, e);
			}
		}
		protected virtual void OnMaxValueChanged(DependencyPropertyChangedEventArgs e)
		{
			if(this.MaxValueChanged != null)
			{
				this.MaxValueChanged(this, e);
			}
		}
		protected virtual void OnNumberFormatInfoChanged(DependencyPropertyChangedEventArgs e)
		{
			if(this.NumberFormatInfoChanged != null)
			{
				this.NumberFormatInfoChanged(this, e);
			}
		}
		protected virtual void OnZeroColorChanged(DependencyPropertyChangedEventArgs e)
		{
			if(this.ZeroColorChanged != null)
			{
				this.ZeroColorChanged(this, e);
			}
		}
		protected virtual void OnMaxValidationChanged(DependencyPropertyChangedEventArgs e)
		{
			if(this.MaxValidationChanged != null)
			{
				this.MaxValidationChanged(this, e);
			}
		}
		protected virtual void OnMinValidationChanged(DependencyPropertyChangedEventArgs e)
		{
			if(this.MinValidationChanged != null)
			{
				this.MinValidationChanged(this, e);
			}
		}
		protected virtual void OnUseNullOptionChanged(DependencyPropertyChangedEventArgs e)
		{
			if(this.UseNullOptionChanged != null)
			{
				this.UseNullOptionChanged(this, e);
			}
		}
		protected virtual void OnStepChanged(DependencyPropertyChangedEventArgs e)
		{
			if(this.textbox != null)
			{
				this.textbox.ScrollInterval = this.Step;
			}
			if(this.StepChanged != null)
			{
				this.StepChanged(this, e);
			}
		}
		protected virtual void OnShowButtonChanged(DependencyPropertyChangedEventArgs e)
		{
			if(e.NewValue is bool value)
			{
				UpdateShowButton(value);
			}
		}
		private void UpdateShowButton(bool value)
		{
			if(Upbutton != null)
			{
				Upbutton.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
			}

			if(Downbutton != null)
			{
				Downbutton.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
			}
		}
		protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
		{
			this.m_exvalue = (double?)e.OldValue;
			this.m_value = (double?)e.NewValue;
			double num = 0.0;
			double? value = this.m_value;
			double minValue = this.MinValue;
			if(value.GetValueOrDefault() < minValue && value.HasValue)
			{
				num = this.MinValue;
			}
			double? value2 = this.m_value;
			double maxValue = this.MaxValue;
			if(value2.GetValueOrDefault() > maxValue && value2.HasValue)
			{
				num = this.MaxValue;
			}
			if(!this.UseNullOption && !this.Value.HasValue)
			{
				this.Value = new double?(num);
			}
			if(num < 0.0)
			{
				double? value3 = this.Value;
				if(value3.GetValueOrDefault() < 0.0 && value3.HasValue && this.EnableNegativeColors && this.textbox != null)
				{
					this.textbox.Foreground = this.NegativeForeground;
					this.textbox.NegativeForeground = this.NegativeForeground;
				}
			}
			this.UpdateBackground();
			if(this.Nulltextbox != null)
			{
				this.Nulltextbox.Text = this.NullValueText;
			}
			if(this.Upbutton != null && this.Downbutton != null && (this.Upbutton.IsPressed || this.Downbutton.IsPressed))
			{
				this.Animation();
			}
			if(this.ValueChanged != null)
			{
				this.ValueChanged(this, e);
			}
		}
		private void ChangeUpValue(object sender, ExecutedRoutedEventArgs e)
		{
			this.ChangeValue(true);
		}
		private void ChangeDownValue(object sender, ExecutedRoutedEventArgs e)
		{
			this.ChangeValue(false);
		}
		private void ChangeValue(bool IsUp)
		{
			if(this.textbox != null)
			{
				this.textbox.ScrollInterval = this.Step;
				if(this.Value.HasValue && !double.IsNaN(this.Value.Value))
				{
					if(IsUp)
					{
						double? value = this.Value;
						double step = this.Step;
						double? num = value.HasValue ? new double?(value.GetValueOrDefault() + step) : null;
						double maxValue = this.MaxValue;
						if(num.GetValueOrDefault() <= maxValue && num.HasValue)
						{
							if(DoubleValueHandler.DoubleValHandler != null)
							{
								DoubleValueHandler.DoubleValHandler.HandleUpKey(this.textbox);
								return;
							}
						}
						else
						{
							double? value2 = this.Value;
							double maxValue2 = this.MaxValue;
							if(value2.GetValueOrDefault() != maxValue2 || !value2.HasValue)
							{
								this.Value = new double?(this.MaxValue);
								return;
							}
						}
					}
					else
					{
						double? value3 = this.Value;
						double step2 = this.Step;
						double? num2 = value3.HasValue ? new double?(value3.GetValueOrDefault() - step2) : null;
						double minValue = this.MinValue;
						if(num2.GetValueOrDefault() >= minValue && num2.HasValue)
						{
							if(DoubleValueHandler.DoubleValHandler != null)
							{
								DoubleValueHandler.DoubleValHandler.HandleDownKey(this.textbox);
								return;
							}
						}
						else
						{
							double? value4 = this.Value;
							double minValue2 = this.MinValue;
							if(value4.GetValueOrDefault() != minValue2 || !value4.HasValue)
							{
								this.Value = new double?(this.MinValue);
							}
						}
					}
				}
			}
		}
		private void UpdateBackground()
		{
			if(this.textbox != null)
			{
				if(this.Downbutton != null && this.Upbutton != null && (this.Upbutton.IsPressed || this.Downbutton.IsPressed || this.textbox.IsFocused) && this.EnableFocusedColors)
				{
					this.IsSpinEditFocused = true;
					return;
				}
				if(this.Downbutton != null && this.Upbutton != null && !this.Upbutton.IsPressed && !this.Downbutton.IsPressed && !this.textbox.IsFocused && this.EnableFocusedColors)
				{
					this.IsSpinEditFocused = false;
					return;
				}
				double? value = this.Value;
				if(value.GetValueOrDefault() < 0.0 && value.HasValue && this.EnableNegativeColors && this.NegativeBackground != null)
				{
					this.IsValueNegative = true;
					return;
				}
				double? value2 = this.Value;
				if(value2.GetValueOrDefault() > 0.0 && value2.HasValue)
				{
					this.IsValueNegative = false;
				}
			}
		}
		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			base.OnPreviewKeyDown(e);
			if(ModifierKeys.Shift == Keyboard.Modifiers && e.Key == Key.Tab)
			{
				this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
				e.Handled = true;
			}
			if(this.textbox != null)
			{
				this.textbox.ScrollInterval = this.Step;
			}
			if(Keyboard.IsKeyDown(Key.Delete) && this.UseNullOption && !this.Value.HasValue)
			{
				base.SetValue(SpinEdit.ValueProperty, this.NullValue);
			}
		}
		private void setAllowEditProperty()
		{
			if(this.textbox != null)
			{
				if(this.AllowEdit)
				{
					this.textbox.IsReadOnly = false;
				}
				if(!this.AllowEdit)
				{
					this.textbox.IsReadOnly = true;
				}
			}
		}
		private void SpinEdit_LostFocus(object sender, RoutedEventArgs e)
		{
			this.IsFocused = false;
			if(this.EnableFocusedColors)
			{
				this.IsSpinEditFocused = false;
			}
			DoubleTextBox doubleTextBox = this.textbox;
			if(doubleTextBox != null && doubleTextBox.IsNegative && this.EnableNegativeColors && this.NegativeBackground != null)
			{
				this.IsValueNegative = true;
			}
			if(this.nagativevaluechanged)
			{
				double? oldvalue = this.m_oldvalue;
				double minValue = this.MinValue;
				if(oldvalue.GetValueOrDefault() < minValue && oldvalue.HasValue)
				{
					this.textbox.MaskedText = this.MinValue.ToString();
					this.Value = new double?(this.MinValue);
				}
			}
		}
		private void SpinEdit_GotFocus(object sender, RoutedEventArgs e)
		{
			this.IsFocused = true;
			if(this.EnableFocusedColors)
			{
				this.IsSpinEditFocused = true;
			}
			if(!this.UseNullOption || this.Value.HasValue)
			{
				DoubleTextBox doubleTextBox = this.textbox;
				if(doubleTextBox != null && !doubleTextBox.IsNegative)
				{
					this.IsValueNegative = false;
				}
				DoubleTextBox doubleTextBox2 = this.textbox;
				if(doubleTextBox2 != null && doubleTextBox2.Text != null && doubleTextBox2.TextSelectionOnFocus && this.IsFocused)
				{
					e.Handled = true;
					this.textbox.Focus();
				}
			}
			if(this.UseNullOption && !this.Value.HasValue && this.IsFocused)
			{
				e.Handled = true;
				this.Nulltextbox.Focus();
			}
		}
		private void Animation()
		{
			if(base.IsLoaded && this.textbox != null && this.secondBlock != null && this.t1 != null)
			{
				this.textbox.Visibility = Visibility.Visible;
				this.textbox.Opacity = 1.0;
				this.secondBlock.Opacity = 1.0;
				this.secondBlock.Visibility = Visibility.Visible;
				this.secondBlock.Background = this.textbox.Background;
				this.textbox.RenderTransform = new TranslateTransform();
				this.secondBlock.RenderTransform = new TranslateTransform();
				this.t1.RenderTransform = new TranslateTransform();
				this.secondBlock.Foreground = this.textbox.Foreground;
				this.t1.Foreground = this.textbox.Foreground;
				Storyboard storyboard = new Storyboard();
				DoubleAnimation doubleAnimation = new DoubleAnimation();
				TranslateTransform translateTransform = (TranslateTransform)this.textbox.RenderTransform;
				DoubleAnimation doubleAnimation2 = new DoubleAnimation();
				TranslateTransform translateTransform2 = (TranslateTransform)this.secondBlock.RenderTransform;
				DoubleAnimation doubleAnimation3 = new DoubleAnimation();
				TranslateTransform translateTransform3 = (TranslateTransform)this.t1.RenderTransform;
				storyboard.Children.Add(doubleAnimation);
				storyboard.Children.Add(doubleAnimation2);
				storyboard.Children.Add(doubleAnimation3);
				if(this.Upbutton != null && this.Upbutton.IsPressed)
				{
					this.t1.Visibility = Visibility.Visible;
					this.t1.Background = this.textbox.Background;
					if(this.NumberDecimalDigits >= 0)
					{
						this.t1.NumberDecimalDigits = this.NumberDecimalDigits;
						this.secondBlock.NumberDecimalDigits = this.NumberDecimalDigits;
					}
					this.t1.Value = this.m_exvalue;
					this.secondBlock.Value = this.m_exvalue;
					doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(this.AnimationSpeed));
					doubleAnimation.From = new double?(this.border.ActualHeight);
					doubleAnimation.To = new double?(0.0);
					translateTransform.BeginAnimation(TranslateTransform.YProperty, doubleAnimation);
					doubleAnimation3.Duration = new Duration(TimeSpan.FromSeconds(this.AnimationSpeed));
					doubleAnimation3.From = new double?(0.0);
					doubleAnimation3.To = new double?(-this.border.ActualHeight);
					translateTransform3.BeginAnimation(TranslateTransform.YProperty, doubleAnimation3);
					this.secondBlock.Visibility = Visibility.Collapsed;
				}
				else
				{
					if(this.Downbutton != null && this.Downbutton.IsPressed)
					{
						this.secondBlock.Visibility = Visibility.Visible;
						this.secondBlock.Background = this.textbox.Background;
						if(this.NumberDecimalDigits >= 0)
						{
							this.secondBlock.NumberDecimalDigits = this.NumberDecimalDigits;
						}
						this.t1.Value = this.m_exvalue;
						this.secondBlock.Value = this.m_exvalue;
						this.textbox.Visibility = Visibility.Visible;
						doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(this.AnimationSpeed));
						doubleAnimation.From = new double?(-this.border.ActualHeight);
						doubleAnimation.To = new double?(0.0);
						translateTransform.BeginAnimation(TranslateTransform.YProperty, doubleAnimation);
						doubleAnimation2.Duration = new Duration(TimeSpan.FromSeconds(this.AnimationSpeed));
						doubleAnimation2.From = new double?(0.0);
						doubleAnimation2.To = new double?(this.border.ActualHeight);
						translateTransform2.BeginAnimation(TranslateTransform.YProperty, doubleAnimation2);
						this.t1.Visibility = Visibility.Collapsed;
					}
				}
				this.m_exvalue = this.m_value;
			}
		}
		public void SelectAll()
		{
			if(this.textbox != null)
			{
				this.textbox.Focus();
				this.textbox.SelectAll();
			}
		}
		public override void OnApplyTemplate()
		{
			if(this.textbox != null)
			{
				this.textbox.ValueChanging -= new DoubleTextBox.ValueChangingEventHandler(this.textbox_ValueChanging);
			}
			base.GotFocus -= new RoutedEventHandler(this.SpinEdit_GotFocus);
			base.LostFocus -= new RoutedEventHandler(this.SpinEdit_LostFocus);
			base.OnApplyTemplate();
			this.Nulltextbox = (TextBox)base.GetTemplateChild("text");
			this.textbox = (DoubleTextBox)base.GetTemplateChild("DoubleTextBox");
			if(this.textbox != null)
			{
				this.textbox.isUpDownDoubleTextBox = true;
			}
			this.Upbutton = (RepeatButton)base.GetTemplateChild("upbutton");
			this.Downbutton = (RepeatButton)base.GetTemplateChild("downbutton");
			this.border = (Border)base.GetTemplateChild("Border");
			this.t1 = (DoubleTextBox)base.GetTemplateChild("textBox");
			this.secondBlock = (DoubleTextBox)base.GetTemplateChild("SecondBlock");
			base.GotFocus += new RoutedEventHandler(this.SpinEdit_GotFocus);
			base.LostFocus += new RoutedEventHandler(this.SpinEdit_LostFocus);
			this.SpinEditForeground = base.Foreground;
			if(this.textbox != null)
			{
				double? value = this.Value;
				if(value.GetValueOrDefault() < 0.0 && value.HasValue && this.EnableNegativeColors)
				{
					this.textbox.Foreground = this.NegativeForeground;
				}
				this.textbox.NegativeForeground = this.NegativeForeground;
				if(this.NumberFormatInfo != null)
				{
					string numberGroupSeparator = this.NumberFormatInfo.NumberGroupSeparator;
					if(!this.NumberFormatInfo.NumberGroupSeparator.Equals(string.Empty) && !char.IsLetterOrDigit(numberGroupSeparator, 0) && numberGroupSeparator.Length == 1)
					{
						this.textbox.NumberGroupSeparator = this.NumberFormatInfo.NumberGroupSeparator;
					}
					else
					{
						this.textbox.NumberGroupSeparator = this.Culture.NumberFormat.NumberGroupSeparator;
					}
				}
			}
			this.setAllowEditProperty();
			if(this.textbox != null)
			{
				this.textbox.ValueChanging += new DoubleTextBox.ValueChangingEventHandler(this.textbox_ValueChanging);
			}
			if(this.Nulltextbox != null)
			{
				this.Nulltextbox.PreviewTextInput += new TextCompositionEventHandler(this.Nulltextbox_PreviewTextInput);
			}

			UpdateShowButton(ShowButton);
		}
		private void Nulltextbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if(this.textbox != null)
			{
				e.Handled = DoubleValueHandler.DoubleValHandler.MatchWithMask(this.textbox, e.Text);
			}
			base.OnTextInput(e);
		}
		private void textbox_ValueChanging(object sender, ValueChangingEventArgs e)
		{
			if(this.ValueChanging != null)
			{
				this.ValueChanging(this, e);
			}
		}
	}
}