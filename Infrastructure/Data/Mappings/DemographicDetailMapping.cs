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
    class DemographicDetailMapping : EntityTypeConfiguration<DemographicDetail>
    {
        public DemographicDetailMapping()
        {
            // PRIMARY KEY

            HasKey(c => c.Id);

            // PROPERTIES

            Property(c => c.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.Detail);

            Property(c => c.Description);

            Property(c => c.YearStarted);

            HasRequired(c => c.Demographic)
                .WithMany();

        }
    }
}
