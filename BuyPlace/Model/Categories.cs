using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BuyPlace.Model
{
    public class Categories
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }=MongoDB.Bson.ObjectId.GenerateNewId().ToString();
        public string nom { get; set; } = "";
    }
}
