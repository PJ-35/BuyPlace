using BuyPlace.Client.Authentication;
using BuyPlace.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using System.Net;

namespace BuyPlace.Client.Pages
{
    public partial class Connexion
    {
        //private User user = new User();
        private LoginRequest logRequest = new LoginRequest();

        public string LoginMesssage { get; set; } = "";
        //private UsersService userService = new UsersService();

        [Inject]
        HttpClient httpClient { get; set; }

        [Inject]
        IJSRuntime js { get; set; }

        [Inject]
        AuthenticationStateProvider authStateProvider { get; set; }

        [Inject]
        NavigationManager navManager { get; set; }



        private async Task HandleLogin()
        {
            try
            {
                var loginResponse = await httpClient.PostAsJsonAsync<LoginRequest>("api/Account/Login", logRequest);
                //var content= await loginResponse.Content.ReadAsStringAsync();
                if (loginResponse.IsSuccessStatusCode)
                {
                    var userSession = await loginResponse.Content.ReadFromJsonAsync<UserSession>();
                    var custum = (CustumAuthStateProvider)authStateProvider;
                    await custum.UpdateAuthState(userSession);
                    if(string.IsNullOrWhiteSpace(FormDataService.details))
                        navManager.NavigateTo("/", true);
                    else
                        navManager.NavigateTo($"/detail/{FormDataService.details}",true);
                }
                else if (loginResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    LoginMesssage="nom d'utilisateur ou mot de passe invalide";
                    return;
                }
            }
            catch (Exception ex)
            {
                String t = ex.Message;
            }
        }
    }
}
