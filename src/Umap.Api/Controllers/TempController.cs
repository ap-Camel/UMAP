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
    public class TempController : ControllerBase
    {
        private readonly IScreenCaptureService _screenCaptureService;
        private readonly IScreenCropService _screenCropService;
        private readonly IWindowService _windowService;
        private readonly IPaddleOcr _paddleOcr;
        private readonly ILogger<TestController> _logger;

        public TempController(ILogger<TestController> logger, IScreenCaptureService screenCaptureService, IScreenCropService screenCropService, 
            IWindowService windowService, IPaddleOcr paddleOcr)
        {
            _logger = logger;
            _screenCaptureService = screenCaptureService;
            _screenCropService = screenCropService;
            _paddleOcr = paddleOcr;
            _windowService = windowService;
        }


        [HttpPost("Start")]
        public async Task StartCareerTest()
        {
           
        }
    }
}
