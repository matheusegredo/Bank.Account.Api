namespace Bank.Api.Infrastructure.Base
{
    public class BaseApiResponse
    {
        public BaseApiResponse(object data, int statusCode = 200)
        {
            Data = data;
            StatusCode = statusCode;
        }

        public BaseApiResponse(List<ErrorResponse> errors, int statusCode = 500)
        {
            Errors = errors;
            StatusCode = statusCode;
        }

        public object? Data { get; set; }

        public int StatusCode { get; set; }

        public List<ErrorResponse> Errors { get; set; }
    }

    public class ErrorResponse
    {
        public string Message { get; set; }

        public string Property { get; set; }
    }
}
