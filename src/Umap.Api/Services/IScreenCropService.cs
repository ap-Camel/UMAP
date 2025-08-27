using System.Drawing;
using Umap.Api.Models;
using static Umap.Api.Services.impl.WindowService;

namespace Umap.Api.Services
{
    public interface IScreenCropService
    {
        Bitmap CropMainGameScreen(Bitmap bitmap, ScreenCoordinates window);
        Bitmap CropLastAdded(Bitmap bitmap, ScreenCoordinates window);
        Bitmap CropRectangle(Rectangle rectangle);
        Bitmap CropRegion(Bitmap image, RegionInfo region);
    }
}
