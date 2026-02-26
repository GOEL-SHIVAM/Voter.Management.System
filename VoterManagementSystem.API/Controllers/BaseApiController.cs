using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace VoterManagementSystem.API.Controllers
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected async Task<ActionResult<T>> ValidateAndExecuteAsync<TRequest, T>(
            TRequest request,
            IValidator<TRequest> validator,
            Func<Task<T>> action)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => new
                {
                    Property = e.PropertyName,
                    Error = e.ErrorMessage
                }));
            }

            try
            {
                var result = await action();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        protected async Task<ActionResult> ValidateAndExecuteAsync<TRequest>(
            TRequest request,
            IValidator<TRequest> validator,
            Func<Task> action)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => new
                {
                    Property = e.PropertyName,
                    Error = e.ErrorMessage
                }));
            }

            try
            {
                await action();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
