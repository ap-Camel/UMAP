using Microsoft.AspNetCore.Mvc;
using Umap.Api.Services;
using Umap.Api.Testing;
using Umap.Api.Testing.impl;

namespace Umap.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OcrTestingController : ControllerBase
    {
        private readonly IPaddleOcr _paddleOcr;
        private readonly IOcrTestingService _testingOcr;

        public OcrTestingController(IPaddleOcr paddleOcr, IOcrTestingService testingOcr)
        {
            _paddleOcr = paddleOcr;
            _testingOcr = testingOcr;
        }


        [HttpPost("testPaddleOcr")]
        public async Task<ActionResult<OcrTestingResult>> TestPaddleOcr()
        {
            return _testingOcr.TestingPaddleOcr();
        }

        [HttpPost("testPaddleOcrDigits")]
        public async Task<ActionResult<OcrTestingResult>> TestPaddleOcr02()
        {
            return _testingOcr.TestingPaddleOcrDigits();
        }

        [HttpPost("testTesseractOcr")]
        public async Task<ActionResult<OcrTestingResult>> TestTesseractOcr()
        {
            return _testingOcr.TestingTesseract();
        }

        [HttpPost("testTesseractOcrNumbersAndStatsIncrease")]
        public async Task<ActionResult<OcrTestingResult>> TestTesseractOcr02()
        {
            return _testingOcr.TestingTesseractNumbersAndStatsIncrease();
        }

        [HttpPost("testBothForDigits")]
        public async Task<ActionResult<OcrTestingResult>> TestBothForDigits()
        {
            return _testingOcr.TestingDigitsWithBoth();
        }

        [HttpPost("testFeatureMatching")]
        public async Task<ActionResult<OcrTestingResult>> TestFeatureMatching()
        {
            return _testingOcr.FeatureMatchingSolutionTesting();
        }
    }
}
