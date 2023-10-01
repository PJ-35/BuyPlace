using BuyPlace.Data;
using BuyPlace.Service;

namespace BuyPlace.Pages
{
    public partial class Connexion
    {

        private User user = new User();
      
        public string LoginMesssage { get; set; } = "";
        private UsersService userService = new UsersService();
    }
}
