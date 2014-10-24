using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Domain;

namespace Infrastructure.Data.Mappings
{
    class DemographicMapping : EntityTypeConfiguration<Demographic>
    {
        public DemographicMapping()
        {
            // PRIMARY KEY

            HasKey(c => c.Id);

            // PROPERTIES

            Property(c => c.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.Description);

            Property(c => c.Name);

        }
    }
}
