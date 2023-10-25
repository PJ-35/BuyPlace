using BuyPlace.Shared;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BuyPlace.Server.Authentication
{
    public class JwtAuthManager
    {
        public const string JWT_SECURITY_KEY = "71324147ee632d408ad58291a31c009526c69de3d4fd902840b0f702804b0da0";
        private const int JWT_TOKEN_VALIDITY_MINS = 20;

        private UsersService _usersService;

        public JwtAuthManager(UsersService usersService)
        {
            _usersService = usersService;
        }

        public UserSession? GenerateJwtToken(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            var userAccount = _usersService.GetUserByUsername(username);
            string hashedPassword = _usersService.HashPassword(password);
            // Enregistrez le hashedPassword dans votre modèle d'utilisateur

            // Pour vérifier le mot de passe lors de l'authentification
            bool isPasswordValid = _usersService.VerifyPassword(password, hashedPassword);

            if (userAccount == null || !isPasswordValid)
            {
                return null;
            }



            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(JWT_TOKEN_VALIDITY_MINS);
            var token_key = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);

            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, userAccount.UserName),
                new Claim(ClaimTypes.Role, userAccount.Role),
            });

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(token_key), SecurityAlgorithms.HmacSha256Signature);

            var secuityTokenDescriptor = new SecurityTokenDescriptor { Subject = claimsIdentity, Expires = tokenExpiryTimeStamp, SigningCredentials = signingCredentials };
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(secuityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);
            var userSession = new UserSession
            {
                UserName = userAccount.UserName,
                Role = userAccount.Role,
                Token = token,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds
            };

            return userSession;
        }
    }
}
