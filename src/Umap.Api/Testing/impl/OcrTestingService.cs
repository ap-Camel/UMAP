using Emgu.CV;
using System.Drawing;
using System.Drawing.Imaging;
using Umap.Api.Services;

namespace Umap.Api.Testing.impl
{
    public class OcrTestingService : IOcrTestingService
    {
        private readonly IPaddleOcr _paddleOcr;

        public OcrTestingService(IPaddleOcr padder)
        {
            _paddleOcr = padder;
        }


        public OcrTestingResult TestingPaddleOcr()
        {
            // load images from a source folder.
            // image name contains expected value
            // foreach image, run text recognition, for numbers, it should be exact match, for strings, it should at least contains the words, or one of the words.

            var images = GetAllCharacterIconTemplates();
            var results = new OcrTestingResult();
            results.OcrTestings = new List<OcrTesting>();
            foreach (var image in images)
            {
                var expected = image.Key.Split('_').Last();
                var actual = _paddleOcr.ReadTextFromImage(image.Value);

                Console.WriteLine($"file name: {image.Key}, expected name: {expected}, actual name: {actual}");
                if (expected.ToLower() == actual.ToLower())
                    results.Passed++;

                results.OcrTestings.Add(new OcrTesting { Expected = expected, Actual = actual, FileName = image.Key});
                image.Value.Dispose();
            }

            return results;
        }

        public Dictionary<string, Bitmap> GetAllCharacterIconTemplates()
        {
            var assetsRelativePath = Path.Combine("C:\\dev\\learning\\gameAutomationLearning\\UMAP\\src\\Umap.Api\\Testing", "Sources");
            var allowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png" };

            var dict = new Dictionary<string, Bitmap>();
            foreach (var file in Directory.EnumerateFiles(assetsRelativePath, "*", SearchOption.AllDirectories))
            {
                if (!allowed.Contains(Path.GetExtension(file)))
                    continue;

                using var image = Image.FromFile(file);

                if(image == null)
                    continue;

                var bitmap = new Bitmap(image);
                bitmap.ConvertFormat(PixelFormat.Format8bppIndexed);
                dict.Add(Path.GetFileNameWithoutExtension(file), bitmap);
            }

            return dict;
        }



    }

    public class OcrTesting
    {
        public string Expected {  get; set; }
        public string Actual {  get; set; }
        public string FileName { get; set; }
    }

    public class OcrTestingResult
    {
        public int Count => OcrTestings.Count;
        public int Passed { get; set; }        
        public List<OcrTesting> OcrTestings { get; set; }
    }
}
