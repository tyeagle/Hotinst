/**
 * ==============================================================================
 *
 * ClassName: SwithCaseExtension
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/9/29 11:07:28
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;

#pragma warning disable 1591

namespace HOTINST.COMMON.Controls.Extension
{
	/// <summary>
	/// 
	/// </summary>
	public static class SwithCaseExtension
	{
		#region SwithCase

		public class SwithCase<TCase, TOther>
		{
			public SwithCase(TCase value, Action<TOther> action)
			{
				Value = value;
				Action = action;
			}

			public TCase Value { get; private set; }
			public Action<TOther> Action { get; private set; }
		}

		#endregion

		#region Swith
		
		public static SwithCase<TCase, TOther> Switch<TCase, TOther>(this TCase t, Action<TOther> action)// where TCase : IEquatable<TCase>
		{
			return new SwithCase<TCase, TOther>(t, action);
		}

		public static SwithCase<TCase, TOther> Switch<TInput, TCase, TOther>(this TInput t, Func<TInput, TCase> selector, Action<TOther> action)// where TCase : IEquatable<TCase>
		{
			return new SwithCase<TCase, TOther>(selector(t), action);
		}

		#endregion
		
		#region Case

		public static SwithCase<TCase, TOther> Case<TCase, TOther>(this SwithCase<TCase, TOther> sc, TCase option, TOther other)// where TCase : IEquatable<TCase>
		{
			return Case(sc, option, other, true);
		}
		
		public static SwithCase<TCase, TOther> Case<TCase, TOther>(this SwithCase<TCase, TOther> sc, TCase option, TOther other, bool bBreak)// where TCase : IEquatable<TCase>
		{
			return Case(sc, c => c.Equals(option), other, bBreak);
		}

		public static SwithCase<TCase, TOther> Case<TCase, TOther>(this SwithCase<TCase, TOther> sc, Predicate<TCase> predict, TOther other)// where TCase : IEquatable<TCase>
		{
			return Case(sc, predict, other, true);
		}

		public static SwithCase<TCase, TOther> Case<TCase, TOther>(this SwithCase<TCase, TOther> sc, Predicate<TCase> predict, TOther other, bool bBreak)// where TCase : IEquatable<TCase>
		{
			if(sc == null)
				return null;
			if(predict(sc.Value))
			{
				sc.Action(other);
				return bBreak ? null : sc;
			}
			return sc;
		}

		#endregion

		#region Default

		public static void Default<TCase, TOther>(this SwithCase<TCase, TOther> sc, TOther other)
		{
			sc?.Action(other);
		}

		#endregion
	}
}