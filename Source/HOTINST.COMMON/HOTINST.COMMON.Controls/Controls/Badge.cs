/**
 * ==============================================================================
 *
 * ClassName: Badge
 * Description: 
 *
 * Version: 1.0
 * Created: 2018/8/27 12:21:19
 * Compiler: Visual Studio 2017
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HOTINST.COMMON.Controls.Controls
{
	/// <summary>
	/// 位置
	/// </summary>
	public enum BadgePlacementMode
	{
		/// <summary>
		/// 
		/// </summary>
		TopLeft,
		/// <summary>
		/// 
		/// </summary>
		Top,
		/// <summary>
		/// 
		/// </summary>
		TopRight,
		/// <summary>
		/// 
		/// </summary>
		Right,
		/// <summary>
		/// 
		/// </summary>
		BottomRight,
		///
		/// <summary>
		/// 
		/// </summary>
		Bottom,
		/// <summary>
		/// 
		/// </summary>
		BottomLeft,
		/// <summary>
		/// 
		/// </summary>
		Left
	}

	/// <summary>
	/// 徽章控件
	/// </summary>
	[TemplatePart(Name = BadgeContainerPartName, Type = typeof(UIElement))]
    public class Badge : ContentControl
	{
		#region fields

		private const string BadgeContainerPartName = "PART_BadgeContainer";

		private FrameworkElement _badgeContainer;

		#endregion

		#region props

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty BadgeContentProperty = DependencyProperty.Register(
			"BadgeContent", typeof(object), typeof(Badge), new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.AffectsArrange, OnBadgeChanged));
		/// <summary>
		/// 
		/// </summary>
		public object BadgeContent
		{
			get => GetValue(BadgeContentProperty);
			set => SetValue(BadgeContentProperty, value);
		}

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty BadgeBackgroundProperty = DependencyProperty.Register(
			"BadgeBackground", typeof(Brush), typeof(Badge), new PropertyMetadata(default(Brush)));
		/// <summary>
		/// 
		/// </summary>
		public Brush BadgeBackground
		{
			get => (Brush)GetValue(BadgeBackgroundProperty);
			set => SetValue(BadgeBackgroundProperty, value);
		}

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty BadgeForegroundProperty = DependencyProperty.Register(
			"BadgeForeground", typeof(Brush), typeof(Badge), new PropertyMetadata(default(Brush)));
		/// <summary>
		/// 
		/// </summary>
		public Brush BadgeForeground
		{
			get => (Brush)GetValue(BadgeForegroundProperty);
			set => SetValue(BadgeForegroundProperty, value);
		}

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty BadgePlacementModeProperty = DependencyProperty.Register(
			"BadgePlacementMode", typeof(BadgePlacementMode), typeof(Badge), new PropertyMetadata(default(BadgePlacementMode)));
		/// <summary>
		/// 
		/// </summary>
		public BadgePlacementMode BadgePlacementMode
		{
			get => (BadgePlacementMode)GetValue(BadgePlacementModeProperty);
			set => SetValue(BadgePlacementModeProperty, value);
		}

		/// <summary>
		/// 
		/// </summary>
		public static readonly RoutedEvent BadgeChangedEvent = EventManager.RegisterRoutedEvent(
			"BadgeChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(Badge));
		/// <summary>
		/// 
		/// </summary>
		public event RoutedPropertyChangedEventHandler<object> BadgeChanged
		{
			add => AddHandler(BadgeChangedEvent, value);
			remove => RemoveHandler(BadgeChangedEvent, value);
		}

		/// <summary>
		/// 
		/// </summary>
		private static readonly DependencyPropertyKey IsBadgeSetPropertyKey = DependencyProperty.RegisterReadOnly(
			"IsBadgeSet", typeof(bool), typeof(Badge), new PropertyMetadata(default(bool)));

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty IsBadgeSetProperty = IsBadgeSetPropertyKey.DependencyProperty;

		/// <summary>
		/// 
		/// </summary>
		public bool IsBadgeSet
		{
			get => (bool)GetValue(IsBadgeSetProperty);
			private set => SetValue(IsBadgeSetPropertyKey, value);
		}

		#endregion

		#region .ctor

		static Badge()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Badge), new FrameworkPropertyMetadata(typeof(Badge)));
        }

		/// <summary>
		/// 
		/// </summary>
        public Badge()
        {

        }

		#endregion

		private static void OnBadgeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Badge instance = (Badge)d;

			instance.IsBadgeSet = !string.IsNullOrWhiteSpace(e.NewValue as string) || e.NewValue != null && !(e.NewValue is string);

			RoutedPropertyChangedEventArgs<object> args = new RoutedPropertyChangedEventArgs<object>(e.OldValue, e.NewValue)
			{
				RoutedEvent = BadgeChangedEvent
			};
			instance.RaiseEvent(args);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="arrangeBounds"></param>
		/// <returns></returns>
		protected override Size ArrangeOverride(Size arrangeBounds)
		{
			Size result = base.ArrangeOverride(arrangeBounds);

			if(_badgeContainer == null)
				return result;

			Size containerDesiredSize = _badgeContainer.DesiredSize;
			if((containerDesiredSize.Width <= 0.0 || containerDesiredSize.Height <= 0.0)
			   && !double.IsNaN(_badgeContainer.ActualWidth) && !double.IsInfinity(_badgeContainer.ActualWidth)
			   && !double.IsNaN(_badgeContainer.ActualHeight) && !double.IsInfinity(_badgeContainer.ActualHeight))
			{
				containerDesiredSize = new Size(_badgeContainer.ActualWidth, _badgeContainer.ActualHeight);
			}

			double h = 0 - containerDesiredSize.Width / 2;
			double v = 0 - containerDesiredSize.Height / 2;
			_badgeContainer.Margin = new Thickness(0);
			_badgeContainer.Margin = new Thickness(h, v, h, v);

			return result;
		}

		#region Overrides of FrameworkElement

		/// <summary>在派生类中重写后，每当应用程序代码或内部进程调用 <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />，都将调用此方法。</summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			_badgeContainer = GetTemplateChild(BadgeContainerPartName) as FrameworkElement;
		}

		#endregion
	}
}