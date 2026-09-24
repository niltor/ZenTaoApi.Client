namespace ZenTaoApi.Client.Models.RequestDtos;

public sealed class UserListRequest : ZenTaoListRequestDto
{
    public long? DepartmentId { get; init; }
    public string? Role { get; init; }

    internal override IEnumerable<KeyValuePair<string, string?>> ToQueryParameters()
    {
        foreach (var item in base.ToQueryParameters())
        {
            yield return item;
        }

        yield return new("dept", DepartmentId?.ToString(CultureInfo.InvariantCulture));
        yield return new("role", Role);
    }
}

public sealed class CreateUserRequest : ZenTaoRequestDto
{
    public required string Account { get; init; }
    public required string Password { get; init; }
    public required string RealName { get; init; }
    [JsonPropertyName("dept")]
    public long? DepartmentId { get; init; }
    public string? Role { get; init; }
    public string? Email { get; init; }
    public string? Gender { get; init; }
    public string? Mobile { get; init; }
    public string? Phone { get; init; }
    public string? Skype { get; init; }
    public string? Qq { get; init; }
    public string? Weixin { get; init; }
    public string? Dingding { get; init; }
}

public sealed class UpdateUserRequest : ZenTaoRequestDto
{
    public string? RealName { get; init; }
    [JsonPropertyName("dept")]
    public long? DepartmentId { get; init; }
    public string? Role { get; init; }
    public string? Email { get; init; }
    public string? Gender { get; init; }
    public string? Mobile { get; init; }
    public string? Phone { get; init; }
    public string? Skype { get; init; }
    public string? Qq { get; init; }
    public string? Weixin { get; init; }
    public string? Dingding { get; init; }
}
