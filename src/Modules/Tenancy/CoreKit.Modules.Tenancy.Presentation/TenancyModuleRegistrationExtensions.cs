using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CoreKit.Modules.Tenancy.Application;
using CoreKit.Modules.Tenancy.Infrastructure;

namespace CoreKit.Modules.Tenancy.Presentation;

public static class TenancyModuleRegistrationExtensions
{
    public static IServiceCollection AddTenancyModule(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<ITenantNoteService, TenantNoteService>();

        return services;
    }

    public static IEndpointRouteBuilder MapTenancyModule(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var group = endpoints.MapGroup("/api/modules/tenancy").WithTags("Tenancy");

        group.MapGet(
            "/status",
            (ITenantContextAccessor tenantContextAccessor) => Results.Ok(
                new
                {
                    module = "Tenancy",
                    status = "registered",
                    tenant = tenantContextAccessor.TenantContext is null
                        ? null
                        : new
                        {
                            tenantContextAccessor.TenantContext.Identifier,
                            tenantContextAccessor.TenantContext.Name,
                            tenantContextAccessor.TenantContext.Host
                        }
                }));

        group.MapGet(
            "/data-status",
            async (ITenantNoteService tenantNoteService, CancellationToken cancellationToken) =>
            {
                var notes = await tenantNoteService.GetNotesAsync(cancellationToken);

                return Results.Ok(
                    new
                    {
                        database = "tenant",
                        notes = notes.Count
                    });
            });

        group.MapGet(
            "/notes",
            async (ITenantNoteService tenantNoteService, CancellationToken cancellationToken) =>
                Results.Ok(await tenantNoteService.GetNotesAsync(cancellationToken)));

        group.MapPost(
            "/notes",
            async (
                AddTenantNoteRequest request,
                ITenantNoteService tenantNoteService,
                CancellationToken cancellationToken) =>
            {
                var note = await tenantNoteService.AddNoteAsync(request, cancellationToken);
                return Results.Created($"/api/modules/tenancy/notes/{note.Id}", note);
            });

        return endpoints;
    }
}
