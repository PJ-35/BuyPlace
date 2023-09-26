using BuyPlace.Data;
using BuyPlace.IService;
using MongoDB.Driver;

namespace BuyPlace.Service
{
    public class UsersService : IUsersService
    {

        private MongoClient _mongoClient = null;
        private IMongoDatabase _database=null;
        private IMongoCollection<User> _userTable = null;

        public UsersService()
        {
                _mongoClient = new MongoClient("mongodb://localhost:27017");
            _database = _mongoClient.GetDatabase("BuyPlace");
            _userTable=_database.GetCollection<User>("Users");

        }
        public string DeleteUser(User user)
        {
            _userTable.DeleteOne(x=>x.Id==user.Id);
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

        public void Save(User user)
        {

            var userObj = _userTable.Find(x => x.Id == user.Id).FirstOrDefault();
            if (userObj == null)
            {
                _userTable.InsertOne(user);
            }

           
        }


        public void Update(User user)
        {
            var userObj = GetUserByCourriel(user.Courriel);
            if (userObj != null)
            {
                _userTable.ReplaceOne(x=>x.Id==userObj.Id, user);  

            }
        }
    }
}
