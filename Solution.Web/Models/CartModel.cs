using Solution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Solution.Web.Models
{
    public class CartModel
    {
        public int id { get; set; }
        public Double prixTotal { get; set; }
        public bool status { get; set; }

        public DateTime dateAchat { get; set; }

        public  ICollection<CartLineModel> CartLines { get; set; }
        public int userId { get; set; }
    }
}