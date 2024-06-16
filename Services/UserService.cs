using HataChatSerives.Data;
using HataChatSerives.Models;
using Microsoft.EntityFrameworkCore;

public class UserService
{
    private readonly HttpClient _httpClient;

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<User> GetUserByIdAsync(int userId)
    {
        var response = await _httpClient.GetAsync($"http://your-user-service-url/api/users/{userId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<User>();
    }
}