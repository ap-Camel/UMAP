using Emgu.CV;
using System.Drawing;
using System.Drawing.Imaging;
using Umap.Api.Models;
using Umap.Api.Utilities;

namespace Umap.Api.Services.impl
{
    public class CareerService : ICareerService
    {
        private readonly IScreenCaptureService _screenCaptureService;
        private readonly IScreenCropService _screenCropService;
        private readonly IPaddleOcr _paddleOcr;
        private readonly IWindowService _windowService;

        private readonly List<CareerInfo> _turnHistory;
        private readonly Dictionary<Regions, RegionInfo> _regions;

        public CareerService(IScreenCaptureService screenCaptureService, IScreenCropService screenCropService, IPaddleOcr paddleOcr, IWindowService windowService)
        {
            _screenCaptureService = screenCaptureService;
            _screenCropService = screenCropService;
            _paddleOcr = paddleOcr;

            _turnHistory = new List<CareerInfo>();
            _regions = ScreenCropRegions.RegionsList.ToDictionary(x => x.region, x => x);
            _windowService = windowService;
        }


        public async Task ExecuteCareer(CareerInfo carrer)
        {

        }

        // maybe turn in own service without context of the outside? just recieving history values?
        public async Task ExecuteTurn()
        {
            //TODO: update to bring window to the front in the correct sequence.
            _windowService.GetWindowCoordinates();
            var windowScreenShot = _screenCaptureService.CaptureActiveWindow();
            var screenTitleImage = _screenCropService.CropRegion(windowScreenShot, _regions[Regions.CURRENT_SCREEN_INDICATOR]);

            var currentScreen = ReadScreenType(screenTitleImage);
            if(currentScreen == ScreenTitles.CAREER)
            {
                //TODO: add code for collecting the data, and then moving to the training, skills, or race sections.
                return;
            }

            if(currentScreen == ScreenTitles.TRAINING)
            {
                //TODO: maybe this will contain code for choosing which training to choose? or decide based on the risks to turn back and either rest or race?
                return;
            }

            if(currentScreen == ScreenTitles.EVENT)
            {
                //TODO: add code here to decide whoch event optionn to choose.
                return;
            }

            if(currentScreen == ScreenTitles.RACE_LIST)
            {
                // is this needed? turn will never start from race list.
                return;
            }

            if(currentScreen == ScreenTitles.CAREER_RACE_DAY)
            {
                //TODO: perform clicking to go to race list screen and then do the racing.
                return;
            }

            if(currentScreen == ScreenTitles.LEARN)
            {
                // is this needed? turn will never start from skills screen.
                return;
            }

            //TODO: decide what to do here, if it's unknown, do some checks to figure out why it's unknow?
        }

        private ScreenTitles ReadScreenType(Bitmap image)
        {
            //image.ConvertFormat(PixelFormat.Format8bppIndexed);
            //CvInvoke.Imshow("image", image.ToMat());
            //CvInvoke.WaitKey();
            var screenTitle = _paddleOcr.ReadTextFromImage(image);

            if (screenTitle.Contains("Career"))
            {
                //TODO: also read if currently and event is hapenning.                
                return ScreenTitles.CAREER;
            }

            if (screenTitle.Contains("training"))
                return ScreenTitles.TRAINING;

            if (screenTitle.Contains("list"))
                return ScreenTitles.RACE_LIST;

            if (screenTitle.Contains("learn"))
                return ScreenTitles.LEARN;

            //TODO: do check if it's racing screen, otherwise return unknow.
            return ScreenTitles.RACING;
        }
    }
}
