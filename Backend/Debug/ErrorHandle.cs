namespace Oscars.Backend.Debug
{
    public class ErrorHandle
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandle> _logger;
        public ErrorHandle(RequestDelegate next, ILogger<ErrorHandle> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred while processing the request.");
                var errorDetails = new
                {
                    message = "An error occurred while processing your request. Please try again later",
                    detail = ex.Message,
                    stackTrace = ex.StackTrace,
                };

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(errorDetails);
            }
        }

    }
}