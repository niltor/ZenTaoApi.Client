using ZenTaoApi.Client.Models.RequestDtos;
using ZenTaoApi.Client.Models.ResponseDtos;

namespace ZenTaoApi.Client.Clients;

public sealed class UserClient : ZenTaoClientBase
{
    internal UserClient(ZenTaoClient client) : base(client) { }

    public Task<CurrentUserResponse> GetCurrentAsync(CancellationToken cancellationToken = default)
        => GetAsync<CurrentUserResponse>("user", null, cancellationToken);

    public Task<UserListResponse> GetListAsync(
        UserListRequest? request = null,
        CancellationToken cancellationToken = default)
        => GetAsync<UserListResponse>("users", request, cancellationToken);

    public Task<UserDetailResponse> GetByIdAsync(long userId, CancellationToken cancellationToken = default)
        => GetAsync<UserDetailResponse>($"users/{Id(userId)}", null, cancellationToken);

    public Task<MutationResponse> CreateAsync(
        CreateUserRequest request,
        CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>("users", request, cancellationToken);

    public Task<MutationResponse> UpdateAsync(
        long userId,
        UpdateUserRequest request,
        CancellationToken cancellationToken = default)
        => PutAsync<MutationResponse>($"users/{Id(userId)}", request, cancellationToken);

    public Task<MutationResponse> DeleteAsync(long userId, CancellationToken cancellationToken = default)
        => base.DeleteAsync<MutationResponse>($"users/{Id(userId)}", cancellationToken);
}
