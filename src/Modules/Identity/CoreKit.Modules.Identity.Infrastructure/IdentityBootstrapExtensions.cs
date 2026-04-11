using CoreKit.Modules.Identity.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreKit.Modules.Identity.Infrastructure;

public static class IdentityBootstrapExtensions
{
    public static async Task InitializeIdentityAsync(
        this IServiceProvider services,
        IConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        await using var scope = services.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

        await dbContext.Database.EnsureCreatedAsync(cancellationToken);

        const string adminRoleName = "Admin";

        if (!await roleManager.RoleExistsAsync(adminRoleName))
        {
            await roleManager.CreateAsync(new AppRole { Name = adminRoleName });
        }

        var adminUserName = configuration["Identity:Seed:AdminUserName"] ?? "admin";
        var adminPassword = configuration["Identity:Seed:AdminPassword"] ?? "Admin1234";

        var existingUser = await userManager.Users.SingleOrDefaultAsync(
            user => user.UserName == adminUserName,
            cancellationToken);

        if (existingUser is null)
        {
            var adminUser = new AppUser
            {
                UserName = adminUserName
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(error => error.Description));
                throw new InvalidOperationException($"Failed to create seed admin user: {errors}");
            }

            await userManager.AddToRoleAsync(adminUser, adminRoleName);
        }
    }
}
