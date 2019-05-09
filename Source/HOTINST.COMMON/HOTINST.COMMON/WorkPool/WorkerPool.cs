/**
* 命名空间: HOTINST.COMMON
*
* 功 能： N/A
* 类 名： BasePrivoder
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
using System.Linq;
using System.Text;
using System.Threading;

namespace HOTINST.COMMON.WorkPool
{
    public class WorkerPool<T>
    {
        private readonly ConditionQueue<T> _workItems;
        private readonly Action<T> _actoin;
        private Thread _workThread;
        public WorkerPool(Action<T> action,  int workcount = int.MaxValue)
        {
            this._actoin = action;
             _workItems = new ConditionQueue<T>(workcount);

            
        }

        public void Start()
        {
            if (_workThread == null)
            {
                _workThread = new Thread(this.WorkThreadProc);
                _workThread.Start();
            }
        }

        public void Push(T data)
        {
            _workItems.Push(data);
        }

        public void Stop()
        {
            if (_workThread != null)
            {
                this._workItems.Stop();
                _workThread.Join(-1);
                _workThread = null;
            }
        }
        private void WorkThreadProc()
        {
            while (_workItems.WaitData(-1) != DataEvent.EventExite)
            {
                try
                {
                    T item = _workItems.Pop();
                    this._actoin(item);
                }
                catch (Exception ex)
                {
                    
                    throw;
                }
            }
        }
    }
}
