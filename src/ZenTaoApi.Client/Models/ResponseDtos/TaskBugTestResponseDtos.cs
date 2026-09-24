namespace ZenTaoApi.Client.Models.ResponseDtos;

public sealed class TaskResponse : ZenTaoEntityResponse
{
    public long? Execution { get; set; }
    public long? Project { get; set; }
    public long? Story { get; set; }
    public long? Module { get; set; }
    public string? Type { get; set; }
    public int? Priority { get; set; }
    public string? AssignedTo { get; set; }
    public string? Deadline { get; set; }
    public decimal? Estimate { get; set; }
    public decimal? Consumed { get; set; }
    public decimal? Left { get; set; }
}

public sealed class TaskListResponse : ZenTaoListResponse<TaskResponse>
{
    public List<TaskResponse> Tasks { get; set; } = [];
    public override IReadOnlyList<TaskResponse> Items => Tasks;
}

public sealed class TaskDetailResponse : ZenTaoResponseDto
{
    public TaskResponse? Task { get; set; }
}

public sealed class TaskEstimateResponse : ZenTaoResponseDto
{
    public long? Id { get; set; }
    public long? Task { get; set; }
    public decimal? Consumed { get; set; }
    public decimal? Left { get; set; }
    public string? Date { get; set; }
    public string? Work { get; set; }
}

public sealed class TaskEstimateListResponse : ZenTaoListResponse<TaskEstimateResponse>
{
    public List<TaskEstimateResponse> Estimates { get; set; } = [];
    public override IReadOnlyList<TaskEstimateResponse> Items => Estimates;
}

public sealed class BugResponse : ZenTaoEntityResponse
{
    public long? Product { get; set; }
    public long? Project { get; set; }
    public long? Execution { get; set; }
    public long? Module { get; set; }
    public string? Steps { get; set; }
    public int? Severity { get; set; }
    public int? Priority { get; set; }
    public string? Type { get; set; }
    public string? AssignedTo { get; set; }
    public string? Resolution { get; set; }
    public string? ResolvedBy { get; set; }
    public string? Deadline { get; set; }
}

public sealed class BugListResponse : ZenTaoListResponse<BugResponse>
{
    public List<BugResponse> Bugs { get; set; } = [];
    public override IReadOnlyList<BugResponse> Items => Bugs;
}

public sealed class BugDetailResponse : ZenTaoResponseDto
{
    public BugResponse? Bug { get; set; }
}

public sealed class TestCaseResponse : ZenTaoEntityResponse
{
    public long? Product { get; set; }
    public long? Module { get; set; }
    public string? Precondition { get; set; }
    public string? Steps { get; set; }
    public int? Priority { get; set; }
    public string? Type { get; set; }
    public string? Stage { get; set; }
    public string? Frequency { get; set; }
}

public sealed class TestCaseListResponse : ZenTaoListResponse<TestCaseResponse>
{
    public List<TestCaseResponse> TestCases { get; set; } = [];
    public override IReadOnlyList<TestCaseResponse> Items => TestCases;
}

public sealed class TestCaseDetailResponse : ZenTaoResponseDto
{
    public TestCaseResponse? TestCase { get; set; }
}

public sealed class TestTaskResponse : ZenTaoEntityResponse
{
    public long? Product { get; set; }
    public long? Project { get; set; }
    public long? Execution { get; set; }
    public string? Begin { get; set; }
    public string? End { get; set; }
    public string? Build { get; set; }
}

public sealed class TestTaskListResponse : ZenTaoListResponse<TestTaskResponse>
{
    public List<TestTaskResponse> TestTasks { get; set; } = [];
    public override IReadOnlyList<TestTaskResponse> Items => TestTasks;
}

public sealed class TestTaskDetailResponse : ZenTaoResponseDto
{
    public TestTaskResponse? TestTask { get; set; }
}
