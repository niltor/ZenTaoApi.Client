using ZenTaoApi.Client.Models.RequestDtos;
using ZenTaoApi.Client.Models.ResponseDtos;

namespace ZenTaoApi.Client.Clients;

public sealed class TestCaseClient : ZenTaoClientBase
{
    internal TestCaseClient(ZenTaoClient client) : base(client) { }

    public Task<TestCaseListResponse> GetForProductAsync(long productId, TestCaseListRequest? request = null, CancellationToken cancellationToken = default)
        => GetAsync<TestCaseListResponse>($"products/{Id(productId)}/testcases", request, cancellationToken);

    public Task<MutationResponse> CreateAsync(long productId, CreateTestCaseRequest request, CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>($"products/{Id(productId)}/testcases", request, cancellationToken);

    public Task<TestCaseDetailResponse> GetByIdAsync(long testCaseId, CancellationToken cancellationToken = default)
        => GetAsync<TestCaseDetailResponse>($"testcases/{Id(testCaseId)}", null, cancellationToken);

    public Task<MutationResponse> UpdateAsync(long testCaseId, UpdateTestCaseRequest request, CancellationToken cancellationToken = default)
        => PutAsync<MutationResponse>($"testcases/{Id(testCaseId)}", request, cancellationToken);

    public Task<MutationResponse> DeleteAsync(long testCaseId, CancellationToken cancellationToken = default)
        => base.DeleteAsync<MutationResponse>($"testcases/{Id(testCaseId)}", cancellationToken);

    public Task<MutationResponse> RunAsync(long testCaseId, RunTestCaseRequest? request = null, CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>($"testcases/{Id(testCaseId)}/results", request, cancellationToken);
}
