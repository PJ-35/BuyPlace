using BuyPlace.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BuyPlace.Client.Pages
{
    public partial class PageAccueil
    {
        [Inject]
        HttpClient httpClient { get; set; }

        private List<CategorieSession> categories;
        private System.Timers.Timer timer;
        protected override async Task OnInitializedAsync()
        {
            categories = await httpClient.GetFromJsonAsync<List<CategorieSession>>("api/categorie/categorie");
            timer = new System.Timers.Timer(2000);
            timer.Elapsed += async (sender, e) =>
            {
                categories = await httpClient.GetFromJsonAsync<List<CategorieSession>>("api/categorie/categorie");
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
