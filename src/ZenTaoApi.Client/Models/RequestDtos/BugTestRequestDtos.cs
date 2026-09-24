namespace ZenTaoApi.Client.Models.RequestDtos;

public sealed class BugListRequest : ZenTaoListRequestDto
{
    public string? Severity { get; init; }
    public string? Priority { get; init; }
    public string? AssignedTo { get; init; }

    internal override IEnumerable<KeyValuePair<string, string?>> ToQueryParameters()
    {
        foreach (var item in base.ToQueryParameters())
        {
            yield return item;
        }

        yield return new("severity", Severity);
        yield return new("pri", Priority);
        yield return new("assignedTo", AssignedTo);
    }
}

public sealed class CreateBugRequest : ZenTaoRequestDto
{
    [JsonPropertyName("product")]
    public required long ProductId { get; init; }
    [JsonPropertyName("module")]
    public long? ModuleId { get; init; }
    [JsonPropertyName("project")]
    public long? ProjectId { get; init; }
    [JsonPropertyName("execution")]
    public long? ExecutionId { get; init; }
    public required string Title { get; init; }
    public string? Steps { get; init; }
    public int? Severity { get; init; }
    [JsonPropertyName("pri")]
    public int? Priority { get; init; }
    public string? Type { get; init; }
    public string? AssignedTo { get; init; }
    public string? Deadline { get; init; }
    [JsonPropertyName("story")]
    public long? RequirementId { get; init; }
    [JsonPropertyName("task")]
    public long? TaskId { get; init; }
    public string? Keywords { get; init; }
}

public sealed class UpdateBugRequest : ZenTaoRequestDto
{
    public string? Title { get; init; }
    public string? Steps { get; init; }
    public int? Severity { get; init; }
    [JsonPropertyName("pri")]
    public int? Priority { get; init; }
    public string? Type { get; init; }
    public string? Status { get; init; }
    public string? AssignedTo { get; init; }
    public string? Deadline { get; init; }
    public string? Keywords { get; init; }
}

public sealed class ConfirmBugRequest : ZenTaoRequestDto
{
    public string? Comment { get; init; }
}

public sealed class CloseBugRequest : ZenTaoRequestDto
{
    public string? Comment { get; init; }
}

public sealed class ActivateBugRequest : ZenTaoRequestDto
{
    public string? Comment { get; init; }
}

public sealed class ResolveBugRequest : ZenTaoRequestDto
{
    public required string Resolution { get; init; }
    public string? ResolvedBy { get; init; }
    public long? DuplicateBugId { get; init; }
    public string? Comment { get; init; }
}

public sealed class TestCaseListRequest : ZenTaoListRequestDto
{
    public long? ModuleId { get; init; }
    public string? Type { get; init; }

    internal override IEnumerable<KeyValuePair<string, string?>> ToQueryParameters()
    {
        foreach (var item in base.ToQueryParameters())
        {
            yield return item;
        }

        yield return new("module", ModuleId?.ToString(CultureInfo.InvariantCulture));
        yield return new("type", Type);
    }
}

public sealed class CreateTestCaseRequest : ZenTaoRequestDto
{
    [JsonPropertyName("product")]
    public required long ProductId { get; init; }
    [JsonPropertyName("module")]
    public long? ModuleId { get; init; }
    public required string Title { get; init; }
    public string? Precondition { get; init; }
    public string? Steps { get; init; }
    [JsonPropertyName("pri")]
    public int? Priority { get; init; }
    public string? Type { get; init; }
    public string? Stage { get; init; }
    public string? Frequency { get; init; }
    public string? Keywords { get; init; }
}

public sealed class UpdateTestCaseRequest : ZenTaoRequestDto
{
    public string? Title { get; init; }
    public string? Precondition { get; init; }
    public string? Steps { get; init; }
    [JsonPropertyName("pri")]
    public int? Priority { get; init; }
    public string? Type { get; init; }
    public string? Stage { get; init; }
    public string? Frequency { get; init; }
    public string? Keywords { get; init; }
}

public sealed class RunTestCaseRequest : ZenTaoRequestDto
{
    public long? TestTaskId { get; init; }
    public string? Version { get; init; }
    public string? Result { get; init; }
    public string? Comment { get; init; }
}

public sealed class TestTaskListRequest : ZenTaoListRequestDto
{
    public long? ProductId { get; init; }
    public long? ProjectId { get; init; }
    public long? ExecutionId { get; init; }

    internal override IEnumerable<KeyValuePair<string, string?>> ToQueryParameters()
    {
        foreach (var item in base.ToQueryParameters())
        {
            yield return item;
        }

        yield return new("product", ProductId?.ToString(CultureInfo.InvariantCulture));
        yield return new("project", ProjectId?.ToString(CultureInfo.InvariantCulture));
        yield return new("execution", ExecutionId?.ToString(CultureInfo.InvariantCulture));
    }
}
