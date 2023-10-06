using MongoDB.Driver;

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


        public bool Save(User user)
        {

            //var userObj = _userTable.Find(x => x.Id == user.Id).FirstOrDefault();
            if (user is null||string.IsNullOrWhiteSpace(user.Nom))
                return false;
            else
            {
                _userTable.InsertOne(user);
                return true;
            }


        }


        public void Update(User user)
        {
            var userObj = GetUserByCourriel(user.Courriel);
            if (userObj != null)
            {
                _userTable.ReplaceOne(x => x.Id == userObj.Id, user);

            }
        }
    }
}
