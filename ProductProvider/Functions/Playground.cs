using Microsoft.AspNetCore.Mvc;
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
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "graphql")] HttpRequestData req)
        {
            _logger.LogInformation("Starting execution of 'Playground' function with request method: {Method}", req.Method);

            try
            {
                _logger.LogInformation("Creating response with GraphQL Playground page.");
                var response = req.CreateResponse();
                response.Headers.Add("Content-type", "text/html; charset=utf-8");

                await response.WriteStringAsync(Playgroundpage());

                _logger.LogInformation("Response created and returned successfully.");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the 'Playground' function request.");
                var errorResponse = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync("An internal server error occurred.");
                return errorResponse;
            }
        }

        private string Playgroundpage()
        {
            _logger.LogDebug("Generating HTML content for the GraphQL Playground page.");

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
                            endpoint: '/api/graphql'
                            })
                        })
                    </script>
                </body>
                </html>";
        }
    }
}
