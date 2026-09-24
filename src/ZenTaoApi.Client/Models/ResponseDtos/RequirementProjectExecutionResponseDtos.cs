namespace ZenTaoApi.Client.Models.ResponseDtos;

public sealed class RequirementResponse : ZenTaoEntityResponse
{
    [JsonPropertyName("product")]
    public long? Product { get; set; }
    public long? Project { get; set; }
    public long? Execution { get; set; }
    [JsonPropertyName("module")]
    public long? Module { get; set; }
    public string? Spec { get; set; }
    public string? Verify { get; set; }
    public string? Type { get; set; }
    public string? Stage { get; set; }
    public int? Priority { get; set; }
    public decimal? Estimate { get; set; }
    public string? AssignedTo { get; set; }
    public string? Deadline { get; set; }
}

public sealed class RequirementListResponse : ZenTaoListResponse<RequirementResponse>
{
    public List<RequirementResponse> Stories { get; set; } = [];
    public override IReadOnlyList<RequirementResponse> Items => Stories;
}

public sealed class RequirementDetailResponse : ZenTaoResponseDto
{
    public RequirementResponse? Story { get; set; }
}

public sealed class ProjectResponse : ZenTaoEntityResponse
{
    [JsonPropertyName("program")]
    public long? Program { get; set; }
    public string? Begin { get; set; }
    public string? End { get; set; }
    [JsonPropertyName("PM")]
    public string? ProjectManager { get; set; }
    public string? Model { get; set; }
    public string? Lifetime { get; set; }
}

public sealed class ProjectListResponse : ZenTaoListResponse<ProjectResponse>
{
    public List<ProjectResponse> Projects { get; set; } = [];
    public override IReadOnlyList<ProjectResponse> Items => Projects;
}

public sealed class ProjectDetailResponse : ZenTaoResponseDto
{
    public ProjectResponse? Project { get; set; }
}

public sealed class BuildResponse : ZenTaoEntityResponse
{
    public long? Project { get; set; }
    public long? Product { get; set; }
    public string? Build { get; set; }
    public string? Date { get; set; }
    public string? Builder { get; set; }
    public string? ScmPath { get; set; }
    public string? FilePath { get; set; }
}

public sealed class BuildListResponse : ZenTaoListResponse<BuildResponse>
{
    public List<BuildResponse> Builds { get; set; } = [];
    public override IReadOnlyList<BuildResponse> Items => Builds;
}

public sealed class BuildDetailResponse : ZenTaoResponseDto
{
    public BuildResponse? Build { get; set; }
}

public sealed class ExecutionResponse : ZenTaoEntityResponse
{
    public long? Project { get; set; }
    public string? Begin { get; set; }
    public string? End { get; set; }
    public string? Team { get; set; }
    [JsonPropertyName("PM")]
    public string? ProjectManager { get; set; }
    public string? LifeTime { get; set; }
}

public sealed class ExecutionListResponse : ZenTaoListResponse<ExecutionResponse>
{
    public List<ExecutionResponse> Executions { get; set; } = [];
    public override IReadOnlyList<ExecutionResponse> Items => Executions;
}

public sealed class ExecutionDetailResponse : ZenTaoResponseDto
{
    public ExecutionResponse? Execution { get; set; }
}
