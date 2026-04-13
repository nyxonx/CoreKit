using CoreKit.BuildingBlocks.Presentation;
using CoreKit.Modules.Identity.Presentation;
using CoreKit.Modules.Tenancy.Presentation;

namespace CoreKit.AppHost.Server.Extensions;

public static class CoreKitModuleCatalog
{
    public static IReadOnlyList<ICoreKitModule> All { get; } =
    [
        new TenancyModule(),
        new IdentityModule()
    ];
}
