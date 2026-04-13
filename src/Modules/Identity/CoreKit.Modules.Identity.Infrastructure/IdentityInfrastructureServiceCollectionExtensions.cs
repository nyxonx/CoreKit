using CoreKit.Modules.Identity.Domain;
using CoreKit.BuildingBlocks.Application;
using CoreKit.Modules.Tenancy.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreKit.Modules.Identity.Infrastructure;

public static class IdentityInfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var connectionString =
            configuration.GetConnectionString("IdentityDatabase")
            ?? "Data Source=corekit.identity.db";

        services.AddDbContext<AppIdentityDbContext>(
            options => options.UseSqlite(connectionString));

        services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = false;
            })
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = "CoreKit.Auth";
            options.Cookie.HttpOnly = true;
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            options.Cookie.IsEssential = true;
            options.LoginPath = "/login";
            options.LogoutPath = "/logout";
            options.SlidingExpiration = true;
        });

        services.AddAuthorization();
        services.AddScoped<ICurrentExecutionContextAccessor, HttpContextCurrentExecutionContextAccessor>();
        services.AddScoped<ICurrentTenantAuthorizationService, CurrentTenantAuthorizationService>();

        return services;
    }
}
