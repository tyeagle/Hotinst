/*******************************************************************************
 * 类 名 称：SerializationHelper
 * 命名空间：HOTINST.COMMON.Serialization
 * 文 件 名：XMLSerializationHelper.cs
 * 创建时间：2014-10-9
 * 作    者：谭玉
 * 说    明：XML序列化及反序列化
 * 修改时间：
 * 修改历史：
 *          2016.5.4 汪锋 添加注释
 *          2016.5.25 汪锋 新增对象序列化成XML格式字符串SaveToXmlString
 *          2016.5.25 汪锋 新增XML格式字符串反序列化成对象LoadFromXmlString
 *******************************************************************************/
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace HOTINST.COMMON.Serialization
{
    public partial class SerializationHelper
    {
        /// <summary>
        /// 序列化到XML文件
        /// </summary>
        /// <typeparam name="T">要序列化对象的数据类型</typeparam>
        /// <param name="filePath">文件名（含路径）</param>
        /// <param name="sourceObj">待序列化的对象</param>
        /// <param name="xmlRootName">XML根节点名称</param>
        public static void SaveToXml<T>(string filePath, T sourceObj, string xmlRootName)
        {
	        if(string.IsNullOrWhiteSpace(xmlRootName))
	        {
		        throw new ArgumentNullException(nameof(xmlRootName), "请指定根节点名称。");
	        }

            try
            {
                if (!string.IsNullOrWhiteSpace(filePath) && sourceObj != null)
                {
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRootName));

                        xmlSerializer.Serialize(writer, sourceObj);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
        }

        /// <summary>
        /// 从XML文件反序列化到变量
        /// </summary>
        /// <typeparam name="T">要反序列化对象的数据类型</typeparam>
        /// <param name="filePath">文件名（含路径）</param>
        /// <param name="xmlRootName">XML根节点名称</param>
        /// <returns>返回反序列化后指定数据类型的变量</returns>
        public static T LoadFromXml<T>(string filePath, string xmlRootName)
		{
			if(string.IsNullOrWhiteSpace(xmlRootName))
			{
				throw new ArgumentNullException(nameof(xmlRootName), "请指定根节点名称。");
			}

			T result = default(T);
            try
            {
                if (File.Exists(filePath))
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRootName));

                        result = (T)xmlSerializer.Deserialize(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
                throw;
            }
            return result;
        }

        /// <summary>
        /// 序列化成XML格式的字符串
        /// </summary>
        /// <typeparam name="T">要序列化对象的数据类型</typeparam>
        /// <param name="sourceObj">待序列化的对象</param>
        /// <param name="xmlRootName">XML根节点名称</param>
        /// <returns>序列化成功返回XML格式的字符串，失败则返回空字符串</returns>
        public static string SaveToXmlString<T>(T sourceObj, string xmlRootName)
		{
			if(string.IsNullOrWhiteSpace(xmlRootName))
			{
				throw new ArgumentNullException(nameof(xmlRootName), "请指定根节点名称。");
			}

			try
			{
                if (sourceObj != null)
                {
                    using (MemoryStream objMemoryStream = new MemoryStream())
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRootName));
                        xmlSerializer.Serialize(objMemoryStream, sourceObj);
                        return Encoding.UTF8.GetString(objMemoryStream.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
            return string.Empty;
        }

        /// <summary>
        /// 从XML格式的字符串反序列化到变量
        /// </summary>
        /// <typeparam name="T">要反序列化对象的数据类型</typeparam>
        /// <param name="xmlString">XML格式的字符串</param>
        /// <param name="xmlRootName">XML根节点名称</param>
        /// <returns>返回反序列化后指定数据类型的变量</returns>
        public static T LoadFromXmlString<T>(string xmlString, string xmlRootName)
		{
			if(string.IsNullOrWhiteSpace(xmlRootName))
			{
				throw new ArgumentNullException(nameof(xmlRootName), "请指定根节点名称。");
			}

			T result = default(T);
            try
            {
                if (!string.IsNullOrWhiteSpace(xmlString))
                {
                    using (MemoryStream objMemoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString)))
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRootName));

                        result = (T)xmlSerializer.Deserialize(objMemoryStream);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
            return result;
        }
    }
}
