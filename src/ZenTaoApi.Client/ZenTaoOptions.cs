namespace ZenTaoApi.Client;

/// <summary>Configuration for the application-wide ZenTao client.</summary>
public sealed class ZenTaoOptions
{
    public string BaseUrl { get; set; } = string.Empty;
    public string Account { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    internal void Validate()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(BaseUrl);
        ArgumentException.ThrowIfNullOrWhiteSpace(Account);
        ArgumentException.ThrowIfNullOrWhiteSpace(Password);
    }
}
