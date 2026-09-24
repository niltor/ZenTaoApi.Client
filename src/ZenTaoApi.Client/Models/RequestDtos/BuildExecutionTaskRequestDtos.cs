namespace ZenTaoApi.Client.Models.RequestDtos;

public sealed class BuildListRequest : ZenTaoListRequestDto
{
}

public sealed class CreateBuildRequest : ZenTaoRequestDto
{
    public required string Name { get; init; }
    public string? Build { get; init; }
    public string? Date { get; init; }
    public string? Builder { get; init; }
    public string? ScmPath { get; init; }
    public string? FilePath { get; init; }
    [JsonPropertyName("desc")]
    public string? Description { get; init; }
    [JsonPropertyName("product")]
    public long? ProductId { get; init; }
}

public sealed class UpdateBuildRequest : ZenTaoRequestDto
{
    public string? Name { get; init; }
    public string? Build { get; init; }
    public string? Date { get; init; }
    public string? ScmPath { get; init; }
    public string? FilePath { get; init; }
    [JsonPropertyName("desc")]
    public string? Description { get; init; }
}

public sealed class ExecutionListRequest : ZenTaoListRequestDto
{
}

public sealed class CreateExecutionRequest : ZenTaoRequestDto
{
    public required string Name { get; init; }
    public string? Code { get; init; }
    public string? Begin { get; init; }
    public string? End { get; init; }
    [JsonPropertyName("project")]
    public long? ProjectId { get; init; }
    public string? Lifetime { get; init; }
    public int? Days { get; init; }
    public string? Team { get; init; }
    [JsonPropertyName("PM")]
    public string? ProjectManager { get; init; }
    [JsonPropertyName("desc")]
    public string? Description { get; init; }
}

public sealed class UpdateExecutionRequest : ZenTaoRequestDto
{
    public string? Name { get; init; }
    public string? Code { get; init; }
    public string? Begin { get; init; }
    public string? End { get; init; }
    public string? Team { get; init; }
    [JsonPropertyName("PM")]
    public string? ProjectManager { get; init; }
    [JsonPropertyName("desc")]
    public string? Description { get; init; }
    public string? Status { get; init; }
}

public sealed class TaskListRequest : ZenTaoListRequestDto
{
    public string? AssignedTo { get; init; }
    public string? Type { get; init; }

    internal override IEnumerable<KeyValuePair<string, string?>> ToQueryParameters()
    {
        foreach (var item in base.ToQueryParameters())
        {
            yield return item;
        }

        yield return new("assignedTo", AssignedTo);
        yield return new("type", Type);
    }
}

public sealed class CreateTaskRequest : ZenTaoRequestDto
{
    public required string Name { get; init; }
    public string? Type { get; init; }
    [JsonPropertyName("pri")]
    public int? Priority { get; init; }
    public string? AssignedTo { get; init; }
    [JsonPropertyName("desc")]
    public string? Description { get; init; }
    [JsonPropertyName("story")]
    public long? StoryId { get; init; }
    [JsonPropertyName("module")]
    public long? ModuleId { get; init; }
    public string? Deadline { get; init; }
    public decimal? Estimate { get; init; }
}

public sealed class UpdateTaskRequest : ZenTaoRequestDto
{
    public string? Name { get; init; }
    public string? Type { get; init; }
    [JsonPropertyName("pri")]
    public int? Priority { get; init; }
    public string? AssignedTo { get; init; }
    [JsonPropertyName("desc")]
    public string? Description { get; init; }
    public string? Deadline { get; init; }
    public decimal? Estimate { get; init; }
    public decimal? Consumed { get; init; }
    public decimal? Left { get; init; }
}

public sealed class StartTaskRequest : ZenTaoRequestDto
{
    public decimal? Consumed { get; init; }
    public decimal? Left { get; init; }
    public string? Comment { get; init; }
}

public sealed class PauseTaskRequest : ZenTaoRequestDto
{
    public string? Comment { get; init; }
}

public sealed class ResumeTaskRequest : ZenTaoRequestDto
{
    public string? Comment { get; init; }
}

public sealed class FinishTaskRequest : ZenTaoRequestDto
{
    public decimal? Consumed { get; init; }
    public decimal? Left { get; init; }
    public string? Comment { get; init; }
}

public sealed class CloseTaskRequest : ZenTaoRequestDto
{
    public string? Comment { get; init; }
}

public sealed class AddTaskEstimateRequest : ZenTaoRequestDto
{
    public decimal? Consumed { get; init; }
    public decimal? Left { get; init; }
    public string? Date { get; init; }
    public string? Work { get; init; }
}
