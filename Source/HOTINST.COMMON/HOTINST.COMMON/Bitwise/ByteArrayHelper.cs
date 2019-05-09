/***************************************************************
*	Copyright (C) 2013-2018 Hotinst CO.,Ltd
*	All rights reserved
*	文 件 名:	BitHelper
*	CLR 版本:	4.0.30319.42000
*
*	作    者:	tanyu
*	创建时间:	2016/6/6 
*	文件描述:	对字节数组进行操作，在指定位置(字节偏移、位偏移、位宽)读取或写入一个指定类型的基础类型数据，
*	
*
***************************************************************/
using System;
using System.Linq;
using System.Collections;

namespace HOTINST.COMMON.Bitwise
{
    /// <summary>
    /// 表示字节序的枚举值
    /// </summary>
    public enum Endian
    {
        /// <summary>
        /// 在端字节序
        /// </summary>
        BigEndian,
        /// <summary>
        /// 小端字节序
        /// </summary>
        LittleEndian
    }

    /// <summary>
    /// 字节数组的Helper类
    /// </summary>
    public static class ByteArrayHelper
    {
        #region 根据字节序和位偏移读取数据
        /// <summary>
        /// 从缓冲区中获取指定位置的一段数据，并转为BYTE类型
        /// </summary>
        /// <param name="buffer">字节数组缓冲区</param>
        /// <param name="byteOffset">起始字节偏移（从0开始）</param>
        /// <param name="bitOffset">起始字节内起始位偏移, [0, 7]</param>
        /// <param name="bitWidth">要获取的数据的位宽，[1, 8]</param>
        /// <param name="returnValue">获取到的值</param>
        /// <returns>获取成功返true，失败返回false</returns>
        public static bool GetValue(byte[] buffer, uint byteOffset, uint bitOffset, uint bitWidth, out byte returnValue)
        {
            returnValue = 0;
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer空.");
            }
            if (byteOffset < 0 || byteOffset >= buffer.Length)
            {
                throw new ArgumentOutOfRangeException($"byteOffset{byteOffset}字节偏移超出buffer的范围.");
            }
            if (bitOffset < 0 || bitOffset >= 8)
            {
                throw new ArgumentOutOfRangeException($"bitOffset{bitOffset}位偏移超出字节类型的范围.");
            }
            if (bitWidth <= 0 || bitWidth + bitOffset > 8)
            {
                throw new ArgumentOutOfRangeException($"bitWidth{bitWidth}位宽超出字节类型的范围.");
            }
            
            //读取Buffer,只取出待读取的位
            byte origByte = buffer[byteOffset];
            returnValue = BitwiseOperator.GetValue(origByte, bitOffset, bitWidth);

            return true;
        }

        /// <summary>
        /// 从缓冲区中获取指定位置的一段数据，并转为短整型
        /// </summary>
        /// <param name="buffer">字节数组缓冲区</param>
        /// <param name="byteOffset">起始字节偏移（从0开始）</param>
        /// <param name="bitOffset">起始字节内起始位偏移,[0, 15]</param>
        /// <param name="bitWidth">要获取的数据的位宽，但不能超出缓冲区长度, [1, 16]</param>
        /// <param name="returnValue">获取到的值</param>
        /// <param name="ed">处理多字节数据时的字节序</param>
        /// <returns>获取成功返true，失败返回false</returns>
        public static bool GetValue(byte[] buffer, uint byteOffset, uint bitOffset, uint bitWidth, out short returnValue, Endian ed = Endian.LittleEndian)
        {
            ushort rawValue = 0;
            
            bool ret = GetValue(buffer, byteOffset, bitOffset, bitWidth, out rawValue, ed);
            returnValue = (short)(rawValue);

            return ret;
        }

