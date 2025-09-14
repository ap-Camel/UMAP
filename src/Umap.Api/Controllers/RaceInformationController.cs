using Microsoft.AspNetCore.Mvc;
using Umap.Api.Data;

namespace Umap.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RaceInformationController : ControllerBase
    {
        private readonly IUmaMusumeRepository _umaMusumeRepository;

        public RaceInformationController(IUmaMusumeRepository umaMusumeRepository)
        {
            _umaMusumeRepository = umaMusumeRepository;
        }


        [HttpGet("raceInfoByTurn/{turnNumber}")]
        public async Task<IActionResult> GetRacesByTurn(int turnNumber)
        {
            return Ok(await _umaMusumeRepository.GetAllRacesByTurnNumber(turnNumber));
        }

        [HttpGet("requiredRacesByCharacter/{charaId}")]
        public async Task<IActionResult> GetRequiredRacesByCharacter(int charaId)
        {
            return Ok(await _umaMusumeRepository.GetRequiredRacesByCharacterId(charaId));
        }

        [HttpGet("requiredFansGoals")]
        public async Task<IActionResult> GetRequiredFansGoalByCharacter()
        {
            return Ok(await _umaMusumeRepository.GetAllCharacterRequiredFanGoals());
        }

        [HttpGet("requiredFansGoalByCharacter/{charaId}")]
        public async Task<IActionResult> GetRequiredFansGoalByCharacter(int charaId)
        {
            return Ok(await _umaMusumeRepository.GetRequiredFanGoalsByCharacterId(charaId));
        }

        [HttpGet("cardRarityInfoByCardId/{cardId}")]
        public async Task<IActionResult> GetCardRarityInfoByCardId(int cardId)
        {
            return Ok(await _umaMusumeRepository.GetCardRarityInfoByCardId(cardId));
        }

        [HttpGet("cardRarityInfoByCharaId/{charaId}")]
        public async Task<IActionResult> GetCardRarityInfoByCharaId(int charaId)
        {
            return Ok(await _umaMusumeRepository.GetCardRarityInfoByCharaId(charaId));
        }
    }
}
