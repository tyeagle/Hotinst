/**
 * ==============================================================================
 *
 * Filename: DoScale
 * Description: 执行转换类
 *
 * Version: 1.0
 * Created: 2016/6/13 08:47:07
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

namespace HOTINST.ICD.ValueConvert
{

	/// <summary>
	/// 表示一个转换执行类
	/// </summary>
	public class Converter
	{
		private readonly PluginConfigMgr _pluginConfigMgr;

		/// <summary>
		/// 函数名与插件的键值对集合
		/// </summary>
		private Dictionary<string, IValueConvert> Plugins { get; set; }

		/// <summary>
		/// 初始化一个执行转换的类
		/// </summary>
		public Converter()
		{
			_pluginConfigMgr = new PluginConfigMgr();
			Plugins = new Dictionary<string, IValueConvert>();

			InitPlugins();
		}

		/// <summary>
		/// 根据ICD里面配置的函数名加载所有的插件
		/// </summary>
		/// <returns></returns>
		private void InitPlugins()
		{
			string location = Path.GetDirectoryName(Assembly.GetAssembly(GetType()).Location);
			if(string.IsNullOrWhiteSpace(location))
				throw new Exception("获取目录信息失败。");

			_pluginConfigMgr.PluginConfigs.ForEach(plugin =>
			{
				string fullPath = Path.Combine(location, "Plugins", plugin.Name);
				Assembly dll = Assembly.LoadFrom(fullPath);
				Type operateClassType = dll.GetType(plugin.FullClassName);
				object objOperateClass = Activator.CreateInstance(operateClassType);

				Plugins[plugin.FunctionName] = (IValueConvert)objOperateClass;
			});

            Linear convert = new Linear();
            Plugins["Linear"] = convert;
            Inverse inverse = new Inverse(); 
            Plugins["Inverse"] = inverse;

        }

		/// <summary>
		/// 根据函数名获取插件
		/// </summary>
		/// <param name="functionName">函数名</param>
		/// <returns></returns>
		public IValueConvert GetPlugin(string functionName)
		{
			if(string.IsNullOrWhiteSpace(functionName)) return null;
			return Plugins.ContainsKey(functionName) ? Plugins[functionName] : null;
		}

		/// <summary>
		/// 根据函数名，参数，获取一个只需要传递待转换值的转换器
		/// </summary>
		/// <param name="funcName">函数名</param>
		/// <param name="param1">转换器的参数1</param>
		/// <param name="param2">转换器的参数2</param>
		/// <param name="param3">转换器的参数3</param>
		/// <param name="param4">转换器的参数4</param>
		/// <returns></returns>
		public ConvertFunction GetConvert(string funcName, object param1, object param2, object param3, object param4)
		{
			IValueConvert fun = GetPlugin(funcName);
			return val => fun.Convert(val, param1, param2, param3, param4);
		}

		/// <summary>
		/// 根据函数名，参数，获取一个只需要传递待转换值的回转器
		/// </summary>
		/// <param name="funcName">函数名</param>
		/// <param name="param1">转换器的参数1</param>
		/// <param name="param2">转换器的参数2</param>
		/// <param name="param3">转换器的参数3</param>
		/// <param name="param4">转换器的参数4</param>
		/// <returns></returns>
		public ConvertBackFunction GetConvertBack(string funcName, object param1, object param2, object param3, object param4)
		{
			IValueConvert fun = GetPlugin(funcName);
			return val => fun.ConvertBack(val, param1, param2, param3, param4);
		}

        public ConvertDoubleFunction GetDoubleConvert(string funcName, object param1, object param2, object param3, object param4)
        {
            IValueConvert fun = GetPlugin(funcName);
            return val => fun.ConvertFromDouble(val, param1, param2, param3, param4);
        }

        public ConvertBackDoubleFunction GetDoubleConvertBack(string funcName, object param1, object param2, object param3, object param4)
        {
            IValueConvert fun = GetPlugin(funcName);
            return val => fun.ConvertBackToDouble(val, param1, param2, param3, param4);
        }
	}
}