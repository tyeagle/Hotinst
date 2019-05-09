namespace HOTINST.ICD.ValueConvert
{
	/// <summary>
	/// 定义一种包含了参数的转换器原型
	/// </summary>
	/// <param name="rawVal">原始值</param>
	/// <returns></returns>
	public delegate object ConvertFunction(uint rawVal);

	/// <summary>
	/// 定义一种包含了参数的转换器反转的原型
	/// </summary>
	/// <param name="sgVal">要转换的值</param>
	/// <returns></returns>
	public delegate uint ConvertBackFunction(object sgVal);

    /// <summary>
	/// 定义一种包含了参数的转换器原型
	/// </summary>
	/// <param name="rawVal">原始值</param>
	/// <returns></returns>
	public delegate object ConvertDoubleFunction(double rawVal);

    /// <summary>
    /// 定义一种包含了参数的转换器反转的原型
    /// </summary>
    /// <param name="sgVal">要转换的值</param>
    /// <returns></returns>
    public delegate double ConvertBackDoubleFunction(object sgVal);
}