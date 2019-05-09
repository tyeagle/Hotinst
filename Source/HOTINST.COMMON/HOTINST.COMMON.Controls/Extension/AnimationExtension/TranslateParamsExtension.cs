/**
 * ==============================================================================
 *
 * ClassName: TranslateParamsExtension
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/3/29 10:09:08
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
	/// 为Translate动画提供参数
	/// </summary>
	public interface ITranslateParams
	{
		/// <summary>
		/// 开始时间
		/// </summary>
		double BeginTime { get; set; }
		/// <summary>
		/// 结束时间
		/// </summary>
		double Duration { get; set; }
		/// <summary>
		/// 起始值
		/// </summary>
		Point From { get; set; }
		/// <summary>
		/// 结束值
		/// </summary>
		Point To { get; set; }
		/// <summary>
		/// 缓动方法
		/// </summary>
		EasingFunctionBase Ease { get; set; }
		/// <summary>
		/// 指定 System.Windows.Media.Animation.Timeline 在超出活动期但其父级仍在活动期或保持期内时的行为方式。
		/// </summary>
		FillBehavior FillBehavior { get; set; }
	}

	/// <summary>
	/// Translate动画参数实现类
	/// </summary>
	public class TranslateParams : ITranslateParams
	{
		/// <summary>
		/// 开始时间
		/// </summary>
		public double BeginTime { get; set; }
		/// <summary>
		/// 结束时间
		/// </summary>
		public double Duration { get; set; }
		/// <summary>
		/// 起始值
		/// </summary>
		public Point From { get; set; }
		/// <summary>
		/// 结束值
		/// </summary>
		public Point To { get; set; }
		/// <summary>
		/// 缓动方法
		/// </summary>
		public EasingFunctionBase Ease { get; set; }
		/// <summary>
		/// 指定 System.Windows.Media.Animation.Timeline 在超出活动期但其父级仍在活动期或保持期内时的行为方式。
		/// </summary>
		public FillBehavior FillBehavior { get; set; }
	}

	/// <summary>
	/// 为Translate提供动画
	/// </summary>
	[MarkupExtensionReturnType(typeof(ITranslateParams))]
	public class TranslateParamsExtension : BaseTransitionzParams<Point>, ITranslateParams
	{
		#region .ctor

		/// <summary>
		/// 初始化 TranslateParamsExtension 类的新实例。
		/// </summary>
		public TranslateParamsExtension()
		{

		}

		/// <summary>
		/// 初始化 TranslateParamsExtension 类的新实例。
		/// </summary>
		/// <param name="beginTime"></param>
		/// <param name="duration"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="ease"></param>
		public TranslateParamsExtension(double beginTime, double duration, Point from, Point to, EasingFunctionBase ease)
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