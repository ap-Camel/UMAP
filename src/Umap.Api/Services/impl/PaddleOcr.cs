using OpenCvSharp;
using OpenCvSharp.Extensions;
using Sdcb.PaddleInference;
using Sdcb.PaddleOCR;
using Sdcb.PaddleOCR.Models;
using Sdcb.PaddleOCR.Models.Local;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Umap.Api.Services.impl
{
    public class PaddleOcr : IPaddleOcr
    {
        private static readonly Regex DigitsPlus = new(@"[^+0-9]", RegexOptions.Compiled);

        public string ReadDigitsFromImage(Bitmap image)
        {
            FullOcrModel model = LocalFullModels.EnglishV3; // PP-OCRv3 English+digits model
            using Mat src = BitmapConverter.ToMat(image);

            // 0) If you already know the ROI, crop it here to reduce clutter/background.
            // var roi = new Rect(x, y, w, h); using Mat cropped = new Mat(src, roi);

            // 1) Light pre-processing: grayscale, enlarge small crops, normalize contrast.
            var channels = src.Channels();
            var ddd = src.Depth();
            //Cv2.ImShow("ssss", src);
            //Cv2.WaitKey();

            //using Mat gray = src.CvtColor(ColorConversionCodes.RGB2GRAY);
            double scale = src.Height < 64 ? 2.0 : 1.0;                // small single digits benefit from scale-up
            using Mat scaled = src.Resize(new OpenCvSharp.Size(0,0), scale, scale, InterpolationFlags.Linear);
            Cv2.Normalize(scaled, scaled, 0, 255, NormTypes.MinMax);
            Cv2.Threshold(scaled, scaled, 0, 255, ThresholdTypes.Otsu | ThresholdTypes.Binary); // clean background

            //var cnfig = new PaddleConfig
            //{

            //};

            //var config2 = new PaddlePredictor
            //{

            //};

            using var all = new PaddleOcrAll(model, PaddleDevice.Mkldnn())
            {
                // 2) Turn off heavy/irrelevant steps to reduce errors and gain speed for UI text:
                AllowRotateDetection = false,        // screenshots/UI are usually 0°
                Enable180Classification = false,     // skip 180° classifier to lower noise/latency :contentReference[oaicite:0]{index=0}
            };
            all.Detector.UnclipRatio = 1.2f;

            // 3) Tighten detector for small, bold glyphs (optional, experiment per game/UI):
            all.Detector.UnclipRatio = 1.2f;        // fewer bloated boxes; helps when digits have outlines :contentReference[oaicite:1]{index=1}
                                                    // If you see missed boxes, also try: all.Detector.MaxSize = 1536 or null for no downscale (better accuracy, slower). :contentReference[oaicite:2]{index=2}

            var result = all.Run(scaled);

            // 4) Post-filter to digits and "+" only; also use scores if you need a threshold.
            //    (You can expose a minScore and drop low-confidence regions.)
            string text = result.Text;
            text = DigitsPlus.Replace(text, string.Empty);
            return text;
        }

        public string ReadTextFromImage(Bitmap image)
        {
            FullOcrModel model = LocalFullModels.EnglishV3;
            using Mat cvSharpMat = BitmapConverter.ToMat(image);

            using (PaddleOcrAll all = new PaddleOcrAll(model, PaddleDevice.Mkldnn())
            {
                //AllowRotateDetection = true, /* 允许识别有角度的文字 */
                //Enable180Classification = false, /* 允许识别旋转角度大于90度的文字 */
            })
            {
                PaddleOcrResult result = all.Run(cvSharpMat);
                Console.WriteLine("Detected all texts: \n" + result.Text);
                //Console.WriteLine($"confidence: {result.sc}")
                //foreach (PaddleOcrResultRegion region in result.Regions)
                //{
                //    Console.WriteLine($"Text: {region.Text}, Score: {region.Score}, RectCenter: {region.Rect.Center}, RectSize:    {region.Rect.Size}, Angle: {region.Rect.Angle}");
                //}

                return result.Text;
            }
        }
    }
}
