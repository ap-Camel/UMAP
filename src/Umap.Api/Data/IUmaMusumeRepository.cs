using Umap.Api.Models.Database;

namespace Umap.Api.Data
{
    public interface IUmaMusumeRepository
    {
        Task<IEnumerable<RaceInfo>> GetAllRacesByTurn(int turnNumber);
    }
}
