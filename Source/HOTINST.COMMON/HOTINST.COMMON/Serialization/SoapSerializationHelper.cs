/****************************************************************
* 类 名 称：SerializationHelper
* 命名空间：HOTINST.COMMON.Serialization
* 文 件 名：SoapSerializationHelper.cs
* 创建时间：2016-3-23
* 作    者：汪锋
* 说    明：Soap序列化及反序列化
* 修改时间：
* 修 改 人：
*****************************************************************/
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;

namespace HOTINST.COMMON.Serialization
{
    public partial class SerializationHelper
    {
        /// <summary>
        /// Soap序列化到文件
        /// </summary>
        /// <typeparam name="T">要序列化对象的数据类型</typeparam>
        /// <param name="filePath">文件名（含路径）</param>
        /// <param name="sourceObj">要序列化的对象</param>
        public static void SaveToSoap<T>(string filePath, T sourceObj)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(filePath) && sourceObj != null)
                {
                    using (Stream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                    {
                        SoapFormatter formatter = new SoapFormatter();
                        formatter.Serialize(stream, sourceObj);
                        stream.Flush();
                        stream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
        }

        /// <summary>
        /// Soap反序列化
        /// </summary>
        /// <typeparam name="T">要反序列化对象的数据类型</typeparam>
        /// <param name="filePath">文件名（含路径）</param>
        /// <returns>返回反序列化后指定数据类型的变量</returns>
        public static T LoadFromSoap<T>(string filePath)
        {
            T result = default(T);

            try
            {
                if (File.Exists(filePath))
                {
                    using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        SoapFormatter formatter = new SoapFormatter();
                        result = (T)formatter.Deserialize(stream);
                        stream.Flush();
                        stream.Close();
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
