/**
 * ==============================================================================
 *
 * ClassName: MarginParamsExtension
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/3/29 10:05:27
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
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace HOTINST.COMMON.Controls.Extension.AnimationExtension
{
	/// <summary>
	/// Margin动画扩展
	/// </summary>
	[MarkupExtensionReturnType(typeof(MarginParamsExtension))]
	public class MarginParamsExtension : BaseTransitionzParams<Thickness>
	{
		#region .ctor

		/// <summary>
		/// 初始化 MarginParamsExtension 类的新实例。
		/// </summary>
		public MarginParamsExtension()
		{

		}

		/// <summary>
		/// 初始化 MarginParamsExtension 类的新实例。
		/// </summary>
		/// <param name="beginTime"></param>
		/// <param name="duration"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="ease"></param>
		public MarginParamsExtension(double beginTime, double duration, Thickness from, Thickness to, EasingFunctionBase ease)
			: base(beginTime, duration, from, to, ease)
		{

		}

		#endregion

		/// <summary>
		/// 在派生类中实现时，返回一个对象，此对象被设置为此标记扩展的目标属性的值。
		/// </summary>
		/// <param name="serviceProvider">可以为标记扩展提供服务的对象。</param>
		/// <returns>要在应用了扩展的属性上设置的对象值。</returns>
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return this;
		}
	}
}