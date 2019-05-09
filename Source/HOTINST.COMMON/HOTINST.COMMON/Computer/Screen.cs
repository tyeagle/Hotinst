using System.Windows.Forms;

namespace HOTINST.COMMON.Computer
{
    public static partial class MyComputer
    {
        /// <summary>
        /// 屏幕静态类
        /// </summary>
        public static class Screen
        {
            /// <summary>
            /// 获取本机显示器数量
            /// </summary>
            /// <returns>返回本机显示器数量</returns>
            public static int GetScreenCount()
            {
                return System.Windows.Forms.Screen.AllScreens.Length;
            }

            /// <summary>
            /// 将窗体显示在第二个屏幕中央
            /// </summary>
            /// <param name="objForm">System.Windows.Forms.Form窗体对象</param>
            /// <remarks>如果不存在第二个屏幕，则不会执行</remarks>
            public static void ShowFormAtSecondScreen(ref Form objForm)
            {
                if (System.Windows.Forms.Screen.AllScreens.Length < 2)
                    return;

                objForm.Left = (System.Windows.Forms.Screen.AllScreens[1].Bounds.Width - objForm.Width) / 2;
                objForm.Top = (System.Windows.Forms.Screen.AllScreens[1].Bounds.Height - objForm.Height) / 2;
            }
        }
    }

}
