namespace ZenTaoApi.Client.Models.ResponseDtos;

/// <summary>Base response DTO. Unknown fields are preserved for forward compatibility.</summary>
public abstract class ZenTaoResponseDto
{
    [JsonExtensionData]
    public Dictionary<string, JsonElement>? AdditionalProperties { get; set; }
}

public abstract class ZenTaoEntityResponse : ZenTaoResponseDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Status { get; set; }
    [JsonPropertyName("desc")]
    public string? Description { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? CreatedDate { get; set; }
    public string? LastEditedBy { get; set; }
    public DateTimeOffset? LastEditedDate { get; set; }
}

public class MutationResponse : ZenTaoResponseDto
{
    public long? Id { get; set; }
    public string? Message { get; set; }
    public string? Status { get; set; }
}

public sealed class TokenResponse : ZenTaoResponseDto
{
    public string? Token { get; set; }
}

public abstract class ZenTaoListResponse<TItem> : ZenTaoResponseDto
    where TItem : ZenTaoResponseDto
{
    public int? Total { get; set; }
    public int? Page { get; set; }
    public int? Limit { get; set; }
    public abstract IReadOnlyList<TItem> Items { get; }
}
