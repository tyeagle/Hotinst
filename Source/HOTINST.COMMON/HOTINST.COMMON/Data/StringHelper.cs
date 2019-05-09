/***************************************************************
*	Copyright (C) 2013-2018 Hotinst CO.,Ltd
*	All rights reserved
*	文 件 名:	StringHelper
*	CLR 版本:	4.0.30319.42000
*
*	作    者:	tanyu
*	创建时间:	2018/6/4 
*	文件描述:	字符串与其它数据格式的转换		   	
*
***************************************************************/
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using HOTINST.COMMON.Bitwise;

namespace HOTINST.COMMON.Data
{
    /// <summary>
    /// 字符串Helper类
    /// </summary>
    public static class StringHelper
    {
        #region 字符串分割        

        /// <summary>
        /// 字符串转全角字符串
        /// </summary>
        /// <param name="InputString">传入字符串</param>
        /// <returns>返回转换后的全角字符串</returns>
        public static string ToSBC(this string InputString)
        {
            //半角转全角：
            char[] aryStringArrayBuffer = InputString.ToCharArray();
            for (int cnt = 0; cnt < aryStringArrayBuffer.Length; cnt++)
            {
                if (aryStringArrayBuffer[cnt] == 32)
                {
                    aryStringArrayBuffer[cnt] = (char)12288;
                    continue;
                }
                if (aryStringArrayBuffer[cnt] < 127)
                    aryStringArrayBuffer[cnt] = (char)(aryStringArrayBuffer[cnt] + 65248);
            }
            return new string(aryStringArrayBuffer);
        }

        /// <summary>
        ///  字符串转半角字符串
        /// </summary>
        /// <param name="InputString">传入字符串</param>
        /// <returns>返回转换后的半角字符串</returns>
        public static string ToDBC(this string InputString)
        {
            char[] aryStringArrayBuffer = InputString.ToCharArray();
            for (int cnt = 0; cnt < aryStringArrayBuffer.Length; cnt++)
            {
                if (aryStringArrayBuffer[cnt] == 12288)
                {
                    aryStringArrayBuffer[cnt] = (char)32;
                    continue;
                }
                if (aryStringArrayBuffer[cnt] > 65280 && aryStringArrayBuffer[cnt] < 65375)
                    aryStringArrayBuffer[cnt] = (char)(aryStringArrayBuffer[cnt] - 65248);
            }
            return new string(aryStringArrayBuffer);
        }

        /// <summary>
        /// 修改字符串中指定位置的子串
        /// </summary>
        /// <param name="SourceData">待修改原始字符串</param>
        /// <param name="ReplaceStart">起始修改位置</param>
        /// <param name="ReplaceLength">替换的字符个数</param>
        /// <param name="ReplaceValue">替换字符串</param>
        /// <returns>返回修改后的字符串。如果SourceData为空则返回空字符串；如果ReplaceValue的长度和ReplaceLength不匹配或ReplaceValue为空则返回原始字符串。</returns>
        public static string ModifyString(string SourceData, int ReplaceStart, int ReplaceLength, string ReplaceValue)
        {
            if (string.IsNullOrWhiteSpace(SourceData))
                return string.Empty;
            if ((ReplaceLength != ReplaceValue.Length) || (ReplaceLength == 0))
                return SourceData;

            SourceData = SourceData.Remove(ReplaceStart, ReplaceLength);
            SourceData = SourceData.Insert(ReplaceStart, ReplaceValue);
            return SourceData;
        }

        /// <summary>
        /// 随机生成指定个数的十六进制字符串(不含0x00和0xFF)
        /// </summary>
        /// <param name="Length">待生成的字节数量，无符号整型</param>
        /// <returns>返回长度为Length*2的十六进制数组成的字符串（不包含"00"值和"FF"值）</returns>
        public static string GenerateRandomHexString(uint Length)
        {
            return GenerateRandomHexString(Length, false);
        }

        /// <summary>
        /// 随机生成指定个数的十六进制字符串
        /// </summary>
        /// <param name="Length">待生成的字节数量，无符号整型</param>
        /// <param name="Include0AndF">指示是否要包含"00"值和"FF"值。</param>
        /// <returns>返回长度为Length*2的十六进制数组成的字符串</returns>
        public static string GenerateRandomHexString(uint Length, bool Include0AndF)
        {
            if (Length < 1)
                return string.Empty;

            Random objRnd = new Random();
            string strRandomHexString = "";
            if (Include0AndF)
            {
                for (uint cnt = 1; cnt <= Length; cnt++)
                    strRandomHexString += objRnd.Next(0, 255).ToString("X2");
            }
            else
            {
                for (uint cnt = 1; cnt <= Length; cnt++)
                    strRandomHexString += objRnd.Next(1, 254).ToString("X2");
            }

            return strRandomHexString;
        }
        #endregion

        #region 字符串与数据转换
        /// <summary>
        /// 将16进制字符串转换为字节数组，如果字符串以0x打头，在转换为数组时先移除掉0x两个字符。
        /// 16进制字符串的字符数为奇数时，如果是小端字节序：则第最后一个字符被认为是最高字节的低4位，
        /// 比如字符串"0x12345",按小端转换为byte数组则为{0x12,0x34,0x05},按大端转换为byte数组则为{0x45,0x23,0x01}
        /// 其实正常来说，不应该有奇数个字符的字符串来转换为byte数组的，这个策略只是对错误输入的一种应对方式，个人觉得比抛出异常要好一丢丢。
        /// 如果是大端字节序：则第1个字符被认为是最高字节的低4位，
        /// </summary>
        /// <param name="hexString"></param>
        /// <param name="endian">字符串描述的十六进制数据的字节序，默认是小端字节序:</param>
        /// <returns></returns>
        public static byte[] HexStringToByteArray(string hexString, Endian endian)
        {
            if(string.IsNullOrWhiteSpace(hexString))
            {
                return null;
            }

            if (!StringFormat.IsHexString(hexString))
            {
                throw new ArgumentException($"{hexString}不是正确的十六进制字符串.");
            }

            int numberOfByte = (hexString.Length + 1) / 2;
            int numberOfChars = hexString.Length;

            List<byte> byteList = new List<byte>();

            for(int i=0; i < numberOfByte;)
            {
                int subStringLen = System.Math.Min(numberOfChars , 2);
                byteList.Add( Convert.ToByte(hexString.Substring(i * 2, subStringLen), 16));
                numberOfChars -= subStringLen;
                i ++;
            }                

            if(endian == Endian.LittleEndian)
            {
                return byteList.ToArray();
            }
            else
            {
                byteList.Reverse();
                return byteList.ToArray();
            }            
        }
        /// <summary>
        /// 将字节数组转换为对应的字符串，转换出来的字符串没有0x开头，转换出来的字符串长度是偶数。
        /// 字符串默认是前面的字符表示低字节，endian参数指明转换到的byte数组的存储顺序
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="endian"></param>
        /// <returns></returns>
        public static string ByteArrayToHexString(byte[] buffer, Endian endian)
        {
            StringBuilder sb = new StringBuilder(buffer.Length * 2);
            if(Endian.LittleEndian == endian)
            {
                for(int i=0; i < buffer.Length; i++)
                {
                    sb.Append(Convert.ToString(buffer[i], 16).PadLeft(2, '0'));
                }
            }
            else
            {
                for (int i = buffer.Length-1; i >= 0; i--)
                {
                    sb.Append(Convert.ToString(buffer[i], 16).PadLeft(2, '0'));
                }
            }

            return sb.ToString();
        }

        #endregion
    }
}
