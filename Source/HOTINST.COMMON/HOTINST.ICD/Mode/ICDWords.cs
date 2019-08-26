/**
 * ==============================================================================
 *
 * Filename: CICDWords.cs
 * Description: 
 *
 * Version: 1.0
 * Created: 2016/8/31 14:06:19
 * Compiler: Visual Studio 2013
 *
 * Author: cc
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace HOTINST.ICD
{
	/// <summary>
	/// 表示一个帧内容的类
	/// </summary>
	[Serializable]
	public class ICDWords
	{
		/// <summary>
		/// 文件ID
		/// </summary>
		[XmlIgnore]
        public int Id { get; set; }
        /// <summary>
        /// ICD名称
        /// </summary>
        [XmlIgnore]
        public string Name { get; set; }
		/// <summary>
		/// 文件名
		/// </summary>
		[XmlIgnore]
		public string FileName { get; set; }
		/// <summary>
		/// 协议头
		/// </summary>
		public ICDHead ICDHead { get; set; }
		/// <summary>
		/// 信号列表
		/// </summary>
		public List<ICDWord> ICDFrame { get; set; }
	}
}