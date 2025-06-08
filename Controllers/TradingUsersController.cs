using DisciplineTradingJournalAPI.Contract;
using DisciplineTradingJournalAPI.DataEntity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DisciplineTradingJournalAPI.Controllers
{
    [Authorize]
    public class TradingUsersController : BaseController
    {
        private readonly ITradingUsersRepository _tradingUsersRepository;

        public TradingUsersController(ITradingUsersRepository tradingUsersRepository)
        {
            _tradingUsersRepository = tradingUsersRepository;
        }

        // GET: api/TradingUsers
        [HttpGet]
        [Route("GetTradingUsers")]
        public async Task<ActionResult<IEnumerable<TradingUsers>>> GetTradingUsers()
        {
            var users = await _tradingUsersRepository.GetAllAsync();
            return Ok(users);
        }

        // GET: api/TradingUsers/5
        [HttpGet]
        [Route("GetTradingUser")]
        public async Task<ActionResult<TradingUsers>> GetTradingUser([FromQuery] int id)
        {
            var user = await _tradingUsersRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST: api/TradingUsers
        [HttpPost]
        [Route("PostTradingUser")]
        public async Task<ActionResult<TradingUsers>> PostTradingUser([FromBody] TradingUsers tradingUser)
        {

            var tradingUsers = await _tradingUsersRepository.AddAsync(tradingUser, tradingUser.PasswordHash);
            return SuccessResponseWithData(tradingUsers);
        }

        // PUT: api/TradingUsers/5
        [HttpPost]
        [Route("PutTradingUser")]
        public async Task<IActionResult> PutTradingUser([FromServices] TradingUsers tradingUser)
        {

            try
            {
                await _tradingUsersRepository.UpdateAsync(tradingUser);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/TradingUsers/5
        [HttpDelete]
        [Route("DeleteTradingUser")]

        public async Task<IActionResult> DeleteTradingUser([FromQuery] int id)
        {
            var user = await _tradingUsersRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _tradingUsersRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
