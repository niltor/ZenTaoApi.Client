namespace ZenTaoApi.Client.Models.RequestDtos;

public sealed class DepartmentListRequest : ZenTaoListRequestDto
{
    public long? ParentId { get; init; }

    internal override IEnumerable<KeyValuePair<string, string?>> ToQueryParameters()
    {
        foreach (var item in base.ToQueryParameters())
        {
            yield return item;
        }

        yield return new("parent", ParentId?.ToString(CultureInfo.InvariantCulture));
    }
}
