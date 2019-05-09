using System;
using System.Collections.Generic;

namespace HOTINST.ICD.Codec.Contract
{
	/// <summary>
	/// ICD编解码器接口定义。
	/// </summary>
    public interface IFrameCodec
	{
        /// <summary>
        /// 所属的编解码器的ID,对应ICD的ID
        /// </summary>
        int CodecID { get; set; }
        /// <summary>
        /// 编解码器的名称，对应ICD的Name
        /// </summary>
        string CodecName { get; }
        /// <summary>
        /// 帧中包含的所有信号对象
        /// </summary>
        /// <returns></returns>
        IList<ISignalCodec> GetAllSignals();
	    ISignalCodec GetSignalCodec(int id);
	    ISignalCodec GetSignalCodec(string name);
        object GetValue(int signalid, byte[] buffer, UInt32 index = 0);
	    object GetValue(string signalName, byte[] buffer, UInt32 index = 0);
        void SetValue(int signalid, byte[] buffer, object value, UInt32 index = 0);
	    void SetValue(string signalName, byte[] buffer, object value, UInt32 index = 0);
    }
}