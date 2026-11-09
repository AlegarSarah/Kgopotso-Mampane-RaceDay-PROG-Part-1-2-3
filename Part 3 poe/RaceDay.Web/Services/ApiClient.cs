
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RaceDay.Web.Services;

public class ApiResult
{
    public bool Success { get; init; }
    public int StatusCode { get; init; }
    public string Error { get; init; } = "";
}

public class ApiResult<T> : ApiResult
{
    public T? Data { get; init; }
}

public class ApiClient
{
    private static readonly JsonSerializerOptions Json =
        new(JsonSerializerDefaults.Web)
        {
            Converters = { new JsonStringEnumConverter() }
        };

    private readonly HttpClient _http;
    private readonly IHttpContextAccessor _ctx;

    public ApiClient(HttpClient http, IHttpContextAccessor ctx)
    {
        _http = http;
        _ctx = ctx;
    }

    public Task<ApiResult<T>> Get<T>(string path) =>
        Send<T>(HttpMethod.Get, path, null);

    public Task<ApiResult<T>> Post<T>(
        string path, object? body = null) =>
        Send<T>(HttpMethod.Post, path, Body(body));

    public Task<ApiResult<object>> Put(
        string path, object body) =>
        Send<object>(HttpMethod.Put, path, Body(body));

    public Task<ApiResult<object>> Delete(string path) =>
        Send<object>(HttpMethod.Delete, path, null);

    public Task<ApiResult<T>> Upload<T>(
        string path, IFormFile file)
    {
        var content = new MultipartFormDataContent();
        var part = new StreamContent(file.OpenReadStream());

        part.Headers.ContentType =
            new MediaTypeHeaderValue(file.ContentType);

        content.Add(part, "file", file.FileName);

        return Send<T>(HttpMethod.Post, path, content);
    }

    private static HttpContent? Body(object? body)
    {
        return body == null
            ? null
            : JsonContent.Create(body, body.GetType(), null, Json);
    }

    private async Task<ApiResult<T>> Send<T>(
        HttpMethod method,
        string path,
        HttpContent? content)
    {
        try
        {
            var session = _ctx.HttpContext?.Session;

            using var request = new HttpRequestMessage(method, path)
            {
                Content = content
            };

            // Attach JWT authentication when available.
            var token = session?.GetString("JwtToken");

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            using var response = await _http.SendAsync(request);

            var responseBody =
                await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                T? data = default;

                if (typeof(T) != typeof(object) &&
                    !string.IsNullOrWhiteSpace(responseBody))
                {
                    data = JsonSerializer.Deserialize<T>(
                        responseBody, Json);
                }

                return new ApiResult<T>
                {
                    Success = true,
                    StatusCode = (int)response.StatusCode,
                    Data = data
                };
            }

            return new ApiResult<T>
            {
                Success = false,
                StatusCode = (int)response.StatusCode,
                Error = ReadError(responseBody, response.StatusCode)
            };
        }
        catch (Exception ex) when (
            ex is HttpRequestException ||
            ex is TaskCanceledException ||
            ex is JsonException)
        {
            return new ApiResult<T>
            {
                Success = false,
                StatusCode = 503,
                Error = "The RaceDay service is unavailable right now. Please try again shortly."
            };
        }
    }

    private static string ReadError(
        string body, HttpStatusCode status)
    {
        try
        {
            using var document = JsonDocument.Parse(body);
            var root = document.RootElement;

            if (root.TryGetProperty("message", out var message) &&
                message.ValueKind == JsonValueKind.String)
            {
                return message.GetString()!;
            }

            if (root.TryGetProperty("errors", out var errors) &&
                errors.ValueKind == JsonValueKind.Object)
            {
                foreach (var property in errors.EnumerateObject())
                {
                    foreach (var error in property.Value.EnumerateArray())
                    {
                        return error.GetString() ??
                               "Please check your input.";
                    }
                }
            }
        }
        catch (JsonException)
        {
            // Use the default status message below.
        }

        return status switch
        {
            HttpStatusCode.Unauthorized =>
                "Please log in to continue.",

            HttpStatusCode.Forbidden =>
                "You do not have permission to do that.",

            HttpStatusCode.NotFound =>
                "We could not find what you were looking for.",

            _ => "Something went wrong. Please try again."
        };
    }
}
