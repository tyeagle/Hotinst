/***************************************************************
*	Copyright (C) 2013-2018 Hotinst CO.,Ltd
*	All rights reserved
*	文 件 名:	BitHelper
*	CLR 版本:	4.0.30319.42000
*
*	作    者:	tanyu
*	创建时间:	2016/6/4 11:57:46
*	文件描述:	对基础数据类型的单个位进行操作，置位，除位，反转位，反转整个数据的位序
*
***************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HOTINST.COMMON.Bitwise
{
    /// <summary>
    /// 对基本类型进行位操作的工具
    /// </summary>
    public class BitwiseOperator
    {
        #region 掩码
        /// <summary>
        /// 创建一个字节类型的掩码，起始位和位宽指定的范围为全1，其余位为全0
        /// </summary>
        /// <param name="bitOffset">字节内起始位偏移, [0, 7]</param>
        /// <param name="bitWidth">全1数据位的位数，[1, 8]</param>
        /// <returns>返回创建好的掩码字节</returns>
        public static byte CreateByteMask(uint bitOffset, uint bitWidth)
        {
            byte mask = (byte)(byte.MaxValue >> (Int32)bitOffset);
            mask = (byte)(mask << (Int32)(8 - bitWidth));
            mask = (byte)(mask >> (Int32)(8 - bitWidth - bitOffset));

            return mask;
        }
        /// <summary>
        /// 创建一个ushort的掩码，起始位和位宽指定的范围为全1，其余位为全0的
        /// </summary>
        /// <param name="bitOffset">字节内起始位偏移, [0, 15]</param>
        /// <param name="bitWidth">全1数据位的位数，[1, 16]</param>
        /// <returns>返回创建好的掩码</returns>
        public static ushort CreateUshortMask(uint bitOffset, uint bitWidth)
        {
            ushort mask = (ushort)(ushort.MaxValue >> (Int32)bitOffset);
            mask = (ushort)(mask << (Int32)(16 - bitWidth));
            mask = (ushort)(mask >> (Int32)(16 - bitWidth - bitOffset));

            return mask;
        }
        /// <summary>
        /// 创建一个uint的掩码，起始位和位宽指定的范围为全1，其余位为全0的
        /// </summary>
        /// <param name="bitOffset">字节内起始位偏移, [0, 15]</param>
        /// <param name="bitWidth">全1数据位的位数，[1, 16]</param>
        /// <returns>返回创建好的掩码</returns>
        public static uint CreateUintMask(uint bitOffset, uint bitWidth)
        {
            uint mask = (uint)(uint.MaxValue >> (Int32)bitOffset);
            mask = (uint)(mask << (Int32)(32 - bitWidth));
            mask = (uint)(mask >> (Int32)(32 - bitWidth - bitOffset));

            return mask;
        }

        /// <summary>
        /// 创建一个ulong的掩码，起始位和位宽指定的范围为全1，其余位为全0的
        /// </summary>
        /// <param name="bitOffset">字节内起始位偏移, [0, 15]</param>
        /// <param name="bitWidth">全1数据位的位数，[1, 16]</param>
        /// <returns>返回创建好的掩码</returns>
        public static ulong CreateUlongMask(uint bitOffset, uint bitWidth)
        {
            ulong mask = (ulong)(ulong.MaxValue >> (Int32)bitOffset);
            mask = (ulong)(mask << (Int32)(64 - bitWidth));
            mask = (ulong)(mask >> (Int32)(64 - bitWidth - bitOffset));

            return mask;
        }
        #endregion

        # region 将位反向
        /*
        将一个基础数据类型的某一位设置为与原来相反的值 
        */
        /// <summary>
        /// 将valeu中mask为1的位取反，其余位不变
        /// </summary>
        /// <param name="value"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        public static byte ReverseBit(byte value, byte mask)
        {
            //保存除取反位之外的其余位
            byte remainBits = (byte)(value & (~mask));

            //
            byte reverseAll = (byte)(value ^ mask);

            value = (byte)(remainBits | reverseAll);

            return value;
        }

        public static ushort ReverseBit(ushort value, ushort mask)
        {
            //保存除取反位之外的其余位
            ushort remainBits = (ushort)(value & (~mask));

            //
            ushort reverseAll = (ushort)(value ^ mask);

            value = (ushort)(remainBits | reverseAll);

            return value;
        }

        public static UInt32 ReverseBit(UInt32 value, UInt32 mask)
        {
            //保存除取反位之外的其余位
            UInt32 remainBits = (UInt32)(value & (~mask));

            //
            UInt32 reverseAll = (UInt32)(value ^ mask);

            value = (UInt32)(remainBits | reverseAll);

            return value;
        }
        /// <summary>
        /// C#的位操作返回值是32整形，好像写起来有点麻烦，不写了
        /// </summary>
        /// <param name="value"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        public static UInt64 ReverseBit(UInt64 value, UInt64 mask)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 反转位序
        /// <summary>
        /// 将一个字节的位序进行逆转
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte ReverseBitOrder(byte value)
        {
            // 交换每两位
            value = (byte)(((value >> 1) & 0x55) | ((value & 0x55) << 1)); // abcdefgh -> badcfehg
            // 交换每四位中的前两位和后两位
            value = (byte)(((value >> 2) & 0x33) | ((value & 0x33) << 2)); // badcfehg -> dcbahgfe
            // 交换前四位和后四位
            value = (byte)((value >> 4) | (value << 4)); // dcbahgfe -> hgfedcba

            return value;
        }

        public static ushort ReverseBitOrder(ushort value)
        {
            // 交换每两位
            value = (ushort)(((value >> 1) & 0x5555) | ((value & 0x5555) << 1));
            // 交换每四位中的前两位和后两位
            value = (ushort)(((value >> 2) & 0x3333) | ((value & 0x3333) << 2));
            // 交换每八位中的前四位和后四位
            value = (ushort)(((value >> 4) & 0x0F0F) | ((value & 0x0F0F) << 4));
            // 交换相邻的两个字节
            value = (ushort)(((value >> 8) & 0x00FF) | ((value & 0x00FF) << 8));

            return value;
        }


        public static UInt32 ReverseBitOrder(UInt32 value)
        {
            // 交换每两位
            value = ((value >> 1) & 0x55555555) | ((value & 0x55555555) << 1);
            // 交换每四位中的前两位和后两位
            value = ((value >> 2) & 0x33333333) | ((value & 0x33333333) << 2);
            // 交换每八位中的前四位和后四位
            value = ((value >> 4) & 0x0F0F0F0F) | ((value & 0x0F0F0F0F) << 4);
            // 交换相邻的两个字节
            value = ((value >> 8) & 0x00FF00FF) | ((value & 0x00FF00FF) << 8);
            // 交换前后两个双字节
            value = (value >> 16) | (value << 16);

            return value;
        }

        #endregion

        #region 读取位
        /// <summary>
        /// 获取一个字节的第几位是否为1,位数为[0-7],为1返回true,为0返回false
        /// </summary>
        /// <param name="value">要读取位的字节值</param>
        /// <param name="bit">第几位</param>
        public static bool GetBit(byte value, int bit)
        {
            byte mask = CreateByteMask((uint)bit, 1);
            return (value & mask) > 0;
        }

        public static bool GetBit(ushort value, int bit)
        {
            ushort mask = CreateUshortMask((uint)bit, 1);
            return (value & mask) > 0;
        }

        public static bool GetBit(uint value, int bit)
        {
            uint mask = CreateUintMask((uint)bit, 1);
            return (value & mask) > 0;
        }

        public static bool GetBit(ulong value, int bit)
        {
            ulong mask = CreateUlongMask((uint)bit, 1);
            return (value & mask) > 0;
        }



        #endregion

        #region 设置位
        /// <summary>
        /// 将一个字节的指定位设置为1
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bit"></param>
        /// <returns></returns>
        public static byte SetBit(byte value, int bit)
        {
            byte mask = CreateByteMask((uint)bit, 1);

            value |= (byte)(mask & 0xFF);

            return value;
        }
        /// <summary>
        /// 将一个ushort类型数据的指定位设置为1
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bit"></param>
        /// <returns></returns>
        public static ushort SetBit(ushort value, int bit)
        {
            ushort mask = CreateUshortMask((uint)bit, 1);

            value |= (ushort)(mask & 0xFF);

            return value;
        }
        /// <summary>
        /// 将一个uint类型数据的指定位设置为1
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bit"></param>
        /// <returns></returns>
        public static uint SetBit(uint value, int bit)
        {
            uint mask = CreateUintMask((uint)bit, 1);

            value |= (uint)(mask & 0xFF);

            return value;
        }
        /// <summary>
        /// 将一个 ulong型数据的指定位设置为1
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bit"></param>
        /// <returns></returns>
        public static ulong SetBit(ulong value, int bit)
        {
            ulong mask = CreateUlongMask((uint)bit, 1);

            value |= (ulong)(mask & 0xFF);

            return value;
        }
        #endregion

        #region 清除位
        /// <summary>
        /// 将一个BYTE数据的指定位设置为0
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bit"></param>
        /// <returns></returns>
        public static byte ClearBit(byte value, int bit)
        {
            byte mask = CreateByteMask((uint)bit, 1);
            value &= (byte)~mask;

            return value;
        }
        /// <summary>
        /// 将一个ushort类型的指定位设置0
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bit"></param>
        /// <returns></returns>
        public static ushort ClearBit(ushort value, int bit)
        {
            ushort mask = CreateUshortMask((uint)bit, 1);

            value &= (ushort)~mask;

            return value;
        }
        /// <summary>
        /// /将一个UINT数据的指定位设置为0
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bit"></param>
        /// <returns></returns>
        public static uint ClearBit(uint value, int bit)
        {
            uint mask = CreateUintMask((uint)bit, 1);

            value &= (uint)~mask;

            return value;
        }
        /// <summary>
        /// 将一个ulong数据的指定位设置为0
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bit"></param>
        /// <returns></returns>
        public static ulong ClearBit(ulong value, int bit)
        {
            ulong mask = CreateUlongMask((uint)bit, 1);

            value &= (ulong)~mask;

            return value;
        }
        #endregion

        #region 按位读取数据
        /// <summary>
        /// 读取一个Byte数据的指定范围，并将读取结果转换为一个byte
        /// </summary>
        /// <param name="value">要读取的byte数据</param>
        /// <param name="bitOffset">范围的指定起始位</param>
        /// <param name="bitWidth">范围的指定结束位</param>
        /// <returns>读取指定范围返回的较小的byte</returns>
        public static byte GetValue(byte value, uint bitOffset, uint bitWidth)
        {
            //创建待读取位的掩码值，待读取位为1，其余位为0
            byte byteMask = CreateByteMask(bitOffset, bitWidth);

            //读取Buffer,只取出待读取的位
            byte readValue = (byte)(value & byteMask);

            //将待读取的起始位移动到0位
            byte returnValue = (byte)(readValue >> (Int32)bitOffset);

            return returnValue;
        }
        /// <summary>
        /// 读取一个ushort数据的指定范围，并将读取结果转换为一个ushort
        /// </summary>
        /// <param name="value">要读取的byte数据</param>
        /// <param name="bitOffset">范围的指定起始位</param>
        /// <param name="bitWidth">范围的指定结束位</param>
        /// <returns>读取指定范围返回的较小的ushort</returns>
        public static ushort GetValue(ushort value, uint bitOffset, uint bitWidth)
        {
            ushort mask = CreateUshortMask(bitOffset, bitWidth);

            ushort readValue = (ushort)(value & mask);

            ushort returnValue = (ushort)(readValue >> (Int32)bitOffset);

            return returnValue;
        }
        /// <summary>
        /// 读取一个uint数据的指定范围，并将读取结果转换为一个uint
        /// </summary>
        /// <param name="value">要读取的byte数据</param>
        /// <param name="bitOffset">范围的指定起始位</param>
        /// <param name="bitWidth">范围的指定结束位</param>
        /// <returns>读取指定范围返回的较小的uint</returns>
        public static uint GetValue(uint value, uint bitOffset, uint bitWidth)
        {
            uint mask = CreateUintMask(bitOffset, bitWidth);

            uint readValue = (value & mask);

            uint returnValue = (readValue >> (Int32)bitOffset);

            return returnValue;
        }
        /// <summary>
        /// 读取一个ulong数据的指定范围，并将读取结果转换为一个ulong
        /// </summary>
        /// <param name="value">要读取的byte数据</param>
        /// <param name="bitOffset">范围的指定起始位</param>
        /// <param name="bitWidth">范围的指定结束位</param>
        /// <returns>读取指定范围返回的较小的ulong</returns>
        public static ulong GetValue(ulong value, uint bitOffset, uint bitWidth)
        {
            ulong mask = CreateUlongMask(bitOffset, bitWidth);

            ulong readValue = (value & mask);

            ulong returnValue = (readValue >> (Int32)bitOffset);

            return returnValue;
        }
        #endregion

        #region 按位写入数据
        /// <summary>
        /// 将一个目标byte数据的指定范围写入一个byte的最低N位，N与指定范围的位宽相同
        /// </summary>
        /// <param name="target">将被写入覆盖的目标数据</param>
        /// <param name="bitOffset">范围的指定起始位</param>
        /// <param name="bitWidth">范围的指定结束位</param>
        /// <param name="setValue">写入内容</param>
        public static void SetValue(ref byte target, uint bitOffset, uint bitWidth, byte setValue)
        {
            //创建待读取位的掩码值，待读取位为1，其余位为0
            byte byteMask = CreateByteMask(bitOffset, bitWidth);

            //清空待写入的位,保留其它位
            target = (byte)(target & (~byteMask));

            //将写入值按起始位切片和对齐
            setValue = (byte)((setValue << (Int32)bitOffset) & byteMask);

            //写入值写入到目标位置
            target = (byte)(target | setValue);
        }
        /// <summary>
        /// 将一个目标ushort数据的指定范围写入一个ushort的最低N位，N与指定范围的位宽相同
        /// </summary>
        /// <param name="target">将被写入覆盖的目标数据</param>
        /// <param name="bitOffset">范围的指定起始位</param>
        /// <param name="bitWidth">范围的指定结束位</param>
        /// <param name="setValue">写入内容</param>
        public static void SetValue(ref ushort target, uint bitOffset, uint bitWidth, ushort setValue)
        {
            //创建待读取位的掩码值，待读取位为1，其余位为0
            ushort byteMask = CreateUshortMask(bitOffset, bitWidth);

            //清空待写入的位,保留其它位
            target = (ushort)(target & (~byteMask));

            //将写入值按起始位切片和对齐
            setValue = (ushort)((setValue << (Int32)bitOffset) & byteMask);

            //写入值写入到目标位置
            target = (ushort)(target | setValue);
        }
        /// <summary>
        /// 将一个目标uint数据的指定范围写入一个uint的最低N位，N与指定范围的位宽相同
        /// </summary>
        /// <param name="target">将被写入覆盖的目标数据</param>
        /// <param name="bitOffset">范围的指定起始位</param>
        /// <param name="bitWidth">范围的指定结束位</param>
        /// <param name="setValue">写入内容</param>
        public static void SetValue(ref uint target, uint bitOffset, uint bitWidth, uint setValue)
        {
            //创建待读取位的掩码值，待读取位为1，其余位为0
            uint byteMask = CreateUintMask(bitOffset, bitWidth);

            //清空待写入的位,保留其它位
            target = (target & (~byteMask));

            //将写入值按起始位切片和对齐
            setValue = ((setValue << (Int32)bitOffset) & byteMask);

            //写入值写入到目标位置
            target = (target | setValue);
        }
        /// <summary>
        /// 将一个目标ulong数据的指定范围写入一个ulong的最低N位，N与指定范围的位宽相同
        /// </summary>
        /// <param name="target">将被写入覆盖的目标数据</param>
        /// <param name="bitOffset">范围的指定起始位</param>
        /// <param name="bitWidth">范围的指定结束位</param>
        /// <param name="setValue">写入内容</param>
        public static void SetValue(ref ulong target, uint bitOffset, uint bitWidth, ulong setValue)
        {
            //创建待读取位的掩码值，待读取位为1，其余位为0
            ulong byteMask = CreateUlongMask(bitOffset, bitWidth);

            //清空待写入的位,保留其它位
            target = (target & (~byteMask));

            //将写入值按起始位切片和对齐
            setValue = ((setValue << (Int32)bitOffset) & byteMask);

            //写入值写入到目标位置
            target = (target | setValue);
        }
        #endregion
    }
}
