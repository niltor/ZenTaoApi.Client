using ZenTaoApi.Client.Models.RequestDtos;
using ZenTaoApi.Client.Models.ResponseDtos;

namespace ZenTaoApi.Client.Clients;

public sealed class ProjectClient : ZenTaoClientBase
{
    internal ProjectClient(ZenTaoClient client) : base(client) { }

    public Task<ProjectListResponse> GetListAsync(ProjectListRequest? request = null, CancellationToken cancellationToken = default)
        => GetAsync<ProjectListResponse>("projects", request, cancellationToken);

    public Task<ProjectDetailResponse> GetByIdAsync(long projectId, CancellationToken cancellationToken = default)
        => GetAsync<ProjectDetailResponse>($"projects/{Id(projectId)}", null, cancellationToken);

    public Task<MutationResponse> CreateAsync(CreateProjectRequest request, CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>("projects", request, cancellationToken);

    public Task<MutationResponse> UpdateAsync(long projectId, UpdateProjectRequest request, CancellationToken cancellationToken = default)
        => PutAsync<MutationResponse>($"projects/{Id(projectId)}", request, cancellationToken);

    public Task<MutationResponse> DeleteAsync(long projectId, CancellationToken cancellationToken = default)
        => base.DeleteAsync<MutationResponse>($"projects/{Id(projectId)}", cancellationToken);
}
