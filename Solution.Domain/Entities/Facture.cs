
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Domain.Entities
{
    public class Facture
    {
        [Key]
        public int IdF { get; set; }
        public string ClientFK { get; set; }
        public virtual Client MyClient { get; set; }
        public int ProductFK { get; set; }
        public virtual Product MyProduct { get; set; }
        public DateTime DateAchat { get; set; }
        public double Prix { get; set; }
    }
}
