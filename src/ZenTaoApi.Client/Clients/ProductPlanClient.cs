using ZenTaoApi.Client.Models.RequestDtos;
using ZenTaoApi.Client.Models.ResponseDtos;

namespace ZenTaoApi.Client.Clients;

public sealed class ProductPlanClient : ZenTaoClientBase
{
    internal ProductPlanClient(ZenTaoClient client) : base(client) { }

    public Task<ProductPlanListResponse> GetListAsync(
        long productId,
        ProductPlanListRequest? request = null,
        CancellationToken cancellationToken = default)
        => GetAsync<ProductPlanListResponse>($"products/{Id(productId)}/plans", request, cancellationToken);

    public Task<ProductPlanDetailResponse> GetByIdAsync(long planId, CancellationToken cancellationToken = default)
        => GetAsync<ProductPlanDetailResponse>($"productplans/{Id(planId)}", null, cancellationToken);

    public Task<MutationResponse> CreateAsync(long productId, CreateProductPlanRequest request, CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>($"products/{Id(productId)}/plans", request, cancellationToken);

    public Task<MutationResponse> UpdateAsync(long planId, UpdateProductPlanRequest request, CancellationToken cancellationToken = default)
        => PutAsync<MutationResponse>($"productplans/{Id(planId)}", request, cancellationToken);

    /// <remarks>The route follows the official V1 documentation: <c>DELETE /productsplan/id</c>.</remarks>
    public Task<MutationResponse> DeleteAsync(long planId, CancellationToken cancellationToken = default)
        => base.DeleteAsync<MutationResponse>($"productsplan/{Id(planId)}", cancellationToken);

    public Task<MutationResponse> LinkStoriesAsync(long planId, LinkStoriesRequest request, CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>($"productplans/{Id(planId)}/linkstories", request, cancellationToken);

    public Task<MutationResponse> UnlinkStoriesAsync(long planId, LinkStoriesRequest request, CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>($"productplans/{Id(planId)}/unlinkstories", request, cancellationToken);

    public Task<MutationResponse> LinkBugsAsync(long planId, LinkBugsRequest request, CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>($"productplans/{Id(planId)}/linkbugs", request, cancellationToken);

    public Task<MutationResponse> UnlinkBugsAsync(long planId, LinkBugsRequest request, CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>($"productplans/{Id(planId)}/unlinkbugs", request, cancellationToken);
}
