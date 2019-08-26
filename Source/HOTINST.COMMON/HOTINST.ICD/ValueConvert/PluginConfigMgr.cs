/**
 * ==============================================================================
 *
 * Filename: PluginConfigMgr
 * Description: 插件配置管理类
 *
 * Version: 1.0
 * Created: 2016/6/13 09:33:38
 * Compiler: Visual Studio 2013
 *
 * Author: cc
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HOTINST.COMMON.Serialization;

namespace HOTINST.ICD.ValueConvert
{
	internal class PluginConfigMgr
	{
		public List<PluginConfig> PluginConfigs { get; private set; }

		public PluginConfigMgr()
		{
			string location = Path.GetDirectoryName(Assembly.GetAssembly(GetType()).Location);
			if(string.IsNullOrWhiteSpace(location))
				throw new Exception("读取目录信息失败。");

			string path = Path.Combine(location, "PluginConfig.xml");
			PluginConfigs = SerializationHelper.LoadFromXml<List<PluginConfig>>(path, "PluginConfigs");
			if (PluginConfigs == null)
			{
				//LogHelper.WriteWarn(GetType(), "未获取到插件配置文件。");
				PluginConfigs = new List<PluginConfig>();
			}
		}
	}
}