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
using System.Threading;

namespace HOTINST.COMMON.WorkPool
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WorkerThread<T>
    {
        private readonly IWorkProvider<T> _data;
		private readonly Action<T>			_action;
        private Thread						_workThread;
        private readonly object _lockobj = new object();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="action"></param>
        public WorkerThread(IWorkProvider<T> data, Action<T> action)
        {      
            _data = data;
            _action = action;
        }

        public void Stop()
        {
            _data.Stop();
            
            Join();
        }

	    public void Start()
		{
		    _workThread = new Thread(WaiteAndHandleData)
		    {
		        IsBackground = true,
		        Name = string.Format(GetType().Name)
		    };
		    _workThread.Start();
	    }

        /// <summary>
        /// 等待线程退出
        /// </summary>
        public void Join()
        {
            _workThread.Join();
        }

        private void WaiteAndHandleData()
        {
            DataEvent dataEvent = _data.WaitData(-1);

            while( DataEvent.EventExite != dataEvent )
            {
                try
                {
                    lock (_lockobj)
                    {
	                    if (_data.Size() > 0)
						{
							T oneElement = _data.Pop();
							_action(oneElement);
						}
                    }
                }
                catch(Exception)
                {
                    // ingnored
                }

                dataEvent = _data.WaitData(-1);
            }
        }

    }

    public class WorkerArrayThread<T>
    {
        private readonly IWorkArrayProvider<T> _data;
        private readonly Action<T[]> _action;
        private Thread _workThread;
        private readonly object _lockobj = new object();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="action"></param>
        public WorkerArrayThread(IWorkArrayProvider<T> data, Action<T[]> action)
        {
            _data = data;
            _action = action;
        }

        public void Stop()
        {
            _data.Stop();

            Join();
        }

        public void Start()
        {
            _workThread = new Thread(WaiteAndHandleData)
            {
                IsBackground = true,
                Name = string.Format(GetType().Name)
            };
            _workThread.Start();
        }

        /// <summary>
        /// 等待线程退出
        /// </summary>
        public void Join()
        {
            _workThread.Join();
        }

        private void WaiteAndHandleData()
        {
            DataEvent dataEvent = _data.WaitData(-1);

            while (DataEvent.EventExite != dataEvent)
            {
                try
                {
                    lock (_lockobj)
                    {
                        if (_data.Size() > 0)
                        {
                            T[] oneElement = _data.Pop();
                            _action(oneElement);
                        }
                    }
                }
                catch (Exception)
                {
                    // ingnored
                }

                dataEvent = _data.WaitData(-1);
            }
        }

    }
}
