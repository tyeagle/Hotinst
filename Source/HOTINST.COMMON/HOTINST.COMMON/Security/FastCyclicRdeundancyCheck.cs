/***************************************************************
*	Copyright (C) 2013-2019 Hotinst CO.,Ltd
*	All rights reserved
*	文 件 名:	FastCyclicRdeundancyCheck
*	CLR 版本:	4.0.30319.42000
*
*	作    者:	tanyu
*	创建时间:	2019/1/4 11:34:45
*	文件描述:	使用查表法快速进行CRC计算，解决普通版本在循环中使用是的性能问题。
*	
*	遗留问题：在初始值不为[0x00000000|0xFFFFFFFF]时，CRC16和CRC32计算有问题，暂时没空调试了，
*	常用的CRC模型的初始值基本都是全0或全1，所以暂时不影响工程使用。
*
***************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HOTINST.COMMON.Bitwise;

namespace HOTINST.COMMON.Security
{
    public class FastCyclicRdeundancyCheck32
    {

        #region 字段
        /// <summary>
        /// 默认的生成多项式,这是一个比较常用的生成多项式
        /// </summary>
        public const uint DEFAULT_POLY = 0x04C11DB7;
        /// <summary>
        /// 用于生成余式表的生成多项式
        /// </summary>
        private readonly uint Poly;
        /// <summary>
        /// 是否反转输入数据
        /// </summary>
        private readonly bool ReverseIn;
        /// <summary>
        /// 由生成多项式生成的余式表
        /// </summary>
        private readonly uint[] table;

        /// <summary>
        /// 初始值
        /// </summary>
        private readonly uint InitValue;
        /// <summary>
        /// 输出异或值
        /// </summary>
        private readonly uint XorOutValue;
        /// <summary>
        /// 是否反转输出结果
        /// </summary>
        private bool reverseOut;
        #endregion

        #region 静态属性
        /// <summary>
        /// 多项式0x04C11DB7, 初始值0xFFFFFFFF, 结果异或值0xFFFFFFFF, 输入值反转:false, 输出值反转:false
        /// </summary>
        public static FastCyclicRdeundancyCheck32 Crc32 = new FastCyclicRdeundancyCheck32(0x04C11DB7, 0xFFFFFFFF, 0xFFFFFFFF, true, true);
        /// <summary>
        /// 多项式0x04C11DB7, 初始值0xFFFFFFFF, 结果异或值0xFFFFFFFF, 输入值反转:false, 输出值反转:false
        /// </summary>
        public static FastCyclicRdeundancyCheck32 Crc32BZip = new FastCyclicRdeundancyCheck32(0x04C11DB7, 0xFFFFFFFF, 0xFFFFFFFF, false, false);
        /// <summary>
        /// 多项式0x04C11DB7, 初始值0xFFFFFFFF, 结果异或值0x00000000, 输入值反转:false, 输出值反转:false
        /// </summary>
        public static FastCyclicRdeundancyCheck32 Crc32Mpeg2 = new FastCyclicRdeundancyCheck32(0x04C11DB7, 0xFFFFFFFF, 0x00000000, false, false);
        /// <summary>
        /// 多项式0xED888320, 初始值0xFFFFFFFF, 结果异或值0x00000000, 输入值反转:true, 输出值反转:false
        /// </summary>
        public static FastCyclicRdeundancyCheck32 Crc32Jmacrc = new FastCyclicRdeundancyCheck32(0xED888320, 0xFFFFFFFF, 0x00000000, true, false);
        /// <summary>
        /// 多项式0x1EDC6F41, 初始值0xFFFFFFFF, 结果异或值0xFFFFFFFF, 输入值反转:false, 输出值反转:false
        /// </summary>
        public static FastCyclicRdeundancyCheck32 Crc32Sctp = new FastCyclicRdeundancyCheck32(0x1EDC6F41, 0xFFFFFFFF, 0xFFFFFFFF, false, false);
        #endregion

        #region 构造板构
        public FastCyclicRdeundancyCheck32() : this(DEFAULT_POLY, 0xFFFFFFFF, 0xFFFFFFFF, true, false)
        {

        }
        public FastCyclicRdeundancyCheck32(uint poly, uint initValue, uint xorOut, bool reversIn, bool reverseOut)
        {
            this.Poly = poly;
            this.InitValue = initValue;
            this.XorOutValue = xorOut;

            this.ReverseIn = reversIn;
            this.reverseOut = reverseOut;

            table = CreateCrc32Table(poly, reversIn);
        }


        #endregion

        #region 公共方法
        public uint CrcCode(byte[] buffer)
        {
            return CrcCode(buffer, 0, buffer.Length);
        }
        public uint CrcCode(byte[] buffer, int offset, int len)
        {
            return CrcCode(buffer, offset, len, this.InitValue, this.XorOutValue, this.reverseOut);
        }
        public uint CrcCode(byte[] buffer, int offset, int len, uint initValue, uint xorOut, bool reverseOut)
        {
            uint crcValue = initValue;

            int maxIndex = offset + len;
            ///低位在前
            if (ReverseIn)
            {
                for (int i = offset; i < maxIndex; i++)
                {
                    crcValue = ((crcValue >> 8) ^ table[((crcValue ^ buffer[i]) & 0xFF)]);
                }
            }
            else//高位在前
            {
                for (int i = offset; i < maxIndex; i++)
                {
                    crcValue = ((crcValue << 8) ^ table[((crcValue >> 24 ^ buffer[i]) & 0xFF)]);
                }
            }

            if (ReverseIn && !reverseOut)
            {
                crcValue = BitwiseOperator.ReverseBitOrder(crcValue);
            }
            crcValue = crcValue ^ xorOut;

            return crcValue;
        }


        #endregion

        #region 私有方法
        private uint[] CreateCrc32Table(uint poly, bool reverseIn)
        {
            uint[] table = new uint[byte.MaxValue + 1];

            for (int i = 0; i <= byte.MaxValue; i++)
            {
                table[i] = GetPolySum((byte)i, poly, reverseIn);
            }

            return table;
        }
        private uint GetPolySum(byte data, uint poly, bool reverseIn)
        {
            uint crcValue = (uint)(data << 24);
            if (reverseIn)
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

            if (reverseIn)
            {
                crcValue = BitwiseOperator.ReverseBitOrder(crcValue);
            }

            return crcValue;
        }


        #endregion
    }


    public class FastCyclicRdeundancyCheck16
    {
        #region 字段
        /// <summary>
        /// 默认的生成多项式,这是一个比较常用的生成多项式
        /// </summary>
        public const ushort DEFAULT_POLY = 0x8005;
        /// <summary>
        /// 用于生成余式表的生成多项式
        /// </summary>
        private readonly ushort Poly;
        /// <summary>
        /// 是否反转输入数据
        /// </summary>
        private readonly bool ReverseIn;
        /// <summary>
        /// 由生成多项式生成的余式表
        /// </summary>
        private readonly ushort[] table;

        /// <summary>
        /// 初始值
        /// </summary>
        private readonly ushort InitValue;
        /// <summary>
        /// 输出异或值
        /// </summary>
        private readonly ushort XorOutValue;
        /// <summary>
        /// 是否反转输出结果
        /// </summary>
        private bool reverseOut;


        #endregion

        #region 静态属性
        /// <summary>
        /// 多项式0x8005, 初始值0x0000, 结果异或值0x0000, 输入值反转:true, 输出值反转:true
        /// </summary>
        public static FastCyclicRdeundancyCheck16 Crc16Ibm = new FastCyclicRdeundancyCheck16(0x8005, 0, 0, true, true);
        /// <summary>
        /// 多项式0x8005, 初始值0x0000, 结果异或值0xFFFF, 输入值反转:true, 输出值反转:true
        /// </summary>
        public static FastCyclicRdeundancyCheck16 Crc16Maxim = new FastCyclicRdeundancyCheck16(0x8005, 0, 0xFFFF, true, true);
        /// <summary>
        /// 多项式0x8005, 初始值0xFFFF, 结果异或值0xFFFF, 输入值反转:true, 输出值反转:true
        /// </summary>
        public static FastCyclicRdeundancyCheck16 Crc16Usb = new FastCyclicRdeundancyCheck16(0x8005, 0xffff, 0xffff, true, true);
        /// <summary>
        /// 多项式0x8005, 初始值0xFFFF, 结果异或值0x0000, 输入值反转:true, 输出值反转:true
        /// </summary>
        public static FastCyclicRdeundancyCheck16 Crc16Modbus = new FastCyclicRdeundancyCheck16(0x8005, 0xffff, 0x0000, true, true);
        /// <summary>
        /// 多项式0x1021, 初始值0x0000, 结果异或值0x0000, 输入值反转:true, 输出值反转:true
        /// </summary>
        public static FastCyclicRdeundancyCheck16 Crc16Ccit = new FastCyclicRdeundancyCheck16(0x1021, 0, 0, true, true);
        /// <summary>
        /// 多项式0x1021, 初始值0xFFFF, 结果异或值0x0000, 输入值反转:false, 输出值反转:false
        /// </summary>
        public static FastCyclicRdeundancyCheck16 Crc16CcitFalce = new FastCyclicRdeundancyCheck16(0x1021, 0xffff, 0, false, false);
        /// <summary>
        /// 多项式0x1021, 初始值0xFFFF, 结果异或值0xFFFF, 输入值反转:true, 输出值反转:true
        /// </summary>
        public static FastCyclicRdeundancyCheck16 Crc16X25 = new FastCyclicRdeundancyCheck16(0x1021, 0xffff, 0xffff, true, true);
        /// <summary>
        /// 多项式0x1021, 初始值0x0000, 结果异或值0x0000, 输入值反转:false, 输出值反转:false
        /// </summary>
        public static FastCyclicRdeundancyCheck16 Crc16XModem = new FastCyclicRdeundancyCheck16(0x1021, 0, 0, false, false);
        /// <summary>
        /// 多项式0x3D65, 初始值0x0000, 结果异或值0xFFFF, 输入值反转:true, 输出值反转:true
        /// </summary>
        public static FastCyclicRdeundancyCheck16 Crc16Dnp = new FastCyclicRdeundancyCheck16(0x3d65, 0x0000, 0xffff, true, true);

        #endregion

        #region 构造板构
        public FastCyclicRdeundancyCheck16() : this(DEFAULT_POLY, 0xFFFF, 0xFFFF, true, true)
        {

        }
        public FastCyclicRdeundancyCheck16(ushort poly, ushort initValue, ushort xorOut, bool reversIn, bool reverseOut)
        {
            this.Poly = poly;
            this.InitValue = initValue;
            this.XorOutValue = xorOut;

            this.ReverseIn = reversIn;
            this.reverseOut = reverseOut;

            table = CreateCrc16Table(poly, reversIn);
        }


        #endregion

        #region 公共方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public ushort CrcCode(byte[] buffer)
        {
            return CrcCode(buffer, 0, buffer.Length);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public ushort CrcCode(byte[] buffer, int offset, int len)
        {
            return CrcCode(buffer, offset, len, this.InitValue, this.XorOutValue, this.reverseOut);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <param name="initValue"></param>
        /// <param name="xorOut"></param>
        /// <param name="reverseOut"></param>
        /// <returns></returns>
        public ushort CrcCode(byte[] buffer, int offset, int len, uint initValue, uint xorOut, bool reverseOut)
        {
            uint crcValue = initValue;

            int maxIndex = offset + len;

            if (ReverseIn)
            {
                for (int i = offset; i < maxIndex; i++)
                {
                    crcValue = ((crcValue >> 8) ^ table[((crcValue ^ buffer[i]) & 0xFF)]);
                }
            }
            else
            {
                for (int i = offset; i < maxIndex; i++)
                {
                    crcValue = ((crcValue << 8) ^ table[((crcValue >> 8 ^ buffer[i]) & 0xFF)]);
                }
            }

            if (ReverseIn && !this.reverseOut)
            {
                crcValue = BitwiseOperator.ReverseBitOrder((ushort)crcValue);
            }
            crcValue = crcValue ^ xorOut;

            return (ushort)crcValue;
        }


        #endregion
        
        #region 私有方法
        private ushort[] CreateCrc16Table(ushort poly, bool reverseIn)
        {
            ushort[] table = new ushort[byte.MaxValue + 1];

            for (int i = 0; i <= byte.MaxValue; i++)
            {
                table[i] = GetPolySum((byte)i, poly, reverseIn);
            }

            return table;
        }
        private ushort GetPolySum(byte data, ushort poly, bool reverseIn)
        {
            uint crcValue = (uint)(data << 8);
            if (reverseIn)
            {
                crcValue = (uint)(BitwiseOperator.ReverseBitOrder(data) << 8);
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

            if (reverseIn)
            {
                crcValue = BitwiseOperator.ReverseBitOrder((ushort)crcValue);
            }

            return (ushort)crcValue;
        }


        #endregion
    }

    public class FastCyclicRdeundancyCheck8
    {
        #region 字段
        /// <summary>
        /// 默认的生成多项式,这是一个比较常用的生成多项式
        /// </summary>
        public const Byte DEFAULT_POLY = 0x07;
        /// <summary>
        /// 用于生成余式表的生成多项式
        /// </summary>
        private readonly Byte Poly;
        /// <summary>
        /// 是否反转输入数据
        /// </summary>
        private readonly bool ReverseIn;
        /// <summary>
        /// 由生成多项式生成的余式表
        /// </summary>
        private readonly Byte[] table;

        /// <summary>
        /// 初始值
        /// </summary>
        private readonly Byte InitValue;
        /// <summary>
        /// 输出异或值
        /// </summary>
        private readonly Byte XorOutValue;
        /// <summary>
        /// 是否反转输出结果
        /// </summary>
        private bool reverseOut;


        #endregion

        #region 静态属性
        /// <summary>
        /// 宽度8，多项式0x07, 初始值0,结果异或值0,输入值反转:false, 输出值反转:false
        /// </summary>
        public static FastCyclicRdeundancyCheck8 Crc8 = new FastCyclicRdeundancyCheck8(0x07, 0, 0, false, false);
        /// <summary>
        /// 宽度8，多项式0x07, 初始值0, 结果异或值0x55,输入值反转:false, 输出值反转:false
        /// </summary>
        public static FastCyclicRdeundancyCheck8 Crc8Itu = new FastCyclicRdeundancyCheck8(0x07, 0, 0x55, false, false);
        /// <summary>
        /// 宽度8，多项式0x07, 初始值0xff, 结果异或值0x00,输入值反转:true, 输出值反转:true
        /// </summary>
        public static FastCyclicRdeundancyCheck8 Crc8Rohc = new FastCyclicRdeundancyCheck8(0x07, 0xff, 0, true, true);
        /// <summary>
        /// 宽度8，多项式0x31, 初始值0x00, 结果异或值0x00,输入值反转:true, 输出值反转:true
        /// </summary>
        public static FastCyclicRdeundancyCheck8 Crc8Maxim = new FastCyclicRdeundancyCheck8(0x31, 0, 0, true, true);
        

        #endregion

        #region 构造板构
        public FastCyclicRdeundancyCheck8() : this(DEFAULT_POLY, 0xFF, 0xFF, true, true)
        {

        }
        public FastCyclicRdeundancyCheck8(Byte poly, Byte initValue, Byte xorOut, bool reversIn, bool reverseOut)
        {
            this.Poly = poly;
            this.InitValue = initValue;
            this.XorOutValue = xorOut;

            this.ReverseIn = reversIn;
            this.reverseOut = reverseOut;

            table = CreateTable(poly, reversIn);
        }


        #endregion

        #region 公共方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public Byte CrcCode(byte[] buffer)
        {
            return CrcCode(buffer, 0, buffer.Length);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public Byte CrcCode(byte[] buffer, int offset, int len)
        {
            return CrcCode(buffer, offset, len, this.InitValue, this.XorOutValue, this.reverseOut);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <param name="initValue"></param>
        /// <param name="xorOut"></param>
        /// <param name="reverseOut"></param>
        /// <returns></returns>
        public Byte CrcCode(byte[] buffer, int offset, int len, uint initValue, uint xorOut, bool reverseOut)
        {
            uint crcValue = initValue;

            int maxIndex = offset + len;


            for (int i = offset; i < maxIndex; i++)
            {
                crcValue = table[((crcValue ^ buffer[i]) & 0xFF)];
            }

            if (ReverseIn && ReverseIn != reverseOut)
            {
                crcValue = BitwiseOperator.ReverseBitOrder((byte)crcValue);
            }
            crcValue = crcValue ^ xorOut;

            return (Byte)crcValue;
        }


        #endregion

        #region 私有方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="poly"></param>
        /// <param name="reverseIn"></param>
        /// <returns></returns>
        private Byte[] CreateTable(Byte poly, bool reverseIn)
        {
            Byte[] table = new Byte[byte.MaxValue + 1];

            for (int i = 0; i <= byte.MaxValue; i++)
            {
                table[i] = GetPolySum((byte)i, poly, reverseIn);
            }

            return table;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="poly"></param>
        /// <param name="reverseIn"></param>
        /// <returns></returns>
        private Byte GetPolySum(byte data, Byte poly, bool reverseIn)
        {
            Byte crcValue = data;
            if (reverseIn)
            {
                crcValue = BitwiseOperator.ReverseBitOrder(data);
            }

            for (int i = 0; i < 8; i++)
            {
                if ((crcValue & 0x80) > 0)
                {
                    crcValue = (Byte)((crcValue << 1) ^ poly);
                }
                else
                {
                    crcValue <<= 1;
                }
            }

            if (reverseIn)
            {
                crcValue = BitwiseOperator.ReverseBitOrder((Byte)crcValue);
            }

            return crcValue;
        }


        #endregion
    }
}

