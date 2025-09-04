# eForm SDK .NET

eForm SDK .NET is a .NET 9.0 class library by Microting that provides an SDK for integrating with the Microting eForm API v1. The project includes comprehensive unit and integration tests using NUnit and uses MariaDB/MySQL as the primary database with TestContainers for testing.

Always reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.

## Working Effectively

### Prerequisites and Setup
- Install .NET 9.0 SDK:
  - Download and run the official installer script: `wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh && chmod +x dotnet-install.sh && ./dotnet-install.sh --channel 9.0`
  - Add to PATH: `export PATH="$HOME/.dotnet:$PATH"`
  - Verify installation: `dotnet --version` (should show 9.0.x)
  - The project will NOT build with .NET 8.0 or earlier versions
- Docker is required for integration tests (RabbitMQ and MariaDB containers)

### Building the Project
- Restore dependencies: `dotnet restore` -- takes ~22 seconds. Set timeout to 60+ seconds.
- Build the solution: `dotnet build --configuration Release --no-restore` -- takes ~38 seconds. NEVER CANCEL. Set timeout to 90+ seconds.
- The build generates warnings about NUnit analyzers and security vulnerabilities in Magick.NET-Q16-AnyCPU package - these are expected and do not prevent successful compilation.

### Testing

#### Unit Tests
Run unit tests using the provided shell scripts. NEVER CANCEL these operations:

- `./unit-tests-base.sh` -- takes ~2:20 minutes. NEVER CANCEL. Set timeout to 180+ seconds.
  - Tests the base SDK functionality with MariaDB TestContainer
  - Runs 28 tests, all should pass
  - Includes database setup and teardown operations

- `./unit-tests-checklists.sh` -- takes ~43 seconds. NEVER CANCEL. Set timeout to 90+ seconds.
  - Tests checklist functionality
  - Runs 31 tests, currently has 3 known failures (Object reference not set to an instance of an object)
  - Known issue: some cleanup operations in teardown fail but don't affect core functionality

- `./unit-tests-insight.sh` -- takes ~32 seconds. NEVER CANCEL. Set timeout to 60+ seconds.  
  - Tests insight/survey functionality with MariaDB TestContainer
  - Runs 42 tests, all should pass

#### Integration Tests
Integration tests require RabbitMQ running in Docker. Start it first:
```bash
docker pull rabbitmq:latest
docker run -d --hostname my-rabbit --name some-rabbit -e RABBITMQ_DEFAULT_USER=admin -e RABBITMQ_DEFAULT_PASS=password -p5672:5672 rabbitmq:latest
```

Run integration tests using the provided shell scripts. NEVER CANCEL these operations:

- `./integration-tests-core-base.sh` -- takes ~31 seconds. NEVER CANCEL. Set timeout to 90+ seconds.
  - Tests core functionality against real API endpoints
  - Runs 32 tests, all should pass
  - Uses MariaDB TestContainer

- `./integration-tests-sqlcontroller-base.sh` -- takes ~1:19 minutes. NEVER CANCEL. Set timeout to 150+ seconds.
  - Tests SQL controller functionality with extensive database operations
  - Runs 63 tests, all should pass  
  - Uses MariaDB TestContainer

Additional integration test scripts available:
- `./integration-tests-core-case.sh`
- `./integration-tests-core-checklists.sh`
- `./integration-tests-sqlcontroller-case.sh` 
- `./integration-tests-sqlcontroller-checklists.sh`

### CI/CD Pipeline Validation
Always run these before committing changes to ensure CI pipeline success:
- `dotnet build --configuration Release --no-restore`
- At minimum, run `./unit-tests-base.sh` and `./unit-tests-insight.sh` (both pass reliably)
- For thorough validation, run integration tests after starting RabbitMQ container

## Validation
- ALWAYS test database migration operations if you modify the eFormCore/Migrations directory
- Integration tests use fake API tokens and mock external services - they test business logic, not live API connectivity
- The SDK uses Entity Framework Core with MariaDB/MySQL - database schema changes require migrations
- TestContainers automatically handles database setup/teardown for tests - no manual database configuration needed
- Build warnings about Magick.NET security vulnerabilities are acknowledged but acceptable for this use case

## Common Tasks

