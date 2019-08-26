using System;

namespace HOTINST.ICD.Codec.Contract
{
	/// <summary>
	/// 
	/// </summary>
    public interface ISignalCodec
    {
        /// <summary>
        /// 信号ID 
        /// </summary>
        int ID { get; }
        /// <summary>
        /// 信号名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 
        /// </summary>
		ICDWord Word { get; }
		/// <summary>
        /// 从帧内存中读取一个信号的值（标定过后的值）
        /// </summary>
        /// <param name="buffer">帧内存</param>
        /// <param name="index">相对于内存起始处的偏移，并不是信号ICD中的字节偏移，多数情况下为0，主要是用于处理网络包时跳过包头的偏移</param>
        /// <returns>返回解码后的信号值</returns>
        object GetValue(byte[] buffer, UInt32 index = 0);
        /// <summary>
        /// 获取帧内存中的信号原始值
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        uint GetRawValue(byte[] buffer, UInt32 index = 0);
        /// <summary>
        /// 向帧内存中写入一个信号值
        /// </summary>
        /// <param name="buffer">帧内存地址</param>
        /// <param name="index">相对于内存起始处的偏移，并不是信号ICD中的字节偏移，多数情况下为0，主要是用于处理网络包时跳过包头的偏移</param>
        /// <param name="value">要写入的信号值</param>
        void SetValue(byte[] buffer, object value, UInt32 index = 0);
    }
}
