using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CommitParser.Domain;

namespace CommitParser.EntityMapping
{
    class SubCatFieldMapping : EntityTypeConfiguration<SubCatField>
    {
        public SubCatFieldMapping()
        {
            // PRIMARY KEY

            HasKey(c => c.Id);

            // PROPERTIES

            Property(c => c.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.Subject);

            Property(c => c.Category);

            Property(c => c.Field);

        }
    }
}
