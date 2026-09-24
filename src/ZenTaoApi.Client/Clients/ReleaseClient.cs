using ZenTaoApi.Client.Models.RequestDtos;
using ZenTaoApi.Client.Models.ResponseDtos;

namespace ZenTaoApi.Client.Clients;

public sealed class ReleaseClient : ZenTaoClientBase
{
    internal ReleaseClient(ZenTaoClient client) : base(client) { }

    public Task<ReleaseListResponse> GetForProductAsync(
        long productId,
        ReleaseListRequest? request = null,
        CancellationToken cancellationToken = default)
        => GetAsync<ReleaseListResponse>($"products/{Id(productId)}/releases", request, cancellationToken);

    public Task<ReleaseListResponse> GetForProjectAsync(
        long projectId,
        ReleaseListRequest? request = null,
        CancellationToken cancellationToken = default)
        => GetAsync<ReleaseListResponse>($"projects/{Id(projectId)}/releases", request, cancellationToken);
}
