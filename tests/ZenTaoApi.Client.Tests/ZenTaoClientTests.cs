using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Xunit;
using ZenTaoApi.Client;
using ZenTaoApi.Client.Models.RequestDtos;

namespace ZenTaoApi.Client.Tests;

public sealed class ZenTaoClientTests
{
    [Fact]
    public async Task CreateAsync_AuthenticatesBeforeReturningClient()
    {
        using var handler = new RecordingHandler(
            _ => JsonResponse(HttpStatusCode.OK, "{\"token\":\"initial-token\"}"),
            _ => JsonResponse(HttpStatusCode.OK, "{\"departments\":[]}"));
        using var httpClient = new HttpClient(handler);

        await using var client = await ZenTaoClient.CreateAsync(
            "https://zentao.example/api.php/v1",
            "alice",
            "secret",
            httpClient);

        Assert.Equal("initial-token", client.Token);
        var departments = await client.Departments.GetListAsync();

        Assert.Empty(departments.Departments);
        Assert.Equal(2, handler.Requests.Count);
        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal("https://zentao.example/api.php/v1/tokens", handler.Requests[0].Uri.ToString());
        Assert.Contains("\"account\":\"alice\"", handler.Requests[0].Body);
        Assert.Contains("\"password\":\"secret\"", handler.Requests[0].Body);
        Assert.Equal("Token", handler.Requests[1].Headers.Single(header => header.Key == "Token").Key);
        Assert.Equal("initial-token", handler.Requests[1].Headers["Token"].Single());
    }

    [Fact]
    public async Task DepartmentListAsync_ReadsFlatRootArrayAndSortsByParentDepth()
    {
        using var handler = new RecordingHandler(
            _ => JsonResponse(HttpStatusCode.OK, "{\"token\":\"token\"}"),
            _ => JsonResponse(HttpStatusCode.OK, """
                [
                  {"id":3,"name":"Child","parent":2},
                  {"id":1,"name":"Root","parent":0},
                  {"id":2,"name":"Group","parent":1}
                ]
                """));
        using var httpClient = new HttpClient(handler);

        await using var client = await ZenTaoClient.CreateAsync(
            "https://zentao.example/api.php/v1",
            "alice",
            "secret",
            httpClient);

        var response = await client.Departments.GetListAsync();

        Assert.Equal(new[] { "Root", "Group", "Child" }, response.Departments.Select(department => department.Name));
        Assert.Null(response.Total);
        Assert.Equal(1, response.Departments[1].Parent);
    }

    [Fact]
    public async Task DepartmentListAsync_FlattensNestedItemsAndInfersMissingParent()
    {
        using var handler = new RecordingHandler(
            _ => JsonResponse(HttpStatusCode.OK, "{\"token\":\"token\"}"),
            _ => JsonResponse(HttpStatusCode.OK, """
                [
                  {
                    "id":1,
                    "name":"Root",
                    "parent":0,
                    "items":[
                      {"id":3,"name":"Leaf","parent":2},
                      {"id":2,"name":"Group","items":[{"id":4,"name":"Team"}]}
                    ]
                  }
                ]
                """));
        using var httpClient = new HttpClient(handler);

        await using var client = await ZenTaoClient.CreateAsync(
            "https://zentao.example/api.php/v1",
            "alice",
            "secret",
            httpClient);

        var response = await client.Departments.GetListAsync();

        Assert.Equal(new[] { "Root", "Group", "Leaf", "Team" }, response.Departments.Select(department => department.Name));
        Assert.Equal(1, response.Departments.Single(department => department.Id == 2).Parent);
        Assert.Equal(2, response.Departments.Single(department => department.Id == 4).Parent);
        Assert.All(response.Departments, department =>
            Assert.True(department.AdditionalProperties is null ||
                        !department.AdditionalProperties.ContainsKey("items")));
    }

    [Fact]
    public async Task UnauthorizedResponse_RefreshesTokenAndRetriesOnce()
    {
        using var handler = new RecordingHandler(
            _ => JsonResponse(HttpStatusCode.OK, "{\"token\":\"token-one\"}"),
            _ => new HttpResponseMessage(HttpStatusCode.Unauthorized),
            _ => JsonResponse(HttpStatusCode.OK, "{\"token\":\"token-two\"}"),
            _ => JsonResponse(HttpStatusCode.OK, "{\"products\":[]}"));
        using var httpClient = new HttpClient(handler);

        await using var client = await ZenTaoClient.CreateAsync(
            "https://zentao.example/api.php/v1",
            "alice",
            "secret",
            httpClient);

        await client.Products.GetListAsync();

        Assert.Equal(4, handler.Requests.Count);
        Assert.Equal("token-one", handler.Requests[1].Headers["Token"].Single());
        Assert.Equal("token-two", handler.Requests[3].Headers["Token"].Single());
        Assert.Equal("token-two", client.Token);
    }

    [Fact]
    public async Task ResourceClient_BuildsEscapedQueryAndOfficialRoute()
    {
        using var handler = new RecordingHandler(
            _ => JsonResponse(HttpStatusCode.OK, "{\"token\":\"token\"}"),
            _ => JsonResponse(HttpStatusCode.OK, "{\"stories\":[]}"));
        using var httpClient = new HttpClient(handler);

        await using var client = await ZenTaoClient.CreateAsync(
            "https://zentao.example/api.php/v1",
            "alice",
            "secret",
            httpClient);

        await client.Requirements.GetForProductAsync(
            42,
            new RequirementListRequest { Status = "all stories", Page = 2 });

        Assert.Equal(
            "https://zentao.example/api.php/v1/products/42/stories?page=2&status=all%20stories",
            handler.Requests[1].Uri.AbsoluteUri);
    }

    [Fact]
    public async Task FailedResponse_ThrowsWithStatusAndBody()
    {
        using var handler = new RecordingHandler(
            _ => JsonResponse(HttpStatusCode.OK, "{\"token\":\"token\"}"),
            _ => JsonResponse(HttpStatusCode.NotFound, "{\"message\":\"not found\"}"));
        using var httpClient = new HttpClient(handler);

        await using var client = await ZenTaoClient.CreateAsync(
            "https://zentao.example/api.php/v1",
            "alice",
            "secret",
            httpClient);

        var exception = await Assert.ThrowsAsync<ZenTaoApiException>(
            () => client.Projects.GetByIdAsync(123));

        Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
        Assert.Contains("not found", exception.ResponseBody);
        Assert.Equal("projects/123", exception.RequestPath);
    }

    private static HttpResponseMessage JsonResponse(HttpStatusCode statusCode, string json)
    {
        return new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
    }

    private sealed class RecordingHandler : HttpMessageHandler
    {
        private readonly Queue<Func<HttpRequestMessage, HttpResponseMessage>> _responses;

        public RecordingHandler(params Func<HttpRequestMessage, HttpResponseMessage>[] responses)
        {
            _responses = new Queue<Func<HttpRequestMessage, HttpResponseMessage>>(responses);
        }

        public List<RecordedRequest> Requests { get; } = [];

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var body = request.Content is null
                ? string.Empty
                : await request.Content.ReadAsStringAsync(cancellationToken);
            Requests.Add(new RecordedRequest(
                request.Method,
                request.RequestUri!,
                body,
                request.Headers.ToDictionary(header => header.Key, header => header.Value.ToArray())));

            if (_responses.Count == 0)
            {
                throw new InvalidOperationException("No response configured for the request.");
            }

            return _responses.Dequeue()(request);
        }
    }

    private sealed record RecordedRequest(
        HttpMethod Method,
        Uri Uri,
        string Body,
        Dictionary<string, string[]> Headers);
}
