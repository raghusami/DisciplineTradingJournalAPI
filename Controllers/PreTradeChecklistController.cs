using DisciplineTradingJournalAPI.Contract;
using DisciplineTradingJournalAPI.DataEntity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DisciplineTradingJournalAPI.Controllers
{
    public class PreTradeChecklistController : BaseController
    {
        public readonly  IPreTradeChecklistRepository _preTradeChecklistRepository;
        public PreTradeChecklistController(IPreTradeChecklistRepository preTradeChecklist)
        {
            _preTradeChecklistRepository = preTradeChecklist;
        }

        [HttpPost("CreatePreTradeCheckList")]
        public async Task<IActionResult> CreatePreTradeCheckAsync([FromBody] PreTradeChecklist preTradeChecklist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _preTradeChecklistRepository.AddAsync(preTradeChecklist);
                return Ok(new { message = "PreTrade Check List created successfully." });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = "An error occurred", error = ex.Message });
            }
        }
    }
}
