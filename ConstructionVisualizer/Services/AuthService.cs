using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConstructionVisualizer.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly IMobileStorageService _storage;

    public bool IsAuthenticated { get; private set; }
    public User User { get; private set; }

    public AuthService(HttpClient httpClient, IMobileStorageService storage)
    {
        _httpClient = httpClient;
        _storage = storage;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", new { email, password });
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuthResult>();
                await _storage.SaveUserDataAsync("auth_token", result.Token);
                await _storage.SaveUserDataAsync("user_data", JsonSerializer.Serialize(result.User));

                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.Token);

                IsAuthenticated = true;
                User = result.User;
                return true;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Login failed: {ex.Message}");
        }
        return false;
    }

    public async Task<bool> RegisterAsync(string email, string password, string name)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register",
                new { email, password, name });

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Registration failed: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> TryAutoLoginAsync()
    {
        var token = await _storage.GetUserDataAsync("auth_token");
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // Verify token is still valid
            var response = await _httpClient.GetAsync("api/auth/validate");
            if (response.IsSuccessStatusCode)
            {
                var userData = await _storage.GetUserDataAsync("user_data");
                User = JsonSerializer.Deserialize<User>(userData);
                IsAuthenticated = true;
                return true;
            }
        }
        return false;
    }
}