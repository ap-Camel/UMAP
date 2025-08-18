using System.Drawing;

namespace Umap.Api.Services
{
    public interface IPaddleOcr
    {
        string ReadTextFromImage(Bitmap image);
    }
}
