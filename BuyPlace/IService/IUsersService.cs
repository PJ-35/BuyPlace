using BuyPlace.Data;

namespace BuyPlace.IService
{
    public interface IUsersService
    {

        void Save(User user);
        void Update(User user);
        User GetUserByCourriel(string courriel);
        List<User> GetAllUser();
        string DeleteUser(User user);
    }
}
