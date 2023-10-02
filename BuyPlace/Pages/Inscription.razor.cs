﻿using BuyPlace.Data;
using Microsoft.AspNetCore.Components.Forms;
using System.Text.RegularExpressions;
using BuyPlace.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace BuyPlace.Pages
{

    public partial class Inscription
    {

        private User user = new User();
        private string cMdp = "";
        public string LoginMesssage { get; set; } = "";
        UsersService userService=new UsersService();
        private string url;

        private long maxFileSize = 1024 * 1024 * 3; //3MB
        private const int maxAllowedFiles = 1;
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
                    if (string.IsNullOrWhiteSpace(user.UserName)|| string.IsNullOrWhiteSpace(user.Courriel) || string.IsNullOrWhiteSpace(user.Mdp))
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
                if (LoginMesssage == "" && cMdp!=user.Mdp)
                {
                    LoginMesssage = "Veuillez mettre des mots de passe identique";
                    user.Mdp = cMdp = "";
                }
                else if (isValid && cMdp == user.Mdp)
                {
                    LoginMesssage = "";
                }

                if (userService.GetUserByCourriel(user.Courriel)!=null|| userService.GetUserByUsername(user.UserName) != null && isValid && LoginMesssage=="")
                {

                    if (userService.GetUserByCourriel(user.Courriel) != null)
                    {
                        LoginMesssage = "Ce courriel appartient déjà à un autre utilisateur";
                    }

                    if (userService.GetUserByUsername(user.UserName) != null)
                    {
                        LoginMesssage = "Ce nom d'utilisateur est déjà attribué à un autre compte";
                    }

                    if (userService.GetUserByCourriel(user.Courriel) != null && userService.GetUserByUsername(user.UserName)!=null)
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
            else if(url == "next" )
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

                if (LoginMesssage=="" && error.Count < 1)
                {
                    string relativePath = await CaptureFiles();
                    userService.Save(user);
                    NavManager.NavigateTo("/");
                }


            }




        }
    }
}
