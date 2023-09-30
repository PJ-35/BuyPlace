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

        private void Redirection()
        {
            navigationManager.NavigateTo("/magasiner");
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
