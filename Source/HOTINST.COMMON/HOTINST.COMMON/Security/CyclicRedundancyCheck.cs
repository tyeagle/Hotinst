/***************************************************************
*	Copyright (C) 2013-2018 Hotinst CO.,Ltd
*	All rights reserved
*	文 件 名:	CyclicRedundancyCheck
*	CLR 版本:	4.0.30319.42000
*
*	作    者:	tanyu
*	创建时间:	2018/12/29 14:48:57
*	文件描述:	循环冗余校验		   	
*	使用生成多项式，计算CRC8，CRC16，CRC32，支持用户指定初始值、生成多项式、异或值、输入反转、输出反转
*	等参数配置多种生成算法。
*	不建议在循环中使用本库中的方法，如果要在循环中使用，建议使用查表法的优化版本。
*   
***************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HOTINST.COMMON.Bitwise;

namespace HOTINST.COMMON.Security
{
    /// <summary>
    /// 
    /// </summary>
    public static class CyclicRedundancyCheck
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="poly"></param>
        /// <param name="initValue"></param>
        /// <param name="xorOut"></param>
        /// <param name="reverseIn"></param>
        /// <param name="reverseOut"></param>
        /// <returns></returns>
        public static byte Crc8(byte[]buffer, byte poly, byte initValue, byte xorOut, bool reverseIn, bool reverseOut)
        {
            return Crc8(buffer, 0, buffer.Length, poly, initValue, xorOut, reverseIn, reverseOut);
        }

        /// <summary>
        /// 使用朴素算法计算8位冗余校验和,不建议在循环中使用,主要用于单元测试中校验快速CRC8算法的准确性。
        /// 建议使用用查表法实现的快速算法
        /// 优点是可以通过指定生成多项式、初值等参数实现各种CRC32算法。
        /// </summary>
        /// <param name="buffer">要计算CRC8的字节数组</param>
        /// <param name="offset">指定要计算CRC8的数据在数组中的起始位置</param>
        /// <param name="len">指定要计算CRC8的数据的长度</param>
        /// <param name="initValue">CRC8寄存器的初始值</param>
        /// <param name="poly">CRC8的生成多项式</param>
        /// <param name="xorOut">所有输入数据异或计算完成后，在输出最终值前，与该值进行异或操作。</param>
        /// <param name="reverseIn">输入数据的每个字节是否需要反转位序</param>
        /// <param name="reverseOut">在计算所有输入数据的异或值后，在与xorOut 进行异或前，是否需要对值进行位序的反转。</param>
        /// <returns>16位冗余校验和</returns>
        public static byte Crc8(byte[] buffer, int offset, int len,
             byte poly, byte initValue, byte xorOut, bool reverseIn, bool reverseOut)
        {
            int maxIndex = offset + len;
            int crcValue = initValue;

            if (reverseIn)
            {
                for (int i = offset; i < maxIndex; i++)
                {
                    crcValue ^= (BitwiseOperator.ReverseBitOrder(buffer[i]));
                    for (int j = 0; j < 8; j++)
                    {
                        if ((crcValue & 0x80) > 0)
                        {
                            crcValue = (crcValue << 1) ^ poly;
                        }
                        else
                        {
                            crcValue <<= 1;
                        }
                    }
                }
            }
            else
            {
                for (int i = offset; i < maxIndex; i++)
                {
                    crcValue ^= buffer[i];
                    for (int j = 0; j < 8; j++)
                    {
                        if ((crcValue & 0x80) > 0)
                        {
                            crcValue = (crcValue << 1) ^ poly;
                        }
                        else
                        {
                            crcValue <<= 1;
                        }
                    }
                }
            }

            if (reverseOut)
            {
                crcValue = BitwiseOperator.ReverseBitOrder((byte) crcValue);
            }

            crcValue ^= xorOut;

            return (byte) crcValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="poly"></param>
        /// <param name="initValue"></param>
        /// <param name="xorOut"></param>
        /// <param name="reverseIn"></param>
        /// <param name="reverseOut"></param>
        /// <returns></returns>
        public static ushort Crc16(byte[] buffer, ushort poly, ushort initValue, ushort xorOut, bool reverseIn,
            bool reverseOut)
        {
            return Crc16(buffer, 0, buffer.Length, poly, initValue, xorOut, reverseIn, reverseOut);
        }
        /// <summary>
        /// 使用朴素算法计算16位冗余校验和,不建议在循环中使用,主要用于单元测试中校验快速CRC16算法的准确性。
        /// 建议使用用查表法实现的快速算法
        /// 优点是可以通过指定生成多项式、初值等参数实现各种CRC32算法。
        /// </summary>
        /// <param name="buffer">要计算CRC16的字节数组</param>
        /// <param name="offset">指定要计算CRC16的数据在数组中的起始位置</param>
        /// <param name="len">指定要计算CRC16的数据的长度</param>
        /// <param name="initValue">CRC16寄存器的初始值</param>
        /// <param name="poly">CRC16的生成多项式</param>
        /// <param name="xorOut">所有输入数据异或计算完成后，在输出最终值前，与该值进行异或操作。</param>
        /// <param name="reverseIn">输入数据的每个字节是否需要反转位序</param>
        /// <param name="reverseOut">在计算所有输入数据的异或值后，在与xorOut 进行异或前，是否需要对值进行位序的反转。</param>
        /// <returns>16位冗余校验和</returns>
        public static ushort Crc16(byte[] buffer, int offset, int len,
            ushort poly, ushort initValue, ushort xorOut, bool reverseIn, bool reverseOut)
        {
            int maxIndex = offset + len;
            ushort crcValue = initValue;

            if (reverseIn)
            {
                for (int i = offset; i < maxIndex; i++)
                {
                    crcValue ^= (ushort)(BitwiseOperator.ReverseBitOrder(buffer[i]) << 8);
                    for (int j = 0; j < 8; j++)
                    {
                        if ((crcValue & 0x8000) > 0)
                        {
                            crcValue = (ushort)((crcValue << 1) ^ poly);
                        }
                        else
                        {
                            crcValue <<= 1;
                        }
                    }
                }
            }
            else
            {
                for (int i = offset; i < maxIndex; i++)
                {
                    crcValue ^= (ushort)(buffer[i] << 8);
                    for (int j = 0; j < 8; j++)
                    {
                        if ((crcValue & 0x8000) > 0)
                        {
                            crcValue = (ushort)((crcValue << 1) ^ poly);
                        }
                        else
                        {
                            crcValue <<= 1;
                        }
                    }
                }
            }

            if (reverseOut)
            {
                crcValue = BitwiseOperator.ReverseBitOrder(crcValue);
            }

            crcValue ^= xorOut;

            return crcValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="poly"></param>
        /// <param name="initValue"></param>
        /// <param name="xorOut"></param>
        /// <param name="reverseIn"></param>
        /// <param name="reverseOut"></param>
        /// <returns></returns>
        public static uint Crc32(byte[]buffer, uint poly, uint initValue, uint xorOut, bool reverseIn,
            bool reverseOut)
        {
            return Crc32(buffer, 0, buffer.Length, poly, initValue, xorOut, reverseIn, reverseOut);
        }

        /// <summary>
        /// 使用朴素算法计算32位冗余校验和,不建议在循环中使用,主要用于单元测试中校验快速CRC16算法的准确性。
        /// 建议使用用查表法实现的快速算法
        /// 优点是可以通过指定生成多项式、初值等参数实现各种CRC32算法。
        /// </summary>
        /// <param name="buffer">要计算CRC32的字节数组</param>
        /// <param name="offset">指定要计算CRC32的数据在数组中的起始位置</param>
        /// <param name="len">指定要计算CRC32的数据的长度</param>
        /// <param name="initValue">CRC32寄存器的初始值</param>
        /// <param name="poly">CRC16的生成多项式</param>
        /// <param name="xorOut">所有输入数据异或计算完成后，在输出最终值前，与该值进行异或操作。</param>
        /// <param name="reverseIn">输入数据的每个字节是否需要反转位序</param>
        /// <param name="reverseOut">在计算所有输入数据的异或值后，在与xorOut 进行异或前，是否需要对值进行位序的反转。</param>
        /// <returns>32位冗余校验和</returns>
        public static uint Crc32(byte[] buffer, int offset, int len,
            uint poly, uint initValue, uint xorOut, bool reverseIn, bool reverseOut)
        {
            int maxIndex = offset + len;
            uint crcValue = initValue;

            if (reverseIn)
            {
                for (int i = offset; i < maxIndex; i++)
                {
                    crcValue ^= (uint) (BitwiseOperator.ReverseBitOrder(buffer[i]) << 24);
                    for (int j = 0; j < 8; j++)
                    {
                        if ((crcValue & 0x80000000) > 0)
                        {
                            crcValue = (crcValue << 1) ^ poly;
                        }
                        else
                        {
                            crcValue <<= 1;
                        }
                    }
                }
            }
            else
            {
                for (int i = offset; i < maxIndex; i++)
                {
                    crcValue ^= (uint) buffer[i] << 24;
                    for (int j = 0; j < 8; j++)
                    {
                        if ((crcValue & 0x80000000) > 0)
                        {
                            crcValue = (crcValue << 1) ^ poly;
                        }
                        else
                        {
                            crcValue <<= 1;
                        }
                    }
                }
            }

            if (reverseOut)
            {
                crcValue = BitwiseOperator.ReverseBitOrder(crcValue);
            }

            crcValue ^= xorOut;

            return crcValue;
        }

        public static byte[] CreateCrcTable(byte poly, bool reversIn)
        {
            byte[] table = new byte[byte.MaxValue + 1];


            for (int i = 0; i <= byte.MaxValue; i++)
            {
                table[i] = GetPolySum((byte)i, poly, reversIn);
            }

            return table;
        }
        private static byte GetPolySum(byte data, byte poly, bool reversIn)
        {
            uint crcValue = data;
            if (reversIn)
            {
                crcValue = (uint)BitwiseOperator.ReverseBitOrder(data);
            }

            for (int i = 0; i < 8; i++)
            {
                if ((crcValue & 0x80) > 0)
                {
                    crcValue = ((crcValue << 1) ^ poly);
                }
                else
                {
                    crcValue <<= 1;
                }
            }

            if (reversIn)
            {
                crcValue = BitwiseOperator.ReverseBitOrder((byte)crcValue);
            }

            return (byte)crcValue;
        }
        public static ushort[] CreateCrcTable(ushort poly, bool reversIn)
        {
            ushort[] table = new ushort[byte.MaxValue + 1];


            for (int i = 0; i <= byte.MaxValue; i++)
            {
                table[i] = GetPolySum((byte)i, poly, reversIn);
            }

            return table;
        }
        private static ushort GetPolySum(byte data, ushort poly, bool reversIn)
        {
            uint crcValue = 0;
            if (reversIn)
            {
                crcValue = (uint)(BitwiseOperator.ReverseBitOrder(data) << 8);
            }
            else
            {
                crcValue = (uint)(data << 8);
            }

            for (int i = 0; i < 8; i++)
            {
                if ((crcValue & 0x8000) > 0)
                {
                    crcValue = ((crcValue << 1) ^ poly);
                }
                else
                {
                    crcValue <<= 1;
                }
            }

            if (reversIn)
            {
                crcValue = BitwiseOperator.ReverseBitOrder((ushort)crcValue);
            }
            return (ushort)crcValue;
        }
        public static uint[] CreateCrcTable(uint poly, bool reversIn)
        {
            uint[] table = new uint[byte.MaxValue + 1];


            for (int i = 0; i <= byte.MaxValue; i++)
            {
                table[i] = GetPolySum((byte)i, poly, reversIn);
            }

            return table;
        }
        private static uint GetPolySum(byte data, uint poly, bool reversIn)
        {
            uint crcValue = (uint)(data << 24);
            if (reversIn)
            {
                crcValue = (uint)(BitwiseOperator.ReverseBitOrder(data) << 24);
            }

            for (int i = 0; i < 8; i++)
            {
                if ((crcValue & 0x80000000) > 0)
                {
                    crcValue = ((crcValue << 1) ^ poly);
                }
                else
                {
                    crcValue <<= 1;
                }
            }

            if (reversIn)
            {
                crcValue = BitwiseOperator.ReverseBitOrder(crcValue);
            }

            return crcValue;
        }
        
    }
}
