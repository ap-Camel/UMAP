using static Umap.Api.Services.impl.WindowBoundsService;

namespace Umap.Api.Services
{
    public interface IWindowBoundsService
    {
        RECT GetVisibleWindowBounds(IntPtr hWnd);
    }
}
