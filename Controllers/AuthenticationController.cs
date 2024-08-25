using DisciplineTradingJournalAPI.Contract;
using JWTAuthenticationManager;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DisciplineTradingJournalAPI.Controllers
{
    public class AuthenticationController : BaseController
    {

        private readonly ITradingUsersRepository _tradingUsersRepository;

        public AuthenticationController(ITradingUsersRepository tradingUsersRepository)
        {
            _tradingUsersRepository = tradingUsersRepository;
        }

        [HttpPost]
        [Route("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] LoginRequest request, [FromServices] JWTAuthenticationHandler _ITokenHandler)
        {
            try
            {
                var userLoginInformation = await _tradingUsersRepository.SignInAsync(request.Username, request.Password);
                var userLoginDetails = new UserInformation
                {
                    UserName = userLoginInformation?.Username,
                    EmailId = userLoginInformation?.Email,
                    UserUniqueId = Convert.ToString(userLoginInformation?.UserID),
                    ClaimId = Convert.ToString(userLoginInformation?.ClaimId)
                };
                var responseContent = new
                {
                    EncToken = _ITokenHandler.GeneratingJWTToken(userLoginDetails),
                };
                return SuccessResponseWithData(responseContent);
            }
            catch (Exception ex)
            {
                return UnauthorizedResponse(ex.Message);
            }
        }
        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
