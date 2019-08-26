/***************************************************************
*	Copyright (C) 2013-2018 Hotinst CO.,Ltd
*	All rights reserved
*	文 件 名:	DataConverter
*	CLR 版本:	4.0.30319.42000
*
*	作    者:	tanyu
*	创建时间:	2018/6/4 11:57:46
*	文件描述:			   	
*
***************************************************************/

using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace HOTINST.COMMON.Data
{
    /// <summary>
    /// 常的正则表格式格式
    /// </summary>
    public static class RegexRule
    {
        /// <summary>
        /// IP地址的正则验证表达式
        /// </summary>
        public const string IP = @"^(?:(?:(?:25[0-5]|2[0-4]\d|(?:(?:1\d{2})|(?:[1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|(?:(?:1\d{2})|(?:[1-9]?\d))))$";

        /// <summary>
        /// 端口号的正则验证表达式, 0 ~ 65535
        /// </summary>
        public const string Port = @"^([0-9][0-9]{0,3}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]{1}|6553[0-5])$";

        /// <summary>
        /// IP地址加端口号的正则验证表达式, 0.0.0.0:0, 分组名: ip, port
        /// </summary>
        public const string IPColonPort = @"^(?<ip>(?:(?:(?:25[0-5]|2[0-4]\d|(?:(?:1\d{2})|(?:[1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|(?:(?:1\d{2})|(?:[1-9]?\d))))):(?<port>[0-9][0-9]{0,3}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]{1}|6553[0-5])$";
    }

    /// <summary>
    /// 数据转换器类
    /// </summary>
    public static class Converter
    {
        #region 转换为Byte[]
        public static Byte[] ToByteArray<T>(T value)
        {
            int size = Marshal.SizeOf(value);
            byte[] result = new byte[size];

            IntPtr buffer = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(value, buffer, false);
            Marshal.Copy(buffer, result, 0, size);
            Marshal.FreeHGlobal(buffer);
            return result;
        }

        public static T FromeByteArray<T>(byte[] array)
        {
            T tmp = default(T);
            int size = Marshal.SizeOf(tmp);

            IntPtr buffer = Marshal.AllocHGlobal(size);
            Marshal.Copy(array, 0, buffer, size);
            return (T)Marshal.PtrToStructure(buffer, typeof(T));
        }

        #endregion

        #region 将object安全地转换成数值类型
        /// <summary>
        /// 把一个基础类型恰当地转换为Bool类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBoolean(object value)
        {
            string valueType = value.GetType().Name;
            bool result = false;
            switch(valueType)
            {
                case TypeName.TypeSByte:
                    return 0 != (sbyte)value;
                case TypeName.TypeInt16:
                    return 0 != (short)value;
                case TypeName.TypeInt32:
                    return 0 != (int)value;
                case TypeName.TypeInt64:
                    return 0 != (long)value;
                case TypeName.TypeByte:
                    return 0 != (byte)value;
                case TypeName.TypeUInt16:
                    return 0 != (ushort)value;
                case TypeName.TypeUInt32:
                    return 0 != (uint)value;
                case TypeName.TypeUInt64:
                    return 0 != (ulong)value;
                case TypeName.TypeSingle:
                    return 0 != (float)value;
                case TypeName.TypeDouble:
                    return 0 != (double)value;
                case TypeName.TypeString:
                    bool.TryParse(Convert.ToString(value), out result);
                    return result;
                case TypeName.TypeBoolean:
                    return (bool)value;
                default:
                    throw new NotSupportedException();
            }
        }
        /// <summary>
        /// 把一个基础类型恰当地转换为Int32类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int32 ToInt32(object value)
        {
            string valueType = value.GetType().Name;
            Int32 result = 0;
            switch (valueType)
            {
                case TypeName.TypeSByte:
                    return (sbyte)value;
                case TypeName.TypeInt16:
                    return (short)value;
                case TypeName.TypeInt32:
                    return (int)value;
                case TypeName.TypeInt64:
                    return (Int32)(long)value;
                case TypeName.TypeByte:
                    return (byte)value;
                case TypeName.TypeUInt16:
                    return (ushort)value;
                case TypeName.TypeUInt32:
                    return (Int32)(uint)value;
                case TypeName.TypeUInt64:
                    return (Int32)(ulong)value;
                case TypeName.TypeSingle:
                    return (Int32)System.Math.Round((float)value);
                case TypeName.TypeDouble:
                    return (Int32)System.Math.Round((double)value);
                case TypeName.TypeString:
                    Int32.TryParse(Convert.ToString(value), out result);
                    return result;
                case TypeName.TypeBoolean:
                    return (bool)value ? 1 : 0;
                default:
                    throw new NotSupportedException();
            }
        }
        /// <summary>
        /// 把一个基础类型恰当地转换为Double类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble(object value)
        {
            string valueType = value.GetType().Name;
            double result = 0.1;
            switch (valueType)
            {
                case TypeName.TypeSByte:
                    return (sbyte)value;
                case TypeName.TypeInt16:
                    return (short)value;
                case TypeName.TypeInt32:
                    return (int)value;
                case TypeName.TypeInt64:
                    return (long)value;
                case TypeName.TypeByte:
                    return (byte)value;
                case TypeName.TypeUInt16:
                    return (ushort)value;
                case TypeName.TypeUInt32:
                    return (uint)value;
                case TypeName.TypeUInt64:
                    return (ulong)value;
                case TypeName.TypeSingle:
                    return (float)value;
                case TypeName.TypeDouble:
                    return (double)value;
                //case TypeName.TypeDateTime:
                //    return DateTime.FromFileTime((long)value);
                case TypeName.TypeString:
                    double.TryParse(Convert.ToString(value), out result);
                    return result;
                case TypeName.TypeBoolean:
                    return (bool)value?1.0:0.0;
                default:
                    throw new NotSupportedException();
            }
        }

        public static string ToString(object value)
        {
            string valueType = value.GetType().Name;
            switch (valueType)
            {
                case TypeName.TypeSByte:
                    return ((sbyte)value).ToString(CultureInfo.CurrentCulture);
                case TypeName.TypeInt16:
                    return ((short)value).ToString(CultureInfo.CurrentCulture);
                case TypeName.TypeInt32:
                    return ((int)value).ToString(CultureInfo.CurrentCulture);
                case TypeName.TypeInt64:
                    return ((long)value).ToString(CultureInfo.CurrentCulture);
                case TypeName.TypeByte:
                    return ((byte)value).ToString(CultureInfo.CurrentCulture);
                case TypeName.TypeUInt16:
                    return ((ushort)value).ToString(CultureInfo.CurrentCulture);
                case TypeName.TypeUInt32:
                    return ((uint)value).ToString(CultureInfo.CurrentCulture);
                case TypeName.TypeUInt64:
                    return ((ulong)value).ToString(CultureInfo.CurrentCulture);
                case TypeName.TypeSingle:
                    return ((float)value).ToString(CultureInfo.CurrentCulture);
                case TypeName.TypeDouble:
                    return ((double)value).ToString(CultureInfo.CurrentCulture);
                //case TypeName.TypeDateTime:
                //    return DateTime.FromFileTime((long)value);
                case TypeName.TypeString:
                    return (string)value;
                case TypeName.TypeBoolean:
                    return ((bool)value).ToString(CultureInfo.CurrentCulture);
                default:
                    throw new NotSupportedException();
            }
        }

#endregion
    }
}
