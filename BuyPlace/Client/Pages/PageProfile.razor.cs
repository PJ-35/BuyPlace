﻿using BuyPlace.Client.Authentication;
using BuyPlace.Client.Layout;
using BuyPlace.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace BuyPlace.Client.Pages
{
    public partial class PageProfile
    {
        UserSession storedUser;
        private List<ArticleSession>? lstArticles;
        private List<ArticleSession> lstArticlesFacture;
        private List<string> lstImageArticle;
        private List<FactureSession>? lstFactures;
        private List<RelationUserArticleSession>? lstRelationUsers;
        string[]? lstImages;
        private System.Timers.Timer timer;
        private NewUser userSessionCopie;
        string modal="";
        private string? LoginMesssage = "";
        private string colorMessage { get; set; } = "bg-danger";
        private bool disabled {  get; set; }
        private decimal SousTotal;
        private decimal Taxe;
        //private double taxe

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

            disabled = true;

            var custum = (CustumAuthStateProvider)authProvider;

            storedUser = await custum.GetUserSession();

            if (storedUser is not null)
            {

                var user = await httpClient.PostAsJsonAsync("api/Account/GetbyUserName", storedUser.UserName);
                userSession = await user.Content.ReadFromJsonAsync<NewUser>();


                Console.WriteLine(userSession.UserName);
            }

             userSessionCopie = new NewUser
            {
                Nom = userSession.Nom,
                UserName = userSession.UserName,
                Courriel = userSession.Courriel,
                Image = userSession.Image,
                Prenom = userSession.Prenom,
                Id= userSession.Id,

            };///Copie de l'utilisateur de session



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
        //[Inject]
        //IJSRuntime Js {  get; set; }

        private async Task UpdateProfile()
        {

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(userSession, new ValidationContext(userSession), validationResults, true);

            LoginMesssage = "";


            if (string.IsNullOrWhiteSpace(userSession.UserName) || string.IsNullOrWhiteSpace(userSession.Courriel) || string.IsNullOrWhiteSpace(userSession.Nom) || string.IsNullOrWhiteSpace(userSession.Prenom))
            {
                LoginMesssage += "Veuillez remplir tous les champs\n";
            }
            if(userSession.Nom.Length < 3) {
                LoginMesssage += "Le nom choisir doit avoir plus de 3 caractère\n";
            }
            if (userSession.Prenom.Length < 3)
            {
                LoginMesssage += "Le prenom choisir doit avoir plus de 3 caractère\n";
            }

            if (userSession.UserName.Length < 3)
            {
                LoginMesssage += "Le nom d'utilisateur choisir doit avoir plus de 3 caractère\n";
            }

            if (!(string.IsNullOrWhiteSpace(userSession.UserName) 
                || string.IsNullOrWhiteSpace(userSession.Courriel) 
                || string.IsNullOrWhiteSpace(userSession.Nom) 
                || string.IsNullOrWhiteSpace(userSession.Prenom)) && userSession.UserName.Length >= 3 && 
                userSession.Prenom.Length >= 3 && userSession.Nom.Length>=3)
            {
                LoginMesssage = "";

                if (!userSession.Equals(userSessionCopie))
                {
                    userSessionCopie.Mdp = "Qwerty1234";

                    var loginResponse = await httpClient.PostAsJsonAsync<NewUser>("api/Account/UpdateUser", userSessionCopie);

                    string tt = await loginResponse.Content.ReadAsStringAsync();
                    // await Js.InvokeVoidAsync("alert",tt);
                    if (!loginResponse.IsSuccessStatusCode)
                    {
                        LoginMesssage = tt;

                    }
                    else
                    {
                        LoginMesssage = "Modiffication";
                        colorMessage = "bg-success";
                        disabled = true;
                        userSession.Nom = userSessionCopie.Nom;
                        userSession.UserName = userSessionCopie.UserName;
                        userSession.Courriel = userSessionCopie.Courriel;
                        userSession.Image = userSessionCopie.Image;
                        userSession.Prenom = userSessionCopie.Prenom;
                        //userSession = userSessionCopie;
                        //InvokeAsync(StateHasChanged);
                    }

                }
                //else
                //    modal = "modal";
                //InvokeAsync(StateHasChanged);
            }

            //if (isValid)
            //{
            //    if (string.IsNullOrWhiteSpace(userSession.UserName) || string.IsNullOrWhiteSpace(userSession.Courriel) || string.IsNullOrWhiteSpace(userSession.Nom) || string.IsNullOrWhiteSpace(userSession.Prenom))
            //    {
            //        LoginMesssage += "Veuillez remplir tous les champs\n";
            //    }
               


            //}
            //else
            //{
            //    if (!(string.IsNullOrWhiteSpace(userSession.UserName) || string.IsNullOrWhiteSpace(userSession.Courriel) || string.IsNullOrWhiteSpace(userSession.Nom) || string.IsNullOrWhiteSpace(userSession.Prenom)))
            //    {
            //        LoginMesssage = "";
            //        //InvokeAsync(StateHasChanged);
            //    }
            //    else
            //        LoginMesssage = "Veuillez remplir tous les champs";
            //}

            //if(isValid && LoginMesssage == "")
            //{
            //    if (!userSession.Equals(userSessionCopie))
            //    {
            //        userSession.Mdp = "Qwerty1234";

            //        var loginResponse = await httpClient.PostAsJsonAsync<NewUser>("api/Account/UpdateUser", userSession);

            //        string tt = await loginResponse.Content.ReadAsStringAsync();
            //       // await Js.InvokeVoidAsync("alert",tt);
            //        if (!loginResponse.IsSuccessStatusCode)
            //        {
            //            LoginMesssage = tt;
            //        }
            //        else
            //        {
            //            userSessionCopie.Nom = userSession.Nom;
            //            userSessionCopie.UserName = userSession.UserName;
            //            userSessionCopie.Courriel = userSession.Courriel;
            //            userSessionCopie.Image = userSession.Image;
            //            userSessionCopie.Prenom = userSession.Prenom;
            //        }

            //    }else
            //        modal = "modal";
            //}

           
        }

        private void HandleInputChange()
        {
            
            if (userSession.Nom !=userSessionCopie.Nom ||
                userSession.Courriel != userSessionCopie.Courriel || userSession.Prenom!= userSessionCopie.Prenom)
            {
                disabled = false;
            }
            else
            {
                disabled = true;
            }
            //InvokeAsync(StateHasChanged);

        }

        private async Task GetFacture(string id)
        {
             lstRelationUsers = await httpClient.GetFromJsonAsync<List<RelationUserArticleSession>>($"/api/factures/UserArticle?idFacture={id}");
            lstImageArticle = null;
            lstArticlesFacture = null;
            SousTotal = 0;
            Taxe = 0;
            if (lstRelationUsers != null)
            {

                lstArticlesFacture = new List<ArticleSession>();
                lstImageArticle = new List<string>();
                foreach (RelationUserArticleSession relUserArticle in lstRelationUsers)
                {

                    SousTotal += relUserArticle.Quantite * relUserArticle.PrixUnitaire;

                    var reponse = await httpClient.GetAsync($"api/article/getarticlebyid?idArticle={relUserArticle.ArticleId}");
                    if (reponse.StatusCode != HttpStatusCode.BadRequest)
                    {
                       ArticleSession article = await reponse.Content.ReadFromJsonAsync<ArticleSession>();
                        byte[] imageBytes = await httpClient.GetByteArrayAsync($"api/article/{article.Id}?timestamp={DateTime.UtcNow.Ticks}");
                        string image = $"data:image/jpg;base64,{Convert.ToBase64String(imageBytes)}";
                        lstArticlesFacture.Add(article);
                        lstImageArticle.Add(image);
                    }
                }

                Taxe = SousTotal * 10 / 100;
            }
        }

        private async Task ChargerFacture()
        {
            List<string> LstRelationId = new List<string>();
            List<string> LstArtId = new List<string>();
            try
            {
                lstFactures = await httpClient.GetFromJsonAsync<List<FactureSession>>($"/api/factures/factureUser?idUser={userSession.Id}");

                //if (lstFactures!=null)
                //{
                //    foreach (FactureSession fct in lstFactures)
                //    {
                //        if (fct.RelationsUserArticles != null)
                //        {
                //            foreach (string idUserArt in fct.RelationsUserArticles)
                //            {
                //                LstRelationId.Add(idUserArt);
                //            }
                //        }

                //    }
                //}


                //lstRelationUsers= await httpClient.GetFromJsonAsync<List<RelationUserArticleSession>>($"/api/userArticle/articleUserList?lstIdUA={LstRelationId}");

                //if (lstRelationUsers!=null && lstRelationUsers.Count!=0)
                //{
                //    foreach (RelationUserArticleSession lstRel in lstRelationUsers)
                //    {
                //        LstArtId.Add(lstRel.ArticleId.ToString());
                //    }

                //    lstArticlesFacture = await httpClient.GetFromJsonAsync<List<ArticleSession>>($"/api/Article/articleUserFact?lstIdUA={LstArtId}");

                //}

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

        private void CloseModalForm()
        {
            userSessionCopie = new NewUser
            {
                Nom = userSession.Nom,
                UserName = userSession.UserName,
                Courriel = userSession.Courriel,
                Image = userSession.Image,
                Prenom = userSession.Prenom,
                Id = userSession.Id,

            };

            LoginMesssage = "";
            disabled = true;
        }
        private async void GetImageArticle()
        {
            lstArticles = await httpClient.GetFromJsonAsync<List<ArticleSession>>($"api/article/articleUser?idUser={userSession.Id}");


            if (lstArticles is not null)
            {
                lstImages = new string[lstArticles.Count];
                for (int i = 0; i < lstArticles.Count; i++)
                {

                    byte[] imageBytes = await httpClient.GetByteArrayAsync($"api/article/{lstArticles[i].Id}?timestamp={DateTime.UtcNow.Ticks}");
                    lstImages[i] = $"data:image/jpg;base64,{Convert.ToBase64String(imageBytes)}";
                   
                }
            }
        }
    }
}
