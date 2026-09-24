using System.Net.Http;
using ZenTaoApi.Client.DependencyInjection;

namespace ZenTaoApi.Client;

/// <summary>Creates authenticated clients for accounts that vary at runtime.</summary>
public sealed class ZenTaoClientFactory
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ZenTaoClientFactory(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }

    /// <summary>
    /// Creates a new authenticated client for the supplied account.
    /// The caller owns the returned client and should dispose it after use.
    /// </summary>
    public Task<ZenTaoClient> CreateAsync(
        string baseUrl,
        string account,
        string password,
        CancellationToken cancellationToken = default)
    {
        var normalizedBaseUrl = ZenTaoClient.ValidateConfiguration(baseUrl, account, password);
        var httpClient = _httpClientFactory.CreateClient(ZenTaoServiceCollectionExtensions.HttpClientName);
        return ZenTaoClient.CreateOwnedAsync(normalizedBaseUrl, account, password, httpClient, cancellationToken);
    }
}
