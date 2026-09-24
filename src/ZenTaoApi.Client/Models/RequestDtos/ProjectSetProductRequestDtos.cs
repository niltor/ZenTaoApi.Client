namespace ZenTaoApi.Client.Models.RequestDtos;

public sealed class ProjectSetListRequest : ZenTaoListRequestDto
{
}

public sealed class CreateProjectSetRequest : ZenTaoRequestDto
{
    public required string Name { get; init; }
    public string? Code { get; init; }
    [JsonPropertyName("desc")]
    public string? Description { get; init; }
    public string? Begin { get; init; }
    public string? End { get; init; }
    [JsonPropertyName("products")]
    public string? ProductIds { get; init; }
}

public sealed class UpdateProjectSetRequest : ZenTaoRequestDto
{
    public string? Name { get; init; }
    public string? Code { get; init; }
    [JsonPropertyName("desc")]
    public string? Description { get; init; }
    public string? Begin { get; init; }
    public string? End { get; init; }
    public string? Status { get; init; }
}

public sealed class ProductListRequest : ZenTaoListRequestDto
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

public sealed class CreateProductRequest : ZenTaoRequestDto
{
    public required string Name { get; init; }
    public string? Code { get; init; }
    [JsonPropertyName("program")]
    public long? ProjectSetId { get; init; }
    [JsonPropertyName("desc")]
    public string? Description { get; init; }
    public string? Type { get; init; }
    public string? Status { get; init; }
    [JsonPropertyName("PO")]
    public string? ProductOwner { get; init; }
    [JsonPropertyName("QD")]
    public string? Qa { get; init; }
    [JsonPropertyName("RD")]
    public string? ReleaseDeveloper { get; init; }
    public string? Begin { get; init; }
    public string? End { get; init; }
}

public sealed class UpdateProductRequest : ZenTaoRequestDto
{
    public string? Name { get; init; }
    public string? Code { get; init; }
    [JsonPropertyName("desc")]
    public string? Description { get; init; }
    public string? Type { get; init; }
    public string? Status { get; init; }
    [JsonPropertyName("PO")]
    public string? ProductOwner { get; init; }
    [JsonPropertyName("QD")]
    public string? Qa { get; init; }
    [JsonPropertyName("RD")]
    public string? ReleaseDeveloper { get; init; }
    public string? Begin { get; init; }
    public string? End { get; init; }
}

public sealed class ProductPlanListRequest : ZenTaoListRequestDto
{
}

public sealed class CreateProductPlanRequest : ZenTaoRequestDto
{
    public required string Name { get; init; }
    [JsonPropertyName("desc")]
    public string? Description { get; init; }
    public string? Begin { get; init; }
    public string? End { get; init; }
    public string? Status { get; init; }
    public string? Attribute { get; init; }
    public bool? Milestone { get; init; }
    public int? Order { get; init; }
}

public sealed class UpdateProductPlanRequest : ZenTaoRequestDto
{
    public string? Name { get; init; }
    [JsonPropertyName("desc")]
    public string? Description { get; init; }
    public string? Begin { get; init; }
    public string? End { get; init; }
    public string? Status { get; init; }
    public bool? Milestone { get; init; }
    public int? Order { get; init; }
}
