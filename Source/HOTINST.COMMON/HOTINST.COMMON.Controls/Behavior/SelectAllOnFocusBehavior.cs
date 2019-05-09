/**
 * ==============================================================================
 *
 * ClassName: SelectAllOnFocusBehavior
 * Description: 
 *
 * Version: 1.0
 * Created: 2018/5/14 15:58:23
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

namespace HOTINST.COMMON.Controls.Behavior
{
	/// <summary>
	/// 获得焦点时全选
	/// </summary>
	public class SelectAllOnFocusBehavior
	{
		#region props

		/// <summary>
		/// IsEnabledProperty
		/// </summary>
		public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
			"IsEnabled", typeof(bool), typeof(SelectAllOnFocusBehavior), new PropertyMetadata(default(bool), OnValueChanged));

		/// <summary>
		/// 是否启用
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
		public static void SetIsEnabled(DependencyObject element, bool value)
		{
			element.SetValue(IsEnabledProperty, value);
		}

		/// <summary>
		/// 是否启用
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static bool GetIsEnabled(DependencyObject element)
		{
			return (bool)element.GetValue(IsEnabledProperty);
		}

		#endregion

		private static void OnValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			if(dependencyObject is TextBox uiElement)
			{
				if(e.NewValue is bool val && val)
				{
					uiElement.GotFocus += UIElementOnGotFocus;
				}
				else
				{
					uiElement.GotFocus -= UIElementOnGotFocus;
				}
			}
		}

		private static void UIElementOnGotFocus(object sender, RoutedEventArgs e)
		{
			if(sender is TextBox textBox)
			{
				textBox.SelectAll();
			}
		}
	}
}