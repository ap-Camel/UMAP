using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Umap.Api.Services.impl
{
    public interface IDigitsChamferMatch
    {
        string MatchDigits(Mat image, int baseWidth = 2342, int baseHight = 1362);
    }

    public class DigitsChamferMatch : IDigitsChamferMatch
    {
        private const double _originalWidth = 2560;
        private const double _originalHeight = 1600;

        public string MatchDigits(Mat image, int baseWidth = 2342, int baseHight = 1362)
        {
            var digits = LoadDigits();
            var digitMatches = new OrderedDictionary<int, DigitTemplateMatchingResult>();

            foreach (var digit in digits)
            {
                var originalAspectRatio = _originalWidth / _originalHeight;
                var digitsAspectRatio = digit.Value.Width * 1.0d / digit.Value.Height;

                double widthToUse = digit.Value.Width;
                // width more than height
                if (digitsAspectRatio < 1)
                {
                    widthToUse = digit.Value.Height * originalAspectRatio;
                }

                var widthToUseFloor = (int)Math.Floor(widthToUse);
                var difference = _originalWidth / baseWidth;

                var resized = new Mat();
                CvInvoke.Resize(image, resized, new Size(0, 0), difference, difference);

                var temp = new List<Size>();
                for (int i = -10; i < 10; i++)
                {
                    temp.Add(new Size(digit.Value.Width + i, digit.Value.Height + i));
                }

                var bestMatch = MultiScaleChamferMatching(resized, digit.Value.ToMat(), temp);
                //if (bestMatch == null || bestMatch.maxVal < 0.65d)
                //    continue;

                //bestMatch.digit = digit.Key;
                //if (!digitMatches.TryGetValue(bestMatch.position.X, out var match))
                //{
                //    digitMatches.Add(bestMatch.position.X, bestMatch);
                //    continue;
                //}

                //if (bestMatch.maxVal > match.maxVal)
                //    digitMatches[bestMatch.position.X] = bestMatch;


                //var adjustedDigitsHeight = digit.Value.Width * 1.0d / originalAspectRatio;
                //var missingDigitsHightPixels = adjustedDigitsHeight - digit.Value.Height;

                //var percentageDigitWidth = digit.Value.Width / _originalWidth;
                //var percentageDigitHeight = digit.Value.Height / _originalHeight;


                //var percentageImageWidth = image.Width / baseWidth;
                //var percentageImageHeight = image.Height / baseHight;

            }

            var stringResult = string.Empty;
            foreach (var digitMatch in digitMatches)
            {
                stringResult += digitMatch.Value.digit;
            }

            return stringResult;
        }


        public List<Detection> MultiScaleChamferMatching(Mat image, Mat digit, List<Size> sizes)
        {
            var imageGray = ToGray(image);
            var imageEdges = CannyEdges(imageGray);
            var imageDistance = DistanceMapOfEdges(imageEdges);

            var allPeaks = new List<Detection>();

            foreach (var size in sizes)
            {
                if (size.Width > image.Width || size.Height > image.Height)
                {
                    Console.WriteLine($"size became bigger than image. image width: {image.Width}, image height: {image.Height}, template width: {size.Width}, template height: {size.Height}");
                    break;
                }

                var template = new Mat();
                CvInvoke.Resize(digit, template, size, interpolation: Inter.Linear);

                var templateGray = ToGray(template);
                var templateEdges = CannyEdges(templateGray);
                var templateDistance = DistanceMapOfEdges(templateEdges);

                using var kernel = EdgeKernel01(templateEdges, out int edgeCount);
                if (edgeCount < 10) continue; // too sparse; skip

                // 3) Slide via filter2D => mean distance under template edges
                using var score = new Mat();
                CvInvoke.Filter2D(imageDistance, score, kernel, new Point(-1, -1), 0.0, BorderType.Reflect101);

                // 4) Get top local minima with simple NMS
                int topK = 3;
                foreach (var p in TopLocalMinima(score, templateEdges.Size, topK))
                {
                    using var scoreImg = score.ToImage<Gray, float>();
                    float val = scoreImg.Data[p.Y, p.X, 0];
                    allPeaks.Add(new Detection { Center = p, Score = val, Size = size });
                }
            }

            return allPeaks;
        }


        private Mat ToGray(Mat src)
        {
            if (src.NumberOfChannels == 1)
                return src.Clone();

            var tempGray = new Mat();
            CvInvoke.CvtColor(src, tempGray, ColorConversion.Bgr2Gray);
            return tempGray;
        }


        private Mat CannyEdges(Mat src, double t1 = 150, double t2 = 50)
        {
            var tempBlured = new Mat();
            CvInvoke.GaussianBlur(src, tempBlured, new Size(3, 3), 0);

            var tempEdges = new Mat();
            CvInvoke.Canny(tempBlured, tempEdges, t1, t2);
            tempBlured.Dispose();

            return tempEdges;
        }

        private Mat DistanceMapOfEdges(Mat edges)
        {
            var inverted = new Mat();
            CvInvoke.BitwiseNot(edges, inverted); // non-edges=255, edges=0

            var dist = new Mat();
            CvInvoke.DistanceTransform(inverted, dist, null, DistType.L2, 3); // CV_32F
            inverted.Dispose();
            return dist;
        }

        private Mat EdgeKernel01(Mat templEdges, out int count)
        {
            var mask01 = new Mat();
            CvInvoke.Threshold(templEdges, mask01, 0, 1, ThresholdType.Binary); // 0/1 (8U)
            count = CvInvoke.CountNonZero(mask01);

            var kernel = new Mat();
            mask01.ConvertTo(kernel, DepthType.Cv32F, count > 0 ? (1.0 / count) : 1.0, 0.0);
            mask01.Dispose();

            return kernel;
        }

        private IEnumerable<Point> TopLocalMinima(Mat score, Size templSize, int k)
        {
            // Expect: score is single-channel CV_32F
            if (score.NumberOfChannels != 1 || score.Depth != Emgu.CV.CvEnum.DepthType.Cv32F)
                throw new ArgumentException("score must be single-channel CV_32F");

            // Invert to turn minima into maxima
            using var neg = new Mat();
            CvInvoke.Multiply(score, new ScalarArray(-1.0), neg);

            int radX = Math.Max(templSize.Width / 6, 3);
            int radY = Math.Max(templSize.Height / 6, 3);
            var ksize = new Size(radX * 2 + 1, radY * 2 + 1);
            using var kernel = CvInvoke.GetStructuringElement(ElementShape.Rectangle, ksize, new Point(-1, -1));

            using var maxed = new Mat();
            CvInvoke.Dilate(neg, maxed, kernel, new Point(-1, -1), 1, BorderType.Reflect101, new MCvScalar());

            // Convert to Image<Gray,float> for easy pixel access
            using var negImg = neg.ToImage<Gray, float>();
            using var maxImg = maxed.ToImage<Gray, float>();

            var candidates = new List<(Point p, float val)>();
            for (int y = 0; y < negImg.Height; y++)
            {
                for (int x = 0; x < negImg.Width; x++)
                {
                    float v = negImg.Data[y, x, 0];
                    float mv = maxImg.Data[y, x, 0];
                    if (Math.Abs(v - mv) <= 1e-6f) // local max in 'neg' => local min in 'score'
                        candidates.Add((new Point(x, y), -v)); // store original (positive) score
                }
            }

            return candidates.OrderBy(c => c.val).Take(k).Select(c => c.p);
        }


        private List<Detection> NmsByIoU(List<Detection> dets, double iouThresh)
        {
            var kept = new List<Detection>();
            var used = new bool[dets.Count];
            for (int i = 0; i < dets.Count; i++)
            {
                if (used[i]) continue;
                kept.Add(dets[i]);
                var a = dets[i].Box;
                for (int j = i + 1; j < dets.Count; j++)
                {
                    if (used[j]) continue;
                    var b = dets[j].Box;
                    double iou = IoU(a, b);
                    if (iou >= iouThresh) used[j] = true;
                }
            }
            return kept;
        }

        private double IoU(Rectangle a, Rectangle b)
        {
            int x1 = Math.Max(a.Left, b.Left);
            int y1 = Math.Max(a.Top, b.Top);
            int x2 = Math.Min(a.Right, b.Right);
            int y2 = Math.Min(a.Bottom, b.Bottom);
            int inter = Math.Max(0, x2 - x1) * Math.Max(0, y2 - y1);
            int uni = a.Width * a.Height + b.Width * b.Height - inter;
            return uni == 0 ? 0 : (double)inter / uni;
        }

        public class Detection
        {
            public Rectangle Box;
            public Point Center;     // center of template at best location
            public double Score;     // mean chamfer distance (lower is better)
            public Size Size;
        }

        public DigitTemplateMatchingResult TemplateMatchForScaleAround(Mat image, Mat digit, List<Size> sizes)
        {
            // resize
            // blure
            // canny
            // morphology
            // 




            var bestMath = 0d;
            var bestSizeMatch = sizes[0];
            var bestResult = new DigitTemplateMatchingResult();
            var bestMat = new Mat();

            CvInvoke.Resize(image, image, new Size(0, 0), 2.0d, 2.0d, interpolation: Inter.Linear);

            CvInvoke.GaussianBlur(image, image, new Size(3, 3), 0);

            var cannyImage = new Mat();
            CvInvoke.Canny(image, cannyImage, 150, 30);

            CvInvoke.Imshow("", cannyImage);
            CvInvoke.WaitKey();

            var filled2 = FillClosedRegions(cannyImage);

            //var k = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(3, 3), new Point(-1, -1));
            //CvInvoke.MorphologyEx(cannyImage, cannyImage, MorphOp.Close, k, new Point(-1, -1), 1, BorderType.Reflect, default(MCvScalar));

            //CvInvoke.GaussianBlur(image, image, new Size(3, 3), 0);
            ////CvInvoke.GaussianBlur(digit, digit, new Size(3, 3), 0);

            //CvInvoke.CvtColor(image, image, ColorConversion.Bgr2Gray);
            ////var cannyImage = new Mat();
            ////CvInvoke.Canny(image, cannyImage, 150, 50);

            //Mat bin2 = new Mat();
            //CvInvoke.Threshold(image, bin2, 0, 255, ThresholdType.Binary | ThresholdType.Otsu);

            //Mat kOpen2 = CvInvoke.GetStructuringElement(ElementShape.Ellipse, new Size(3, 3), new Point(-1, -1));
            //Mat kClose2 = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(5, 5), new Point(-1, -1));

            //Mat opened2 = new Mat();
            //CvInvoke.MorphologyEx(bin2, opened2, MorphOp.Open, kOpen2, new Point(-1, -1), 1, BorderType.Constant, new MCvScalar(0));

            //// close = dilate → erode (fill small gaps/holes)
            //Mat closed2 = new Mat();
            //CvInvoke.MorphologyEx(opened2, closed2, MorphOp.Close, kClose2, new Point(-1, -1), 1, BorderType.Constant, new MCvScalar(0));

            ////CvInvoke.CvtColor(digit, digit, ColorConversion.Bgra2Gray);

            //using var digitGray = new Mat();
            //CvInvoke.CvtColor(digit, digitGray, ColorConversion.Bgra2Gray); // alpha ignored here (on purpose)
            //var cannyDigits = new Mat();
            //CvInvoke.Canny(digit, cannyDigits, 150, 50);

            //using var alpha = new Mat();
            //CvInvoke.ExtractChannel(digit, alpha, 3);                        // alpha channel

            //using var fullMask = new Mat();
            //// Binary mask: foreground = 255 where alpha > 0 (tweak threshold if you want feathering)
            //CvInvoke.Threshold(alpha, fullMask, 0, 255, ThresholdType.Binary);

            for (int i = 0; i < sizes.Count; i++)
            {
                var templateMatching = new Mat();

                if (sizes[i].Width > image.Width || sizes[i].Height > image.Height)
                {
                    Console.WriteLine($"size became bigger than image. image width: {image.Width}, image height: {image.Height}, template width: {sizes[i].Width}, template height: {sizes[i].Height}");
                    break;
                }

                //var resized = new Mat();
                //CvInvoke.Resize(digit, resized, sizes[i]);

                using var templ = new Mat();
                //using var mask = new Mat();

                CvInvoke.Resize(digit, templ, sizes[i], interpolation: Inter.Linear);

                CvInvoke.Resize(templ, templ, new Size(0, 0), 2.0d, 2.0d, interpolation: Inter.Linear);

                CvInvoke.GaussianBlur(templ, templ, new Size(3, 3), 0);

                var cannyDigits = new Mat();
                CvInvoke.Canny(templ, cannyDigits, 150, 50);

                var filled = FillClosedRegions(cannyDigits);

                //var k2 = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(3, 3), new Point(-1, -1));
                //CvInvoke.MorphologyEx(cannyDigits, cannyDigits, MorphOp.Close, k2, new Point(-1, -1), 1, BorderType.Reflect, default(MCvScalar));

                //Mat bin = new Mat();
                //CvInvoke.Threshold(digitGray, bin, 0, 255, ThresholdType.Binary | ThresholdType.Otsu);

                //Mat kOpen = CvInvoke.GetStructuringElement(ElementShape.Ellipse, new Size(3, 3), new Point(-1, -1));
                //Mat kClose = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(5, 5), new Point(-1, -1));

                //Mat opened = new Mat();
                //CvInvoke.MorphologyEx(bin, opened, MorphOp.Open, kOpen, new Point(-1, -1), 1, BorderType.Constant, new MCvScalar(0));

                //// close = dilate → erode (fill small gaps/holes)
                //Mat closed = new Mat();
                //CvInvoke.MorphologyEx(opened, closed, MorphOp.Close, kClose, new Point(-1, -1), 1, BorderType.Constant, new MCvScalar(0));

                //var cannyDigits = new Mat();
                //CvInvoke.Canny(templ, cannyDigits, 150, 50);

                // Keep mask crisp (no interpolation of 0/255)
                //CvInvoke.Resize(fullMask, mask, sizes[i], interpolation: Inter.Nearest);

                //CvInvoke.Canny(digit, digit, 150, 50);

                //CvInvoke.Imshow("image", image);
                //CvInvoke.Imshow("digit", resized);
                //CvInvoke.WaitKey();

                //if (i == 20)
                //{
                //    CvInvoke.Imshow("image", image);
                //    CvInvoke.Imshow("digt", templ);
                //    CvInvoke.WaitKey();
                //}

                CvInvoke.MatchTemplate(filled2, filled, templateMatching, TemplateMatchingType.CcorrNormed);

                double minVal = 0;
                double maxVal = 0;
                Point minLoc = new Point();
                Point maxLoc = new Point();

                CvInvoke.MinMaxLoc(templateMatching, ref minVal, ref maxVal, ref minLoc, ref maxLoc);

                if (maxVal > bestMath)
                {
                    bestMath = maxVal;
                    bestMat = filled.Clone();
                    var rect = new Rectangle(maxLoc.X, maxLoc.Y, digit.Width, digit.Height);
                    bestResult = new DigitTemplateMatchingResult { digit = "", maxVal = maxVal, position = rect };
                }

            }

            CvInvoke.Imshow("best", bestMat);
            CvInvoke.Imshow("image", filled2);
            CvInvoke.WaitKey();
            return bestResult;
        }


        public Dictionary<string, Bitmap> LoadDigits()
        {
            var assetsRelativePath = Path.Combine("C:\\dev\\learning\\gameAutomationLearning", "stats_digits_cleaned");
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

        public Mat FillClosedRegions(Mat edges, bool onlyCircles = false, double circularityMin = 0.70)
        {
            if (edges.NumberOfChannels != 1 || edges.Depth != Emgu.CV.CvEnum.DepthType.Cv8U)
                throw new ArgumentException("Expect 8-bit single-channel edge image");

            // 1) Make edges a bit thicker / close tiny breaks so loops are truly closed.
            var k = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(3, 3), new Point(-1, -1));
            using var edgesThick = new Mat();
            CvInvoke.MorphologyEx(edges, edgesThick, MorphOp.Close, k, new Point(-1, -1), 1, BorderType.Reflect, default);
            CvInvoke.Dilate(edgesThick, edgesThick, k, new Point(-1, -1), 1, BorderType.Reflect, default);

            // 2) Invert: boundaries black (0), everywhere else white (255).
            using var inv = new Mat();
            CvInvoke.BitwiseNot(edgesThick, inv);

            // 3) Flood-fill from outside to mark background. Remaining white pixels are enclosed regions.
            var flood = inv.Clone();               // will become: outside=0, edges=0, enclosed=255
            using var mask = new Mat(flood.Rows + 2, flood.Cols + 2, DepthType.Cv8U, 1);
            mask.SetTo(new MCvScalar(0));
            Rectangle _; // unused
            CvInvoke.FloodFill(
                flood,           // image (modified in place)
                mask,            // mask (must be 2 bigger in each dim)
                new Point(0, 0), // seed outside the edges
                new MCvScalar(0),
                out _, new MCvScalar(0), new MCvScalar(0),
                Connectivity.EightConnected);

            // 4) 'flood' now has only the enclosed regions as white (255).
            if (!onlyCircles)
            {
                // If you want the edges + filled interiors:
                var filled = new Mat();
                CvInvoke.BitwiseOr(edgesThick, flood, filled);
                return filled; // white edges and white filled interiors
            }
            else
            {
                // Keep only circular-ish holes using contour circularity.
                using var holesOnly = new Mat();
                flood.CopyTo(holesOnly);

                using var contours = new VectorOfVectorOfPoint();
                using var hier = new Mat();
                CvInvoke.FindContours(holesOnly, contours, hier, RetrType.External, ChainApproxMethod.ChainApproxSimple);

                var outMask = Mat.Zeros(edges.Size.Height, edges.Size.Width, DepthType.Cv8U, 1);
                for (int i = 0; i < contours.Size; i++)
                {
                    using var c = contours[i];
                    double area = CvInvoke.ContourArea(c);
                    double perim = CvInvoke.ArcLength(c, true);
                    if (perim <= 1) continue;
                    double circularity = 4.0 * Math.PI * area / (perim * perim); // 1.0 is perfect circle
                    if (circularity >= circularityMin)
                    {
                        CvInvoke.DrawContours(outMask, contours, i, new MCvScalar(255), -1);
                    }
                }

                // Combine (optional): edges + circular holes
                var combined = new Mat();
                CvInvoke.BitwiseOr(edgesThick, outMask, combined);
                return combined;
            }
        }
    }
}
