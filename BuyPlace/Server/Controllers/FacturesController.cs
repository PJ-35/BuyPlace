using BuyPlace.Server.Authentication;
using BuyPlace.Server.Service;
using BuyPlace.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AspNetCore.ReportingServices.ReportProcessing.ReportObjectModel;

namespace BuyPlace.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturesController : ControllerBase
    {

        private MongoServiceFacture _factService;

        private MongoServiceRelationUserArticles _userArtService;

        public FacturesController(MongoServiceFacture factService, MongoServiceRelationUserArticles uaService, MongoServiceArticle article)
        {
            _factService = factService;
            _userArtService = uaService;
        }


        [HttpGet]
        [Route("factureUser")]
        public ActionResult<List<FactureSession>> GetFacture([FromQuery] string idUser)
        {
            List<Factures> lstfact = new List<Factures>();

            lstfact = _factService.GetFActUser(idUser);
           
            List<FactureSession> lstFactSession = new List<FactureSession>();
            foreach (Factures fact in lstfact)
            {
                FactureSession factureSession=new FactureSession
                {
                    date = fact.Date,
                    id_user = fact.UserId,
                    Montant = fact.Montant,
                    Id=fact.Id.ToString(),
                    RelationsUserArticles = fact.RelationsUserArticles,
                    
                };
                lstFactSession.Add(factureSession);
            }



            return lstFactSession;
        }

        [HttpPost]
        [Route("{idUser}")]
        public ActionResult FaireFacture([FromBody] double montant,string idUser)
        {
            try
            {
                List<RelationUserArticle> lstUserArticle = _userArtService.GetRelationUserArticleIsNotBuy(idUser);
                Factures facture = new Factures { Date = DateTime.Now, UserId = idUser, Montant = montant };
                foreach (RelationUserArticle item in lstUserArticle)
                {
                    facture.RelationsUserArticles.Add(item.Id.ToString());
                }
                if (lstUserArticle.Count <= 0)
                    return BadRequest();
                if (_factService.AddFacture(facture) && _userArtService.UpdateArticlePanier(idUser))
                {
                    return Ok();
                }
                else
                    return BadRequest();
            }
            catch 
            {

                return BadRequest();
            }

        }


        [HttpGet]
        [Route("UserArticle")]
        public ActionResult<List<RelationUserArticleSession>> GetRelationUA([FromQuery] string idFacture)
        {
            List<string> lstRelation=_factService.GetRelation(idFacture);
            //ArticlePanier panier = new ArticlePanier();
            if (lstRelation is null)
                return BadRequest();
            

            List<RelationUserArticle> lstUA = _userArtService.GetRelationUserArticle(lstRelation);

            if (lstUA is null)
            {
                return BadRequest("Cette facture n'existe pas");
            }
            List<RelationUserArticleSession> lstFactSession = new List<RelationUserArticleSession>();


            foreach (RelationUserArticle session in lstUA)
            {
                lstFactSession.Add(new RelationUserArticleSession
                {
                    Id = session.Id.ToString(),
                    Quantite = session.Quantite,
                    IdUser = session.UserId.ToString(),
                    ArticleId = session.ArticleId.ToString(),
                    IsBuy = session.IsBuy,
                    PrixUnitaire = session.PrixUnitaire,
                });


                //Article art = _articleService.GetArticle(session.ArticleId.ToString());

                //panier.Article = new 
            }


            return lstFactSession;
        }


    }
}
