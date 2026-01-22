## Copilot Instructions

- Purpose: this repo is a .NET 10 console-hosted Model Context Protocol (MCP) server that exposes Plane API operations (list states, create work items) over stdio.
- Startup: host built in [PlaneMCPServer/Program.cs](PlaneMCPServer/Program.cs#L1-L45); configuration sources are cleared then rebuilt from `appsettings.json`, user secrets, and environment variables, so defaults in the repo are overridden by secrets/env.
- Required config keys: `PlaneAPIKey` (secret), `BaseUrl`, `WorkspaceId`, `ProjectId`; missing values throw during startup [Program.cs](PlaneMCPServer/Program.cs#L14-L26). The checked-in `appsettings.json` supplies everything except the API key [PlaneMCPServer/appsettings.json](PlaneMCPServer/appsettings.json#L1-L5).
- Set secrets via `dotnet user-secrets set "PlaneAPIKey" <token>` in the `PlaneMCPServer` project directory (UserSecretsId lives in the csproj). Env vars with the same names also work.
- Dependency injection: `PlaneApiService` is registered as a singleton using `IHttpClientFactory` [PlaneMCPServer/Program.cs](PlaneMCPServer/Program.cs#L28-L38). MCP tools are auto-discovered from this assembly via `.WithToolsFromAssembly()` [Program.cs](PlaneMCPServer/Program.cs#L40-L43).
- HTTP integration: `PlaneApiService` trims the base URL, sets `x-API-Key` header on every request, and hits Plane endpoints for project states (GET) and work items (POST) [PlaneMCPServer/PlaneApiService.cs](PlaneMCPServer/PlaneApiService.cs#L1-L51). Payloads are serialized with `System.Text.Json`.
- Tools surface: `GetAllWorkItemStatuses` and `CreateWorkItem` are MCP tools that take `PlaneApiService` as an injected parameter [PlaneMCPServer/Planetools.cs](PlaneMCPServer/Planetools.cs#L5-L25). Return values are currently JSON-serialized strings (double-encoded) to keep responses as JSON; keep this quirk in mind if you change result handling.
- Patterns for new tools: add static methods with `[McpServerTool]` inside a `[McpServerToolType]` class; accept `PlaneApiService` instead of manually creating `HttpClient`. Use descriptive `[Description]` attributes so MCP clients surface good UX.
- Error handling: HTTP calls rely on `EnsureSuccessStatusCode`; there is no retry logic or typed deserialization—preserve that simplicity unless requirements change.
- Build/run locally: from repo root run `dotnet build PlaneMCPServer/PlaneMCPServer.csproj`; run the server with `dotnet run --project PlaneMCPServer/PlaneMCPServer.csproj`. It listens on stdio for MCP clients rather than exposing HTTP directly.
- Dependency snapshot: packages come only from `Microsoft.Extensions.*` hosting/http and `ModelContextProtocol` preview [PlaneMCPServer/PlaneMCPServer.csproj](PlaneMCPServer/PlaneMCPServer.csproj#L1-L21).
- File locations: main code in `PlaneMCPServer/`; no tests present. Keep code ASCII-only unless upstream already includes Unicode.
- When modifying config, remember `appsettings.json` is copied to output via `CopyToOutputDirectory` [PlaneMCPServer/PlaneMCPServer.csproj](PlaneMCPServer/PlaneMCPServer.csproj#L15-L19); ensure runtime can find it relative to `AppContext.BaseDirectory`.
- Security: never commit API keys; the only secret is `PlaneAPIKey`. Prefer env/secret store when adding more Plane settings.
- If adding functionality that needs richer Plane models, consider introducing typed DTOs near `PlaneApiService` and updating existing tools to return typed JSON instead of double-serialized strings.

Questions or gaps? Tell me what feels unclear and I’ll tighten these notes.