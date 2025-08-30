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


        [HttpPost("testOcr")]
        public async Task<ActionResult<OcrTestingResult>> TestOcr()
        {
            return _testingOcr.TestingPaddleOcr();
        }


    }
}
