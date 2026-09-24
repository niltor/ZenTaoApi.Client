using ZenTaoApi.Client.Models.RequestDtos;
using ZenTaoApi.Client.Models.ResponseDtos;

namespace ZenTaoApi.Client.Clients;

public sealed class TicketClient : ZenTaoClientBase
{
    internal TicketClient(ZenTaoClient client) : base(client) { }

    public Task<TicketListResponse> GetListAsync(TicketListRequest? request = null, CancellationToken cancellationToken = default)
        => GetAsync<TicketListResponse>("tickets", request, cancellationToken);

    public Task<TicketDetailResponse> GetByIdAsync(long ticketId, CancellationToken cancellationToken = default)
        => GetAsync<TicketDetailResponse>($"tickets/{Id(ticketId)}", null, cancellationToken);

    public Task<MutationResponse> CreateAsync(CreateTicketRequest request, CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>("tickets", request, cancellationToken);

    public Task<MutationResponse> UpdateAsync(long ticketId, UpdateTicketRequest request, CancellationToken cancellationToken = default)
        => PutAsync<MutationResponse>($"tickets/{Id(ticketId)}", request, cancellationToken);

    public Task<MutationResponse> DeleteAsync(long ticketId, CancellationToken cancellationToken = default)
        => base.DeleteAsync<MutationResponse>($"tickets/{Id(ticketId)}", cancellationToken);
}
