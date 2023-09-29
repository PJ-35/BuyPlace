using BuyPlace.Data;
using Microsoft.AspNetCore.Components.Forms;
using System.Text.RegularExpressions;
using BuyPlace.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.IO;



namespace BuyPlace.Pages
{

    public partial class Inscription
    {

        private User user = new User();
        private string cMdp = "";
        public string LoginMesssage { get; set; }
        UsersService userService=new UsersService();
        private string url;

        private long maxFileSize = 1024 * 1024 * 3; //3MB
        private int maxAllowedFiles = 1;
        private List<string> error = new List<string>();
        private IBrowserFile? file;

       

        [Inject]
        private NavigationManager NavManager { get; set; }

        [Inject]
        private IConfiguration config { get; set; }



        protected override void OnInitialized()
        {
            
            string tt = NavManager.BaseUri;
            string ti = NavManager.Uri;
            url=ti.Replace(tt,"");
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
                string newFileName = Path.ChangeExtension(Path.GetRandomFileName(), Path.GetExtension(file.Name));
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

        private void SignInUser()
        {
            var userobj = user;
            string rgxMdp = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{8,}$";

            // userService.Save(user);

            // NavManager.NavigateTo("/");

            var editContext = new EditContext(user);
            var isValid = editContext.Validate();

            if (url == "inscription")
            {

                if (isValid)
                {
                    if (cMdp == userobj.Mdp)
                    {
                        if (Regex.IsMatch(userobj.Mdp, rgxMdp))
                        {

                            user.IsAdmin = false;
                            user.IsCompleted = false;
                            //userService.Save(user);

                           // NavManager.NavigateTo("/next");
                            url = "next";

                            InvokeAsync(StateHasChanged);


                        }
                        else
                            LoginMesssage = "Votre mot de passe doit contenir au moins 8 caractères, une lettre majiscule, au moins une lettre minuscule,au moins un chiffre .";
                    }
                    else
                    {
                        LoginMesssage = "Veuillez confirmer votre mot de passe.";
                    }

                }
                else 
                {
                    // Les données ne sont pas valides, affichez un message d'erreur
                    LoginMesssage = "Veuillez corriger les erreurs du formulaire.";
                }
            }
            else if(url == "next" )
            {

              
                if (isValid)
                {
                    string relativePath = CaptureFiles().Result;
                    userService.Save(user);
                    NavManager.NavigateTo("/");
                }
                else
                {
                    // Les données ne sont pas valides, affichez un message d'erreur
                    LoginMesssage = "Veuillez corriger les erreurs du formulaire.";
                }
            }




        }
    }
}
