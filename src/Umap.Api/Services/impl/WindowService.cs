using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Umap.Api.Services.impl
{
    public class WindowService : IWindowService
    {
        private readonly IUmaMusumeWindowBringToFrontService _windowBringToFrontService;
        private readonly IWindowBoundsService _windowBoundsService;

        public WindowService(IUmaMusumeWindowBringToFrontService windowBringToFrontService, IWindowBoundsService windowBoundsService)
        {
            _windowBringToFrontService = windowBringToFrontService;
            _windowBoundsService = windowBoundsService;
        }



        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);


        public ScreenCoordinates GetWindowCoordinates()
        {
            Process[] processes = Process.GetProcessesByName("UmamusumePrettyDerby");

            var process = processes.First();
            process.WaitForInputIdle();
            IntPtr hWnd = process.MainWindowHandle;
            if (hWnd == IntPtr.Zero)
            {
                Console.WriteLine("No main window found for this process.");
                throw new Exception("No main window found for this process.");
            }

            _windowBringToFrontService.BringToFront(hWnd);
            Thread.Sleep(200);

            var rect = _windowBoundsService.GetVisibleWindowBounds(hWnd);


            //RECT rect = new RECT();
            //bool success = GetWindowRect(hWnd, ref rect);
            //if (!success)
            //{
            //    Console.WriteLine("GetWindowRect failed: " + Marshal.GetLastWin32Error());
            //    throw new Exception("GetWindowRect failed:");
            //}

            int x = rect.left;
            int y = rect.top;
            int width = rect.right - rect.left;
            int height = rect.bottom - rect.top;

            Console.WriteLine($"Position: ({x}, {y}), Size: {width}×{height}");
            return new ScreenCoordinates
            {
                x = x,
                y = y,
                width = width,
                height = height
            };
        }

        public class ScreenCoordinates
        {
            public int x { get; set; }
            public int y { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }



        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
    }
}
