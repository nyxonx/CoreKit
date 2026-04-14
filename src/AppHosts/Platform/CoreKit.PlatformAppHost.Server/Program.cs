using CoreKit.PlatformAppHost.Server.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPlatformAppHost(builder.Configuration);

var app = builder.Build();

if (!await app.InitializePlatformAppAsync(args))
{
    return;
}

app.UsePlatformAppHost();
app.MapPlatformInfrastructureEndpoints();

app.Run();

public partial class Program;
