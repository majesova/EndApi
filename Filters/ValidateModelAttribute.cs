using EndApi.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EndApi.Filters
{
    public class ValidateModelAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context){

                if (!context.ModelState.IsValid){
                    var result = new ManagedErrorResponse(ManagedErrorCode.Validation,"Hay errores de validación",context.ModelState);
                    context.Result = new BadRequestObjectResult(result);
                }
        }
    }
}