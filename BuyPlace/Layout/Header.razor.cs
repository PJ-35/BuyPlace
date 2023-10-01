using BuyPlace.Data;
using BuyPlace.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace BuyPlace.Layout
{
    public partial class Header
    {
        bool collapseNavMenu = true;

        string baseMenuClass = "navbar-collapse d-sm-inline-flex flex-sm-row-reverse";
        string validation = "";
        bool validationRequis=true;

        private MongoServiceArticle mongoService = new MongoServiceArticle();
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

            }
            else
                validationRequis = false;

            StateHasChanged();
        }
        List<string> option=new List<string>();
        private void HandleInputChange(ChangeEventArgs e)
        {

            string input = e.Value.ToString();

            if (!string.IsNullOrWhiteSpace(input))
            {
                List<Article> lst = mongoService.GetArticles().Where(x => x.nom.IndexOf(input) != -1).ToList();
                option = lst.Select(art => art.nom).Take(5).ToList();
                lst= mongoService.GetArticles().Where(x => x.description.IndexOf(input) != -1).ToList();
                option.AddRange(lst.Select(art=>art.description).Take(5).ToList());
            }
            else
                option.Clear();
        }

        private void Redirection()
        {
            if (navigationManager.Uri.IndexOf("magasiner") == -1)
                navigationManager.NavigateTo($"{navigationManager.Uri}magasiner?recherche={FormDataService.recherche}");
            else
                navigationManager.NavigateTo($"{navigationManager.Uri}?recherche={FormDataService.recherche}");
            FormDataService.boolRecherche = true;
            validation = "";
        }
        private async Task SubmitForm()
        {
            if(validationRequis)
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
    }
}