        /// <summary>
        /// 从缓冲区中获取指定位置的一段数据，并转为无符号短整型
        /// </summary>
        /// <param name="buffer">字节数组缓冲区</param>
        /// <param name="byteOffset">起始字节偏移（从0开始）</param>
        /// <param name="bitOffset">起始字节内起始位偏移, [0, 15]</param>
        /// <param name="bitWidth">要获取的数据的位宽，[1, 16]</param>
        /// <param name="returnValue">获取到的值</param>
        /// <param name="ed">处理多字节数据时的字节序</param>
        /// <returns>获取成功返true，失败返回false</returns>
        public static bool GetValue(byte[] buffer, uint byteOffset, uint bitOffset, uint bitWidth, out ushort returnValue, Endian ed = Endian.LittleEndian)
        {
            returnValue = 0;
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer空.");
            }
            if (byteOffset < 0 || byteOffset + 1 >= buffer.Length)
            {
                throw new ArgumentOutOfRangeException($"byteOffset{byteOffset}字节偏移超出buffer的范围.");
            }
            if (bitOffset < 0 || bitOffset >= 16)
            {
                throw new ArgumentOutOfRangeException($"bitOffset{bitOffset}位偏移超出ushort类型的范围.");
            }
            if (bitWidth <= 0 || bitWidth + bitOffset > 16)
            {
                throw new ArgumentOutOfRangeException($"bitWidth{bitWidth}位宽超出ushort类型的范围.");
            }

            ushort rawValue = ReadUshortFromBuffer(buffer, byteOffset, ed);

            returnValue = BitwiseOperator.GetValue(rawValue, bitOffset, bitWidth);

