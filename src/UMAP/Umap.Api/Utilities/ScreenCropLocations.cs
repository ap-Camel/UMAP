using Umap.Api.Models;
using static Umap.Api.Services.impl.ScreenCropService;

namespace Umap.Api.Utilities
{
    public class ScreenCropLocations
    {
        public static Dictionary<string, LocationInfo> LocationsDictionary = new Dictionary<string, LocationInfo>
        {
            { 
                CareerScreenLocations.mainSectionIndicator.ToString(), 
                new LocationInfo { Name = CareerScreenLocations.mainSectionIndicator.ToString(), Screen = CurrentScreen.Career, x = 186, y = 53, width = 350, height = 27 } 
            },
            {
                CareerScreenLocations.turnName.ToString(),
                new LocationInfo { Name = CareerScreenLocations.turnName.ToString(), Screen = CurrentScreen.Career, x = 320, y = 88, width = 240, height = 26 }
            },
            {
                CareerScreenLocations.numberOfTurnsLeftUntilGoal.ToString(),
                new LocationInfo { Name = CareerScreenLocations.numberOfTurnsLeftUntilGoal.ToString(), Screen = CurrentScreen.Career, x = 322, y = 145, width = 140, height = 65 }
            },
            {
                CareerScreenLocations.nameOfGoal.ToString(),
                new LocationInfo { Name = CareerScreenLocations.nameOfGoal.ToString(), Screen = CurrentScreen.Career, x = 564, y = 120, width = 500, height = 32 }
            },
            {
                CareerScreenLocations.energyBar.ToString(),
                new LocationInfo { Name = CareerScreenLocations.energyBar.ToString(), Screen = CurrentScreen.Career, x = 551, y = 213, width = 298, height = 11 }
            },
        };


