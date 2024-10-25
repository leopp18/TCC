using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

public class AuthService : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private readonly NavigationManager _navigationManager;

    public AuthService(HttpClient httpClient, IJSRuntime jsRuntime, NavigationManager navigationManager)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
        _navigationManager = navigationManager;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

        if (string.IsNullOrEmpty(token))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token) as JwtSecurityToken;
        var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");

        // Adicione a permissão como uma claim ao ClaimsIdentity
        var permissaoClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "permissao");

        if (permissaoClaim != null)
        {
            identity.AddClaim(new Claim("permissao", permissaoClaim.Value));
        }

        var user = new ClaimsPrincipal(identity);

        return new AuthenticationState(user);
    }

    public async Task<bool> Login(string nome, string senha)
    {
        var loginModel = new { Nome = nome, Senha = senha };
        var response = await _httpClient.PostAsJsonAsync("https://localhost:7083/api/Usuario/auth/login", loginModel);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var loginResponse = System.Text.Json.JsonSerializer.Deserialize<LoginResponse>(responseContent);

            // Adicione a permissão como uma claim ao ClaimsIdentity
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(loginResponse.token) as JwtSecurityToken;
            var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");

            // Adiciona a permissão como uma claim separada
            identity.AddClaim(new Claim("permissao", loginResponse.permissao.ToString()));

            var user = new ClaimsPrincipal(identity);

            // Armazene o token no localStorage
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", loginResponse.token);

            return true;
        }

        return false;
    }

    public async Task Logout()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
        _navigationManager.NavigateTo("/login");
    }

    private class LoginResponse
    {
        public string token { get; set; }
        public int permissao { get; set; }
    }
}
