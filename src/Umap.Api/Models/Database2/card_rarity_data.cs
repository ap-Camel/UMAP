namespace Umap.Api.Models.Database2
{
    public class card_rarity_data
    {
		public int Id { get; set; }
        public int card_id { get; set; }
        public int rarity { get; set; }
        public int race_dress_id { get; set; }
        public int skill_set { get; set; }
        public int speed { get; set; }
        public int stamina { get; set; }
        public int pow { get; set; }
        public int guts { get; set; }
        public int wiz { get; set; }
        public int max_speed { get; set; }
        public int max_stamina { get; set; }
        public int max_pow { get; set; }
        public int max_guts { get; set; }
        public int max_wiz { get; set; }
        public int proper_distance_short { get; set; }
        public int proper_distance_mile { get; set; }
        public int proper_distance_middle { get; set; }
        public int proper_distance_long { get; set; }
        public int proper_running_style_nige { get; set; }
        public int proper_running_style_senko { get; set; }
        public int proper_running_style_sashi { get; set; }
        public int proper_running_style_oikomi { get; set; }
        public int proper_ground_turf { get; set; }
        public int proper_ground_dirt { get; set; }
        public int get_dress_id_1 { get; set; }
        public int get_dress_id_2 { get; set; }

    }
}

/*
 * 
 *
 CREATE TABLE `card_rarity_data` (
	`id` INT NOT NULL,
	`card_id` INT NOT NULL,
	`rarity` INT NOT NULL,
	`race_dress_id` INT NOT NULL,
	`skill_set` INT NOT NULL,
	`speed` INT NOT NULL,
	`stamina` INT NOT NULL,
	`pow` INT NOT NULL,
	`guts` INT NOT NULL,
	`wiz` INT NOT NULL,
	`max_speed` INT NOT NULL,
	`max_stamina` INT NOT NULL,
	`max_pow` INT NOT NULL,
	`max_guts` INT NOT NULL,
	`max_wiz` INT NOT NULL,
	`proper_distance_short` INT NOT NULL,
	`proper_distance_mile` INT NOT NULL,
	`proper_distance_middle` INT NOT NULL,
	`proper_distance_long` INT NOT NULL,
	`proper_running_style_nige` INT NOT NULL,
	`proper_running_style_senko` INT NOT NULL,
	`proper_running_style_sashi` INT NOT NULL,
	`proper_running_style_oikomi` INT NOT NULL,
	`proper_ground_turf` INT NOT NULL,
	`proper_ground_dirt` INT NOT NULL,
	`get_dress_id_1` INT NOT NULL,
	`get_dress_id_2` INT NOT NULL,
	PRIMARY KEY (`id`) USING BTREE,
	UNIQUE INDEX `card_rarity_data_0_card_id_1_rarity` (`card_id`, `rarity`) USING BTREE,
	INDEX `card_rarity_data_0_card_id` (`card_id`) USING BTREE
)
COLLATE='utf8mb4_0900_ai_ci'
ENGINE=InnoDB
;

 * 
 * 
 */
