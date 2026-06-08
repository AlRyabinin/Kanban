using KanbanFlow.Application.Interfaces;
using KanbanFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KanbanFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<KanbanDbContext>(options =>
            options.UseSqlite("Data Source=/app/data/kanban.db"));

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<KanbanDbContext>());

        return services;
    }
}