using System;

using checkoutdotcom.Exceptions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace checkoutdotcom.Filters
{
    /// <summary>
    /// An action filter to return a 400 Bad request response if an <see cref="ArgumentException"/> was thrown during the execution of the Action.
    /// </summary>
    public class ArgumentExceptionToBadRequestActionFilter : ActionFilterAttribute
    {

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var argumentException = context?.Exception as ArgumentException;
            if (argumentException != null)
            {
                context.ModelState.AddModelError(argumentException.ParamName, argumentException.Message);
                context.Result = new BadRequestObjectResult(context.ModelState);
                context.ExceptionHandled = true;
                return;                
            }
            base.OnActionExecuted(context);
        }
    }
}