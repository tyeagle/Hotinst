using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HOTINST.COMMON.Timer;
using System.Diagnostics;
using HOTINST.COMMON.Win32;

namespace UnitTestProject
{
    /// <summary>
    /// UnitTimer 的摘要说明
    /// </summary>
    [TestClass]
    public class UnitTimer
    {
        private Stopwatch[] _objStopwatchs;
        private ITimer[] _objTimers;
        private int _intMinRange;
        private int _intMaxRange;
        private List<int> _objCompleteList;

        public UnitTimer()
        {
            //
            //TODO:  在此处添加构造函数逻辑
            //
            _objStopwatchs = new Stopwatch[12];
            _objTimers = new ITimer[12];
            _intMinRange = 0;
            _intMaxRange = 0;
            _objCompleteList = new List<int>();
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性: 
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestTimer()
        {
            //初始化实例
            for (int cnt = 0; cnt < 12; cnt++)
            {
                _objStopwatchs[cnt] = new Stopwatch();
            }
            _objTimers[0] = TimerHelper.CreateTimer(TimerKind.SystemTimer);
            _objTimers[1] = TimerHelper.CreateTimer(TimerKind.WinFormTimer);
            _objTimers[2] = TimerHelper.CreateTimer(TimerKind.DispatcherTimer);
            _objTimers[3] = TimerHelper.CreateTimer(TimerKind.ThreadTimer);
            _objTimers[4] = TimerHelper.CreateTimer(TimerKind.SleepTimer);
            _objTimers[5] = TimerHelper.CreateTimer(TimerKind.WaitHandleTimer);
            _objTimers[6] = TimerHelper.CreateTimer(TimerKind.StopwatchTimer);
            _objTimers[7] = TimerHelper.CreateTimer(TimerKind.SocketPollTimer);
            _objTimers[8] = TimerHelper.CreateTimer(TimerKind.EnvironmentTickCountTimer);
            _objTimers[9] = TimerHelper.CreateTimer(TimerKind.DateTimeTickCountTimer);
            _objTimers[10] = TimerHelper.CreateTimer(TimerKind.WinmmTimer);
            _objTimers[11] = TimerHelper.CreateTimer(TimerKind.QueryPerformanceTimer);
            //注册事件
            _objTimers[0].Tick += UnitTimer_Tick0;
            _objTimers[1].Tick += UnitTimer_Tick1;
            _objTimers[2].Tick += UnitTimer_Tick2;
            _objTimers[3].Tick += UnitTimer_Tick3;
            _objTimers[4].Tick += UnitTimer_Tick4;
            _objTimers[5].Tick += UnitTimer_Tick5;
            _objTimers[6].Tick += UnitTimer_Tick6;
            _objTimers[7].Tick += UnitTimer_Tick7;
            _objTimers[8].Tick += UnitTimer_Tick8;
            _objTimers[9].Tick += UnitTimer_Tick9;
            _objTimers[10].Tick += UnitTimer_Tick10;
            _objTimers[11].Tick += UnitTimer_Tick11;
            //设置判断范围
            _intMinRange = 480;
            _intMaxRange = 510;
            //启动
            for (int cnt = 0; cnt < 12; cnt++)
            {
                _objTimers[cnt].TimingMode = Mode.OnceOnly;
                _objTimers[cnt].Interval = 500;
                _objTimers[cnt].Start();
                _objStopwatchs[cnt].Start();
            }
            //循环等待结果
            Win32Helper.DelayEx(1500);
            Assert.IsTrue(_objCompleteList.Count == 12);
            //string str = "";
            //foreach (int item in _objCompleteList)
            //{
            //    str += item.ToString() + ",";
            //}
            //Assert.Fail("完成定时任务的计时器数量:" + str);
        }

        private void UnitTimer_Tick0(object sender, EventArgs e)
        {
            _objStopwatchs[0].Stop();
            bool IsInRange = _objStopwatchs[0].ElapsedMilliseconds > _intMinRange && _objStopwatchs[0].ElapsedMilliseconds < _intMaxRange;
            if (IsInRange)
                _objCompleteList.Add(0);
        }

        private void UnitTimer_Tick1(object sender, EventArgs e)
        {
            _objStopwatchs[1].Stop();
            bool IsInRange = _objStopwatchs[1].ElapsedMilliseconds > _intMinRange && _objStopwatchs[1].ElapsedMilliseconds < _intMaxRange;
            if (IsInRange)
                _objCompleteList.Add(1);
        }

        private void UnitTimer_Tick2(object sender, EventArgs e)
        {
            _objStopwatchs[2].Stop();
            bool IsInRange = _objStopwatchs[2].ElapsedMilliseconds > _intMinRange && _objStopwatchs[2].ElapsedMilliseconds < _intMaxRange;
            if (IsInRange)
                _objCompleteList.Add(2);
        }

        private void UnitTimer_Tick3(object sender, EventArgs e)
        {
            _objStopwatchs[3].Stop();
            bool IsInRange = _objStopwatchs[3].ElapsedMilliseconds > _intMinRange && _objStopwatchs[3].ElapsedMilliseconds < _intMaxRange;
            if (IsInRange)
                _objCompleteList.Add(3);
        }

        private void UnitTimer_Tick4(object sender, EventArgs e)
        {
            _objStopwatchs[4].Stop();
            bool IsInRange = _objStopwatchs[4].ElapsedMilliseconds > _intMinRange && _objStopwatchs[4].ElapsedMilliseconds < _intMaxRange;
            if (IsInRange)
                _objCompleteList.Add(4);
        }

        private void UnitTimer_Tick5(object sender, EventArgs e)
        {
            _objStopwatchs[5].Stop();
            bool IsInRange = _objStopwatchs[5].ElapsedMilliseconds > _intMinRange && _objStopwatchs[5].ElapsedMilliseconds < _intMaxRange;
            if (IsInRange)
                _objCompleteList.Add(5);
        }

        private void UnitTimer_Tick6(object sender, EventArgs e)
        {
            _objStopwatchs[6].Stop();
            bool IsInRange = _objStopwatchs[6].ElapsedMilliseconds > _intMinRange && _objStopwatchs[6].ElapsedMilliseconds < _intMaxRange;
            if (IsInRange)
                _objCompleteList.Add(6);
        }

        private void UnitTimer_Tick7(object sender, EventArgs e)
        {
            _objStopwatchs[7].Stop();
            bool IsInRange = _objStopwatchs[7].ElapsedMilliseconds > _intMinRange && _objStopwatchs[7].ElapsedMilliseconds < _intMaxRange;
            if (IsInRange)
                _objCompleteList.Add(7);
        }

        private void UnitTimer_Tick8(object sender, EventArgs e)
        {
            _objStopwatchs[8].Stop();
            bool IsInRange = _objStopwatchs[8].ElapsedMilliseconds > _intMinRange && _objStopwatchs[8].ElapsedMilliseconds < _intMaxRange;
            if (IsInRange)
                _objCompleteList.Add(8);
        }

        private void UnitTimer_Tick9(object sender, EventArgs e)
        {
            _objStopwatchs[9].Stop();
            bool IsInRange = _objStopwatchs[9].ElapsedMilliseconds > _intMinRange && _objStopwatchs[9].ElapsedMilliseconds < _intMaxRange;
            if (IsInRange)
                _objCompleteList.Add(9);
        }

        private void UnitTimer_Tick10(object sender, EventArgs e)
        {
            _objStopwatchs[10].Stop();
            bool IsInRange = _objStopwatchs[10].ElapsedMilliseconds > _intMinRange && _objStopwatchs[10].ElapsedMilliseconds < _intMaxRange;
            if (IsInRange)
                _objCompleteList.Add(10);
        }

        private void UnitTimer_Tick11(object sender, EventArgs e)
        {
            _objStopwatchs[11].Stop();
            bool IsInRange = _objStopwatchs[11].ElapsedMilliseconds > _intMinRange && _objStopwatchs[11].ElapsedMilliseconds < _intMaxRange;
            if (IsInRange)
                _objCompleteList.Add(11);
        }
    }
}
