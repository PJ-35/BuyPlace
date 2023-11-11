using BuyPlace.Server.Authentication;
using BuyPlace.Server.Service;
using BuyPlace.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuyPlace.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserArticleController : ControllerBase
    {

        private MongoServiceRelationUserArticles _userArticles;

        public UserArticleController(MongoServiceRelationUserArticles userArticles)
        {
            _userArticles = userArticles; 
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
    }
}
