using BuyPlace.Client.Authentication;
using BuyPlace.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Microsoft.VisualBasic;
using System.Net;
using System.Net.Http.Json;

namespace BuyPlace.Client.Pages
{
    public partial class PageDetails
    {
        [Parameter]
        public string idArticle { get; set; }

        private ArticleSession article;
        
        private string erreur;
        private string title;
        private string image;
        private AuthenticationState authState;
        NewUser userSession = new NewUser();
        UserSession storedUser;
        [Inject]
        IJSRuntime ijsruntime {  get; set; }

        [Inject]
        NavigationManager navigationManager { get; set; }
        [Inject]
        HttpClient httpClient { get; set; }

        [Inject]
        AuthenticationStateProvider authProvider { get; set; }

        private System.Timers.Timer timer;


        protected override async Task OnInitializedAsync()
        {

            chargement();
            timer = new System.Timers.Timer(2000);
            timer.Elapsed += async (sender, e) =>
            {
                chargement();
                InvokeAsync(StateHasChanged);
            };
            timer.Start();
        }

        private async void redirection()
        {
            if (storedUser is null)
                await ijsruntime.InvokeVoidAsync("localStorage.setItem", "sharedInformation", article.Id);
            navigationManager.NavigateTo("connexion", true);
        }
        private async void chargement()
        {


            var custum = (CustumAuthStateProvider)authProvider;

            storedUser = await custum.GetUserSession();

            if (storedUser is not null)
            {

                var user = await httpClient.PostAsJsonAsync("api/Account/GetbyUserName", storedUser.UserName);
                userSession = await user.Content.ReadFromJsonAsync<NewUser>();
            }

            var reponse= await httpClient.GetAsync($"api/article/getarticlebyid?idArticle={idArticle}");
            if(reponse.StatusCode == HttpStatusCode.BadRequest)
            {
                erreur= await reponse.Content.ReadAsStringAsync();
                title = "Détails||Aucun article trouvé";
            }
            else
            {
                article= await reponse.Content.ReadFromJsonAsync<ArticleSession>();
                erreur = "";
                title = $"Détails||{article.nom}";
                byte[] imageBytes = await httpClient.GetByteArrayAsync($"api/article/{article.Id}?timestamp={DateTime.UtcNow.Ticks}");
                image = $"data:image/jpg;base64,{Convert.ToBase64String(imageBytes)}";

            }

        }
    }
}
