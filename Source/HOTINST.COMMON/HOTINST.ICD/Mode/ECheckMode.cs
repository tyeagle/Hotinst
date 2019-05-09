/**
 * ==============================================================================
 *
 * Filename: Enum_CheckMode.cs
 * Description: 
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
	/// 校验模式
	/// </summary>
    public enum ECheckMode
    {
		/// <summary>
		/// 不校验
		/// </summary>
		[Description("不校验")]
		DonotCheck = -1,
        /// <summary>
        /// 8位和校验
        /// </summary>
		[Description("8位和校验")]
		CheckSum8 = 0,
        /// <summary>
        /// 16位和校验
		/// </summary>
		[Description("16位和校验")]
        CheckSum16 = 1,
        /// <summary>
        /// 32位和校验
		/// </summary>
		[Description("32位和校验")]
        CheckSum32 = 2,
        /// <summary>
        /// 8位异或校验
		/// </summary>
		[Description("8位异或校验")]
        CheckXor8 = 3,
        /// <summary>
        /// 16位异或校验
		/// </summary>
		[Description("16位异或校验")]
        CheckXor16 = 4,
        /// <summary>
        /// 32位异或校验
		/// </summary>
		[Description("32位异或校验")]
        CheckXor32 = 5,
        /// <summary>
        /// 8位CRC校验
		/// </summary>
		[Description("8位CRC校验")]
        CheckCRC8 = 6,
        /// <summary>
        /// 16位CRC校验
		/// </summary>
		[Description("16位CRC校验")]
        CheckCRC16 = 7,
        /// <summary>
        /// 32位CRC校验
		/// </summary>
		[Description("32位CRC校验")]
        CheckCRC32 = 8,
    }
}