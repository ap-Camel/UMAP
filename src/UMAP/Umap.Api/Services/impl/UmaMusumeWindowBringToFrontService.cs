using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Umap.Api.Services.impl
{
    public class UmaMusumeWindowBringToFrontService : IUmaMusumeWindowBringToFrontService
    {
        public UmaMusumeWindowBringToFrontService() { }


        private const int SW_RESTORE = 9;

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        public void BringToFront(IntPtr hWnd)
        {
            // 1. Restore if minimized
            ShowWindow(hWnd, SW_RESTORE);

            // 2. Attach input threads (bypass focus rules)
            uint currentThread = GetCurrentThreadId();
            uint targetThread = GetWindowThreadProcessId(hWnd, out _);
            AttachThreadInput(currentThread, targetThread, true);

            // 3. Set as foreground
            SetForegroundWindow(hWnd);

            // 4. Detach threads
            AttachThreadInput(currentThread, targetThread, false);
        }

        public void BringProcessToFront(string processName)
        {
            Process[] procs = Process.GetProcessesByName(processName);
            if (procs.Length == 0) throw new InvalidOperationException("Process not found.");
            BringToFront(procs[0].MainWindowHandle);
        }

    }
}
