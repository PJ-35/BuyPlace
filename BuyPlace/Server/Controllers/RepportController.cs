using BuyPlace.Server.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text;

namespace BuyPlace.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepportController : ControllerBase
    {

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly MongoServiceFacture factureService = new MongoServiceFacture();
        public RepportController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        //[HttpGet]
        //[Route ("GetReport")]
        //public IActionResult GetReport()
        //{
        //    var dt = new DataTable();
        //    dt=factureService.GetFactures()
        //}
    }
}
