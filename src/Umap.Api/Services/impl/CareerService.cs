using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;
using System.Drawing.Imaging;
using Umap.Api.Data;
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
        private readonly IUmaMusumeRepository _repository;

        private readonly List<CareerInfo> _turnHistory;
        private readonly Dictionary<Regions, RegionInfo> _regions;

        public CareerService(IScreenCaptureService screenCaptureService, IScreenCropService screenCropService, 
            IPaddleOcr paddleOcr, IWindowService windowService/*, IUmaMusumeRepository repository*/)
        {
            _screenCaptureService = screenCaptureService;
            _screenCropService = screenCropService;
            _paddleOcr = paddleOcr;
            //_repository = repository;

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

            //TODO: maybe it's better to to think of it like to just confirm it's career screen, since beginning of a turn won't be in a different screen
            //      and if it's in a different screen that can be navigated back to career, then navigate back to career and start the turn there properly.
            var currentScreen = ReadScreenType(screenTitleImage);
            if(currentScreen == ScreenTitles.CAREER)
            {
                //TODO: add code for collecting the data, and then moving to the training, skills, or race sections.
                var energybarImage = _screenCropService.CropRegion(windowScreenShot, _regions[Regions.ENERGY_BAR]);
                var energyPercentage = GetEnergyPercentage(energybarImage);

                //NOTE: this not nrcessary for now, better to have each turn in db with race information and character's mandatory races.
                //var numberOfTurnsImage = _screenCropService.CropRegion(windowScreenShot, _regions[Regions.TURNS_LEFT_UNTIL_GOAL]);
                //var numberOfTurnsUntilGoal = GetNumberOfTurnsUntilGoal(numberOfTurnsImage);


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

        private double GetEnergyPercentage(Bitmap image)
        {
            var mat = image.ToMat();
            CvInvoke.FastNlMeansDenoisingColored(mat, mat, 10, 10, 7, 21);

            var canny = new Mat();
            CvInvoke.Canny(mat, canny, 150, 50);

            int h = canny.Rows, w = canny.Cols;
            using var vertKernel = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(1, Math.Max(3, h / 5)), new Point(-1, -1));
            using var edgesV = new Mat();
            CvInvoke.MorphologyEx(canny, edgesV, MorphOp.Open, vertKernel, new Point(-1, -1), 1, BorderType.Reflect, new MCvScalar());

            var nonZeroIndex = ReturnNonZeroIndex(edgesV);
            return nonZeroIndex == 0 ? 100 : Math.Round(nonZeroIndex * 1.0d / edgesV.Width, 2);
        }

        private int ReturnNonZeroIndex(Mat binary)
        {
            int h = binary.Rows, w = binary.Cols;

            // Simple and clear: scan one-column ROIs
            var index = 0;
            for (int x = 0; x < w; x++)
            {
                var col = new Mat(binary, new Rectangle(x, 0, 1, h));
                if (CvInvoke.CountNonZero(col) != 0)
                {
                    index = x;
                    break;
                }
            }
            return index;
        }

        private int GetNumberOfTurnsUntilGoal(Bitmap image)
        {
            var mat = image.ToMat();
            CvInvoke.Imshow("dddd", mat);
            CvInvoke.WaitKey();

            var text = _paddleOcr.ReadTextFromImage(image);


            return 0;
        }
    }
}
