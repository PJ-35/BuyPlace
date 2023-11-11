using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BuyPlace.Server.Authentication
{
    public class RelationUserArticle
    {

        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public ObjectId UserId { get; set; }
        public ObjectId ArticleId { get; set; }

        public decimal PrixUnitaire {  get; set; }
        public bool IsBuy {  get; set; }
        public int Quantite {  get; set; }
      
    }
}
