using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BuyPlace.Shared
{
    public class FactureSession
    {
        public string Id { get; set; }
        
        public string id_user { get; set; }
        public double Montant { get; set; }
        public DateTime date { get; set; }
        public List<string>? RelationsUserArticles { get; set; }


      
    }
}
