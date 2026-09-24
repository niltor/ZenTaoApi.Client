namespace ZenTaoApi.Client.Models.RequestDtos;

public sealed class ReleaseListRequest : ZenTaoListRequestDto
{
}

public sealed class RequirementListRequest : ZenTaoListRequestDto
{
    public long? ProductId { get; init; }
    public long? ProjectId { get; init; }
    public long? ExecutionId { get; init; }
    public long? ModuleId { get; init; }
    public string? Branch { get; init; }
    public string? Type { get; init; }

    internal override IEnumerable<KeyValuePair<string, string?>> ToQueryParameters()
    {
        foreach (var item in base.ToQueryParameters())
        {
            yield return item;
        }

        yield return new("product", ProductId?.ToString(CultureInfo.InvariantCulture));
        yield return new("project", ProjectId?.ToString(CultureInfo.InvariantCulture));
        yield return new("execution", ExecutionId?.ToString(CultureInfo.InvariantCulture));
        yield return new("module", ModuleId?.ToString(CultureInfo.InvariantCulture));
        yield return new("branch", Branch);
        yield return new("type", Type);
    }
}

public sealed class CreateRequirementRequest : ZenTaoRequestDto
{
    [JsonPropertyName("product")]
    public required long ProductId { get; init; }
    public long? Branch { get; init; }
    [JsonPropertyName("module")]
    public long? ModuleId { get; init; }
    public string? Source { get; init; }
    [JsonPropertyName("sourcenote")]
    public string? SourceNote { get; init; }
    public required string Title { get; init; }
    [JsonPropertyName("spec")]
    public string? Specification { get; init; }
    public string? Verify { get; init; }
    [JsonPropertyName("pri")]
    public int? Priority { get; init; }
    public decimal? Estimate { get; init; }
    public string? Keywords { get; init; }
    [JsonPropertyName("mailto")]
    public string? MailTo { get; init; }
    public string? AssignedTo { get; init; }
    public string? Deadline { get; init; }
}

public sealed class ChangeRequirementRequest : ZenTaoRequestDto
{
    public string? Title { get; init; }
    [JsonPropertyName("spec")]
    public string? Specification { get; init; }
    public string? Verify { get; init; }
    public string? AssignedTo { get; init; }
    public string? Comment { get; init; }
}

public sealed class UpdateRequirementRequest : ZenTaoRequestDto
{
    public string? Title { get; init; }
    public string? Status { get; init; }
    public string? Stage { get; init; }
    [JsonPropertyName("pri")]
    public int? Priority { get; init; }
    public string? AssignedTo { get; init; }
    public decimal? Estimate { get; init; }
    public string? Deadline { get; init; }
}

public sealed class ProjectListRequest : ZenTaoListRequestDto
{
    public long? ProjectSetId { get; init; }
    public string? Type { get; init; }

    internal override IEnumerable<KeyValuePair<string, string?>> ToQueryParameters()
    {
        foreach (var item in base.ToQueryParameters())
        {
            yield return item;
        }

        yield return new("program", ProjectSetId?.ToString(CultureInfo.InvariantCulture));
        yield return new("type", Type);
    }
}

public sealed class CreateProjectRequest : ZenTaoRequestDto
{
    public required string Name { get; init; }
    public string? Code { get; init; }
    [JsonPropertyName("program")]
    public long? ProjectSetId { get; init; }
    public string? Begin { get; init; }
    public string? End { get; init; }
    [JsonPropertyName("PM")]
    public string? ProjectManager { get; init; }
    [JsonPropertyName("desc")]
    public string? Description { get; init; }
    public string? Model { get; init; }
    public string? Lifetime { get; init; }
}

public sealed class UpdateProjectRequest : ZenTaoRequestDto
{
    public string? Name { get; init; }
    public string? Code { get; init; }
    public string? Begin { get; init; }
    public string? End { get; init; }
    [JsonPropertyName("PM")]
    public string? ProjectManager { get; init; }
    [JsonPropertyName("desc")]
    public string? Description { get; init; }
    public string? Status { get; init; }
}
