/**
* 命名空间: HOTINST.COMMON
*
* 功 能： N/A
* 类 名： BasePrivoder
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
* V0.01 2017/3/22 13:16:41 谭玉 初版
*
* Copyright (c) 2017 Hotinst Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　本源代码为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都华太测控技术有限公司 　　　　　　　　　　　　　　 │
*└──────────────────────────────────┘
*/
using System;
using System.Threading;

namespace HOTINST.COMMON.WorkPool
{
    public class BasePrivoder
    {
        protected readonly Semaphore Condition;
        protected readonly ManualResetEvent ExiteEvent;

        protected WaitHandle[] WaitHandles;


        protected int MaxWorker { get; set; }


        protected BasePrivoder(int maxworker = Int32.MaxValue)
        {
            this.MaxWorker = maxworker;

            Condition = new Semaphore(0, MaxWorker);
            ExiteEvent = new ManualResetEvent(false);

            WaitHandles = new WaitHandle[] { ExiteEvent, Condition };
        }

        public void ReleaseSemaphore(int count)
        {
            this.Condition.Release(count);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            ExiteEvent.Set();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            ExiteEvent.Reset();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="millisecondsTimeout"></param>
        /// <returns></returns>
        public DataEvent WaitData(int millisecondsTimeout)
        {
            int index = WaitHandle.WaitAny(WaitHandles, millisecondsTimeout);

            return (DataEvent)index;

            //_condition.WaitOne(millisecondsTimeout);
        }
    }
}
