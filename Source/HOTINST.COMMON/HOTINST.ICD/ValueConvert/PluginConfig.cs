/**
 * ==============================================================================
 *
 * Filename: PluginConfig
 * Description: 插件配置类
 *
 * Version: 1.0
 * Created: 2016/6/13 09:34:30
 * Compiler: Visual Studio 2013
 *
 * Author: cc
 * Company: hotinst
 *
 * ==============================================================================
 */

namespace HOTINST.ICD.ValueConvert
{
	/// <summary>
	/// 插件配置
	/// </summary>
	public class PluginConfig
	{
		/// <summary>
		/// 插件名称(文件名)
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 函数名称
		/// </summary>
		public string FunctionName { get; set; }
		/// <summary>
		/// 类的完全限定名称
		/// </summary>
		public string FullClassName { get; set; }
	}
}