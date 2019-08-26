/**
 * ==============================================================================
 *
 * ClassName: FeaturedSciChart
 * Description: 在 SciChartSurface 基础上提供属性 SelectedSeries
 *
 * Version: 1.0
 * Created: 2017/7/3 8:50:38
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System.Collections.Specialized;
using System.Windows;
using Abt.Controls.SciChart.Visuals;
using Abt.Controls.SciChart.Visuals.RenderableSeries;

namespace HOTINST.COMMON.Controls.Net4._0.Controls.Chart
{
	/// <summary>
	/// 提供 <see cref="SciChartSurface"/> 图表额外的功能
	/// </summary>
	public class FeaturedSciChart : SciChartSurface
	{
		#region props

		/// <summary>
		/// 定义依赖属性 SelectedSeriesProperty
		/// </summary>
		public static readonly DependencyProperty SelectedSeriesProperty = DependencyProperty.Register(
			"SelectedSeries", typeof(IRenderableSeries), typeof(FeaturedSciChart), new PropertyMetadata(default(IRenderableSeries)));
		/// <summary>
		/// 获取或设置图表当前选中的曲线。
		/// </summary>
		public IRenderableSeries SelectedSeries
		{
			get => (IRenderableSeries)GetValue(SelectedSeriesProperty);
			set => SetValue(SelectedSeriesProperty, value);
		}

		#endregion

		#region .ctor

		/// <summary>
		/// 初始化类 <see cref="FeaturedSciChart"/> 的新实例。
		/// </summary>
		public FeaturedSciChart()
		{
			SelectedRenderableSeries.CollectionChanged += SelectedRenderableSeriesOnCollectionChanged;
		}

		#endregion

		private void SelectedRenderableSeriesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if(e.NewItems != null && e.NewItems.Count == 1)
			{
				SelectedSeries = e.NewItems[0] as IRenderableSeries;
			}
		}
	}
}