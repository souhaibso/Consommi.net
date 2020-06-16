using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Domain.Entities
{
    public class Client
    {
        //Carte d'identité nationale valide
        [Key]
        public string CIN { get; set; }
        public DateTime DateNaissance { get; set; }
        public string Mail { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public virtual ICollection<Facture> Factures { get; set; }
    }
}
