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
    class DistrictMapping : EntityTypeConfiguration<District>
    {
        public DistrictMapping()
        {
            // PRIMARY KEY

            HasKey(c => c.Id);

            // PROPERTIES

            Property(c => c.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.Number);

            Property(c => c.Name);

            HasRequired(c => c.Region)
                .WithMany()
                .HasForeignKey(c => c.Region_Id);

        }
    }
}
