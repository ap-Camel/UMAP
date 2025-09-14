using Umap.Api.Models.Database;

namespace Umap.Api.Data
{
    public interface IUmaMusumeRepository
    {
        Task<IEnumerable<RaceInfo>> GetAllRacesByTurnNumber(int turnNumber);
        Task<IEnumerable<RequiredRaceInfo>> GetRequiredRacesByCharacterId(int charaId);
        Task<IEnumerable<RequiredFanGoalInfo>> GetAllCharacterRequiredFanGoals();
        Task<IEnumerable<RequiredFanGoalInfo>> GetRequiredFanGoalsByCharacterId(int charaId);
        Task<IEnumerable<CharaRarityInfo>> GetCardRarityInfoByCardId(int cardId);
        Task<IEnumerable<CharaRarityInfo>> GetCardRarityInfoByCharaId(int charaId);
    }
}
