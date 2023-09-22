namespace BuyPlace.Layout
{
    public partial class Header
    {
        bool collapseNavMenu = true;

        string baseMenuClass = "navbar-collapse d-sm-inline-flex flex-sm-row-reverse";

        string NavMenuCssClass => baseMenuClass + (collapseNavMenu ? " collapse" : "");

        void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }
    }
}
