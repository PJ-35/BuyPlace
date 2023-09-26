using BuyPlace.Data;
using BuyPlace.Model;
using System;

namespace BuyPlace.Pages
{
    public partial class PageAccueil
    {
        private List<Categories> categories;
        private MongoServiceCategories mongoService=new MongoServiceCategories();
        private System.Timers.Timer timer;
        protected override async Task OnInitializedAsync()
        {
            categories = mongoService.GetCategories();
            timer = new System.Timers.Timer(2000);
            timer.Elapsed += async (sender, e) =>
            {
                categories = mongoService.GetCategories();
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
