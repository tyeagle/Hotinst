/**
 * ==============================================================================
 *
 * ClassName: DataGridBehavior
 * Description: 
 *
 * Version: 1.0
 * Created: 2018/3/12 10:44:23
 * Compiler: Visual Studio 2017
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HOTINST.COMMON.Controls.Attaches
{
	/// <summary>
	/// DataGrid 附加属性
	/// </summary>
	public static class DataGridEx
	{
		#region UseDisplayNameProperty

		/// <summary>
		/// UseDisplayNameProperty
		/// </summary>
		public static readonly DependencyProperty UseDisplayNameProperty = DependencyProperty.RegisterAttached(
			"UseDisplayName", typeof(bool), typeof(DataGridEx), new PropertyMetadata(default(bool), UseDisplayNameChanged));

		/// <summary>
		/// 获取是否对绑定列使用DisplayName属性
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
		public static void SetUseDisplayName(DependencyObject element, bool value)
		{
			element.SetValue(UseDisplayNameProperty, value);
		}

		/// <summary>
		/// 设置是否对绑定列使用DisplayName属性
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static bool GetUseDisplayName(DependencyObject element)
		{
			return (bool)element.GetValue(UseDisplayNameProperty);
		}

		private static void UseDisplayNameChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			if(obj is DataGrid dg)
			{
				if((bool)e.NewValue)
				{
					dg.AutoGeneratingColumn += DgOnAutoGeneratingColumnDisplayName;
				}
				else
				{
					dg.AutoGeneratingColumn -= DgOnAutoGeneratingColumnDisplayName;
				}
			}
		}

		private static void DgOnAutoGeneratingColumnDisplayName(object sender, DataGridAutoGeneratingColumnEventArgs e)
		{
			if(e.PropertyDescriptor is PropertyDescriptor propDesc)
			{
				DisplayNameAttribute attr = propDesc.Attributes.Cast<Attribute>().FirstOrDefault(a => a is DisplayNameAttribute) as DisplayNameAttribute;
				e.Column.Header = attr?.DisplayName ?? e.PropertyName;
			}
		}

		#endregion

		#region UseBrowsableProperty

		/// <summary>
		/// UseBrowsableProperty
		/// </summary>
		public static readonly DependencyProperty UseBrowsableProperty = DependencyProperty.RegisterAttached(
			"UseBrowsable", typeof(bool), typeof(DataGridEx), new PropertyMetadata(default(bool), UseBrowsableChanged));
		/// <summary>
		/// 设置是否对绑定列使用Browsable属性
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
		public static void SetUseBrowsable(DependencyObject element, bool value)
		{
			element.SetValue(UseBrowsableProperty, value);
		}
		/// <summary>
		/// 获取是否对绑定列使用Browsable属性
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static bool GetUseBrowsable(DependencyObject element)
		{
			return (bool)element.GetValue(UseBrowsableProperty);
		}

		private static void UseBrowsableChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			if(obj is DataGrid dg)
			{
				if((bool)e.NewValue)
				{
					dg.AutoGeneratingColumn += DgOnAutoGeneratingColumnBrowsable;
				}
				else
				{
					dg.AutoGeneratingColumn -= DgOnAutoGeneratingColumnBrowsable;
				}
			}
		}

		private static void DgOnAutoGeneratingColumnBrowsable(object sender, DataGridAutoGeneratingColumnEventArgs e)
		{
			if(e.PropertyDescriptor is PropertyDescriptor propDesc)
			{
				BrowsableAttribute attr = propDesc.Attributes.Cast<Attribute>().FirstOrDefault(a => a is BrowsableAttribute) as BrowsableAttribute;
				e.Cancel = !attr?.Browsable ?? false;
			}
		}

		#endregion
	}
}