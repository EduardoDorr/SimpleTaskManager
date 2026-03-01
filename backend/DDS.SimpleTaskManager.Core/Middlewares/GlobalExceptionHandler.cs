using System.Net;

using DDS.SimpleTaskManager.Core.Results.Api;
using DDS.SimpleTaskManager.Core.Results.Errors;
using DDS.SimpleTaskManager.Core.Results.Extensions;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DDS.SimpleTaskManager.Core.Middlewares;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private const string CorrelationIdHeader = "X-Correlation-Id";
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var correlationId = httpContext.TraceIdentifier;

        using (_logger.BeginScope(new Dictionary<string, object> { ["CorrelationId"] = correlationId }))
        {
            _logger.LogError(
                exception,
                "Exception occurred: {Message}",
                exception.Message);
        }

        var errors =
            new List<IError>
            {
                new Error(
                    "Exception",
                    "An unexpected error occurred",
                    ErrorType.Failure)
            };

        var result =
            new ApiResult(
                HttpStatusCode.InternalServerError,
                errors.ToApiError())
            .AddInfo(new ApiInfo("CorrelationId", correlationId))
            .ToActionResult();

        httpContext.Response.StatusCode = result.StatusCode!.Value;
        httpContext.Response.Headers[CorrelationIdHeader] = correlationId;

        await httpContext.Response
            .WriteAsJsonAsync(result.Value, cancellationToken);

        return true;
    }
}