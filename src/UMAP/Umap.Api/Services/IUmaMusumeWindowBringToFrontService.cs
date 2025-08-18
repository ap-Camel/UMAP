namespace Umap.Api.Services
{
    public interface IUmaMusumeWindowBringToFrontService
    {
        void BringToFront(IntPtr hWnd);
        void BringProcessToFront(string processName);
    }
}
