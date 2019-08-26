using System.Collections.Generic;

namespace HOTINST.ICD.Codec.Contract
{
	/// <summary>
	/// 
	/// </summary>
    public interface ICodecFactory
    {
		/// <summary>
		/// 总线ICD的编解码器
		/// </summary>
		/// <returns></returns>
        IBusCodec CreateCodec(IList<ICDWords> wordsList);

    }
}