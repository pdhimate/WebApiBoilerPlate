using Api.BusinessEntities;
using Api.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;
using System.Web.Http.Results;

namespace Api.Filters
{
    /// <summary>
    /// Validates the model parameter for the action on which it is specified.
    /// </summary>
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // Null arguments are not allowed
            if (actionContext.ActionArguments.Any(v => v.Value == null))
            {
                // Create errorDto
                var errorDto = new ErrorDto
                {
                    Title = Errors.ValidationErrorMessage,
                    Messages = new List<string> { Errors.ParameterMissing },
                    ErrorReference = Guid.Empty.ToString(),
                };
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errorDto);
            }

            // Invalid models are not allowed
            if (!actionContext.ModelState.IsValid)
            {
                var errors = new List<string>();
                foreach (var error in actionContext.ModelState)
                {
                    var errorMessages = error.Value.Errors.Select(e => e.ErrorMessage);
                    var errorMessage = string.Join(". ", errorMessages);
                    errors.Add(errorMessage);
                }

                // Create errorDto
                var errorDto = new ErrorDto
                {
                    Title = Errors.ValidationErrorMessage,
                    Messages = errors,
                    ErrorReference = Guid.Empty.ToString(),
                };

                // Create http response
                var response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errorDto);
                actionContext.Response = response;
            }
        }
    }
}