using ZenTaoApi.Client.Models.RequestDtos;
using ZenTaoApi.Client.Models.ResponseDtos;

namespace ZenTaoApi.Client.Clients;

public sealed class FeedbackClient : ZenTaoClientBase
{
    internal FeedbackClient(ZenTaoClient client) : base(client) { }

    public Task<FeedbackListResponse> GetListAsync(FeedbackListRequest? request = null, CancellationToken cancellationToken = default)
        => GetAsync<FeedbackListResponse>("feedbacks", request, cancellationToken);

    public Task<MutationResponse> CreateAsync(CreateFeedbackRequest request, CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>("feedbacks", request, cancellationToken);

    public Task<FeedbackDetailResponse> GetByIdAsync(long feedbackId, CancellationToken cancellationToken = default)
        => GetAsync<FeedbackDetailResponse>($"feedbacks/{Id(feedbackId)}", null, cancellationToken);

    public Task<MutationResponse> UpdateAsync(long feedbackId, UpdateFeedbackRequest request, CancellationToken cancellationToken = default)
        => PutAsync<MutationResponse>($"feedbacks/{Id(feedbackId)}", request, cancellationToken);

    public Task<MutationResponse> DeleteAsync(long feedbackId, CancellationToken cancellationToken = default)
        => base.DeleteAsync<MutationResponse>($"feedbacks/{Id(feedbackId)}", cancellationToken);

    public Task<MutationResponse> AssignAsync(long feedbackId, AssignFeedbackRequest request, CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>($"feedbacks/{Id(feedbackId)}/assign", request, cancellationToken);

    public Task<MutationResponse> CloseAsync(long feedbackId, CloseFeedbackRequest? request = null, CancellationToken cancellationToken = default)
        => PostAsync<MutationResponse>($"feedbacks/{Id(feedbackId)}/close", request, cancellationToken);
}
