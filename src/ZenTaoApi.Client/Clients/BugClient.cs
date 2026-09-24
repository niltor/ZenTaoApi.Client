using ZenTaoApi.Client.Models.RequestDtos;
using ZenTaoApi.Client.Models.ResponseDtos;

namespace ZenTaoApi.Client.Clients;

public sealed class BugClient : ZenTaoClientBase
{
    internal BugClient(ZenTaoClient client) : base(client) { }

    public Task<BugListResponse> GetForProductAsync(long productId, BugListRequest? request = null, CancellationToken cancellationToken = default)
        => GetAsync<BugListResponse>($"products/{Id(productId)}/bugs", request, cancellationToken);

    public Task<MutationResponse> CreateAsync(long productId, CreateBugRequest request, CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>($"products/{Id(productId)}/bugs", request, cancellationToken);

    public Task<BugDetailResponse> GetByIdAsync(long bugId, CancellationToken cancellationToken = default)
        => GetAsync<BugDetailResponse>($"bugs/{Id(bugId)}", null, cancellationToken);

    public Task<MutationResponse> UpdateAsync(long bugId, UpdateBugRequest request, CancellationToken cancellationToken = default)
        => PutAsync<MutationResponse>($"bugs/{Id(bugId)}", request, cancellationToken);

    public Task<MutationResponse> DeleteAsync(long bugId, CancellationToken cancellationToken = default)
        => base.DeleteAsync<MutationResponse>($"bugs/{Id(bugId)}", cancellationToken);

    public Task<MutationResponse> ConfirmAsync(long bugId, ConfirmBugRequest? request = null, CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>($"bugs/{Id(bugId)}/confirm", request, cancellationToken);

    public Task<MutationResponse> CloseAsync(long bugId, CloseBugRequest? request = null, CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>($"bugs/{Id(bugId)}/close", request, cancellationToken);

    public Task<MutationResponse> ActivateAsync(long bugId, ActivateBugRequest? request = null, CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>($"bugs/{Id(bugId)}/active", request, cancellationToken);

    public Task<MutationResponse> ResolveAsync(long bugId, ResolveBugRequest request, CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>($"bugs/{Id(bugId)}/resolve", request, cancellationToken);
}
