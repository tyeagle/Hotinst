using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace HOTINST.COMMON.Controls.Win32
{
	/// <summary>
	/// 
	/// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct RECT
    {
		/// <summary>
		/// 
		/// </summary>
        public int left;
		/// <summary>
		/// 
		/// </summary>
        public int top;
		/// <summary>
		/// 
		/// </summary>
        public int right;
		/// <summary>
		/// 
		/// </summary>
        public int bottom;

		/// <summary>
		/// 
		/// </summary>
        public static readonly RECT Empty;

		/// <summary>
		/// 
		/// </summary>
        public int Width
        {
            get { return System.Math.Abs(right - left); }  // Abs needed for BIDI OS
        }

		/// <summary>
		/// 
		/// </summary>
        public int Height
        {
            get { return bottom - top; }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="left"></param>
		/// <param name="top"></param>
		/// <param name="right"></param>
		/// <param name="bottom"></param>
        public RECT(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rcSrc"></param>
        public RECT(RECT rcSrc)
        {
            left = rcSrc.left;
            top = rcSrc.top;
            right = rcSrc.right;
            bottom = rcSrc.bottom;
        }

		/// <summary>
		/// 
		/// </summary>
        public bool IsEmpty
        {
            get
            {
                // BUGBUG : On Bidi OS (hebrew arabic) left > right
                return left >= right || top >= bottom;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        public override string ToString()
        {
            if (this == Empty) 
                return "RECT {Empty}";
            return "RECT { left : " + left + " / top : " + top + " / right : " + right + " / bottom : " + bottom + " }";
        }

        /// <summary> Determine if 2 RECT are equal (deep compare) </summary>
        public override bool Equals(object obj)
        {
			if(!(obj is Rect))
				return false;

	        // ReSharper disable once PossibleInvalidCastException
            return this == (RECT)obj;
        }

        /// <summary>Return the HashCode for this struct (not garanteed to be unique)</summary>
        public override int GetHashCode()
        {
            return left.GetHashCode() + top.GetHashCode() + right.GetHashCode() + bottom.GetHashCode();
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rect1"></param>
		/// <param name="rect2"></param>
		/// <returns></returns>
        public static bool operator ==(RECT rect1, RECT rect2)
        {
            return (rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom);
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rect1"></param>
		/// <param name="rect2"></param>
		/// <returns></returns>
        public static bool operator !=(RECT rect1, RECT rect2)
        {
            return !(rect1 == rect2);
        }


    }
}