using CoreKit.BuildingBlocks.Presentation;
using CoreKit.Modules.Customers.Presentation;
using CoreKit.Modules.Identity.Presentation;
using CoreKit.Modules.Tenancy.Presentation;

namespace CoreKit.PlatformAppHost.Server.Extensions;

public static class CoreKitModuleCatalog
{
    public static IReadOnlyList<ICoreKitModule> All { get; } =
    [
        new CustomersModule(),
        new TenancyModule(),
        new IdentityModule()
    ];
}
