# Plane MCP Server

Console-hosted Model Context Protocol (MCP) server that exposes Plane API operations over stdio. Ships two tools: list project states and create work items in Plane.

## Project layout
- Entry point and hosting setup: [PlaneMCPServer/Program.cs](PlaneMCPServer/Program.cs)
- Plane HTTP client: [PlaneMCPServer/PlaneApiService.cs](PlaneMCPServer/PlaneApiService.cs) (adds `x-API-Key`, trims base URL, uses `EnsureSuccessStatusCode`)
- MCP tools: [PlaneMCPServer/Planetools.cs](PlaneMCPServer/Planetools.cs) (auto-discovered via `.WithToolsFromAssembly()`)
- Default config: [PlaneMCPServer/appsettings.json](PlaneMCPServer/appsettings.json) (ships BaseUrl/WorkspaceId/ProjectId; API key is secret only)
- Dev container: [.devcontainer/](.devcontainer) (dotnet 10 + Node, forwarded ports for MCP inspector/proxy)

## Configuration
Configuration sources are cleared then rebuilt in this order: `appsettings.json`, user secrets, environment variables. Required keys: `PlaneAPIKey`, `BaseUrl`, `WorkspaceId`, `ProjectId` (startup throws if any are missing).

Set the API key via user secrets from the project folder:

```bash
dotnet user-secrets set "PlaneAPIKey" "<your-plane-api-key>" --project PlaneMCPServer/PlaneMCPServer.csproj
```

Or via environment variables:

```bash
export PlaneAPIKey="<your-plane-api-key>"
export BaseUrl="https://api.plane.so/"
export WorkspaceId="<workspace-id>"
export ProjectId="<project-id>"
```

## Build and run

```bash
dotnet build PlaneMCPServer/PlaneMCPServer.csproj
dotnet run --project PlaneMCPServer/PlaneMCPServer.csproj
```

The server listens on stdio for MCP clients (no HTTP listener). Ensure `appsettings.json` is copied to the output; this is handled by the csproj.

## Available MCP tools
- `Planetools.GetAllWorkItemStatuses`: GET Plane project states; returns the Plane JSON as a double-encoded JSON string (designed so clients still receive JSON payloads).
- `Planetools.CreateWorkItem`: POST a new work item with name, markdown description, and state id; returns the created item as a double-encoded JSON string.

## Dev container
Open in VS Code with Dev Containers to get the .NET 10 + Node image ([.devcontainer/devcontainer.json](.devcontainer/devcontainer.json)). Ports 6274/6277 are labeled for MCP inspector/proxy; other common app/database ports are forwarded. Node is installed via NVM in [.devcontainer/Dockerfile](.devcontainer/Dockerfile).

## Extending
- Add new tools by creating static methods with `[McpServerTool]` inside a `[McpServerToolType]` class and accept `PlaneApiService` via DI instead of creating `HttpClient` manually.
- Keep payload handling simple unless requirements change (no retries or typed DTOs today). If you change return shapes, account for the current double-serialization expectation in clients.