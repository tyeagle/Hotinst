/**
 * ==============================================================================
 *
 * ClassName: BaseTransitionzParams
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/3/29 9:42:48
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace HOTINST.COMMON.Controls.Extension.AnimationExtension
{
	/// <summary>
	/// 提供动画扩展基类
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class BaseTransitionzParams<T> : MarkupExtension
	{
		#region props

		/// <summary>
		/// 开始时间
		/// </summary>
		[ConstructorArgument("BeginTime")]
		public double BeginTime { get; set; }

		/// <summary>
		/// 持续时间
		/// </summary>
		[ConstructorArgument("Duration")]
		public double Duration { get; set; }

		/// <summary>
		/// 起始值
		/// </summary>
		[ConstructorArgument("From")]
		public T From { get; set; }

		/// <summary>
		/// 结束值
		/// </summary>
		[ConstructorArgument("To")]
		public T To { get; set; }

		/// <summary>
		/// 缓动方法
		/// </summary>
		[ConstructorArgument("Ease")]
		public EasingFunctionBase Ease { get; set; }

		/// <summary>
		/// 指定 System.Windows.Media.Animation.Timeline 在超出活动期但其父级仍在活动期或保持期内时的行为方式。
		/// </summary>
		public FillBehavior FillBehavior { get; set; }

		#endregion

		#region .ctor

		/// <summary>
		/// 默认构造器
		/// </summary>
		protected BaseTransitionzParams()
		{

		}

		/// <summary>
		/// 构造器
		/// </summary>
		/// <param name="beginTime"></param>
		/// <param name="duration"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="ease"></param>
		protected BaseTransitionzParams(double beginTime, double duration, T from, T to, EasingFunctionBase ease)
		{
			BeginTime = beginTime;
			Duration = duration;
			From = from;
			To = to;
			Ease = ease;

			FillBehavior = FillBehavior.HoldEnd;
		}

		#endregion
	}
}