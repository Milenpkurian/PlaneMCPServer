using System.Text.Json;

public class PlaneApiService(
    IHttpClientFactory httpClientFactory,
    string baseUrl,
    string planeApiKey,
    string workspaceId,
    string projectId)
{
    string _baseUrl = baseUrl.TrimEnd('/');

    public async Task<string> GetProjectStateAsync()
    {
        var httpClient = httpClientFactory.CreateClient();

        httpClient.DefaultRequestHeaders.Add("x-API-Key", planeApiKey);

        var url = $"{_baseUrl}/api/v1/workspaces/{workspaceId}/projects/{projectId}/states/";

        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return content;
    }

    public async Task<string> CreateWorkItemAsync(string name, string descriptionHtml, string stateId)
    {
        var httpClient = httpClientFactory.CreateClient();

        httpClient.DefaultRequestHeaders.Add("x-API-Key", planeApiKey);

        var url = $"{_baseUrl}/api/v1/workspaces/{workspaceId}/projects/{projectId}/work-items/";

        var requestBody = new
        {
            name = name,
            description_html = descriptionHtml,
            state = stateId
        };

        var jsonContent = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        return responseContent;
    }
}