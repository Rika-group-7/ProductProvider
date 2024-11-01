using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductProvider.Infrastructure.Data.Context;
using ProductProvider.Infrastructure.GraphQL.Mutations;
using ProductProvider.Infrastructure.GraphQL.ObjectType;
using ProductProvider.Infrastructure.GraphQL.Queries;
using ProductProvider.Infrastructure.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        // Use the full connection string in COSMOS_URI and the database name in COSMOS_DB
        services.AddPooledDbContextFactory<DataContext>(x =>
            x.UseCosmos(Environment.GetEnvironmentVariable("COSMOS_URI")!, Environment.GetEnvironmentVariable("COSMOS_DB")!)
             .UseLazyLoadingProxies());

        var sb = services.BuildServiceProvider();
        using var scope = sb.CreateScope();
        var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DataContext>>();
        using var context = dbContextFactory.CreateDbContext();
        context.Database.EnsureCreated();

        services.AddScoped<IProductService, ProductService>();

        services.AddGraphQLFunction()
            .AddQueryType<ProductQuery>()
            .AddType<ProductType>()
            .AddMutationType<ProductMutation>();
    })
    .Build();

host.Run();
