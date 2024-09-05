using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FunctionProj.Data;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((hostContext, services) =>
    {
        var connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING");

        services.AddDbContext<AdventureWorksContext>(options => options.UseSqlServer(connectionString));
    })
    .Build();

host.Run();
