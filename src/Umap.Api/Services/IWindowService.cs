using static Umap.Api.Services.impl.WindowService;

namespace Umap.Api.Services
{
    public interface IWindowService
    {
        ScreenCoordinates GetWindowCoordinates();
    }
}
