namespace Umap.Api.Models
{
    public class Skill
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public SkillType Type { get; set; }
        public bool FromEvent { get; set; }
    }
}
