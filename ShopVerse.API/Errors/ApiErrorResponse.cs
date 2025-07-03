namespace ShopVerse.API.Errors
{
    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiErrorResponse(int statusCode, string? message = null, string? details = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "The request was invalid or cannot be processed due to client-side error.",
                401 => "You are not authorized to access this resource. Please check your credentials.",
                403 => "Access to this resource is forbidden. You do not have the required permissions.",
                404 => "The requested resource was not found. Please verify the endpoint or ID.",
                405 => "The HTTP method used is not allowed for this endpoint.",
                409 => "There is a conflict with the current state of the resource.",
                422 => "The request was well-formed but contains semantic errors (e.g., validation failure).",
                500 => "An unexpected server error occurred. Our team has been notified.",
                503 => "The server is temporarily unavailable. Please try again later.",
                _ => "An unexpected error occurred while processing your request."
            };
        }
    }

}
