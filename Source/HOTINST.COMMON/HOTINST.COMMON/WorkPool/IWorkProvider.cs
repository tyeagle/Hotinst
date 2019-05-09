/**
* 命名空间: HOTINST.COMMON
*
* 功 能： N/A
* 类 名： BasePrivoder
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
* V0.01 2017/3/22 13:15:41 谭玉 初版
*
* Copyright (c) 2017 Hotinst Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　本源代码为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都华太测控技术有限公司 　　　　　　　　　　　　　　 │
*└──────────────────────────────────┘
*/

namespace HOTINST.COMMON.WorkPool
{
    /// <summary>
    /// 
    /// </summary>
    public enum DataEvent
    {
        /// <summary>
        /// 
        /// </summary>
        EvnetData = 1,

        /// <summary>
        /// 
        /// </summary>
        EventExite = 0
    }

    public interface IWorkProvider
    {
        void Push(object data);
        int Size();
        object Pop();
        void Start();
        void Stop();
        void Clear();
        DataEvent WaitData(int millisecondsTimeout);
    }

    public interface IWorkProvider<T> : IWorkProvider
    {
        void Push(T data);
        new T Pop();
    }

    public interface IWorkArrayProvider<T> : IWorkProvider
    {
        void Push(T[] data);
        new T[] Pop();
    }
}
