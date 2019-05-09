/**
 * ==============================================================================
 *
 * ClassName: AccessMode
 * Description: 
 *
 * Version: 1.0
 * Created: 2018/6/25 10:28:14
 * Compiler: Visual Studio 2017
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

namespace HOTINST.ICD
{
	/// <summary>
	/// 信号访问模式
	/// </summary>
	public enum AccessMode
	{
		/// <summary>
		/// 可读写
		/// </summary>
		ReadWrite,
		/// <summary>
		/// 只读
		/// </summary>
		Read,
		/// <summary>
		/// 只写
		/// </summary>
		Write
	}
}