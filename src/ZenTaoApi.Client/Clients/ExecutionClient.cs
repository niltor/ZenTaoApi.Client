using ZenTaoApi.Client.Models.RequestDtos;
using ZenTaoApi.Client.Models.ResponseDtos;

namespace ZenTaoApi.Client.Clients;

public sealed class ExecutionClient : ZenTaoClientBase
{
    internal ExecutionClient(ZenTaoClient client) : base(client) { }

    public Task<ExecutionListResponse> GetForProjectAsync(long projectId, ExecutionListRequest? request = null, CancellationToken cancellationToken = default)
        => GetAsync<ExecutionListResponse>($"projects/{Id(projectId)}/executions", request, cancellationToken);

    public Task<MutationResponse> CreateAsync(long projectId, CreateExecutionRequest request, CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>($"projects/{Id(projectId)}/executions", request, cancellationToken);

    public Task<ExecutionDetailResponse> GetByIdAsync(long executionId, CancellationToken cancellationToken = default)
        => GetAsync<ExecutionDetailResponse>($"executions/{Id(executionId)}", null, cancellationToken);

    public Task<MutationResponse> UpdateAsync(long executionId, UpdateExecutionRequest request, CancellationToken cancellationToken = default)
        => PutAsync<MutationResponse>($"executions/{Id(executionId)}", request, cancellationToken);

    public Task<MutationResponse> DeleteAsync(long executionId, CancellationToken cancellationToken = default)
        => base.DeleteAsync<MutationResponse>($"executions/{Id(executionId)}", cancellationToken);
}
