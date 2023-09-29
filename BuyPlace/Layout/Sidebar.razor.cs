using BuyPlace.Data;
using BuyPlace.Model;
using BuyPlace.Service;
using Microsoft.AspNetCore.Components;
using MongoDB.Bson.IO;
using System.Text;
using System.Text.Json;

namespace BuyPlace.Layout
{
    public partial class Sidebar
    {
        private List<Categories> categories;
        private MongoServiceCategories mongoService = new MongoServiceCategories();
        private System.Timers.Timer timer;
        private string validMin = "";
        private string validMax = "";


        private class FormData
        {
            public string Min { get; set; }
            public string Max { get; set; }
        }

        private FormData formData = new FormData();
       

        private async Task SubmitForm()
        {
            int variableTravail = 0;
            if (int.TryParse(formData.Min, out variableTravail))
            {
                formDataService.Min = variableTravail;
                validMin = "is-valid";
                formDataService.MinChange = true;
            }
            else
            {
                validMin = "is-invalid";
                formDataService.MinChange= false;
            }
            if (string.IsNullOrWhiteSpace(formData.Min))
                validMin = "";
            if (int.TryParse(formData.Max, out variableTravail))
            {
                formDataService.Max = variableTravail;
                validMax = "is-valid";
                formDataService.MaxChange = true;
            }
            else
            {
                validMax = "is-invalid";
                formDataService.MaxChange = false;
            }
            if (string.IsNullOrWhiteSpace(formData.Max))
                validMax = "";
            
        }
        private void chargement()
        {
            if(formDataService.MinChange)
                formData.Min = formDataService.Min.ToString();
            if (formDataService.MaxChange)
                formData.Max = formDataService.Max.ToString();

        }

    protected override async Task OnInitializedAsync()
        {
            chargement();
            categories = mongoService.GetCategories();
            timer = new System.Timers.Timer(2000);
            timer.Elapsed += async (sender, e) =>
            {
                //chargement();
                categories = mongoService.GetCategories();
                InvokeAsync(StateHasChanged);
            };
            timer.Start();

        }

        public void Dispose()
        {
            timer?.Stop();
            timer?.Dispose();
        }
    }
}
