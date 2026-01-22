using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;

[McpServerToolType]
public class Planetools
{
    [McpServerTool, Description("Get all possible work item statuses that a work item could be created in. These statuses denote where a work item would be in a typical kanban workflow.The state ids returned can be used with other tools such as creating work items.")]
    public static async Task<string> GetAllWorkItemStatuses(PlaneApiService planeApiService)
    {
        var statuses = await planeApiService.GetProjectStateAsync();
        return JsonSerializer.Serialize(statuses);
    }

    [McpServerTool, Description("Create a new work item in Plane with the given name, description, and state id.Returns the created work item details.")]
    public static async Task<string> CreateWorkItem(
        PlaneApiService planeApiService,
        [Description("The name of the work item to create.Make it brief")] string name,
        [Description("The description of the work item to create, in HTML format.Should include acceptence criteria")] string description,
        [Description("The state or status id of the work item, derived from the GettAllWorkItemStatuses tool.")] string stateId)
    {
        var workItem = await planeApiService.CreateWorkItemAsync(name, description, stateId);
        return JsonSerializer.Serialize(workItem);
    }
}