/**
 * ==============================================================================
 *
 * ClassName: DoubleUtil
 * Description: 
 *
 * Version: 1.0
 * Created: 2018/3/6 11:45:33
 * Compiler: Visual Studio 2017
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace HOTINST.COMMON.Controls.Core
{
	/// <summary>
	/// double 常用操作
	/// </summary>
	public static class DoubleUtil
	{
		internal const double DBL_EPSILON = 2.22044604925031E-16;
		internal const float FLT_MIN = 1.175494E-38f;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value1"></param>
		/// <param name="value2"></param>
		/// <returns></returns>
		public static bool AreClose(double value1, double value2)
		{
			// ReSharper disable once CompareOfFloatsByEqualityOperator
			if(value1 == value2)
				return true;
			double num1 = (System.Math.Abs(value1) + System.Math.Abs(value2) + 10.0) * 2.22044604925031E-16;
			double num2 = value1 - value2;
			if(-num1 < num2)
				return num1 > num2;
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value1"></param>
		/// <param name="value2"></param>
		/// <returns></returns>
		public static bool LessThan(double value1, double value2)
		{
			if(value1 < value2)
				return !AreClose(value1, value2);
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value1"></param>
		/// <param name="value2"></param>
		/// <returns></returns>
		public static bool GreaterThan(double value1, double value2)
		{
			if(value1 > value2)
				return !AreClose(value1, value2);
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value1"></param>
		/// <param name="value2"></param>
		/// <returns></returns>
		public static bool LessThanOrClose(double value1, double value2)
		{
			if(value1 >= value2)
				return AreClose(value1, value2);
			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value1"></param>
		/// <param name="value2"></param>
		/// <returns></returns>
		public static bool GreaterThanOrClose(double value1, double value2)
		{
			if(value1 <= value2)
				return AreClose(value1, value2);
			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool IsOne(double value)
		{
			return System.Math.Abs(value - 1.0) < 2.22044604925031E-15;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool IsZero(double value)
		{
			return System.Math.Abs(value) < 2.22044604925031E-15;
		}

		 /// <summary>
		 /// 
		 /// </summary>
		 /// <param name="point1"></param>
		 /// <param name="point2"></param>
		 /// <returns></returns>
		public static bool AreClose(Point point1, Point point2)
		{
			if(AreClose(point1.X, point2.X))
				return AreClose(point1.Y, point2.Y);
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="size1"></param>
		/// <param name="size2"></param>
		/// <returns></returns>
		public static bool AreClose(Size size1, Size size2)
		{
			if(AreClose(size1.Width, size2.Width))
				return AreClose(size1.Height, size2.Height);
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="vector1"></param>
		/// <param name="vector2"></param>
		/// <returns></returns>
		public static bool AreClose(Vector vector1, Vector vector2)
		{
			if(AreClose(vector1.X, vector2.X))
				return AreClose(vector1.Y, vector2.Y);
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rect1"></param>
		/// <param name="rect2"></param>
		/// <returns></returns>
		public static bool AreClose(Rect rect1, Rect rect2)
		{
			if(rect1.IsEmpty)
				return rect2.IsEmpty;
			if(!rect2.IsEmpty && AreClose(rect1.X, rect2.X) && (AreClose(rect1.Y, rect2.Y) && AreClose(rect1.Height, rect2.Height)))
				return AreClose(rect1.Width, rect2.Width);
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public static bool IsBetweenZeroAndOne(double val)
		{
			if(GreaterThanOrClose(val, 0.0))
				return LessThanOrClose(val, 1.0);
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public static int DoubleToInt(double val)
		{
			if(0.0 >= val)
				return (int)(val - 0.5);
			return (int)(val + 0.5);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="r"></param>
		/// <returns></returns>
		public static bool RectHasNaN(Rect r)
		{
			return IsNaN(r.X) || IsNaN(r.Y) || (IsNaN(r.Height) || IsNaN(r.Width));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool IsNaN(double value)
		{
			NanUnion nanUnion = new NanUnion { DoubleValue = value };
			ulong num1 = nanUnion.UintValue & 18442240474082181120UL;
			ulong num2 = nanUnion.UintValue & 4503599627370495UL;
			if((long)num1 == 9218868437227405312L || (long)num1 == -4503599627370496L)
				return num2 > 0UL;
			return false;
		}

		[StructLayout(LayoutKind.Explicit)]
		private struct NanUnion
		{
			[FieldOffset(0)]
			internal double DoubleValue;

			[FieldOffset(0)]
			internal readonly ulong UintValue;
		}
	}
}