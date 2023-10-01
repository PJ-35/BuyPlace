using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using BuyPlace.IService;
using BuyPlace.Service;

namespace BuyPlace.Data
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class UniqueEmailAttribute : ValidationAttribute

    {

        UsersService userService = new UsersService();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var courriel = value.ToString();

                // Obtenez la référence à votre service utilisateur
                //var userService = (IUsersService)validationContext.GetService(typeof(IUsersService));

                // Utilisez la méthode GetByCourriel pour vérifier si l'e-mail est unique
                var existingUser =  userService.GetUserByCourriel(courriel);

                if (existingUser != null)
                {
                    return new ValidationResult("Cette adresse e-mail est déjà utilisée.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
