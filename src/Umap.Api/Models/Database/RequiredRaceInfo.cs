namespace Umap.Api.Models.Database
{
    public class RequiredRaceInfo
    {
        public int race_set_id {  get; set; }
        public int chara_id { get; set; }
        public int condition_id { get; set; }
        public int single_mode_program_id { get; set; }
        public int race_instance_id { get; set; }
        public int race_id { get; set; }
        public int turn { get; set; }
        public string race_name { get; set; }
        public string chara_name { get; set; }
        public int place_required { get; set; }
        public int determine_race_flag { get; set; }
    }


    /*
     smrr.race_set_id,
     smr.chara_id, 
     smrr.condition_id,
     smp.id AS single_mode_program_id,
     smp.race_instance_id, 
     r.id AS race_id,
     smrr.turn,
     tdr.`text` AS race_name, 
     tdc.`text` AS chara_name, 
     smrr.condition_value_1 AS place_required,
     smrr.determine_race_flag
     * 
     * 
     */
}
