using ZenTaoApi.Client.Models.RequestDtos;
using ZenTaoApi.Client.Models.ResponseDtos;

namespace ZenTaoApi.Client.Clients;

public sealed class RequirementClient : ZenTaoClientBase
{
    internal RequirementClient(ZenTaoClient client) : base(client) { }

    public Task<RequirementListResponse> GetForProductAsync(long productId, RequirementListRequest? request = null, CancellationToken cancellationToken = default)
        => GetAsync<RequirementListResponse>($"products/{Id(productId)}/stories", request, cancellationToken);

    public Task<RequirementListResponse> GetForProjectAsync(long projectId, RequirementListRequest? request = null, CancellationToken cancellationToken = default)
        => GetAsync<RequirementListResponse>($"projects/{Id(projectId)}/stories", request, cancellationToken);

    public Task<RequirementListResponse> GetForExecutionAsync(long executionId, RequirementListRequest? request = null, CancellationToken cancellationToken = default)
        => GetAsync<RequirementListResponse>($"executions/{Id(executionId)}/stories", request, cancellationToken);

    public Task<RequirementDetailResponse> GetByIdAsync(long requirementId, CancellationToken cancellationToken = default)
        => GetAsync<RequirementDetailResponse>($"stories/{Id(requirementId)}", null, cancellationToken);

    public Task<MutationResponse> CreateAsync(CreateRequirementRequest request, CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>("stories", request, cancellationToken);

    public Task<MutationResponse> ChangeAsync(long requirementId, ChangeRequirementRequest request, CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>($"stories/{Id(requirementId)}/change", request, cancellationToken);

    public Task<MutationResponse> UpdateAsync(long requirementId, UpdateRequirementRequest request, CancellationToken cancellationToken = default)
        => PutAsync<MutationResponse>($"stories/{Id(requirementId)}", request, cancellationToken);

    public Task<MutationResponse> DeleteAsync(long requirementId, CancellationToken cancellationToken = default)
        => base.DeleteAsync<MutationResponse>($"stories/{Id(requirementId)}", cancellationToken);

    public Task<MutationResponse> CloseAsync(long requirementId, CommentRequest? request = null, CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>($"stories/{Id(requirementId)}/close", request, cancellationToken);
}
