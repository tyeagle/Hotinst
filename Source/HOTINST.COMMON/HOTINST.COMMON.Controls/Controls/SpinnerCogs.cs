/**
 * ==============================================================================
 *
 * ClassName: SpinnerCogs
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/9/28 19:19:07
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

namespace HOTINST.COMMON.Controls.Controls
{
	/// <summary>
	/// 
	/// </summary>
	public class SpinnerCogs : Control
	{
		#region fields

		/// <summary>
		/// root
		/// </summary>
		protected Grid PART_RootGrid;

		#endregion

		#region props

		/// <summary>
		/// Identifies the <see cref="LoadingIndicator.IsActive"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(
			"IsActive", typeof(bool), typeof(SpinnerCogs), new PropertyMetadata(true, (o, e) =>
			{
				SpinnerCogs li = (SpinnerCogs)o;

				if(li.PART_RootGrid == null)
				{
					return;
				}

				if((bool)e.NewValue == false)
				{
					VisualStateManager.GoToElementState(li.PART_RootGrid, "Inactive", false);
					li.PART_RootGrid.Visibility = Visibility.Collapsed;
				}
				else
				{
					VisualStateManager.GoToElementState(li.PART_RootGrid, "Active", false);
					li.PART_RootGrid.Visibility = Visibility.Visible;

					foreach(VisualStateGroup group in VisualStateManager.GetVisualStateGroups(li.PART_RootGrid))
					{
						if(group.Name == "ActiveStates")
						{
							foreach(VisualState state in group.States)
							{
								if(state.Name == "Active")
								{
									state.Storyboard.SetSpeedRatio(li.PART_RootGrid, li.SpeedRatio);
								}
							}
						}
					}
				}
			}));
		/// <summary>
		/// Get/set whether the loading indicator is active.
		/// </summary>
		public bool IsActive
		{
			get => (bool)GetValue(IsActiveProperty);
			set => SetValue(IsActiveProperty, value);
		}

		/// <summary>
		/// Identifies the <see cref="LoadingIndicator.SpeedRatio"/> dependency property.
		/// </summary>
		public static readonly DependencyProperty SpeedRatioProperty = DependencyProperty.Register(
			"SpeedRatio", typeof(double), typeof(SpinnerCogs), new PropertyMetadata(1d, (o, e) =>
			{
				SpinnerCogs li = (SpinnerCogs)o;

				if(li.PART_RootGrid == null || li.IsActive == false)
				{
					return;
				}

				foreach(VisualStateGroup group in VisualStateManager.GetVisualStateGroups(li.PART_RootGrid))
				{
					if(group.Name == "ActiveStates")
					{
						foreach(VisualState state in group.States)
						{
							if(state.Name == "Active")
							{
								state.Storyboard.SetSpeedRatio(li.PART_RootGrid, (double)e.NewValue);
							}
						}
					}
				}
			}));

		/// <summary>
		/// Get/set the speed ratio of the animation.
		/// </summary>
		public double SpeedRatio
		{
			get => (double)GetValue(SpeedRatioProperty);
			set => SetValue(SpeedRatioProperty, value);
		}

		#endregion

		#region .ctor

		static SpinnerCogs()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(SpinnerCogs), new FrameworkPropertyMetadata(typeof(SpinnerCogs)));
		}

		/// <summary>
		/// 
		/// </summary>
		public SpinnerCogs()
		{

		}

		#endregion

		#region Overrides of FrameworkElement

		/// <summary>在派生类中重写后，每当应用程序代码或内部进程调用 <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />，都将调用此方法。</summary>
		public override void OnApplyTemplate()
		{
			PART_RootGrid = (Grid)GetTemplateChild("RootGrid");

			if(PART_RootGrid != null)
			{
				VisualStateManager.GoToElementState(PART_RootGrid, IsActive ? "Active" : "Inactive", false);
				foreach(VisualStateGroup group in VisualStateManager.GetVisualStateGroups(PART_RootGrid))
				{
					if(group.Name == "ActiveStates")
					{
						foreach(VisualState state in group.States)
						{
							if(state.Name == "Active")
							{
								state.Storyboard.SetSpeedRatio(PART_RootGrid, SpeedRatio);
							}
						}
					}
				}

				PART_RootGrid.Visibility = IsActive ? Visibility.Visible : Visibility.Collapsed;
			}

			base.OnApplyTemplate();
		}

		#endregion
	}
}