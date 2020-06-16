using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Solution.Web.Models
{
    public class ProductModel
    {
        public int id { get; set; }
        public string nom { get; set; }
        public int prix { get; set; }
        public int quantite { get; set; }
        public string description { get; set; }
        public string imageString { get; set; }
        public byte[] imageByte { get; set; }

        public string Categorie { get; set; }
        public DateTime dateAjout { get; set; }

        public int userId { get; set; }
    }
}