using MongoDB.Driver;
using BCrypt.Net;
using MongoDB.Bson;

namespace BuyPlace.Server.Authentication
{
    public class UsersService
    {
        private MongoClient _mongoClient = null;
        private IMongoDatabase _database = null;
        private IMongoCollection<User> _userTable = null;

        public UsersService()
        {
            _mongoClient = new MongoClient("mongodb+srv://BuyPlace:4onRFgKYRV1pt6ow@cluster0.xw7danz.mongodb.net/");
            _database = _mongoClient.GetDatabase("BuyPlace");
            _userTable = _database.GetCollection<User>("Users");

        }
        public string DeleteUser(User user)
        {
            _userTable.DeleteOne(x => x.Id == user.Id);
            return "Deleted";
        }

        public List<User> GetAllUser()
        {
            return _userTable.Find(FilterDefinition<User>.Empty).ToList();
        }

        public User GetUserByCourriel(string courriel)
        {
            return _userTable.Find(x => x.Courriel == courriel).FirstOrDefault();
        }

        public User GetUserByUsername(string username)
        {

            return _userTable.Find(x => x.UserName == username).FirstOrDefault();
        }

        public User GetUserById(ObjectId idUser)
        {

            return _userTable.Find(x => x.Id == idUser).SingleOrDefault();
        }


        public bool Save(User user)
        {
            try
            {
                //var userObj = _userTable.Find(x => x.Id == user.Id).FirstOrDefault();
                if (user is null)
                    return false;
                else
                {
                    string hashedPassword = HashPassword(user.Mdp);
                    user.Mdp = hashedPassword;
                    _userTable.InsertOne(user);
                    return true;
                }

            }
            catch { return false; }


        }


        public void Update(User user)
        {
            var userObj = GetUserByCourriel(user.Courriel);
            if (userObj != null)
            {
                _userTable.ReplaceOne(x => x.Id == userObj.Id, user);

            }
        }

        public string HashPassword(string password)
        {
            // Générez un sel aléatoire
            string salt = BCrypt.Net.BCrypt.GenerateSalt(12); // Vous pouvez ajuster la complexité du hachage ici (12 est un bon choix par défaut).

            // Hachez le mot de passe avec le sel
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

            return hashedPassword;
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            // Vérifiez si le mot de passe correspond au hachage
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
