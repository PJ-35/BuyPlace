using BuyPlace.Server.Authentication;
using BuyPlace.Server.Service;
using BuyPlace.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
