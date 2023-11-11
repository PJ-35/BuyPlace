using BuyPlace.Server.Authentication;
using MongoDB.Driver;

namespace BuyPlace.Server.Service
{
    public class MongoServiceRelationUserArticles
    {

        private IMongoDatabase _database = null;
        private MongoClient _mongoClient = null;
        private IMongoCollection<RelationUserArticle> _relationUsArtTable = null;


        /// <summary>
        /// Constructeur
        /// </summary>
        public MongoServiceRelationUserArticles()
        {
            _mongoClient = new MongoClient("mongodb+srv://BuyPlace:4onRFgKYRV1pt6ow@cluster0.xw7danz.mongodb.net/");
            _database = _mongoClient.GetDatabase("BuyPlace");
            _relationUsArtTable = _database.GetCollection<RelationUserArticle>("RelationUserArticle");
        }

        public List<RelationUserArticle> GetRelationUserArticle(List<string> listId)
        {

            //var filter = Builders<RelationUserArticle>.Filter.And(
            //       Builders<RelationUserArticle>.Filter.In(x => x.Id.ToString(), listId),
            //       Builders<RelationUserArticle>.Filter.Eq(x => x.IsBuy, false)
            //   );


            //List<Article> articles = 

            
            return _relationUsArtTable.Find(a => listId.Contains(a.Id.ToString())).ToList();
        }

    }
}
