using Bank.CrossCutting.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Infrastructure.Base
{
    public class BaseApiController : Controller
    {
        protected static async Task<IActionResult> ResponseHandler<TDataObject>(Func<Task<TDataObject>> func)
        {
            try
            {
                var response = await func();
                return new OkObjectResult(new BaseApiResponse(response));
            }
            catch (Exception ex)
            {
                return ex switch
                {
                    InvalidRequestException invalid => PrepareObjectResultWithErrors(invalid, 400),
                    UnauthorizedAccessException unauthorized => PrepareObjectResultWithErrors(unauthorized, 401),
                    ForbidenRequestException forbiden => PrepareObjectResultWithErrors(forbiden, 403),
                    NotFoundException notFound => PrepareObjectResultWithErrors(notFound, 404),
                    ValidationException validation => PrepareValidationResultWithErrors(validation),
                    _ => PrepareObjectResultWithErrors(ex, 500)
                };
            }
        }

        private static IActionResult PrepareObjectResultWithErrors(Exception ex, int statusCode)
        {
            var baseResponse = new BaseApiResponse(new List<ErrorResponse> { new ErrorResponse { Message = ex.Message } }, statusCode);

            return statusCode switch
            {
                400 => new BadRequestObjectResult(baseResponse),
                401 => new UnauthorizedObjectResult(baseResponse),
                403 => new ObjectResult(baseResponse) { StatusCode = statusCode },
                404 => new NotFoundObjectResult(baseResponse),
                _ => new ObjectResult(baseResponse) { StatusCode = 500 }
            };
        }

        private static IActionResult PrepareValidationResultWithErrors(ValidationException ex)
        {
            var errorResponse = ex.Errors.Select(p => new ErrorResponse { Message = p.ErrorMessage, Property = p.PropertyName }).ToList();

            return new BadRequestObjectResult(new BaseApiResponse(errorResponse, 400));
        }
    }
}
