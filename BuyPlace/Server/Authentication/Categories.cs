using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BuyPlace.Server.Authentication
{
    public class Categories
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string nom { get; set; }
    }
}
