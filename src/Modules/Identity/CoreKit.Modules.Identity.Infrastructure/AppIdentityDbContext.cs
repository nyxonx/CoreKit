using CoreKit.Modules.Identity.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoreKit.Modules.Identity.Infrastructure;

public sealed class AppIdentityDbContext : IdentityDbContext<AppUser, AppRole, string>
{
    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
        : base(options)
    {
    }
}
