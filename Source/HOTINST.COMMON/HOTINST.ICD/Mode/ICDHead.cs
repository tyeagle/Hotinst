/**
 * ==============================================================================
 *
 * Filename: CICDHead.cs
 * Description: 
 *
 * Version: 1.0
 * Created: 2016/8/31 14:08:20
 * Compiler: Visual Studio 2013
 *
 * Author: cc
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using HOTINST.COMMON.Bitwise;
using HOTINST.ICD.Codec;

namespace HOTINST.ICD
{
	/// <summary>
	/// 协议头
	/// </summary>
	[Serializable]
	public class ICDHead
	{
		/// <summary>
		/// 总长度（单位：字节）
		/// </summary>
        public Int32 TotalSize { get; set; }
		/// <summary>
		///	如果是定长则指定此字段，否则此字段为0。
		/// </summary>
        public Int32 FixedSize { get; set; }
		/// <summary>
		/// 帧起始标志
		/// </summary>
		public string BeginFlag { get; set; }
		/// <summary>
		/// 节点ID（类似命令字，用来说明是哪个命令帧）的字节偏移量。
		/// </summary>
        public UInt32 NodeIdPosition { get; set; }
		/// <summary>
		/// 节点ID的字节长度。
		/// </summary>
        public UInt32 NodeIdSize { get; set; }
		/// <summary>
		/// 帧长度的字节偏移量。
		/// </summary>
        public UInt32 LengthPosition { get; set; }
		/// <summary>
		/// 帧长度的字节长度。
		/// </summary>
        public UInt32 LengthSize { get; set; }
		/// <summary>
		/// 校验起始字节偏移量。
		/// </summary>
        public UInt32 CheckStartPosition { get; set; }
		/// <summary>
		/// 校验数据的字节长度。
		/// </summary>
        public UInt32 CheckLengthSize { get; set; }
		/// <summary>
		/// 校验的字节偏移量。
		/// </summary>
        public UInt32 CheckPosition { get; set; }
		/// <summary>
		/// 校验的字节长度。
		/// </summary>
        public UInt32 CheckSize { get; set; }
		/// <summary>
		/// 校验模式。
		/// </summary>
		public ECheckMode CheckMode { get; set; }
		/// <summary>
		/// 大小端。
		/// </summary>
		public Endian Endian { get; set; }
	}
}