/**
 * ==============================================================================
 *
 * ClassName: ICloneable
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/7/17 13:59:28
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

namespace HOTINST.COMMON.Controls.Core
{
	/// <summary>
	/// 克隆支持
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ICloneable<out T>
	{
		/// <summary>
		/// 创建作为当前实例副本的新对象
		/// </summary>
		/// <returns>作为此实例副本的新对象。</returns>
		T Clone();
	}
}