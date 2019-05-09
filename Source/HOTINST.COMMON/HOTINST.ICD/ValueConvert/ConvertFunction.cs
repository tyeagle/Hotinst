namespace HOTINST.ICD.ValueConvert
{
	/// <summary>
	/// ����һ�ְ����˲�����ת����ԭ��
	/// </summary>
	/// <param name="rawVal">ԭʼֵ</param>
	/// <returns></returns>
	public delegate object ConvertFunction(uint rawVal);

	/// <summary>
	/// ����һ�ְ����˲�����ת������ת��ԭ��
	/// </summary>
	/// <param name="sgVal">Ҫת����ֵ</param>
	/// <returns></returns>
	public delegate uint ConvertBackFunction(object sgVal);

    /// <summary>
	/// ����һ�ְ����˲�����ת����ԭ��
	/// </summary>
	/// <param name="rawVal">ԭʼֵ</param>
	/// <returns></returns>
	public delegate object ConvertDoubleFunction(double rawVal);

    /// <summary>
    /// ����һ�ְ����˲�����ת������ת��ԭ��
    /// </summary>
    /// <param name="sgVal">Ҫת����ֵ</param>
    /// <returns></returns>
    public delegate double ConvertBackDoubleFunction(object sgVal);
}