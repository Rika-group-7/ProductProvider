using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductProvider.Infrastructure.GraphQL.Mutations;
using ProductProvider.Infrastructure.GraphQL.ObjectType;
using ProductProvider.Infrastructure.GraphQL.Queries;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();



        services.AddGraphQLFunction()
        .AddQueryType<ProductQuery>()
        .AddType<ProductType>()
        .AddMutationType<ProductMutation>();
    })
    .Build();

host.Run();