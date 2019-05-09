/**
 * ==============================================================================
 *
 * ClassName: PackIconControl
 * Description: 
 *
 * Version: 1.0
 * Created: 2018/8/27 14:31:03
 * Compiler: Visual Studio 2017
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HOTINST.COMMON.Controls.Controls.PackIcon
{
	/// <summary>
	/// Enum PackIconFlipOrientation for the Flip property of any PackIcon control.
	/// </summary>
	public enum PackIconFlipOrientation
	{
		/// <summary>
		/// No flip
		/// </summary>
		Normal,

		/// <summary>
		/// Flip the icon horizontal
		/// </summary>
		Horizontal,

		/// <summary>
		/// Flip the icon vertical
		/// </summary>
		Vertical,

		/// <summary>
		/// Flip the icon vertical and horizontal
		/// </summary>
		Both
	}

    /// <summary>
    /// Class PackIconControl which is the custom base class for any PackIcon control.
    /// </summary>
    /// <typeparam name="TKind">The type of the enum kind.</typeparam>
    /// <seealso cref="PackIconBase{TKind}" />
    public abstract class PackIconControl<TKind> : PackIconBase<TKind>
    {
        static PackIconControl()
        {
            OpacityProperty.OverrideMetadata(typeof(PackIconControl<TKind>), new UIPropertyMetadata(1d, (d, e) => d.CoerceValue(SpinProperty)));
            VisibilityProperty.OverrideMetadata(typeof(PackIconControl<TKind>), new UIPropertyMetadata(Visibility.Visible, (d, e) => d.CoerceValue(SpinProperty)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataIndexFactory"></param>
        protected PackIconControl(Func<IDictionary<TKind, string>> dataIndexFactory) : base(dataIndexFactory)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            CoerceValue(SpinProperty);
            if(Spin)
            {
                StopSpinAnimation();
                BeginSpinAnimation();
            }
        }

        /// <summary>
        /// Identifies the Flip dependency property.
        /// </summary>
        public static readonly DependencyProperty FlipProperty = DependencyProperty.Register(
            "Flip", typeof(PackIconFlipOrientation), typeof(PackIconControl<TKind>), new PropertyMetadata(PackIconFlipOrientation.Normal));

        /// <summary>
        /// Gets or sets the flip orientation.
        /// </summary>
        public PackIconFlipOrientation Flip
        {
            get => (PackIconFlipOrientation)GetValue(FlipProperty);
            set => SetValue(FlipProperty, value);
        }

        /// <summary>
        /// Identifies the Rotation dependency property.
        /// </summary>
        public static readonly DependencyProperty RotationProperty = DependencyProperty.Register(
            "Rotation", typeof(double), typeof(PackIconControl<TKind>), new PropertyMetadata(0d, null, RotationPropertyCoerceValueCallback));

        private static object RotationPropertyCoerceValueCallback(DependencyObject dependencyObject, object value)
        {
            double val = (double)value;
            return val < 0 ? 0d : (val > 360 ? 360d : value);
        }

        /// <summary>
        /// Gets or sets the rotation (angle).
        /// </summary>
        /// <value>The rotation.</value>
        public double Rotation
        {
            get => (double)GetValue(RotationProperty);
            set => SetValue(RotationProperty, value);
        }

        /// <summary>
        /// Identifies the Spin dependency property.
        /// </summary>
        public static readonly DependencyProperty SpinProperty = DependencyProperty.Register(
            "Spin", typeof(bool), typeof(PackIconControl<TKind>), new PropertyMetadata(default(bool), SpinPropertyChangedCallback, SpinPropertyCoerceValueCallback));

        private static object SpinPropertyCoerceValueCallback(DependencyObject dependencyObject, object value)
        {
            if(dependencyObject is PackIconControl<TKind> packIcon && (!packIcon.IsVisible || packIcon.Opacity <= 0 || packIcon.SpinDuration <= 0.0))
            {
                return false;
            }
            return value;
        }

        private static void SpinPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if(dependencyObject is PackIconControl<TKind> packIcon && e.OldValue != e.NewValue && e.NewValue is bool)
            {
                bool spin = (bool)e.NewValue;
                if(spin)
                {
                    packIcon.BeginSpinAnimation();
                }
                else
                {
                    packIcon.StopSpinAnimation();
                }
            }
        }

        private static readonly string SpinnerStoryBoardName = $"{typeof(PackIconControl<TKind>).Name}SpinnerStoryBoard";

        private FrameworkElement _innerGrid;
        private FrameworkElement InnerGrid => _innerGrid ?? (_innerGrid = GetTemplateChild("PART_InnerGrid") as FrameworkElement);

        private void BeginSpinAnimation()
        {
            FrameworkElement element = InnerGrid;
            if(null == element)
            {
                return;
            }
            TransformGroup transformGroup = element.RenderTransform as TransformGroup ?? new TransformGroup();
            RotateTransform rotateTransform = transformGroup.Children.OfType<RotateTransform>().LastOrDefault();

            if(rotateTransform != null)
            {
                rotateTransform.Angle = 0;
            }
            else
            {
                transformGroup.Children.Add(new RotateTransform());
                element.RenderTransform = transformGroup;
            }

            Storyboard storyboard = new Storyboard();
            DoubleAnimation animation = new DoubleAnimation
            {
                From = 0,
                To = 360,
                AutoReverse = SpinAutoReverse,
                EasingFunction = SpinEasingFunction,
                RepeatBehavior = RepeatBehavior.Forever,
                Duration = new Duration(TimeSpan.FromSeconds(SpinDuration))
            };
            storyboard.Children.Add(animation);

            Storyboard.SetTarget(animation, element);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(0).(1)[2].(2)", RenderTransformProperty, TransformGroup.ChildrenProperty, RotateTransform.AngleProperty));

            element.Resources.Add(SpinnerStoryBoardName, storyboard);
            storyboard.Begin();
        }

        private void StopSpinAnimation()
        {
            FrameworkElement element = InnerGrid;
            if(element?.Resources[SpinnerStoryBoardName] is Storyboard storyboard)
            {
                storyboard.Stop();
                element.Resources.Remove(SpinnerStoryBoardName);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the inner icon is spinning.
        /// </summary>
        /// <value><c>true</c> if spin; otherwise, <c>false</c>.</value>
        public bool Spin
        {
            get => (bool)GetValue(SpinProperty);
            set => SetValue(SpinProperty, value);
        }

        /// <summary>
        /// Identifies the SpinDuration dependency property.
        /// </summary>
        public static readonly DependencyProperty SpinDurationProperty = DependencyProperty.Register(
            "SpinDuration", typeof(double), typeof(PackIconControl<TKind>), new PropertyMetadata(1d, SpinDurationPropertyChangedCallback, SpinDurationCoerceValueCallback));

        private static void SpinDurationPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if(dependencyObject is PackIconControl<TKind> packIcon && e.OldValue != e.NewValue && packIcon.Spin && e.NewValue is double)
            {
                packIcon.StopSpinAnimation();
                packIcon.BeginSpinAnimation();
            }
        }

        private static object SpinDurationCoerceValueCallback(DependencyObject dependencyObject, object value)
        {
            double val = (double)value;
            return val < 0 ? 0d : value;
        }

        /// <summary>
        /// Gets or sets the duration of the spinning animation (in seconds). This will also restart the spin animation.
        /// </summary>
        /// <value>The duration of the spin in seconds.</value>
        public double SpinDuration
        {
            get => (double)GetValue(SpinDurationProperty);
            set => SetValue(SpinDurationProperty, value);
        }

        /// <summary>
        /// Identifies the SpinEasingFunction dependency property.
        /// </summary>
        public static readonly DependencyProperty SpinEasingFunctionProperty = DependencyProperty.Register(
            "SpinEasingFunction", typeof(IEasingFunction), typeof(PackIconControl<TKind>), new PropertyMetadata(null, SpinEasingFunctionPropertyChangedCallback));

        private static void SpinEasingFunctionPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if(dependencyObject is PackIconControl<TKind> packIcon && e.OldValue != e.NewValue && packIcon.Spin)
            {
                packIcon.StopSpinAnimation();
                packIcon.BeginSpinAnimation();
            }
        }

        /// <summary>
        /// Gets or sets the EasingFunction of the spinning animation. This will also restart the spin animation.
        /// </summary>
        /// <value>The spin easing function.</value>
        public IEasingFunction SpinEasingFunction
        {
            get => (IEasingFunction)GetValue(SpinEasingFunctionProperty);
            set => SetValue(SpinEasingFunctionProperty, value);
        }

        /// <summary>
        /// Identifies the SpinAutoReverse dependency property.
        /// </summary>
        public static readonly DependencyProperty SpinAutoReverseProperty = DependencyProperty.Register(
            "SpinAutoReverse", typeof(bool), typeof(PackIconControl<TKind>), new PropertyMetadata(default(bool), SpinAutoReversePropertyChangedCallback));

        private static void SpinAutoReversePropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if(dependencyObject is PackIconControl<TKind> packIcon && e.OldValue != e.NewValue && packIcon.Spin && e.NewValue is bool)
            {
                packIcon.StopSpinAnimation();
                packIcon.BeginSpinAnimation();
            }
        }

        /// <summary>
        /// Gets or sets the AutoReverse of the spinning animation. This will also restart the spin animation.
        /// </summary>
        /// <value><c>true</c> if [spin automatic reverse]; otherwise, <c>false</c>.</value>
        public bool SpinAutoReverse
        {
            get => (bool)GetValue(SpinAutoReverseProperty);
            set => SetValue(SpinAutoReverseProperty, value);
        }
    }
}