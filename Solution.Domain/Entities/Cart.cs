using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Domain.Entities
{
    public class Cart
    {
        public int id { get; set; }
        public Double prixTotal { get; set; }
        public bool status { get; set; }

        public DateTime dateAchat { get; set; }

        public virtual ICollection<CartLine> CartLines { get; set; }
        public int userId { get; set; }
        
    }
}
