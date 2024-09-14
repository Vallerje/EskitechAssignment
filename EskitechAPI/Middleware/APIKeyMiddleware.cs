public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string APIKEYNAME = "X-API-KEY";

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IConfiguration configuration, ILogger<ApiKeyMiddleware> logger)
    {
        if (!context.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
        {
            logger.LogWarning("API Key was not provided.");
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key was not provided.");
            return;
        }

        var apiKey = configuration.GetValue<string>("AllowedApiKey");

        if (string.IsNullOrEmpty(apiKey))
        {
            logger.LogError("API Key configuration is missing.");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Internal server error. Please contact support.");
            return;
        }

        if (!apiKey.Equals(extractedApiKey))
        {
            logger.LogWarning("Unauthorized client attempted to access with invalid API key.");
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized client.");
            return;
        }

        await _next(context);
    }
}