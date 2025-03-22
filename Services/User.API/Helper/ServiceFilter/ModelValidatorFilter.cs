using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace User.API.Helper.ServiceFilter
{
    public class ModelValidatorFilter : ControllerBase, IActionFilter
    {
        private readonly ILogger<ModelValidatorFilter> _logger;

        public ModelValidatorFilter(ILogger<ModelValidatorFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .SelectMany(kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage))
                    .ToList();

                var logMessage = new StringBuilder()
                    .AppendLine($"Custom Model Validator: {CommonMethods.GetCurrentTime()}")
                    .AppendLine($"Request Path: {context.HttpContext.Request.Path}")
                    .AppendLine($"Request Method: {context.HttpContext.Request.Method}")
                    .AppendLine("Error Messages:");

                foreach (var error in errors)
                {
                    logMessage.AppendLine($"   - {error}");
                }

                _logger.LogError("{LogMessage}", logMessage.ToString());

                context.Result = StatusCode(StatusCodes.Status400BadRequest, Utilities.ValidationErrorResponse(errors));
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
