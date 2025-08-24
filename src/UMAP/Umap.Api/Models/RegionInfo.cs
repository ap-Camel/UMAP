using Umap.Api.Utilities;

namespace Umap.Api.Models
{
    public class RegionInfo
    {
        public Regions region { get; set; }
        public ScreenTitles Screen { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
}
