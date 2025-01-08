using Financia.Communication.Responses.Error;
using Financia.Exception;
using Financia.Exception.ExceptionBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Financia.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is FinanciaException)
            {
                HandleProjectException(context);
            }
            else
            {
                ThrowUnknowError(context);
            }
        }
        private void HandleProjectException(ExceptionContext context)
        {
            var exception = (FinanciaException) context.Exception;
            var errorResponse = new ResponseErrorJson(exception.GetErrors());

            context.HttpContext.Response.StatusCode = exception.StatusCode;
            context.Result = new ObjectResult(errorResponse);
        }
        private void ThrowUnknowError(ExceptionContext context)
        {
            var errorResponse = new ResponseErrorJson(ResourceErrorMessages.UNKNOWN_ERROR);
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(errorResponse);
        }
    }
}
