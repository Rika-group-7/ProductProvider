using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ProductProvider.Functions;

public class Playground
{
    private readonly ILogger<Playground> _logger;

    public Playground(ILogger<Playground> logger)
    {
        _logger = logger;
    }

    [Function("Playground")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "playground")] HttpRequestData req)
    {
        var response = req.CreateResponse();
        response.Headers.Add("Content-Type", "text/html; charset=utf-8");
        await response.WriteStringAsync("<html><body><h1>Hello World</h1></body></html>");
        return response;
    }

}
