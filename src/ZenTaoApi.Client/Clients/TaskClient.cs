using ZenTaoApi.Client.Models.RequestDtos;
using ZenTaoApi.Client.Models.ResponseDtos;

namespace ZenTaoApi.Client.Clients;

public sealed class TaskClient : ZenTaoClientBase
{
    internal TaskClient(ZenTaoClient client) : base(client) { }

    public Task<TaskListResponse> GetForExecutionAsync(long executionId, TaskListRequest? request = null, CancellationToken cancellationToken = default)
        => GetAsync<TaskListResponse>($"executions/{Id(executionId)}/tasks", request, cancellationToken);

    public Task<MutationResponse> CreateAsync(long executionId, CreateTaskRequest request, CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>($"executions/{Id(executionId)}/tasks", request, cancellationToken);

    public Task<TaskDetailResponse> GetByIdAsync(long taskId, CancellationToken cancellationToken = default)
        => GetAsync<TaskDetailResponse>($"tasks/{Id(taskId)}", null, cancellationToken);

    public Task<MutationResponse> UpdateAsync(long taskId, UpdateTaskRequest request, CancellationToken cancellationToken = default)
        => PutAsync<MutationResponse>($"tasks/{Id(taskId)}", request, cancellationToken);

    public Task<MutationResponse> DeleteAsync(long taskId, CancellationToken cancellationToken = default)
        => base.DeleteAsync<MutationResponse>($"tasks/{Id(taskId)}", cancellationToken);

    public Task<MutationResponse> StartAsync(long taskId, StartTaskRequest? request = null, CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>($"tasks/{Id(taskId)}/start", request, cancellationToken);

    public Task<MutationResponse> PauseAsync(long taskId, PauseTaskRequest? request = null, CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>($"tasks/{Id(taskId)}/pause", request, cancellationToken);

    /// <remarks>The V1 route is named <c>restart</c> in the official documentation.</remarks>
    public Task<MutationResponse> ResumeAsync(long taskId, ResumeTaskRequest? request = null, CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>($"tasks/{Id(taskId)}/restart", request, cancellationToken);

    public Task<MutationResponse> FinishAsync(long taskId, FinishTaskRequest? request = null, CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>($"tasks/{Id(taskId)}/finish", request, cancellationToken);

    public Task<MutationResponse> CloseAsync(long taskId, CloseTaskRequest? request = null, CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>($"tasks/{Id(taskId)}/close", request, cancellationToken);

    public Task<MutationResponse> AddEstimateAsync(long taskId, AddTaskEstimateRequest request, CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>($"tasks/{Id(taskId)}/estimate", request, cancellationToken);

    public Task<TaskEstimateListResponse> GetEstimatesAsync(long taskId, TaskListRequest? request = null, CancellationToken cancellationToken = default)
        => GetAsync<TaskEstimateListResponse>($"tasks/{Id(taskId)}/estimate", request, cancellationToken);
}
