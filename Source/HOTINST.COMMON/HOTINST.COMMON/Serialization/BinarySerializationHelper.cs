/****************************************************************
 * 类 名 称：SerializationHelper
 * 命名空间：HOTINST.COMMON.Serialization
 * 文 件 名：BinarySerializationHelper.cs
 * 创建时间：2016-3-23
 * 作    者：汪锋
 * 说    明：Binary序列化及反序列化
 * 修改历史：
 *          2016-7-21 汪锋 新增对象与字节数组之间的Binary序
 *          列化/反序列化
 ****************************************************************/
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace HOTINST.COMMON.Serialization
{
    /// <summary>
    /// 序列化Helper类
    /// </summary>
    public partial class SerializationHelper
    {
        /// <summary>
        /// Binary序列化到文件
        /// </summary>
        /// <typeparam name="T">要序列化对象的数据类型</typeparam>
        /// <param name="filePath">文件名（含路径）</param>
        /// <param name="sourceObj">要序列化的对象</param>
        public static void SaveToBinary<T>(string filePath, T sourceObj)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(filePath) && sourceObj != null)
                {
                    using (Stream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
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
        /// Binary反序化
        /// </summary>
        /// <typeparam name="T">要反序列化对象的数据类型</typeparam>
        /// <param name="filePath">文件名（含路径）</param>
        /// <returns>返回反序列化后指定数据类型的变量</returns>
        public static T LoadFromBinary<T>(string filePath)
        {
            T result = default(T);

            try
            {
                if (File.Exists(filePath))
                {
                    using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
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

        /// <summary>
        /// Binary序列化到字节数组
        /// </summary>
        /// <typeparam name="T">要序列化对象的数据类型</typeparam>
        /// <param name="sourceObj">要序列化的对象</param>
        /// <returns>序列化成功返回序列化后的数组，失败则返回null</returns>
        public static byte[] SaveToBytes<T>(T sourceObj)
        {
            try
            {
                if ( sourceObj != null)
                {
                    byte[] Buffer;
                    using (MemoryStream stream = new MemoryStream())
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(stream, sourceObj);
                        Buffer = stream.ToArray();
                        stream.Flush();
                        stream.Close();
                    }
                    return Buffer;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// 从字节数组Binary反序化
        /// </summary>
        /// <typeparam name="T">要反序列化对象的数据类型</typeparam>
        /// <param name="byteBuffer">要反序列化的字节数组</param>
        /// <returns>返回反序列化后指定数据类型的变量</returns>
        public static T LoadFromBytes<T>(ref byte[] byteBuffer)
        {
            T result = default(T);

            try
            {
                if (byteBuffer!=null)
                {
                    using (MemoryStream stream = new MemoryStream(byteBuffer))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
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
