using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.Sources.Clear();
builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables();

var planeApiKey = builder.Configuration["PlaneAPIKey"];
var baseUrl = builder.Configuration["BaseUrl"];
var workspaceId = builder.Configuration["WorkspaceId"];
var projectId = builder.Configuration["ProjectId"];

if(string.IsNullOrEmpty(planeApiKey))
    throw new InvalidOperationException("PlaneAPIKey is missing from config.");
if(string.IsNullOrEmpty(baseUrl))
    throw new InvalidOperationException("BaseUrl is missing from config.");
if(string.IsNullOrEmpty(workspaceId))
    throw new InvalidOperationException("WorkspaceId is missing from config.");
if(string.IsNullOrEmpty(projectId))
    throw new InvalidOperationException("ProjectId is missing from config.");