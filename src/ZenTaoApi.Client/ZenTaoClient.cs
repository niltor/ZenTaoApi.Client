using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ZenTaoApi.Client.Clients;
using ZenTaoApi.Client.Models.RequestDtos;
using ZenTaoApi.Client.Models.ResponseDtos;

namespace ZenTaoApi.Client;

/// <summary>
/// Authenticated client for ZenTao RESTful API v1.
/// Use <see cref="CreateAsync"/> for eager authentication, or register it in ASP.NET Core
/// to share a fixed-account instance across the application.
/// </summary>
public sealed class ZenTaoClient : IDisposable, IAsyncDisposable
{
    private static readonly JsonSerializerOptions DefaultJsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = false
    };

    private readonly HttpClient _httpClient;
    private readonly bool _ownsHttpClient;
    private readonly Uri _baseAddress;
    private readonly string _account;
    private readonly string _password;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly SemaphoreSlim _tokenLock = new(1, 1);
    private string? _token;
    private bool _disposed;

    private ZenTaoClient(
        HttpClient httpClient,
        bool ownsHttpClient,
        string baseUrl,
        string account,
        string password)
    {
        _httpClient = httpClient;
        _ownsHttpClient = ownsHttpClient;
        _baseAddress = new Uri(baseUrl.TrimEnd('/') + "/", UriKind.Absolute);
        _account = account;
        _password = password;
        _jsonOptions = new JsonSerializerOptions(DefaultJsonOptions);

        Departments = new DepartmentClient(this);
        Users = new UserClient(this);
        ProjectSets = new ProjectSetClient(this);
        Products = new ProductClient(this);
        ProductPlans = new ProductPlanClient(this);
        Releases = new ReleaseClient(this);
        Requirements = new RequirementClient(this);
        Projects = new ProjectClient(this);
        Versions = new BuildClient(this);
        Executions = new ExecutionClient(this);
        Tasks = new TaskClient(this);
        Bugs = new BugClient(this);
        TestCases = new TestCaseClient(this);
        TestTasks = new TestTaskClient(this);
        Feedbacks = new FeedbackClient(this);
        Tickets = new TicketClient(this);
    }

    /// <summary>
    /// Creates an authenticated client and obtains a Token before returning.
    /// </summary>
    public static async Task<ZenTaoClient> CreateAsync(
        string baseUrl,
        string account,
        string password,
        HttpClient? httpClient = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedBaseUrl = ValidateConfiguration(baseUrl, account, password);
        return await CreateAuthenticatedAsync(
            normalizedBaseUrl,
            account,
            password,
            httpClient ?? new HttpClient(),
            ownsHttpClient: httpClient is null,
            cancellationToken).ConfigureAwait(false);
    }

    internal static ZenTaoClient CreateForDependencyInjection(
        string baseUrl,
        string account,
        string password,
        HttpClient httpClient)
    {
        var normalizedBaseUrl = ValidateConfiguration(baseUrl, account, password);
        return new ZenTaoClient(httpClient, ownsHttpClient: true, normalizedBaseUrl, account, password);
    }

    internal static Task<ZenTaoClient> CreateOwnedAsync(
        string baseUrl,
        string account,
        string password,
        HttpClient httpClient,
        CancellationToken cancellationToken)
        => CreateAuthenticatedAsync(baseUrl, account, password, httpClient, ownsHttpClient: true, cancellationToken);

    private static async Task<ZenTaoClient> CreateAuthenticatedAsync(
        string baseUrl,
        string account,
        string password,
        HttpClient httpClient,
        bool ownsHttpClient,
        CancellationToken cancellationToken)
    {
        var normalizedBaseUrl = ValidateConfiguration(baseUrl, account, password);
        var client = new ZenTaoClient(httpClient, ownsHttpClient, normalizedBaseUrl, account, password);

        try
        {
            await client.AuthenticateAsync(cancellationToken).ConfigureAwait(false);
            return client;
        }
        catch
        {
            client.Dispose();
            throw;
        }
    }

    internal static string ValidateConfiguration(string baseUrl, string account, string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(baseUrl);
        ArgumentException.ThrowIfNullOrWhiteSpace(account);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        if (!Uri.TryCreate(baseUrl.TrimEnd('/') + "/", UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            throw new ArgumentException("Base URL must be an absolute HTTP or HTTPS URL.", nameof(baseUrl));
        }

        return uri.ToString();
    }

    public Uri BaseAddress => _baseAddress;

    /// <summary>The current ZenTao Token. It is refreshed automatically after a 401 response.</summary>
    public string Token => _token ?? throw new InvalidOperationException("The client has not been authenticated.");

    /// <summary>Authenticates the client. Useful for validating credentials during application startup.</summary>
    public Task AuthenticateAsync(CancellationToken cancellationToken = default)
        => EnsureTokenAsync(cancellationToken);

    public DepartmentClient Departments { get; }
    public UserClient Users { get; }
    public ProjectSetClient ProjectSets { get; }
    public ProductClient Products { get; }
    public ProductPlanClient ProductPlans { get; }
    public ReleaseClient Releases { get; }
    public RequirementClient Requirements { get; }
    public ProjectClient Projects { get; }
    public BuildClient Versions { get; }
    public ExecutionClient Executions { get; }
    public TaskClient Tasks { get; }
    public BugClient Bugs { get; }
    public TestCaseClient TestCases { get; }
    public TestTaskClient TestTasks { get; }
    public FeedbackClient Feedbacks { get; }
    public TicketClient Tickets { get; }

    /// <summary>
    /// Sends an authenticated GET request for an endpoint not yet represented by a resource client.
    /// The path is relative to the configured API base URL.
    /// </summary>
    public Task<TResponse> RequestAsync<TResponse>(
        HttpMethod method,
        string relativePath,
        ZenTaoListRequestDto? query = null,
        CancellationToken cancellationToken = default)
        where TResponse : ZenTaoResponseDto, new()
    {
        ArgumentNullException.ThrowIfNull(method);
        ArgumentException.ThrowIfNullOrWhiteSpace(relativePath);
        return SendAsync<TResponse>(
            method,
            relativePath,
            body: null,
            query?.ToQueryParameters(),
            includeToken: true,
            retryAfterUnauthorized: true,
            cancellationToken);
    }

    /// <summary>
    /// Sends an authenticated request for an endpoint not yet represented by a resource client.
    /// </summary>
    public Task<TResponse> RequestAsync<TRequest, TResponse>(
        HttpMethod method,
        string relativePath,
        TRequest body,
        ZenTaoListRequestDto? query = null,
        CancellationToken cancellationToken = default)
        where TRequest : ZenTaoRequestDto
        where TResponse : ZenTaoResponseDto, new()
    {
        ArgumentNullException.ThrowIfNull(method);
        ArgumentException.ThrowIfNullOrWhiteSpace(relativePath);
        ArgumentNullException.ThrowIfNull(body);
        return SendAsync<TResponse>(
            method,
            relativePath,
            body,
            query?.ToQueryParameters(),
            includeToken: true,
            retryAfterUnauthorized: true,
            cancellationToken);
    }

    internal async Task<TResponse> SendAsync<TResponse>(
        HttpMethod method,
        string relativePath,
        ZenTaoRequestDto? body,
        IEnumerable<KeyValuePair<string, string?>>? query,
        bool includeToken,
        bool retryAfterUnauthorized,
        CancellationToken cancellationToken)
        where TResponse : ZenTaoResponseDto, new()
    {
        ThrowIfDisposed();
        var requestPath = BuildRequestPath(relativePath, query);
        if (includeToken)
        {
            await EnsureTokenAsync(cancellationToken).ConfigureAwait(false);
        }

        var tokenUsed = _token;

        for (var attempt = 0; ; attempt++)
        {
            using var request = new HttpRequestMessage(method, new Uri(_baseAddress, requestPath));
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (includeToken)
            {
                if (string.IsNullOrWhiteSpace(tokenUsed))
                {
                    throw new InvalidOperationException("The client has not been authenticated.");
                }

                request.Headers.TryAddWithoutValidation("Token", tokenUsed);
            }

            if (body is not null)
            {
                var json = JsonSerializer.Serialize(body, body.GetType(), _jsonOptions);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            using var response = await _httpClient
                .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                .ConfigureAwait(false);
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.Unauthorized &&
                includeToken &&
                retryAfterUnauthorized &&
                attempt == 0)
            {
                await RefreshTokenAsync(tokenUsed, cancellationToken).ConfigureAwait(false);
                tokenUsed = _token;
                continue;
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new ZenTaoApiException(response.StatusCode, requestPath, responseBody);
            }

            return Deserialize<TResponse>(responseBody);
        }
    }

    private async Task RefreshTokenAsync(string? expectedInvalidToken, CancellationToken cancellationToken)
    {
        await _tokenLock.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (expectedInvalidToken is not null &&
                !string.Equals(_token, expectedInvalidToken, StringComparison.Ordinal))
            {
                return;
            }

            await RequestNewTokenAsync(cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            _tokenLock.Release();
        }
    }

    private async Task EnsureTokenAsync(CancellationToken cancellationToken)
    {
        ThrowIfDisposed();
        if (!string.IsNullOrWhiteSpace(_token))
        {
            return;
        }

        await _tokenLock.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (string.IsNullOrWhiteSpace(_token))
            {
                await RequestNewTokenAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        finally
        {
            _tokenLock.Release();
        }
    }

    private async Task RequestNewTokenAsync(CancellationToken cancellationToken)
    {
        var response = await SendAsync<TokenResponse>(
            HttpMethod.Post,
            "tokens",
            new TokenRequest { Account = _account, Password = _password },
            query: null,
            includeToken: false,
            retryAfterUnauthorized: false,
            cancellationToken).ConfigureAwait(false);

        if (string.IsNullOrWhiteSpace(response.Token))
        {
            throw new ZenTaoAuthenticationException("ZenTao did not return a usable token.");
        }

        _token = response.Token;
    }

    private TResponse Deserialize<TResponse>(string responseBody)
        where TResponse : ZenTaoResponseDto, new()
    {
        if (string.IsNullOrWhiteSpace(responseBody))
        {
            return new TResponse();
        }

        try
        {
            return JsonSerializer.Deserialize<TResponse>(responseBody, _jsonOptions) ?? new TResponse();
        }
        catch (JsonException exception)
        {
            throw new ZenTaoResponseException(
                "ZenTao returned a successful response that was not valid JSON.",
                exception);
        }
    }

    private static string BuildRequestPath(
        string relativePath,
        IEnumerable<KeyValuePair<string, string?>>? query)
    {
        var path = relativePath.Trim().TrimStart('/');
        if (Uri.TryCreate(path, UriKind.Absolute, out _))
        {
            throw new ArgumentException("Request paths must be relative to the configured API base URL.", nameof(relativePath));
        }

        if (query is null)
        {
            return path;
        }

        var queryString = string.Join(
            "&",
            query
                .Where(item => item.Value is not null)
                .Select(item => $"{Uri.EscapeDataString(item.Key)}={Uri.EscapeDataString(item.Value!)}"));

        return string.IsNullOrEmpty(queryString)
            ? path
            : $"{path}{(path.Contains('?') ? '&' : '?')}{queryString}";
    }

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        _tokenLock.Dispose();
        if (_ownsHttpClient)
        {
            _httpClient.Dispose();
        }
    }

    public ValueTask DisposeAsync()
    {
        Dispose();
        return ValueTask.CompletedTask;
    }
}
