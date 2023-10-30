using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BuyPlace.Server.Authentication
{
    public class DetailFacture
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; } // _id géré par MongoDB
        public ObjectId FactureId { get; set; } // Référence à la facture
        public List<string> IdArticles { get; set; } // Référence aux produits
        public int Quantite { get; set; }
        public decimal PrixUnitaire { get; set; }
        public decimal MontantTotal { get; set; }
    }
}
