using System;
using System.Xml.Serialization;

namespace HOTINST.ICD
{
    [Serializable]
    public class ICDFile
    {
        [XmlAttribute]
        public int ID { get; set; }
		/// <summary>
		/// 仅对XML格式的ICD文件有效
		/// </summary>
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string FileName { get; set; }
    }
}
