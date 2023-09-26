using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

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

        public double Solde { get; set; }
        public string UserName { get; set; }



        public string Image { get; set; }



        public string CMdp { get; set; }


        public string Mdp { get; set; }



        public string Courriel { get; set; }



        public string Nom { get; set; }



        public string Prenom { get; set; }

        public bool IsAdmin { get; set; }




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
