using System.Net;
using System.Text.Json;

namespace SmartLeaf.Middleware
{
    /// <summary>
    /// Middleware global que captura todas las excepciones no manejadas
    /// y devuelve una respuesta JSON consistente en lugar de páginas de error HTML.
    /// </summary>
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
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
                _logger.LogError(ex, "Excepción no manejada: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, message) = exception switch
            {
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "No autorizado."),
                ArgumentException e         => (HttpStatusCode.BadRequest, e.Message),
                KeyNotFoundException e      => (HttpStatusCode.NotFound, e.Message),
                _                           => (HttpStatusCode.InternalServerError, "Ocurrió un error interno. Por favor intentá más tarde.")
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode  = (int)statusCode;

            var response = JsonSerializer.Serialize(new
            {
                status  = (int)statusCode,
                message = message
            });

            return context.Response.WriteAsync(response);
        }
    }
}
