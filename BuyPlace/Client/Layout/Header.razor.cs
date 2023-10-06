using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components;
using BuyPlace.Client.Authentication;
using BuyPlace.Shared;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;

namespace BuyPlace.Client.Layout
{
    public partial class Header
    {
        bool collapseNavMenu = true;

        string baseMenuClass = "navbar-collapse d-sm-inline-flex flex-sm-row-reverse";
        string validation = "";
        bool validationRequis = true;


        [Inject]
        HttpClient httpClient { get; set; }

        [Inject]
        FormDataService FormDataService { get; set; }

        [Inject]
        AuthenticationStateProvider authProvider { get; set; }

        // private MongoServiceArticle mongoService = new MongoServiceArticle();
       string NavMenuCssClass => baseMenuClass + (collapseNavMenu ? " collapse" : "");

        [Inject]
        NavigationManager navigationManager { get; set; }
        void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }

        protected override void OnInitialized()
        {
            navigationManager.LocationChanged += HandleLocationChanged;
        }

        private void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        {
            validation = "";
            if (navigationManager.Uri.IndexOf("magasiner") == -1)
            {
                validationRequis = true;
                FormDataService.recherche = "";
                FormDataService.categorie = null;
            }
            else
                validationRequis = false;

            StateHasChanged();
        }
        List<string> option = new List<string>();
        private async void HandleInputChange(ChangeEventArgs e)
        {

            string input = e.Value.ToString();

            if (!string.IsNullOrWhiteSpace(input))
            {
                try
                {
                    List<ArticleSession> lst = await httpClient.GetFromJsonAsync<List<ArticleSession>>("api/article/article");
                    lst = lst.Where(x => x.nom.IndexOf(input) != -1).ToList();
                    option = lst.Select(art => art.nom).Take(5).ToList();
                    lst = await httpClient.GetFromJsonAsync<List<ArticleSession>>("api/article/article");
                    lst = lst.Where(x => x.description.IndexOf(input) != -1).ToList();
                    option.AddRange(lst.Select(art => art.description).Take(5).ToList());
                }
                catch
                {

                }

            }
            else
                option.Clear();
        }

        private void Redirection()
        {
            if (navigationManager.Uri.IndexOf("magasiner") == -1)
                navigationManager.NavigateTo($"{navigationManager.BaseUri}magasiner?recherche={FormDataService.recherche}");
            else if (navigationManager.Uri.IndexOf("recherche") == -1)
                navigationManager.NavigateTo($"{navigationManager.Uri}?recherche={FormDataService.recherche}");
            else if (string.IsNullOrWhiteSpace(FormDataService.categorie))
                navigationManager.NavigateTo($"{navigationManager.BaseUri}magasiner?recherche={FormDataService.recherche}");
            else
                navigationManager.NavigateTo($"{navigationManager.BaseUri}magasiner/{FormDataService.categorie}?recherche={FormDataService.recherche}");
            FormDataService.boolRecherche = true;;
            validation = "";
        }
        private async Task SubmitForm()
        {
            if (validationRequis)
            {
                if (string.IsNullOrWhiteSpace(FormDataService.recherche))
                    validation = "is-invalid";
                else
                {
                    Redirection();
                }
            }
            else
                Redirection();
        }

        private async Task Logout()
        {
            var custumAuth = (CustumAuthStateProvider)authProvider;
            await custumAuth.UpdateAuthState(null);
            navigationManager.NavigateTo("/", true);
        }
    }
}
