using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using API.Services.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddLogging();

        services.AddScoped<FilePathProvider>();
        services.AddScoped<LMSforWLZ>();
        services.AddScoped<LMSService>();
        services.AddScoped<LMSforWAZ>();
        services.AddScoped<LMSforLAZ>();
        services.AddScoped<ZScoreService>();
    })
    .Build();

host.Run();