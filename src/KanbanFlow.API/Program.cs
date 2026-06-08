using KanbanFlow.API.Middleware;
using KanbanFlow.Application;
using KanbanFlow.Infrastructure;
using KanbanFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.WithOrigins(
            "http://localhost:5173",
            "http://localhost",  
            "http://localhost:80"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<KanbanDbContext>();

        context.Database.EnsureCreated();
        Console.WriteLine("✅ База данных создана/проверена!");
    }
    catch(Exception ex)
    {
        Console.WriteLine($"❌ Ошибка создания БД: {ex.Message}");
    }
}

// Seed данных
using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<KanbanDbContext>();

        bool hasData = context.Boards.Any();

        if(!hasData)
        {
            Console.WriteLine("📦 Заполняем тестовыми данными...");
            await KanbanFlow.Infrastructure.DbSeeder.SeedAsync(context);
            Console.WriteLine("✅ Тестовые данные добавлены!");
        }
        else
        {
            Console.WriteLine("ℹ️ База уже содержит данные");
        }
    }
    catch(Exception ex)
    {
        Console.WriteLine($"❌ Ошибка seed: {ex.Message}");
    }
}

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowReact");
app.UseValidationExceptionHandler();
app.UseAuthorization();
app.MapControllers();

app.Run();