        public static List<ObjectCoordinates> list = new List<ObjectCoordinates>
        {
            new ObjectCoordinates { x = 186, y = 53, width = 350, height = 27, name = "mainSectionIndicator", screenSection = ScreenSection.career },
            new ObjectCoordinates { x = 320, y = 88, width = 240, height = 26, name = "turnName", screenSection = ScreenSection.career },
            new ObjectCoordinates { x = 322, y = 145, width = 140, height = 65, name = "numberOfTurnsLeftUntilGoal", screenSection = ScreenSection.career },
            new ObjectCoordinates { x = 564, y = 120, width = 500, height = 32, name = "nameOfGoal", screenSection = ScreenSection.career },
            new ObjectCoordinates { x = 551, y = 213, width = 298, height = 11, name = "energyBard", screenSection = ScreenSection.career },
            //new ObjectCoordinates { x = 1069, y = 341, width = 81, height = 5, name = "CharacterTraining01EnergyBard", screenSection = ScreenSection.career },
            //new ObjectCoordinates { x = 912, y = 197, width = 70, height = 33, name = "moodIndicator", screenSection = ScreenSection.career }, // size is fixed, doesn't seem to be changing based on mood name.
            //new ObjectCoordinates { x = 336, y = 909, width = 592, height = 86, name = "statsTable", screenSection = ScreenSection.career },
            //new ObjectCoordinates { x = 341, y = 1024, width = 189, height = 120, name = "restingButton", screenSection = ScreenSection.career },
            //new ObjectCoordinates { x = 544, y = 1024, width = 287, height = 120, name = "trainingButton", screenSection = ScreenSection.career },
            //new ObjectCoordinates { x = 846, y = 1024, width = 186, height = 120, name = "skillsButton", screenSection = ScreenSection.career },
            //new ObjectCoordinates { x = 393, y = 1170, width = 186, height = 120, name = "infermaryButton", screenSection = ScreenSection.career },
            //new ObjectCoordinates { x = 594, y = 1170, width = 186, height = 120, name = "recreationButton", screenSection = ScreenSection.career },
            //new ObjectCoordinates { x = 795, y = 1170, width = 186, height = 120, name = "racesButton", screenSection = ScreenSection.career },

            //new ObjectCoordinates { x = 210, y = 1294, width = 124, height = 79, name = "backButton", screenSection = ScreenSection.training },
            //new ObjectCoordinates { x = 357, y = 1110, width = 130, height = 130, name = "speedTrainingNotSelectedButton", screenSection = ScreenSection.training },
            //new ObjectCoordinates { x = 491, y = 1110, width = 130, height = 130, name = "staminaTrainingNotSelectedButton", screenSection = ScreenSection.training },
            //new ObjectCoordinates { x = 624, y = 1110, width = 130, height = 130, name = "strengthTrainingNotSelectedButton", screenSection = ScreenSection.training },
            //new ObjectCoordinates { x = 758, y = 1110, width = 130, height = 130, name = "gutsTrainingNotSelectedButton", screenSection = ScreenSection.training },
            //new ObjectCoordinates { x = 892, y = 1110, width = 130, height = 130, name = "witsTrainingNotSelectedButton", screenSection = ScreenSection.training },

            //new ObjectCoordinates { x = 351, y = 1000, width = 140, height = 185, name = "speedTrainingIsSelectedButton", screenSection = ScreenSection.training },
            //new ObjectCoordinates { x = 485, y = 1000, width = 140, height = 185, name = "staminaTrainingIsSelectedButton", screenSection = ScreenSection.training },
            //new ObjectCoordinates { x = 618, y = 1000, width = 140, height = 185, name = "strengthTrainingIsSelectedButton", screenSection = ScreenSection.training },
            //new ObjectCoordinates { x = 752, y = 1000, width = 140, height = 185, name = "gutsTrainingIsSelectedButton", screenSection = ScreenSection.training },
            //new ObjectCoordinates { x = 886, y = 1000, width = 140, height = 185, name = "witsTrainingIsSelectedButton", screenSection = ScreenSection.training },

            //new ObjectCoordinates { x = 1062, y = 250, width = 96, height = 101, name = "firstFriendshipTraining", screenSection = ScreenSection.training },
            //new ObjectCoordinates { x = 1062, y = 490, width = 96, height = 111, name = "thirdFriendshipTraining", screenSection = ScreenSection.training },
            //new ObjectCoordinates { x = 1052, y = 200, width = 156, height = 650, name = "allFriendshipTraining", screenSection = ScreenSection.training },
            //new ObjectCoordinates { x = 363, y = 181, width = 208, height = 272, name = "friendshipPartnerCardImage", screenSection = ScreenSection.training },
            //new ObjectCoordinates { x = 595, y = 170, width = 464, height = 70, name = "friendshipPartnerCardTitles", screenSection = ScreenSection.training },
            //new ObjectCoordinates { x = 598, y = 242, width = 147, height = 35, name = "friendshipPartnerCardLevel", screenSection = ScreenSection.training },

            //new ObjectCoordinates { x = 300, y = 285, width = 463, height = 63, name = "eventName", screenSection = ScreenSection.other },
            //new ObjectCoordinates { x = 336, y = 789, width = 694, height = 103, name = "twoChoicesFirstChoice", screenSection = ScreenSection.eventt },
            //new ObjectCoordinates { x = 336, y = 927, width = 694, height = 103, name = "twoChoicesSecondChoice", screenSection = ScreenSection.eventt },

            //new ObjectCoordinates { x = 338, y = 855, width = 120, height = 55, name = "pointsAddedSpeed", screenSection = ScreenSection.training },
            //new ObjectCoordinates { x = 457, y = 855, width = 120, height = 55, name = "pointsAddedStamina", screenSection = ScreenSection.training },
            //new ObjectCoordinates { x = 576, y = 855, width = 120, height = 55, name = "pointsAddedPower", screenSection = ScreenSection.training },
            //new ObjectCoordinates { x = 693, y = 855, width = 120, height = 55, name = "pointsAddedGut", screenSection = ScreenSection.training },
            //new ObjectCoordinates { x = 811, y = 855, width = 120, height = 55, name = "pointsAddedWit", screenSection = ScreenSection.training },
            //new ObjectCoordinates { x = 937, y = 855, width = 120, height = 55, name = "pointsAddedSkill", screenSection = ScreenSection.training },

            //new ObjectCoordinates { x = 705, y = 1074, width = 290, height = 163, name = "raceButtonOnRaceDay", screenSection = ScreenSection.other },
            //new ObjectCoordinates { x = 386, y = 1118, width = 289, height = 117, name = "raceButtonOnRaceDay", screenSection = ScreenSection.other },

            //new ObjectCoordinates { x = 564, y = 120, width = 386, height = 64, name = "nameOfGoalInRaceScreen", screenSection = ScreenSection.raceList },
            //new ObjectCoordinates { x = 647, y = 498, width = 403, height = 168, name = "raceInformationWeather", screenSection = ScreenSection.raceList },
            //new ObjectCoordinates { x = 343, y = 787, width = 687, height = 145, name = "firstRaceInRaceList", screenSection = ScreenSection.raceList },
            //new ObjectCoordinates { x = 551, y = 800, width = 480, height = 43, name = "firstRaceInRaceListNameInfo", screenSection = ScreenSection.raceList },
            //new ObjectCoordinates { x = 595, y = 866, width = 235, height = 34, name = "firstRaceInRaceListFansGain", screenSection = ScreenSection.raceList },
            //new ObjectCoordinates { x = 911, y = 850, width = 108, height = 28, name = "firstRaceInRaceListCompatebility01", screenSection = ScreenSection.raceList },
            //new ObjectCoordinates { x = 911, y = 882, width = 108, height = 28, name = "firstRaceInRaceListCompatebility02", screenSection = ScreenSection.raceList },
            //new ObjectCoordinates { x = 700, y = 1138, width = 298, height = 84, name = "raceListRaceButton", screenSection = ScreenSection.raceList },
            //new ObjectCoordinates { x = 377, y = 1138, width = 298, height = 84, name = "raceListPredictionsButton", screenSection = ScreenSection.raceList },

            //new ObjectCoordinates { x = 706, y = 966, width = 298, height = 84, name = "raceDetailRaceButton", screenSection = ScreenSection.raceList },
            //new ObjectCoordinates { x = 375, y = 966, width = 298, height = 84, name = "raceDetailCancelButton", screenSection = ScreenSection.raceList },
            //new ObjectCoordinates { x = 327, y = 735, width = 724, height = 94, name = "raceDetailRaceInformation", screenSection = ScreenSection.raceList },

            //new ObjectCoordinates { x = 476, y = 1234, width = 202, height = 80, name = "raceScreenResultsButton", screenSection = ScreenSection.racing },
            //new ObjectCoordinates { x = 698, y = 1234, width = 202, height = 80, name = "raceScreenRaceButton", screenSection = ScreenSection.racing },
            //new ObjectCoordinates { x = 805, y = 230, width = 357, height = 261, name = "raceScreenStatsPanel", screenSection = ScreenSection.racing },
            //new ObjectCoordinates { x = 806, y = 506, width = 357, height = 117, name = "raceScreenStatsPanel", screenSection = ScreenSection.racing },
            //new ObjectCoordinates { x = 806, y = 637, width = 357, height = 64, name = "raceScreenAptitudePanel", screenSection = ScreenSection.racing },
            //new ObjectCoordinates { x = 806, y = 714, width = 357, height = 141, name = "raceScreenStrategyPanel", screenSection = ScreenSection.racing },

            //new ObjectCoordinates { x = 444, y = 1185, width = 496, height = 76, name = "afterRaceFinishTapInstruction", screenSection = ScreenSection.racing },
            //new ObjectCoordinates { x = 540, y = 1239, width = 298, height = 84, name = "afterRaceFinishFirstNextButton", screenSection = ScreenSection.racing },
            //new ObjectCoordinates { x = 704, y = 1230, width = 288, height = 117, name = "afterRaceFinishSecondNextButton", screenSection = ScreenSection.racing },
        };
    }
}