### Repository Structure
```
eform-sdk-dotnet/
├── eFormCore/                          # Main SDK library project (Microting.eForm.csproj)
├── eFormSDK.Base.Tests/               # Unit tests for base functionality  
├── eFormSDK.CheckLists.Tests/         # Unit tests for checklist functionality
├── eFormSDK.InSight.Tests/            # Unit tests for insight/survey functionality
├── eFormSDK.Integration.Base.CoreTests/      # Integration tests for base core functionality
├── eFormSDK.Integration.Base.SqlControllerTests/  # Integration tests for SQL operations
├── eFormSDK.Integration.Case.CoreTests/      # Integration tests for case functionality
├── eFormSDK.Integration.Case.SqlControllerTests/  # Integration tests for case SQL operations  
├── eFormSDK.Integration.CheckLists.CoreTests/     # Integration tests for checklist core
├── eFormSDK.Integration.CheckLists.SqlControllerTests/ # Integration tests for checklist SQL
├── DBMigrator/                        # Database migration utility
├── .github/workflows/                 # CI/CD pipeline definitions
├── *.sh                              # Test execution scripts
└── eFormSDK.sln                      # Solution file
```

### Key Files and Their Purpose
- `eFormCore/Core.cs` - Main SDK entry point (312KB, core functionality)
- `eFormCore/AdminTools.cs` - Administrative utilities
- `eFormCore/Migrations/` - Entity Framework database migrations
- `armprepareinstall.sh` - ARM architecture .NET setup (legacy, may not work with current versions)

### Build Artifacts
- Main library: `eFormCore/bin/Release/net9.0/Microting.eForm.dll`
- NuGet package: Generated in `./artifacts/` when using `dotnet pack`
- Test assemblies: Generated in respective `bin/Release/net9.0/` directories

### Database Operations  
- TestContainers automatically provisions MariaDB for tests with database name `eformsdk-tests`
- No manual database setup required for testing
- Migration commands (if needed): `dotnet ef migrations add MIGRATIONNAME --project eFormCore/Microting.eForm.csproj --startup-project SourceCode/SourceCode.csproj --context MicrotingDbAnySQL`

### Dependencies
- .NET 9.0 Runtime/SDK (required)
- Docker (for RabbitMQ and MariaDB containers during testing)
- NUnit testing framework
- Entity Framework Core
- Various AWS SDK packages
- Magick.NET for image processing (has known security warnings that are acceptable)

### Common Commands Reference
- Check .NET version: `dotnet --version`
- Restore packages: `dotnet restore` 
- Build solution: `dotnet build --configuration Release --no-restore`
- Run specific test project: `dotnet test --no-restore -c Release -v n [ProjectName].csproj`
- Pack NuGet: `dotnet pack eFormCore/Microting.eForm.csproj -c Release -o ./artifacts`
- List Docker containers: `docker ps`
- Stop RabbitMQ container: `docker stop some-rabbit && docker rm some-rabbit`

### NuGet Package Creation
- Create NuGet package: `dotnet pack eFormCore/Microting.eForm.csproj -c Release -o ./artifacts -p:PackageVersion=X.X.X --no-build`
- Package is created in `./artifacts/Microting.eForm.X.X.X.nupkg`
- Always specify version when creating packages for distribution

### Database Migrations
- The project uses Entity Framework Core migrations located in `eFormCore/Migrations/`
- DBMigrator project is a utility for database operations but requires specific configuration
- For development, tests handle database setup automatically via TestContainers

### Validation Scenarios
After making changes to the codebase, validate by running:
1. Clean build: `dotnet build --configuration Release --no-restore`
2. Quick test: `./unit-tests-insight.sh` (fastest, most reliable)
3. Comprehensive test: `./unit-tests-base.sh` (covers core functionality)
4. For database/integration changes: Start RabbitMQ and run `./integration-tests-core-base.sh`

### Known Issues and Workarounds
- `unit-tests-checklists.sh` has 3 failing tests due to null reference exceptions in cleanup - this is a known issue that doesn't affect functionality
- Magick.NET package has security warnings that are acknowledged but not currently resolved
- The project requires .NET 9.0 specifically - earlier versions will fail with framework targeting errors
- ARM architecture support via `armprepareinstall.sh` may be outdated for current .NET versions

### Timeout Guidelines for Automation
When running automated builds/tests, use these minimum timeout values:
- `dotnet restore`: 60 seconds
- `dotnet build`: 90 seconds  
- Unit tests: 180 seconds (base), 90 seconds (checklists/insight)
- Integration tests: 90-150 seconds depending on complexity
- `dotnet pack`: 60 seconds
- NEVER use default timeout values that may cause premature cancellation