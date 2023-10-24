using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BuyPlace.Server.Authentication
{
    public class Article
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string nom { get; set; }
        public int quantite { get; set; }
        public string id_categorie { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime date { get; set; }
        public decimal prix { get; set; }
        public string description { get; set; }

        public string id_user { get; set; }
    }
}
