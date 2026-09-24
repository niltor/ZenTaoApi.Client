using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ZenTaoApi.Client.DependencyInjection;

/// <summary>Registers a shared ZenTao client and a factory for per-account clients.</summary>
public static class ZenTaoServiceCollectionExtensions
{
    internal const string HttpClientName = "ZenTaoApi.Client";

    /// <summary>
    /// Registers an application-wide client using the supplied fixed account, plus a factory
    /// that can create disposable clients for dynamic accounts.
    /// </summary>
    public static IServiceCollection AddZenTaoClient(
        this IServiceCollection services,
        Action<ZenTaoOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        services.AddOptions<ZenTaoOptions>()
            .Configure(configure)
            .Validate(options => IsValid(options), "ZenTao BaseUrl must be an absolute HTTP(S) URL, and Account/Password are required.")
            .ValidateOnStart();

        services.AddZenTaoClientFactory();

        services.TryAddSingleton<ZenTaoClient>(serviceProvider =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<ZenTaoOptions>>().Value;
            options.Validate();

            var httpClient = serviceProvider
                .GetRequiredService<IHttpClientFactory>()
                .CreateClient(HttpClientName);

            return ZenTaoClient.CreateForDependencyInjection(
                options.BaseUrl,
                options.Account,
                options.Password,
                httpClient);
        });

        return services;
    }

    /// <summary>
    /// Registers the factory for creating and disposing clients with runtime-supplied accounts.
    /// Use this by itself when the application does not need a shared fixed-account client.
    /// </summary>
    public static IServiceCollection AddZenTaoClientFactory(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        if (!services.Any(descriptor => descriptor.ServiceType == typeof(HttpClientRegistrationMarker)))
        {
            services.AddSingleton(new HttpClientRegistrationMarker());
            services.AddHttpClient(HttpClientName)
                .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
                {
                    UseCookies = false,
                    PooledConnectionLifetime = TimeSpan.FromMinutes(5)
                })
                .SetHandlerLifetime(Timeout.InfiniteTimeSpan);
        }

        services.TryAddSingleton<ZenTaoClientFactory>();
        return services;
    }

    private static bool IsValid(ZenTaoOptions options)
    {
        try
        {
            _ = ZenTaoClient.ValidateConfiguration(options.BaseUrl, options.Account, options.Password);
            return true;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }

    private sealed class HttpClientRegistrationMarker { }
}
