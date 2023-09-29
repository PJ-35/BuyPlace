namespace BuyPlace.Layout
{
    public partial class Header
    {
        bool collapseNavMenu = true;

        string baseMenuClass = "navbar-collapse d-sm-inline-flex flex-sm-row-reverse";

        string NavMenuCssClass => baseMenuClass + (collapseNavMenu ? " collapse" : "");

        void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }


        //private class FormData
        //{
        //    public string recherche { get; set; }
        //}

        //private FormData formData = new FormData();
        //private async Task SubmitForm()
        //{
        //    formDataService.recherche=formData.recherche;
        //}
    }
}
