using BuyPlace.Client.Authentication;
using BuyPlace.Shared;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace BuyPlace.Client.Pages
{
    public partial class PageAjoutArticle
    {
        private ArticleSession article = new ArticleSession();

        [Inject]
        HttpClient httpClient { get; set; }

        [Inject]
        IJSRuntime js { get; set; }

        public string id_categorie { get; set; }

        [Inject]
        AuthenticationStateProvider authStateProvider { get; set; }

        [Inject]
        NavigationManager navManager { get; set; }

        public string LoginMesssage { get; set; } = "";

        private async Task HandleLogin()
        {
            //try
            //{
                //var loginResponse = await httpClient.PostAsJsonAsync<LoginRequest>("api/Account/Login", logRequest);
                //var content= await loginResponse.Content.ReadAsStringAsync();
            //    if (loginResponse.IsSuccessStatusCode)
            //    {
            //        var userSession = await loginResponse.Content.ReadFromJsonAsync<UserSession>();
            //        var custum = (CustumAuthStateProvider)authStateProvider;
            //        await custum.UpdateAuthState(userSession);
            //        navManager.NavigateTo("/", true);
            //    }
            //    else if (loginResponse.StatusCode == HttpStatusCode.Unauthorized)
            //    {
            //        LoginMesssage = "nom d'utilisateur ou mot de passe invalide";
            //        return;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    String t = ex.Message;
            //}
        }
    }
}
