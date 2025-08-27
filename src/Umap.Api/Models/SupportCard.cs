namespace Umap.Api.Models
{
    public class SupportCard
    {
        public string Name { get; set; }
        public TrainingType TrainingType { get; set; }
        public List<Skill> Skills { get; set; }
        public int Level { get; set; }
        public int SpecialtyPriority { get; set; }
    }
}
