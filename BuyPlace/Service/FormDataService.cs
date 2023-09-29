namespace BuyPlace.Service
{
    public class FormDataService
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public string recherche { get; set; }
        public bool MinChange { get; set; }=false;
        public bool MaxChange { get; set; } = false;
        public bool boolRecherche { get; set; } = false;
    }
}
