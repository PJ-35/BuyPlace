﻿using BuyPlace.Client.Authentication;
using BuyPlace.Shared;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using System.ComponentModel.DataAnnotations;
using System;

namespace BuyPlace.Client.Pages
{
    public partial class PageAjoutArticle
    {
        private ArticleSession article = new ArticleSession();

        [Inject]
        HttpClient httpClient { get; set; }

        public string id_categorie { get; set; }

        private List<CategorieSession> categories;


        [Inject]
        AuthenticationStateProvider authStateProvider { get; set; }

        [Inject]
        NavigationManager navManager { get; set; }

        public string LoginMesssage { get; set; } = "";

        private void ValidationForm()
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
            if (article.id_categorie.Length == 0)
                LoginMesssage += "\n-Veuillez sélectionner une catégorie";
            if (LoginMesssage.Equals("Veuillez bien remplis tous les champs"))
                LoginMesssage = "";
        }

        private AuthenticationState authenticationState;

        protected override async Task OnInitializedAsync()
        {
            categories = await httpClient.GetFromJsonAsync<List<CategorieSession>>("api/categorie/categorie");
            //authenticationState = await authStateProvider.GetAuthenticationStateAsync();

            //if (!authenticationState.User.Identity.IsAuthenticated)
            //{
            //    navManager.NavigateTo("/connexion");
            //}
        }
        private async Task HandleLogin()
        {
            article.nom = article.nom.Trim();
            article.description = article.description.Trim();
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(article, new ValidationContext(article), validationResults, true);
            //if (isValid)
            //{
            //    ValidationForm();
            //}
            //else
            //{
            //    ValidationForm();  
            //}
            ValidationForm();
            if (isValid && LoginMesssage == "")
            {

            }
            
        }
    }
}