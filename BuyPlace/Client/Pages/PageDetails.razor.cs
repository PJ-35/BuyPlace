using BuyPlace.Shared;
using Microsoft.AspNetCore.Components;
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

        [Inject]
        HttpClient httpClient { get; set; }

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
            //}
        }

        private async void chargement()
        {
             var reponse= await httpClient.GetAsync($"api/article/getarticlebyid?idArticle={idArticle}");
            FormDataService.details = "";
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
                FormDataService.details = article.Id;
                var imageReponse = await httpClient.GetAsync("/images_articles/" + article.Id + ".jpg");
                if (imageReponse.IsSuccessStatusCode)
                {
                    image = $"{article.Id}.jpg";
                }
                else
                {
                    image = "indisponible.jpg";
                }

            }

        }
    }
}
