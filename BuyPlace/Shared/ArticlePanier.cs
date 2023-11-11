using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyPlace.Shared
{
    public class ArticlePanier
    {

        public ArticleSession Article {  get; set; }
        public Decimal PrixUnitaire {  get; set; }

        public int Quantite {  get; set; }
    }
}
