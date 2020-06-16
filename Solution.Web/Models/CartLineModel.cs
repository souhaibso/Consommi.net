using Solution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Solution.Web.Models
{
    public class CartLineModel
    {
        public int id { get; set; }
        public DateTime dateAjout { get; set; }
        public int quantiteChoisie { get; set; }
        public Double prixDuProduit { get; set; }
        public Double prixTotal { get; set; }

        public  ProductModel myProduct { get; set; }
        public int productId { get; set; }

       
        public int CartId { get; set; }

    }
}