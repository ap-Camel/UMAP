using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using Tesseract;
using Umap.Api.Services;

namespace Umap.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IScreenCaptureService _screenCaptureService;
        private readonly IScreenCropService _screenCropService;
        private readonly IWindowService _windowService;
        private readonly IPaddleOcr _paddleOcr;
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger, IScreenCaptureService screenCaptureService, IScreenCropService screenCropService, 
            IWindowService windowService, IPaddleOcr paddleOcr)
        {
            _logger = logger;
            _screenCaptureService = screenCaptureService;
            _screenCropService = screenCropService;
            _paddleOcr = paddleOcr;
            _windowService = windowService;
        }


        [HttpPost("capture_screen")]
        public async Task CaptureScreen()
        {
            var window = _windowService.GetWindowCoordinates();
            //UmaMusumeWindowBringToFrontService.BringToFront(windowHandle);

            var screenCapture = _screenCaptureService.CaptureActiveWindow();
            var umaMusumeWindow = _screenCropService.CropMainGameScreen(screenCapture, window);

            Bitmap cropped = _screenCropService.CropLastAdded(screenCapture, window);
            //cropped.Dispose();

            //Bitmap bmp32 = cropped.copy(Bitmap..ARGB_8888, true);
            cropped.ConvertFormat(PixelFormat.Format8bppIndexed);
            //Utils.bitmapToMat(bmp32, mat);

            Mat src01 = new Mat();
            var src02 = new Mat();
            cropped.ToMat(src01);

            umaMusumeWindow.ConvertFormat(PixelFormat.Format8bppIndexed);
            umaMusumeWindow.ToMat(src02);

            //var gray = new Mat();
            //CvInvoke.CvtColor(src01, gray, ColorConversion.Bgr2Gray);

            //var clahe = CvInvoke.create(clipLimit: 3.0, tileGridSize: new Size(8, 8));
            //var contrast = new Mat();
            //clahe.Apply(gray, contrast);

            Emgu.CV.Mat up2x = new Mat();
            CvInvoke.Resize(src01, up2x, Size.Empty, 2.0, 2.0, Inter.Lanczos4);
            //CvInvoke.Imshow("resized", up2x);
            //CvInvoke.WaitKey();

            Mat Denoised = new Mat();
            CvInvoke.FastNlMeansDenoisingColored(up2x, Denoised, 10, 10, 7, 21);
            //CvInvoke.Imshow("resized", Denoised);
            //CvInvoke.WaitKey();

            CvInvoke.CvtColor(Denoised, Denoised, ColorConversion.Bgra2Gray);
            //CvInvoke.Imshow("denoised", Denoised);
            //CvInvoke.WaitKey();


            using (var engine = new TesseractEngine(@"./tessdata ", "eng", EngineMode.Default))
            {

                // If you only expect numbers, whitelist just those characters (add the punctuation you expect)
                //engine.SetVariable("tessedit_char_whitelist", "0123456789");

                //// Optional: reduce language-model/dictionary influence (helps with pure numbers)
                //engine.SetVariable("load_system_dawg", "0");
                //engine.SetVariable("load_freq_dawg", "0");


                using (var page = engine.Process(Denoised.ToBitmap(), PageSegMode.SingleLine))
                {
                    var text = page.GetText().Trim();
                    Console.WriteLine("Mean confidence: {0}", page.GetMeanConfidence());

                    Console.WriteLine("Text (GetText): \r\n{0}", text);
                }
            }

            Console.WriteLine("and now for paddleOCR");
            _paddleOcr.ReadTextFromImage(Denoised.ToBitmap());

            var updated = Denoised.ToBitmap();
            string fileName04 = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
            "screenshot04.png");
            updated.Save(fileName04);
            updated.Dispose();

            cropped.Dispose();

            // 3. Save the bitmap (dispose afterwards)
            //screenCapture.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
            screenCapture.Dispose();

            //umaMusumeWindow.Save(fileName03, System.Drawing.Imaging.ImageFormat.Png);
            umaMusumeWindow.Dispose();



            //MachineLearning.function01();

            //var assetsRelativePath = Path.Combine("C:\\dev\\learning\\gameAutomationLearning", "character_db04");
            //var imagePaths = LoadImagesFromDirectory(assetsRelativePath);

            //var outt = BestTemplate(src01, imagePaths);
            //Console.WriteLine($"best match: lable: {outt.label}, score: {outt.score}");

            //var matching = TemplateMatching02(src02, imagePaths);
            //var kitasan = CvInvoke.Imread("C:\\dev\\learning\\gameAutomationLearning\\character_db04\\kitasan_black_profile_icon.png", ImreadModes.ColorBgr);
            var kitasan = CvInvoke.Imread("C:\\dev\\learning\\gameAutomationLearning\\character_db_manual\\kitasan_black.png", ImreadModes.ColorBgr);
            //TemplateMatching03(src02, kitasan);

            //TemplateMatching04(src01);

            //CvInvoke.CvtColor(src01, src01, ColorConversion.Bgr2Gray);
            //CvInvoke.Canny(src01, src01, 150, 50);
            //CvInvoke.Imshow("ssss", src01);
            //CvInvoke.WaitKey();


            foreach (var icon in GetAllCharacterIconTemplates())
            {
                //CvInvoke.Canny(icon.Value, icon.Value, 150, 50);
                var vectorOfPoint = FeatureMatching01(icon.Value, src01, icon.Key);
                if (vectorOfPoint.Length > 0)
                {
                    var xPoint = src01.Width;
                    var yPoint = src01.Height;
                    for (int i = 0; i < Math.Floor((vectorOfPoint.Length / 8) * 1.0); i++)
                    {
                        var point = vectorOfPoint[i];

                        if (point.X < xPoint)
                            xPoint = point.X;

                        if (point.Y < yPoint)
                            yPoint = point.Y;
                    }

                    //var point = vectorOfPoint[-1];
                    CvInvoke.Polylines(src01, vectorOfPoint, true, new MCvScalar(0, 0, 255), 5);
                    CvInvoke.PutText(src01, icon.Key, new Point(xPoint, yPoint), FontFace.HersheyPlain, 1.3, new MCvScalar(0, 0, 255), 2);
                    //Console.WriteLine($"found character: {icon.Key}");
                    //CvInvoke.Imshow("namr", screenshot);
                    //CvInvoke.WaitKey();
                }
            }

            CvInvoke.Imshow("namr", src01);
            CvInvoke.WaitKey();

            //var tttt = FeatureMatching01(kitasan, src01);

        }


        private List<string> LoadImagesFromDirectory(string folder)
        {
            var allowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png" };

            var listIfImagePaths = new List<string>();
            foreach (var file in Directory.EnumerateFiles(folder, "*", SearchOption.AllDirectories))
            {
                if (!allowed.Contains(Path.GetExtension(file)))
                    continue;

                listIfImagePaths.Add(file);
            }

            return listIfImagePaths;
        }

        private List<string> TemplateMatching04(Mat screenshot)
        {
            var list = new List<string>();
            var rects = new List<Rectangle>();
            var characters = GetAllCharacterIconTemplates();

            CvInvoke.Resize(screenshot, screenshot, new Size(0, 0), 1.072d, 1.072d);
            //CvInvoke.FastNlMeansDenoisingColored(screenshot, screenshot, 10, 10, 7, 21);
            CvInvoke.CvtColor(screenshot, screenshot, ColorConversion.Bgr2Gray);
            CvInvoke.Canny(screenshot, screenshot, 150, 50);


            foreach (var character in characters)
            {
                var templateMatching = new Mat();

                var depthType01 = character.Value.Depth;
                var depthType02 = screenshot.Depth;

                var numOfChanels01 = character.Value.NumberOfChannels;
                var numOfChanels02 = screenshot.NumberOfChannels;

                CvInvoke.CvtColor(character.Value, character.Value, ColorConversion.Bgra2Gray);
                CvInvoke.Canny(character.Value, character.Value, 150, 50);

                CvInvoke.MatchTemplate(screenshot, character.Value, templateMatching, TemplateMatchingType.SqdiffNormed);

                double minVal = 0;
                double maxVal = 0;
                Point minLoc = new Point();
                Point maxLoc = new Point();

                CvInvoke.MinMaxLoc(templateMatching, ref minVal, ref maxVal, ref minLoc, ref maxLoc);

                if (character.Key.Contains("sakura") || character.Key.Contains("super"))
                {
                    CvInvoke.Imshow($"{character.Key}", character.Value);
                }


                if (maxVal > 0.75)
                {
                    list.Add(character.Key);
                    Console.WriteLine($"found character: {character.Key}, confidence: {maxVal}");

                    var rect = new Rectangle(maxLoc.X, maxLoc.Y, character.Value.Width, character.Value.Height);
                    rects.Add(rect);
                }
            }

            for (int i = 0; i < rects.Count; i++)
            {
                CvInvoke.Rectangle(screenshot, rects[i], new MCvScalar(0, 0, 255), 3);
                CvInvoke.PutText(screenshot, list[i], new Point(rects[i].X, rects[i].Y - 10), FontFace.HersheyPlain, 1.3, new MCvScalar(0, 0, 255), 2);

            }

            CvInvoke.Imshow("matching", screenshot);
            CvInvoke.WaitKey();

            return list;
        }

        private Dictionary<string, Mat> GetAllCharacterIconTemplates()
        {
            var assetsRelativePath = Path.Combine("C:\\dev\\learning\\gameAutomationLearning", "character_db_manual");
            var allowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png" };

            var dict = new Dictionary<string, Mat>();
            foreach (var file in Directory.EnumerateFiles(assetsRelativePath, "*", SearchOption.AllDirectories))
            {
                if (!allowed.Contains(Path.GetExtension(file)))
                    continue;

                var tempMat = CvInvoke.Imread(file, ImreadModes.Grayscale);
                dict.Add(Path.GetFileNameWithoutExtension(file), tempMat);
                //CvInvoke.Imshow("char", tempMat);
                //CvInvoke.PollKey();
            }

            return dict;
        }

        private (double score, string label) BestTemplate(Mat cropPath, List<string> templatePaths)
        {

            double bestScore = -1;
            string bestLabel = "";
            foreach (var t in templatePaths)
            {
                using var templFull = CvInvoke.Imread(t);
                foreach (var scale in new double[] { 0.8, 0.9, 1.0, 1.1, 1.2 })
                {
                    //using var templ = new Mat();
                    //CvInvoke.Resize(templFull, templ, new Size(0, 0), scale, scale, Inter.Nearest);

                    //if (templ.Rows > cropPath.Rows || templ.Cols > cropPath.Cols) continue;

                    using var res = new Mat();
                    CvInvoke.MatchTemplate(cropPath, templFull, res, TemplateMatchingType.CcoeffNormed);
                    double minVal = 0;
                    double maxVal = 0;
                    Point maxLoc = new Point();
                    Point minLoc = new Point();
                    CvInvoke.MinMaxLoc(res, ref minVal, ref maxVal, ref minLoc, ref maxLoc); // 0..1, higher is better

                    if (maxVal > bestScore)
                    {
                        bestScore = maxVal;
                        bestLabel = Path.GetFileName(Path.GetFileName(t)!)!;
                    }
                }
            }
            return (bestScore, bestLabel);
        }

        private List<string> TemplateMatching02(Mat screenshot, List<string> iconPaths)
        {
            var matching = new List<string>();
            CvInvoke.Resize(screenshot, screenshot, new Size(0, 0), .7d, .7d);
            var channelUpdated = new Mat();
            CvInvoke.CvtColor(screenshot, channelUpdated, Emgu.CV.CvEnum.ColorConversion.Bgra2Bgr);

            foreach (var path in iconPaths)
            {
                var templFull = CvInvoke.Imread(path, ImreadModes.ColorBgr);
                //CvInvoke.CvtColor(templFull, templFull, Emgu.CV.CvEnum.ColorConversion.Bgra2Gray);

                CvInvoke.Resize(templFull, templFull, new Size(0, 0), .7d, .7d);
                var templateMatching = new Mat();

                var depthType01 = templFull.Depth;
                var depthType02 = screenshot.Depth;

                var numOfChanels01 = templFull.NumberOfChannels;
                var numOfChanels02 = channelUpdated.NumberOfChannels;

                //CvInvoke.Imshow("name", screenshot);
                //CvInvoke.WaitKey();


                CvInvoke.MatchTemplate(templFull, channelUpdated, templateMatching, TemplateMatchingType.CcoeffNormed);

                double minVal = 0;
                double maxVal = 0;
                Point minLoc = new Point();
                Point maxLoc = new Point();

                CvInvoke.MinMaxLoc(templateMatching, ref minVal, ref maxVal, ref minLoc, ref maxLoc);
                var name = Path.GetFileName(path);
                //CvInvoke.Imshow($"{name} - template", templateMatching);
                //CvInvoke.Imshow($"{name} - channel", channelUpdated);
                //CvInvoke.Imshow($"{name} - full", templFull);
                //CvInvoke.WaitKey();

                CvInvoke.Threshold(templateMatching, templateMatching, 0.5, 1, ThresholdType.ToZero);
                //CvInvoke.Imshow($"{name} - template", templateMatching);
                //CvInvoke.WaitKey();

                if (minVal > 0.5)
                {
                    matching.Add(path);
                    Console.WriteLine($"found matching instances in path: {Path.GetFileName(path)}");
                }
            }

            return matching;
        }


        private void TemplateMatching03(Mat screenshot, Mat cropped)
        {
            var widths2 = screenshot.Width;
            var heights2 = screenshot.Height;

            var screenshotArea = screenshot.Width * screenshot.Height;
            var screenshotIconArea = screenshotArea * 0.002761d;
            var difference = screenshotArea / screenshotIconArea;
            var virtualScreenArea = difference * 48841d;
            var difference2 = virtualScreenArea / screenshotArea;

            //CvInvoke.Resize(screenshot, screenshot, new Size(0, 0), 2.35d, 2.35d);
            //CvInvoke.Resize(screenshot, screenshot, new Size(0, 0), 0.4d, 0.4d);
            //CvInvoke.Resize(cropped, cropped, new Size(0, 0), 0.4d, 0.4d);
            //CvInvoke.FastNlMeansDenoisingColored(screenshot, screenshot, 10, 10, 7, 21);
            var widths = screenshot.Width;
            var heights = screenshot.Height;


            Mat src = new Mat();
            Mat templ = new Mat();

            //CvInvoke.Imshow("screenshot", screenshot);
            //CvInvoke.Imshow("icon", cropped);
            //CvInvoke.WaitKey();

            CvInvoke.CvtColor(screenshot, src, ColorConversion.Bgra2Gray);
            CvInvoke.CvtColor(cropped, templ, ColorConversion.Bgra2Gray);

            CvInvoke.GaussianBlur(src, src, new Size(3, 3), 0);
            CvInvoke.GaussianBlur(templ, templ, new Size(3, 3), 0);

            //Mat srcEdge = new Mat(); CvInvoke.Canny(src, srcEdge, 50, 150);
            //Mat templEdge = new Mat(); CvInvoke.Canny(templ, templEdge, 50, 150);

            //(double score, Rectangle box) best = (-1, Rectangle.Empty);

            //foreach (var useEdges in new[] { false, true })
            //{
            //    Mat I = useEdges ? srcEdge : src;
            //    Mat T = useEdges ? templEdge : templ;

            //    // 3) Multi-scale search: scale template 60%..140% (tune as needed)
            //    for (double s = 0.6; s <= 1.4; s += 0.05)
            //    {
            //        Size tsz = new Size((int)(T.Cols * s), (int)(T.Rows * s));
            //        if (tsz.Width < 8 || tsz.Height < 8) continue;
            //        if (tsz.Width >= I.Cols || tsz.Height >= I.Rows) continue;

            //        using var Tscaled = new Mat();
            //        CvInvoke.Resize(T, Tscaled, tsz, 0, 0, Inter.Linear);

            //        using var result = new Mat(
            //            I.Rows - Tscaled.Rows + 1, I.Cols - Tscaled.Cols + 1,
            //            DepthType.Cv32F, 1);

            //        CvInvoke.MatchTemplate(I, Tscaled, result, TemplateMatchingType.CcoeffNormed); // max is best
            //        double minVal = 0;
            //        double maxVal = 0;
            //        Point minLoc = new Point();
            //        Point maxLoc = new Point();
            //        CvInvoke.MinMaxLoc(result, ref minVal, ref maxVal, ref minLoc, ref maxLoc);

            //        if (maxVal > best.score)
            //            best = (maxVal, new Rectangle(maxLoc, Tscaled.Size));
            //    }
            //}

            //Console.WriteLine($"best is: {best}");

            CvInvoke.CvtColor(screenshot, screenshot, Emgu.CV.CvEnum.ColorConversion.Bgra2Bgr);
            //CvInvoke.Resize(screenshot, screenshot, new Size(0, 0), .7d, .7d);
            //CvInvoke.Resize(cropped, cropped, new Size(0, 0), .7d, .7d);

            var depthType01 = screenshot.Depth;
            var depthType02 = cropped.Depth;

            var numOfChanels01 = screenshot.NumberOfChannels;
            var numOfChanels02 = cropped.NumberOfChannels;

            //CvInvoke.Resize(screenshot, screenshot, new Size(0, 0), 0.5d, 0.5d);
            //CvInvoke.Resize(cropped, cropped, new Size(0, 0), 0.5d, 0.5d);

            //CvInvoke.FastNlMeansDenoisingColored(screenshot, screenshot, 10, 10, 7, 21);
            //CvInvoke.Imshow("screenshot", screenshot);
            //CvInvoke.Imshow("icon", cropped);
            //CvInvoke.WaitKey();


            var templateMatching = new Mat();
            //CvInvoke.MatchTemplate(screenshot, cropped, templateMatching, TemplateMatchingType.CcoeffNormed);
            CvInvoke.MatchTemplate(src, templ, templateMatching, TemplateMatchingType.CcoeffNormed);

            //var channelUpdated = new Mat();
            //CvInvoke.CvtColor(screenshot, channelUpdated, Emgu.CV.CvEnum.ColorConversion.Bgra2Bgr);

            //var templFull = CvInvoke.Imread(path, ImreadModes.ColorBgr);
            //CvInvoke.CvtColor(templFull, templFull, Emgu.CV.CvEnum.ColorConversion.Bgra2Gray);

            //CvInvoke.Resize(templFull, templFull, new Size(0, 0), .7d, .7d);
            //var templateMatching = new Mat();


            //var depthType01 = templFull.Depth;
            //var depthType02 = screenshot.Depth;

            //var numOfChanels01 = templFull.NumberOfChannels;
            //var numOfChanels02 = channelUpdated.NumberOfChannels;

            //CvInvoke.Imshow("name", screenshot);
            //CvInvoke.WaitKey();


            //CvInvoke.MatchTemplate(templFull, channelUpdated, templateMatching, TemplateMatchingType.CcoeffNormed);

            double minVal = 0;
            double maxVal = 0;
            Point minLoc = new Point();
            Point maxLoc = new Point();

            CvInvoke.MinMaxLoc(templateMatching, ref minVal, ref maxVal, ref minLoc, ref maxLoc);
            //var name = Path.GetFileName(path);
            //CvInvoke.Imshow($"{name} - template", templateMatching);
            //CvInvoke.Imshow($"{name} - channel", channelUpdated);
            //CvInvoke.Imshow($"{name} - full", templFull);
            //CvInvoke.WaitKey();

            CvInvoke.Threshold(templateMatching, templateMatching, 0.85, 1, ThresholdType.ToZero);
            CvInvoke.Imshow($"template", templateMatching);
            CvInvoke.WaitKey();
        }

        private VectorOfPoint FeatureMatching01(Mat icon, Mat screenshot, string name)
        {
            //CvInvoke.Resize(screenshot, screenshot, new Size(0, 0), 2.35d, 2.35d);
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
            double uniqueThreshold = 0.8;

            var matches = new VectorOfVectorOfDMatch();

            var featurDetector = new Brisk();
            featurDetector.DetectAndCompute(icon, null, screenshotKeyPoints, screenshotDescriptor, false);
            featurDetector.DetectAndCompute(screenshot, null, croppedKeyPoints, croppedDescriptor, false);

            if (screenshotKeyPoints.Size <= 6 || croppedKeyPoints.Size <= 6)
                return null;

            using var matcher = new BFMatcher(DistanceType.Hamming);
            matcher.Add(screenshotDescriptor);
            matcher.KnnMatch(croppedDescriptor, matches, k: 3);

            mask = new Mat(matches.Size, 1, DepthType.Cv8U, 1);
            mask.SetTo(new MCvScalar(255));

            Features2DToolbox.VoteForUniqueness(matches, uniqueThreshold, mask);
            int count = Features2DToolbox.VoteForSizeAndOrientation(screenshotKeyPoints, croppedKeyPoints, matches, mask, 1.5, 15);

            if (count >= 25)
            {
                homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(screenshotKeyPoints, croppedKeyPoints, matches, mask, 10);
                Console.WriteLine($"found character: {name}, count: {count}");
            }

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
    }
}