            return true;
        }

        /// <summary>
        /// 从缓冲区中获取指定位置的一段数据，并转为整型
        /// </summary>
        /// <param name="buffer">字节数组缓冲区</param>
        /// <param name="byteOffset">起始字节偏移（从0开始）</param>
        /// <param name="bitOffset">起始字节内起始位偏移,[0, 31]</param>
        /// <param name="bitWidth">要获取的数据的位宽，[1, 32]</param>
        /// <param name="returnValue">获取到的值</param>
        /// <param name="ed">处理多字节数据时的字节序</param>
        /// <returns>获取成功返true，失败返回false</returns>
        public static bool
            GetValue(byte[] buffer, uint byteOffset, uint bitOffset, uint bitWidth, 
            out int returnValue, Endian ed = Endian.LittleEndian)
        {
            uint rawValue = 0;

            bool ret = GetValue(buffer, byteOffset, bitOffset, bitWidth, out rawValue, ed);

            returnValue = (int)rawValue;

            return ret;
        }

        /// <summary>
        /// 从缓冲区中获取指定位置的一段数据，并转为无符号整型
        /// </summary>
        /// <param name="buffer">字节数组缓冲区</param>
        /// <param name="byteOffset">起始字节偏移（从0开始）</param>
        /// <param name="bitOffset">起始字节内起始位偏移, [0, 31]</param>
        /// <param name="bitWidth">要获取的数据的位宽，[1, 32]</param>
        /// <param name="returnValue">获取到的值</param>
        /// <param name="ed">处理多字节数据时的字节序</param> 
        /// <returns>获取成功返true，失败返回false</returns>
        public static bool
            GetValue(byte[] buffer, uint byteOffset, uint bitOffset, uint bitWidth, 
            out uint returnValue, Endian ed = Endian.LittleEndian)
        {
            returnValue = 0;
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer空.");
            }
            if (byteOffset < 0 || byteOffset + 3 >= buffer.Length)
            {
                throw new ArgumentOutOfRangeException($"byteOffset{byteOffset}字节偏移超出buffer的范围.");
            }
            if (bitOffset < 0 || bitOffset >= 32)
            {
                throw new ArgumentOutOfRangeException($"bitOffset{bitOffset}位偏移超出uint类型的范围.");
            }
            if (bitWidth <= 0 || bitWidth + bitOffset > 32)
            {
                throw new ArgumentOutOfRangeException($"bitWidth{bitWidth}位宽超出uint类型的范围.");
            }

            uint rawValue = ReadUintFromBuffer(buffer, byteOffset, ed);

            returnValue = BitwiseOperator.GetValue(rawValue, bitOffset, bitWidth);

            return true;
        }

        /// <summary>
        /// 从缓冲区中获取指定位置的一段数据，并转为长整型
        /// </summary>
        /// <param name="buffer">字节数组缓冲区</param>
        /// <param name="byteOffset">起始字节偏移（从0开始）</param>
        /// <param name="bitOffset">起始字节内起始位偏移[0, 63]</param>
        /// <param name="bitWidth">要获取的数据的位宽，[1, 64]</param>
        /// <param name="returnValue">获取到的值</param>
        /// <param name="ed">读取内存时使用的字节序</param>
        /// <returns>获取成功返true，失败返回false</returns>
        public static bool
            GetValue(byte[] buffer, uint byteOffset, uint bitOffset, uint bitWidth, 
            out long returnValue, Endian ed= Endian.LittleEndian)
        {
            ulong rawValue = 0;

            bool ret = GetValue(buffer, byteOffset, bitOffset, bitWidth, out rawValue, ed);

            returnValue = (long)(rawValue);

            return ret;
        }

        /// <summary>
        /// 从缓冲区中获取指定位置的一段数据，并转为无符号长整型
        /// </summary>
        /// <param name="buffer">字节数组缓冲区</param>
        /// <param name="byteOffset">起始字节偏移（从0开始）</param>
        /// <param name="bitOffset">起始字节内起始位偏移, [0, 63]</param>
        /// <param name="bitWidth">要获取的数据的位宽，[1, 64]</param>
        /// <param name="returnValue">获取到的值</param>
        /// <param name="ed">读取内存时使用的字节序</param>
        /// <returns>获取成功返true，失败返回false</returns>
        public static bool
            GetValue(byte[] buffer, uint byteOffset, uint bitOffset, uint bitWidth, 
            out ulong returnValue, Endian ed = Endian.LittleEndian)
        {
            returnValue = 0;
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer空.");
            }
            if (byteOffset < 0 || byteOffset + 7 >= buffer.Length)
            {
                throw new ArgumentOutOfRangeException($"byteOffset{byteOffset}字节偏移超出buffer的范围.");
            }
            if (bitOffset < 0 || bitOffset >= 64)
            {
                throw new ArgumentOutOfRangeException($"bitOffset{bitOffset}位偏移超出ulong类型的范围.");
            }
            if (bitWidth <= 0 || bitWidth + bitOffset > 64)
            {
                throw new ArgumentOutOfRangeException($"bitWidth{bitWidth}位宽超出ulong类型的范围.");
            }

            ulong rawValue = ReadUlongFromBuffer(buffer, byteOffset, ed);

            returnValue = BitwiseOperator.GetValue(rawValue, bitOffset, bitWidth);            

            return true;
        }
        #endregion

        #region 根据字节序和位偏移写入数据

        /// <summary>
        /// 设置缓冲区指定位置的的值
        /// </summary>
        /// <param name="buffer">字节数组缓冲区</param>
        /// <param name="byteOffset">起始字节偏移（从0开始），不能超出缓冲区长度</param>
        /// <param name="bitOffset">起始字节内起始位偏移（从0开始最大为7）</param>
        /// <param name="bitWidth">要设置的数据的位宽，不能超出缓冲区长度</param>
        /// <param name="setValue">要设置的值</param>
        /// <returns>设置成功返回true，设置失败返回false</returns>
        public static bool SetValue(byte[] buffer, int byteOffset, int bitOffset, int bitWidth, ref byte[] setValue)
        {
            if (buffer == null || buffer.Length < 1)
                return false;
            if (byteOffset < 0 || byteOffset > buffer.Length - 1)
                return false;
            if (bitOffset < 0 || bitOffset > 7)
                return false;
            if (bitWidth < 1)
                return false;
            if (setValue == null || setValue.Length < 1)
                return false;

            int BeginIndex = byteOffset * 8 + bitOffset;
            int EndIndex = BeginIndex + bitWidth;
            if ((float)EndIndex / 8 > buffer.Length)
                return false;

            try
            {
                BitArray objBitsBuffer = new BitArray(buffer);
                BitArray objValueBitArray = new BitArray(setValue);
                for (int cnt = BeginIndex; cnt < EndIndex; cnt++)
                    objBitsBuffer[cnt] = objValueBitArray[cnt - BeginIndex];
                objBitsBuffer.CopyTo(buffer, 0);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 设置缓冲区指定位置的的值
        /// </summary>
        /// <param name="buffer">字节数组缓冲区</param>
        /// <param name="byteOffset">起始字节偏移（从0开始），不能超出缓冲区长度</param>
        /// <param name="bitOffset">起始字节内起始位偏移（从0开始最大为7）</param>
        /// <param name="bitWidth">要设置的数据的位宽，不能超出缓冲区长度, [1, 8]</param>
        /// <param name="setValue">要设置的值</param>
        /// <returns>设置成功返回true，设置失败返回false</returns>
        public static bool SetValue(byte[] buffer, uint byteOffset, uint bitOffset, uint bitWidth, byte setValue)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer空.");
            }
            if (byteOffset < 0 || byteOffset >= buffer.Length)
            {
                throw new ArgumentOutOfRangeException($"byteOffset{byteOffset}字节偏移超出buffer的范围.");
            }
            if (bitOffset < 0 || bitOffset >= 8)
            {
                throw new ArgumentOutOfRangeException($"bitOffset{bitOffset}位偏移超出字节类型的范围.");
            }
            if (bitWidth <= 0 || bitWidth + bitOffset > 8)
            {
                throw new ArgumentOutOfRangeException($"bitWidth{bitWidth}位宽超出字节类型的范围.");
            }

            //读取Buffer
            byte origByte = buffer[byteOffset];

            BitwiseOperator.SetValue(ref origByte, bitOffset, bitWidth, setValue);

            //将值写入Buffer
            buffer[byteOffset] = origByte;

            return true;
        }

        /// <summary>
        /// 设置缓冲区指定位置的的值
        /// </summary>
        /// <param name="buffer">字节数组缓冲区</param>
        /// <param name="byteOffset">起始字节偏移（从0开始），不能超出缓冲区长度</param>
        /// <param name="bitOffset">起始字节内起始位偏移（从0开始最大为15）</param>
        /// <param name="bitWidth">要设置的数据的位宽，不能超出缓冲区长度, [1, 16]</param>
        /// <param name="setValue">要设置的值</param>
        /// <param name="ed">写入多字节数据时的字节序</param>
        /// <returns>设置成功返回true，设置失败返回false</returns>
        public static bool SetValue(byte[] buffer, uint byteOffset, uint bitOffset, uint bitWidth, short setValue, Endian ed = Endian.LittleEndian)
        {
            return SetValue(buffer, byteOffset, bitOffset, bitWidth, (ushort)setValue, ed);
        }

        /// <summary>
        /// 设置缓冲区指定位置的的值
        /// </summary>
        /// <param name="buffer">字节数组缓冲区</param>
        /// <param name="byteOffset">起始字节偏移（从0开始），不能超出缓冲区长度</param>
        /// <param name="bitOffset">起始字节内起始位偏移（从0开始最大为15）</param>
        /// <param name="bitWidth">要设置的数据的位宽，不能超出缓冲区长度, [1, 16]</param>
        /// <param name="setValue">要设置的值</param>
        /// <param name="ed">写入多字节数据时的字节序</param>
        /// <returns>设置成功返回true，设置失败返回false</returns>
        public static bool SetValue(byte[] buffer, uint byteOffset, uint bitOffset, uint bitWidth, ushort setValue, Endian ed = Endian.LittleEndian)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer空.");
            }
            if (byteOffset < 0 || byteOffset + 1 >= buffer.Length)
            {
                throw new ArgumentOutOfRangeException($"byteOffset{byteOffset}字节偏移超出buffer的范围.");
            }
            if (bitOffset < 0 || bitOffset >= 16)
            {
                throw new ArgumentOutOfRangeException($"bitOffset{bitOffset}位偏移超出ushort类型的范围.");
            }
            if (bitWidth <= 0 || bitWidth + bitOffset > 16)
            {
                throw new ArgumentOutOfRangeException($"bitWidth{bitWidth}位宽超出ushort类型的范围.");
            }

            //读取Buffer,将待写入的位清0
            ushort origShort = ReadUshortFromBuffer(buffer, byteOffset, ed);

            BitwiseOperator.SetValue(ref origShort, bitOffset, bitWidth, setValue);

            //将值写入Buffer
            WriteUshortToBuffer(buffer, byteOffset, origShort, ed);

            return true;
        }

        /// <summary>
        /// 设置缓冲区指定位置的的值
        /// </summary>
        /// <param name="buffer">字节数组缓冲区</param>
        /// <param name="byteOffset">起始字节偏移（从0开始），不能超出缓冲区长度</param>
        /// <param name="bitOffset">起始字节内起始位偏移（从0开始最大为31）</param>
        /// <param name="bitWidth">要设置的数据的位宽，不能超出缓冲区长度, [1, 32]</param>
        /// <param name="setValue">要设置的值</param>
        /// <param name="ed">写入多字节数据时的字节序</param>
        /// <returns>设置成功返回true，设置失败返回false</returns>
        public static bool SetValue(byte[] buffer, uint byteOffset, uint bitOffset, uint bitWidth, int setValue, Endian ed = Endian.LittleEndian)
        {
            return SetValue(buffer, byteOffset, bitOffset, bitWidth, (uint)setValue, ed);
        }

        /// <summary>
        /// 设置缓冲区指定位置的的值
        /// </summary>
        /// <param name="buffer">字节数组缓冲区</param>
        /// <param name="byteOffset">起始字节偏移（从0开始），不能超出缓冲区长度</param>
        /// <param name="bitOffset">起始字节内起始位偏移（从0开始最大为31）</param>
        /// <param name="bitWidth">要设置的数据的位宽，不能超出缓冲区长度, [1, 32]</param>
        /// <param name="setValue">要设置的值</param>
        /// <param name="ed">写入多字节数据时的字节序</param>
        /// <returns>设置成功返回true，设置失败返回false</returns>
        public static bool SetValue(byte[] buffer, uint byteOffset, uint bitOffset, uint bitWidth, uint setValue, Endian ed = Endian.LittleEndian)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer空.");
            }
            if (byteOffset < 0 || byteOffset + 3 >= buffer.Length)
            {
                throw new ArgumentOutOfRangeException($"byteOffset{byteOffset}字节偏移超出buffer的范围.");
            }
            if (bitOffset < 0 || bitOffset >= 32)
            {
                throw new ArgumentOutOfRangeException($"bitOffset{bitOffset}位偏移超出uint类型的范围.");
            }
            if (bitWidth <= 0 || bitWidth + bitOffset > 32)
            {
                throw new ArgumentOutOfRangeException($"bitWidth{bitWidth}位宽超出uint类型的范围.");
            }

            //读取Buffer,将待写入的位清0
            uint origValue = ReadUintFromBuffer(buffer, byteOffset, ed);

            BitwiseOperator.SetValue(ref origValue, bitOffset, bitWidth, setValue);
            //将值写入Buffer
            WriteUintToBuffer(buffer, byteOffset, origValue, ed);

            return true;
        }

        /// <summary>
        /// 设置缓冲区指定位置的的值
        /// </summary>
        /// <param name="buffer">字节数组缓冲区</param>
        /// <param name="byteOffset">起始字节偏移（从0开始），不能超出缓冲区长度</param>
        /// <param name="bitOffset">起始字节内起始位偏移（从0开始最大为63）</param>
        /// <param name="bitWidth">要设置的数据的位宽，不能超出缓冲区长度, [1, 64]</param>
        /// <param name="setValue">要设置的值</param>
        /// <param name="ed">写入多字节数据时的字节序</param>
        /// <returns>设置成功返回true，设置失败返回false</returns>
        public static bool SetValue(byte[] buffer, uint byteOffset, uint bitOffset, uint bitWidth, long setValue, Endian ed = Endian.LittleEndian)
        {
            return SetValue(buffer, byteOffset, bitOffset, bitWidth, (ulong)setValue, ed);
        }

        /// <summary>
        /// 设置缓冲区指定位置的的值
        /// </summary>
        /// <param name="buffer">字节数组缓冲区</param>
        /// <param name="byteOffset">起始字节偏移（从0开始），不能超出缓冲区长度</param>
        /// <param name="bitOffset">起始字节内起始位偏移（从0开始最大为63）</param>
        /// <param name="bitWidth">要设置的数据的位宽，不能超出缓冲区长度, [1, 64]</param>
        /// <param name="setValue">要设置的值</param>
        /// <param name="ed">写入多字节数据时的字节序</param>
        /// <returns>设置成功返回true，设置失败返回false</returns>
        public static bool SetValue(byte[] buffer, uint byteOffset, uint bitOffset, uint bitWidth, ulong setValue, Endian ed = Endian.LittleEndian)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer空.");
            }
            if (byteOffset < 0 || byteOffset + 7 >= buffer.Length)
            {
                throw new ArgumentOutOfRangeException($"byteOffset{byteOffset}字节偏移超出buffer的范围.");
            }
            if (bitOffset < 0 || bitOffset >= 64)
            {
                throw new ArgumentOutOfRangeException($"bitOffset{bitOffset}位偏移超出uint类型的范围.");
            }
            if (bitWidth <= 0 || bitWidth + bitOffset > 64)
            {
                throw new ArgumentOutOfRangeException($"bitWidth{bitWidth}位宽超出uint类型的范围.");
            }

            //读取Buffer,将待写入的位清0
            ulong origValue = ReadUlongFromBuffer(buffer, byteOffset, ed);

            BitwiseOperator.SetValue(ref origValue, bitOffset, bitWidth, setValue);
            //将值写入Buffer
            WriteUlongToBuffer(buffer, byteOffset, origValue, ed);

            return true;
        }
        #endregion

        #region 根据字节序读取多字节内容

        /// <summary>
        /// 从byte数组的指定字节偏移处读取一个ushort型数据
        /// </summary>
        /// <param name="buffer">字节数组缓冲区</param>
        /// <param name="byteOffset">从指定的字节偏移开始读取,X86计算机本身是小端模式</param>
        /// <param name="ed">指定是按大端字节序读取数据还是按小端字节序读取数据</param>
        /// <returns>返回读取的数据</returns>
        public static ushort ReadUshortFromBuffer(byte[] buffer, uint byteOffset, Endian ed)
        {
            ushort value = 0;
            if (Endian.LittleEndian == ed)
            {
                value = (ushort)(buffer[byteOffset] | buffer[byteOffset + 1] << 8);
            }
            else
            {
                value = (ushort)(buffer[byteOffset] << 8 | buffer[byteOffset + 1]);
            }
            return value;
        }

        /// <summary>
        /// 往byte数组的指定字节偏移处写入一个ushort型数据
        /// </summary>
        /// <param name="buffer">字节数组缓冲区</param>
        /// <param name="byteOffset">从指定的字节偏移开始写入,X86计算机本身是小端模式</param>
        /// <param name="value">欲写入的值</param>
        /// <param name="ed">指定是按大端字节序处理数据还是按小端字节序处理数据</param>
        /// <returns>返回修改后的BUFFER</returns>
        public static byte[] WriteUshortToBuffer(byte[] buffer, uint byteOffset, ushort value, Endian ed)
        {
            if (Endian.LittleEndian == ed)
            {
                buffer[byteOffset] = (byte)(value & 0xFF);
                buffer[byteOffset + 1] = (byte)(value >> 8);
            }
            else
            {
                buffer[byteOffset] = (byte)(value >> 8);
                buffer[byteOffset + 1] = (byte)(value & 0xFF);
            }
            return buffer;
        }


        /// <summary>
        /// 从byte数组的指定字节偏移处读取一个uint型数据
        /// </summary>
        /// <param name="buffer">字节数组缓冲区</param>
        /// <param name="byteOffset">从指定的字节偏移开始读取,X86计算机本身是小端模式</param>
        /// <param name="ed">指定是按大端字节序读取数据还是按小端字节序读取数据</param>
        /// <returns>返回读取的数据</returns>
        public static uint ReadUintFromBuffer(byte[] buffer, uint byteOffset, Endian ed)
        {
            uint value = 0;
            if (Endian.LittleEndian == ed)
            {
                value = buffer[byteOffset] | (uint)buffer[byteOffset + 1] << 8 | (uint)buffer[byteOffset + 2] << 16 | (uint)buffer[byteOffset + 3] << 24;
            }
            else
            {
                value = (uint)buffer[byteOffset] << 24 | (uint)buffer[byteOffset + 1] << 16 | (uint)buffer[byteOffset + 2] << 8 | buffer[byteOffset + 3];
            }
            return value;
        }

        /// <summary>
        /// 往byte数组的指定字节偏移处写入一个uint型数据
        /// </summary>
        /// <param name="buffer">字节数组缓冲区</param>
        /// <param name="byteOffset">从指定的字节偏移开始写入,X86计算机本身是小端模式</param>
        /// <param name="value">欲写入的值</param>
        /// <param name="ed">指定是按大端字节序处理数据还是按小端字节序处理数据</param>
        /// <returns>返回修改后的BUFFER</returns>
        public static byte[] WriteUintToBuffer(byte[] buffer, uint byteOffset, uint value, Endian ed)
        {
            if (Endian.LittleEndian == ed)
            {
                buffer[byteOffset] = (byte)(value & 0xFF);
                buffer[byteOffset + 1] = (byte)((value >> 8) & 0xFF);
                buffer[byteOffset + 2] = (byte)((value >> 16) & 0xFF);
                buffer[byteOffset + 3] = (byte)((value >> 24) & 0xFF);
            }
            else
            {
                buffer[byteOffset] = (byte)((value >> 24) & 0xFF);
                buffer[byteOffset + 1] = (byte)((value >> 16) & 0xFF);
                buffer[byteOffset + 2] = (byte)((value >> 8) & 0xFF);
                buffer[byteOffset + 3] = (byte)(value & 0xFF);
            }
            return buffer;
        }

        /// <summary>
        /// 从byte数组的指定字节偏移处读取一个long型数据
        /// </summary>
        /// <param name="buffer">字节数组缓冲区</param>
        /// <param name="byteOffset">从指定的字节偏移开始读取,X86计算机本身是小端模式</param>
        /// <param name="ed">指定是按大端字节序读取数据还是按小端字节序读取数据</param>
        /// <returns>返回读取的数据</returns>
        public static ulong ReadUlongFromBuffer(byte[] buffer, uint byteOffset, Endian ed)
        {
            ulong value = 0;
            if (Endian.LittleEndian == ed)
            {
                //c#中不允许移位超过32位，所以这里将高32位和低32位分别取出来，再将高32位左移32位组合在一起
                //c#中和移位是32位的循环移动，移动32位的值等于原值。
                value = (ulong)buffer[byteOffset] | (ulong)(buffer[byteOffset + 1] << 8)
                    | (ulong)(buffer[byteOffset + 2] << 16) | (ulong)buffer[byteOffset + 3] << 24;

                ulong ulHight = (ulong)buffer[byteOffset + 4] | (ulong)buffer[byteOffset + 5] << 8
                    | (ulong)buffer[byteOffset + 6] << 16 | (ulong)buffer[byteOffset + 7] << 24;

                value = (ulHight << 32) | value;
            }
            else
            {
                ulong ulHight = (ulong)buffer[byteOffset] << 24 | (ulong)buffer[byteOffset + 1] << 16 
                    | (ulong)buffer[byteOffset + 2] << 8 | (ulong)buffer[byteOffset + 3];

                value =  (ulong)buffer[byteOffset + 4] << 24 | (ulong)buffer[byteOffset + 5] << 16 
                    | (ulong)buffer[byteOffset + 6] << 8 | buffer[byteOffset + 7];

                value = ulHight << 32 | value;
            }
            return value;
        }

        /// <summary>
        /// 往byte数组的指定字节偏移处写入一个ulong型数据
        /// </summary>
        /// <param name="buffer">字节数组缓冲区</param>
        /// <param name="byteOffset">从指定的字节偏移开始写入,X86计算机本身是小端模式</param>
        /// <param name="value">欲写入的值</param>
        /// <param name="ed">指定是按大端字节序处理数据还是按小端字节序处理数据</param>
        /// <returns>返回修改后的BUFFER</returns>
        public static byte[] WriteUlongToBuffer(byte[] buffer, uint byteOffset, ulong value, Endian ed)
        {
            if (Endian.LittleEndian == ed)
            {
                buffer[byteOffset] = (byte)(value & 0xFF);
                buffer[byteOffset + 1] = (byte)((value >> 8) & 0xFF);
                buffer[byteOffset + 2] = (byte)((value >> 16) & 0xFF);
                buffer[byteOffset + 3] = (byte)((value >> 24) & 0xFF);
                buffer[byteOffset + 4] = (byte)((value >> 32) & 0xFF);
                buffer[byteOffset + 5] = (byte)((value >> 40) & 0xFF);
                buffer[byteOffset + 6] = (byte)((value >> 48) & 0xFF);
                buffer[byteOffset + 7] = (byte)((value >> 56) & 0xFF);
            }
            else
            {
                buffer[byteOffset] = (byte)((value >> 56) & 0xFF);
                buffer[byteOffset + 1] = (byte)((value >> 48) & 0xFF);
                buffer[byteOffset + 2] = (byte)((value >> 40) & 0xFF);
                buffer[byteOffset + 3] = (byte)((value >> 32) & 0xFF);
                buffer[byteOffset + 4] = (byte)((value >> 24) & 0xFF);
                buffer[byteOffset + 5] = (byte)((value >> 16) & 0xFF);
                buffer[byteOffset + 6] = (byte)((value >> 8) & 0xFF);
                buffer[byteOffset + 7] = (byte)(value & 0xFF);
            }

            return buffer;
        }

