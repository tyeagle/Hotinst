namespace HOTINST.ICD.ValueConvert
{
	/// <summary>
	/// 提供一种自定义逻辑的数值转换器插件接口
	/// </summary>
	public interface IValueConvert
	{
		/// <summary>
		/// 计算一次标定系数
		/// </summary>
		/// <param name="x">已知点的x坐标集合</param>
		/// <param name="y">已知点的y坐标集合</param>
		/// <returns>系数数组</returns>
		double[] Calibration(double[] x, double[] y);

		/// <summary>
		/// 转换值。
		/// </summary>
		/// <param name="value">源值</param>
		/// <param name="param1">要使用的转换器参数1</param>
		/// <param name="param2">要使用的转换器参数2</param>
		/// <param name="param3">要使用的转换器参数3</param>
		/// <param name="param4">要使用的转换器参数4</param>
		/// <returns></returns>
		object Convert(uint value, object param1, object param2, object param3, object param4);

		/// <summary>
		/// 转换值。
		/// </summary>
		/// <param name="value">源值</param>
		/// <param name="param1">要使用的转换器参数1</param>
		/// <param name="param2">要使用的转换器参数2</param>
		/// <param name="param3">要使用的转换器参数3</param>
		/// <param name="param4">要使用的转换器参数4</param>
		/// <returns></returns>
		uint ConvertBack(object value, object param1, object param2, object param3, object param4);
		
        /// <summary>
        /// 转换值。
        /// </summary>
        /// <param name="value">源值</param>
        /// <param name="param1">要使用的转换器参数1</param>
        /// <param name="param2">要使用的转换器参数2</param>
        /// <param name="param3">要使用的转换器参数3</param>
        /// <param name="param4">要使用的转换器参数4</param>
        /// <returns></returns>
        object ConvertFromDouble(double value, object param1, object param2, object param3, object param4);

        /// <summary>
		/// 转换值。
		/// </summary>
		/// <param name="value">源值</param>
		/// <param name="param1">要使用的转换器参数1</param>
		/// <param name="param2">要使用的转换器参数2</param>
		/// <param name="param3">要使用的转换器参数3</param>
		/// <param name="param4">要使用的转换器参数4</param>
		/// <returns></returns>
		double ConvertBackToDouble(object value, object param1, object param2, object param3, object param4);
    }
}