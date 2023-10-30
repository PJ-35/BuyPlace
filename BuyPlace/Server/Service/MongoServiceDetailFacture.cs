using BuyPlace.Server.Authentication;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BuyPlace.Server.Service
{
    public class MongoServiceDetailFacture
    {

        private readonly IMongoCollection<DetailFacture> _detailsCollection;

        public MongoServiceDetailFacture(IMongoClient mongoClient)
        {
            // Remplacez "buyplace" par le nom de votre base de données MongoDB
            var database = mongoClient.GetDatabase("buyplace");
            _detailsCollection = database.GetCollection<DetailFacture>("DetailFactures");
        }

        // Méthode pour ajouter un détail de facture
        public async Task CreateDetailFacture(DetailFacture detailFacture)
        {
            await _detailsCollection.InsertOneAsync(detailFacture);
        }

        // Méthode pour récupérer tous les détails de facture
        public async Task<List<DetailFacture>> GetAllDetailFactures()
        {
            return await _detailsCollection.Find(new BsonDocument()).ToListAsync();
        }

        // Méthode pour récupérer les détails de facture par ID
        public async Task<DetailFacture> GetDetailFactureById(string id)
        {
            var objectId = new ObjectId(id);
            return await _detailsCollection.Find(df => df.Id == objectId).FirstOrDefaultAsync();
        }

        // Méthode pour mettre à jour un détail de facture
        public async Task UpdateDetailFacture(string id, DetailFacture updatedDetailFacture)
        {
            var objectId = new ObjectId(id);
            updatedDetailFacture.Id = objectId;
            await _detailsCollection.ReplaceOneAsync(df => df.Id == objectId, updatedDetailFacture);
        }

        // Méthode pour supprimer un détail de facture par ID
        public async Task DeleteDetailFacture(string id)
        {
            var objectId = new ObjectId(id);
            await _detailsCollection.DeleteOneAsync(df => df.Id == objectId);
        }
    }
}
