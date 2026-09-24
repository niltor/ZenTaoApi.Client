namespace ZenTaoApi.Client.Models.RequestDtos;

/// <summary>Base type for all ZenTao request DTOs.</summary>
public abstract class ZenTaoRequestDto
{
}

/// <summary>Common query parameters supported by ZenTao list endpoints.</summary>
public abstract class ZenTaoListRequestDto : ZenTaoRequestDto
{
    public int? Page { get; init; }
    public int? Limit { get; init; }
    public string? Status { get; init; }
    public string? OrderBy { get; init; }
    public string? Keywords { get; init; }

    internal virtual IEnumerable<KeyValuePair<string, string?>> ToQueryParameters()
    {
        yield return new("page", Page?.ToString(CultureInfo.InvariantCulture));
        yield return new("limit", Limit?.ToString(CultureInfo.InvariantCulture));
        yield return new("status", Status);
        yield return new("orderBy", OrderBy);
        yield return new("keywords", Keywords);
    }
}

public sealed class TokenRequest : ZenTaoRequestDto
{
    [JsonPropertyName("account")]
    public required string Account { get; init; }

    [JsonPropertyName("password")]
    public required string Password { get; init; }
}

public sealed class EmptyRequest : ZenTaoRequestDto
{
}

public sealed class LinkStoriesRequest : ZenTaoRequestDto
{
    [JsonPropertyName("stories")]
    public List<long> StoryIds { get; init; } = [];
}

public sealed class LinkBugsRequest : ZenTaoRequestDto
{
    [JsonPropertyName("bugs")]
    public List<long> BugIds { get; init; } = [];
}

public sealed class CommentRequest : ZenTaoRequestDto
{
    [JsonPropertyName("comment")]
    public string? Comment { get; init; }
}
