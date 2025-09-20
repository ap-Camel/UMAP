using Emgu.CV;
using Emgu.CV.Util;

namespace Umap.Api.Services
{
    public interface IStatsValueReader
    {
        string MatchDigits(Mat image);
        VectorOfPoint DigitsFeatureMatching(Mat icon, Mat screenshot, string name);
    }
}
