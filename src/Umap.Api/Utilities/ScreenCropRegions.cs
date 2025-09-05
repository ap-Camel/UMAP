using Umap.Api.Models;

namespace Umap.Api.Utilities
{
    public class ScreenCropRegions
    {
        public static List<RegionInfo> RegionsList = new List<RegionInfo>
        {
            new RegionInfo { x = 186, y = 53, width = 293, height = 25, Screen = ScreenTitles.CAREER, region = Regions.CURRENT_SCREEN_INDICATOR },
            new RegionInfo { x = 320, y = 88, width = 240, height = 26, Screen = ScreenTitles.CAREER, region = Regions.TURN_NAME },
            new RegionInfo { x = 322, y = 145, width = 138, height = 63, Screen = ScreenTitles.CAREER, region = Regions.TURNS_LEFT_UNTIL_GOAL },
            new RegionInfo { x = 564, y = 120, width = 500, height = 32, Screen = ScreenTitles.CAREER, region = Regions.GOAL_NAME },
            new RegionInfo { x = 550, y = 209, width = 289, height = 10, Screen = ScreenTitles.CAREER, region = Regions.ENERGY_BAR },
            new RegionInfo { x = 912, y = 197, width = 70, height = 33, Screen = ScreenTitles.CAREER, region = Regions.MOOD_INDICATOR },
            new RegionInfo { x = 336, y = 909, width = 592, height = 86, Screen = ScreenTitles.CAREER, region = Regions.STATS_TABLE },
            new RegionInfo { x = 341, y = 1024, width = 189, height = 120, Screen = ScreenTitles.CAREER, region = Regions.REST_BUTTON },
            new RegionInfo { x = 544, y = 1024, width = 287, height = 120, Screen = ScreenTitles.CAREER, region = Regions.TRAINING_BUTTON },
            new RegionInfo { x = 846, y = 1024, width = 186, height = 120, Screen = ScreenTitles.CAREER, region = Regions.SKILLS_BUTTON },
            new RegionInfo { x = 393, y = 1170, width = 186, height = 120, Screen = ScreenTitles.CAREER, region = Regions.INFIRMARY_BUTTON },
            new RegionInfo { x = 594, y = 1170, width = 186, height = 120, Screen = ScreenTitles.CAREER, region = Regions.RECREATION_BUTTON },
            new RegionInfo { x = 795, y = 1170, width = 186, height = 120, Screen = ScreenTitles.CAREER, region = Regions.RACES_BUTTON },

            new RegionInfo { x = 210, y = 1294, width = 124, height = 79, Screen = ScreenTitles.TRAINING, region = Regions.BACK_BUTTON },
            new RegionInfo { x = 357, y = 1110, width = 130, height = 130, Screen = ScreenTitles.TRAINING, region = Regions.SPEED_TRAINING_BUTTON_NOT_SELECTED },
            new RegionInfo { x = 491, y = 1110, width = 130, height = 130, Screen = ScreenTitles.TRAINING, region = Regions.STAMINA_TRAINING_BUTTON_NOT_SELECTED },
            new RegionInfo { x = 624, y = 1110, width = 130, height = 130, Screen = ScreenTitles.TRAINING, region = Regions.POWER_TRAINING_BUTTON_NOT_SELECTED },
            new RegionInfo { x = 752, y = 1000, width = 140, height = 185, Screen = ScreenTitles.TRAINING, region = Regions.GUT_TRAINING_BUTTON_NOT_SELECTED },
            new RegionInfo { x = 892, y = 1110, width = 130, height = 130, Screen = ScreenTitles.TRAINING, region = Regions.WIT_TRAINING_BUTTON_NOT_SELECTED },

            new RegionInfo { x = 351, y = 1000, width = 140, height = 185, Screen = ScreenTitles.TRAINING, region = Regions.SPEED_TRAINING_BUTTON_IS_SELECTED },
            new RegionInfo { x = 485, y = 1000, width = 140, height = 185, Screen = ScreenTitles.TRAINING, region = Regions.STAMINA_TRAINING_BUTTON_IS_SELECTED },
            new RegionInfo { x = 618, y = 1000, width = 140, height = 185, Screen = ScreenTitles.TRAINING, region = Regions.POWER_TRAINING_BUTTON_IS_SELECTED },
            new RegionInfo { x = 752, y = 1000, width = 140, height = 185, Screen = ScreenTitles.TRAINING, region = Regions.GUT_TRAINING_BUTTON_IS_SELECTED },
            new RegionInfo { x = 886, y = 1000, width = 140, height = 185, Screen = ScreenTitles.TRAINING, region = Regions.WIT_TRAINING_BUTTON_IS_SELECTED },
            new RegionInfo { x = 1062, y = 250, width = 96, height = 101, Screen = ScreenTitles.TRAINING, region = Regions.FRIENDSHIP_TRAINING_FIRST },
            new RegionInfo { x = 1062, y = 490, width = 96, height = 111, Screen = ScreenTitles.TRAINING, region = Regions.FRIENDSHIP_TRAINING_THIRD },
            new RegionInfo { x = 1052, y = 200, width = 156, height = 650, Screen = ScreenTitles.TRAINING, region = Regions.FRIENDSHIP_TRAINING_ALL },
            new RegionInfo { x = 363, y = 181, width = 208, height = 272, Screen = ScreenTitles.TRAINING, region = Regions.FRIENDSHIP_PARTNER_CARD_IMAGE },
            new RegionInfo { x = 595, y = 170, width = 464, height = 70, Screen = ScreenTitles.TRAINING, region = Regions.FRIENDSHIP_PARTNER_CARD_TITLE },
            new RegionInfo { x = 598, y = 242, width = 147, height = 35, Screen = ScreenTitles.TRAINING, region = Regions.FRIENDSHIP_PARTNER_CARD_LEVEL },

            new RegionInfo { x = 300, y = 285, width = 463, height = 63, Screen = ScreenTitles.EVENT, region = Regions.EVENT_NAME },
            new RegionInfo { x = 336, y = 789, width = 694, height = 103, Screen = ScreenTitles.EVENT, region = Regions.EVENT_CHOICE_FIRST_OF_TWO },
            new RegionInfo { x = 336, y = 927, width = 694, height = 103, Screen = ScreenTitles.EVENT, region = Regions.EVENT_CHOICE_SECOND_OF_TWO },

            new RegionInfo { x = 338, y = 855, width = 120, height = 53, Screen = ScreenTitles.TRAINING, region = Regions.TRAINING_POINTS_ADDED_SPEED },
            new RegionInfo { x = 457, y = 855, width = 120, height = 53, Screen = ScreenTitles.TRAINING, region = Regions.TRAINING_POINTS_ADDED_STAMINA },
            new RegionInfo { x = 576, y = 855, width = 120, height = 53, Screen = ScreenTitles.TRAINING, region = Regions.TRAINING_POINTS_ADDED_POWER },
            new RegionInfo { x = 693, y = 855, width = 120, height = 53, Screen = ScreenTitles.TRAINING, region = Regions.TRAINING_POINTS_ADDED_GUT },
            new RegionInfo { x = 811, y = 855, width = 120, height = 53, Screen = ScreenTitles.TRAINING, region = Regions.TRAINING_POINTS_ADDED_WIT },
            new RegionInfo { x = 937, y = 855, width = 120, height = 53, Screen = ScreenTitles.TRAINING, region = Regions.TRAINING_POINTS_ADDED_SKILLS },

            new RegionInfo { x = 705, y = 1074, width = 290, height = 163, Screen = ScreenTitles.CAREER_RACE_DAY, region = Regions.RACE_DAY_RACE_BUTTON },
            new RegionInfo { x = 386, y = 1118, width = 289, height = 117, Screen = ScreenTitles.CAREER_RACE_DAY, region = Regions.RACE_DAY_SKILLS_BUTTON },

            new RegionInfo { x = 564, y = 120, width = 386, height = 64, Screen = ScreenTitles.RACE_LIST, region = Regions.RACE_LIST_GOAL_NAME },
            new RegionInfo { x = 647, y = 498, width = 403, height = 168, Screen = ScreenTitles.RACE_LIST, region = Regions.RACE_LIST_WEATHER_INFO },
            new RegionInfo { x = 551, y = 800, width = 480, height = 43, Screen = ScreenTitles.RACE_LIST, region = Regions.RACE_LIST_FIRST_RACE_INFO },
            new RegionInfo { x = 595, y = 866, width = 235, height = 34, Screen = ScreenTitles.RACE_LIST, region = Regions.RACE_LIST_FIRST_RACE_FANS_GAIN },
            new RegionInfo { x = 911, y = 850, width = 108, height = 28, Screen = ScreenTitles.RACE_LIST, region = Regions.RACE_LIST_FIRST_RACE_COMPATEBILITY01 },
            new RegionInfo { x = 911, y = 882, width = 108, height = 28, Screen = ScreenTitles.RACE_LIST, region = Regions.RACE_LIST_FIRST_RACE_COMPATEBILITY02 },
            new RegionInfo { x = 700, y = 1138, width = 298, height = 84, Screen = ScreenTitles.RACE_LIST, region = Regions.RACE_LIST_RACE_BUTTON },
            new RegionInfo { x = 377, y = 1138, width = 298, height = 84, Screen = ScreenTitles.RACE_LIST, region = Regions.RACE_LIST_PREDICTIONS_BUTTON },

            new RegionInfo { x = 706, y = 966, width = 298, height = 84, Screen = ScreenTitles.RACE_LIST, region = Regions.RACE_LIST_RACE_DETAILS_RACE_BUTTON },
            new RegionInfo { x = 375, y = 966, width = 298, height = 84, Screen = ScreenTitles.RACE_LIST, region = Regions.RACE_LIST_RACE_DETAILS_CANCEL_BUTTON },
            new RegionInfo { x = 327, y = 735, width = 724, height = 94, Screen = ScreenTitles.RACE_LIST, region = Regions.RACE_LIST_RACE_DETAILS_RACE_INFORMATION },

            new RegionInfo { x = 476, y = 1234, width = 202, height = 80, Screen = ScreenTitles.RACING, region = Regions.RACING_RESULTS_BUTTON },
            new RegionInfo { x = 698, y = 1234, width = 202, height = 80, Screen = ScreenTitles.RACING, region = Regions.RACING_RACE_BUTTON },
            new RegionInfo { x = 805, y = 230, width = 357, height = 261, Screen = ScreenTitles.RACING, region = Regions.RACING_STATS_PANEL },
            new RegionInfo { x = 806, y = 506, width = 357, height = 117, Screen = ScreenTitles.RACING, region = Regions.RACING_APTITUDE_PANEL },
            new RegionInfo { x = 806, y = 637, width = 357, height = 64, Screen = ScreenTitles.RACING, region = Regions.RACING_MOOD_PANEL },
            new RegionInfo { x = 806, y = 714, width = 357, height = 141, Screen = ScreenTitles.RACING, region = Regions.RACING_STRATEGY_PANEL },

            new RegionInfo { x = 444, y = 1185, width = 496, height = 76, Screen = ScreenTitles.RACING, region = Regions.RACING_FINISH_TAP_TO_SKIP },
            new RegionInfo { x = 540, y = 1239, width = 298, height = 84, Screen = ScreenTitles.RACING, region = Regions.RACING_FINISH_FIRST_NEXT_BUTTON },
            new RegionInfo { x = 704, y = 1230, width = 288, height = 117, Screen = ScreenTitles.RACING, region = Regions.RACING_FINISH_SECOND_NEXT_BUTTON },



            //new ObjectCoordinates { x = 387, y = 939, width = 69, height = 31, name = "currentSpeedPoints", screenSection = ScreenSection.training },
            //new ObjectCoordinates { x = 505, y = 939, width = 69, height = 31, name = "currentStaminaPoints", screenSection = ScreenSection.training },
            //new ObjectCoordinates { x = 622, y = 939, width = 69, height = 31, name = "currentPowerPoints", screenSection = ScreenSection.training },
            //new ObjectCoordinates { x = 740, y = 939, width = 69, height = 31, name = "currentGutPoints", screenSection = ScreenSection.training },
            //new ObjectCoordinates { x = 855, y = 939, width = 69, height = 31, name = "currentWitPoints", screenSection = ScreenSection.training },
            //new ObjectCoordinates { x = 938, y = 939, width = 100, height = 52, name = "currentSkillPoints", screenSection = ScreenSection.training },
        };




    }
}
