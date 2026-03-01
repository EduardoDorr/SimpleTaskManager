using System.Diagnostics;

using DDS.SimpleTaskManager.Core.Results.Base;

using Microsoft.Extensions.Logging;

namespace DDS.SimpleTaskManager.Core.CommandQueries;

public abstract class BaseRequestHandler<THandler, TRequest, TResult>
    : IRequestHandler<TRequest, TResult>
    where THandler : class
    where TRequest : notnull
    where TResult : notnull
{
    protected readonly ILogger _logger;

    protected BaseRequestHandler(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<TResult> HandleAsync(
        TRequest request,
        CancellationToken cancellationToken = default)
    {
        var sw = Stopwatch.StartNew();
        var outcome = "Succeeded";

        _logger.LogDebug(
            "Handling {Handler} for {RequestType}",
            typeof(THandler).Name,
            typeof(TRequest).Name);

        try
        {
            var result = await ExecuteAsync(request, cancellationToken);

            if (result is ResultBase r && !r.Success)
                outcome = "BusinessFailed";

            return result;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            outcome = "Canceled";
            throw;
        }
        catch
        {
            outcome = "ExceptionOccurred";
            throw;
        }
        finally
        {
            sw.Stop();
            _logger.LogInformation(
                "Finished {Handler} for {RequestType}. Outcome={Outcome} ElapsedMs={ElapsedMs}",
                typeof(THandler).Name,
                typeof(TRequest).Name,
                outcome,
                sw.ElapsedMilliseconds);
        }
    }

    protected abstract Task<TResult> ExecuteAsync(
        TRequest request,
        CancellationToken cancellationToken = default);
}