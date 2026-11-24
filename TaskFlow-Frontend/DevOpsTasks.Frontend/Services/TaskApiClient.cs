using System.Net.Http.Json;
using DevOpsTasks.Frontend.Models;

namespace DevOpsTasks.Frontend.Services;

public sealed class TaskApiClient(HttpClient http)
{
    public async Task<List<WorkTask>> GetAllAsync()
        => await http.GetFromJsonAsync<List<WorkTask>>("api/tasks") ?? [];

    public async Task CreateAsync(SaveTaskRequest request)
    {
        using var response = await http.PostAsJsonAsync("api/tasks", request);
        await EnsureSuccessfulAsync(response);
    }

    public async Task UpdateAsync(int id, SaveTaskRequest request)
    {
        using var response = await http.PutAsJsonAsync($"api/tasks/{id}", request);
        await EnsureSuccessfulAsync(response);
    }

    public async Task DeleteAsync(int id)
    {
        using var response = await http.DeleteAsync($"api/tasks/{id}");
        await EnsureSuccessfulAsync(response);
    }

    private static async Task EnsureSuccessfulAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode) return;
        var detail = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException(
            $"API returned {(int)response.StatusCode} ({response.ReasonPhrase}). {detail}");
    }
}
