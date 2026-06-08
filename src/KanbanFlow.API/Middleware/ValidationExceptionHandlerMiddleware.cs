using FluentValidation;
using System.Net;
using System.Text.Json;

namespace KanbanFlow.API.Middleware;

/// <summary>
/// Middleware для глобальной обработки исключений валидации FluentValidation.
/// Перехватывает ValidationException и возвращает 400 Bad Request с деталями ошибок.
/// </summary>
public class ValidationExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ValidationExceptionHandlerMiddleware> _logger;

    /// <summary>
    /// Конструктор middleware.
    /// </summary>
    /// <param name="next">Следующий middleware в конвейере.</param>
    /// <param name="logger">Логгер для записи ошибок.</param>
    public ValidationExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<ValidationExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Метод, выполняющий обработку запроса.
    /// </summary>
    /// <param name="context">Контекст HTTP-запроса.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(ValidationException ex)
        {
            _logger.LogWarning(ex, "Ошибка валидации при обработке запроса {Method} {Path}",
                context.Request.Method, context.Request.Path);

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            var errors = ex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            var response = new
            {
                type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                title = "Validation Error",
                status = 400,
                errors
            };

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Необработанное исключение при обработке запроса {Method} {Path}",
                context.Request.Method, context.Request.Path);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new
            {
                type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                title = "Internal Server Error",
                status = 500,
                detail = "Произошла внутренняя ошибка сервера."
            };

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }
}

/// <summary>
/// Методы расширения для удобной регистрации middleware.
/// </summary>
public static class ValidationExceptionHandlerMiddlewareExtensions
{
    /// <summary>
    /// Добавляет middleware для обработки ошибок валидации в конвейер обработки запросов.
    /// </summary>
    /// <param name="builder">Конвейер обработки запросов.</param>
    /// <returns>Конвейер обработки запросов с добавленным middleware.</returns>
    public static IApplicationBuilder UseValidationExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ValidationExceptionHandlerMiddleware>();
    }
}