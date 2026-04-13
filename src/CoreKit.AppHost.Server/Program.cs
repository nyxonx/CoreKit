using CoreKit.AppHost.Server.Extensions;
using CoreKit.Modules.Identity.Infrastructure;
using CoreKit.Modules.Tenancy.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCoreKitAppHost(builder.Configuration);

var app = builder.Build();

await app.Services.InitializeCoreKitModulesAsync(app.Configuration);

if (args.Contains("--provision-only", StringComparer.OrdinalIgnoreCase))
{
    return;
}

app.UseCoreKitAppHost();
app.MapCoreKitInfrastructureEndpoints();

app.Run();
