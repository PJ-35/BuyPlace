using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components;
using BuyPlace.Client.Authentication;
using BuyPlace.Shared;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.SessionStorage;
//using BuyPlace.Shared;

namespace BuyPlace.Client.Layout
{
    public partial class Header
    {
        bool collapseNavMenu = true;

        string baseMenuClass = "navbar-collapse d-sm-inline-flex flex-sm-row-reverse";
        string validation = "";
        bool validationRequis = true;
        private UserSession storedUser ;

        public NewUser userSession = new NewUser();
        //private readonly ISessionStorageService sessionStorageService;

        // DropDown Profile
        public string show {  get; set; }
        public string showPanier {  get; set; }
        public bool DropDownValue=false;
        public bool DropDownValuePanier=false;
        private bool collapswNavMenu = true;
        // private string NavMenuCss1Class => collapseNavMenu ? "collapse" : null;




        #region Inject
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

        [Inject]
        ISessionStorageService sessionStorageService { get; set; }
        #endregion

        #region Menu contextuele

        public void MouseOverDropDown()
        {
            show = "show";
            DropDownValue = true;
        }

        public void MoiseOutDropDown()
        {
            show = string.Empty;
            DropDownValue = false;
        }

        public void MouseOverDropDownPanier()
        {
            showPanier = "show";
            DropDownValuePanier = true;
        }

        public void MoiseOutDropDownPanier()
        {
            showPanier = string.Empty;
            DropDownValuePanier = false;
        }

        void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }

        #endregion

        protected override void OnInitialized()
        {
            navigationManager.LocationChanged += HandleLocationChanged;

            
           
        }

        protected override async Task OnInitializedAsync()
        {
            var custum = (CustumAuthStateProvider)authProvider;

            storedUser = await custum.GetUserSession();

            if (storedUser is not null)
            {

                var user = await httpClient.PostAsJsonAsync("api/Account/GetbyUserName", storedUser.UserName);
                userSession = await user.Content.ReadFromJsonAsync<NewUser>();
               

                Console.WriteLine(userSession.UserName);
            }
            InvokeAsync(StateHasChanged);
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
                    List<ArticleSession>lst2 = lst.Where(x => x.nom.ToLower().IndexOf(input.ToLower()) != -1).ToList();
                    option = lst2.Select(art => art.nom).Take(3).ToList();
                    //lst = await httpClient.GetFromJsonAsync<List<ArticleSession>>("api/article/article");
                    lst2 = lst.Where(x => x.description.ToLower().IndexOf(input.ToLower()) != -1).ToList();
                    option.AddRange(lst2.Select(art => art.description).Take(3).ToList());
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
