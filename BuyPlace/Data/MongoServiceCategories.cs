using BuyPlace.Model;
using MongoDB.Bson;
using MongoDB.Driver;


namespace BuyPlace.Data
{
    public class MongoServiceCategories
    {
        private IMongoDatabase _database=null;
        private MongoClient _mongoClient = null;
        private IMongoCollection<Categories> _categorieTable=null;

        public MongoServiceCategories()
        {
            _mongoClient = new MongoClient("mongodb://localhost:27017");
            _database = _mongoClient.GetDatabase("BuyPlace");
            _categorieTable = _database.GetCollection<Categories>("Categories");
        }

        public List<Categories> GetCategories()
        {
            return _categorieTable.Find(FilterDefinition<Categories>.Empty).ToList();
        }

        public void AddCategorie(Categories categories)
        {
            _categorieTable.InsertOne(categories);
        }

        public Categories GetCategorie(string id)
        {
            return _categorieTable.Find(u => u.Id == id).SingleOrDefault();
        }

        public void UpdateUser(Categories categories)
        {
            _categorieTable.ReplaceOne(u => u.Id == categories.Id, categories);
        }

        public void DeleteCategorie(string id)
        {
            _categorieTable.DeleteOne(u => u.Id ==id);
        }
    }
}
