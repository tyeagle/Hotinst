/****************************************************************
* 类 名 称：IniHelper
* 命名空间：HOTINST.COMMON.IniProcess
* 文 件 名：IniHelper.cs
* 创建时间：2017-3-10
* 作    者：蔡先松
* 说    明：简易INI文件处理类。
* 修改时间：
* 修 改 人：
*****************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace HOTINST.COMMON.IniProcess
{
	/// <summary>
	/// 提供对INI文件的简单处理
	/// </summary>
	public class IniHelper
	{
		private readonly string _iniFile;

		/// <summary>
		/// 字符编码
		/// </summary>
		public static Encoding Encoding = Encoding.UTF8;

		/// <summary>
		/// .ctor
		/// </summary>
		/// <param name="file">文件路径。如果路径不存在，将自动创建</param>
		public IniHelper(string file)
		{
			_iniFile = file;
			if(!File.Exists(_iniFile))
				File.Create(file);
		}

		/// <summary>
		/// 批量读取键值对
		/// </summary>
		/// <returns>返回INI配置结构体列表,单独结构可以通过索引获取或设置</returns>
		public List<IniStruct> ReadValues()
		{
			return ReadValues(_iniFile);
		}

		/// <summary>
		/// 读取指定键的值
		/// </summary>
		/// <param name="key">要读取的键</param>
		/// <param name="section">键所在的节点</param>
		/// <returns>返回指定键对应的值</returns>
		public string ReadValue(string key, string section)
		{
			string comments = "";
			return ReadValue(_iniFile, key, section, ref comments);
		}

		/// <summary>
		/// 读取指定键的值
		/// </summary>
		/// <param name="key">要读取的键</param>
		/// <param name="section">键所在的节点</param>
		/// <param name="comments">注释</param>
		/// <returns>返回指定键对应的值</returns>
		public string ReadValue(string key, string section, ref string comments)
		{
			return ReadValue(_iniFile, key, section, ref comments);
		}

		private static string GetText(string file)
		{
			string content = File.ReadAllText(file);
			if(content.Contains("�"))
			{
				Encoding = Encoding.GetEncoding("GBK");
				content = File.ReadAllText(file, Encoding.GetEncoding("GBK"));
			}
			return content;
		}

		/// <summary>
		/// 读取指定文件指定键的值
		/// </summary>
		/// <param name="file">文件路径</param>
		/// <param name="key">要读取的键</param>
		/// <param name="section">键所在的节点</param>
		/// <param name="comments">注释</param>
		/// <returns>返回指定键对应的值</returns>
		public static string ReadValue(string file, string key, string section, ref string comments)
		{
			string valueText = "";
			string content = GetText(file);
			if(!string.IsNullOrEmpty(section)) //首先遍历节点
			{
				MatchCollection matches = new Regex(@"\[\s*(?'section'[^\[\]\s]+)\s*\]").Matches(content);
				if(matches.Count <= 0)
					return "";
				Match currMatch = null;
				Match tailMatch = null;
				foreach(Match match in matches)
				{
					string matchSection = match.Groups["section"].Value;
					if(string.Equals(matchSection, section, StringComparison.CurrentCultureIgnoreCase))
					{
						currMatch = match;
						continue;
					}
					if(currMatch != null)
					{
						tailMatch = match;
						break;
					}
				}
				if(currMatch != null)
					valueText = content.Substring(currMatch.Index + currMatch.Length, (tailMatch != null ? tailMatch.Index : content.Length) - currMatch.Index - currMatch.Length);//截取有效值域
			}
			else
				valueText = content;
			string[] lines = valueText.Split(new[] { "\r\n" }, StringSplitOptions.None);
			foreach(string line in lines)
			{
				if(string.IsNullOrEmpty(line) || line == "\r\n" || line.Contains("["))
					continue;
				string valueLine = line;
				if(line.Contains(";"))
				{
					string[] seqPairs = line.Split(';');
					if(seqPairs.Length > 1)
						comments = seqPairs[1].Trim();
					valueLine = seqPairs[0];
				}
				string[] keyValuePairs = valueLine.Split('=');
				string lineKey = keyValuePairs[0];
				string lineValue = "";
				if(keyValuePairs.Length > 1)
				{
					lineValue = keyValuePairs[1];
				}
				if(key.ToLower().Trim() == lineKey.ToLower().Trim())
				{
					return lineValue;
				}
			}
			return "";
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public static List<IniStruct> ReadValues(string file)
		{
			List<IniStruct> iniStructList = new List<IniStruct>();
			string content = GetText(file);
			MatchCollection matches = new Regex(@"\[\s*(?'section'[^\[\]\s]+)\s*\](?'valueContent'[^\[\]]*)").Matches(content);
			foreach(Match match in matches)
			{
				IniStruct iniStruct = new IniStruct();
				string matchSection = match.Groups["section"].Value;
				string matchValue = match.Groups["valueContent"].Value;
				iniStruct.Section = matchSection;

				string[] lines = matchValue.Split(new[] { "\r\n" }, StringSplitOptions.None);
				foreach(string line in lines)
				{
					if(string.IsNullOrEmpty(line) || line == "\r\n" || line.Contains("["))
						continue;
					string comments = "";//注释
					string valueLine = line;
					if(line.Contains(";"))
					{
						string[] seqPairs = line.Split(';');
						if(seqPairs.Length > 1)
							comments = seqPairs[1].Trim();
						valueLine = seqPairs[0];
					}
					string[] keyValuePairs = valueLine.Split('=');
					string lineKey = keyValuePairs[0];
					string lineValue = "";
					if(keyValuePairs.Length > 1)
					{
						lineValue = keyValuePairs[1];
					}
					iniStruct.Add(lineKey, lineValue, comments);
				}
				iniStructList.Add(iniStruct);
			}

			return iniStructList;
		}

		/// <summary>
		/// 对指定键写入值
		/// </summary>
		/// <param name="section">键所在的节点</param>
		/// <param name="key">要写的键</param>
		/// <param name="value">要写入的值</param>
		public void Write(string section, string key, string value)
		{
			Write(section, key, value, null);
		}
		
		/// <summary>
		/// 对指定键写入值
		/// </summary>
		/// <param name="section">键所在的节点</param>
		/// <param name="key">要写的键</param>
		/// <param name="value">要写入的值</param>
		/// <param name="comment">注释</param>
		public void Write(string section, string key, string value, string comment)
		{
			Write(_iniFile, section, key, value, comment);
		}

		/// <summary>
		/// 对指定文件指定键写入值
		/// </summary>
		/// <param name="file">文件路径</param>
		/// <param name="section">键所在的节点</param>
		/// <param name="key">要写的键</param>
		/// <param name="value">要写入的值</param>
		/// <param name="comment">注释</param>
		public static void Write(string file, string section, string key, string value, string comment)
		{
			bool isModified = false;
			StringBuilder stringBuilder = new StringBuilder();
			string content = GetText(file);
			StringBuilder newValueContent = new StringBuilder();
			#region 写入了节点
			if(!string.IsNullOrEmpty(section))
			{
				string pattern = string.Format("\\[\\s*{0}\\s*\\](?'valueContent'[^\\[\\]]*)", section);
				MatchCollection matches = new Regex(pattern).Matches(content);
				if(matches.Count <= 0)
				{
					stringBuilder.AppendLine(string.Format("[{0}]", section)); //检查节点是否存在
					//stringBuilder.AppendLine($"{key}={value}{(!string.IsNullOrEmpty(comment) ? ";" + comment : "")}");
					stringBuilder.AppendLine(string.Format("{0}={1}{2}", key, value, !string.IsNullOrEmpty(comment) ? ";" + comment : ""));
					stringBuilder.AppendLine(content);
					isModified = true;
				}
				else
				{
					Match match = matches[0];
					string valueContent = match.Groups["valueContent"].Value;
					string[] lines = valueContent.Split(new[] { "\r\n" }, StringSplitOptions.None);

					newValueContent.AppendLine(string.Format("[{0}]", section));
					foreach(string line in lines)
					{
						if(string.IsNullOrEmpty(line) || line == "\r\n" || line.Contains("["))
						{
							continue;
						}

						string valueLine = line;
						if(line.Contains(";"))
						{
							string[] seqPairs = line.Split(';');
							valueLine = seqPairs[0];
						}
						string[] keyValuePairs = valueLine.Split('=');
						string lineKey = keyValuePairs[0];
						if(keyValuePairs.Length > 1)
						{
						}
						if(key.ToLower().Trim() == lineKey.ToLower().Trim())
						{
							isModified = true;
							//newValueContent.AppendLine($"{key}={value}{(!string.IsNullOrEmpty(comment) ? (";" + comment) : "")}");
							newValueContent.AppendLine(string.Format("{0}={1}{2}", key, value, !string.IsNullOrEmpty(comment) ? ";" + comment : ""));
						}
						else
						{
							newValueContent.AppendLine(line);
						}


					}
					if(!isModified)
						//newValueContent.AppendLine($"{key}={value}{(!string.IsNullOrEmpty(comment) ? (";" + comment) : "")}");
						newValueContent.AppendLine(string.Format("{0}={1}{2}", key, value, !string.IsNullOrEmpty(comment) ? ";" + comment : ""));
					string newVal = newValueContent.ToString();
					content = content.Replace(match.Value, newVal);
					stringBuilder.Append(content);

				}
			}
			#endregion
			#region 没有指明节点
			else
			{
				//如果节点为空
				MatchCollection matches = new Regex(@"\[\s*(?'section'[^\[\]\s]+)\s*\](?'valueContent'[^\[\]]*)").Matches(content);
				if(matches.Count > 0)
				{
					var valueText = matches[0].Index > 0 ? content.Substring(0, matches[0].Index) : "";
					string[] lines = valueText.Split(new[] { "\r\n" }, StringSplitOptions.None);
					foreach(string line in lines)
					{
						if(string.IsNullOrEmpty(line) || line == "\r\n" || line.Contains("["))
						{
							continue;
						}

						string valueLine = line;
						if(line.Contains(";"))
						{
							string[] seqPairs = line.Split(';');
							valueLine = seqPairs[0];
						}
						string[] keyValuePairs = valueLine.Split('=');
						string lineKey = keyValuePairs[0];
						if(keyValuePairs.Length > 1)
						{
						}
						if(key.ToLower().Trim() == lineKey.ToLower().Trim())
						{
							isModified = true;
							//newValueContent.AppendLine($"{key}={value}{(!string.IsNullOrEmpty(comment) ? (";" + comment) : "")}");
							newValueContent.AppendLine(string.Format("{0}={1}{2}", key, value, !string.IsNullOrEmpty(comment) ? ";" + comment : ""));
						}
						else
						{
							newValueContent.AppendLine(line);
						}


					}
					if(!isModified)
						//newValueContent.AppendLine($"{key}={value}{(!string.IsNullOrEmpty(comment) ? (";" + comment) : "")}");
						newValueContent.AppendLine(string.Format("{0}={1}{2}", key, value, !string.IsNullOrEmpty(comment) ? ";" + comment : ""));
					string newVal = newValueContent.ToString();
					content = content.Replace(valueText, newVal);
					stringBuilder.Append(content);
				}
				else
				{
					//stringBuilder.AppendLine($"{key}={value}{(!string.IsNullOrEmpty(comment) ? (";" + comment) : "")}");
					stringBuilder.AppendLine(string.Format("{0}={1}{2}", key, value, !string.IsNullOrEmpty(comment) ? ";" + comment : ""));
				}
			}
			#endregion
			File.WriteAllText(file, stringBuilder.ToString(), Encoding);
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class IniStruct : IEnumerable
	{
		private readonly List<string> _commentList;

		/// <summary>
		/// 
		/// </summary>
		public IniStruct()
		{
			_keyValuePairs = new SortedList<string, string>();
			_commentList = new List<string>();
		}

		/// <summary>
		/// 获取注释内容
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public string GetComment(string key)
		{
			if(_keyValuePairs.ContainsKey(key))
			{
				int index = _keyValuePairs.IndexOfKey(key);
				return _commentList[index];
			}
			return "";
		}

		/// <summary>
		/// 支持下标访问
		/// </summary>
		/// <param name="index">索引</param>
		/// <returns></returns>
		public string this[int index]
		{
			get { return _keyValuePairs.Count > index ? _keyValuePairs.Values[index] : ""; }
			set { if(_keyValuePairs.Count > index) _keyValuePairs.Values[index] = value; }
		}

		/// <summary>
		/// 支持下标访问
		/// </summary>
		/// <param name="key">键</param>
		/// <returns></returns>
		public string this[string key]
		{
			get { return _keyValuePairs.ContainsKey(key) ? _keyValuePairs[key] : ""; }
			set { if(_keyValuePairs.ContainsKey(key)) _keyValuePairs[key] = value; }
		}

		/// <summary>
		/// 节点名称
		/// </summary>
		public string Section { get; set; }

		private readonly SortedList<string, string> _keyValuePairs;

		/// <summary>
		/// 添加键值对
		/// </summary>
		/// <param name="key">键</param>
		/// <param name="value">值</param>
		/// <param name="commont">注释</param>
		public void Add(string key, string value, string commont)
		{
			_keyValuePairs.Add(key, value);
			_commentList.Add(commont);
		}

		public override string ToString()
		{
			return Section;
		}

		/// <summary>
		/// 确定是否包含指定的键
		/// </summary>
		/// <param name="key">键</param>
		/// <returns></returns>
		public bool ContainKey(string key)
		{
			return _keyValuePairs.ContainsKey(key);
		}

		public IEnumerator GetEnumerator()
		{
			return _keyValuePairs.GetEnumerator();
		}
	}
}