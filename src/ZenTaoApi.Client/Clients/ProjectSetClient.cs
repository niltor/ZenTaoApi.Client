using ZenTaoApi.Client.Models.RequestDtos;
using ZenTaoApi.Client.Models.ResponseDtos;

namespace ZenTaoApi.Client.Clients;

public sealed class ProjectSetClient : ZenTaoClientBase
{
    internal ProjectSetClient(ZenTaoClient client) : base(client) { }

    public Task<ProjectSetListResponse> GetListAsync(
        ProjectSetListRequest? request = null,
        CancellationToken cancellationToken = default)
        => GetAsync<ProjectSetListResponse>("programs", request, cancellationToken);

    public Task<ProjectSetDetailResponse> GetByIdAsync(long projectSetId, CancellationToken cancellationToken = default)
        => GetAsync<ProjectSetDetailResponse>($"programs/{Id(projectSetId)}", null, cancellationToken);

    public Task<MutationResponse> CreateAsync(CreateProjectSetRequest request, CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>("programs", request, cancellationToken);

    public Task<MutationResponse> UpdateAsync(long projectSetId, UpdateProjectSetRequest request, CancellationToken cancellationToken = default)
        => PutAsync<MutationResponse>($"programs/{Id(projectSetId)}", request, cancellationToken);

    public Task<MutationResponse> DeleteAsync(long projectSetId, CancellationToken cancellationToken = default)
        => base.DeleteAsync<MutationResponse>($"programs/{Id(projectSetId)}", cancellationToken);
}
