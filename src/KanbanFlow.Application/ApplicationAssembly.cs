namespace KanbanFlow.Application;

/// <summary>
/// Маркерный класс для доступа к сборке Application.
/// Используется для регистрации MediatR и других сервисов.
/// </summary>
public static class ApplicationAssembly
{
    public static System.Reflection.Assembly Assembly => typeof(ApplicationAssembly).Assembly;
}