#if UNITY_EDITOR_WIN

using System.Runtime.InteropServices;

namespace EditorWindowMaximizer.Editor
{
	internal class Window
	{
		public static Window Current => new Window { hWnd = GetActiveWindow() };
		
		private System.IntPtr hWnd;

		public bool IsFullScreen
		{
			get => (Style & WS.POPUP) != 0;
			set
			{
				Style = value
					? (Style & ~(WS.BORDER | WS.DLGFRAME | WS.THICKFRAME)) | WS.POPUP
					: (Style & ~WS.POPUP) | WS.BORDER | WS.DLGFRAME | WS.THICKFRAME;
				ExStyle = value
					? ExStyle & ~(WS_EX.WINDOWEDGE | WS_EX.DLGMODALFRAME | WS_EX.CLIENTEDGE | WS_EX.STATICEDGE)
					: ExStyle | WS_EX.WINDOWEDGE;
			}
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

			SetWindowPos(
				hWnd,
				IsOverlay ? new System.IntPtr(-1) : new System.IntPtr(-2),
				fitRect.left, fitRect.top,
				fitRect.right - fitRect.left, fitRect.bottom - fitRect.top,
				(uint)SWP.FRAMECHANGED
			);
		}

		private WS Style
		{
			get => (WS) GetWindowLongPtrA(hWnd, -16);
			set => SetWindowLongPtrA(hWnd, -16, (long) value);
		}

		private WS_EX ExStyle
		{
			get => (WS_EX) GetWindowLongPtrA(hWnd, -20);
			set => SetWindowLongPtrA(hWnd, -20, (long) value);
		}

		#region WinApi
		
		private const string dllUser32 = "user32.dll";
		[DllImport(dllUser32)] private static extern System.IntPtr GetActiveWindow();
		[DllImport(dllUser32)] private static extern System.IntPtr MonitorFromWindow(System.IntPtr hWnd, uint dwFlags);
		[DllImport(dllUser32)] private static extern bool GetMonitorInfoA(System.IntPtr hMonitor, ref MonitorInfo lpmi);
		[DllImport(dllUser32)] private static extern bool SetWindowPos(System.IntPtr hWnd, System.IntPtr hWndInsertAfter, int  X, int  Y, int  cx, int  cy, uint uFlags);
		[DllImport(dllUser32)] private static extern long GetWindowLongPtrA(System.IntPtr hWnd, int index);
		[DllImport(dllUser32)] private static extern long SetWindowLongPtrA(System.IntPtr hWnd, int index, long newValue);

		[System.Flags]
		private enum WS_EX : long
		{
			DLGMODALFRAME = 0x00000001L,
			WINDOWEDGE    = 0x00000100L,
			CLIENTEDGE    = 0x00000200L,
			STATICEDGE    = 0x00020000L,
		}

		[System.Flags]
		private enum WS : long
		{
			BORDER     = 0x00800000L,
			DLGFRAME   = 0x00400000L,
			POPUP      = 0x80000000L,
			THICKFRAME = 0x00040000L,
		}

		private enum SWP : uint
		{
			FRAMECHANGED = 0x0020,
		}

		private enum Monitor
		{
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
