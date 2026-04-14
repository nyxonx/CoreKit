using CoreKit.AppHost.Server.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCoreKitAppHost(builder.Configuration);

var app = builder.Build();

if (!await app.InitializeCoreKitAppAsync(args))
{
    return;
}

app.UseCoreKitAppHost();
app.MapCoreKitInfrastructureEndpoints();

app.Run();

public partial class Program;
