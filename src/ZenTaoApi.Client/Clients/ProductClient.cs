using ZenTaoApi.Client.Models.RequestDtos;
using ZenTaoApi.Client.Models.ResponseDtos;

namespace ZenTaoApi.Client.Clients;

public sealed class ProductClient : ZenTaoClientBase
{
    internal ProductClient(ZenTaoClient client) : base(client) { }

    public Task<ProductListResponse> GetListAsync(
        ProductListRequest? request = null,
        CancellationToken cancellationToken = default)
        => GetAsync<ProductListResponse>("products", request, cancellationToken);

    public Task<ProductDetailResponse> GetByIdAsync(long productId, CancellationToken cancellationToken = default)
        => GetAsync<ProductDetailResponse>($"products/{Id(productId)}", null, cancellationToken);

    public Task<MutationResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>("products", request, cancellationToken);

    /// <remarks>The route follows the official V1 documentation: <c>PUT /product/id</c>.</remarks>
    public Task<MutationResponse> UpdateAsync(long productId, UpdateProductRequest request, CancellationToken cancellationToken = default)
        => PutAsync<MutationResponse>($"product/{Id(productId)}", request, cancellationToken);

    public Task<MutationResponse> DeleteAsync(long productId, CancellationToken cancellationToken = default)
        => base.DeleteAsync<MutationResponse>($"products/{Id(productId)}", cancellationToken);
}
