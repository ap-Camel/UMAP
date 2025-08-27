namespace Umap.Api.Models
{
    public class TraineeStats
    {
        public int Speed { get; set; }
        public int Stamina  { get; set; }
        public int Power { get; set; }
        public int Gut { get; set; }
        public int Wit { get; set; }

        public int LegacySpeed { get; set; }
        public int LegacyStamina { get; set; }
        public int LegacyPower { get; set; }
        public int LegacyGut { get; set; }
        public int LegacyWit { get; set; }

        public float SpeedGrowth { get; set; }
        public float StaminaGrowth { get; set; }
        public float PowerGrowth { get; set; }
        public float GutGrowth { get; set; }
        public float WitGrowth { get; set; }

        public List<TraineeAptitude> TrackApptitudes { get; set; }
        public List<TraineeAptitude> DistanceApptitudes { get; set; }
        public List<TraineeAptitude> StyleApptitude {  get; set; }

    }
}
