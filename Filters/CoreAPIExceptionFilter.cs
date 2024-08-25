namespace DisciplineTradingJournalAPI.Filters
{
    using Microsoft.AspNetCore.Http;
    using System;
    using Microsoft.AspNetCore.Mvc.Filters;
    using DisciplineTradingJournalAPI.Contract;

    public class CoreAPIExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IAsyncLogger _logger;
        public CoreAPIExceptionFilter(IAsyncLogger logger)
        {
            this._logger = logger;
        }
        public async override void OnException(ExceptionContext context)
        {
            await _logger.LogAsync(context.Exception, "Exception Occur.");

            if (!Environment.UserInteractive)
            {
                context.HttpContext.Response.Clear();
                context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.ExceptionHandled = true;
                return;
            }
            base.OnException(context);
        }
    }
}
