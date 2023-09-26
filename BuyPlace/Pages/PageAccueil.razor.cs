using BuyPlace.Data;
using BuyPlace.Model;

namespace BuyPlace.Pages
{
    public partial class PageAccueil
    {
        private List<Categories> categories;
        private MongoService mongoService=new MongoService();

        protected override async Task OnInitializedAsync()
        {
            categories = mongoService.GetCategories();
        }
    }
}
