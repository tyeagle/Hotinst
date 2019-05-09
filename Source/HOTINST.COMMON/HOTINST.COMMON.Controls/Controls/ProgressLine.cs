/**
 * ==============================================================================
 *
 * ClassName: ProgressLine
 * Description: 
 *
 * Version: 1.0
 * Created: 2018/10/12 14:58:28
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

#pragma warning disable CS1591

namespace HOTINST.COMMON.Controls.Controls
{
	public class ProgressLine : ItemsControl
	{
		#region fields

		#endregion

		#region props

		public static readonly DependencyProperty CurrentPositionProperty = DependencyProperty.Register(
			"CurrentPosition", typeof(int), typeof(ProgressLine), new PropertyMetadata(1, (o, args) =>
			{
				if(((ProgressLine)o).Items.Count == 0)
				{
					return;
				}
				if((int)args.NewValue > ((ProgressLine)o).Items.Count + 1)
				{
					((ProgressLine)o).CurrentPosition = (int)args.OldValue;
				}
				if((int)args.NewValue < 1)
				{
					((ProgressLine)o).CurrentPosition = 1;
				}
			}));

		public int CurrentPosition
		{
			get => (int)GetValue(CurrentPositionProperty);
			set => SetValue(CurrentPositionProperty, value);
		}

		#endregion

		#region .ctor

		static ProgressLine()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressLine), new FrameworkPropertyMetadata(typeof(ProgressLine)));
		}

		public ProgressLine()
		{
			
		}

		#endregion
	}
}