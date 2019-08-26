using System.Collections.Generic;

namespace HOTINST.ICD.Codec.Contract
{
    public interface IBusCodec
    {
        /// <summary>
        /// 编码器的版本号
        /// </summary>
        string Version { get; set; }
        /// <summary>
        /// 获取编解码器支持的帧结构
        /// </summary>
        /// <returns></returns>
        IList<IFrameCodec> GetAllFrames();

        IFrameCodec GetFrameCodec(string frameName);
        ISignalCodec GetSignalCodec(string frameName, string signalName);

        IFrameCodec this[string id] { get;}
    }
}
