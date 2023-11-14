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
        [Route("UpdateMdp")]
        public ActionResult UpdateMdp([FromBody] LoginRequest loginRequest)
        {
            User existingUser = _userService.GetUserByUsername(loginRequest.UserName);
            if(existingUser is null)
                return BadRequest("Échec de la mise à jour.");
            existingUser.Mdp=_userService.HashPassword(loginRequest.Password);

            if (_userService.Update(existingUser))
            {
                return Ok("Mise à jour réussie.");
            }
            else
            {
                return BadRequest("Échec de la mise à jour.");
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
                NewUser newUser = new NewUser() {Mdp="",Id=user.Id.ToString(), UserName = user.UserName, Nom = user.Nom, Courriel = user.Courriel, Role = user.Role, Image = user.Image, Solde = user.Solde, Prenom = user.Prenom };

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
                return Ok("123456");
            else
                return BadRequest("pfeef");

        }

        [HttpPost]
        [Route("UpdateUser")]
        
        public ActionResult UpdateUser([FromBody] NewUser updatedUser)
        {
            // Assurez-vous que l'utilisateur actuellement authentifié correspond à l'utilisateur que vous souhaitez mettre à jour.
            // Vous pouvez utiliser User.Identity.Name pour obtenir le nom d'utilisateur de l'utilisateur actuellement authentifié.
            //if (User.Identity.Name != updatedUser.UserName)
            //{
            //    return Unauthorized("Vous n'êtes pas autorisé à mettre à jour cet utilisateur.");
            //}

            //User existingUser = _userService.GetUserByUsername(updatedUser.UserName);

            //if (existingUser is not null)
            //{
            //    return BadRequest("Ce nom d'utilisateur est déjà attribué.");
            //}

            //User existingUser1 = _userService.GetUserByCourriel(updatedUser.Courriel);
            //if (existingUser1 is not null)
            //{
            //    return BadRequest("Ce courriel est déjà attribué.");
            //}

            User existingUser = _userService.GetUserById(updatedUser.Id);
            //existingUser = new User(updatedUser.Image,updatedUser.Courriel,updatedUser.Nom,updatedUser.Prenom,updatedUser.Mdp,updatedUser.UserName,updatedUser.Solde,updatedUser.Role);

            // Mettez à jour les informations de l'utilisateur avec les nouvelles données.
            existingUser.Courriel = updatedUser.Courriel;
            existingUser.Nom = updatedUser.Nom;
            existingUser.Prenom = updatedUser.Prenom;
            existingUser.Image = updatedUser.Image;

            // Enregistrez les modifications dans votre service ou base de données.
            if (_userService.Update(existingUser))
            {
                return Ok("Mise à jour réussie.");
            }
            else
            {
                return BadRequest("Échec de la mise à jour.");
            }
        }

    }
}
