namespace ZenTaoApi.Client.Models.ResponseDtos;

public sealed class DepartmentResponse : ZenTaoEntityResponse
{
    public long? Parent { get; set; }
    public string? Path { get; set; }
    public int? Grade { get; set; }
    public int? Order { get; set; }
}

public sealed class DepartmentListResponse : ZenTaoListResponse<DepartmentResponse>
{
    public List<DepartmentResponse> Departments { get; set; } = [];
    public override IReadOnlyList<DepartmentResponse> Items => Departments;
}

public sealed class DepartmentDetailResponse : ZenTaoResponseDto
{
    public DepartmentResponse? Department { get; set; }
}

public sealed class UserRoleResponse : ZenTaoResponseDto
{
    public string? Code { get; set; }
    public string? Name { get; set; }
}

public sealed class UserResponse : ZenTaoEntityResponse
{
    public string? Account { get; set; }
    public string? RealName { get; set; }
    [JsonPropertyName("dept")]
    public long? Department { get; set; }
    public string? Role { get; set; }
    public string? Email { get; set; }
    public string? Gender { get; set; }
    public string? Mobile { get; set; }
    public string? Phone { get; set; }
    public string? Skype { get; set; }
    public string? Qq { get; set; }
    public string? Weixin { get; set; }
    public string? ClientStatus { get; set; }
    public bool? Admin { get; set; }
}

public sealed class UserListResponse : ZenTaoListResponse<UserResponse>
{
    public List<UserResponse> Users { get; set; } = [];
    public override IReadOnlyList<UserResponse> Items => Users;
}

public sealed class UserDetailResponse : ZenTaoResponseDto
{
    public UserResponse? User { get; set; }
}

public sealed class UserViewResponse : ZenTaoResponseDto
{
    public string? Account { get; set; }
    public string? Programs { get; set; }
    public string? Products { get; set; }
    public string? Projects { get; set; }
    public string? Sprints { get; set; }
}

public sealed class UserProfileResponse : ZenTaoResponseDto
{
    public long Id { get; set; }
    public long? Company { get; set; }
    public string? Type { get; set; }
    [JsonPropertyName("dept")]
    public long? Department { get; set; }
    public string? Account { get; set; }
    public UserRoleResponse? Role { get; set; }
    public string? RealName { get; set; }
    public string? Superior { get; set; }
    public string? Pinyin { get; set; }
    public string? Nickname { get; set; }
    public string? Commiter { get; set; }
    public string? Avatar { get; set; }
    public DateOnly? Birthday { get; set; }
    public string? Gender { get; set; }
    public string? Email { get; set; }
    public string? Skype { get; set; }
    public string? Qq { get; set; }
    public string? Mobile { get; set; }
    public string? Phone { get; set; }
    public string? Weixin { get; set; }
    public string? Dingding { get; set; }
    public string? Slack { get; set; }
    public string? Whatsapp { get; set; }
    public string? Address { get; set; }
    public string? Zipcode { get; set; }
    public string? Nature { get; set; }
    public string? Analysis { get; set; }
    public string? Strategy { get; set; }
    public DateOnly? Join { get; set; }
    public int? Visits { get; set; }
    public string? Visions { get; set; }
    public string? Ip { get; set; }
    public DateTimeOffset? Last { get; set; }
    public int? Fails { get; set; }
    public DateTimeOffset? Locked { get; set; }
    public string? Feedback { get; set; }
    public string? Ranzhi { get; set; }
    public string? Ldap { get; set; }
    public int? Score { get; set; }
    public int? ScoreLevel { get; set; }
    public string? ResetToken { get; set; }
    public string? Deleted { get; set; }
    public string? ClientStatus { get; set; }
    public string? ClientLang { get; set; }
    public bool? Admin { get; set; }
    public bool? SuperReviewer { get; set; }
    public UserViewResponse? View { get; set; }
}

public sealed class CurrentUserResponse : ZenTaoResponseDto
{
    public UserProfileResponse? Profile { get; set; }
}
