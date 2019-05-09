/**
 * ==============================================================================
 *
 * ClassName: Attaches
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/4/20 14:20:31
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System.Windows;
using System.Windows.Input;

namespace HOTINST.COMMON.Controls.Attaches
{
	/// <summary>
	/// 附加属性管理类
	/// </summary>
	public class Attaches
	{
		/// <summary>
		/// 定义附加属性 InputBindingsProperty
		/// </summary>
		public static readonly DependencyProperty InputBindingsProperty = DependencyProperty.RegisterAttached(
			"InputBindings", typeof(InputBindingCollection), typeof(Attaches), new PropertyMetadata(default(InputBindingCollection),
				(o, args) =>
				{
					UIElement element = o as UIElement;
					if(element == null)
						return;
					element.InputBindings.Clear();
					element.InputBindings.AddRange((InputBindingCollection)args.NewValue);
				}));
		/// <summary>
		/// 设置附加属性 InputBindings
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
		public static void SetInputBindings(DependencyObject element, InputBindingCollection value)
		{
			element.SetValue(InputBindingsProperty, value);
		}
		/// <summary>
		/// 获取附加属性 InputBindings
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static InputBindingCollection GetInputBindings(DependencyObject element)
		{
			return (InputBindingCollection)element.GetValue(InputBindingsProperty);
		}

		/// <summary>
		/// 定义附加属性 CommandBindingsProperty
		/// </summary>
		public static readonly DependencyProperty CommandBindingsProperty = DependencyProperty.RegisterAttached(
			"CommandBindings", typeof(CommandBindingCollection), typeof(Attaches), new PropertyMetadata(default(CommandBindingCollection),
				(o, args) =>
				{
					UIElement element = o as UIElement;
					if(element == null)
						return;
					element.CommandBindings.Clear();
					element.CommandBindings.AddRange((CommandBindingCollection)args.NewValue);
				}));
		/// <summary>
		/// 设置附加属性 CommandBindings
		/// </summary>
		/// <param name="element"></param>
		/// <param name="value"></param>
		public static void SetCommandBindings(DependencyObject element, CommandBindingCollection value)
		{
			element.SetValue(CommandBindingsProperty, value);
		}
		/// <summary>
		/// 获取附加属性 CommandBindings
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static CommandBindingCollection GetCommandBindings(DependencyObject element)
		{
			return (CommandBindingCollection)element.GetValue(CommandBindingsProperty);
		}
	}
}