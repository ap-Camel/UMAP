using Dapper;
using MySql.Data.MySqlClient;
using Umap.Api.Models.Database;

namespace Umap.Api.Data.Impl
{
    public class UmaMusumeRepository : IUmaMusumeRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public UmaMusumeRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<RaceInfo>> GetAllRacesByTurnNumber(int turnNumber)
        {
            using(var connection = _connectionFactory.Create<MySqlConnection>())
            {
                await connection.OpenAsync();
                var list = await connection.QueryAsync<RaceInfo>(
                    """
                    SELECT 
                    	smp.id AS single_mode_program_id,
                    	ri.id AS race_instance_id,
                    	r.id AS race_id,
                    	tdr.`text` AS race_name,
                    	tdi.`text` AS race_instance_name,
                    	r.grade,
                    	rcs.`distance`,
                    	rcs.ground,
                    	rcs.`inout`,
                    	rcs.turn AS direction,
                    	smfc.fan_count,
                    	smp.need_fan_count,
                    	smp.race_permission,
                    	smp.filly_only_flag 
                    FROM single_mode_program AS smp 
                    JOIN single_mode_turn AS smt ON smp.`month` = smt.`month` AND smp.half = smt.half
                    JOIN race_instance AS ri ON ri.id = smp.race_instance_id
                    JOIN race AS r ON r.id = ri.race_id
                    JOIN race_course_set AS rcs ON rcs.id = r.course_set
                    LEFT JOIN single_mode_fan_count AS smfc ON smfc.fan_set_id = smp.fan_set_id AND smfc.`order` = 1
                    LEFT JOIN text_data AS tdr ON tdr.`index` = r.id AND tdr.category = 32
                    LEFT JOIN text_data AS tdi ON tdi.`index` = ri.id AND tdi.category = 28
                    WHERE smp.base_program_id = 0 AND smt.turn_set_id = 1
                    AND smt.turn = @turnNumber
                    ORDER BY r.grade ASC, smp.need_fan_count, r.id;                    
                    """, new { turnNumber }
                    );

                return list;
            }
        }

        public async Task<IEnumerable<RequiredRaceInfo>> GetRequiredRacesByCharacterId(int charaId)
        {
            using (var connection = _connectionFactory.Create<MySqlConnection>())
            {
                await connection.OpenAsync();
                var list = await connection.QueryAsync<RequiredRaceInfo>(
                    """
                    SELECT
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
                    FROM single_mode_route AS smr 
                    join single_mode_route_race AS smrr ON smrr.race_set_id = smr.race_set_id
                    JOIN single_mode_program AS smp ON smp.id = smrr.condition_id
                    JOIN race_instance AS ri ON ri.id = smp.race_instance_id
                    LEFT JOIN text_data AS tdr ON tdr.`index` = ri.race_id AND tdr.category = 32
                    JOIN race AS r ON r.id = ri.race_id
                    LEFT JOIN text_data AS tdc ON tdc.`index` = smr.chara_id AND tdc.category = 6
                    WHERE smr.chara_id = @charaId                
                    """, new { charaId }
                    );

                return list;
            }
        }

        public async Task<IEnumerable<RequiredFanGoalInfo>> GetAllCharacterRequiredFanGoals()
        {
            using (var connection = _connectionFactory.Create<MySqlConnection>())
            {
                await connection.OpenAsync();
                var list = await connection.QueryAsync<RequiredFanGoalInfo>(
                    """
                    SELECT
                      smrr.turn,
                      smrr.condition_value_1 AS required_fans,
                      smr.chara_id,
                      td.`text` AS chara_name
                    FROM single_mode_route AS smr
                    JOIN single_mode_route_race AS smrr ON smrr.race_set_id = smr.race_set_id 
                    JOIN text_data AS td ON td.`index` = smr.chara_id AND td.category = 6
                    WHERE smrr.condition_type = 3             
                    """
                    );

                return list;
            }
        }

        public async Task<IEnumerable<RequiredFanGoalInfo>> GetRequiredFanGoalsByCharacterId(int charaId)
        {
            return (await GetAllCharacterRequiredFanGoals()).Where(x => x.chara_id == charaId);
        }

        public async Task<IEnumerable<CharaRarityInfo>> GetCardRarityInfoByCardId(int cardId)
        {
            using (var connection = _connectionFactory.Create<MySqlConnection>())
            {
                await connection.OpenAsync();
                var list = await connection.QueryAsync<CharaRarityInfo>(
                    """
                    SELECT 
                    	td.`text` AS card_name,
                    	tdc.`text` AS chara_name,
                    	cd.chara_id,
                    	crd.*
                    FROM card_data AS cd
                    join card_rarity_data AS crd ON crd.card_id = cd.id
                    LEFT JOIN text_data AS td ON td.`index` = crd.card_id AND td.category = 5
                    LEFT JOIN text_data AS tdc ON tdc.`index` = cd.chara_id AND tdc.category = 6
                    WHERE crd.card_id = @cardId
                    """, new { cardId }
                    );

                return list;
            }
        }

        public async Task<IEnumerable<CharaRarityInfo>> GetCardRarityInfoByCharaId(int charaId)
        {
            using (var connection = _connectionFactory.Create<MySqlConnection>())
            {
                await connection.OpenAsync();
                var list = await connection.QueryAsync<CharaRarityInfo>(
                    """
                    SELECT 
                    	td.`text` AS card_name,
                    	tdc.`text` AS chara_name,
                    	cd.chara_id,
                    	crd.*
                    FROM card_data AS cd
                    join card_rarity_data AS crd ON crd.card_id = cd.id
                    LEFT JOIN text_data AS td ON td.`index` = crd.card_id AND td.category = 5
                    LEFT JOIN text_data AS tdc ON tdc.`index` = cd.chara_id AND tdc.category = 6
                    WHERE cd.chara_id = @charaId  
                    """, new { charaId }
                    );

                return list;
            }
        }
    }
}
