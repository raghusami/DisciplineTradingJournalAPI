namespace DisciplineTradingJournalAPI.Controllers
{
    using Asp.Versioning;
    using DisciplineTradingJournalAPI.Helper;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BaseController : ControllerBase
    {
        public BaseController()
        {

        }
        [NonAction]
        public OkObjectResult SuccessResponse(string responseMessage = null)
        {
            return Ok(new ResponseDetail
            {
                ResponseStatusCode = StatusCodes.Status200OK,
                ResponseMessage = string.IsNullOrEmpty(responseMessage) ? ResponseMessageInfo.Success : responseMessage
            });
        }

        [NonAction]
        public OkObjectResult SuccessResponseWithData(object responseData, string responseMessage = null)
        {
            return Ok(new ResponseDetail
            {
                ResponseStatusCode = StatusCodes.Status200OK,
                ResponseData = responseData,
                ResponseMessage = string.IsNullOrEmpty(responseMessage) ? ResponseMessageInfo.Success : responseMessage
            });
        }

        [NonAction]
        public UnauthorizedObjectResult UnauthorizedResponse(string responseMessage = null)
        {
            return Unauthorized(new ResponseDetail
            {
                ResponseStatusCode = StatusCodes.Status401Unauthorized,
                ResponseMessage = string.IsNullOrEmpty(responseMessage) ? ResponseMessageInfo.Success : responseMessage
            });
        }

        [NonAction]
        public OkObjectResult DeleteSuccessResponse(string responseMessage = null)
        {
            return Ok(new ResponseDetail
            {
                ResponseStatusCode = StatusCodes.Status200OK,
                ResponseMessage = string.IsNullOrEmpty(responseMessage) ? ResponseMessageInfo.DeleteSuccess : responseMessage
            });
        }
        [NonAction]
        public OkObjectResult TransactionExistResponse(string responseMessage = null)
        {
            return Ok(new ResponseDetail
            {
                ResponseStatusCode = StatusCodes.Status200OK,
                ResponseMessage = string.IsNullOrEmpty(responseMessage) ? ResponseMessageInfo.TransactionExist : responseMessage
            });
        }
    }
}
