using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace KanbanFlow.Application;

/// <summary>
/// Класс для регистрации зависимостей слоя приложения (Application Layer).
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Расширяет IServiceCollection, добавляя MediatR и FluentValidation.
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Регистрируем MediatR, указывая сборку, где лежат наши команды и хендлеры
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // Регистрируем все валидаторы FluentValidation из текущей сборки
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}