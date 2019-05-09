/**
 * ==============================================================================
 *
 * ClassName: EditorBase
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/3/23 14:12:33
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
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using HOTINST.COMMON.Controls.Core.Editors;

#pragma warning disable 1591

namespace HOTINST.COMMON.Controls.Controls.Editors
{
	public class EditorBase : TextBox
	{
		internal bool minusPressed;
		private AdornerLayer aLayer;
		private TextBoxSelectionAdorner txtSelectionAdorner1;
		private ExtendedScrollingAdorner vAdorner;
		public static readonly DependencyProperty ReadOnlyProperty = DependencyProperty.Register("ReadOnly", typeof(bool), typeof(EditorBase), new PropertyMetadata(false));
		public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(EditorBase), new PropertyMetadata(new PropertyChangedCallback(EditorBase.OnCornerRadiusChanged)));
		public static readonly DependencyProperty EnableTouchProperty = DependencyProperty.Register("EnableTouch", typeof(bool), typeof(EditorBase), new PropertyMetadata(false, new PropertyChangedCallback(EditorBase.OnEnableTouchChanged)));
		public static readonly DependencyProperty EnableRangeAdornerProperty = DependencyProperty.Register("EnableRangeAdorner", typeof(bool), typeof(EditorBase), new PropertyMetadata(false));
		public static readonly DependencyProperty EnableExtendedScrollingProperty = DependencyProperty.Register("EnableExtendedScrolling", typeof(bool), typeof(EditorBase), new PropertyMetadata(false, new PropertyChangedCallback(EditorBase.OnEnableExtendedScrollingChanged)));
		public static readonly DependencyProperty RangeAdornerBackgroundProperty = DependencyProperty.Register("RangeAdornerBackground", typeof(Brush), typeof(EditorBase), new PropertyMetadata(Brushes.LightGray));
		public static readonly DependencyProperty FocusedBackgroundProperty = DependencyProperty.Register("FocusedBackground", typeof(Brush), typeof(EditorBase), new PropertyMetadata(new PropertyChangedCallback(EditorBase.OnFocusedBackgroundChanged)));
		public static readonly DependencyProperty FocusedForegroundProperty = DependencyProperty.Register("FocusedForeground", typeof(Brush), typeof(EditorBase), new PropertyMetadata(new PropertyChangedCallback(EditorBase.OnFocusedForegroundChanged)));
		public static readonly DependencyProperty FocusedBorderBrushProperty = DependencyProperty.Register("FocusedBorderBrush", typeof(Brush), typeof(EditorBase), new PropertyMetadata(new PropertyChangedCallback(EditorBase.OnFocusedBorderBrushChanged)));
		public static readonly DependencyProperty ReadOnlyBackgroundProperty = DependencyProperty.Register("ReadOnlyBackground", typeof(Brush), typeof(EditorBase), new PropertyMetadata(new PropertyChangedCallback(EditorBase.OnReadOnlyBackgroundChanged)));
		public static readonly DependencyProperty SelectionForegroundProperty = DependencyProperty.Register("SelectionForeground", typeof(Brush), typeof(EditorBase), new PropertyMetadata(new SolidColorBrush(Colors.Black)));
		public static readonly DependencyProperty EnableFocusColorsProperty = DependencyProperty.Register("EnableFocusColors", typeof(bool), typeof(EditorBase), new PropertyMetadata(false));
		internal bool allowchange;
		public static readonly DependencyProperty PositiveForegroundProperty = DependencyProperty.Register("PositiveForeground", typeof(Brush), typeof(EditorBase), new PropertyMetadata(new PropertyChangedCallback(EditorBase.OnPositiveForegroundChanged)));
		public static readonly DependencyProperty EditorForegroundProperty = DependencyProperty.Register("EditorForeground", typeof(Brush), typeof(EditorBase), new PropertyMetadata(new SolidColorBrush(Colors.Black), new PropertyChangedCallback(EditorBase.OnForegroundChanged)));
		public static readonly DependencyProperty NegativeForegroundProperty = DependencyProperty.Register("NegativeForeground", typeof(Brush), typeof(EditorBase), new PropertyMetadata(new SolidColorBrush(Colors.Red), new PropertyChangedCallback(EditorBase.OnNegativeForegroundChanged)));
		public static readonly DependencyProperty ApplyNegativeForegroundProperty = DependencyProperty.Register("ApplyNegativeForeground", typeof(bool), typeof(EditorBase), new PropertyMetadata(false, new PropertyChangedCallback(EditorBase.OnApplyNegativeForegroundChanged)));
		public static readonly DependencyProperty IsNegativeProperty = DependencyProperty.Register("IsNegative", typeof(bool), typeof(EditorBase), new PropertyMetadata(false, new PropertyChangedCallback(EditorBase.OnIsNegativeChanged)));
		public static readonly DependencyProperty IsZeroProperty = DependencyProperty.Register("IsZero", typeof(bool), typeof(EditorBase), new PropertyMetadata(false, new PropertyChangedCallback(EditorBase.OnIsZeroChanged)));
		public static readonly DependencyProperty MaxValidationProperty = DependencyProperty.Register("MaxValidation", typeof(MaxValidation), typeof(EditorBase), new PropertyMetadata(MaxValidation.OnKeyPress));
		public static readonly DependencyProperty MinValidationProperty = DependencyProperty.Register("MinValidation", typeof(MinValidation), typeof(EditorBase), new PropertyMetadata(MinValidation.OnKeyPress, new PropertyChangedCallback(EditorBase.OnMinValidationChanged)));
		public static readonly DependencyProperty MaxValueOnExceedMaxDigitProperty = DependencyProperty.Register("MaxValueOnExceedMaxDigit", typeof(bool), typeof(EditorBase), new PropertyMetadata(false));
		public static readonly DependencyProperty MinValueOnExceedMinDigitProperty = DependencyProperty.Register("MinValueOnExceedMinDigit", typeof(bool), typeof(EditorBase), new PropertyMetadata(false));
		public static readonly DependencyProperty IsNullProperty = DependencyProperty.Register("IsNull", typeof(bool), typeof(EditorBase), new PropertyMetadata(false, new PropertyChangedCallback(EditorBase.OnIsNullChanged)));
		public static readonly DependencyProperty UseNullOptionProperty = DependencyProperty.Register("UseNullOption", typeof(bool), typeof(EditorBase), new PropertyMetadata(false, new PropertyChangedCallback(EditorBase.OnUseNullOptionChanged)));
		public static readonly DependencyProperty CultureProperty = DependencyProperty.Register("Culture", typeof(CultureInfo), typeof(EditorBase), new PropertyMetadata(CultureInfo.CurrentCulture, new PropertyChangedCallback(EditorBase.OnCultureChanged)));
		public static readonly DependencyProperty ZeroColorProperty = DependencyProperty.Register("ZeroColor", typeof(Brush), typeof(EditorBase), new PropertyMetadata(new SolidColorBrush(Colors.Green), new PropertyChangedCallback(EditorBase.OnZeroNegativeColorChanged)));
		public static readonly DependencyProperty ApplyZeroColorProperty = DependencyProperty.Register("ApplyZeroColor", typeof(bool), typeof(EditorBase), new PropertyMetadata(false, new PropertyChangedCallback(EditorBase.OnApplyZeroColorChanged)));
		public static readonly DependencyProperty NumberFormatProperty = DependencyProperty.Register("NumberFormat", typeof(NumberFormatInfo), typeof(EditorBase), new PropertyMetadata(null, new PropertyChangedCallback(EditorBase.OnNumberFormatChanged)));
		public new static readonly DependencyProperty IsUndoEnabledProperty = DependencyProperty.Register("IsUndoEnabled", typeof(bool), typeof(EditorBase), new PropertyMetadata(true, new PropertyChangedCallback(EditorBase.OnIsUndoEnabledChanged)));
		public static readonly DependencyProperty MaskedTextProperty = DependencyProperty.Register("MaskedText", typeof(string), typeof(EditorBase), new PropertyMetadata(string.Empty));
		public static DependencyProperty WatermarkTemplateProperty = DependencyProperty.Register("WatermarkTemplate", typeof(DataTemplate), typeof(EditorBase), new PropertyMetadata(new PropertyChangedCallback(EditorBase.OnWaterMarkTemplateChanged)));
		public static DependencyProperty WatermarkTextProperty = DependencyProperty.Register("WatermarkText", typeof(string), typeof(EditorBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(EditorBase.OnWaterMarkTextChanged)));
		public static DependencyProperty WatermarkVisibilityProperty = DependencyProperty.Register("WatermarkVisibility", typeof(Visibility), typeof(EditorBase), new PropertyMetadata(Visibility.Collapsed, new PropertyChangedCallback(EditorBase.OnWatermarkVisibilityPropertyChanged), new CoerceValueCallback(EditorBase.CoerceWatermarkVisibility)));
		public static readonly DependencyProperty ContentElementVisibilityProperty = DependencyProperty.Register("ContentElementVisibility", typeof(Visibility), typeof(EditorBase), new PropertyMetadata(Visibility.Visible));
		public static readonly DependencyProperty WatermarkTextForegroundProperty = DependencyProperty.Register("WatermarkTextForeground", typeof(Brush), typeof(EditorBase), new PropertyMetadata(new SolidColorBrush()));
		public static readonly DependencyProperty WatermarkBackgroundProperty = DependencyProperty.Register("WatermarkBackground", typeof(Brush), typeof(EditorBase), new PropertyMetadata(new SolidColorBrush()));
		public static readonly DependencyProperty WatermarkOpacityProperty = DependencyProperty.Register("WatermarkOpacity", typeof(double), typeof(EditorBase), new PropertyMetadata(0.5));
		public static readonly DependencyProperty WatermarkTextIsVisibleProperty = DependencyProperty.Register("WatermarkTextIsVisible", typeof(bool), typeof(EditorBase), new PropertyMetadata(false));
		public static readonly DependencyProperty IsScrollingOnCircleProperty = DependencyProperty.Register("IsScrollingOnCircle", typeof(bool), typeof(EditorBase), new PropertyMetadata(true));
		public static readonly DependencyProperty EnterToMoveNextProperty = DependencyProperty.Register("EnterToMoveNext", typeof(bool), typeof(EditorBase), new PropertyMetadata(true, new PropertyChangedCallback(EditorBase.OnEnterToMoveNextChanged)));
		public static readonly DependencyProperty TextSelectionOnFocusProperty = DependencyProperty.Register("TextSelectionOnFocus", typeof(bool), typeof(EditorBase), new PropertyMetadata(true, new PropertyChangedCallback(EditorBase.OnTextSelectionOnFocusChanged)));
		public static readonly DependencyProperty IsCaretAnimationEnabledProperty = DependencyProperty.Register("IsCaretAnimationEnabled", typeof(bool), typeof(EditorBase), new PropertyMetadata(false));
		public static readonly DependencyProperty CaretIndexProperty = DependencyProperty.Register("CaretIndex", typeof(int), typeof(EditorBase), new PropertyMetadata(0));
		public event PropertyChangedCallback CultureChanged;
		public event PropertyChangedCallback NumberFormatChanged;
		public event PropertyChangedCallback WaterMarkTemplateChanged;
		public event PropertyChangedCallback WaterMarkTextChanged;
		public event PropertyChangedCallback IsUndoEnabledChanged;
		public event PropertyChangedCallback TextSelectionOnFocusChanged;
		public event PropertyChangedCallback NegativeForegroundChanged;
		public event PropertyChangedCallback IsValueNegativeChanged;
		public event PropertyChangedCallback EnterToMoveNextChanged;
		[Obsolete("Use IsReadOnly Property"), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
		public bool ReadOnly
		{
			get
			{
				return (bool)base.GetValue(EditorBase.ReadOnlyProperty);
			}
			set
			{
				base.SetValue(EditorBase.ReadOnlyProperty, value);
			}
		}
		public CornerRadius CornerRadius
		{
			get
			{
				return (CornerRadius)base.GetValue(EditorBase.CornerRadiusProperty);
			}
			set
			{
				base.SetValue(EditorBase.CornerRadiusProperty, value);
			}
		}
		public bool EnableTouch
		{
			get
			{
				return (bool)base.GetValue(EditorBase.EnableTouchProperty);
			}
			set
			{
				base.SetValue(EditorBase.EnableTouchProperty, value);
			}
		}
		public bool EnableRangeAdorner
		{
			get
			{
				return (bool)base.GetValue(EditorBase.EnableRangeAdornerProperty);
			}
			set
			{
				base.SetValue(EditorBase.EnableRangeAdornerProperty, value);
			}
		}
		public bool EnableExtendedScrolling
		{
			get
			{
				return (bool)base.GetValue(EditorBase.EnableExtendedScrollingProperty);
			}
			set
			{
				base.SetValue(EditorBase.EnableExtendedScrollingProperty, value);
			}
		}
		public Brush RangeAdornerBackground
		{
			get
			{
				return (Brush)base.GetValue(EditorBase.RangeAdornerBackgroundProperty);
			}
			set
			{
				base.SetValue(EditorBase.RangeAdornerBackgroundProperty, value);
			}
		}
		internal Brush FocusedBackground
		{
			get
			{
				return (Brush)base.GetValue(EditorBase.FocusedBackgroundProperty);
			}
			set
			{
				base.SetValue(EditorBase.FocusedBackgroundProperty, value);
			}
		}
		internal Brush FocusedForeground
		{
			get
			{
				return (Brush)base.GetValue(EditorBase.FocusedForegroundProperty);
			}
			set
			{
				base.SetValue(EditorBase.FocusedForegroundProperty, value);
			}
		}
		[Obsolete("Property will not help due to internal arhitecture changes")]
		public Brush FocusedBorderBrush
		{
			get
			{
				return (Brush)base.GetValue(EditorBase.FocusedBorderBrushProperty);
			}
			set
			{
				base.SetValue(EditorBase.FocusedBorderBrushProperty, value);
			}
		}
		[Obsolete("Property will not help due to internal arhitecture changes")]
		public Brush ReadOnlyBackground
		{
			get
			{
				return (Brush)base.GetValue(EditorBase.ReadOnlyBackgroundProperty);
			}
			set
			{
				base.SetValue(EditorBase.ReadOnlyBackgroundProperty, value);
			}
		}
		[Obsolete("Property will not help due to internal arhitecture changes")]
		public Brush SelectionForeground
		{
			get
			{
				return (Brush)base.GetValue(EditorBase.SelectionForegroundProperty);
			}
			set
			{
				base.SetValue(EditorBase.SelectionForegroundProperty, value);
			}
		}
		public bool EnableFocusColors
		{
			get
			{
				return (bool)base.GetValue(EditorBase.EnableFocusColorsProperty);
			}
			set
			{
				base.SetValue(EditorBase.EnableFocusColorsProperty, value);
			}
		}
		public CultureInfo Culture
		{
			get
			{
				return (CultureInfo)base.GetValue(EditorBase.CultureProperty);
			}
			set
			{
				base.SetValue(EditorBase.CultureProperty, value);
			}
		}
		public NumberFormatInfo NumberFormat
		{
			get
			{
				return (NumberFormatInfo)base.GetValue(EditorBase.NumberFormatProperty);
			}
			set
			{
				base.SetValue(EditorBase.NumberFormatProperty, value);
			}
		}
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public Brush EditorForeground
		{
			get
			{
				return (Brush)base.GetValue(EditorBase.EditorForegroundProperty);
			}
			set
			{
				base.SetValue(EditorBase.EditorForegroundProperty, value);
			}
		}
		public Brush PositiveForeground
		{
			get
			{
				return (Brush)base.GetValue(EditorBase.PositiveForegroundProperty);
			}
			set
			{
				base.SetValue(EditorBase.PositiveForegroundProperty, value);
			}
		}
		public bool ApplyNegativeForeground
		{
			get
			{
				return (bool)base.GetValue(EditorBase.ApplyNegativeForegroundProperty);
			}
			set
			{
				base.SetValue(EditorBase.ApplyNegativeForegroundProperty, value);
			}
		}
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public bool IsNegative
		{
			get
			{
				return (bool)base.GetValue(EditorBase.IsNegativeProperty);
			}
			set
			{
				base.SetValue(EditorBase.IsNegativeProperty, value);
			}
		}
		public Brush NegativeForeground
		{
			get
			{
				return (Brush)base.GetValue(EditorBase.NegativeForegroundProperty);
			}
			set
			{
				base.SetValue(EditorBase.NegativeForegroundProperty, value);
			}
		}
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public bool IsZero
		{
			get
			{
				return (bool)base.GetValue(EditorBase.IsZeroProperty);
			}
			set
			{
				base.SetValue(EditorBase.IsZeroProperty, value);
			}
		}
		public bool ApplyZeroColor
		{
			get
			{
				return (bool)base.GetValue(EditorBase.ApplyZeroColorProperty);
			}
			set
			{
				base.SetValue(EditorBase.ApplyZeroColorProperty, value);
			}
		}
		public Brush ZeroColor
		{
			get
			{
				return (Brush)base.GetValue(EditorBase.ZeroColorProperty);
			}
			set
			{
				base.SetValue(EditorBase.ZeroColorProperty, value);
			}
		}
		public bool UseNullOption
		{
			get
			{
				return (bool)base.GetValue(EditorBase.UseNullOptionProperty);
			}
			set
			{
				base.SetValue(EditorBase.UseNullOptionProperty, value);
			}
		}
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public bool IsNull
		{
			get
			{
				return (bool)base.GetValue(EditorBase.IsNullProperty);
			}
			set
			{
				base.SetValue(EditorBase.IsNullProperty, value);
			}
		}
		public MaxValidation MaxValidation
		{
			get
			{
				return (MaxValidation)base.GetValue(EditorBase.MaxValidationProperty);
			}
			set
			{
				base.SetValue(EditorBase.MaxValidationProperty, value);
			}
		}
		public MinValidation MinValidation
		{
			get
			{
				return (MinValidation)base.GetValue(EditorBase.MinValidationProperty);
			}
			set
			{
				base.SetValue(EditorBase.MinValidationProperty, value);
			}
		}
		public bool MaxValueOnExceedMaxDigit
		{
			get
			{
				return (bool)base.GetValue(EditorBase.MaxValueOnExceedMaxDigitProperty);
			}
			set
			{
				base.SetValue(EditorBase.MaxValueOnExceedMaxDigitProperty, value);
			}
		}
		public bool MinValueOnExceedMinDigit
		{
			get
			{
				return (bool)base.GetValue(EditorBase.MinValueOnExceedMinDigitProperty);
			}
			set
			{
				base.SetValue(EditorBase.MinValueOnExceedMinDigitProperty, value);
			}
		}
		public new bool IsUndoEnabled
		{
			get
			{
				return (bool)base.GetValue(EditorBase.IsUndoEnabledProperty);
			}
			set
			{
				base.SetValue(EditorBase.IsUndoEnabledProperty, value);
			}
		}
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public string MaskedText
		{
			get
			{
				return (string)base.GetValue(TextBox.TextProperty);
			}
			set
			{
				this.allowchange = false;
				base.SetValue(TextBox.TextProperty, value);
				base.SetValue(EditorBase.MaskedTextProperty, value);
			}
		}
		public DataTemplate WatermarkTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(EditorBase.WatermarkTemplateProperty);
			}
			set
			{
				base.SetValue(EditorBase.WatermarkTemplateProperty, value);
			}
		}
		public string WatermarkText
		{
			get
			{
				return (string)base.GetValue(EditorBase.WatermarkTextProperty);
			}
			set
			{
				base.SetValue(EditorBase.WatermarkTextProperty, value);
			}
		}
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public Visibility WatermarkVisibility
		{
			get
			{
				return (Visibility)base.GetValue(EditorBase.WatermarkVisibilityProperty);
			}
			set
			{
				base.SetValue(EditorBase.WatermarkVisibilityProperty, value);
			}
		}
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public Visibility ContentElementVisibility
		{
			get
			{
				return (Visibility)base.GetValue(EditorBase.ContentElementVisibilityProperty);
			}
			set
			{
				base.SetValue(EditorBase.ContentElementVisibilityProperty, value);
			}
		}
		public Brush WatermarkTextForeground
		{
			get
			{
				return (Brush)base.GetValue(EditorBase.WatermarkTextForegroundProperty);
			}
			set
			{
				base.SetValue(EditorBase.WatermarkTextForegroundProperty, value);
			}
		}
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public Brush WatermarkBackground
		{
			get
			{
				return (Brush)base.GetValue(EditorBase.WatermarkBackgroundProperty);
			}
			set
			{
				base.SetValue(EditorBase.WatermarkBackgroundProperty, value);
			}
		}
		public double WatermarkOpacity
		{
			get
			{
				return (double)base.GetValue(EditorBase.WatermarkOpacityProperty);
			}
			set
			{
				base.SetValue(EditorBase.WatermarkOpacityProperty, value);
			}
		}
		public bool WatermarkTextIsVisible
		{
			get
			{
				return (bool)base.GetValue(EditorBase.WatermarkTextIsVisibleProperty);
			}
			set
			{
				base.SetValue(EditorBase.WatermarkTextIsVisibleProperty, value);
			}
		}
		public bool IsScrollingOnCircle
		{
			get
			{
				return (bool)base.GetValue(EditorBase.IsScrollingOnCircleProperty);
			}
			set
			{
				base.SetValue(EditorBase.IsScrollingOnCircleProperty, value);
			}
		}
		public bool EnterToMoveNext
		{
			get
			{
				return (bool)base.GetValue(EditorBase.EnterToMoveNextProperty);
			}
			set
			{
				base.SetValue(EditorBase.EnterToMoveNextProperty, value);
			}
		}
		public bool TextSelectionOnFocus
		{
			get
			{
				return (bool)base.GetValue(EditorBase.TextSelectionOnFocusProperty);
			}
			set
			{
				base.SetValue(EditorBase.TextSelectionOnFocusProperty, value);
			}
		}
		[Obsolete("Property will not help due to internal arhitecture changes")]
		public bool IsCaretAnimationEnabled
		{
			get
			{
				return (bool)base.GetValue(EditorBase.IsCaretAnimationEnabledProperty);
			}
			set
			{
				base.SetValue(EditorBase.IsCaretAnimationEnabledProperty, value);
			}
		}
		internal new int CaretIndex
		{
			get
			{
				return base.SelectionStart;
			}
			set
			{
				base.SelectionStart = value;
				base.SetValue(EditorBase.CaretIndexProperty, value);
			}
		}
		public EditorBase()
		{
			base.MouseDoubleClick -= new MouseButtonEventHandler(this.EditorBase_MouseDoubleClick);
			base.MouseDoubleClick += new MouseButtonEventHandler(this.EditorBase_MouseDoubleClick);
			base.Loaded -= new RoutedEventHandler(this.EditorBase_Loaded);
			base.Loaded += new RoutedEventHandler(this.EditorBase_Loaded);
			base.Unloaded += new RoutedEventHandler(this.EditorBase_Unloaded);
		}
		private void EditorBase_Unloaded(object sender, RoutedEventArgs e)
		{
			base.MouseDoubleClick -= new MouseButtonEventHandler(this.EditorBase_MouseDoubleClick);
			base.Unloaded -= new RoutedEventHandler(this.EditorBase_Unloaded);
		}
		private void EditorBase_Loaded(object sender, RoutedEventArgs e)
		{
			if(this.EnableExtendedScrolling)
			{
				if(this.aLayer == null)
				{
					this.aLayer = AdornerLayer.GetAdornerLayer(this);
				}
				if(this.aLayer != null && this.vAdorner == null)
				{
					this.vAdorner = new ExtendedScrollingAdorner(this);
				}
				if(this.aLayer != null && this.vAdorner != null && this.aLayer.GetAdorners(this) == null)
				{
					this.aLayer.Add(this.vAdorner);
				}
			}
			if(this.EnableTouch)
			{
				if(this.aLayer == null)
				{
					this.aLayer = AdornerLayer.GetAdornerLayer(this);
				}
				if(this.aLayer != null && this.txtSelectionAdorner1 == null)
				{
					this.txtSelectionAdorner1 = new TextBoxSelectionAdorner(this);
					this.aLayer.Add(this.txtSelectionAdorner1);
				}
			}
		}
		private void EditorBase_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			base.SelectAll();
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
		protected override void OnDrop(DragEventArgs e)
		{
			e.Handled = true;
			base.OnDrop(e);
		}
		public static void OnCornerRadiusChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
		}
		public static void OnEnableTouchChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			if(obj is EditorBase)
			{
				(obj as EditorBase).OnEnableTouchChanged(args);
			}
		}
		private void OnEnableTouchChanged(DependencyPropertyChangedEventArgs args)
		{
			if(this.aLayer == null)
			{
				this.aLayer = AdornerLayer.GetAdornerLayer(this);
			}
			if((bool)args.NewValue)
			{
				if(this.aLayer != null)
				{
					this.txtSelectionAdorner1 = new TextBoxSelectionAdorner(this);
					this.aLayer.Add(this.txtSelectionAdorner1);
					return;
				}
			}
			else
			{
				if(this.aLayer != null)
				{
					this.aLayer.Remove(this.txtSelectionAdorner1);
				}
			}
		}
		public static void OnEnableExtendedScrollingChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			if(obj is EditorBase)
			{
				(obj as EditorBase).OnEnableExtendedScrollingChanged(args);
			}
		}
		private void OnEnableExtendedScrollingChanged(DependencyPropertyChangedEventArgs args)
		{
			if(this.aLayer == null)
			{
				this.aLayer = AdornerLayer.GetAdornerLayer(this);
			}
			if((bool)args.NewValue)
			{
				if(this.aLayer != null)
				{
					this.vAdorner = new ExtendedScrollingAdorner(this);
					this.aLayer.Add(this.vAdorner);
					return;
				}
			}
			else
			{
				if(this.aLayer != null)
				{
					this.aLayer.Remove(this.vAdorner);
				}
			}
		}
		public static void OnFocusedBackgroundChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
		}
		public static void OnFocusedForegroundChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			EditorBase editorBase = (EditorBase)obj;
			if(editorBase != null)
			{
				editorBase.OnFocusedForegroundChanged(args);
			}
		}
		private void OnFocusedForegroundChanged(DependencyPropertyChangedEventArgs args)
		{
		}
		public static void OnFocusedBorderBrushChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
		}
		public static void OnReadOnlyBackgroundChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
		}
		public static void OnPositiveForegroundChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			EditorBase editorBase = (EditorBase)obj;
			if(editorBase != null)
			{
				editorBase.OnPositiveForegroundChanged(args);
			}
		}
		protected void OnPositiveForegroundChanged(DependencyPropertyChangedEventArgs args)
		{
			if(this.IsZero && this.ApplyZeroColor)
			{
				return;
			}
			if(this.IsNegative && this.ApplyNegativeForeground)
			{
				return;
			}
			base.Foreground = this.PositiveForeground;
		}
		public static void OnApplyNegativeForegroundChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			EditorBase editorBase = (EditorBase)obj;
			if(editorBase != null)
			{
				editorBase.OnApplyNegativeForegroundChanged(args);
			}
		}
		protected void OnApplyNegativeForegroundChanged(DependencyPropertyChangedEventArgs args)
		{
			this.SetForeground();
		}
		public static void OnMinValidationChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
		}
		public static void OnZeroNegativeColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			EditorBase editorBase = (EditorBase)obj;
			if(editorBase != null)
			{
				editorBase.OnZeroNegativeColorChanged(args);
			}
		}
		protected void OnZeroNegativeColorChanged(DependencyPropertyChangedEventArgs args)
		{
			this.SetForeground();
		}
		public static void OnApplyZeroColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			EditorBase editorBase = (EditorBase)obj;
			if(editorBase != null)
			{
				editorBase.OnApplyZeroColorChanged(args);
			}
		}
		protected void OnApplyZeroColorChanged(DependencyPropertyChangedEventArgs args)
		{
			this.SetForeground();
		}
		internal void SetForeground()
		{
			if(this.ApplyZeroColor && this.IsZero)
			{
				base.Foreground = this.ZeroColor;
				return;
			}
			if(this.ApplyNegativeForeground && this.IsNegative)
			{
				base.Foreground = this.NegativeForeground;
				return;
			}
			if(!this.IsNegative)
			{
				base.Foreground = this.PositiveForeground;
				return;
			}
			base.Foreground = new SolidColorBrush(Colors.Black);
		}
		public static void OnWaterMarkTemplateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			if((EditorBase)obj != null)
			{
				((EditorBase)obj).OnWaterMarkTemplateChanged(args);
			}
		}
		protected void OnWaterMarkTemplateChanged(DependencyPropertyChangedEventArgs args)
		{
			if(this.WaterMarkTemplateChanged != null)
			{
				this.WaterMarkTemplateChanged(this, args);
			}
		}
		public static void OnWaterMarkTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			if((EditorBase)obj != null)
			{
				((EditorBase)obj).OnWaterMarkTextChanged(args);
			}
		}
		protected void OnWaterMarkTextChanged(DependencyPropertyChangedEventArgs args)
		{
			if(this.WaterMarkTextChanged != null)
			{
				this.WaterMarkTextChanged(this, args);
			}
		}
		private static object CoerceWatermarkVisibility(DependencyObject d, object baseValue)
		{
			EditorBase editorBase = (EditorBase)d;
			if(editorBase.WatermarkTextIsVisible && (Visibility)baseValue == Visibility.Visible)
			{
				editorBase.ContentElementVisibility = Visibility.Collapsed;
				return Visibility.Visible;
			}
			editorBase.ContentElementVisibility = Visibility.Visible;
			return Visibility.Collapsed;
		}
		public static void OnWatermarkVisibilityPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			EditorBase editorBase = (EditorBase)obj;
			if(editorBase != null)
			{
				editorBase.OnWatermarkVisibilityPropertyChanged(args);
			}
		}
		protected void OnWatermarkVisibilityPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
		}
		protected override void OnLostFocus(RoutedEventArgs e)
		{
			this.SetForeground();
			base.OnLostFocus(e);
			if(this.IsNull)
			{
				this.WatermarkVisibility = Visibility.Visible;
			}
		}
		protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			DoubleValueHandler.DoubleValHandler.AllowSelectionStart = false;
			if(this.TextSelectionOnFocus && !base.IsFocused)
			{
				e.Handled = true;
				base.Focus();
			}
			base.OnPreviewMouseLeftButtonDown(e);
		}
		protected override void OnGotFocus(RoutedEventArgs e)
		{
			this.SetForeground();
			base.OnGotFocus(e);
			this.WatermarkVisibility = Visibility.Collapsed;
			if(this.TextSelectionOnFocus)
			{
				base.SelectAll();
			}
		}
		public static void OnIsUndoEnabledChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			EditorBase editorBase = (EditorBase)obj;
			if(editorBase != null)
			{
				editorBase.OnIsUndoEnabledChanged(args);
			}
		}
		protected void OnIsUndoEnabledChanged(DependencyPropertyChangedEventArgs args)
		{
			if(this.IsUndoEnabledChanged != null)
			{
				this.IsUndoEnabledChanged(this, args);
			}
		}
		public static void OnNegativeForegroundChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			EditorBase editorBase = (EditorBase)obj;
			if(editorBase != null)
			{
				editorBase.OnNegativeForegroundChanged(args);
				editorBase.SetForeground();
			}
		}
		protected void OnNegativeForegroundChanged(DependencyPropertyChangedEventArgs args)
		{
			if(this.NegativeForegroundChanged != null)
			{
				this.NegativeForegroundChanged(this, args);
			}
		}
		public static void OnEnterToMoveNextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			EditorBase editorBase = (EditorBase)obj;
			if(editorBase != null)
			{
				editorBase.OnEnterToMoveNextChanged(args);
			}
		}
		protected void OnEnterToMoveNextChanged(DependencyPropertyChangedEventArgs args)
		{
			if(this.EnterToMoveNextChanged != null)
			{
				this.EnterToMoveNextChanged(this, args);
			}
		}
		public static void OnForegroundChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			if((EditorBase)obj != null)
			{
				((EditorBase)obj).OnForegroundChanged(args);
			}
		}
		protected void OnForegroundChanged(DependencyPropertyChangedEventArgs args)
		{
			if(this.IsZero && this.ApplyZeroColor)
			{
				return;
			}
			if(this.IsNegative)
			{
				bool arg_1F_0 = this.ApplyNegativeForeground;
			}
		}
		public static void OnUseNullOptionChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			if((EditorBase)obj != null)
			{
				((EditorBase)obj).OnUseNullOptionChanged(args);
			}
		}
		public virtual void OnUseNullOptionChanged(DependencyPropertyChangedEventArgs args)
		{
		}
		public static void OnIsNegativeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			if((EditorBase)obj != null)
			{
				((EditorBase)obj).OnIsNegativeChanged(args);
			}
		}
		protected void OnIsNegativeChanged(DependencyPropertyChangedEventArgs args)
		{
			this.SetForeground();
			if(this.IsValueNegativeChanged != null)
			{
				this.IsValueNegativeChanged(this, args);
			}
		}
		public static void OnIsZeroChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			if((EditorBase)obj != null)
			{
				((EditorBase)obj).OnIsZeroChanged(args);
			}
		}
		protected void OnIsZeroChanged(DependencyPropertyChangedEventArgs args)
		{
			this.SetForeground();
		}
		public static void OnIsNullChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			if((EditorBase)obj != null)
			{
				((EditorBase)obj).OnIsNullChanged(args);
			}
		}
		protected void OnIsNullChanged(DependencyPropertyChangedEventArgs args)
		{
		}
		public static void OnCultureChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			if((EditorBase)obj != null)
			{
				((EditorBase)obj).OnCultureChanged(args);
			}
		}
		protected void OnCultureChanged(DependencyPropertyChangedEventArgs args)
		{
			if(this.CultureChanged != null)
			{
				this.CultureChanged(this, args);
			}
			this.OnCultureChanged();
		}
		public static void OnNumberFormatChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			if((EditorBase)obj != null)
			{
				((EditorBase)obj).OnNumberFormatChanged(args);
			}
		}
		protected void OnNumberFormatChanged(DependencyPropertyChangedEventArgs args)
		{
			if(this.NumberFormatChanged != null)
			{
				this.NumberFormatChanged(this, args);
			}
			this.OnNumberFormatChanged();
		}
		public static void OnTextSelectionOnFocusChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			EditorBase editorBase = (EditorBase)obj;
			if(editorBase != null)
			{
				editorBase.OnTextSelectionOnFocusChanged(args);
			}
		}
		protected void OnTextSelectionOnFocusChanged(DependencyPropertyChangedEventArgs args)
		{
			if(this.TextSelectionOnFocusChanged != null)
			{
				this.TextSelectionOnFocusChanged(this, args);
			}
		}
		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			base.OnPreviewKeyDown(e);
			if(e.Key == Key.Return && !this.EnterToMoveNext && base.SelectionStart + 1 <= this.MaskedText.Length)
			{
				base.SelectionStart++;
			}
			if(ModifierKeys.Control == Keyboard.Modifiers)
			{
				Key arg_4C_0 = e.Key;
				Key arg_56_0 = e.Key;
				Key arg_60_0 = e.Key;
			}
		}
		internal virtual void OnCultureChanged()
		{
		}
		internal virtual void OnNumberFormatChanged()
		{
		}
	}
}