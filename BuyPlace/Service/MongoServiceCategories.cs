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


        /// <summary>
        /// Constructeur
        /// </summary>
        public MongoServiceCategories()
        {
            _mongoClient = new MongoClient("mongodb+srv://BuyPlace:4onRFgKYRV1pt6ow@cluster0.xw7danz.mongodb.net/");
            _database = _mongoClient.GetDatabase("BuyPlace");
            _categorieTable = _database.GetCollection<Categories>("Categories");
        }

        /// <summary>
        /// Retourne une liste de catégories
        /// </summary>
        /// <returns>la liste de catégories</returns>
        public List<Categories> GetCategories()
        {
            return _categorieTable.Find(FilterDefinition<Categories>.Empty).ToList();
        }

        /// <summary>
        /// Ajoute une catégorie dans la bd
        /// </summary>
        /// <param name="categories">la catégorie a ajouoté</param>
        /// <returns>un bool représentant l'état si jamais il y'avait déjà une catégorie de même nom</returns>
        public bool AddCategorie(Categories categories)
        {
            var obj=_categorieTable.Find(u => u.nom == categories.nom).FirstOrDefault();
            if (obj is null)
            {
                _categorieTable.InsertOne(categories);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Obtient une catégorie avec le nom
        /// </summary>
        /// <param name="idcategorie">Le nom d'une catégorie</param>
        /// <returns>Une catégorie</returns>
        public Categories GetCategorie(string idcategorie)
        {
            return _categorieTable.Find(u => u.Id.ToString() == idcategorie).SingleOrDefault();
        }

        
        /// <summary>
        /// Met à jour une catégorie
        /// </summary>
        /// <param name="anciennom">Son ancien nom</param>
        /// <param name="nouveaunom">Son nouveau nom</param>
        /// <returns>retourne un état</returns>
        public bool UpdateCategorie(string anciennom,string nouveaunom)
        {
            Categories cat=GetCategorie(anciennom);
            if(cat != null)
            {
                Categories newcategorie= new Categories();
                newcategorie.nom = nouveaunom;
                newcategorie.Id = cat.Id;
                _categorieTable.ReplaceOne(u => u.Id == cat.Id, newcategorie);
                return true;
            }
            return false;
        }



        /// <summary>
        /// Supprime une catégorie si il en reste plus de un
        /// </summary>
        /// <param name="nom">nom de la catégorie</param>
        /// <returns>Retourne un état</returns>
        public bool DeleteCategorie(string nom)
        {
            Categories cat =GetCategorie(nom);
            if (cat != null)
            {
                List<Categories> lstCategorie = GetCategories();
                if (lstCategorie.Count != 1)
                {
                    _categorieTable.DeleteOne(u => u.nom == nom);
                    return true;
                }
                    
            }
            return false;
        }
    }
}
