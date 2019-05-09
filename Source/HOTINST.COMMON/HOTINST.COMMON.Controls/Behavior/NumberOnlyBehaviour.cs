/**
 * ==============================================================================
 *
 * ClassName: NumberOnlyBehaviour
 * Description: 
 *
 * Version: 1.0
 * Created: 2018/5/14 15:47:56
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
	/// 只允许输入框输入数字
	/// </summary>
	public class NumberOnlyBehaviour
	{
		#region props

		/// <summary>
		/// IsEnabledProperty
		/// </summary>
		public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
			"IsEnabled", typeof(bool), typeof(NumberOnlyBehaviour), new PropertyMetadata(default(bool), OnValueChanged));
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
					uiElement.TextChanged += UIElementOnTextChanged;
				}
				else
				{
					uiElement.TextChanged -= UIElementOnTextChanged;
				}
			}
		}
		
		private static void UIElementOnTextChanged(object sender, TextChangedEventArgs e)
		{
			if(sender is TextBox textBox)
			{
				TextChange[] change = new TextChange[e.Changes.Count];
				e.Changes.CopyTo(change, 0);
				int offset = change[0].Offset;
				if(change[0].AddedLength > 0)
				{
					if(!double.TryParse(textBox.Text, out _))
					{
						textBox.Text = textBox.Text.Remove(offset, change[0].AddedLength);
						textBox.Select(offset, 0);
					}
				}
			}
		}
	}
}