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

        public FacturesController(MongoServiceFacture factService)
        {
            _factService = factService;
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
                    Id=fact.Id.ToString()
                    
                };
                // je charge ma liste d'idArticle
                //foreach (string Id in fact.IdArticles)
                //{
                //    factureSession.IdArticles.Add(Id);
                //}
                lstFactSession.Add(factureSession);
            }
            return lstFactSession;
        }
    }
}
