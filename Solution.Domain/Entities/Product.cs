using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Domain.Entities
{
    public class Product
    {
        public int id { get; set; }
        public string nom { get; set; }
        public int prix { get; set; }
        public int quantite { get; set; }
        public string description { get; set; }
        public string imageString { get; set; }
        [JsonIgnore]
        public byte[] imageByte { get; set; }
        public string Categorie { get; set; }
        public DateTime dateAjout { get; set; }

        public int userId { get; set; }

      
    }
}
