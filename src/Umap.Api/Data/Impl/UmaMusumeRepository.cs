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

        public async Task<IEnumerable<RaceInfo>> GetAllRacesByTurn(int turnNumber)
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
    }
}
