using Umap.Api.Models.Database2;

namespace Umap.Api.Models.Database
{
    public class CharaRarityInfo : card_rarity_data
    {
        public int chara_id { get; set; }
        public string card_name { get; set; }
        public string chara_name { get; set; }
    }
}