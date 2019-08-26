/**
 * ==============================================================================
 *
 * Filename: Enum_DataType.cs
 * Description: ICD文件信号数据类型定义。
 *
 * Version: 1.0
 * Created: 2016/5/24 10:02:51
 * Compiler: Visual Studio 2013
 *
 * Author: cc
 * Company: hotinst
 *
 * ==============================================================================
 */

using System.ComponentModel;

namespace HOTINST.ICD
{
	/// <summary>
	/// 数据类型
	/// </summary>
	public enum DataType
	{
		/// <summary>
		/// 无效数据。
		/// </summary>
		[Description("无效数据")]
		InValid = 0,
        /// <summary>
        /// 8位有符号整形数据
        /// </summary>
        [Description("Int8")]
        Int8,
		/// <summary>
		/// 表示一个 8 位无符号整数。
		/// </summary>
		[Description("UInt8")]
		UInt8,
		/// <summary>
		/// 表示 16 位有符号的整数。
		/// </summary>
		[Description("Int16")]
		Int16,
		/// <summary>
		/// 表示 16 位无符号的整数。
		/// </summary>
		[Description("UInt16")]
		UInt16,
		/// <summary>
		/// 表示 32 位有符号的整数。
		/// </summary>
		[Description("Int32")]
		Int32,
		/// <summary>
		/// 表示 32 位无符号的整数。
		/// </summary>
		[Description("UInt32")]
		UInt32,
		/// <summary>
		/// 表示 64 位有符号的整数。
		/// </summary>
		[Description("Int64")]
		Int64,
		/// <summary>
		/// 表示 64 位无符号的整数。
		/// </summary>
		[Description("UInt64")]
		UInt64,
		/// <summary>
		/// 表示一个单精度浮点数字。
		/// </summary>
		[Description("Float")]
		Float,
		/// <summary>
		/// 表示一个双精度浮点数字。
		/// </summary>
		[Description("Double")]
		Double,
		/// <summary>
		/// 表示布尔值。
		/// </summary>
		[Description("Boolean")]
		Boolean,
		/// <summary>
		/// 表示此数据是一个帧。
		/// </summary>
		[Description("Frame")]
		Frame
	}
  //  /// <summary>
  //  /// 信号字节序列枚举
  //  /// </summary>
  //  public enum EndianType
  //  {
  //      /// <summary>
  //      /// 大端字节序
  //      /// </summary>
  //      [Description("大端字节序")]
  //      BigEndian = 0,
  //      /// <summary>
  //      /// 小端字节序
  //      /// </summary>
  //      [Description("小端字节序列")]
  //      LittleEndian = 1,
		//大 = 0,
		//小 = 1,
		//大端 = 0,
		//小端 = 1
  //  }
}