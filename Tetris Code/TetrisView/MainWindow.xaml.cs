using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using TetrisLogic;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace Tetris
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class SaveAspectWindow : Window
	{
		/// <summary>
		/// Aspect ratio of the window
		/// </summary>
		private double aspectRatio;

		/// <summary>
		/// Memorized height
		/// </summary>
		private bool? adjustingHeight = null;

		/// <summary>
		/// Window constructor
		/// </summary>
		public SaveAspectWindow()
		{
			InitializeComponent();
			SourceInitialized += Window_SourceInitialized;
			canvas.Child = new GameCanvas();
			Renderer.GraphicsSize = new System.Drawing.Size((int)canvas.RenderSize.Width - 1, (int)canvas.RenderSize.Height - 1);
		}

		/// <summary>
		/// Keyboard interception function
		/// </summary>
		/// <param name="sender">Event sender</param>
		/// <param name="e">Parameters</param>
		private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			GameCanvas.GameCanvas_KeyDown((Keys)KeyInterop.VirtualKeyFromKey(e.Key));
		}

		internal enum WM
		{
			WINDOWPOSCHANGING = 0x0046,
			EXITSIZEMOVE = 0x0232,
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct WINDOWPOS
		{
			public IntPtr hwnd;
			public IntPtr hwndInsertAfter;
			public int x;
			public int y;
			public int cx;
			public int cy;
			public int flags;
		}

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetCursorPos(ref Win32Point pt);

		[StructLayout(LayoutKind.Sequential)]
		internal struct Win32Point
		{
			public int X;
			public int Y;
		};

		public static Point GetMousePosition()
		{
			Win32Point w32Mouse = new Win32Point();
			GetCursorPos(ref w32Mouse);
			return new Point(w32Mouse.X, w32Mouse.Y);
		}

		private void Window_SourceInitialized(object sender, EventArgs ea)
		{
			HwndSource hwndSource = (HwndSource)PresentationSource.FromVisual((Window)sender);
			hwndSource.AddHook(DragHook);
			aspectRatio = Width / Height;
		}

		private IntPtr DragHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			switch ((WM)msg)
			{
				case WM.WINDOWPOSCHANGING:
					{
						WINDOWPOS pos = (WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));

						if ((pos.flags & 0x0002) != 0)
							return IntPtr.Zero;
						Window wnd = (Window)HwndSource.FromHwnd(hwnd).RootVisual;
						if (wnd == null)
							return IntPtr.Zero;
						if (!adjustingHeight.HasValue)
						{
							Point p = GetMousePosition();

							double diffWidth = Math.Min(Math.Abs(p.X - pos.x), Math.Abs(p.X - pos.x - pos.cx));
							double diffHeight = Math.Min(Math.Abs(p.Y - pos.y), Math.Abs(p.Y - pos.y - pos.cy));

							adjustingHeight = diffHeight > diffWidth;
						}
						if (adjustingHeight.Value)
							pos.cy = (int)(pos.cx / aspectRatio);
						else
							pos.cx = (int)(pos.cy * aspectRatio);

						Marshal.StructureToPtr(pos, lParam, true);
						handled = true;
					}
					break;
				case WM.EXITSIZEMOVE:
					adjustingHeight = null;
					break;
			}

			return IntPtr.Zero;
		}
	}
}
