
using BuyPlace.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BuyPlace.Client.Pages
{
    public partial class PageMagasiner
    {
        [Parameter]
        public string categorie { get; set; }

        string[] lstImages;


        [Inject]
        HttpClient httpClient { get; set; }


        [Inject]
        NavigationManager navigationManager { get; set; }

        private List<ArticleSession> lstArticles;

        //private MongoServiceArticle mongoService = new MongoServiceArticle();
        private System.Timers.Timer timer;

        private class FormData
        {
            public string recherche { get; set; }
        }


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
            //}
        }
        private string searchQuery;
        protected override void OnParametersSet()
        {
            chargement();
            if (navigationManager.Uri.IndexOf('?') != -1)
            {

                var uri = new Uri(navigationManager.Uri);
                searchQuery = uri.Query.TrimStart('?').Split('&')
                    .Select(p => p.Split('='))
                    .Where(p => p.Length == 2 && p[0] == "recherche")
                    .Select(p => Uri.UnescapeDataString(p[1]))
                    .FirstOrDefault();
                if (searchQuery is null)
                {
                    FormDataService.boolRecherche = false;
                    navigationManager.NavigateTo("/error404");
                }
                else
                {
                    FormDataService.recherche = searchQuery;
                    FormDataService.boolRecherche = true;
                }

            }
            else
                FormDataService.boolRecherche = false;
        }

        private void Recherche()
        {
            if (lstArticles is not null && !string.IsNullOrWhiteSpace(FormDataService.recherche))
            {
                lstArticles = lstArticles.Where(x => x.nom.ToLower().IndexOf(FormDataService.recherche.ToLower()) != -1 || x.description.ToLower().IndexOf(FormDataService.recherche.ToLower()) != -1).ToList();
                FormDataService.boolRecherche = true;
            }
            else if (string.IsNullOrWhiteSpace(FormDataService.recherche))
                FormDataService.boolRecherche = false;
        }
        private async void chargement()
        {
            //FormDataService.details = "";
            if (!string.IsNullOrWhiteSpace(categorie))
            {
                var reponse = await httpClient.GetAsync($"api/categorie/cherche?categorie={categorie}");
                if (reponse.StatusCode == HttpStatusCode.BadRequest)
                    navigationManager.NavigateTo("/error404", true);
                FormDataService.categorie = categorie;
                lstArticles = await httpClient.GetFromJsonAsync<List<ArticleSession>>($"api/article/article?categorie={categorie}");
            }
            else
            {
                lstArticles = await httpClient.GetFromJsonAsync<List<ArticleSession>>("api/article/article");
                FormDataService.categorie = null;
            }

            if (lstArticles is not null)
            {
                if (FormDataService.MinChange && FormDataService.MaxChange)
                {
                    lstArticles = lstArticles.Where(x => x.prix >= FormDataService.Min && x.prix <= FormDataService.Max).ToList();
                }
                else
                {
                    if (FormDataService.MinChange)
                        lstArticles = lstArticles.Where(x => x.prix >= FormDataService.Min).ToList();
                    if (FormDataService.MaxChange)
                        lstArticles = lstArticles.Where(x => x.prix <= FormDataService.Max).ToList();
                }
            }

            if (FormDataService.boolRecherche)
            {
                Recherche();

            }


            if(lstArticles is not null)
            {
                lstImages = new string[lstArticles.Count];
                for (int i = 0; i < lstArticles.Count; i++)
                {

                        byte[] imageBytes = await httpClient.GetByteArrayAsync($"api/article/{lstArticles[i].Id}?timestamp={DateTime.UtcNow.Ticks}");
                    //string image = $"https://picsum.photos/800/600?category={categories[i].nom}&id={imageId}";
                    lstImages[i] = $"data:image/jpg;base64,{Convert.ToBase64String(imageBytes)}";
                    /*if (lstImages[i] == null)
                    {
                        lstImages[i]= $"https://picsum.photos/800/600?category=watch&id={lstArticles[i].Id}";
                    }*/

                }
            }


        }
        public void Dispose()
        {
            timer?.Stop();
            timer?.Dispose();
        }
    }
}
