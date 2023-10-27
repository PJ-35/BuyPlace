using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BuyPlace.Shared
{
    public class ArticleSession
    {
        public string Id { get; set; }


        [MinLength(3, ErrorMessage = "Le nom de l'article doit contenir au moins 3 caractères.")]
        public string nom { get; set; }


        public int quantite { get; set; }
        public string id_categorie { get; set; }
        public DateTime date { get; set; }
        public decimal prix { get; set; }


        [MinLength(3, ErrorMessage = "La description doit contenir au moins 3 caractères.")]
         public string description { get; set; }

        public string id_user { get; set; }
    }
}
