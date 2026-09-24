using ZenTaoApi.Client.Models.RequestDtos;
using ZenTaoApi.Client.Models.ResponseDtos;

namespace ZenTaoApi.Client.Clients;

public sealed class TestTaskClient : ZenTaoClientBase
{
    internal TestTaskClient(ZenTaoClient client) : base(client) { }

    public Task<TestTaskListResponse> GetListAsync(TestTaskListRequest? request = null, CancellationToken cancellationToken = default)
        => GetAsync<TestTaskListResponse>("testtasks", request, cancellationToken);

    public Task<TestTaskListResponse> GetForProjectAsync(long projectId, TestTaskListRequest? request = null, CancellationToken cancellationToken = default)
        => GetAsync<TestTaskListResponse>($"projects/{Id(projectId)}/testtasks", request, cancellationToken);

    public Task<TestTaskDetailResponse> GetByIdAsync(long testTaskId, CancellationToken cancellationToken = default)
        => GetAsync<TestTaskDetailResponse>($"testtasks/{Id(testTaskId)}", null, cancellationToken);
}
