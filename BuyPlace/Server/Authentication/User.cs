using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BuyPlace.Server.Authentication
{
    public class User
    {
        [BsonRepresentation(BsonType.ObjectId)]
        #region Propriétés et indexeurs

        public ObjectId Id { get; set; }

        public decimal Solde { get; set; } = 5000;

        //[Required(ErrorMessage = "Le nom d'utilisateur est obligatoire.")]
        [MinLength(3, ErrorMessage = "Le nom d'utilisateur doit contenir au moins 3 caractères.")]
        public string UserName { get; set; }



        public string Image { get; set; }



        //public string CMdp { get; set; }

        //[Required(ErrorMessage = "Le mot de passe est obligatoire.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{8,}$", ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères, au moins une majuscule et au moins un chiffre.")]
        public string Mdp { get; set; }


        //[Required(ErrorMessage = "Le courriel est obligatoire.")]
        //[UniqueEmail(ErrorMessage = "Cette adresse e-mail est déjà utilisée.")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Veuillez entrer une adresse de courriel valide.")]
      
        public string Courriel { get; set; }


        //[Required(ErrorMessage = "Le nom d'utilisateur est obligatoire.")]
        [MinLength(3, ErrorMessage = "Le nom doit contenir au moins 3 caractères.")]
        public string Nom { get; set; }


        //[Required(ErrorMessage = "Le nom d'utilisateur est obligatoire.")]
        [MinLength(3, ErrorMessage = "Le prenom d'utilisateur doit contenir au moins 3 caractères.")]
        public string Prenom { get; set; }

        //public bool IsAdmin { get; set; } = false;
        public string Role { get; set; } = "user";
        //public bool IsCompleted { get; set; }




        #endregion

        #region Constructeurs

        public User(string image, string courriel, string name, string prenom,string mdp, string username, decimal solde,string role)
        {
            Image = image;
            Courriel = courriel;
            Nom = name;
            Prenom = prenom;
            Mdp = mdp;
            UserName = username;
            Solde = solde;
            Role=role;
        }

        public User()
        {

        }
        #endregion
    }
}