/**
 * ==============================================================================
 *
 * ClassName: ModifierHelper
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/6/30 9:56:40
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;

namespace HOTINST.COMMON.Controls.Net4._0.Controls.Chart
{
	/// <summary>
	/// ModifierHelper
	/// </summary>
	public static class ModifierHelper
	{
		private const string RolloverTooltipTemplate = "<ControlTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:s=\"http://schemas.abtsoftware.co.uk/scichart\"><ControlTemplate.Resources><s:ColorToBrushConverter x:Key=\"ColorToBrushCvt\"/></ControlTemplate.Resources><Border Background=\"#3F808080\" BorderBrush=\"#A8A9A9A9\" BorderThickness=\"1\" CornerRadius=\"2\" Padding=\"2\"><StackPanel><TextBlock FontSize=\"12\" Text=\"{Binding SeriesName}\" Foreground=\"{Binding SeriesColor,Converter={StaticResource ColorToBrushCvt}}\"/><TextBlock FontSize=\"12\" Text=\"{Binding Value}\" Foreground=\"{Binding SeriesColor,Converter={StaticResource ColorToBrushCvt}}\"/></StackPanel></Border></ControlTemplate>";
		private const string LegendItemTemplate = "<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:s=\"http://schemas.abtsoftware.co.uk/scichart\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\" DataType=\"s:SeriesInfo\"><DataTemplate.Resources><s:ColorToBrushConverter x:Key=\"ColorToBrushCvt\"/></DataTemplate.Resources><StackPanel Orientation=\"Horizontal\"><CheckBox x:Name=\"PART_CbVisible\" IsChecked=\"{Binding RenderableSeries.IsVisible}\"/><dxe:PopupColorEdit HorizontalAlignment=\"Left\" VerticalAlignment=\"Center\" AllowDefaultButton=\"False\" ShowDefaultColorButton=\"False\" ShowNoColorButton=\"False\" ShowEditorButtons=\"False\" DisplayMode=\"Color\" Padding=\"-2\" Color=\"{Binding RenderableSeries.SeriesColor, Mode=TwoWay}\"/><TextBlock Text=\"{Binding SeriesName}\" Foreground=\"{Binding SeriesColor, Converter={StaticResource ColorToBrushCvt}}\" Margin=\"2,0,0,0\" VerticalAlignment=\"Center\"/></StackPanel></DataTemplate>";
		private const string PointMarkerTemplate = "<ControlTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:s=\"http://schemas.abtsoftware.co.uk/scichart\"><s:EllipsePointMarker Width=\"5\" Height=\"5\" Stroke=\"White\" Fill=\"Transparent\"/></ControlTemplate>";

		/// <summary>
		/// 创建选中曲线的样式
		/// </summary>
		/// <returns></returns>
		public static Style CreateSelectedSeriesStyle()
		{
			return new Style(typeof(BaseRenderableSeries))
			{
				Setters =
				{
					new Setter(BaseRenderableSeries.PointMarkerTemplateProperty, CreatePointMarkerTemplate())
				}
			};
		}

		/// <summary>
		/// 创建光标竖线的样式
		/// </summary>
		/// <returns></returns>
		public static Style CreateRolloverLineStyle()
		{
			return new Style(typeof(Line))
			{
				Setters =
				{
					new Setter(UIElement.IsHitTestVisibleProperty, false),
					new Setter(Shape.StrokeThicknessProperty, 1.0),
					new Setter(Shape.StrokeProperty, new SolidColorBrush(Color.FromArgb(192, 128, 128, 128)))
				}
			};
		}

		/// <summary>
		/// 创建光标显示坐标的模版
		/// </summary>
		/// <returns></returns>
		public static ControlTemplate CreateRolloverTooltipTemplate()
		{
			return (ControlTemplate)XamlReader.Parse(RolloverTooltipTemplate);
		}

		/// <summary>
		/// 创建图例模版
		/// </summary>
		/// <returns></returns>
		public static DataTemplate CreateLegendItemTemplate()
		{
/*
    <s:LegendModifier.LegendItemTemplate>
	    <DataTemplate DataType="s:SeriesInfo">
			<StackPanel Orientation="Horizontal">
				<CheckBox IsChecked="{Binding RenderableSeries.IsVisible}">
					<i:Interaction.Triggers>
						<i:EventTrigger EventName="Click">
							<i:InvokeCommandAction Command="{Binding DataContext.CmdShowHideSeries, ElementName=sciChart}" CommandParameter="{Binding}"/>
						</i:EventTrigger>
					</i:Interaction.Triggers>
				</CheckBox>
				<dxe:PopupColorEdit HorizontalAlignment="Left" VerticalAlignment="Center" AllowDefaultButton="False"
				                    ShowDefaultColorButton="False" ShowNoColorButton="False" ShowEditorButtons="False"
				                    DisplayMode="Color" Padding="-2"
				                    Color="{Binding RenderableSeries.SeriesColor, Mode=TwoWay}"/>
				<TextBlock Text="{Binding SeriesName}" Foreground="{Binding SeriesColor, Converter={StaticResource ColorToBrushCvt}}" Margin="2,0,0,0" VerticalAlignment="Center"/>
			</StackPanel>
		</DataTemplate>
	</s:LegendModifier.LegendItemTemplate>
* 
*/

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
			colorEdit.SetValue(Control.PaddingProperty, new Thickness(-2.0));
			colorEdit.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left);
			colorEdit.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
			colorEdit.SetValue(ButtonEdit.AllowDefaultButtonProperty, false);
			colorEdit.SetValue(ButtonEdit.ShowEditorButtonsProperty, false);
			colorEdit.SetValue(PopupColorEdit.ShowDefaultColorButtonProperty, false);
			colorEdit.SetValue(PopupColorEdit.ShowNoColorButtonProperty, false);
			colorEdit.SetValue(PopupColorEdit.DisplayModeProperty, PopupColorEditDisplayMode.Color);
			colorEdit.SetBinding(PopupColorEdit.ColorProperty, new Binding("RenderableSeries.SeriesColor") { Mode = BindingMode.TwoWay });

			// 曲线名显示文本
			FrameworkElementFactory textBlock = new FrameworkElementFactory(typeof(TextBlock));
			textBlock.SetValue(FrameworkElement.MarginProperty, new Thickness(2.0, 0, 0, 0));
			textBlock.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
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

		private static void CbVisibleOnClick(object sender, RoutedEventArgs e)
		{
			var a = SciChartToolbar.CmdShowHideWaveProperty.GetMetadata(typeof(DependencyObject)).DefaultValue;
		}

		/// <summary>
		/// 创建数据点的标记模版
		/// </summary>
		/// <returns></returns>
		public static ControlTemplate CreatePointMarkerTemplate()
		{
			return (ControlTemplate)XamlReader.Parse(PointMarkerTemplate);
		}
	}
}