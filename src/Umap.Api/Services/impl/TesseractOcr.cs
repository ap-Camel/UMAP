using Emgu.CV;
using Emgu.CV.CvEnum;
using System.Drawing;
using Tesseract;

namespace Umap.Api.Services.impl
{
    public class TesseractOcr : ITesseractOcr
    {
        public string ReadTextFromImage(Bitmap image)
        {
            var src01 = image.ToMat();

            Mat up2x = new Mat();
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



            using (var engine = new TesseractEngine("C:\\dev\\learning\\gameAutomationLearning\\UMAP\\src\\Umap.Api\\tessdata", "eng", EngineMode.Default))
            {

                // If you only expect numbers, whitelist just those characters (add the punctuation you expect)
                //engine.SetVariable("tessedit_char_whitelist", "0123456789+");

                //// Optional: reduce language-model/dictionary influence (helps with pure numbers)
                //engine.SetVariable("load_system_dawg", "0");
                //engine.SetVariable("load_freq_dawg", "0");


                using (var page = engine.Process(Denoised.ToBitmap(), PageSegMode.SingleLine))
                {
                    var text = page.GetText().Trim();
                    Console.WriteLine("Mean confidence: {0}", page.GetMeanConfidence());

                    Console.WriteLine("Text (GetText): \r\n{0}", text);
                    return text;
                }
            }
        }

        public string ReadNumbersAndSignsFromImage(Bitmap image)
        {
            var src01 = image.ToMat();

            Mat up2x = new Mat();
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



            using (var engine = new TesseractEngine("C:\\dev\\learning\\gameAutomationLearning\\UMAP\\src\\Umap.Api\\tessdata", "eng", EngineMode.Default))
            {

                // If you only expect numbers, whitelist just those characters (add the punctuation you expect)
                engine.SetVariable("tessedit_char_whitelist", "0123456789+");

                //// Optional: reduce language-model/dictionary influence (helps with pure numbers)
                //engine.SetVariable("load_system_dawg", "0");
                //engine.SetVariable("load_freq_dawg", "0");


                using (var page = engine.Process(Denoised.ToBitmap(), PageSegMode.SingleLine))
                {
                    var text = page.GetText().Trim();
                    Console.WriteLine("Mean confidence: {0}", page.GetMeanConfidence());

                    Console.WriteLine("Text (GetText): \r\n{0}", text);
                    return text;
                }
            }
        }
    }
}
