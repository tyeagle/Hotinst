/**
 * ==============================================================================
 *
 * ClassName: ColorPicker
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/3/29 10:33:26
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
using System.Windows.Media;
using System.Windows.Shapes;

#pragma warning disable 169

namespace HOTINST.COMMON.Controls.Controls
{
	/// <summary>
	/// ColorPicker
	/// </summary>
	[TemplatePart(Name = PART_ColorUnitPanel, Type = typeof(Border))]
	[TemplatePart(Name = PART_BaseColorPanel, Type = typeof(Border))]
	[TemplatePart(Name = PART_PickStylus, Type = typeof(Canvas))]
	[TemplatePart(Name = PART_ColorSlidePanel, Type = typeof(Canvas))]
	[TemplatePart(Name = PART_SlideCliping, Type = typeof(Canvas))]
	[TemplatePart(Name = PART_SlideStylus, Type = typeof(Canvas))]
	public class ColorPicker : Control
	{
		#region fields

		private const string PART_ColorUnitPanel = "PART_ColorUnitPanel";
		private const string PART_BaseColorPanel = "PART_BaseColorPanel";
		private const string PART_PickStylus = "PART_PickStylus";
		private const string PART_ColorSlidePanel = "PART_ColorSlidePanel";
		private const string PART_SlideCliping = "PART_SlideCliping";
		private const string PART_SlideStylus = "PART_SlideStylus";

		private Border _colorUintPanel;
		private Canvas _baseColorPanel;
		private Canvas _pickStylus;
		private Border _colorSlidePanel;
		private Canvas _slideCliping;
		private Path _slideStylus;

		#endregion

		#region props

		/// <summary>
		/// 定义依赖属性 CurrentColorProperty
		/// </summary>
		public static readonly DependencyProperty CurrentColorProperty = DependencyProperty.Register(
			"CurrentColor", typeof(Color), typeof(ColorPicker), new PropertyMetadata(Colors.White));
		
		/// <summary>
		/// 获取或设置CurrentColor(当前颜色)的值
		/// </summary>
		public Color CurrentColor
		{
			get { return (Color)GetValue(CurrentColorProperty); }
			set { SetValue(CurrentColorProperty, value); }
		}

		/// <summary>
		/// 定义依赖属性 IsColorPanelOpenProperty
		/// </summary>
		public static readonly DependencyProperty IsColorPanelOpenProperty = DependencyProperty.Register(
			"IsColorPanelOpen", typeof(bool), typeof(ColorPicker), new PropertyMetadata(default(bool)));
		/// <summary>
		/// 颜色选择器是否打开
		/// </summary>
		public bool IsColorPanelOpen
		{
			get { return (bool)GetValue(IsColorPanelOpenProperty); }
			set { SetValue(IsColorPanelOpenProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty UsingAlphaChannelProperty = DependencyProperty.Register(
			"UsingAlphaChannel", typeof(bool), typeof(ColorPicker), new PropertyMetadata(default(bool)));
		/// <summary>
		/// 
		/// </summary>
		public bool UsingAlphaChannel
		{
			get { return (bool)GetValue(UsingAlphaChannelProperty); }
			set { SetValue(UsingAlphaChannelProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty ShowRGBSlideProperty = DependencyProperty.Register(
			"ShowRGBSlide", typeof(bool), typeof(ColorPicker), new PropertyMetadata(default(bool)));
		/// <summary>
		/// 
		/// </summary>
		public bool ShowRGBSlide
		{
			get { return (bool)GetValue(ShowRGBSlideProperty); }
			set { SetValue(ShowRGBSlideProperty, value); }
		}

		#endregion

		#region .ctor

		static ColorPicker()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker), new FrameworkPropertyMetadata(typeof(ColorPicker)));
		}

		/// <summary>
		/// 初始化 ColorPicker 类的新实例。
		/// </summary>
		public ColorPicker()
		{

		}

		#endregion

		//#region override methods

		///// <summary>
		///// 重写父类OnApplyTemplate事件。
		///// </summary>
		//public override void OnApplyTemplate()
		//{
		//	_colorUintPanel = GetTemplateChild(PART_ColorUnitPanel) as Border;
		//	_pickStylus = GetTemplateChild(PART_PickStylus) as Canvas;
		//	if(_colorUintPanel != null && _pickStylus != null)
		//	{
		//		_colorUintPanel.MouseMove += ColorUintPanelOnMouseMove;
		//		_colorUintPanel.PreviewMouseLeftButtonDown += ColorUintPanelOnPreviewMouseLeftButtonDown;
		//		_colorUintPanel.PreviewMouseLeftButtonUp += ColorUintPanelOnPreviewMouseLeftButtonUp;
		//	}
		//	_colorSlidePanel = GetTemplateChild(PART_ColorSlidePanel) as Border;
		//	_baseColorPanel = GetTemplateChild(PART_BaseColorPanel) as Canvas;
		//	_slideCliping = GetTemplateChild(PART_SlideCliping) as Canvas;
		//	_slideStylus = GetTemplateChild(PART_SlideStylus) as Path;
		//	if(_colorSlidePanel != null && _slideStylus != null && _slideCliping != null && _baseColorPanel != null)
		//	{
		//		_colorSlidePanel.MouseMove += ColorSlidePanelOnMouseMove;
		//		_colorSlidePanel.PreviewMouseLeftButtonDown += ColorSlidePanelOnPreviewMouseLeftButtonDown;
		//		_colorSlidePanel.PreviewMouseLeftButtonUp += ColorSlidePanelOnPreviewMouseLeftButtonUp;
		//	}

		//	base.OnApplyTemplate();
		//}

		//#endregion

		//#region ColorUintPanel event handler

		//private void ColorUintPanelOnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		//{
		//	e.Handled = true;

		//	//Sets the mouse position to the centere of _PickerStylus
		//	Thickness newMargin = new Thickness(e.GetPosition(_colorUintPanel).X - 7.5, e.GetPosition(_colorUintPanel).Y - 7.5, 0, 0);

		//	ThicknessAnimation animation = new ThicknessAnimation(newMargin, new Duration(TimeSpan.FromMilliseconds(150)), FillBehavior.Stop);
		//	animation.Completed += (o, args) => _pickStylus.Margin = newMargin;

		//	_pickStylus.BeginAnimation(MarginProperty, animation);

		//	//Clips the mouse to the _colorPicerCliping.
		//	CLPCursor.OnUIElement(_colorUintPanel);

		//	_colorUintPanel.Cursor = Cursors.Pen;
		//	//Sets the IsCurrentColorProperty to the color under the mouse.
		//	SetValue(CurrentColorProperty, MouseControlling.PixelUnderMouse());
		//}

		//private void ColorUintPanelOnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		//{
		//	e.Handled = true;

		//	CLPCursor.Release();
		//	_pickStylus.Visibility = Visibility.Visible;
		//	_colorUintPanel.Cursor = Cursors.Arrow;
		//}
		
		//private void ColorUintPanelOnMouseMove(object sender, MouseEventArgs e)
		//{
		//	e.Handled = true;

		//	if(e.LeftButton != MouseButtonState.Pressed || _pickStylus == null)
		//		return;

		//	_pickStylus.Visibility = Visibility.Hidden;

		//	//Sets the position of the PickerStylus.
		//	_pickStylus.Margin = new Thickness(e.GetPosition(_colorUintPanel).X - 7.5, e.GetPosition(_colorUintPanel).Y - 7.5, 0, 0);
		//	//Sets the IsCurrentColorProperty to the color under the mouse.
		//	Dispatcher.BeginInvoke(new Action(() =>
		//	{
		//		SetValue(CurrentColorProperty, MouseControlling.PixelUnderMouse());
		//	}), DispatcherPriority.ApplicationIdle);
		//}

		//#endregion

		//#region ColorSlidePanel event handler

		//private void ColorSlidePanelOnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		//{
		//	e.Handled = true;

		//	Thickness newMargin = new Thickness(0, e.GetPosition(_colorSlidePanel).Y - 4, 0, 0);

		//	ThicknessAnimation animation = new ThicknessAnimation(newMargin, new Duration(TimeSpan.FromMilliseconds(200)), FillBehavior.Stop);
		//	animation.Completed += (o, args) => _slideStylus.Margin = newMargin;

		//	_slideStylus.BeginAnimation(MarginProperty, animation);
			
		//	CLPCursor.OnUIElement(_slideCliping);
		//	_colorSlidePanel.Cursor = Cursors.SizeNS;

		//	_baseColorPanel.Background = new SolidColorBrush(MouseControlling.PixelUnderMouse());

		//	//Sets the IsCurrentColorProperty to the color under the mouse.
		//	Point pos = _pickStylus.PointToScreen(new Point(7.5, 7.5));
		//	SetValue(CurrentColorProperty, MouseControlling.PixelColor((int)pos.X, (int)pos.Y));
		//}

		//private void ColorSlidePanelOnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		//{
		//	e.Handled = true;

		//	CLPCursor.Release();
		//	_colorSlidePanel.Cursor = Cursors.Arrow;
		//}
		
		//private void ColorSlidePanelOnMouseMove(object sender, MouseEventArgs e)
		//{
		//	e.Handled = true;

		//	if(e.LeftButton != MouseButtonState.Pressed)
		//		return;
			
		//	_slideStylus.Margin = new Thickness(0, e.GetPosition(_colorSlidePanel).Y - 4, 0, 0);
			
		//	CLPCursor.OnUIElement(_slideCliping);
		//	_colorSlidePanel.Cursor = Cursors.SizeNS;

		//	//Sets the IsCurrentColorProperty to the color under the mouse.
		//	Point pos = _pickStylus.PointToScreen(new Point(7.5, 7.5));
		//	Dispatcher.BeginInvoke(new Action(() =>
		//	{
		//		_baseColorPanel.Background = new SolidColorBrush(MouseControlling.PixelUnderMouse());

		//		SetValue(CurrentColorProperty, MouseControlling.PixelColor((int)pos.X, (int)pos.Y));
		//	}), DispatcherPriority.ApplicationIdle);
		//}

		//#endregion
	}
}