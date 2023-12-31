﻿using BuyPlace.Server.Authentication;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;

namespace BuyPlace.Server.Service
{
    public class MongoServiceArticle
    {
        private IMongoDatabase _database = null;
        private MongoClient _mongoClient = null;
        private IMongoCollection<Article> _articlesTable = null;


        /// <summary>
        /// Constructeur
        /// </summary>
        public MongoServiceArticle()
        {
            _mongoClient = new MongoClient("mongodb+srv://BuyPlace:4onRFgKYRV1pt6ow@cluster0.xw7danz.mongodb.net/");
            _database = _mongoClient.GetDatabase("BuyPlace");
            _articlesTable = _database.GetCollection<Article>("Articles");
        }

        /// <summary>
        /// Retourne une liste d' articles
        /// </summary>
        /// <returns>la liste d'articles</returns>
        public List<Article> GetArticles()
        {
            return _articlesTable.Find(FilterDefinition<Article>.Empty).ToList();
        }

        /// <summary>
        /// Retourne une liste d' articles correspondant à une catégorie
        /// </summary>
        /// <param name="idCategorie">id de la catégorie</param>
        /// <returns>la liste d'articles</returns>
        public List<Article> GetArticles(string idCategorie)
        {
            return _articlesTable.Find(u => u.id_categorie == idCategorie).ToList();
        }

        /// <summary>
        /// Ajoute un article dans la bd
        /// </summary>
        /// <param name="article">l'article à ajouter</param>
        /// <returns>un bool représentant l'état si jamais il y'avait déjà un article de même nom</returns>
        public string AddArticle(Article article)
        {
            try
            {
                _articlesTable.InsertOne(article);
                return article.Id.ToString();
            }
            catch
            {
                return "";
            }
           
        }

        /// <summary>
        /// Obtient un article avec l'id
        /// </summary>
        /// <param name="id">L'id d'un article</param>
        /// <returns>Un article</returns>
        public Article GetArticleById(string id)
        {
            return _articlesTable.Find(u => u.Id.ToString() == id).SingleOrDefault();
        }

        /// <summary>
        /// Obtient un article avec le id de l'utilisateur
        /// </summary>
        /// <param name="idUser">L'id d'un article</param>
        /// <returns>Une liste article</returns>
        public List<Article> GetArticlesByUserId(string idUser)
        {
            return _articlesTable.Find(u => u.id_user == idUser).ToList();
        }


        /// <summary>
        /// Met à jour une article
        /// </summary>
        /// <param name="article">L'article à ajouter</param>
        /// <returns>retourne un état</returns>
        public bool UpdateArticle(Article article)
        {
            Article art = GetArticleById(article.Id.ToString());
            if (art != null)
            {
                _articlesTable.ReplaceOne(u => u.Id == art.Id, article);
                return true;
            }
            return false;
        }



        /// <summary>
        /// Supprime un article
        /// </summary>
        /// <param name="nom">nom de l'article</param>
        /// <returns>Retourne un état</returns>
        public bool DeleteArticle(ObjectId id)
        {
            Article art = GetArticleById(id.ToString());
            if (art != null)
            {
                _articlesTable.DeleteOne(u => u.Id == id);
                return true;
            }
            return false;
        }


        public List<Article> GetArticlesForFacture(List<string> idArticles)
        {
            List<Article> articles = _articlesTable.Find(a => idArticles.Contains(a.Id.ToString())).ToList();
            return articles;
        }
    }
}
