/****************************************************************
 * 类 名 称：SerializationHelper
 * 命名空间：HOTINST.COMMON.Serialization
 * 文 件 名：JsonSerializationHelper.cs
 * 创建时间：2016-5-25
 * 作    者：汪锋
 * 说    明：Json序列化及反序列化
 * 修改时间：
 * 修改历史：
 *          
 ****************************************************************/
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace HOTINST.COMMON.Serialization
{
    public partial class SerializationHelper
    {
        /// <summary>
        /// 序列化到Json文件
        /// </summary>
        /// <typeparam name="T">要序列化对象的数据类型</typeparam>
        /// <param name="filePath">文件名（含路径）</param>
        /// <param name="sourceObj">待序列化的对象</param>
        public static void SaveToJson<T>(string filePath, T sourceObj)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(filePath) && sourceObj != null)
                {
                    using (Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T));
                        jsonSerializer.WriteObject(stream, sourceObj);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
        }

		/// <summary>
		/// 序列化到Json文件(带缩进格式)
		/// </summary>
		/// <typeparam name="T">要序列化对象的数据类型</typeparam>
		/// <param name="filePath">文件名（含路径）</param>
		/// <param name="sourceObj">待序列化的对象</param>
		public static void SaveToJsonFormat<T>(string filePath, T sourceObj)
		{
			File.WriteAllText(filePath, ConvertToJsonStringFormat(sourceObj));
		}

        /// <summary>
        /// 从Json文件反序列化到变量
        /// </summary>
        /// <typeparam name="T">要反序列化对象的数据类型</typeparam>
        /// <param name="filePath">文件名（含路径）</param>
        /// <returns>返回反序列化后指定数据类型的变量</returns>
        public static T LoadFromJson<T>(string filePath)
        {
            T result = default(T);
            try
            {
                if (File.Exists(filePath))
                {
                    using (Stream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None))
                    {
                        DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T));
                        result = (T)jsonSerializer.ReadObject(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
            return result;
        }

		/// <summary>
		/// 从Json文件反序列化到变量
		/// </summary>
		/// <typeparam name="T">要反序列化对象的数据类型</typeparam>
		/// <param name="filePath">文件名（含路径）</param>
		/// <returns>返回反序列化后指定数据类型的变量</returns>
		public static T LoadFromJsonFormat<T>(string filePath)
		{
			return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
	    }

		/// <summary>
		/// 序列化成Json格式字符串
		/// </summary>
		/// <typeparam name="T">要序列化对象的数据类型</typeparam>
		/// <param name="sourceObj">待序列化的对象</param>
		/// <returns>成功返回序列化后的Json格式字符串，失败返回空字符串</returns>
		public static string ConvertToJsonString<T>(T sourceObj)
        {
            try
            {
                if (sourceObj != null)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T));
                        jsonSerializer.WriteObject(stream, sourceObj);
                        return Encoding.UTF8.GetString(stream.ToArray());
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
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sourceObj"></param>
		/// <returns></returns>
	    public static string ConvertToJsonStringFormat<T>(T sourceObj)
		{
			return JsonConvert.SerializeObject(sourceObj, Formatting.Indented);
		}
        
        /// <summary>
        /// 从Json格式字符串反序列化到变量
        /// </summary>
        /// <typeparam name="T">要反序列化对象的数据类型</typeparam>
        /// <param name="jsonString">Json格式字符串</param>
        /// <returns>返回反序列化后指定数据类型的变量</returns>
        public static T LoadFromJsonString<T>(string jsonString)
        {
            T result = default(T);
            try
            {
                if (!string.IsNullOrWhiteSpace(jsonString))
                {
                    using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
                    {
                        DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T));
                        result = (T)jsonSerializer.ReadObject(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 将Json格式字符串转为XML格式字符串
        /// </summary>
        /// <param name="jsonString">要转换的Json格式字符串</param>
        /// <returns>转换成功返回XML格式的字符串，失败返回空字符串</returns>
        public static string JsonStringToXMLString(string jsonString)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(jsonString))
                {
                    XmlDictionaryReader objXmlDictionaryReader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(jsonString), XmlDictionaryReaderQuotas.Max);
                    XmlDocument objXmlDocument = new XmlDocument();
                    objXmlDocument.Load(objXmlDictionaryReader);
                    return objXmlDocument.OuterXml;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
            return string.Empty;
        }

        /// <summary>
        /// 将XML格式字符串转为Json格式字符串
        /// </summary>
        /// <param name="xmlString">要转换的XML格式字符串</param>
        /// <returns>转换成功返回Json格式的字符串，失败返回空字符串</returns>
        public static string XmlStringToJsonString(string xmlString)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(xmlString))
                {
                    using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString)))
                    {
                        XmlDictionaryWriter objXmlDictionaryWriter = JsonReaderWriterFactory.CreateJsonWriter(stream);
                        objXmlDictionaryWriter.Flush();
                        objXmlDictionaryWriter.Close();
                        return Encoding.UTF8.GetString(stream.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
            return string.Empty;
        }
    }
}
