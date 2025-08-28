using Microsoft.AspNetCore.Http.Extensions;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Umap.Api.Services.impl
{
    public class ScreenCaptureService : IScreenCaptureService
    {

        public ScreenCaptureService()
        {

        }



        private const int SM_XVIRTUALSCREEN = 76;
        private const int SM_YVIRTUALSCREEN = 77;
        private const int SM_CXVIRTUALSCREEN = 78;
        private const int SM_CYVIRTUALSCREEN = 79;

        private static readonly IntPtr PER_MONITOR_AWARE_V2 = (IntPtr)(-4);

        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);

        [DllImport("user32.dll")]
        private static extern bool SetProcessDpiAwarenessContext(IntPtr dpiContext);

        [DllImport("user32.dll")] private static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("user32.dll")] private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("gdi32.dll")]
        private static extern bool BitBlt(
            IntPtr hDestDC, int x, int y, int width, int height,
            IntPtr hSrcDC, int xSrc, int ySrc, CopyPixelOperation rop);

        public Bitmap CaptureActiveWindow()
        {
            // get a DC for the entire screen (hWnd = IntPtr.Zero)
            //IntPtr hScreenDC = GetDC(IntPtr.Zero);
            // create a compatible DC in memory
            //IntPtr hMemoryDC = Gdi32.CreateCompatibleDC(hScreenDC);

            //var size = SystemInformation.PrimaryMonitorSize;

            //// get screen dimensions
            //int width = Screen.PrimaryScreen.Bounds.Width;
            //int height = Screen.PrimaryScreen.Bounds.Height;

            EnablePerMonitorDpiAwareness();

            int x = GetSystemMetrics(SM_XVIRTUALSCREEN);
            int y = GetSystemMetrics(SM_YVIRTUALSCREEN);
            int width = GetSystemMetrics(SM_CXVIRTUALSCREEN);
            int height = GetSystemMetrics(SM_CYVIRTUALSCREEN);

            //int w = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            //int h = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            // create a compatible bitmap and select it into our in‐memory DC
            IntPtr hScreenDC = GetDC(IntPtr.Zero);
            IntPtr hMemDC = Gdi32.CreateCompatibleDC(hScreenDC);
            IntPtr hBitmap = Gdi32.CreateCompatibleBitmap(hScreenDC, width, height);
            IntPtr hOldBmp = Gdi32.SelectObject(hMemDC, hBitmap);

            // bit‐blt from screen DC to memory DC
            BitBlt(hMemDC, 0, 0, width, height, hScreenDC, x, y, CopyPixelOperation.SourceCopy);

            // 5. Clean up
            Gdi32.SelectObject(hMemDC, hOldBmp);
            Gdi32.DeleteDC(hMemDC);
            ReleaseDC(IntPtr.Zero, hScreenDC);

            // 6. Wrap into .NET Bitmap
            Bitmap bmp = Image.FromHbitmap(hBitmap);
            Gdi32.DeleteObject(hBitmap);

            return bmp;
        }

        public void EnablePerMonitorDpiAwareness()
        {
            // Must be called before any windows are created
            //if (!SetProcessDpiAwarenessContext(PER_MONITOR_AWARE_V2))
            //{
            //    // Check Marshal.GetLastWin32Error() for details if false
            //    throw new InvalidOperationException("Failed to set DPI awareness context");
            //}
        }

        internal static class Gdi32
        {
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

            [DllImport("gdi32.dll")]
            public static extern bool DeleteDC(IntPtr hDC);

            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);

            [DllImport("gdi32.dll")]
            public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr hObject);
        }

    }
}
