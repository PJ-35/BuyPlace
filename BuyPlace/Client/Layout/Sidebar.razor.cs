using BuyPlace.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System.Net;
using System.Net.Http.Json;

namespace BuyPlace.Client.Layout
{
    public partial class Sidebar
    {
        private List<CategorieSession> categories;
        private System.Timers.Timer timer;
        private string validMin = "";
        private string validMax = "";
        [Inject]
        NavigationManager navigationManager { get; set; }

        [Inject]
        HttpClient httpClient { get; set; }

        private class FormData
        {
            public string Min { get; set; }
            public string Max { get; set; }
        }

        private FormData formData = new FormData();
       

        private async Task SubmitForm()
        {
            int variableTravail = 0;
            if (int.TryParse(formData.Min, out variableTravail))
            {
                formDataService.Min = variableTravail;
                validMin = "is-valid";
                formDataService.MinChange = true;
            }
            else
            {
                validMin = "is-invalid";
                formDataService.MinChange= false;
            }
            if (string.IsNullOrWhiteSpace(formData.Min))
                validMin = "";
            if (int.TryParse(formData.Max, out variableTravail))
            {
                formDataService.Max = variableTravail;
                validMax = "is-valid";
                formDataService.MaxChange = true;
            }
            else
            {
                validMax = "is-invalid";
                formDataService.MaxChange = false;
            }
            if (string.IsNullOrWhiteSpace(formData.Max))
                validMax = "";
            
        }
        private void chargement()
        {
            if(formDataService.MinChange)
                formData.Min = formDataService.Min.ToString();
            if (formDataService.MaxChange)
                formData.Max = formDataService.Max.ToString();

        }

        private async Task chargement2()
        {
            categories = await httpClient.GetFromJsonAsync<List<CategorieSession>>("api/categorie/categorie");
            if (!string.IsNullOrWhiteSpace(formDataService.categorie))
            {
                var reponse = await httpClient.GetAsync($"api/categorie/cherche?categorie={formDataService.categorie}");
                if (reponse.StatusCode==HttpStatusCode.BadRequest)
                    navigationManager.NavigateTo("/error404",true);

            }
        }
    protected override async Task OnInitializedAsync()
        {
            chargement();
            chargement2();
            timer = new System.Timers.Timer(2000);
            timer.Elapsed += async (sender, e) =>
            {
                //chargement();
                ////categories = mongoService.GetCategories();
                ////if (!string.IsNullOrWhiteSpace(formDataService.categorie))
                ////{
                ////    Categories cat = mongoService.GetCategorie(formDataService.categorie);
                ////    if (cat is null)
                ////        navigationManager.NavigateTo("/error404");

                ////}
                chargement2();
                InvokeAsync(StateHasChanged);
            };
            timer.Start();

        }

        public void Dispose()
        {
            timer?.Stop();
            timer?.Dispose();
        }
    }
}
