using Blazored.SessionStorage;
using BuyPlace.Client.Extensions;
using BuyPlace.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BuyPlace.Client.Authentication
{
    public class CustumAuthStateProvider:AuthenticationStateProvider
    {
        private readonly ISessionStorageService _sessionStorageService;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        public CustumAuthStateProvider(ISessionStorageService sessionStorageService)
        {
            _sessionStorageService = sessionStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userSession = await _sessionStorageService.ReadEncriptedItemAsync<UserSession>("UserSession");

                if (userSession == null)
                {
                    return await Task.FromResult(new AuthenticationState(_anonymous));
                }

                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name,userSession.UserName),
                    new Claim(ClaimTypes.Role,userSession.Role)
                }, "JwtAuth"));

                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch
            {

                return await Task.FromResult(new AuthenticationState(_anonymous));
            }
        }


        public async Task UpdateAuthState(UserSession? userSession)
        {
            ClaimsPrincipal claimsPrincipal;

            if (userSession is not null)
            {
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userSession.UserName),
                    new Claim(ClaimTypes.Role, userSession.Role),
                }));

                userSession.ExpiryTimeStamp = DateTime.Now.AddSeconds(userSession.ExpiresIn);
                await _sessionStorageService.SaveItemEncryptedAsync("UserSession", userSession);
            }
            else
            {
                claimsPrincipal = _anonymous;
                await _sessionStorageService.RemoveItemAsync("UserSession");
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }


        public async Task<string> GetToken()
        {
            var result = string.Empty;

            try
            {
                var userSession = await _sessionStorageService.ReadEncriptedItemAsync<UserSession>("UserSession");
                if (userSession is not null && DateTime.Now < userSession.ExpiryTimeStamp)
                    result = userSession.Token;
            }
            catch
            {

            }

            return result;
        }
    }
}
