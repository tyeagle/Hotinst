/**
* 命名空间: HOTINST.COMMON
*
* 功 能： N/A
* 类 名： PacketBuffer
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
* V0.01 2017/3/22 13:45:41 谭玉 初版
*
* Copyright (c) 2017 Hotinst Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　本源代码为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都华太测控技术有限公司 　　　　　　　　　　　　　　 │
*└──────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace HOTINST.COMMON.Collection
{
    /// <summary>
    /// 基于循环队列实现的一个包缓存区，与循环队列的区别是缓存数据时会将数据包的大小记录下来，读取时按每次缓存的大小来读取。
    /// 可用于缓存UDP数据报
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PacketBuffer<T>
    {
        #region 字段
        CycleBuffer<T> _innerCycleBuffer;

        Queue<int> _packetList;
        #endregion

        #region 构造析构
        public PacketBuffer(int compacity)
        {
            _innerCycleBuffer = new CycleBuffer<T>(compacity);
            _packetList = new Queue<int>();
        }
        #endregion

        #region 属性
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int PacketCount
        {
            get { return _packetList.Count; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int BufferSize
        {
            get { return _innerCycleBuffer.Count(); }
        }
        #endregion

        #region 公开方法

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public void Enqueue(T[] buffer, int offset, int count)
        {
            lock (this)
            {
                _innerCycleBuffer.Write(buffer, offset, count);
                _packetList.Enqueue(count);    
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T[] Dequeue()
        {
            lock (this)
            {
                int topPacketSize = _packetList.Dequeue();
                T[] result = _innerCycleBuffer.Read(topPacketSize);
                return result;    
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public void Dequeue(T[] buffer, int offset, out int count)
        {
            lock (this)
            {
                count = _packetList.Peek();
                Debug.Assert(offset + count <= buffer.Length);
                if (offset + count > buffer.Length)
                {
                    throw new ArgumentOutOfRangeException("buffer is too small for next packet.");
                }

                _innerCycleBuffer.Read(buffer, offset, count);
                _packetList.Dequeue();    
            }
        }
        #endregion
    }
}
