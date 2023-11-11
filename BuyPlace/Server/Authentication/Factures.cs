using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BuyPlace.Server.Authentication
{
    public class Factures
    {

        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

       // [BsonElement("id_user")]
        public string UserId { get; set; }

        public double Montant {  get; set; }


        
        public List<string> RelationsUserArticles {  get; set; }



        [BsonRepresentation(BsonType.DateTime)]
        public DateTime Date { get; set; }

    }
}
