param(
    [switch]$NoBuild
)

$arguments = @(
    "run",
    "--project", "src\CoreKit.AppHost.Server\CoreKit.AppHost.Server.csproj"
)

if ($NoBuild) {
    $arguments += "--no-build"
}

$arguments += "--"
$arguments += "--provision-only"

dotnet @arguments
