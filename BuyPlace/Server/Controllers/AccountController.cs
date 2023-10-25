using BuyPlace.Server.Authentication;
using BuyPlace.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace BuyPlace.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private UsersService _userService;

        public AccountController(UsersService usersService)
        {
            _userService = usersService;
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public ActionResult<UserSession> Login([FromBody] LoginRequest loginRequest)
        {
            var JwtAuthManager = new JwtAuthManager(_userService);
            var userSession = JwtAuthManager.GenerateJwtToken(loginRequest.UserName, loginRequest.Password);

            if (userSession is null)
            {
                return Unauthorized();
            }
            else
            {
                return userSession;
            }
        }

        [HttpPost]
        [Route("GetbyUserName")]
        [AllowAnonymous]
        public ActionResult<NewUser> GetByUserName([FromBody] string userName)
        {
            User user=_userService.GetUserByUsername(userName);


            if (user is not null)
            {
                NewUser newUser = new NewUser() { UserName = user.UserName, Nom = user.Nom, Courriel = user.Courriel, Role = user.Role, Image = user.Image, Solde = user.Solde, Prenom = user.Prenom };

                return newUser;
            }
            else
            {
                return Unauthorized();
            }
        }


        [HttpPost]
        [Route("DoublonbyUserName")]
        [AllowAnonymous]
        public ActionResult<User> DoublonByUserName([FromBody] string userName)
        {
            User user = _userService.GetUserByUsername(userName);

            if (user is  null)
            {
                return user;
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("GetbyCourriel")]
        [AllowAnonymous]
        public ActionResult<User> GetByCourriel([FromBody] string courriel)
        {
            User user = _userService.GetUserByCourriel(courriel);

            if (user is not null)
            {
                return user;

            }
            else
            {
                return Unauthorized();

            }
        }

        [HttpPost]
        [Route("DoublonbyCourriel")]
        [AllowAnonymous]
        public ActionResult<User> DoublonByCourriel([FromBody] string courriel)
        {
            User user = _userService.GetUserByCourriel(courriel);

            if (user is null)
            {
                return user;

            }
            else
            {
                return Unauthorized();

            }
        }

        [HttpPost]
        [Route("Save")]
        [AllowAnonymous]
        public ActionResult Save([FromBody] NewUser user1)
        {
            User user = new User(user1.Image,user1.Courriel,user1.Nom,user1.Prenom,user1.Mdp,user1.UserName,user1.Solde,user1.Role);

            if (_userService.Save(user))
                return Ok();
            else
                return BadRequest();

        }

    }
}
