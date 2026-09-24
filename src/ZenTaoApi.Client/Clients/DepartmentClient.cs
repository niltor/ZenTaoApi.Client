using ZenTaoApi.Client.Models.RequestDtos;
using ZenTaoApi.Client.Models.ResponseDtos;

namespace ZenTaoApi.Client.Clients;

public sealed class DepartmentClient : ZenTaoClientBase
{
    internal DepartmentClient(ZenTaoClient client) : base(client) { }

    public Task<DepartmentListResponse> GetListAsync(
        DepartmentListRequest? request = null,
        CancellationToken cancellationToken = default)
        => GetAsync<DepartmentListResponse>("departments", request, cancellationToken);

    public Task<DepartmentDetailResponse> GetByIdAsync(
        long departmentId,
        CancellationToken cancellationToken = default)
        => GetAsync<DepartmentDetailResponse>($"departments/{Id(departmentId)}", null, cancellationToken);
}
