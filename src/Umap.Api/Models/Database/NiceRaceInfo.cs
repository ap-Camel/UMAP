namespace Umap.Api.Models.Database
{
    public class NiceRaceInfo
    {
        public int RaceInstanceId { get; set; }
        public int RaceId { get; set; }
        public long? Group { get; set; }
        public long? Grade { get; set; }
        public long? CourseSet { get; set; }
        public long? EntryNum { get; set; }
        public long? RaceTrackId { get; set; }
        public long? Distance { get; set; }
        public long? Terrain { get; set; }
        public long? Course { get; set; }
        public long? Direction { get; set; }
        public long? Month { get; set; }
        public long? Half { get; set; }
        public long? RequiredFans { get; set; }
        public long? Class { get; set; }
        public long? FanCount { get; set; }
    }
}


/*
 * {
    "raceInstanceId": 100101,
    "raceId": 1001,
    "group": 1,
    "grade": 100,
    "courseSet": 10611,
    "entryNum": 16,
    "raceTrackId": 10006,
    "distance": 1600,
    "terrain": 2,
    "course": 1,
    "direction": 2,
    "month": 2,
    "half": 2,
    "requiredFans": 12000,
    "class": 4,
    "fanCount": 10000
  },
 */