namespace ZenTaoApi.Client.Models.RequestDtos;

public sealed class FeedbackListRequest : ZenTaoListRequestDto
{
    public long? ProductId { get; init; }
    public string? Type { get; init; }

    internal override IEnumerable<KeyValuePair<string, string?>> ToQueryParameters()
    {
        foreach (var item in base.ToQueryParameters())
        {
            yield return item;
        }

        yield return new("product", ProductId?.ToString(CultureInfo.InvariantCulture));
        yield return new("type", Type);
    }
}

public sealed class CreateFeedbackRequest : ZenTaoRequestDto
{
    public required string Title { get; init; }
    [JsonPropertyName("product")]
    public long? ProductId { get; init; }
    [JsonPropertyName("module")]
    public long? ModuleId { get; init; }
    public string? Type { get; init; }
    [JsonPropertyName("desc")]
    public string? Description { get; init; }
    public string? Contact { get; init; }
    public string? Phone { get; init; }
    public string? Email { get; init; }
}

public sealed class UpdateFeedbackRequest : ZenTaoRequestDto
{
    public string? Title { get; init; }
    public string? Type { get; init; }
    [JsonPropertyName("desc")]
    public string? Description { get; init; }
    public string? Contact { get; init; }
    public string? Phone { get; init; }
    public string? Email { get; init; }
    public string? Status { get; init; }
}

public sealed class AssignFeedbackRequest : ZenTaoRequestDto
{
    public required string AssignedTo { get; init; }
    public string? Comment { get; init; }
}

public sealed class CloseFeedbackRequest : ZenTaoRequestDto
{
    public string? Comment { get; init; }
}

public sealed class TicketListRequest : ZenTaoListRequestDto
{
    public long? ProductId { get; init; }
    public string? Type { get; init; }

    internal override IEnumerable<KeyValuePair<string, string?>> ToQueryParameters()
    {
        foreach (var item in base.ToQueryParameters())
        {
            yield return item;
        }

        yield return new("product", ProductId?.ToString(CultureInfo.InvariantCulture));
        yield return new("type", Type);
    }
}

public sealed class CreateTicketRequest : ZenTaoRequestDto
{
    public required string Title { get; init; }
    [JsonPropertyName("product")]
    public long? ProductId { get; init; }
    public string? Type { get; init; }
    [JsonPropertyName("desc")]
    public string? Description { get; init; }
    public string? Source { get; init; }
    public string? Contact { get; init; }
    public string? Phone { get; init; }
    public string? Email { get; init; }
    public string? AssignedTo { get; init; }
    public string? Status { get; init; }
    [JsonPropertyName("pri")]
    public int? Priority { get; init; }
}

public sealed class UpdateTicketRequest : ZenTaoRequestDto
{
    public string? Title { get; init; }
    public string? Type { get; init; }
    [JsonPropertyName("desc")]
    public string? Description { get; init; }
    public string? AssignedTo { get; init; }
    public string? Status { get; init; }
    [JsonPropertyName("pri")]
    public int? Priority { get; init; }
}
