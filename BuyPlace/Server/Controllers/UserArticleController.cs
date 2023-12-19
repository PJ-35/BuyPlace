using BuyPlace.Server.Authentication;
using BuyPlace.Server.Service;
using BuyPlace.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.IO;

namespace BuyPlace.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserArticleController : ControllerBase
    {

        private MongoServiceRelationUserArticles _userArticles;
        private UsersService _usersService;
        private MongoServiceArticle _articleService;

        public UserArticleController(MongoServiceRelationUserArticles userArticles, UsersService usersService,MongoServiceArticle articleService)
        {
            _userArticles = userArticles;
            _usersService = usersService;
            _articleService = articleService;
        }

        [HttpGet]
        [Route("articleUserList")]
        public ActionResult<List<RelationUserArticleSession>> GetArticleUser([FromQuery] List<string> lstIdUA)
        {
            List<RelationUserArticle> lstUserArticle = new List<RelationUserArticle>();

            lstUserArticle = _userArticles.GetRelationUserArticle(lstIdUA);

            List<RelationUserArticleSession> lstUserArticleSession = new List<RelationUserArticleSession>();
            foreach (RelationUserArticle UserArt in lstUserArticle)
            {
                RelationUserArticleSession userArtSession = new RelationUserArticleSession
                {
                  Id = UserArt.Id.ToString(),
                  ArticleId = UserArt.ArticleId.ToString(),
                  IdUser = UserArt.UserId.ToString(),
                  PrixUnitaire = UserArt.PrixUnitaire,
                  IsBuy=UserArt.IsBuy,
                  Quantite=UserArt.Quantite,

                };




                lstUserArticleSession.Add(userArtSession);
            }
            return lstUserArticleSession;
        }

        [HttpDelete]
        [Route("deleteArticle")]
        public ActionResult DeleteArticle([FromQuery] string articleId)
        {
            try
            {
                RelationUserArticle relationUserArticle = _userArticles.FindArticle(articleId);
                if (relationUserArticle is null)
                    return BadRequest("");
                if (!_userArticles.RemoveUserArticle(relationUserArticle.Id.ToString()))
                    return BadRequest("");
                return Ok();
            }
            catch
            {
                return BadRequest("");
            }

        }

        /// <summary>
        /// récupère la liste d'article pour le panier
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>ArticleSession</returns>
        [HttpGet]
        [Route("articleUserPanier")]
        public ActionResult<List<ArticleSession>> GetPanier([FromQuery] string userId)
        {

            List<RelationUserArticle> lstUserArticle = _userArticles.GetRelationUserArticleIsNotBuy(userId);
            List<Article> lstArticle = new List<Article>();
            List<ArticleSession> lstSession = new List<ArticleSession>();
            List<string> lstIdArticle = new List<string>();

            
            foreach (RelationUserArticle Article in lstUserArticle)
            {
                lstIdArticle.Add(Article.ArticleId.ToString());
            }

            lstArticle = _articleService.GetArticlesForFacture(lstIdArticle);
            int i = 0;
            foreach (Article article in lstArticle)
            {
                ArticleSession session = new ArticleSession
                {
                    Id=article.Id.ToString(),
                    nom=article.nom.ToString(),
                    quantite = lstUserArticle[i].Quantite,
                    description=article.description,
                    id_user=article.id_user,
                    prix = lstUserArticle[i].PrixUnitaire,
                    id_categorie=article.id_categorie,

                };

                lstSession.Add(session);
                i++;
            }

            return lstSession;
        }


        [HttpPost("{idArticle}")]
        [AllowAnonymous]
        public ActionResult SaveUserArt([FromBody] string idUser, string idArticle)
        {
            string message = "Requête invalide";
            try
            {
                if(_usersService.GetUserById(idUser) is null)
                    return BadRequest(message);
                Article art = _articleService.GetArticleById(idArticle);
                if (art is null)
                    return BadRequest(message);
                RelationUserArticle relationUserArticle = new RelationUserArticle
                {
                    ArticleId = new ObjectId(idArticle),
                    UserId = new ObjectId(idUser),
                    PrixUnitaire=art.prix,
                    IsBuy = false,
                    Quantite = 1
                };
                if (_userArticles.AddUserArticle(relationUserArticle))
                    return Ok();
                else
                    return BadRequest(message);
            }
            catch
            {
                return BadRequest("Une erreur s'est produite. Veuillez réessayer");
            }

        }
    }
}
