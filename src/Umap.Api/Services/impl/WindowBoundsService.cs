using System.Runtime.InteropServices;

namespace Umap.Api.Services.impl
{
    public class WindowBoundsService : IWindowBoundsService
    {
        private const int DWMWA_EXTENDED_FRAME_BOUNDS = 9;

        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmGetWindowAttribute(
            IntPtr hwnd,
            int dwAttribute,
            out RECT pvAttribute,
            int cbAttribute);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left, top, right, bottom;
        }

        public RECT GetVisibleWindowBounds(IntPtr hWnd)
        {
            // Retrieves the bounds *excluding* invisible borders/shadows
            DwmGetWindowAttribute(hWnd,
                                  DWMWA_EXTENDED_FRAME_BOUNDS,
                                  out RECT bounds,
                                  Marshal.SizeOf<RECT>());
            return bounds;
        }
    }
}
