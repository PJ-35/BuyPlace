using BuyPlace.Data;
using BuyPlace.Service;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System;
using Microsoft.AspNetCore.Components.Authorization;

namespace BuyPlace.Pages
{
    public partial class Connexion
    {

        private User user = new User();
      
        public string LoginMesssage { get; set; } = "";
        private UsersService userService = new UsersService();


        [Inject]
        private NavigationManager NavManager { get; set; }

        [Inject]
        ILocalStorageService localStorage { get; set; }

        [Inject]
        AuthenticationStateProvider AuthStateProvider { get; set; } 
        private async void HandleLogin()
        {
            await localStorage.SetItemAsync<string>("username", user.UserName);
            await AuthStateProvider.GetAuthenticationStateAsync();
            NavManager.NavigateTo("");
            // LocalStorage.SetI
        }
    }
}
