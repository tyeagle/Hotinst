/**
 * ==============================================================================
 *
 * ClassName: CursorHandler
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/3/23 14:18:35
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

using System.Runtime.InteropServices;

namespace HOTINST.COMMON.Controls.Core.Editors
{
	/// <summary>
	/// 
	/// </summary>
	public class CursorHandler
	{
		/// <summary>
		/// 
		/// </summary>
		public struct POINT
		{
			/// <summary>
			/// 
			/// </summary>
			public int X;
			/// <summary>
			/// 
			/// </summary>
			public int Y;

			/// <summary>
			/// 
			/// </summary>
			/// <param name="x"></param>
			/// <param name="y"></param>
			public POINT(int x, int y)
			{
				X = x;
				Y = y;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="lpPoint"></param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetCursorPos(out POINT lpPoint);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		[DllImport("User32.dll")]
		public static extern bool SetCursorPos(int x, int y);
	}
}