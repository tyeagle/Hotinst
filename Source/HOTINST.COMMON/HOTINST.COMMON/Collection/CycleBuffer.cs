/**
* 命名空间: HOTINST.COMMON
*
* 功 能： N/A
* 类 名： CycleBuffer
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
using Math = System.Math;

namespace HOTINST.COMMON.Collection
{
    /// <summary>
    /// 一个循环队列，将内存缓存到内部分配好的数组中，减少内存分配次数，可做为通用的缓冲区使用.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CycleBuffer<T>
    {
        #region 字段
        private object _locker;

        private T[] _buffer;

        int _front;
        int _rear;
        #endregion

        #region 构造

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compacity"></param>
        public CycleBuffer(int compacity)
        {
            Debug.Assert(compacity > 1);

            _locker = new object();
            _buffer = new T[compacity + 1];

            _front = 0;
            _rear = 0;
        }

        #endregion

        #region 公有方法
        /// <summary>
        /// 容器的最大容量
        /// </summary>
        /// <returns></returns>
        public int Compacity()
        {
            return _buffer.Length - 1;
        }
        /// <summary>
        /// 容器中当前存储的元素数目
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            lock (_locker)
            {
                return (Rear() + _buffer.Length - Front()) % (_buffer.Length);
            }
        }
        /// <summary>
        /// 判断容器是否为空
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            //rear与front在相同位置时，容器为空
            lock (_locker)
            {
                return Rear() == Front();
            }
        }
        /// <summary>
        /// 判断容器是否为满
        /// </summary>
        /// <returns></returns>
        public bool IsFull()
        {
            //保留一个空位区分满与空状态,rear在front前一个位置时，容器满
            lock (_locker)
            {
                return (Rear() + 1) % (_buffer.Length) == Front();
            }
        }
        /// <summary>
        /// 清空缓存中存放的数据，但不释放内存
        /// </summary>
        public void Clear()
        {
            lock (_locker)
            {
                _front = _rear = 0;
            }
        }
		/// <summary>
		/// 向缓存中写入数组中指定偏移的数据
		/// </summary>
		/// <param name="data">在放写入数据的数组</param>
		/// <param name="offset">写入的偏移</param>
		/// <param name="count">写入的偏移</param>
		/// <returns></returns>
		public int Write(T[] data, int offset, int count)
        {
            lock (this)
            {
                int result = 0;
                int totalFree = Compacity() - Count();
                int totalToWrite = count;

                if (offset + count > data.Length)
                {
                    throw new ArgumentOutOfRangeException("Bad offset or count.");
                }

                if (IsFull())
                {
                    throw new ArgumentOutOfRangeException("Buffer is full.");
                }

                if (totalFree < totalToWrite)
                {
                    throw new ArgumentOutOfRangeException("Too much data to write.");
                }

                int sizeToWrite = 0;
                //分两段数据写
                if (Rear() >= Front())
                {
                    //先写后半段
                    //后半段最大能写的长度
                    int free2ndHalf = System.Math.Min(totalFree, _buffer.Length - Rear());

                    //
                    sizeToWrite = System.Math.Min(totalToWrite, free2ndHalf);

                    Array.Copy(data, offset, _buffer, Rear(), sizeToWrite);
                    AddRear(sizeToWrite);
                    result = sizeToWrite;

                    //再递归调用，写前半段
                    if (totalToWrite > result)
                    {
                        result += Write(data, offset + result, totalToWrite - result);
                    }
                }
                else if (Rear() + 1 < Front())
                {
                    int free1StHalf = Front() - Rear() - 1;

                    sizeToWrite = System.Math.Min(free1StHalf, totalToWrite);
                    
                    Array.Copy(data, offset, _buffer, Rear(), sizeToWrite);
                    AddRear(sizeToWrite);
                    result = sizeToWrite;
                }

                return result;
            }
        }

        /// <summary>
        /// 将Buffer中的数据读到指定的数组中,并且向后移动读指针
        /// </summary>
        /// <param name="data">接收读取内容的数组</param>
        /// <param name="offset">接收读取内容的数组偏移</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public int Read(T[] data, int offset, int count)
        {
            lock (this)
            {
                int result = 0;
                int totalCount = this.Count();
                int totalToRead = count;

                if (IsEmpty())
                {
                    throw new ArgumentOutOfRangeException("Buffer is empty.");
                }

                if (totalCount < totalToRead)
                {
                    throw new ArgumentOutOfRangeException("Not enough data to read.");
                }

                int sizeToRead = 0;
                //只有一截可读
                if (Rear() > Front())
                {
                    sizeToRead = System.Math.Min(totalToRead, Rear() - Front());
                    Array.Copy(_buffer, Front(), data, offset, sizeToRead);
                    AddFront(sizeToRead);
                    result = sizeToRead;
                }
                else
                {
                    sizeToRead = System.Math.Min(totalToRead, _buffer.Length - Front());
                    Array.Copy(_buffer, Front(), data, offset, sizeToRead);
                    AddFront(sizeToRead);

                    if (totalToRead > sizeToRead)
                    {
                        result += Read(data, offset + sizeToRead, totalToRead - sizeToRead);
                    }
                }
                return result;
            }
        }
        /// <summary>
        /// 读取Buffer中指定长度的内容，并以数组的形式返回结果。
        /// </summary>
        /// <param name="len">欲读取的长度</param>
        /// <returns>结果数组 </returns>
        public T[] Read(int len)
        {
            T[] result = new T[len];

            Read(result, 0, len);
            return result;
        }
        #endregion

        #region 保护方法
        /// <summary>
        /// 当前有效元素的附着下标
        /// </summary>
        /// <returns></returns>
        protected int Front()
        {
            lock (_locker)
            {
                return _front;
            }
        }
        /// <summary>
        /// 当前有效元素的队尾下标
        /// </summary>
        /// <returns></returns>
        protected int Rear()
        {
            lock (_locker)
            {
                return _rear;
            }
        }
        /// <summary>
        /// 移动队尾下标，到达最后时重绕回到前面
        /// </summary>
        /// <param name="step"></param>
        protected void AddRear(int step)
        {
            lock (_locker)
            {
                _rear = (Rear() + step) % _buffer.Length;
            }
            
        }
        /// <summary>
        /// 移动队首下标，到达最后时重绕回到前面
        /// </summary>
        /// <param name="step"></param>
        protected void AddFront(int step)
        {
            lock (_locker)
            {
                _front = (Front() + step) % _buffer.Length;
            }
        }
        #endregion
    }
}
