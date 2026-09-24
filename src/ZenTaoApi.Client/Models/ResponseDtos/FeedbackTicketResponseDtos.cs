namespace ZenTaoApi.Client.Models.ResponseDtos;

public sealed class FeedbackResponse : ZenTaoEntityResponse
{
    public long? Product { get; set; }
    public long? Module { get; set; }
    public string? Type { get; set; }
    public string? Contact { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? AssignedTo { get; set; }
}

public sealed class FeedbackListResponse : ZenTaoListResponse<FeedbackResponse>
{
    public List<FeedbackResponse> Feedbacks { get; set; } = [];
    public override IReadOnlyList<FeedbackResponse> Items => Feedbacks;
}

public sealed class FeedbackDetailResponse : ZenTaoResponseDto
{
    public FeedbackResponse? Feedback { get; set; }
}

public sealed class TicketResponse : ZenTaoEntityResponse
{
    public long? Product { get; set; }
    public string? Type { get; set; }
    public string? Source { get; set; }
    public string? Contact { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? AssignedTo { get; set; }
    public int? Priority { get; set; }
}

public sealed class TicketListResponse : ZenTaoListResponse<TicketResponse>
{
    public List<TicketResponse> Tickets { get; set; } = [];
    public override IReadOnlyList<TicketResponse> Items => Tickets;
}

public sealed class TicketDetailResponse : ZenTaoResponseDto
{
    public TicketResponse? Ticket { get; set; }
}
