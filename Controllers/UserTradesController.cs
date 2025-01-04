using DisciplineTradingJournalAPI.Contract;
using DisciplineTradingJournalAPI.DataEntity;
using JWTAuthenticationManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DisciplineTradingJournalAPI.Controllers
{
 //   [Authorize]
    public class UserTradesController : BaseController
    {
        private readonly IUserTradesRepository _userTradesRepository;
        private readonly IUserClaimManager _userClaimManager;
        public UserTradesController(IUserTradesRepository userTradesRepository, IUserClaimManager userClaimManager)
        {
            _userTradesRepository = userTradesRepository;
            _userClaimManager = userClaimManager;
        }

        // GET: api/UserTrades
        [HttpGet]
        [Route("GetUserTrades")]
        public async Task<ActionResult<IEnumerable<UserTrades>>> GetUserTrades()
        {
            var userID = _userClaimManager.UserUniqueId;
            var userTrades = await _userTradesRepository.GetAllAsync(userID);
            return Ok(userTrades);
        }

        // GET: api/UserTrades/5
        [HttpGet]
        [Route("GetUserTrade")]
        public async Task<ActionResult<UserTrades>> GetUserTrade([FromQuery] int id)
        {
            var userTrade = await _userTradesRepository.GetByIdAsync(id);
            if (userTrade == null)
            {
                return NotFound();
            }
            return Ok(userTrade);
        }

        // POST: api/UserTrades
        [HttpPost]
        [Route("PostUserTrade")]
        public async Task<ActionResult<UserTrades>> PostUserTrade([FromBody] UserTrades userTrade)
        {
            
            var userID = _userClaimManager.UserUniqueId;
            userTrade.UserID = userID;
            var userTrades = await _userTradesRepository.AddAsync(userTrade);
            return SuccessResponseWithData(userTrades);
        }

        // PUT: api/UserTrades/5
        [HttpPost]
        [Route("PutUserTrade")]
        public async Task<ActionResult<UserTrades>> PutUserTrade([FromBody] UserTrades userTrade)
        {
               var userTrades = await _userTradesRepository.UpdateAsync(userTrade);
                return SuccessResponseWithData(userTrades);  
        }

        // DELETE: api/UserTrades/5
        [HttpDelete]
        [Route("DeleteUserTrade")]
        public async Task<IActionResult> DeleteUserTrade([FromQuery] int id)
        {
            var userTrade = await _userTradesRepository.GetByIdAsync(id);
            if (userTrade == null)
            {
                return NotFound();
            }

            await _userTradesRepository.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet]
        [Route("GetUsersOpenPositions")]
        public async Task<IActionResult> GetUsersOpenPositions()
        {
            var usersOpenPositionsResult = await _userTradesRepository.GetUsersOpenPositionsWithTradeMetricAsync(_userClaimManager.UserUniqueId);
            if (usersOpenPositionsResult == null)
            {
                return NotFound();
            }
            return SuccessResponseWithData(usersOpenPositionsResult);
        }
        [HttpGet]
        [Route("GetUsersClosedPositions")]
        public async Task<IActionResult> GetUsersClosedPositions()
        {
            var usersOpenPositionsResult = await _userTradesRepository.GetUsersClosedPositionsWithTradeMetricAsync(_userClaimManager.UserUniqueId);
            if (usersOpenPositionsResult == null)
            {
                return NotFound();
            }
            return SuccessResponseWithData(usersOpenPositionsResult);
        }
    }
}
