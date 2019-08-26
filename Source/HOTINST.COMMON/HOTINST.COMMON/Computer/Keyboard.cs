using System;
using System.Threading;
using HOTINST.COMMON.Win32;

namespace HOTINST.COMMON.Computer
{
    public static partial class MyComputer
    {
        /// <summary>
        /// 键盘静态类
        /// </summary>
        /// <remarks>可以获取键盘状态及执行键盘有关操作</remarks>
        public static class Keyboard
        {
            /// <summary>
            /// 获取Caps Lock状态。
            /// </summary>
            /// <returns>true开启，false关闭</returns>
            public static bool CapsLock()
            {
                return Win32Helper.CapsLock();
            }

            /// <summary>
            /// 获取Num Lock状态。
            /// </summary>
            /// <returns>true开启，false关闭</returns>
            public static bool NumLock()
            {
                return Win32Helper.NumLock();
            }

            /// <summary>
            /// 获取Scroll Lock状态。
            /// </summary>
            /// <returns>true开启，false关闭</returns>
            public static bool ScrollLock()
            {
                return Win32Helper.ScrollLock();
            }

            /// <summary>
            /// 获取ALT键是否按下状态。
            /// </summary>
            /// <returns>true已按下，false未按下</returns>
            public static bool AltKeyDown()
            {
                return Win32Helper.AltKeyDown();
            }

            /// <summary>
            /// 获取CTRL键是否按下状态。
            /// </summary>
            /// <returns>true已按下，false未按下</returns>
            public static bool CtrlKeyDown()
            {
                return Win32Helper.CtrlKeyDown();
            }

            /// <summary>
            /// 获取SHIFT键是否按下状态。
            /// </summary>
            /// <returns>true已按下，false未按下</returns>
            public static bool ShiftKeyDown()
            {
                return Win32Helper.ShiftKeyDown();
            }

            /// <summary>
            /// 向活动窗口发送一个或多个击键，就像在键盘键入一样
            /// </summary>
            /// <param name="keys">要发送的键</param>
            /// <param name="flush">处理消息队列中当前的所有Windows消息</param>
            /// <remarks>使用 Flush 等待应用程序处理键盘敲击以及消息队
            /// 列中的其他操作系统消息。 在处理完所有键操作前，这与调
            /// 用 Application.DoEvents 等同。</remarks>
            public static void SendKeys(string keys, bool flush)
            {
                System.Windows.Forms.SendKeys.Send(keys);
                if (flush)
                {
                    Thread.Sleep(100);
                    System.Windows.Forms.SendKeys.Flush();
                }
            }

            /// <summary>
            /// 向活动窗口发送一个或多个击键，就像在键盘键入一样
            /// </summary>
            /// <param name="wait">是否等待击键处理完成。</param>
            /// <param name="keys">要发送的键</param>
            /// <remarks>使用Wait可以将键击或组合键击发送到活动应用程序，并
            /// 等待处理键击消息。 可以用此方法将键击发送到一个应用程序，并
            /// 等待完成由这些键击所启动的任何进程。 如果必须在其他应用程序
            /// 完成之后才能继续您的应用程序的话，那么这一点很重要。</remarks>
            public static void SendKeys(bool wait,string keys)
            {
                if (wait)
                    System.Windows.Forms.SendKeys.SendWait(keys);
                else
                    System.Windows.Forms.SendKeys.Send(keys);
            }
        }
    }
}
