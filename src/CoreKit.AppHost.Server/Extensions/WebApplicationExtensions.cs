namespace CoreKit.AppHost.Server.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseCoreKitAppHost(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        if (!app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
        }

        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }

    public static WebApplication MapCoreKitInfrastructureEndpoints(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.MapGet(
                "/",
                () => Results.Ok(
                    new
                    {
                        name = "CoreKit AppHost Server",
                        status = "ok",
                        environment = app.Environment.EnvironmentName
                    }))
            .WithName("GetServerRoot")
            .WithTags("System");

        app.MapGet(
                "/api/system/info",
                () => Results.Ok(
                    new
                    {
                        application = "CoreKit",
                        host = "Server",
                        environment = app.Environment.EnvironmentName
                    }))
            .WithName("GetSystemInfo")
            .WithTags("System");

        app.MapHealthChecks("/health").WithTags("System");
        app.MapCoreKitModules();
        app.MapFallbackToFile("index.html");

        return app;
    }
}
