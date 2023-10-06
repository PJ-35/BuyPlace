
using BuyPlace.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using static System.Net.Mime.MediaTypeNames;

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
                lstArticles = lstArticles.Where(x => x.nom.IndexOf(FormDataService.recherche) != -1 || x.description.IndexOf(FormDataService.recherche) != -1).ToList();
                FormDataService.boolRecherche = true;
            }
            else if (string.IsNullOrWhiteSpace(FormDataService.recherche))
                FormDataService.boolRecherche = false;
        }
        private async void chargement()
        {
            if (!string.IsNullOrWhiteSpace(categorie))
            {
                navigationManager.NavigateTo("/error404", true);
                CategorieSession cat = await httpClient.GetFromJsonAsync<CategorieSession>($"api/categorie/cherche?categorie={categorie}");
                if (string.IsNullOrWhiteSpace(cat.nom))
                    navigationManager.NavigateTo("/error404",true);
                FormDataService.categorie = categorie;
                lstArticles = await httpClient.GetFromJsonAsync<List<ArticleSession>>($"api/article/article?categorie={categorie}");
                //lstArticles = lstArticles.Count == 0 ? null : lstArticles;
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
                    //lstArticles = lstArticles.Count == 0 ? null : lstArticles;
                }
                else
                {
                    if (FormDataService.MinChange)
                        lstArticles = lstArticles.Where(x => x.prix >= FormDataService.Min).ToList();
                    if (FormDataService.MaxChange)
                        lstArticles = lstArticles.Where(x => x.prix <= FormDataService.Max).ToList();
                    //lstArticles = lstArticles.Count == 0 ? null : lstArticles;
                }
            }

            if (FormDataService.boolRecherche)
            {
                Recherche();
                //if (lstArticles is not null)
                    //lstArticles = lstArticles.Count == 0 ? null : lstArticles;

            }
            if(lstArticles is not null)
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
        public void Dispose()
        {
            timer?.Stop();
            timer?.Dispose();
        }
    }
}
