using Solution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Data.Configurations
{
    public class FactureConfig: EntityTypeConfiguration<Facture>
    {
        public FactureConfig()
        {
            HasRequired(f => f.MyClient)
                .WithMany(c => c.Factures)
                .HasForeignKey(f => f.ClientFK);
       
            HasKey(f => new {f.ClientFK, f.ProductFK, f.DateAchat});
        }
    }
}
