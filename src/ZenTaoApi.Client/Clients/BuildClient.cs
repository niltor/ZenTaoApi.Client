using ZenTaoApi.Client.Models.RequestDtos;
using ZenTaoApi.Client.Models.ResponseDtos;

namespace ZenTaoApi.Client.Clients;

public sealed class BuildClient : ZenTaoClientBase
{
    internal BuildClient(ZenTaoClient client) : base(client) { }

    public Task<BuildListResponse> GetForProjectAsync(long projectId, BuildListRequest? request = null, CancellationToken cancellationToken = default)
        => GetAsync<BuildListResponse>($"projects/{Id(projectId)}/builds", request, cancellationToken);

    public Task<BuildListResponse> GetForExecutionAsync(long executionId, BuildListRequest? request = null, CancellationToken cancellationToken = default)
        => GetAsync<BuildListResponse>($"executions/{Id(executionId)}/builds", request, cancellationToken);

    public Task<MutationResponse> CreateAsync(long projectId, CreateBuildRequest request, CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>($"projects/{Id(projectId)}/builds", request, cancellationToken);

    public Task<BuildDetailResponse> GetByIdAsync(long buildId, CancellationToken cancellationToken = default)
        => GetAsync<BuildDetailResponse>($"builds/{Id(buildId)}", null, cancellationToken);

    public Task<MutationResponse> UpdateAsync(long buildId, UpdateBuildRequest request, CancellationToken cancellationToken = default)
        => PutAsync<MutationResponse>($"builds/{Id(buildId)}", request, cancellationToken);

    public Task<MutationResponse> DeleteAsync(long buildId, CancellationToken cancellationToken = default)
        => base.DeleteAsync<MutationResponse>($"builds/{Id(buildId)}", cancellationToken);
}
