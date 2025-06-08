using DisciplineTradingJournalAPI.DBModel;
using DisciplineTradingJournalAPI.ViewEntity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using JWTAuthenticationManager;

namespace DisciplineTradingJournalAPI.Controllers
{
    public class SeriesTradeController : BaseController
    {
        private readonly SeriesService _seriesService;
        private readonly IUserClaimManager _userClaimManager;
        public SeriesTradeController(SeriesService seriesService,IUserClaimManager userClaimManager)
        {
            _seriesService = seriesService;
            _userClaimManager = userClaimManager;
        }
        [HttpPost("CreateTrackerWithTrades")]
        public async Task<IActionResult> CreateTrackerWithTradesAsync([FromBody] SeriesTrackerViewEntity seriesTracker)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _seriesService.CreateTrackerWithTrades(seriesTracker, _userClaimManager.UserUniqueId);
                return Ok(new { message = "Series tracker and trades created successfully." });
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, new { message = "An error occurred", error = ex.Message });
            }
        }
    }
}
