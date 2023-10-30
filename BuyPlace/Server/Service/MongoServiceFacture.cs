﻿using BuyPlace.Server.Authentication;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BuyPlace.Server.Service
{
    public class MongoServiceFacture
    {

        private IMongoDatabase _database = null;
        private MongoClient _mongoClient = null;
        private IMongoCollection<Factures> _facturesTable = null;


        /// <summary>
        /// Constructeur
        /// </summary>
        public MongoServiceFacture()
        {
            _mongoClient = new MongoClient("mongodb+srv://BuyPlace:4onRFgKYRV1pt6ow@cluster0.xw7danz.mongodb.net/");
            _database = _mongoClient.GetDatabase("BuyPlace");
            _facturesTable = _database.GetCollection<Factures>("Factures");
        }

        /// <summary>
        /// Retourne une liste d' articles
        /// </summary>
        /// <returns>la liste de facture</returns>
        public List<Factures> GetFactures()
        {
            return _facturesTable.Find(FilterDefinition<Factures>.Empty).ToList();
        }

        /// <summary>
        /// Retourne une liste d' articles correspondant à une catégorie
        /// </summary>
        /// <param name="idCategorie">id de la catégorie</param>
        /// <returns>la liste de factures</returns>
        public List<Factures> GetFActUser(string idUser)
        {
            return _facturesTable.Find(u => u.UserId == idUser).ToList();
        }

        /// <summary>
        /// Ajoute un article dans la bd
        /// </summary>
        /// <param name="article">l'article à ajouter</param>
        /// <returns>un bool représentant l'état si jamais il y'avait déjà un article de même nom</returns>
        public bool AddFacture(Factures facture)
        {

            try
            {
                _facturesTable.InsertOne(facture);
                return true; // L'insertion s'est déroulée avec succès
            }
            catch (Exception)
            {
                // Gérez l'exception ou enregistrez des informations de journalisation ici
                return false; // Une erreur s'est produite lors de l'insertion
            }

        }

        /// <summary>
        /// Obtient un article avec le nom
        /// </summary>
        /// <param name="id">L'id d'un article</param>
        /// <returns>Un article</returns>
        public Factures GetFacture(ObjectId id)
        {
            return _facturesTable.Find(u => u.Id == id).SingleOrDefault();
        }





        /// <summary>
        /// Supprime un article
        /// </summary>
        /// <param name="nom">nom de l'article</param>
        /// <returns>Retourne un état</returns>
        public bool DeleteFacture(ObjectId id)
        {
            Factures fact = GetFacture(id);
            if (fact != null)
            {
                _facturesTable.DeleteOne(u => u.Id == id);
                return true;
            }
            return false;
        }
    }
}
