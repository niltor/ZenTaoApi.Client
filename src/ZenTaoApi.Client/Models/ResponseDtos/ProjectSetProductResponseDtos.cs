namespace ZenTaoApi.Client.Models.ResponseDtos;

public sealed class ProjectSetResponse : ZenTaoEntityResponse
{
    public string? Begin { get; set; }
    public string? End { get; set; }
    [JsonPropertyName("products")]
    public string? ProductIds { get; set; }
}

public sealed class ProjectSetListResponse : ZenTaoListResponse<ProjectSetResponse>
{
    public List<ProjectSetResponse> Programs { get; set; } = [];
    public override IReadOnlyList<ProjectSetResponse> Items => Programs;
}

public sealed class ProjectSetDetailResponse : ZenTaoResponseDto
{
    public ProjectSetResponse? Program { get; set; }
}

public sealed class ProductResponse : ZenTaoEntityResponse
{
    [JsonPropertyName("program")]
    public long? Program { get; set; }
    public string? Type { get; set; }
    [JsonPropertyName("PO")]
    public string? ProductOwner { get; set; }
    [JsonPropertyName("QD")]
    public string? Qa { get; set; }
    [JsonPropertyName("RD")]
    public string? ReleaseDeveloper { get; set; }
    public string? Begin { get; set; }
    public string? End { get; set; }
}

public sealed class ProductListResponse : ZenTaoListResponse<ProductResponse>
{
    public List<ProductResponse> Products { get; set; } = [];
    public override IReadOnlyList<ProductResponse> Items => Products;
}

public sealed class ProductDetailResponse : ZenTaoResponseDto
{
    public ProductResponse? Product { get; set; }
}

public sealed class ProductPlanResponse : ZenTaoEntityResponse
{
    [JsonPropertyName("product")]
    public long? Product { get; set; }
    public string? Begin { get; set; }
    public string? End { get; set; }
    public string? Attribute { get; set; }
    public bool? Milestone { get; set; }
    public int? Order { get; set; }
}

public sealed class ProductPlanListResponse : ZenTaoListResponse<ProductPlanResponse>
{
    public List<ProductPlanResponse> Plans { get; set; } = [];
    public override IReadOnlyList<ProductPlanResponse> Items => Plans;
}

public sealed class ProductPlanDetailResponse : ZenTaoResponseDto
{
    public ProductPlanResponse? Plan { get; set; }
}

public sealed class ReleaseResponse : ZenTaoEntityResponse
{
    public long? Product { get; set; }
    public long? Project { get; set; }
    public string? Date { get; set; }
}

public sealed class ReleaseListResponse : ZenTaoListResponse<ReleaseResponse>
{
    public List<ReleaseResponse> Releases { get; set; } = [];
    public override IReadOnlyList<ReleaseResponse> Items => Releases;
}
