using System.Text.Json;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://localhost:7083/");
    }

    public T GetData<T>(string endpoint, string token) where T : class, new()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = _httpClient.Send(request);
        response.EnsureSuccessStatusCode();
        var responseString = response.Content.ReadAsStringAsync().Result;

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var result = JsonSerializer.Deserialize<T>(responseString, options);
        return result ?? new T();
    }

    public async Task<T> GetDataAsync<T>(string endpoint, string token) where T : class, new()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Configura a desserialização para não ser sensível a maiúsculas/minúsculas
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // Desserializa e verifica nulo
        var result = JsonSerializer.Deserialize<T>(responseString, options);
        return result ?? new T();  // Retorna uma nova instância padrão se a desserialização resultar em nulo
    }

    public async Task<T> PostDataAsync<T>(string endpoint, HttpContent content, string token) where T : class, new()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
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

    public async Task<T> PutDataAsync<T>(string endpoint, HttpContent content, string token) where T : class, new()
    {
        var request = new HttpRequestMessage(HttpMethod.Put, endpoint);
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
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

    public async Task<T> DeleteDataAsync<T>(string endpoint, string token) where T : class, new()
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, endpoint);
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
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
