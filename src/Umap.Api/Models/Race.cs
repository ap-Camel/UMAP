namespace Umap.Api.Models
{
    public class Race
    {
        public int RaceId { get; set; }
        public string RaceName { get; set; }
        public Aptitude SurfaceType { get; set; }
        public Aptitude DistanceType { get; set; }
        public int FansGained { get; set; }
        public int FansRequired { get; set; }
        public int SkillPointsGained { get; set; }
        public int DistanceLength { get; set; }
        public int Grade { get; set; }
        public Spark Spark { get; set; }
    }
}
