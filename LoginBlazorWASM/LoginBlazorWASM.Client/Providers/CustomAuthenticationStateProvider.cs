using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LoginBlazorWASM.Client
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;

        public CustomAuthenticationStateProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                string token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "Token");
                var identity = new ClaimsIdentity();

                if (!string.IsNullOrEmpty(token))
                {
                    var jwtToken = ValidateAndReadToken(token);
                    if (jwtToken != null)
                    {
                        identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
                    }
                    else
                    {
                        // Eliminar el token inválido o expirado
                        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "Token");
                    }
                }

                var user = new ClaimsPrincipal(identity);
                var state = new AuthenticationState(user);
                NotifyAuthenticationStateChanged(Task.FromResult(state));
                return state;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error al obtener el estado de autenticación: {ex.Message}");
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        private JwtSecurityToken ValidateAndReadToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(token))
                return null;

            var jwtToken = handler.ReadJwtToken(token);
            if (jwtToken.ValidTo < DateTime.UtcNow)
                return null;

            return jwtToken;
        }
    }
}