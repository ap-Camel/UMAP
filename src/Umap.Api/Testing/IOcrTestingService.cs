using Umap.Api.Testing.impl;

namespace Umap.Api.Testing
{
    public interface IOcrTestingService
    {
        OcrTestingResult FeatureMatchingSolutionTesting();
        OcrTestingResult TemplateMatchingSolutionTesting();
        OcrTestingResult ChampherMatchingSolutionTesting();
        OcrTestingResult TestingPaddleOcr();
        OcrTestingResult TestingPaddleOcrDigits();
        OcrTestingResult TestingTesseract();
        OcrTestingResult TestingTesseractNumbersAndStatsIncrease();
        OcrTestingResult TestingDigitsWithBoth();
    }
}
