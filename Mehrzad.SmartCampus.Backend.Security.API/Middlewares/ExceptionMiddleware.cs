using Mehrzad.SmartCampus.Backend.Core.Exceptions;

namespace Mehrzad.SmartCampus.Backend.Security.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppException ex)
            {
                Console.WriteLine($"[Handled Exception] {ex.GetType().Name}: {ex.Message}");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ex.StatusCode;

                var payload = new
                {
                    error = ex.Message,
                    details = (ex is ValidationException ve) ? ve.Errors : null
                };

                await context.Response.WriteAsJsonAsync(payload);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Unhandled Exception] {ex}");
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred." });
            }
        }
    }
}
