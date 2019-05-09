/**
 * ==============================================================================
 *
 * Filename: Linear
 * Description: 线性转换实现类
 *
 * Version: 1.0
 * Created: 2016/6/12 17:26:14
 * Compiler: Visual Studio 2013
 *
 * Author: cc
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Diagnostics;

namespace HOTINST.ICD.ValueConvert
{
	/// <summary>
	/// 用于形如y=ax+b的x系数的计算
	/// </summary>
	public class Linear : IValueConvert
	{
		#region IPlugin 成员

		/// <summary>
		/// 计算一次标定系数
		/// </summary>
		/// <param name="x">已知点的x坐标集合</param>
		/// <param name="y">已知点的y坐标集合</param>
		/// <returns>系数数组</returns>
		public double[] Calibration(double[] x, double[] y)
		{
			Debug.Assert(x.Length == y.Length);

			return HOTINST.COMMON.Math.MultiLine(x, y, x.Length, 1);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="param1"></param>
		/// <param name="param2"></param>
		/// <param name="param3"></param>
		/// <param name="param4"></param>
		/// <returns></returns>
		public object Convert(uint value, object param1, object param2, object param3, object param4)
		{
			double val = value;
			double coefficients1 = (double)param1;
			double coefficients2 = (double)param2;

			return val * coefficients1 + coefficients2;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="param1"></param>
		/// <param name="param2"></param>
		/// <param name="param3"></param>
		/// <param name="param4"></param>
		/// <returns></returns>
		public uint ConvertBack(object value, object param1, object param2, object param3, object param4)
		{
			double val = (double)value;
			double coefficients1 = (double)param1;
			double coefficients2 = (double)param2;

			if(Math.Abs(coefficients1) < 0.00000001)
				throw new Exception("转换失败：第一个系数不能为0。" + coefficients1);

			return (uint)Math.Round((val - coefficients2) / coefficients1);
		}

        public object ConvertFromDouble(double value, object param1, object param2, object param3, object param4)
        {
            double coefficients1 = (double)param1;
            double coefficients2 = (double)param2;

            return value * coefficients1 + coefficients2;
        }

        public double ConvertBackToDouble(object value, object param1, object param2, object param3, object param4)
        {
            double val = System.Convert.ToDouble(value);

            double coefficients1 = (double)param1;
            double coefficients2 = (double)param2;

            if (Math.Abs(coefficients1) < 0.00000001)
                throw new Exception("转换失败：第一个系数不能为0。" + coefficients1);

            return (val - coefficients2) / coefficients1;
        }

      

        #endregion
    }
}