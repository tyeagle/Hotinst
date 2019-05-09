/**********************************************************************
 * 类 名 称：Win32API
 * 命名空间：HOTINST.COMMON.Win32API
 * 文 件 名：Win32API.cs
 * 创建时间：2016-4-12
 * 作    者：汪锋
 * 说    明：Win32API各种函数定义
 * 待 补 充: 
 * 修改时间：
 * 修改历史：
 *          2016-9-27 汪锋 修复其他同事加入RtlCompareMemory时的错误
 *          2016-6-24 汪锋 增加GetWindowLong的API
 **********************************************************************/

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace HOTINST.COMMON.Win32
{
	public class Win32API
	{
		#region .ctor()
		// No need to construct this object

		private Win32API()
		{
		}

		#endregion
		
		#region Constans values

		public const string TOOLBARCLASSNAME = "ToolbarWindow32";
		public const string REBARCLASSNAME = "ReBarWindow32";
		public const string PROGRESSBARCLASSNAME = "msctls_progress32";
		public const string SCROLLBAR = "SCROLLBAR";

		#endregion

		#region CallBacks

		public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        #endregion

        #region advapi32.dll

	    [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
	    public static extern bool LogonUser(string userName, string domain, string password, int logonType,
	        int logonPrivider, ref IntPtr token);

	    [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
	    public static extern bool ImpersonateLoggedOnUser(IntPtr token);

	    [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
	    public static extern bool RevertToSelf();
        #endregion


        #region Kernel32.dll functions

        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
	    public static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern void Sleep(int dwMilliseconds);

        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern int GetTickCount();

        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern bool Beep(int frequency, int duration);

		[DllImport("kernel32.dll", ExactSpelling=true, CharSet=CharSet.Auto)]
		public static extern int GetCurrentThreadId();

        [DllImport("kernel32", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern void GlobalMemoryStatus(ref MEMORY_INFO meminfo);

        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern int GetVolumeInformation(string lpRootPathName, string lpVolumeNameBuffer, int nVolumeNameSize, ref int lpVolumeSerialNumber, int lpMaximumComponentLength, int lpFileSystemFlags, string lpFileSystemNameBuffer, int nFileSystemNameSize);

        [DllImport("Kernel32.dll")]
        public static extern bool SetSystemTime(ref SystemTime sysTime);

        [DllImport("Kernel32.dll")]
        public static extern bool SetLocalTime(ref SystemTime sysTime);

        [DllImport("Kernel32.dll")]
        public static extern void GetSystemTime(ref SystemTime sysTime);

        [DllImport("Kernel32.dll")]
        public static extern void GetLocalTime(ref SystemTime sysTime);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetDiskFreeSpace([MarshalAs(UnmanagedType.LPTStr)]string rootPathName, ref int sectorsPerCluster, ref int bytesPerSector, ref int numberOfFreeClusters, ref int totalNumbeOfClusters);

        [DllImport("kernel32.dll")]
	    public extern static IntPtr LoadLibrary(string path);

	    [DllImport("kernel32.dll")]
	    public extern static IntPtr GetProcAddress(IntPtr lib, string funcName);

	    [DllImport("kernel32.dll")]
        public extern static bool FreeLibrary(IntPtr lib);

        #endregion

        #region Gdi32.dll functions

        [DllImport("gdi32.dll")]
        public static extern bool StretchBlt(IntPtr hDCDest, int XOriginDest, int YOriginDest, int WidthDest, int HeightDest, IntPtr hDCSrc, int XOriginScr, int YOriginSrc, int WidthScr, int HeightScr, uint Rop);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int Width, int Heigth);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hDCDest, int XOriginDest, int YOriginDest, int WidthDest, int HeightDest, IntPtr hDCSrc, int XOriginScr, int YOriginSrc, uint Rop);

        [DllImport("gdi32.dll")]
        public static extern IntPtr DeleteDC(IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern bool PatBlt(IntPtr hDC, int XLeft, int YLeft, int Width, int Height, uint Rop);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        public static extern uint GetPixel(IntPtr hDC, int XPos, int YPos);

        [DllImport("gdi32.dll")]
        public static extern int SetMapMode(IntPtr hDC, int fnMapMode);

        [DllImport("gdi32.dll")]
        public static extern int GetObjectType(IntPtr handle);

        [DllImport("gdi32")]
        public static extern IntPtr CreateDIBSection(IntPtr hdc, ref BITMAPINFO_FLAT bmi, int iUsage, ref int ppvBits, IntPtr hSection, int dwOffset);

        [DllImport("gdi32")]
        public static extern int GetDIBits(IntPtr hDC, IntPtr hbm, int StartScan, int ScanLines, int lpBits, BITMAPINFOHEADER bmi, int usage);

        [DllImport("gdi32")]
        public static extern int GetDIBits(IntPtr hdc, IntPtr hbm, int StartScan, int ScanLines, int lpBits, ref BITMAPINFO_FLAT bmi, int usage);

        [DllImport("gdi32")]
        public static extern IntPtr GetPaletteEntries(IntPtr hpal, int iStartIndex, int nEntries, byte[] lppe);

        [DllImport("gdi32")]
        public static extern IntPtr GetSystemPaletteEntries(IntPtr hdc, int iStartIndex, int nEntries, byte[] lppe);

        [DllImport("gdi32")]
        public static extern uint SetDCBrushColor(IntPtr hdc,  uint crColor);

        [DllImport("gdi32")]
        public static extern IntPtr CreateSolidBrush(uint crColor);

        [DllImport("gdi32")]
        public static extern int SetBkMode(IntPtr hDC, BackgroundMode mode);

        [DllImport("gdi32")]
        public static extern int SetViewportOrgEx(IntPtr hdc,  int x, int y,  int param);

        [DllImport("gdi32")]
        public static extern uint SetTextColor(IntPtr hDC, uint colorRef);

        [DllImport("gdi32")]
        public static extern int SetStretchBltMode(IntPtr hDC, int StrechMode);

        #endregion

		#region Uxtheme.dll functions

		[DllImport("uxtheme.dll")]
		public static extern int SetWindowTheme(IntPtr hWnd, string AppID, string ClassID);

		#endregion
	
		#region User32.dll functions

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool MessageBeep(BeepType beepType);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern bool ShowWindow(IntPtr hWnd, short State);

		///<summary>
		/// 该函数设置由不同线程产生的窗口的显示状态
		/// </summary>
		/// <param name="hWnd">窗口句柄</param>
		/// <param name="cmdShow">指定窗口如何显示。查看允许值列表，请查阅ShowWlndow函数的说明部分</param>
		/// <returns>如果函数原来可见，返回值为非零；如果函数原来被隐藏，返回值为零</returns>
		[DllImport("user32.dll")]
		public static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

		[DllImport("user32.dll")]
		public static extern bool EnableWindow(IntPtr hWnd, bool bEnable);

		[DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern bool UpdateWindow(IntPtr hWnd);

		[DllImport("user32", CharSet = CharSet.Unicode)]
		public static extern IntPtr FindWindow(string cls, string win);
		
		[DllImport("user32")]
		public static extern bool IsIconic(IntPtr hWnd);

		[DllImport("user32")]
		public static extern bool OpenIcon(IntPtr hWnd);

		[DllImport("user32.dll")]
		public static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int Width, int Height, int flags);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern bool CloseClipboard();

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern bool EmptyClipboard();

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SetClipboardData( uint Format, IntPtr hData);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern bool GetMenuItemRect(IntPtr hWnd, IntPtr hMenu, uint Item, ref RECT rc);

        [DllImport("user32.dll", ExactSpelling=true, CharSet=CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern void SendMessage(IntPtr hWnd, int msg, int wParam, ref RECT lParam);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, ref POINT lParam);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern void SendMessage(IntPtr hWnd, int msg, int wParam, ref TBBUTTON lParam);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern void SendMessage(IntPtr hWnd, int msg, int wParam, ref TBBUTTONINFO lParam);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, ref REBARBANDINFO lParam);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern void SendMessage(IntPtr hWnd, int msg, int wParam, ref TVITEM lParam);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern void SendMessage(IntPtr hWnd, int msg, int wParam, ref LVITEM lParam);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern void SendMessage(IntPtr hWnd, int msg, int wParam, ref HDITEM lParam);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern void SendMessage(IntPtr hWnd, int msg, int wParam, ref HD_HITTESTINFO hti);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SetWindowsHookEx(int hookid, HookProc pfnhook, IntPtr hinst, int threadid);

        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhook);

        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhook, int code, IntPtr wparam, IntPtr lparam);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern int DrawText(IntPtr hdc, string lpString, int nCount, ref RECT lpRect, int uFormat);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SetParent(IntPtr hChild, IntPtr hParent);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr GetDlgItem(IntPtr hDlg, int nControlID);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern int GetClientRect(IntPtr hWnd, ref RECT rc);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern int InvalidateRect(IntPtr hWnd,  IntPtr rect, int bErase);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool WaitMessage();

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool PeekMessage(ref MSG msg, int hWnd, uint wFilterMin, uint wFilterMax, uint wFlag);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool GetMessage(ref MSG msg, int hWnd, uint wFilterMin, uint wFilterMax);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool TranslateMessage(ref MSG msg);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool DispatchMessage(ref MSG msg);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr LoadCursor(IntPtr hInstance, uint cursor);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SetCursor(IntPtr hCursor);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr GetFocus();

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool ReleaseCapture();

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr BeginPaint(IntPtr hWnd, ref PAINTSTRUCT ps);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT ps);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref POINT pptDst, ref SIZE psize, IntPtr hdcSrc, ref POINT pprSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT rect);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool ClientToScreen(IntPtr hWnd, ref POINT pt);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool TrackMouseEvent(ref TRACKMOUSEEVENTS tme);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool redraw);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern ushort GetKeyState(int virtKey);

        [DllImport("User32", CharSet = CharSet.Auto)]
        public static extern ushort GetAsyncKeyState(int virtKey);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int width, int height, bool repaint);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd,  out STRINGBUFFER ClassName, int nMaxCount);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr GetDCEx(IntPtr hWnd, IntPtr hRegion, uint flags);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int FillRect(IntPtr hDC, ref RECT rect, IntPtr hBrush);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT wp);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int SetWindowText(IntPtr hWnd, string text);

        [DllImport("User32.dll", CharSet=CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, out STRINGBUFFER text, int maxCount);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam); 

        [DllImport("user32.dll", CharSet=CharSet.Auto)] 
        public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer); 

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern int ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern int GetSystemMetrics(int nIndex);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern int SetScrollInfo(IntPtr hwnd,  int bar, ref SCROLLINFO si, int fRedraw);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern int ShowScrollBar(IntPtr hWnd, int bar,  int show);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern int EnableScrollBar(IntPtr hWnd, uint flags, uint arrows);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern int BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern int GetScrollInfo(IntPtr hwnd, int bar, ref SCROLLINFO si);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int ScrollWindowEx(IntPtr hWnd, int dx, int dy, ref RECT rcScroll, ref RECT rcClip, IntPtr UpdateRegion, ref RECT rcInvalidated, uint flags);

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern int IsWindow(IntPtr hWnd);

        [DllImport("user32",CharSet=CharSet.Auto)] 
        public static extern int GetKeyboardState(byte[] pbKeyState);

		[DllImport("user32.dll")]
		public static extern bool GetCursorPos(out POINT lpPoint);

		[DllImport("gdi32.dll")]
		public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

		[DllImport("gdi32.dll")]
		public static extern IntPtr GetTopWindow(IntPtr hWnd);

		[DllImport("User32")]
		public static extern IntPtr GetWindow(IntPtr hWnd, uint wCmd);

		[DllImport("user32.dll")]
		public static extern IntPtr GetSystemMenu(IntPtr hwnd, int revert);

		[DllImport("user32.dll")]
		public static extern int GetMenuItemCount(IntPtr hmenu);

		[DllImport("user32.dll")]
		public static extern int RemoveMenu(IntPtr hmenu, int npos, int wflags);

		[DllImport("user32.dll")]
		public static extern int DrawMenuBar(IntPtr hwnd);

		[DllImport("user32.dll")]
		public static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

		[DllImport("user32.dll")]
		public static extern void LockWorkStation();
		
		/// <summary>
		/// To Ascii Code
		/// </summary>
		/// <param name="uVirtKey">[in] Specifies the virtual-key code to be translated.</param>
		/// <param name="uScanCode">[in] Specifies the hardware scan code of the key to be translated. The high-order bit of this value is set if the key is up (not pressed). </param>
		/// <param name="lpbKeyState">[in] Pointer to a 256-byte array that contains the current keyboard state. Each element (byte) in the array contains the state of one key. If the high-order bit of a byte is set, the key is down (pressed). The low bit, if set, indicates that the key is toggled on. In this function, only the toggle bit of the CAPS LOCK key is relevant. The toggle state of the NUM LOCK and SCROLL LOCK keys is ignored.</param>
		/// <param name="lpwTransKey">[out] Pointer to the buffer that receives the translated character or characters. </param>
		/// <param name="fuState">[in] Specifies whether a menu is active. This parameter must be 1 if a menu is active, or 0 otherwise.</param>
		/// <returns></returns>
		[DllImport("user32")]
        public static extern int ToAscii(int uVirtKey, int uScanCode, byte[] lpbKeyState, byte[] lpwTransKey, int fuState);

		#endregion

		#region Common Controls functions

		[DllImport("comctl32.dll")]
        public static extern bool InitCommonControlsEx(INITCOMMONCONTROLSEX icc);

		[DllImport("comctl32.dll")]
        public static extern bool InitCommonControls();

        [DllImport("comctl32.dll", EntryPoint="DllGetVersion")]
        public static extern int GetCommonControlDLLVersion(ref DLLVERSIONINFO dvi);

        [DllImport("comctl32.dll")]
        public static extern IntPtr ImageList_Create(int width, int height, uint flags, int count, int grow);

        [DllImport("comctl32.dll")]
        public static extern bool ImageList_Destroy(IntPtr handle);

        [DllImport("comctl32.dll")]
        public static extern int ImageList_Add(IntPtr imageHandle, IntPtr hBitmap, IntPtr hMask);

        [DllImport("comctl32.dll")]
        public static extern bool ImageList_Remove(IntPtr imageHandle, int index);

        [DllImport("comctl32.dll")]
        public static extern bool ImageList_BeginDrag(IntPtr imageHandle, int imageIndex, int xHotSpot, int yHotSpot);

        [DllImport("comctl32.dll")]
        public static extern bool ImageList_DragEnter(IntPtr hWndLock, int x, int y);

        [DllImport("comctl32.dll")]
        public static extern bool ImageList_DragMove(int x, int y);

        [DllImport("comctl32.dll")]
        public static extern bool ImageList_DragLeave(IntPtr hWndLock);

        [DllImport("comctl32.dll")]
        public static extern void ImageList_EndDrag();

		#endregion

        #region Ntdll.dll functions
        /// <summary>
        /// Windows系统API中的内存比较函数,返回的是两块内存相同的字节数，跟crt中的memcmp是不一要的，要特别注意
        /// </summary>
        /// <param name="Destination"></param>
        /// <param name="Source"></param>
        /// <param name="Length"></param>
        /// <returns>返回相同的字节数，如果与Length相等，则认为两块内在相同</returns>
        [DllImport("ntdll.dll")]
        public static extern int RtlCompareMemory(IntPtr Destination, IntPtr Source, int Length);
        /// <summary>
        /// 比较两个字节数组的内容是否相同，返回值的意义与RtlCompareMemory一致
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="source"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static int RtlCompareMemory(byte[] dest, byte[]source, int length)
        {
            if(dest.Length < length || source.Length < length)
            {
                throw new ArgumentOutOfRangeException($"Param length is larger than array.length");
            }
            IntPtr destPtr = Marshal.AllocHGlobal(length);
            IntPtr srcPtr = Marshal.AllocHGlobal(length);

            Marshal.Copy(dest, 0, destPtr, length);
            Marshal.Copy(source, 0, srcPtr, length);

            int comparResult = RtlCompareMemory(destPtr, srcPtr, length);
            Marshal.FreeHGlobal(destPtr);
            Marshal.FreeHGlobal(srcPtr);
            return comparResult;
        }
        #endregion

        #region PowrProf.dll functions

        [DllImport("PowrProf.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		public static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);

		#endregion

		#region shlwapi.dll functions

		[DllImport("shlwapi.dll")]
		public static extern void StrFormatByteSize64(ulong qdw, StringBuilder builder, uint cchBuf);

		#endregion
	}
}
