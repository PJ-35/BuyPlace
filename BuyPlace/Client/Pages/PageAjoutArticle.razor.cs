using BuyPlace.Client.Authentication;
using BuyPlace.Shared;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using System.ComponentModel.DataAnnotations;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;

namespace BuyPlace.Client.Pages
{
    public partial class PageAjoutArticle
    {
        private ArticleSession article = new ArticleSession();
        private image? image;
        [Inject]
        HttpClient httpClient { get; set; }

        public string id_categorie { get; set; }

        private List<CategorieSession> categories;


        [Inject]
        AuthenticationStateProvider authStateProvider { get; set; }

        [Inject]
        IJSRuntime js { get; set; }

        [Inject]
        NavigationManager navManager { get; set; }

        public string LoginMesssage { get; set; } = "";

        private async Task OnClick()
        {
            // Réinitialise l'objet image à null lors du clic sur le bouton de sélection
            image = null;
        }


        async Task OnChange(InputFileChangeEventArgs e)
        {
           
            if (e.File is not null&& e.File.Size>0)
            {
                var file = e.File;
                //js.InvokeVoidAsync("alert",file);
                var resizefile = await file.RequestImageFileAsync(file.ContentType, 640, 480);
                var buff = new byte[resizefile.Size];
                using (var stream = resizefile.OpenReadStream())
                {
                    await stream.ReadAsync(buff);
                }
                image = new image() { base64 = Convert.ToBase64String(buff), fileName = file.Name, contenttype = file.ContentType };
            }
        }

        private async Task ValidationFormAsync()
        {
            LoginMesssage = "Veuillez bien remplir tous les champs";
            if (article.nom.Length < 3)
                LoginMesssage += "\n-le nom doit avoir plus de 3 caractères";
            if (article.quantite < 0)
                LoginMesssage += "\n-la quantité doit être plus grand que 0";
            if (article.description.Length < 3)
                LoginMesssage += "\n-la description doit avoir plus de 3 caractères";
            if (article.prix < 0)
                LoginMesssage += "\n-le prix doit être plus grand que 0";
            if (string.IsNullOrWhiteSpace(article.id_categorie))
                LoginMesssage += "\n-Veuillez sélectionner une catégorie";
            else
            {
                var reponse = await httpClient.GetAsync($"api/categorie/cherche?categorie={article.id_categorie}");
                if (reponse.StatusCode == HttpStatusCode.BadRequest)
                    navManager.NavigateTo("/error404", true);
            }

            if (image is null)
                LoginMesssage += "\n-Veuillez télécharger une image";

        }

        private AuthenticationState authenticationState;

        protected override async Task OnInitializedAsync()
        {
            categories = await httpClient.GetFromJsonAsync<List<CategorieSession>>("api/categorie/categorie");
            authenticationState = await authStateProvider.GetAuthenticationStateAsync();

            if (!authenticationState.User.Identity.IsAuthenticated)
            {
                navManager.NavigateTo("/connexion");
            }
        }
        private async Task HandleAjout()
        {
            article.nom = article.nom.Trim();
            article.description = article.description.Trim();
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(article, new ValidationContext(article), validationResults, true);

            await ValidationFormAsync();
            if (LoginMesssage=="Veuillez bien remplir tous les champs")
            {
                LoginMesssage = ""; 

            }
            if (isValid && LoginMesssage == "")
            {
                var custum = (CustumAuthStateProvider)authStateProvider;
                UserSession storedUser = await custum.GetUserSession();

                if (storedUser is not null)
                {

                    var user = await httpClient.PostAsJsonAsync("api/Account/GetbyUserName", storedUser.UserName);
                    NewUser? userSession = await user.Content.ReadFromJsonAsync<NewUser>();
                    if (userSession is not null)
                    {
                        ArticleSession article2 = new ArticleSession()
                        {
                            Id="...",
                            id_categorie = article.id_categorie,
                            id_user = userSession.Id,
                            nom = article.nom,
                            description = article.description,
                            prix = article.prix,
                            quantite = article.quantite,
                            date = DateTime.Now

                        };

                        var reponse = await httpClient.PostAsJsonAsync("api/Article/savearticle", article2);

                    if (reponse.StatusCode == HttpStatusCode.OK)
                        {
                            string tt = await reponse.Content.ReadAsStringAsync();
                            image.id = tt;
                            await js.InvokeVoidAsync("alert",tt);
                            using (var msg = await httpClient.PostAsJsonAsync<image>("api/Article/uploadimage", image, CancellationToken.None)) {
                                string tt2 = await msg.Content.ReadAsStringAsync();
                                if (msg.IsSuccessStatusCode)
                                    navManager.NavigateTo($"/detail/{tt}");
                            }
                        }
                    }

                }

                LoginMesssage = "Désolé, une erreur s'est produite, Veuillez réessayer";
            }
            
        }
    }
}
