using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace HOTINST.COMMON.Controls.Helper
{
    /// <summary>
    /// A helper class that provides various controls.
    /// </summary>
    public static class ControlsHelper
    {
		/// <summary>
		/// 
		/// </summary>
        public static readonly DependencyProperty DisabledVisualElementVisibilityProperty = DependencyProperty.RegisterAttached(
			"DisabledVisualElementVisibility", typeof(Visibility), typeof(ControlsHelper), new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Gets the value to handle the visibility of the DisabledVisualElement in the template.
        /// </summary>
        
        [AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
        [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
        
        public static Visibility GetDisabledVisualElementVisibility(UIElement element)
        {
            return (Visibility)element.GetValue(DisabledVisualElementVisibilityProperty);
        }

        /// <summary>
        /// Sets the value to handle the visibility of the DisabledVisualElement in the template.
        /// </summary>
        public static void SetDisabledVisualElementVisibility(UIElement element, Visibility value)
        {
            element.SetValue(DisabledVisualElementVisibilityProperty, value);
        }

        /// <summary>
        /// The DependencyProperty for the CharacterCasing property.
        /// Controls whether or not content is converted to upper or lower case
        /// </summary>
        public static readonly DependencyProperty ContentCharacterCasingProperty =
            DependencyProperty.RegisterAttached(
                "ContentCharacterCasing",
                typeof (CharacterCasing),
                typeof (ControlsHelper),
                new FrameworkPropertyMetadata(CharacterCasing.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure),
                value => CharacterCasing.Normal <= (CharacterCasing) value && (CharacterCasing) value <= CharacterCasing.Upper);

        /// <summary>
        /// Gets the character casing of the control
        /// </summary>
        
        [AttachedPropertyBrowsableForType(typeof(ContentControl))]
        
        
        public static CharacterCasing GetContentCharacterCasing(UIElement element)
        {
            return (CharacterCasing)element.GetValue(ContentCharacterCasingProperty);
        }

        /// <summary>
        /// Sets the character casing of the control
        /// </summary>
        public static void SetContentCharacterCasing(UIElement element, CharacterCasing value)
        {
            element.SetValue(ContentCharacterCasingProperty, value);
        }

		/// <summary>
		/// 
		/// </summary>
        public static readonly DependencyProperty HeaderFontSizeProperty =
            DependencyProperty.RegisterAttached("HeaderFontSize", typeof(double), typeof(ControlsHelper), new FrameworkPropertyMetadata(SystemFonts.MessageFontSize){ Inherits = true});
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
        [AttachedPropertyBrowsableForType(typeof(HeaderedContentControl))]
        public static double GetHeaderFontSize(UIElement element)
        {
            return (double)element.GetValue(HeaderFontSizeProperty);
        }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
        public static void SetHeaderFontSize(UIElement element, double value)
        {
            element.SetValue(HeaderFontSizeProperty, value);
        }

		/// <summary>
		/// 
		/// </summary>
        public static readonly DependencyProperty HeaderFontStretchProperty =
            DependencyProperty.RegisterAttached("HeaderFontStretch", typeof(FontStretch), typeof(ControlsHelper), new UIPropertyMetadata(FontStretches.Normal));

        /// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
        [AttachedPropertyBrowsableForType(typeof(HeaderedContentControl))]
        public static FontStretch GetHeaderFontStretch(UIElement element)
        {
            return (FontStretch)element.GetValue(HeaderFontStretchProperty);
        }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
        public static void SetHeaderFontStretch(UIElement element, FontStretch value)
        {
            element.SetValue(HeaderFontStretchProperty, value);
        }

		/// <summary>
		/// 
		/// </summary>
        public static readonly DependencyProperty HeaderFontWeightProperty =
            DependencyProperty.RegisterAttached("HeaderFontWeight", typeof(FontWeight), typeof(ControlsHelper), new UIPropertyMetadata(FontWeights.Normal));

		/// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
        [AttachedPropertyBrowsableForType(typeof(HeaderedContentControl))]
        public static FontWeight GetHeaderFontWeight(UIElement element)
        {
            return (FontWeight)element.GetValue(HeaderFontWeightProperty);
        }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
        public static void SetHeaderFontWeight(UIElement element, FontWeight value)
        {
            element.SetValue(HeaderFontWeightProperty, value);
        }

		/// <summary>
		/// 
		/// </summary>
        public static readonly DependencyProperty HeaderMarginProperty =
            DependencyProperty.RegisterAttached("HeaderMargin", typeof(Thickness), typeof(ControlsHelper), new UIPropertyMetadata(new Thickness()));

		/// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
        [AttachedPropertyBrowsableForType(typeof(HeaderedContentControl))]
        public static Thickness GetHeaderMargin(UIElement element)
        {
            return (Thickness)element.GetValue(HeaderMarginProperty);
        }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
        public static void SetHeaderMargin(UIElement element, Thickness value)
        {
            element.SetValue(HeaderMarginProperty, value);
        }

        /// <summary>
        /// This property can be used to set the button width (PART_ClearText) of TextBox, PasswordBox, ComboBox, NumericUpDown
        /// </summary>
        [Obsolete(@"This property will be deleted in the next release. You should use TextBoxHelper.ButtonWidth instead.")]
        public static readonly DependencyProperty ButtonWidthProperty =
            DependencyProperty.RegisterAttached("ButtonWidth", typeof(double), typeof(ControlsHelper),
                                                new FrameworkPropertyMetadata(
                                                    22d,
                                                    FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits,
                                                    (o, e) =>
                                                        {
                                                            var element = o as UIElement;
                                                            if (element != null && e.OldValue != e.NewValue && e.NewValue is double)
                                                            {
                                                                TextBoxHelper.SetButtonWidth(element, (double)e.NewValue);
                                                            }
                                                        }));

        /// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
        public static double GetButtonWidth(DependencyObject obj)
        {
#pragma warning disable 618
            return (double)obj.GetValue(ButtonWidthProperty);
#pragma warning restore 618
        }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="value"></param>
        public static void SetButtonWidth(DependencyObject obj, double value)
        {
#pragma warning disable 618
            obj.SetValue(ButtonWidthProperty, value);
#pragma warning restore 618
        }
		/// <summary>
		/// 
		/// </summary>
        public static readonly DependencyProperty FocusBorderBrushProperty = DependencyProperty.RegisterAttached("FocusBorderBrush", typeof(Brush), typeof(ControlsHelper), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));
		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty MouseOverBorderBrushProperty = DependencyProperty.RegisterAttached("MouseOverBorderBrush", typeof(Brush), typeof(ControlsHelper), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Sets the brush used to draw the focus border.
        /// </summary>
        public static void SetFocusBorderBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(FocusBorderBrushProperty, value);
        }

        /// <summary>
        /// Gets the brush used to draw the focus border.
        /// </summary>
        
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        [AttachedPropertyBrowsableForType(typeof(CheckBox))]
        [AttachedPropertyBrowsableForType(typeof(RadioButton))]
        [AttachedPropertyBrowsableForType(typeof(DatePicker))]
        [AttachedPropertyBrowsableForType(typeof(ComboBox))]
        public static Brush GetFocusBorderBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(FocusBorderBrushProperty);
        }

        /// <summary>
        /// Sets the brush used to draw the mouse over brush.
        /// </summary>
        public static void SetMouseOverBorderBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(MouseOverBorderBrushProperty, value);
        }

        /// <summary>
        /// Gets the brush used to draw the mouse over brush.
        /// </summary>
        
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        [AttachedPropertyBrowsableForType(typeof(CheckBox))]
        [AttachedPropertyBrowsableForType(typeof(RadioButton))]
        [AttachedPropertyBrowsableForType(typeof(DatePicker))]
        [AttachedPropertyBrowsableForType(typeof(ComboBox))]
        public static Brush GetMouseOverBorderBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(MouseOverBorderBrushProperty);
        }

        /// <summary>
        /// DependencyProperty for <see cref="CornerRadius" /> property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty
            = DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(ControlsHelper),
                                                  new FrameworkPropertyMetadata(
                                                      new CornerRadius(),
                                                      FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary> 
        /// The CornerRadius property allows users to control the roundness of the button corners independently by 
        /// setting a radius value for each corner. Radius values that are too large are scaled so that they
        /// smoothly blend from corner to corner. (Can be used e.g. at MetroButton style)
        /// Description taken from original Microsoft description :-D
        /// </summary>
        
        public static CornerRadius GetCornerRadius(UIElement element)
        {
            return (CornerRadius)element.GetValue(CornerRadiusProperty);
        }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
        public static void SetCornerRadius(UIElement element, CornerRadius value)
        {
            element.SetValue(CornerRadiusProperty, value);
        }
    }
}