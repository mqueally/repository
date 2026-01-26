Param(
    [string]$RepoRoot = (Get-Location).Path
)

$ErrorActionPreference = "Stop"

Write-Host "AMQ Company Secretarial Manager setup" -ForegroundColor Cyan
Write-Host "Repo root: $RepoRoot" -ForegroundColor Cyan

$dotnet = Get-Command dotnet -ErrorAction SilentlyContinue
if (-not $dotnet) {
    Write-Host "dotnet not found. Please install the .NET 8 SDK from Microsoft and re-run this script." -ForegroundColor Yellow
    exit 1
}

Write-Host "dotnet version:" -ForegroundColor Green
& dotnet --version

Write-Host "Restoring packages..." -ForegroundColor Green
Push-Location $RepoRoot
try {
    & dotnet restore

    Write-Host "Running tests (optional)..." -ForegroundColor Green
    & dotnet test

    Write-Host "Setup complete." -ForegroundColor Green
    Write-Host "To run the app:" -ForegroundColor Cyan
    Write-Host "dotnet run --project src/AmqCompanySecretarialManager/AmqCompanySecretarialManager.csproj" -ForegroundColor Cyan

    Write-Host "To create a self-contained EXE:" -ForegroundColor Cyan
    Write-Host "dotnet publish src/AmqCompanySecretarialManager/AmqCompanySecretarialManager.csproj -c Release -r win-x64 --self-contained true" -ForegroundColor Cyan
} finally {
    Pop-Location
}
