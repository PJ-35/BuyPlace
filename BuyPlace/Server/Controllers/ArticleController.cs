using BuyPlace.Server.Authentication;
using BuyPlace.Server.Service;
using BuyPlace.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BuyPlace.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private MongoServiceArticle _artService;
        private readonly IWebHostEnvironment _environment;

        public ArticleController(MongoServiceArticle usersService, IWebHostEnvironment environment)
        {
            _artService = usersService;
            _environment = environment;
        }

        [HttpPost]
        [Route("uploadimage")]
        public async Task Upload([FromBody] image file)
        {
            var buff = Convert.FromBase64String(file.base64);
            // Chemin vers le dossier wwwroot
            string wwwrootPath = _environment.ContentRootPath;

            await System.IO.File.WriteAllBytesAsync(wwwrootPath+"\\"+"images"+"\\"+"images_articles"+"\\"+file.id+".jpg",buff);
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
                Article article = _artService.GetArticle(idArticle);

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

        [HttpGet("{imageName}")]
        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult GetImage(string imageName)
        {
            // Construit le chemin complet du fichier dans le dossier wwwroot/uploads
            string wwwrootPath = _environment.ContentRootPath;
            string filePath = Path.Combine(wwwrootPath, "images","images_articles", imageName+".jpg");

            // Vérifie si le fichier existe
            if (System.IO.File.Exists(filePath))
            {
                // Retourne le fichier en tant que résultat de l'action
                return PhysicalFile(filePath, "image/jpg");
            }

            // Retourne un statut 404 si le fichier n'est pas trouvé
            return PhysicalFile(Path.Combine(wwwrootPath, "images", "images_articles", "indisponible.jpg"), "image/jpg");
        }


        [HttpPost]
        [Route("savearticle")]
        [AllowAnonymous]
        public ActionResult SaveArticle([FromBody] ArticleSession article2)
        {
            Article article = new Article()
            {
                id_categorie = article2.id_categorie,
                id_user = article2.id_user,
                nom = article2.nom,
                description = article2.description,
                prix = article2.prix,
                quantite = article2.quantite,
                date = article2.date,
            };
            string id=_artService.AddArticle(article);

            if (!string.IsNullOrWhiteSpace(id))
                return Ok(id);
            else
                return BadRequest();

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

        [HttpGet]
        [Route("articleUserFact")]
        public ActionResult<List<ArticleSession>> GetArticleFactureUser([FromQuery] List<string> lstIdUA)
        {
            List<Article> lstArt = new List<Article>();

            lstArt = _artService.GetArticlesForFacture(lstIdUA);

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
