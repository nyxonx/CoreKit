using CoreKit.AppHost.Server.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCoreKitAppHost(builder.Configuration);

var app = builder.Build();

app.UseCoreKitAppHost();
app.MapCoreKitInfrastructureEndpoints();

app.Run();
