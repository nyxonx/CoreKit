using CoreKit.Modules.Identity.Application;
using CoreKit.Modules.Identity.Domain;
using Microsoft.EntityFrameworkCore;

namespace CoreKit.Modules.Identity.Infrastructure;

public sealed class TenantMembershipAdministrationService(AppIdentityDbContext dbContext)
    : ITenantMembershipAdministrationService
{
    public async Task<IReadOnlyList<TenantMembershipDto>> GetMembershipsAsync(
        string tenantIdentifier,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantIdentifier);

        return await dbContext.UserTenantMemberships
            .AsNoTracking()
            .Where(membership => membership.TenantIdentifier == tenantIdentifier)
            .Join(
                dbContext.Users.AsNoTracking(),
                membership => membership.UserId,
                user => user.Id,
                (membership, user) => new TenantMembershipDto(
                    user.UserName ?? user.Id,
                    membership.Role,
                    membership.IsActive))
            .OrderBy(membership => membership.UserName)
            .ToListAsync(cancellationToken);
    }

    public async Task<TenantMembershipDto?> UpsertMembershipAsync(
        string tenantIdentifier,
        string userName,
        string role,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(userName);
        ArgumentException.ThrowIfNullOrWhiteSpace(role);

        var normalizedUserName = userName.Trim();
        var user = await dbContext.Users.SingleOrDefaultAsync(
            entity => entity.UserName == normalizedUserName,
            cancellationToken);

        if (user is null)
        {
            return null;
        }

        var membership = await dbContext.UserTenantMemberships.SingleOrDefaultAsync(
            entity => entity.UserId == user.Id
                && entity.TenantIdentifier == tenantIdentifier,
            cancellationToken);

        if (membership is null)
        {
            membership = new AppUserTenantMembership
            {
                UserId = user.Id,
                TenantIdentifier = tenantIdentifier
            };

            dbContext.UserTenantMemberships.Add(membership);
        }

        membership.Role = role;
        membership.IsActive = true;

        await dbContext.SaveChangesAsync(cancellationToken);

        return new TenantMembershipDto(
            user.UserName ?? user.Id,
            membership.Role,
            membership.IsActive);
    }

    public async Task<TenantMembershipDto?> SetMembershipActivationAsync(
        string tenantIdentifier,
        string userName,
        bool isActive,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(userName);

        var normalizedUserName = userName.Trim();
        var user = await dbContext.Users.SingleOrDefaultAsync(
            entity => entity.UserName == normalizedUserName,
            cancellationToken);

        if (user is null)
        {
            return null;
        }

        var membership = await dbContext.UserTenantMemberships.SingleOrDefaultAsync(
            entity => entity.UserId == user.Id
                && entity.TenantIdentifier == tenantIdentifier,
            cancellationToken);

        if (membership is null)
        {
            return null;
        }

        membership.IsActive = isActive;
        await dbContext.SaveChangesAsync(cancellationToken);

        return new TenantMembershipDto(
            user.UserName ?? user.Id,
            membership.Role,
            membership.IsActive);
    }
}
