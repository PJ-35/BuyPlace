using BuyPlace.Client.Authentication;
using BuyPlace.Client.Layout;
using BuyPlace.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Net.Http.Json;

namespace BuyPlace.Client.Pages
{
    public partial class PageProfile
    {
        UserSession storedUser;
        private List<ArticleSession>? lstArticles;
        private List<FactureSession>? lstFactures;
        string[]? lstImages;
        private System.Timers.Timer timer;

        string erreur = "";
        //private UserSession user;

        NewUser userSession = new NewUser();
        private AuthenticationState authState;
        [Inject]
        NavigationManager navmanager { get; set; }

   

        [Inject]
        HttpClient httpClient { get; set; }

        [Inject]
        AuthenticationStateProvider authProvider { get; set; }

        protected override async Task OnInitializedAsync()
        {
            authState = await authProvider.GetAuthenticationStateAsync();

            if (!authState.User.Identity.IsAuthenticated)
            {
                navmanager.NavigateTo("/connexion");
            }



            var custum = (CustumAuthStateProvider)authProvider;

            storedUser = await custum.GetUserSession();

            if (storedUser is not null)
            {

                var user = await httpClient.PostAsJsonAsync("api/Account/GetbyUserName", storedUser.UserName);
                userSession = await user.Content.ReadFromJsonAsync<NewUser>();


                Console.WriteLine(userSession.UserName);
            }


            timer = new System.Timers.Timer(2000);
            timer.Elapsed += async (sender, e) =>
            {
                GetImageArticle();
                ChargerFacture();
                InvokeAsync(StateHasChanged);
            };
            timer.Start();

           
            //InvokeAsync(StateHasChanged);
        }

        private async Task ChargerFacture()
        {
            try
            {
                lstFactures = await httpClient.GetFromJsonAsync<List<FactureSession>>($"/api/factures/factureUser?idUser={userSession.Id}");

            }
            catch (HttpRequestException e)
            {
                // Gérer les erreurs liées à la requête HTTP.
                Console.WriteLine($"Erreur de requête HTTP : {e.Message}");
                erreur = e.Message;
            }
            catch (Exception ex)
            {
                // Gérer toute autre exception inattendue.
                Console.WriteLine($"Une erreur s'est produite : {ex.Message}");
            }
        }

        private async void GetImageArticle()
        {
            lstArticles = await httpClient.GetFromJsonAsync<List<ArticleSession>>($"api/article/articleUser?idUser={userSession.Id}");


            if (lstArticles is not null)
            {
                lstImages = new string[lstArticles.Count];
                for (int i = 0; i < lstArticles.Count; i++)
                {
                    var reponse = await httpClient.GetAsync("/images_articles/" + lstArticles[i].Id + ".jpg");
                    if (reponse.IsSuccessStatusCode)
                    {
                        lstImages[i] = $"{lstArticles[i].Id}.jpg";
                    }
                    else
                    {
                        lstImages[i] = "indisponible.jpg";
                    }
                }
            }
        }
    }
}
