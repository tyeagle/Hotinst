using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace HOTINST.ICD.ValueConvert
{
    /// <summary>
    /// 用于形如y=（ax+b)/(cx+d)的两线制电阻和PT100电阻的Ad值转换为实际电阻值
    /// </summary>
    public class Inverse : IValueConvert
	{
		/// <summary>
		/// 计算一次标定系数
		/// </summary>
		/// <param name="x">已知点的x坐标集合</param>
		/// <param name="y">已知点的y坐标集合</param>
		/// <returns>系数数组</returns>
		public double[] Calibration(double[] x, double[] y)
		{
			throw new NotImplementedException();
		}

		public object Convert(uint value, object param1, object param2, object param3, object param4)
        {
            double val = value;
            double coefficients1 = (double)param1;
            double coefficients2 = (double)param2;
            double coefficients3 = (double)param3;
            double coefficients4 = (double)param4;

            if ((coefficients3 * val + coefficients4) == 0)
                throw new Exception($"Inverse转换失败：参数3[{coefficients3}]乘以Value[{val}] 加上参数4[{coefficients4}]不能为0");
            return (coefficients1 * val + coefficients2)/ (coefficients3 * val + coefficients4);
        }

        public uint ConvertBack(object value, object param1, object param2, object param3, object param4)
        {
            double val = System.Convert.ToDouble(value);
            double coefficients1 = (double)param1;
            double coefficients2 = (double)param2;
            double coefficients3 = (double)param3;
            double coefficients4 = (double)param4;

            if (coefficients3 == 0)
            {
                if (coefficients4 == 0)
                    throw new Exception($"Inverse转换(ConvertBack)失败：参数3[{ coefficients3 }],参数4[{ coefficients3 }]不能同时为0");
                if (coefficients1 == 0)
                    throw new Exception($"Inverse转换(ConvertBack)失败：参数3[{ coefficients3 }],参数1[{ coefficients3 }]不能同时为0");
                return (uint)((val * coefficients4 - coefficients2) / coefficients1);
            }
            if ((coefficients3 * val - coefficients1) == 0)
                throw new Exception($"Inverse转换(ConvertBack)失败：参数3[{coefficients3}]乘以Value[{val}] 减去参数1[{coefficients1}]不能为0");
            return (uint)((coefficients2 * coefficients3 - coefficients1 * coefficients4) 
                / (coefficients3 * coefficients3 * val - coefficients3 * coefficients1) 
                - coefficients4 / coefficients3);
        }

        public object ConvertFromDouble(double value, object param1, object param2, object param3, object param4)
        {
            double val = System.Convert.ToDouble(value);
            double coefficients1 = (double)param1;
            double coefficients2 = (double)param2;
            double coefficients3 = (double)param3;
            double coefficients4 = (double)param4;

            if ((coefficients3 * val + coefficients4) == 0)
                throw new Exception($"Inverse转换失败：参数3[{coefficients3}]乘以Value[{val}] 加上参数4[{coefficients4}]不能为0");
            return (coefficients1 * val + coefficients2) / (coefficients3 * val + coefficients4);
        }

        public double ConvertBackToDouble(object value, object param1, object param2, object param3, object param4)
        {
            double val = System.Convert.ToDouble(value);
            double coefficients1 = (double)param1;
            double coefficients2 = (double)param2;
            double coefficients3 = (double)param3;
            double coefficients4 = (double)param4;

            if (coefficients3 == 0)
            {
                if (coefficients4 == 0)
                    throw new Exception($"Inverse转换(ConvertBack)失败：参数3[{ coefficients3 }],参数4[{ coefficients3 }]不能同时为0");
                if (coefficients1 == 0)
                    throw new Exception($"Inverse转换(ConvertBack)失败：参数3[{ coefficients3 }],参数1[{ coefficients3 }]不能同时为0");
                return (uint)((val * coefficients4 - coefficients2) / coefficients1);
            }
            if ((coefficients3 * val - coefficients1) == 0)
                throw new Exception($"Inverse转换(ConvertBack)失败：参数3[{coefficients3}]乘以Value[{val}] 减去参数1[{coefficients1}]不能为0");
            return (coefficients2 * coefficients3 - coefficients1 * coefficients4)
                / (coefficients3 * coefficients3 * val - coefficients3 * coefficients1)
                - coefficients4 / coefficients3;

           
        }
    }
}
