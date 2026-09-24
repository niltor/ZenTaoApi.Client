namespace ZenTaoApi.Client;

/// <summary>Represents an unsuccessful HTTP response returned by ZenTao.</summary>
public sealed class ZenTaoApiException : Exception
{
    public ZenTaoApiException(
        HttpStatusCode statusCode,
        string requestPath,
        string responseBody)
        : base($"ZenTao API request failed with {(int)statusCode} ({statusCode}) for '{requestPath}'.")
    {
        StatusCode = statusCode;
        RequestPath = requestPath;
        ResponseBody = responseBody;
    }

    public HttpStatusCode StatusCode { get; }

    public string RequestPath { get; }

    public string ResponseBody { get; }
}

/// <summary>Thrown when ZenTao does not return a usable authentication token.</summary>
public sealed class ZenTaoAuthenticationException : Exception
{
    public ZenTaoAuthenticationException(string message)
        : base(message)
    {
    }
}

/// <summary>Thrown when ZenTao returns a successful response that the client cannot parse.</summary>
public sealed class ZenTaoResponseException : Exception
{
    public ZenTaoResponseException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
