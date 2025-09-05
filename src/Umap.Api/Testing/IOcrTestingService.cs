using Umap.Api.Testing.impl;

namespace Umap.Api.Testing
{
    public interface IOcrTestingService
    {
        OcrTestingResult TestingPaddleOcr();
        OcrTestingResult TestingTesseract();
    }
}
