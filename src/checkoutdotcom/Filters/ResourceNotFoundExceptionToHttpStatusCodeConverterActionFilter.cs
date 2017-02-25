using checkoutdotcom.Exceptions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace checkoutdotcom.Filters
{
    /// <summary>
    /// An action filter to return a bodyless 404 Not Found response if the ResourceNotFoundException was thrown during the execution of the Action.
    /// </summary>
    public class ResourceNotFoundExceptionToHttpStatusCodeConverterActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context?.Exception as ResourceNotFoundException != null)
            {
                context.Result = new NotFoundResult();
                context.ExceptionHandled = true;
                return;                
            }
            base.OnActionExecuted(context);
        }
    }
}