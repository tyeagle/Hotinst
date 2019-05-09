/**
 * ==============================================================================
 *
 * ClassName: AutoAxesDragModifier
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/6/29 11:14:27
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Collections.Specialized;
using System.Linq;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.ChartModifiers;
using Abt.Controls.SciChart.Visuals.Axes;

namespace HOTINST.COMMON.Controls.Net4._0.Controls.Chart
{
	/// <summary>
	/// 自动为每个坐标轴添加可拖动行为
	/// </summary>
	public class AutoAxesDragModifier : ChartModifierBase
	{
		#region .ctor

		/// <summary>
		/// 初始化类 <see cref="AutoAxesDragModifier"/> 的新实例。
		/// </summary>
		public AutoAxesDragModifier()
		{

		}

		#endregion

		#region Overrides of ChartModifierBase

		/// <summary>
		/// Called with the <see cref="P:Abt.Controls.SciChart.Visuals.SciChartSurface.XAxes" /> <see cref="T:Abt.Controls.SciChart.AxisCollection" /> changes. Overridden in derived classes to get notification of this event
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="T:System.Collections.Specialized.NotifyCollectionChangedEventArgs" /> instance containing the event data.</param>
		protected override void OnXAxesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if(e.OldItems != null)
			{
				if(ParentSurface.ChartModifier is ModifierGroup group)
				{
					foreach(IAxis axis in e.OldItems)
					{
						IChartModifier cm = null;
						try
						{
							cm = group.ChildModifiers.FirstOrDefault(m => m.XAxis == axis);
						}
						catch
						{
							// ignored
						}
						if(cm != null)
						{
							group.ChildModifiers.Remove(cm);
						}
					}
				}
			}
			if(e.NewItems != null)
			{
				if(ParentSurface.ChartModifier is ModifierGroup group)
				{
					foreach(IAxis axis in e.NewItems)
					{
						group.ChildModifiers.Add(new XAxisDragModifier
						{
							AxisId = axis.Id,
							ReceiveHandledEvents = true,
							ClipModeX = ClipMode.None
						});
					}
				}
			}
		}

		/// <summary>
		/// Called with the <see cref="P:Abt.Controls.SciChart.Visuals.SciChartSurface.XAxes" /> <see cref="T:Abt.Controls.SciChart.AxisCollection" /> changes. Overridden in derived classes to get notification of this event
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="T:System.Collections.Specialized.NotifyCollectionChangedEventArgs" /> instance containing the event data.</param>
		protected override void OnYAxesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if(e.OldItems != null)
			{
				if(ParentSurface.ChartModifier is ModifierGroup group)
				{
					foreach(IAxis axis in e.OldItems)
					{
						IChartModifier cm = null;
						try
						{
							cm = group.ChildModifiers.FirstOrDefault(m => m.YAxis == axis);
						}
						catch
						{
							// ignored
						}
						if(cm != null)
						{
							group.ChildModifiers.Remove(cm);
						}
					}
				}
			}
			if(e.NewItems != null)
			{
				if(ParentSurface.ChartModifier is ModifierGroup group)
				{
					foreach(IAxis axis in e.NewItems)
					{
						group.ChildModifiers.Add(new YAxisDragModifier
						{
							AxisId = axis.Id,
							ReceiveHandledEvents = true
						});
					}
				}
			}
		}
		
		#endregion
	}
}