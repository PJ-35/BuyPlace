using BuyPlace.Server.Authentication;
using BuyPlace.Server.Service;
using BuyPlace.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace BuyPlace.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private MongoServiceArticle _artService;

        public ArticleController(MongoServiceArticle usersService)
        {
            _artService = usersService;
        }

        [HttpGet]
        [Route("article")]
        public ActionResult<List<ArticleSession>> GetCategorie([FromQuery] string categorie="")
        {
            List<Article> lstArt=new List<Article>();
            if (string.IsNullOrWhiteSpace(categorie))
                lstArt = _artService.GetArticles();
            else
                lstArt = _artService.GetArticles(categorie);
            List<ArticleSession>lstArtSession= new List<ArticleSession>();
            foreach (Article art in lstArt)
            {
                lstArtSession.Add(new ArticleSession() { date = art.date, Id = art.Id.ToString(), 
                nom=art.nom,quantite=art.quantite,id_categorie=art.id_categorie,prix=art.prix,description=art.description,
                id_user=art.id_user});
            }
            return lstArtSession;
        }

        [HttpGet]
        [Route("GetArticleById")]
        [AllowAnonymous]
        public ActionResult<ArticleSession> GetArticleById([FromQuery] string idArticle)
        {
            try
            {
                ObjectId idArticle2 = new ObjectId(idArticle);
                Article article = _artService.GetArticle(idArticle2);

                if (article is not null)
                {
                    return new ArticleSession()
                    {
                        Id = article.Id.ToString(),
                        nom = article.nom,
                        quantite = article.quantite,
                        date = article.date,
                        id_categorie = article.id_categorie,
                        prix = article.prix,
                        description = article.description,
                        id_user = article.id_user
                    };
                }
                return BadRequest("Aucun article trouvé");
            }
            catch
            {
                return BadRequest("Aucun article trouvé");
            }

        }

        [HttpGet]
        [Route("articleUser")]
        public ActionResult<List<ArticleSession>> GetArticleByIdUser([FromQuery] string idUser)
        {
            List<Article> lstArt = new List<Article>();

            lstArt = _artService.GetArticlesByUserId(idUser);

            List<ArticleSession> lstArtSession = new List<ArticleSession>();
            foreach (Article art in lstArt)
            {
                lstArtSession.Add(new ArticleSession()
                {
                    date = art.date,
                    Id = art.Id.ToString(),
                    nom = art.nom,
                    quantite = art.quantite,
                    id_categorie = art.id_categorie,
                    prix = art.prix,
                    description = art.description,
                    id_user = art.id_user
                });
            }
            return lstArtSession;
        }
    }
}
