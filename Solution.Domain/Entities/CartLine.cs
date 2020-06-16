using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Solution.Domain.Entities
{
    public class CartLine
    {
        public int id { get; set; }
        public DateTime dateAjout { get; set; }
        public int quantiteChoisie { get; set; }
        public Double prixDuProduit { get; set; }
        public Double prixTotal { get; set; }

        public virtual Product myProduct { get; set; }
        [ForeignKey("myProduct")]
        public int productId { get; set; }

        [JsonIgnore]
        public virtual Cart myCart { get; set; }
        [ForeignKey("myCart")]
        public int CartId { get; set; }


    }
}
