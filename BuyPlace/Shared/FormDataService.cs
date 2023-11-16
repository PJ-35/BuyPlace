using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyPlace.Shared
{
    public class FormDataService
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public string recherche { get; set; }
        public string categorie { get; set; }
        public bool MinChange { get; set; } = false;
        public bool MaxChange { get; set; } = false;
        public bool boolRecherche { get; set; } = false;
    }
}
