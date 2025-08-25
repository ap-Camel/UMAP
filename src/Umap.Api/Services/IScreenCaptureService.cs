using System.Drawing;

namespace Umap.Api.Services
{
    public interface IScreenCaptureService
    {
        public Bitmap CaptureActiveWindow();
    }
}
