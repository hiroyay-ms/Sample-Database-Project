using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FunctionProj.Data;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((services) =>
    {
        var connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING");

        services.AddDbContext<AdventureWorksContext>(options => options.UseSqlServer(connectionString));
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

host.Run();
