/**
 * ==============================================================================
 *
 * Filename: ConditionQueue.cs
 * Description: 
 *
 * Version: 1.0
 * Created: 2016/7/26 11:18:42
 * Compiler: Visual Studio 2013
 *
 * Author: cc
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using System.Collections.Concurrent;
using HOTINST.COMMON.Collection;

namespace HOTINST.COMMON.WorkPool
{
    public class PacketProvider<T> : BasePrivoder, IWorkArrayProvider<T>
    {
        private PacketBuffer<T> datas;

        public PacketProvider(int compacity)
        {
            datas = new PacketBuffer<T>(compacity);
        }

        public void Push(object data)
        {
            T[] buffer = (T[])(data);
            datas.Enqueue(buffer, 0, buffer.Length);
            ReleaseSemaphore(1);
        }

        public int Size()
        {
            return datas.PacketCount;
        }

        public T[] Pop()
        {
            T[] ret = datas.Dequeue();

            return ret;
        }

        object IWorkProvider.Pop()
        {
            return Pop();
        }

        public void Push(T[] data)
        {
            datas.Enqueue(data, 0, data.Length);
            ReleaseSemaphore(1);
        }


        public void Clear()
        {
            throw new NotImplementedException();
        }
    }



	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
    public class ConditionQueue<T>: BasePrivoder, IWorkProvider<T>
	{
		private ConcurrentQueue<T> _datas;
        

		/// <summary>
		/// 
		/// </summary>
		/// <param name="maxWorker"></param>
        public ConditionQueue( int maxWorker = int.MaxValue):base(maxWorker)
		{
			_datas = new ConcurrentQueue<T>();
		}
	    public void Push(object data)
	    {
	        Push((T) data);
	    }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
		public void Push(T data)
		{
			_datas.Enqueue(data);
            ReleaseSemaphore(1);
		}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public T Top()
		{
			T ret = default(T);

            _datas.TryPeek(out ret);

            return ret;
		}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public T Pop()
		{
			T ret = default(T);

            _datas.TryDequeue(out ret);

            return ret;
		}

	    object IWorkProvider.Pop()
	    {
	        return Pop();
	    }
        public void Clear()
	    {
	        _datas = new ConcurrentQueue<T>();
        }

	    

	    /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public int Size()
		{
			return _datas.Count;
		}

	   
	}
}