#endregion

        #region 基础数据类型的连续多位读写
        /// <summary>
        /// 将一个BYTE的指定连续位转换为一个新的BYTE
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bitOffset"></param>
        /// <param name="bitWidth"></param>
        /// <returns></returns>
        public static byte GetValue(byte value, uint bitOffset, uint bitWidth)
        {
            if (bitOffset > 7)
                return 0;
            if (bitWidth > 8 - bitOffset)
                return 0;

            byte mask = BitwiseOperator.CreateByteMask(bitOffset, bitWidth);

            value = (byte) ((value & mask) >> (int)bitOffset);
            return value;

        }
        /// <summary>
        /// 将一个ushort数据的指定连续位转换为一个新的ushort数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bitOffset"></param>
        /// <param name="bitWidth"></param>
        /// <returns></returns>
        public static ushort GetValue(ushort value, uint bitOffset, uint bitWidth)
        {
            if (bitOffset > 15)
                return 0;
            if (bitWidth > 16 - bitOffset)
                return 0;

            ushort mask = BitwiseOperator.CreateUshortMask(bitOffset, bitWidth);

            value = (ushort)((value & mask)>>(int)bitOffset);
            return value;
        }
        /// <summary>
        /// 将一个UINT数据的指定连续位转换为一个新的UINT
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bitOffset"></param>
        /// <param name="bitWidth"></param>
        /// <returns></returns>
        public static uint GetValue(uint value, uint bitOffset, uint bitWidth)
        {
            if (bitOffset > 31)
                return 0;
            if (bitWidth > 32 - bitOffset)
                return 0;

            uint mask = BitwiseOperator.CreateUintMask(bitOffset, bitWidth);

            value = (uint)(value & mask) >> (int)bitOffset;
            return value;
        }
        /// <summary>
        /// 将一个ulong数据的指定连续拉转换为新的ulong数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bitOffset"></param>
        /// <param name="bitWidth"></param>
        /// <returns></returns>
        public static ulong GetValue(ulong value, uint bitOffset, uint bitWidth)
        {
            if (bitOffset > 63)
                return 0;
            if (bitWidth > 64 - bitOffset)
                return 0;

            ulong mask = BitwiseOperator.CreateUlongMask(bitOffset, bitWidth);

            value = (ulong)(value & mask) >> (int)bitOffset;
            return value;
        }
        /// <summary>
        /// 使用一个byte数据的低n位填充一个byte数据的指定连续n位，如果n小于8,则取value的低n位
        /// </summary>
        /// <param name="dest">将被修改的目标byte空间</param>
        /// <param name="bitOffset">被填充的byte的起始位</param>
        /// <param name="bitWidth">被填充的byte位宽</param>
        /// <param name="value">用来填充目标的值</param>
        /// <returns></returns>
        public static byte SetValue(ref byte dest, uint bitOffset, uint bitWidth, byte value)
        {
            if (bitOffset > 7)
                return 0;
            if (bitWidth > 8 - bitOffset)
                return 0;

            byte mask = BitwiseOperator.CreateByteMask((uint)bitOffset, (uint)bitWidth);
            
            //将待写入的位清0
            dest = (byte)(dest & ~mask);
            //
            value = (byte)(mask & value << (int)bitOffset);
            dest = (byte)(dest | value);
              
            return dest;

        }
        public static ushort SetValue(ref ushort dest, uint bitOffset, uint bitWidth, ushort value)
        {
            if (bitOffset > 15)
                return 0;
            if (bitWidth > 16 - bitOffset)
                return 0;

            ushort mask = BitwiseOperator.CreateUshortMask(bitOffset, bitWidth);

            dest = (ushort)(dest & ~mask);
            value = (ushort)(mask & value << (int)bitOffset);
            dest = (ushort)(dest | value);
            return dest;
        }
        public static uint SetValue(ref uint dest, uint bitOffset, uint bitWidth, uint value)
        {
            if (bitOffset > 31)
                return 0;
            if (bitWidth > 32 - bitOffset)
                return 0;

            uint mask = BitwiseOperator.CreateUintMask(bitOffset, bitWidth);

            dest = (uint)(dest & ~mask);
            value = (uint)(mask & value << (int)bitOffset);
            dest = (uint)(dest | value);
            return dest;
        }
        public static ulong SetValue(ref ulong dest, uint bitOffset, uint bitWidth, ulong value)
        {
            if (bitOffset > 63)
                return 0;
            if (bitWidth > 64 - bitOffset)
                return 0;

            ulong mask = BitwiseOperator.CreateUlongMask(bitOffset, bitWidth);

            dest = (ulong)(dest & ~mask);
            value = (ulong)(mask & value << (int)bitOffset);
            dest = (ulong)(dest | value);
            return dest;
        }
        #endregion

    }
}
