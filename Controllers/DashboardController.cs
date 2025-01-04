using DisciplineTradingJournalAPI.Contract;
using JWTAuthenticationManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DisciplineTradingJournalAPI.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IUserClaimManager _userClaimManager;

        public DashboardController(IDashboardRepository dashboardRepository, IUserClaimManager userClaimManager)
        {
            _dashboardRepository = dashboardRepository;
            _userClaimManager = userClaimManager;
        }
        [HttpGet]
        [Route("GetUsersDashbaord")]
        public async Task<IActionResult> GetUsersClosedPositions()
        {

            var usersOpenPositionsResult = await _dashboardRepository.GetUserPerformanceDashBoardAsync(_userClaimManager.UserUniqueId);
            if (usersOpenPositionsResult == null)
            {
                return NotFound();
            }
            return SuccessResponseWithData(usersOpenPositionsResult);
        }
    }
}
