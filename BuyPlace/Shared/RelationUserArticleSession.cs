using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyPlace.Shared
{
    public class RelationUserArticleSession
    {
        public string Id { get; set; }
        public string IdUser { get; set; }
        public string ArticleId { get; set; }

        public decimal PrixUnitaire { get; set; }
        public bool IsBuy { get; set; }
        public int Quantite { get; set; }
    }
}
