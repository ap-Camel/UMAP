using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Drawing;
using System.Drawing.Imaging;

namespace Umap.Api.Services.impl
{
    public class StatsValueReader : IStatsValueReader
    {
        public string MatchDigits(Mat image)
        {
            CvInvoke.Imshow("image", image);
            CvInvoke.WaitKey();

            var digits = GetDigits();
            var results = new Dictionary<int, string>();
            var temp = new OrderedDictionary<int, string>();
            foreach (var digit in digits)
            {
                var result = DigitsFeatureMatching(digit.Value.ToMat(), image, digit.Key);
                if (result == null || result.Length == 0)
                    continue;

                var small = GetSmallestX(result);
                temp.Add(small, digit.Key);
            }

            var final = string.Empty;
            foreach (var item in temp)
                final += item.Value;

            return final;
        }


        public int GetSmallestX(VectorOfPoint vector)
        {
            var xPoint = vector[0].X;
            for (int i = 0; i < Math.Floor((vector.Length / 8) * 1.0); i++)
            {
                var point = vector[i];

                if (point.X < xPoint)
                    xPoint = point.X;
            }

            return xPoint;
        }

        public VectorOfPoint DigitsFeatureMatching(Mat icon, Mat screenshot, string name)
        {
            CvInvoke.Resize(icon, icon, new Size(0, 0), 2.35d, 2.35d);
            //CvInvoke.Resize(screenshot, screenshot, new Size(0, 0), 0.45d, 0.45d);
            //CvInvoke.CvtColor(screenshot, screenshot, ColorConversion.Bgr2Gray);
            //CvInvoke.Canny(screenshot, screenshot, 150, 50);
            //CvInvoke.Resize(icon, icon, new Size(0, 0), 0.45d, 0.45d);

            var vectorOfPoint = new VectorOfPoint();

            Mat homography = null;
            var mask = new Mat();

            var screenshotDescriptor = new Mat();
            var croppedDescriptor = new Mat();

            var screenshotKeyPoints = new VectorOfKeyPoint();
            var croppedKeyPoints = new VectorOfKeyPoint();

            int k = 2;
            double uniqueThreshold = 0.75;

            var matches = new VectorOfVectorOfDMatch();

            var featurDetector = new Brisk();
            var orb = new ORB(2000, 1.2f, 8, 15, 0, 2, ORB.ScoreType.Harris, 31, 8);

            orb.DetectAndCompute(icon, null, screenshotKeyPoints, screenshotDescriptor, false);
            orb.DetectAndCompute(screenshot, null, croppedKeyPoints, croppedDescriptor, false);

            CvInvoke.Imshow("digit", icon);
            CvInvoke.WaitKey();

            if (screenshotKeyPoints.Size <= 6 || croppedKeyPoints.Size <= 6)
                return null;

            using var matcher = new BFMatcher(DistanceType.Hamming);
            matcher.Add(screenshotDescriptor);
            matcher.KnnMatch(croppedDescriptor, matches, k: 2);

            mask = new Mat(matches.Size, 1, DepthType.Cv8U, 1);
            mask.SetTo(new MCvScalar(255));

            Features2DToolbox.VoteForUniqueness(matches, uniqueThreshold, mask);

            int nonZero = CvInvoke.CountNonZero(mask);
            if (nonZero < 4) return null; // nothing reliable left → stop

            int count = Features2DToolbox.VoteForSizeAndOrientation(
                screenshotKeyPoints,  // model (your `icon` keypoints)
                croppedKeyPoints,     // observed (your `screenshot` keypoints)
                matches, mask, 1.5, 20); // 20 bins is the sample default

            if (count < 25)
                return null;

            homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(screenshotKeyPoints, croppedKeyPoints, matches, mask, 10);
            Console.WriteLine($"found character: {name}, count: {count}");

            //var show = new Mat();
            //Features2DToolbox.DrawMatches(screenshot, screenshotKeyPoints, icon, croppedKeyPoints, matches, show, new MCvScalar(255), new MCvScalar(100), null, Features2DToolbox.KeypointDrawType.Default);
            //CvInvoke.Imshow("matched?", show);
            //CvInvoke.WaitKey();

            if (homography != null)
            {
                var rect = new Rectangle(Point.Empty, icon.Size);
                var pointsF = new PointF[]
                {
                    new PointF(rect.Left, rect.Bottom),
                    new PointF(rect.Right, rect.Bottom),
                    new PointF(rect.Right, rect.Top),
                    new PointF(rect.Left, rect.Top)
                };

                pointsF = CvInvoke.PerspectiveTransform(pointsF, homography);
                var points = Array.ConvertAll<PointF, Point>(pointsF, Point.Round);
                vectorOfPoint = new VectorOfPoint(points);
            }

            //if(vectorOfPoint.Length > 0)
            //{
            //    CvInvoke.Polylines(screenshot, vectorOfPoint, true, new MCvScalar(0, 0, 255), 5);
            //    CvInvoke.Imshow("namr", screenshot);
            //    CvInvoke.WaitKey();
            //}

            return vectorOfPoint;
        }

        public Dictionary<string, Bitmap> GetDigits()
        {
            var assetsRelativePath = Path.Combine("C:\\dev\\learning\\gameAutomationLearning", "stats_digits");
            var allowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".png" };

            var dict = new Dictionary<string, Bitmap>();
            foreach (var file in Directory.EnumerateFiles(assetsRelativePath, "*", SearchOption.AllDirectories))
            {
                if (!allowed.Contains(Path.GetExtension(file)))
                    continue;

                using var image = Image.FromFile(file);

                if (image == null)
                    continue;

                var bitmap = new Bitmap(image);
                bitmap.ConvertFormat(PixelFormat.Format8bppIndexed);
                dict.Add(Path.GetFileNameWithoutExtension(file), bitmap);
            }

            return dict;
        }
    }
}
