using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ProductProvider.Functions;

public class GraphQL(ILogger<GraphQL> logger, IGraphQLRequestExecutor executor)
{
    private readonly ILogger<GraphQL> _logger = logger;
    private readonly IGraphQLRequestExecutor _executor = executor;

    [Function("GraphQL")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "graphql")] HttpRequest req)
    {
        _logger.LogInformation("Executing 'GraphQL' function with invocation ID: {InvocationId}", req.HttpContext.TraceIdentifier);

        try
        {
            _logger.LogInformation("Received request with content type: {ContentType}", req.ContentType);
            _logger.LogDebug("Request Headers: {Headers}", req.Headers);

            // Your GraphQL execution logic
            var result = await _executor.ExecuteAsync(req);

            _logger.LogInformation("Successfully executed 'GraphQL' function.");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while executing 'GraphQL' function with invocation ID: {InvocationId}", req.HttpContext.TraceIdentifier);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}
