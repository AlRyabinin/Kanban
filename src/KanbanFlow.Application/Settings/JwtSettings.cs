namespace KanbanFlow.Application.Settings;

/// <summary>
/// Настройки JWT-аутентификации.
/// Значения загружаются из секции "JwtSettings" файла appsettings.json.
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// Имя секции в конфигурационном файле.
    /// </summary>
    public const string SectionName = "JwtSettings";

    /// <summary>
    /// Секретный ключ для подписи JWT-токенов.
    /// Должен содержать минимум 32 символа.
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// Издатель (issuer) JWT-токена.
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Аудитория (audience) JWT-токена.
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Время жизни токена в минутах.
    /// По умолчанию — 1440 минут (24 часа).
    /// </summary>
    public int ExpirationInMinutes { get; set; } = 1440;
}