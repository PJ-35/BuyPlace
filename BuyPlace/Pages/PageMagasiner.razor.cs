using BuyPlace.Data;
using BuyPlace.Model;
using BuyPlace.Service;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Globalization;

namespace BuyPlace.Pages
{
    public partial class PageMagasiner
    {
        [Parameter]
        public string categorie { get;set; }




        [Inject]
        NavigationManager navigationManager { get; set; }

        private List<Article> lstArticles;

        private MongoServiceArticle mongoService = new MongoServiceArticle();
        private System.Timers.Timer timer;

        private class FormData
        {
            public string recherche { get; set; }
        }

        private FormData formData = new FormData();

        protected override async Task OnInitializedAsync()
        {
            chargement();
            //if (!FormDataService.boolRecherche)
            //{
                timer = new System.Timers.Timer(2000);
                timer.Elapsed += async (sender, e) =>
                {
                    chargement();
                    InvokeAsync(StateHasChanged);
                };
                timer.Start();
            //}
        }

        protected override void OnParametersSet()
        {
            chargement();
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

        private void chargement()
        {
            string baseurl = navigationManager.BaseUri + "magasiner/";
            string url = navigationManager.Uri;
            string id = url.Replace(baseurl, "");
            if (id != url)
            {
                lstArticles = mongoService.GetArticles(id);
                lstArticles = lstArticles.Count == 0 ? null : lstArticles;
            }
            else
                lstArticles = mongoService.GetArticles();

            if(lstArticles is not null)
            {
                if (FormDataService.MinChange && FormDataService.MaxChange)
                {
                    lstArticles = lstArticles.Where(x => x.prix >= FormDataService.Min && x.prix <= FormDataService.Max).ToList();
                    lstArticles = lstArticles.Count == 0 ? null : lstArticles;
                }
                else
                {
                    if (FormDataService.MinChange)
                        lstArticles = lstArticles.Where(x => x.prix >= FormDataService.Min).ToList();
                    if (FormDataService.MaxChange)
                        lstArticles = lstArticles.Where(x => x.prix <= FormDataService.Max).ToList();
                    lstArticles = lstArticles.Count == 0 ? null : lstArticles;
                }
            }

            if (FormDataService.boolRecherche)
            {
                Recherche();
                if (lstArticles is not null)
                    lstArticles = lstArticles.Count == 0 ? null : lstArticles;

            }

        }
        public void Dispose()
        {
            timer?.Stop();
            timer?.Dispose();
        }
    }
}

