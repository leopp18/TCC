using System.Text.Json;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly AuthService _authService;

    public ApiService(HttpClient httpClient, AuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
        _httpClient.BaseAddress = new Uri("https://sgpmodo-fdh9fahqanfrche3.brazilsouth-01.azurewebsites.net");
    }

    private async Task AddAuthorizationHeader(HttpRequestMessage request)
    {
        var token = await _authService.GetAuthTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }

    public T GetData<T>(string endpoint, string token) where T : class, new()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
        var response = _httpClient.SendAsync(request).GetAwaiter().GetResult(); // Espera de forma síncrona a resposta
        response.EnsureSuccessStatusCode();
        var responseString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult(); // Espera de forma síncrona para obter o conteúdo da resposta

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var result = JsonSerializer.Deserialize<T>(responseString, options);
        return result ?? new T();
    }


    public async Task<T> GetDataAsync<T>(string endpoint) where T : class, new()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
        await AddAuthorizationHeader(request);
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var result = JsonSerializer.Deserialize<T>(responseString, options);
        return result ?? new T();
    }

    public async Task<T> PostDataAsync<T>(string endpoint, HttpContent content) where T : class, new()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
        await AddAuthorizationHeader(request);
        request.Content = content;
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var result = JsonSerializer.Deserialize<T>(responseString, options);
        return result ?? new T();
    }

    public async Task<T> PutDataAsync<T>(string endpoint, HttpContent content) where T : class, new()
    {
        var request = new HttpRequestMessage(HttpMethod.Put, endpoint);
        await AddAuthorizationHeader(request);
        request.Content = content;
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var result = JsonSerializer.Deserialize<T>(responseString, options);
        return result ?? new T();
    }

    public async Task<T> DeleteDataAsync<T>(string endpoint) where T : class, new()
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, endpoint);
        await AddAuthorizationHeader(request);
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var result = JsonSerializer.Deserialize<T>(responseString, options);
        return result ?? new T();
    }
}
