using CoreKit.AppHost.Client;
using CoreKit.AppHost.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(
    _ => new HttpClient
    {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    });
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthApiClient>();
builder.Services.AddScoped<ServerAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(
    serviceProvider => serviceProvider.GetRequiredService<ServerAuthenticationStateProvider>());

await builder.Build().RunAsync();
