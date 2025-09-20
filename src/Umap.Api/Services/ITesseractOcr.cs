using System.Drawing;

namespace Umap.Api.Services
{
    public interface ITesseractOcr
    {
        string ReadTextFromImage(Bitmap image);
        string ReadNumbersAndSignsFromImage(Bitmap image);
    }
}
