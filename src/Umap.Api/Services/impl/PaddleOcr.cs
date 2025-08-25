using OpenCvSharp;
using OpenCvSharp.Extensions;
using Sdcb.PaddleInference;
using Sdcb.PaddleOCR;
using Sdcb.PaddleOCR.Models;
using Sdcb.PaddleOCR.Models.Local;
using System.Drawing;

namespace Umap.Api.Services.impl
{
    public class PaddleOcr : IPaddleOcr
    {
        public string ReadTextFromImage(Bitmap image)
        {
            FullOcrModel model = LocalFullModels.EnglishV3;
            Mat cvSharpMat = BitmapConverter.ToMat(image);

            using (PaddleOcrAll all = new PaddleOcrAll(model, PaddleDevice.Mkldnn())
            {
                //AllowRotateDetection = true, /* 允许识别有角度的文字 */
                //Enable180Classification = false, /* 允许识别旋转角度大于90度的文字 */
            })
            {
                PaddleOcrResult result = all.Run(cvSharpMat);
                Console.WriteLine("Detected all texts: \n" + result.Text);
                //Console.WriteLine($"confidence: {result.sc}")
                foreach (PaddleOcrResultRegion region in result.Regions)
                {
                    Console.WriteLine($"Text: {region.Text}, Score: {region.Score}, RectCenter: {region.Rect.Center}, RectSize:    {region.Rect.Size}, Angle: {region.Rect.Angle}");
                }

                return result.Text;
            }
        }
    }
}
