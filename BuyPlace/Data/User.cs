using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BuyPlace.Data
{
    public class User
    {
        [BsonRepresentation(BsonType.ObjectId)] 
       

        #region Attributs

        private string _firstName;
        private string _name;
        private string _courriel;
        private string _mdp;
        private string _cMdp;
        private string _mage;
        private int _solde;




        #endregion

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

        public bool IsAdmin { get; set; } = false;

        //public bool IsCompleted { get; set; }




        #endregion

        #region Constructeurs

        public User(string image, string courriel, string name, string firstName)
        {
            Image = image;
            Courriel = courriel;
            Nom = name;
            Prenom = firstName;
        }

        public User()
        {

        }
        #endregion

        #region Méthodes

        #endregion
    }
}
