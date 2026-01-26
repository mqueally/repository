# AMQ Company Secretarial Manager

Windows desktop (offline-first) WPF application for Irish company secretarial compliance work.

## Step-by-step project plan
1. **Foundation & solution setup**: create WPF app, test project, and core packages (EF Core + SQLite + QuestPDF).
2. **Schema & migrations**: model the statutory registers, event store, filings, documents, and audit chain; add initial migration and seed data.
3. **Core UI shell**: dashboard, company profile tabs, register views, filing manager, books & records pack builder.
4. **Event logging & audit chain**: append-only events, hash chaining, verify audit integrity.
5. **Register reconstruction**: replay member/share events to build current/as-at views and validate transfers.
6. **Filings & packs**: filing types framework, CRO B1/B10 templates, client-ready filing pack generation.
7. **PDF outputs**: register PDFs, filing packs, and books & records pack bundle.
8. **Testing**: unit tests for reconstruction, transfer validation, audit chain verification, and filing pack logic.
9. **Packaging**: MSIX build steps and offline installer guidance.

## Packaging (MSIX)
1. Install the Windows SDK and MSIX Packaging Tool.
2. From Visual Studio or `dotnet publish`, build the WPF project for `net8.0-windows`.
3. Create an MSIX packaging project and reference the WPF output.
4. Configure app identity, publisher, and install location.
5. Build the MSIX package and distribute internally for offline installation.

## Project structure
- `src/AmqCompanySecretarialManager` - WPF app, EF Core models, services.
- `tests/AmqCompanySecretarialManager.Tests` - xUnit tests.
- `docs` - Additional notes.

## Testing locally
These steps are for a Windows laptop. They use free tools only.

### 1) Install .NET 8 (one-time)
1. Open a browser and search for **“.NET 8 SDK download”** (from Microsoft).
2. Install the **.NET 8 SDK**. No license is required.
3. After install, open **Command Prompt** and run: `dotnet --version` (you should see a version number).

### 2) Run the unit tests (optional)
1. Open **Command Prompt**.
2. Go to the folder where this repo lives, e.g.:
   - `cd C:\path\to\your\repo`
3. Restore packages: `dotnet restore`
4. Run tests: `dotnet test`

### 3) Run the WPF app
1. In the same Command Prompt window, run:
   - `dotnet run --project src/AmqCompanySecretarialManager/AmqCompanySecretarialManager.csproj`

### 4) Create a standalone EXE (no installer)
1. From the repo root, run:
   - `dotnet publish src/AmqCompanySecretarialManager/AmqCompanySecretarialManager.csproj -c Release -r win-x64 --self-contained true`
2. The EXE will be in:
   - `src\AmqCompanySecretarialManager\bin\Release\net8.0-windows\win-x64\publish`

### Optional: run the setup script
From PowerShell in the repo root:
`.\scripts\setup.ps1`
