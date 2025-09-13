namespace Umap.Api.Models.Database
{
    public class RaceInfo
    {
        public int single_mode_program_id {  get; set; }
        public int race_instance_id { get; set; }
        public int race_id { get; set; }
        public string race_name { get; set; }
        public string race_instance_name { get; set; }
        public float grade { get; set; }
        public float distance { get; set; }
        public float ground { get; set; }
        public float inout { get; set; }
        public float direction {  get; set; }
        public float fan_count { get; set; }
        public float need_fan_count { get; set; }
        public float race_permission { get; set; }
        public float filly_only_flag { get; set; }
    }
}
