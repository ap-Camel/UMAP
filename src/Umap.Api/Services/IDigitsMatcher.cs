using Emgu.CV;

namespace Umap.Api.Services
{
    public interface IDigitsMatcher
    {
        string MatchDigits(Mat image, int baseWidth = 2342, int baseHight = 1362);
    }
}
