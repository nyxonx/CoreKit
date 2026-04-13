param(
    [switch]$NoRestore
)

$ErrorActionPreference = "Stop"

$solutionPath = (Resolve-Path (Join-Path $PSScriptRoot "..\\CoreKit.sln")).Path
$arguments = @("format", $solutionPath, "--verify-no-changes")

if ($NoRestore)
{
    $arguments += "--no-restore"
}

& dotnet @arguments
exit $LASTEXITCODE
