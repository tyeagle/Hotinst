/**
 * ==============================================================================
 *
 * ClassName: UITools
 * Description: 
 *
 * Version: 1.0
 * Created: 2018/1/22 9:38:12
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
using System.Windows;

namespace HOTINST.COMMON.Controls.Behavior
{
	/// <summary>
	/// UI 工具类
	/// </summary>
	public static class UITools
	{
		/// <summary>
		/// 订阅值变化事件
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <param name="property"></param>
		/// <param name="handler"></param>
		public static void AddValueChanged<T>(this T obj, DependencyProperty property, EventHandler handler)
			where T : DependencyObject
		{
			var desc = DependencyPropertyDescriptor.FromProperty(property, typeof(T));
			desc.AddValueChanged(obj, handler);
		}
		/// <summary>
		/// 移除值变化事件
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <param name="property"></param>
		/// <param name="handler"></param>
		public static void RemoveValueChanged<T>(this T obj, DependencyProperty property, EventHandler handler)
			where T : DependencyObject
		{
			var desc = DependencyPropertyDescriptor.FromProperty(property, typeof(T));
			desc.RemoveValueChanged(obj, handler);
		}
	}
}