using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ProductProvider.Functions
{
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
            _logger.LogInformation("Playground function invoked.");

            try
            {
                var response = req.CreateResponse();
                response.Headers.Add("Content-Type", "text/html; charset=utf-8");
                await response.WriteStringAsync(PlaygroundPage());

                _logger.LogInformation("Playground page served successfully.");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while serving the Playground page.");
                var errorResponse = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync("An error occurred while loading the GraphQL Playground.");
                return errorResponse;
            }
        }

        private string PlaygroundPage()
        {
            _logger.LogDebug("Generating GraphQL Playground HTML content.");

            return @"
        <!DOCTYPE html>
        <html>
        <head>
            <title>HotChocolate GraphQL Playground</title>
            <link rel=""stylesheet"" href=""https://cdn.jsdelivr.net/npm/graphql-playground-react/build/static/css/index.css"" />
            <link rel=""shortcut icon"" href=""https://cdn.jsdelivr.net/npm/graphql-playground-react/build/favicon.png"" />
            <script src=""https://cdn.jsdelivr.net/npm/graphql-playground-react/build/static/js/middleware.js""></script>
        </head>
        <body>
            <div id=""root""></div>
            <script>
                window.addEventListener('load', function(event) {
                    GraphQLPlayground.init(document.getElementById('root'), {
                        endpoint: '/api/graphql' // Ensure this matches your GraphQL endpoint
                    })
                })
            </script>
        </body>
        </html>";
        }

    }
}
