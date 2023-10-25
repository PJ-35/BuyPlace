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
    public class CategorieController : ControllerBase
    {
        private MongoServiceCategories _catService;

        public CategorieController(MongoServiceCategories usersService)
        {
            _catService = usersService;
        }

        [HttpGet]
        [Route("categorie")]
        //[AllowAnonymous]
        public ActionResult<List<CategorieSession>> GetCategorie()
        {
            List<Categories> lstCategorie= _catService.GetCategories();
            List<CategorieSession> lst = new List<CategorieSession>();


            foreach (Categories categories in lstCategorie)
            {
                lst.Add(new CategorieSession() { Id = categories.Id.ToString(),nom=categories.nom }) ;
            }
            return lst;
        }

        [HttpGet]
        [Route("cherche")]
        [AllowAnonymous]
        public ActionResult<CategorieSession> GetCategorie([FromQuery] string categorie)
        {
            Categories lstCategorie = _catService.GetCategorie(categorie);
            CategorieSession categorieSession;
            if (lstCategorie is not null)
                categorieSession = new CategorieSession() { Id = lstCategorie.Id.ToString(), nom = lstCategorie.nom };
            else
                return null;
            return categorieSession;
        }

    }
}
