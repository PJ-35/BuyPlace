using BuyPlace.Server.Authentication;
using BuyPlace.Server.Service;
using BuyPlace.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BuyPlace.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturesController : ControllerBase
    {

        private MongoServiceFacture _factService;

        private MongoServiceRelationUserArticles _userArtService;
       //private MongoServiceArticle _articleService;

        public FacturesController(MongoServiceFacture factService, MongoServiceRelationUserArticles uaService, MongoServiceArticle article)
        {
            _factService = factService;
            _userArtService = uaService;
            //_articleService = article;
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
        [Route("{montant}")]
        public ActionResult FaireFacture([FromBody] List<string> lstidArticle, double montant)
        {
            try
            {
                if (lstidArticle.Count == 1)
                    return BadRequest();
                Factures facture = new Factures { Date = DateTime.Now, UserId = lstidArticle[0], Montant = montant };
                for (int i = 1; i < lstidArticle.Count; i++)
                    facture.RelationsUserArticles.Add(lstidArticle[i]);
                if (_factService.AddFacture(facture))
                    return Ok();
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
