﻿using BuyPlace.Server.Authentication;
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
            return _relationUsArtTable.Find(a => listId.Contains(a.Id.ToString())).ToList();
        }


        public List<RelationUserArticle> GetRelationUserArticleIsNotBuy(string userId)
        {
            return _relationUsArtTable.Find(u => u.UserId.ToString() == userId && u.IsBuy == false).ToList();

        }

        public RelationUserArticle FindArticle(string articleId)
        {
            return _relationUsArtTable.Find(u => u.ArticleId.ToString() == articleId && u.IsBuy == false).SingleOrDefault();

        }

        /// <summary>
        /// Met à jour une article
        /// </summary>
        /// <param name="article">L'article à ajouter</param>
        /// <returns>retourne un état</returns>
        public bool UpdateArticlePanier(string idUser)
        {
            List<RelationUserArticle> lstR = GetRelationUserArticleIsNotBuy(idUser);
            if (lstR != null)
            {
                foreach (RelationUserArticle article in lstR)
                {
                    article.IsBuy = true;
                    _relationUsArtTable.ReplaceOne(u => u.Id == article.Id, article);

                }
                return true;
            }
            return false;
        }


        /// <summary>
        /// Ici je vérifie que l'article n'existe pas encore alors j'ajoute ou j'update la quantité dépendamment de la situation
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public bool AddUserArticle(RelationUserArticle article)
        {
            try
            {
                RelationUserArticle relationUser = _relationUsArtTable.Find(u => u.ArticleId.ToString() == article.ArticleId.ToString() && u.UserId.ToString() == article.UserId.ToString()
                && u.IsBuy == false).SingleOrDefault();
                 if(relationUser is null)
                {
                    _relationUsArtTable.InsertOne(article);
                }
                else
                {
                    relationUser.Quantite++;
                    _relationUsArtTable.ReplaceOne(u => u.Id == relationUser.Id, relationUser);
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool RemoveUserArticle(string id)
        {
            try
            {
                _relationUsArtTable.DeleteOne(u => u.Id.ToString() == id);
                return true;
            }catch {
                return false;
            }
        }

    }
}
