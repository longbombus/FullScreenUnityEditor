#if UNITY_EDITOR_WIN

using System.Runtime.InteropServices;
using UnityEngine;

namespace WindowMaximizer.Editor
{
	internal class Window
	{
		public static Window Current => new Window { hWnd = GetActiveWindow() };
		
		private System.IntPtr hWnd;

		public bool IsFullScreen
		{
			get => (Style & WS.BORDER) == 0;
			set => Style = value
				? Style & ~(WS.BORDER | WS.POPUP | WS.THICKFRAME)
				: Style | WS.BORDER | WS.POPUP | WS.THICKFRAME;
		}

		public bool IsOverlay
			=> IsFullScreen;

		public void FitScreen()
		{
			var monitorInfo = new MonitorInfo();
			unsafe { monitorInfo.cbSize = (uint) sizeof(MonitorInfo); };

			var hMonitor = MonitorFromWindow(hWnd, (uint)Monitor.DEFAULTTONEAREST);
			GetMonitorInfoA(hMonitor, ref monitorInfo);

			var fitRect = IsOverlay ? monitorInfo.rcMonitor : monitorInfo.rcWork;
			
			if (IsFullScreen)
			{
				var borderSize = new Vector2Int(
					GetSystemMetrics((int)SM.CXFIXEDFRAME),
					GetSystemMetrics((int)SM.CYFIXEDFRAME)
				);
				fitRect.left -= borderSize.x;
				fitRect.right += borderSize.x;
				fitRect.top -= borderSize.y;
				fitRect.bottom += borderSize.y;
			}

			SetWindowPos(
				hWnd,
				IsOverlay ? new System.IntPtr(-1) : new System.IntPtr(-2),
				fitRect.left, fitRect.top,
				fitRect.right - fitRect.left, fitRect.bottom - fitRect.top,
				(uint)SWP.DRAWFRAME
			);
		}

		private WS Style
		{
			get => (WS) GetWindowLongPtrA(hWnd, -16);
			set => SetWindowLongPtrA(hWnd, -16, (long) value);
		}

		#region WinApi
		
		private const string dllUser32 = "user32.dll";
		[DllImport(dllUser32)] private static extern System.IntPtr GetActiveWindow();
		[DllImport(dllUser32)] private static extern System.IntPtr MonitorFromWindow(System.IntPtr hWnd, uint dwFlags);
		[DllImport(dllUser32)] private static extern bool GetMonitorInfoA(System.IntPtr hMonitor, ref MonitorInfo lpmi);
		[DllImport(dllUser32)] private static extern bool SetWindowPos(System.IntPtr hWnd, System.IntPtr hWndInsertAfter, int  X, int  Y, int  cx, int  cy, uint uFlags);
		[DllImport(dllUser32)] private static extern int GetSystemMetrics(int nIndex);
		[DllImport(dllUser32)] private static extern long GetWindowLongPtrA(System.IntPtr hWnd, int index);
		[DllImport(dllUser32)] private static extern long SetWindowLongPtrA(System.IntPtr hWnd, int index, long newValue);

		[System.Flags]
		private enum WS : long
		{
			BORDER = 0x00800000L,
			POPUP = 0x80000000L,
			THICKFRAME = 0x00040000L,
		}

		[System.Flags]
		private enum SWP : uint
		{
			DRAWFRAME = 0x0020,
		}


		private enum SM
		{
			CXFIXEDFRAME = 7,
			CYFIXEDFRAME = 8,
		}

		private enum Monitor
		{
			DEFAULTTONULL = 0x00000000,
			DEFAULTTOPRIMARY = 0x00000001,
			DEFAULTTONEAREST = 0x00000002,
		}
		
		[StructLayout(LayoutKind.Sequential)]
		private struct Rect
		{
			public int left;
			public int top;
			public int right;
			public int bottom;
		}
		
		[StructLayout(LayoutKind.Sequential)]
		private struct MonitorInfo {
			public uint cbSize;
			public Rect  rcMonitor;
			public Rect  rcWork;
			public uint dwFlags;
		}
		
		#endregion
	}
}
#endif
