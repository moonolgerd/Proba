using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;
using System;

namespace Proba
{
    internal class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILogger<CustomAuthStateProvider> logger;

        public ILocalStorageService LocalStorage { get; }

        public CustomAuthStateProvider(ILocalStorageService localStorage, ILogger<CustomAuthStateProvider> logger)
        {
            LocalStorage = localStorage;
            this.logger = logger;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await LocalStorage.GetItemAsync<string>("token");

                if (string.IsNullOrWhiteSpace(token))
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                var handler = new JwtSecurityTokenHandler();
                var readToken = handler.ReadJwtToken(token.Substring(token.IndexOf(' ') + 1));

                var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(readToken.Claims, "apiauth"));
                var state = new AuthenticationState(authenticatedUser);
                return state;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Authentification failed");
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        public async Task MarkUserAsAuthenticated(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var readToken = handler.ReadJwtToken(token.Substring(token.IndexOf(' ') + 1));

            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(readToken.Claims, "apiauth"));

            await LocalStorage.SetItemAsync("token", token);

            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public async Task MarkUserAsLoggedOut()
        {
            await LocalStorage.RemoveItemAsync("token");
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }
    }
}