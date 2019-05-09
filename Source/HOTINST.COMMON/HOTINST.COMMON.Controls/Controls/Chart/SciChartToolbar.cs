/**
 * ==============================================================================
 *
 * ClassName: SciChartToolBar
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/6/27 9:13:39
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.ChartModifiers;
using Abt.Controls.SciChart.Visuals;
using Abt.Controls.SciChart.Visuals.Axes;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using HOTINST.COMMON.Controls.Net4._0.Commands;
using HOTINST.COMMON.Controls.Net4._0.VisualUtil;
using Microsoft.Win32;

namespace HOTINST.COMMON.Controls.Net4._0.Controls.Chart
{
	/// <summary>
	/// SciChartSurface 公用工具栏控件
	/// </summary>
	public class SciChartToolbar : ContentControl
	{
		#region fields
		
		private SimpleCommand _cmdExportImg;

		private SimpleCommand _cmdCanMouseWheel;
		private SimpleCommand _cmdCanShowRollover;

		private readonly RubberBandXyZoomModifier _rbzm;
		private readonly MouseWheelZoomModifier _mwzm;
		private readonly RolloverModifier _rlm;
		private readonly ZoomPanModifier _zpm;
		private readonly LegendModifier _lgm;

		private readonly ModifierGroup _modifierGroup;

		#endregion

		#region props

		/// <summary>
		/// 
		/// </summary>
		public static readonly DependencyProperty CmdShowHideWaveProperty = DependencyProperty.Register(
			"CmdShowHideWave", typeof(ICommand), typeof(SciChartToolbar), new PropertyMetadata(default(ICommand)));
		/// <summary>
		/// 
		/// </summary>
		public ICommand CmdShowHideWave
		{
			get => (ICommand)GetValue(CmdShowHideWaveProperty);
			set => SetValue(CmdShowHideWaveProperty, value);
		}

		#region 基础属性

		/// <summary>
		/// 定义依赖属性 ToolBarIdProperty
		/// </summary>
		public static readonly DependencyProperty ToolBarIdProperty = DependencyProperty.Register(
			"ToolBarId", typeof(int), typeof(SciChartToolbar), new PropertyMetadata(default(int)));
		/// <summary>
		/// 获取或设置工具栏ID
		/// </summary>
		public int ToolBarId
		{
			get => (int)GetValue(ToolBarIdProperty);
			set => SetValue(ToolBarIdProperty, value);
		}

		/// <summary>
		/// 定义依赖属性 TargetSurfaceProperty
		/// </summary>
		public static readonly DependencyProperty TargetSurfaceProperty = DependencyProperty.Register(
			"TargetSurface", typeof(ISciChartSurface), typeof(SciChartToolbar), new PropertyMetadata(default(ISciChartSurface), (o, args) => (o as SciChartToolbar)?.TargetSurfaceChanged(args.NewValue as ISciChartSurface)));
		/// <summary>
		/// 获取或设置目标图表
		/// </summary>
		public ISciChartSurface TargetSurface
		{
			get => (ISciChartSurface)GetValue(TargetSurfaceProperty);
			set => SetValue(TargetSurfaceProperty, value);
		}

		/// <summary>
		/// 定义依赖属性 IsSharedAxiesProperty
		/// </summary>
		public static readonly DependencyProperty IsSharedAxiesProperty = DependencyProperty.Register(
			"IsSharedAxies", typeof(bool), typeof(SciChartToolbar), new PropertyMetadata(default(bool)));
		/// <summary>
		/// 获取或设置是否要共享坐标轴
		/// </summary>
		public bool IsSharedAxies
		{
			get => (bool)GetValue(IsSharedAxiesProperty);
			set => SetValue(IsSharedAxiesProperty, value);
		}
		
		/// <summary>
		/// 定义依赖属性 IsRubberBandZoomEnabledProperty
		/// </summary>
		public static readonly DependencyProperty IsRubberBandZoomEnabledProperty = DependencyProperty.Register(
			"IsRubberBandZoomEnabled", typeof(bool), typeof(SciChartToolbar), new PropertyMetadata(default(bool), (o, args) =>
			{
				SciChartToolbar context = o as SciChartToolbar;
				context?.IsRubberBandZoomEnabledChanged((bool)args.NewValue);
				//context?.IsDragZoomEnabledChanged(context.IsDragZoomEnabled);
			}));
		/// <summary>
		/// 获取或设置是否启用框选缩放功能
		/// </summary>
		public bool IsRubberBandZoomEnabled
		{
			get => (bool)GetValue(IsRubberBandZoomEnabledProperty);
			set => SetValue(IsRubberBandZoomEnabledProperty, value);
		}

		/// <summary>
		/// 定义依赖属性 IsRubberBandZoomXOnlyProperty
		/// </summary>
		public static readonly DependencyProperty IsZoomXOnlyProperty = DependencyProperty.Register(
			"IsZoomXOnly", typeof(bool), typeof(SciChartToolbar), new PropertyMetadata(default(bool), (o, args) => (o as SciChartToolbar)?.IsZoomXOnlyChanged((bool)args.NewValue)));
		/// <summary>
		///  获取或设置框选缩放时是否只缩放X轴
		/// </summary>
		public bool IsZoomXOnly
		{
			get => (bool)GetValue(IsZoomXOnlyProperty);
			set => SetValue(IsZoomXOnlyProperty, value);
		}

		/// <summary>
		/// 定义依赖属性 IsDragZoomEnabledProperty
		/// </summary>
		public static readonly DependencyProperty IsDragZoomEnabledProperty = DependencyProperty.Register(
			"IsDragZoomEnabled", typeof(bool), typeof(SciChartToolbar), new PropertyMetadata(default(bool), (o, args) =>
			{
				SciChartToolbar context = o as SciChartToolbar;
				context?.IsDragZoomEnabledChanged((bool)args.NewValue);
				//context?.IsRubberBandZoomEnabledChanged(context.IsRubberBandZoomEnabled);
			}));
		/// <summary>
		///  获取或设置是否启用拖动缩放功能
		/// </summary>
		public bool IsDragZoomEnabled
		{
			get => (bool)GetValue(IsDragZoomEnabledProperty);
			set => SetValue(IsDragZoomEnabledProperty, value);
		}

		/// <summary>
		/// 定义依赖属性 IsAxisAutoRangeProperty
		/// </summary>
		public static readonly DependencyProperty IsAxisAutoRangeProperty = DependencyProperty.Register(
			"IsAxisAutoRange", typeof(bool), typeof(SciChartToolbar), new PropertyMetadata(default(bool), (o, args) => (o as SciChartToolbar)?.IsAxisAutoRangeChanged((bool)args.NewValue)));
		/// <summary>
		///  获取或设置坐标轴是否自动范围
		/// </summary>
		public bool IsAxisAutoRange
		{
			get => (bool)GetValue(IsAxisAutoRangeProperty);
			set => SetValue(IsAxisAutoRangeProperty, value);
		}

		/// <summary>
		/// 定义依赖属性 IsRolloverModifierEnabledProperty
		/// </summary>
		public static readonly DependencyProperty IsRolloverModifierEnabledProperty = DependencyProperty.Register(
			"IsRolloverModifierEnabled", typeof(bool), typeof(SciChartToolbar), new PropertyMetadata(default(bool), (o, args) => (o as SciChartToolbar)?.IsRolloverModifierEnabledChanged((bool)args.NewValue)));
		/// <summary>
		///  获取或设置是否显示光标值
		/// </summary>
		public bool IsRolloverModifierEnabled
		{
			get => (bool)GetValue(IsRolloverModifierEnabledProperty);
			set => SetValue(IsRolloverModifierEnabledProperty, value);
		}

		/// <summary>
		/// 定义依赖属性 UseInterpolationProperty
		/// </summary>
		public static readonly DependencyProperty UseInterpolationProperty = DependencyProperty.Register(
			"UseInterpolation", typeof(bool), typeof(SciChartToolbar), new PropertyMetadata(default(bool), (o, args) => (o as SciChartToolbar)?.UseInterpolationChanged((bool)args.NewValue)));
		/// <summary>
		///  获取或设置显示光标值时是否使用插值
		/// </summary>
		public bool UseInterpolation
		{
			get => (bool)GetValue(UseInterpolationProperty);
			set => SetValue(UseInterpolationProperty, value);
		}

		/// <summary>
		/// 定义依赖属性 ShowLegendProperty
		/// </summary>
		public static readonly DependencyProperty ShowLegendProperty = DependencyProperty.Register(
			"ShowLegend", typeof(bool), typeof(SciChartToolbar), new PropertyMetadata(default(bool), (o, args) => (o as SciChartToolbar)?.ShowLegendChanged((bool)args.NewValue)));
		/// <summary>
		///  获取或设置是否显示图例
		/// </summary>
		public bool ShowLegend
		{
			get => (bool)GetValue(ShowLegendProperty);
			set => SetValue(ShowLegendProperty, value);
		}

		/// <summary>
		/// 定义依赖属性 ExportImgTypeProperty
		/// </summary>
		public static readonly DependencyProperty ExportImgTypeProperty = DependencyProperty.Register(
			"ExportImgType", typeof(ExportType), typeof(SciChartToolbar), new PropertyMetadata(default(ExportType)));
		/// <summary>
		///  获取或设置导出图片类型
		/// </summary>
		public ExportType ExportImgType
		{
			get => (ExportType)GetValue(ExportImgTypeProperty);
			set => SetValue(ExportImgTypeProperty, value);
		}

		#endregion

		#region 加载缩略图按钮

		/// <summary>
		/// 定义依赖属性 ShowLoadThumbnailButtonProperty
		/// </summary>
		public static readonly DependencyProperty ShowLoadThumbnailButtonProperty = DependencyProperty.Register(
			"ShowLoadThumbnailButton", typeof(bool), typeof(SciChartToolbar), new PropertyMetadata(default(bool)));
		/// <summary>
		/// 获取或设置是否显示加载缩略图形的按钮
		/// </summary>
		public bool ShowLoadThumbnailButton
		{
			get => (bool)GetValue(ShowLoadThumbnailButtonProperty);
			set => SetValue(ShowLoadThumbnailButtonProperty, value);
		}

		/// <summary>
		/// 定义依赖属性 CmdLoadThumbnailProperty
		/// </summary>
		public static readonly DependencyProperty CmdLoadThumbnailProperty = DependencyProperty.Register(
			"CmdLoadThumbnail", typeof(ICommand), typeof(SciChartToolbar), new PropertyMetadata(default(ICommand)));

		/// <summary>
		/// 获取或设置加载缩略图形的命令
		/// </summary>
		public ICommand CmdLoadThumbnail
		{
			get => (ICommand)GetValue(CmdLoadThumbnailProperty);
			set => SetValue(CmdLoadThumbnailProperty, value);
		}

		/// <summary>
		/// 定义依赖属性 CmdLoadThumbnailParameterProperty
		/// </summary>
		public static readonly DependencyProperty CmdLoadThumbnailParameterProperty = DependencyProperty.Register(
			"CmdLoadThumbnailParameter", typeof(object), typeof(SciChartToolbar), new PropertyMetadata(default(object)));

		/// <summary>
		/// 获取或设置加载缩略图形的命令的参数
		/// </summary>
		public object CmdLoadThumbnailParameter
		{
			get => GetValue(CmdLoadThumbnailParameterProperty);
			set => SetValue(CmdLoadThumbnailParameterProperty, value);
		}

		#endregion

		#region 刷新详细波形按钮

		/// <summary>
		/// 定义依赖属性 ShowRefreshDetailButtonProperty
		/// </summary>
		public static readonly DependencyProperty ShowRefreshDetailButtonProperty = DependencyProperty.Register(
			"ShowRefreshDetailButton", typeof(bool), typeof(SciChartToolbar), new PropertyMetadata(default(bool)));
		/// <summary>
		/// 获取或设置是否显示加载详细图形的按钮
		/// </summary>
		public bool ShowRefreshDetailButton
		{
			get => (bool)GetValue(ShowRefreshDetailButtonProperty);
			set => SetValue(ShowRefreshDetailButtonProperty, value);
		}

		/// <summary>
		/// 定义依赖属性 CmdRefreshDetailProperty
		/// </summary>
		public static readonly DependencyProperty CmdRefreshDetailProperty = DependencyProperty.Register(
			"CmdRefreshDetail", typeof(ICommand), typeof(SciChartToolbar), new PropertyMetadata(default(ICommand)));
		/// <summary>
		/// 获取或设置加载详细图形的命令
		/// </summary>
		public ICommand CmdRefreshDetail
		{
			get => (ICommand)GetValue(CmdRefreshDetailProperty);
			set => SetValue(CmdRefreshDetailProperty, value);
		}

		/// <summary>
		/// 定义依赖属性 CmdRefreshDetailParameterProperty
		/// </summary>
		public static readonly DependencyProperty CmdRefreshDetailParameterProperty = DependencyProperty.Register(
			"CmdRefreshDetailParameter", typeof(object), typeof(SciChartToolbar), new PropertyMetadata(default(object)));
		/// <summary>
		/// 获取或设置加载详细图形的命令的参数
		/// </summary>
		public object CmdRefreshDetailParameter
		{
			get => GetValue(CmdRefreshDetailParameterProperty);
			set => SetValue(CmdRefreshDetailParameterProperty, value);
		}

		#endregion

		/// <summary>
		/// 
		/// </summary>
		public ICommand CmdCanMouseWheel => _cmdCanMouseWheel ?? (_cmdCanMouseWheel = new SimpleCommand(o => { }) { CanExecuteDelegate = CanMouseWheel });
		/// <summary>
		/// 
		/// </summary>
		public ICommand CmdCanShowRollover => _cmdCanShowRollover ?? (_cmdCanShowRollover = new SimpleCommand(o => { }) { CanExecuteDelegate = CanShowRollover });

		#endregion

		#region cmd

		/// <summary>
		/// 获取导出图片命令
		/// </summary>
		public ICommand CmdExportImg => _cmdExportImg ?? (_cmdExportImg = new SimpleCommand(ExportImg));
		
		#endregion

		#region .ctor

		/// <summary>
		/// 
		/// </summary>
		static SciChartToolbar()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(SciChartToolbar), new FrameworkPropertyMetadata(typeof(SciChartToolbar)));
		}

		/// <summary>
		/// 初始化类 <see cref="SciChartToolbar"/> 的新实例。
		/// </summary>
		public SciChartToolbar()
		{
			ToolBarId = Guid.NewGuid().GetHashCode();
			
			_rbzm = new RubberBandXyZoomModifier { IsEnabled = false };
			_mwzm = new MouseWheelZoomModifier { IsEnabled = false };
			_zpm = new ZoomPanModifier { IsEnabled = false };
			_rlm = new RolloverModifier { IsEnabled = false, ShowTooltipOn = ShowTooltipOptions.Always, LineOverlayStyle = ModifierHelper.CreateRolloverLineStyle(), TooltipLabelTemplate = ModifierHelper.CreateRolloverTooltipTemplate() };
			_lgm = new LegendModifier { LegendItemTemplate = CreateLegendItemTemplate() };

			_modifierGroup = new ModifierGroup(_rbzm, _zpm, _mwzm, _rlm, _lgm);

			_modifierGroup.ChildModifiers.Add(new ZoomExtentsModifier { IsEnabled = true });
			_modifierGroup.ChildModifiers.Add(new AutoAxesDragModifier { IsEnabled = true });
			_modifierGroup.ChildModifiers.Add(new SeriesSelectionModifier { SelectedSeriesStyle = ModifierHelper.CreateSelectedSeriesStyle() });
		}

		#endregion

		private DataTemplate CreateLegendItemTemplate()
		{
			// 最终模版
			DataTemplate template = new DataTemplate();

			// 根元素
			FrameworkElementFactory rootPanel = new FrameworkElementFactory(typeof(StackPanel));
			rootPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

			// 复选框
			FrameworkElementFactory cbVisible = new FrameworkElementFactory(typeof(CheckBox));
			cbVisible.SetBinding(ToggleButton.IsCheckedProperty, new Binding("RenderableSeries.IsVisible"));
			cbVisible.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(CbVisibleOnClick));

			// 曲线颜色拾取器
			FrameworkElementFactory colorEdit = new FrameworkElementFactory(typeof(PopupColorEdit));
			colorEdit.SetValue(PaddingProperty, new Thickness(-2.0));
			colorEdit.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Left);
			colorEdit.SetValue(VerticalAlignmentProperty, VerticalAlignment.Center);
			colorEdit.SetValue(ButtonEdit.AllowDefaultButtonProperty, false);
			colorEdit.SetValue(ButtonEdit.ShowEditorButtonsProperty, false);
			colorEdit.SetValue(PopupColorEdit.ShowDefaultColorButtonProperty, false);
			colorEdit.SetValue(PopupColorEdit.ShowNoColorButtonProperty, false);
			colorEdit.SetValue(PopupColorEdit.DisplayModeProperty, PopupColorEditDisplayMode.Color);
			colorEdit.SetBinding(PopupColorEdit.ColorProperty, new Binding("RenderableSeries.SeriesColor") { Mode = BindingMode.TwoWay });

			// 曲线名显示文本
			FrameworkElementFactory textBlock = new FrameworkElementFactory(typeof(TextBlock));
			textBlock.SetValue(MarginProperty, new Thickness(2.0, 0, 0, 0));
			textBlock.SetValue(VerticalAlignmentProperty, VerticalAlignment.Center);
			textBlock.SetBinding(TextBlock.TextProperty, new Binding("SeriesName"));
			textBlock.SetBinding(TextBlock.ForegroundProperty, new Binding("SeriesColor")
			{
				Converter = new ColorToBrushConverter()
			});

			rootPanel.AppendChild(cbVisible);
			rootPanel.AppendChild(colorEdit);
			rootPanel.AppendChild(textBlock);

			template.VisualTree = rootPanel;
			template.Seal();

			return template;
		}

		private void CbVisibleOnClick(object sender, RoutedEventArgs e)
		{
			CmdShowHideWave?.Execute(((CheckBox)e.Source)?.DataContext);
		}

		#region property changed event

		private void TargetSurfaceChanged(ISciChartSurface newChart)
		{
			if(newChart == null)
			{
				return;
			}

			((SciChartSurface)newChart).Loaded += (sender, args) =>
			{
				List<SciChartLegend> legends = VisualUtils.GetChildObjects<SciChartLegend>(sender as SciChartSurface, string.Empty);
				if(legends.Count > 0 && legends[0].Background is SolidColorBrush brush)
				{
					legends[0].Background = new SolidColorBrush(Color.FromArgb(30, brush.Color.R, brush.Color.G, brush.Color.B));
				}
			};

			newChart.ChartModifier = _modifierGroup;
			
			ShowLegend = true;
			IsAxisAutoRange = true;
			IsZoomXOnly = true;
		}
		
		private void IsRubberBandZoomEnabledChanged(bool newValue)
		{
			if(_rbzm != null)
			{
				_rbzm.IsEnabled = newValue;
			}
			if(_mwzm != null && newValue)
			{
				_mwzm.IsEnabled = true;
			}
		}

		private void IsZoomXOnlyChanged(bool newValue)
		{
			if(_rbzm != null)
			{
				_rbzm.IsXAxisOnly = newValue;
			}
			if(_mwzm != null)
			{
				_mwzm.XyDirection = newValue ? XyDirection.XDirection : XyDirection.XYDirection;
			}
		}

		private void IsDragZoomEnabledChanged(bool newValue)
		{
			if(_zpm != null)
			{
				_zpm.IsEnabled = newValue;
			}
			if(_mwzm != null && newValue)
			{
				_mwzm.IsEnabled = true;
			}
		}
		
		private void IsAxisAutoRangeChanged(bool newValue)
		{
			if(TargetSurface?.XAxes != null)
			{
				foreach(IAxis axis in TargetSurface.XAxes)
				{
					axis.AutoRange = newValue ? AutoRange.Always : AutoRange.Never;
				}
			}
			if(TargetSurface?.YAxes != null)
			{
				foreach(IAxis axis in TargetSurface.YAxes)
				{
					axis.AutoRange = newValue ? AutoRange.Always : AutoRange.Never;
				}
			}
			if(_mwzm != null && newValue)
			{
				_mwzm.IsEnabled = false;
			}
		}

		private void IsRolloverModifierEnabledChanged(bool newValue)
		{
			if(_rlm != null)
			{
				_rlm.IsEnabled = newValue;
			}
		}

		private void UseInterpolationChanged(bool newValue)
		{
			if(_rlm != null)
			{
				_rlm.UseInterpolation = newValue;
			}
		}

		private void ShowLegendChanged(bool newValue)
		{
			if(_lgm != null)
			{
				_lgm.ShowLegend = newValue;
			}
		}

		#endregion

		#region event handler

		private bool CanMouseWheel(object o)
		{
			if(TargetSurface?.SeriesSource != null)
			{
				bool result = false;
				foreach(IChartSeriesViewModel seriesViewModel in TargetSurface.SeriesSource)
				{
					if(seriesViewModel.DataSeries.HasValues)
					{
						result = true;
					}
				}
				if(!result)
				{
					_mwzm.IsEnabled = false;
				}
			}

			return false;
		}

		private bool CanShowRollover(object o)
		{
			if(TargetSurface?.XAxes?.Count == 0 || TargetSurface?.YAxes?.Count == 0)
			{
				_rlm.IsEnabled = false;
			}

			return false;
		}

		private void ExportImg(object o)
		{
			SaveFileDialog dlg = new SaveFileDialog
			{
				Title = "导出图片...",
				DefaultExt = ExportImgType.ToString(),
				Filter = "图片(*." + ExportImgType + ")|*." + ExportImgType + "|所有文件(*.*)|*.*",
				FileName = DateTime.Now.ToString("MMddHHmmssfff"),
				OverwritePrompt = true,
				RestoreDirectory = true
			};
			if(dlg.ShowDialog() != true)
				return;

			((SciChartSurface)TargetSurface)?.ExportToFile(dlg.FileName, ExportImgType);
		}
		
		#endregion
	}
}