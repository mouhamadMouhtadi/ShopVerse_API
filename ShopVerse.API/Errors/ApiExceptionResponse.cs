namespace ShopVerse.API.Errors
{
    public class ApiExceptionResponse : ApiErrorResponse
    {
        public string? Details { get; set; }
        public ApiExceptionResponse(int statusCode, string? message=null, string? details = null) : base(500)
        {
            Details = details;
        }
    }
}
