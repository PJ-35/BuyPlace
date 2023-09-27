using BuyPlace.Data;
using Microsoft.AspNetCore.Components.Forms;
using System.Text.RegularExpressions;
using BuyPlace.Service;
using Microsoft.AspNetCore.Components;

namespace BuyPlace.Pages
{
    public partial class Inscription
    {

        private User user = new User();
        private string cMdp = "";
        public string LoginMesssage { get; set; }
        UsersService userService=new UsersService();
        private string url;

        [Inject]
        private NavigationManager NavManager { get; set; }
        


        protected override void OnInitialized()
        {
            
            string tt = NavManager.BaseUri;
            string ti = NavManager.Uri;
            url=ti.Replace(tt,"");
                
        }

        private void RegisterUser()
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


                            NavManager.NavigateTo("/next");

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
            else
            {
                if (isValid)
                {
                

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
