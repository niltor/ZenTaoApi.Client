using ZenTaoApi.Client.Models.RequestDtos;
using ZenTaoApi.Client.Models.ResponseDtos;

namespace ZenTaoApi.Client.Clients;

/// <summary>Common typed request helpers used by individual resource clients.</summary>
public abstract class ZenTaoClientBase
{
    private readonly ZenTaoClient _client;

    protected ZenTaoClientBase(ZenTaoClient client)
    {
        _client = client;
    }

    protected Task<TResponse> GetAsync<TResponse>(
        string path,
        ZenTaoListRequestDto? query,
        CancellationToken cancellationToken)
        where TResponse : ZenTaoResponseDto, new()
        => _client.SendAsync<TResponse>(
            HttpMethod.Get,
            path,
            body: null,
            query?.ToQueryParameters(),
            includeToken: true,
            retryAfterUnauthorized: true,
            cancellationToken);

    protected Task<TResponse> PostAsync<TResponse>(
        string path,
        ZenTaoRequestDto? body,
        CancellationToken cancellationToken)
        where TResponse : ZenTaoResponseDto, new()
        => _client.SendAsync<TResponse>(
            HttpMethod.Post,
            path,
            body,
            query: null,
            includeToken: true,
            retryAfterUnauthorized: true,
            cancellationToken);

    protected Task<TResponse> PutAsync<TResponse>(
        string path,
        ZenTaoRequestDto body,
        CancellationToken cancellationToken)
        where TResponse : ZenTaoResponseDto, new()
        => _client.SendAsync<TResponse>(
            HttpMethod.Put,
            path,
            body,
            query: null,
            includeToken: true,
            retryAfterUnauthorized: true,
            cancellationToken);

    protected Task<TResponse> DeleteAsync<TResponse>(
        string path,
        CancellationToken cancellationToken)
        where TResponse : ZenTaoResponseDto, new()
        => _client.SendAsync<TResponse>(
            HttpMethod.Delete,
            path,
            body: null,
            query: null,
            includeToken: true,
            retryAfterUnauthorized: true,
            cancellationToken);

    protected static string Id(long id) => id.ToString(CultureInfo.InvariantCulture);
}
