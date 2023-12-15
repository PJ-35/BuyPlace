﻿using BuyPlace.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Net;
using Microsoft.AspNetCore.Components.Authorization;
using BuyPlace.Client.Authentication;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.JSInterop;

namespace BuyPlace.Client.Pages
{
    [Authorize]
    public partial class Panier
    {

        UserSession storedUser;

        string[] lstImages;

        NewUser userSession = new NewUser();
        private AuthenticationState authState;
        private List<ArticleSession> lstArticles;
        List<CategorieSession> categorieSessions;
        decimal sousTotal = 0;
        decimal total = 0;



        [Inject]
        HttpClient httpClient { get; set; }

        [Inject]
        JSRuntime ijs { get; set; }

        [Inject]
        NavigationManager navmanager { get; set; }


        [Inject]
        AuthenticationStateProvider authProvider { get; set; }



        //private MongoServiceArticle mongoService = new MongoServiceArticle();
        private System.Timers.Timer timer;

        protected override async Task OnInitializedAsync()
        {

            categorieSessions = await httpClient.GetFromJsonAsync<List<CategorieSession>>("api/categorie/categorie");
           

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

            chargement();

            timer = new System.Timers.Timer(2000);
            timer.Elapsed += async (sender, e) =>
            {
                total = 0;
                sousTotal = 0;
                chargement();
              
                InvokeAsync(StateHasChanged);
            };
            timer.Start();
            //CalculerTotal();
            //}
        }

        //protected override void OnAfterRender(bool firstRender)
        //{
        //    base.OnAfterRender(firstRender);
        //    CalculerTotal();
        //}

        private async void paiement()
        {
            List<string> lstId=new List<string>();
            lstId[0] = userSession.Id;
            for (int i = 1; i < lstArticles.Count+1; i++)
            {
                lstId[i] = lstArticles[i - 1].Id;
            }
            var reponse = await httpClient.PostAsJsonAsync($"api/Factures/{total}", lstId);
            if(reponse.IsSuccessStatusCode)
            {
               await ijs.InvokeVoidAsync("alert","Paiement avec succès");
            }
            else
            {
                await ijs.InvokeVoidAsync("alert", "Une erreur s'est produite");

            }
        }


        private async void chargement()
        {
            lstArticles = await httpClient.GetFromJsonAsync<List<ArticleSession>>($"api/userArticle/articleUserPanier?userId={userSession.Id}" );

            if (lstArticles != null && lstArticles.Count() > 0)
            {
                lstImages = new string[lstArticles.Count];
                for (int i = 0; i < lstArticles.Count; i++)
                {

                    byte[] imageBytes = await httpClient.GetByteArrayAsync($"api/article/{lstArticles[i].Id}?timestamp={DateTime.UtcNow.Ticks}");
                    lstImages[i] = $"data:image/jpg;base64,{Convert.ToBase64String(imageBytes)}";

                }
              

            }
        }

        private string GetCategorie(string articleId)
        {
           
            if (categorieSessions is not null)
            {
                foreach (var session in categorieSessions)
                {
                    if(session.Id == articleId)
                    {
                        return session.nom;
                    }
                }
            }

            return "";
        }

        private void CalculerTotal()
        {
            if(lstArticles != null && lstArticles.Count() > 0)
            {

                foreach(var session in lstArticles)
                {
                    sousTotal += session.prix;
                }
                total = (decimal)1.15*sousTotal + 10 ;
                
            }
            else { total = 0; }
            
        }
        //private async void GetImageArticle()
        //{
        //    lstArticles = await httpClient.GetFromJsonAsync<List<ArticleSession>>($"api/article/articleUser?idUser={userSession.Id}");


        //    if (lstArticles is not null)
        //    {
        //        lstImages = new string[lstArticles.Count];
        //        for (int i = 0; i < lstArticles.Count; i++)
        //        {

        //            byte[] imageBytes = await httpClient.GetByteArrayAsync($"api/article/{lstArticles[i].Id}?timestamp={DateTime.UtcNow.Ticks}");
        //            lstImages[i] = $"data:image/jpg;base64,{Convert.ToBase64String(imageBytes)}";

        //        }
        //    }
        //}

        private async void Payer()
        {

        }
    }
}
