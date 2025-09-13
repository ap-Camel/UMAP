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
            return Ok(await _umaMusumeRepository.GetAllRacesByTurn(turnNumber));
        }
    }
}
