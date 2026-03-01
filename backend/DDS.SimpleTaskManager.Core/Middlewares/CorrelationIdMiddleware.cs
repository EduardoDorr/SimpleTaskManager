using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Http;

using Serilog.Context;

namespace DDS.SimpleTaskManager.Core.Middlewares;

public sealed partial class CorrelationIdMiddleware
{
    public const string HeaderName = "X-Correlation-Id";
    private static readonly Regex Allowed = AllowedCorrelationIdPattern();
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context)
    {
        var incoming = context.Request.Headers[HeaderName].ToString().Trim();

        var correlationId =
            !string.IsNullOrWhiteSpace(incoming) && Allowed.IsMatch(incoming)
                ? incoming
                : Guid.NewGuid().ToString("N");

        context.TraceIdentifier = correlationId;
        context.Response.Headers[HeaderName] = correlationId;

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            await _next(context);
        }
    }

    [GeneratedRegex("^[A-Za-z0-9._:-]{1,64}$")]
    private static partial Regex AllowedCorrelationIdPattern();
}