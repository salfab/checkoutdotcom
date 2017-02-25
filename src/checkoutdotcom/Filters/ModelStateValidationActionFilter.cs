using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace checkoutdotcom.Filters
{
    /// <summary>
    /// An action filter responsible for returning BadRequest responses when the model state is invalid. 
    /// The returned result will contain a serialized representation of the model state
    /// </summary>
    public class ModelStateValidationActionFilter : ActionFilterAttribute
    {
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var modelState = context.ModelState;

            if (!modelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(modelState);
            }
            return base.OnActionExecutionAsync(context, next);
        }        
    }
}