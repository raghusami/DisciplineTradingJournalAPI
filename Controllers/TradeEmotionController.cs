using Microsoft.AspNetCore.Mvc;
using DisciplineTradingJournalAPI.Contract;
using DisciplineTradingJournalAPI.DataEntity;
using DisciplineTradingJournalAPI.DBModel;
using System.Threading.Tasks;
using System;

namespace DisciplineTradingJournalAPI.Controllers
{
    public class TradeEmotionController : BaseController
    {
        private readonly ITradeEmotionRepository _tradeEmotionRepository;
        public TradeEmotionController(ITradeEmotionRepository tradeEmotionRepository)
        {
            _tradeEmotionRepository = tradeEmotionRepository ?? throw new ArgumentNullException(nameof(tradeEmotionRepository));
        }
        [HttpPost("CreateTradeEmotions")]
        public async Task<IActionResult> CreateTradeEmotionsAsync([FromBody] TradeEmotions tradeEmotions)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _tradeEmotionRepository.AddAsync(tradeEmotions);
                return Ok(new { message = "Trade Emotions created successfully." });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = "An error occurred", error = ex.Message });
            }
        }
    }
}
