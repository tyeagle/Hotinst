/**
 * ==============================================================================
 *
 * ClassName: FlatGroupBox
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/3/21 13:01:11
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
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace HOTINST.COMMON.Controls.Controls.Layout
{
	/// <summary>
	/// 表示一个可创建容器的控件，该容器具有针对 user interface (UI) 内容的边框和标题。
	/// </summary>
	public class FlatGroupBox : ContentControl
	{
		#region fields

		private const string PART_AdditionalBtn = "PART_AdditionalButton";
		private ToggleButton _additionalBtn;

		#endregion

		#region props

		/// <summary>
		/// 定义依赖属性 Header
		/// </summary>
		public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
			"Header", typeof(object), typeof(FlatGroupBox), new PropertyMetadata(default(object)));
		/// <summary>
		/// 标题
		/// </summary>
		public object Header
		{
			get { return (object)GetValue(HeaderProperty); }
			set { SetValue(HeaderProperty, value); }
		}

		/// <summary>
		/// 定义依赖属性 HeaderStringFormat
		/// </summary>
		public static readonly DependencyProperty HeaderStringFormatProperty = DependencyProperty.Register(
			"HeaderStringFormat", typeof(string), typeof(FlatGroupBox), new PropertyMetadata(default(string)));
		/// <summary>
		/// 标题格式化字符串
		/// </summary>
		public string HeaderStringFormat
		{
			get { return (string)GetValue(HeaderStringFormatProperty); }
			set { SetValue(HeaderStringFormatProperty, value); }
		}

		/// <summary>
		/// 定义依赖属性 HeaderAlignment
		/// </summary>
		public static readonly DependencyProperty HeaderAlignmentProperty = DependencyProperty.Register(
			"HeaderAlignment", typeof(HorizontalAlignment), typeof(FlatGroupBox), new PropertyMetadata(HorizontalAlignment.Left));
		/// <summary>
		/// 标题对齐方式
		/// </summary>
		public HorizontalAlignment HeaderAlignment
		{
			get { return (HorizontalAlignment)GetValue(HeaderAlignmentProperty); }
			set { SetValue(HeaderAlignmentProperty, value); }
		}

		/// <summary>
		/// 定义依赖属性 HeaderTemplate
		/// </summary>
		public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
			"HeaderTemplate", typeof(DataTemplate), typeof(FlatGroupBox), new PropertyMetadata(default(DataTemplate)));
		/// <summary>
		/// 标题模版
		/// </summary>
		public DataTemplate HeaderTemplate
		{
			get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
			set { SetValue(HeaderTemplateProperty, value); }
		}

		/// <summary>
		/// 定义依赖属性 CornerRadius
		/// </summary>
		public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
			"CornerRadius", typeof(CornerRadius), typeof(FlatGroupBox), new PropertyMetadata(default(CornerRadius)));
		/// <summary>
		/// 边框圆角
		/// </summary>
		public CornerRadius CornerRadius
		{
			get { return (CornerRadius)GetValue(CornerRadiusProperty); }
			set { SetValue(CornerRadiusProperty, value); }
		}

		/// <summary>
		/// 定义依赖属性 GroupBorderBrush
		/// </summary>
		public static readonly DependencyProperty GroupBorderBrushProperty = DependencyProperty.Register(
			"GroupBorderBrush", typeof(Brush), typeof(FlatGroupBox), new PropertyMetadata(default(Brush)));
		/// <summary>
		/// 边框颜色
		/// </summary>
		public Brush GroupBorderBrush
		{
			get { return (Brush)GetValue(GroupBorderBrushProperty); }
			set { SetValue(GroupBorderBrushProperty, value); }
		}

		/// <summary>
		/// 定义依赖属性 AdditionalBtnEnable
		/// </summary>
		public static readonly DependencyProperty IsShowAdditionalBtnProperty = DependencyProperty.Register(
			"IsShowAdditionalBtn", typeof(bool), typeof(FlatGroupBox), new PropertyMetadata(true));
		/// <summary>
		/// 是否显示附加按钮
		/// </summary>
		public bool IsShowAdditionalBtn
		{
			get { return (bool)GetValue(IsShowAdditionalBtnProperty); }
			set { SetValue(IsShowAdditionalBtnProperty, value); }
		}

		/// <summary>
		/// 定义依赖属性 AdditionalMenu
		/// </summary>
		public static readonly DependencyProperty AdditionalMenuProperty = DependencyProperty.Register(
			"AdditionalMenu", typeof(ContextMenu), typeof(FlatGroupBox), new PropertyMetadata(default(ContextMenu), (o, args) =>
			{
				FlatGroupBox context = o as FlatGroupBox;
				if(context != null)
					context.AdditionalMenuChanged((ContextMenu)args.NewValue);
			}));
		/// <summary>
		/// 附加按钮的菜单
		/// </summary>
		public ContextMenu AdditionalMenu
		{
			get { return (ContextMenu)GetValue(AdditionalMenuProperty); }
			set { SetValue(AdditionalMenuProperty, value); }
		}

		/// <summary>
		/// 定义依赖属性 IsAdditionalMenuOpen
		/// </summary>
		public static readonly DependencyProperty IsAdditionalMenuOpenProperty = DependencyProperty.Register(
			"IsAdditionalMenuOpen", typeof(bool), typeof(FlatGroupBox), new PropertyMetadata(default(bool), (o, args) =>
			{
				FlatGroupBox context = o as FlatGroupBox;
				if(context != null)
					context.TurnMenuState((bool)args.NewValue);
			}));
		/// <summary>
		/// 附加按钮的打开状态
		/// </summary>
		public bool IsAdditionalMenuOpen
		{
			get { return (bool)GetValue(IsAdditionalMenuOpenProperty); }
			set { SetValue(IsAdditionalMenuOpenProperty, value); }
		}

		#endregion

		#region .ctor

		static FlatGroupBox()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(FlatGroupBox),
				new FrameworkPropertyMetadata(typeof(FlatGroupBox)));
		}

		#endregion

		#region override methods

		/// <summary>
		/// 重写父类OnApplyTemplate事件。
		/// </summary>
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_additionalBtn = GetTemplateChild(PART_AdditionalBtn) as ToggleButton;
			if(_additionalBtn != null)
				_additionalBtn.PreviewMouseRightButtonUp += (sender, args) => args.Handled = true;
		}

		#endregion

		#region private methods

		private void AdditionalMenuChanged(ContextMenu menu)
		{
			if(menu == null)
				return;

			menu.Closed += (sender, args) => IsAdditionalMenuOpen = false;
		}

		private void TurnMenuState(bool state)
		{
			if(AdditionalMenu == null)
				return;

			AdditionalMenu.PlacementTarget = _additionalBtn;
			AdditionalMenu.Placement = PlacementMode.Bottom;
			AdditionalMenu.IsOpen = state;
		}

		#endregion
	}
}