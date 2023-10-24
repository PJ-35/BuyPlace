using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using BuyPlace.Client.Authentication;
using BuyPlace.Shared;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net;

namespace BuyPlace.Client.Pages
{
    public partial class Inscription
    {
        private NewUser user = new NewUser();
        private string cMdp = "";
        public string LoginMesssage { get; set; } = "";
        private string url;

        private long maxFileSize = 1024 * 1024 * 3; //3MB
        private const int maxAllowedFiles = 1;
        private List<string> error = new List<string>();
        private IBrowserFile? file;



        [Inject]
        HttpClient httpClient { get; set; }

        [Inject]
        private NavigationManager NavManager { get; set; }

        [Inject]
        private IConfiguration config { get; set; }



        protected override void OnInitialized()
        {
            
            string tt = NavManager.BaseUri;
            string ti = NavManager.Uri;
            url = ti.Replace(tt, "");

            if(user.UserName == null && url=="next") {

                NavManager.NavigateTo("inscription");
            }
            InvokeAsync(StateHasChanged);

        }

        private void LoadFiles(InputFileChangeEventArgs e)
        {
            error.Clear();
            if (e.FileCount > maxAllowedFiles)
            {
                error.Add($"tentative de téléchargement de {e.FileCount} fichiers, mais seulement {maxAllowedFiles} fichiers est autorisé");
                return;
            }
            file = e.File;
        }

        private async Task<string> CaptureFiles()
        {


            if (file == null)
            {
                return "";
            }
            try
            {
                string newFileName = Path.ChangeExtension(Path.GetRandomFileName(), ".jpg");
                string path = Path.Combine(config.GetValue<string>("FileStorage")!, newFileName);
                string relativePath = Path.Combine(newFileName);
                //Directory.CreateDirectory(Path.Combine(config.GetValue<string>("FileStorage")!, $"{user.UserName}"));


                await using FileStream fs = new(path, FileMode.Create);
                await file.OpenReadStream(maxFileSize).CopyToAsync(fs);
                user.Image = newFileName;

                return path;
            }
            catch (Exception ex)
            {

                error.Add($"file: {file.Name} Error: {ex.Message}");
                throw;
            }

        }

        private async Task SignInUser()
        {
            var userobj = user;
            string rgxMdp = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{8,}$";

            // userService.Save(user);

            // NavManager.NavigateTo("/");

            var editContext = new EditContext(user);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(user, new ValidationContext(user), validationResults, true);


            if (url == "inscription")
            {
                if (isValid)
                {
                    if (string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(user.Courriel) || string.IsNullOrWhiteSpace(user.Mdp))
                    {
                        LoginMesssage = "Veuillez remplir tous les champs";
                    }

                }
                else
                {
                    if (!(string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(user.Courriel) || string.IsNullOrWhiteSpace(user.Mdp)))
                    {
                        LoginMesssage = "";
                        //InvokeAsync(StateHasChanged);
                    }
                    else
                        LoginMesssage = "Veuillez remplir tous les champs";
                }
                if (LoginMesssage == "" && cMdp != user.Mdp)
                {
                    LoginMesssage = "Veuillez mettre des mots de passe identique";
                    user.Mdp = cMdp = "";
                }
                else if (isValid && cMdp == user.Mdp)
                {
                    LoginMesssage = "";
                }
                var loginResponseUserName = await httpClient.PostAsJsonAsync("api/Account/DoublonbyUserName", user.Nom);
                var loginResponseCourriel = await httpClient.PostAsJsonAsync("api/Account/DoublonbyCourriel", user.Courriel);
                if (loginResponseUserName.StatusCode == HttpStatusCode.Unauthorized|| loginResponseCourriel.StatusCode == HttpStatusCode.Unauthorized && isValid && LoginMesssage == "")
                {

                    if (loginResponseUserName.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoginMesssage = "Ce nom d'utilisateur est déjà attribué à un autre compte";
                    }

                    if (loginResponseCourriel.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoginMesssage = "Ce courriel appartient déjà à un autre utilisateur";
                    }

                    if (loginResponseCourriel.StatusCode == HttpStatusCode.Unauthorized && loginResponseUserName.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        LoginMesssage = "le nom d'utilisateur et le courriel sont déjà attribué à un autre compte. Veuillez choisir un nouveau";
                    }
                }


                if (isValid && LoginMesssage == "")
                {
                    url = "next";

                    //InvokeAsync(StateHasChanged);
                }


            }
            else if (url == "next")
            {
                
                
                LoginMesssage = "";
                if (isValid)
                {
                    if (string.IsNullOrWhiteSpace(user.Nom) || string.IsNullOrWhiteSpace(user.Prenom))
                    {
                        LoginMesssage = "Veuillez remplir tous les champs";
                    }

                }
                else
                {
                    if (!(string.IsNullOrWhiteSpace(user.Nom) || string.IsNullOrWhiteSpace(user.Prenom)))
                    {
                        LoginMesssage = "";
                    }
                    else
                        LoginMesssage = "Veuillez remplir tous les champs";
                }

                if (LoginMesssage == "" && error.Count < 1)
                {
                    //string relativePath = await CaptureFiles();
                    var loginResponse = await httpClient.PostAsJsonAsync("api/Account/Save", user);
                    if (loginResponse.IsSuccessStatusCode)
                    {
                        NavManager.NavigateTo("connexion", true);
                    }
                    else
                    {
                        LoginMesssage = "Désolé une erreur s'est produite veuillez réessayer";
                        return;
                    }
                    //userService.Save(user);

                }
                

                


            }




        }
    }
}
