
using BuyPlace.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace BuyPlace.Client.Pages
{
    public partial class PageAccueil
    {
        [Inject]
        HttpClient httpClient { get; set; }

        [Inject]
        IJSRuntime js {  get; set; }

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

            //await js.InvokeVoidAsync("typewriterEffect.init", "wel", new
            //{
            //    strings = new[] { "Welcome To Our Studio!" },
            //    autoStart = true,
            //    loop = true,
            //});

            var options = new
            {
                autoStart = true,
                loop = true,
                delay = 150,
            };

            await js.InvokeVoidAsync("typewriterEffect.init", "wel", options);
        }

        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    if (firstRender)
        //    {
        //        // Appel à JSRuntime ici après le rendu initial
        //        await js.InvokeVoidAsync("typewriterEffect.init", "wel", new
        //        {
        //            strings = new[] { "Hello!", "This is the Typewriter effect in Blazor!" },
        //            autoStart = true,
        //            loop = true,
        //        });
        //    }
        //}

        public void Dispose()
        {
            timer?.Stop();
            timer?.Dispose();
        }
    }
}
