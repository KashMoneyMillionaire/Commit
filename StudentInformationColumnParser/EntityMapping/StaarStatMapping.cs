using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CommitParser.Domain;

namespace CommitParser.EntityMapping
{
    class StaarStatMapping : EntityTypeConfiguration<StaarStat>
    {
        public StaarStatMapping()
        {
            // PRIMARY KEY

            HasKey(c => c.Id);

            // PROPERTIES

            Property(c => c.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.Year);

            Property(c => c.Grade);

            Property(c => c.Language);

            Property(c => c.Field);

            Property(c => c.Category);

            Property(c => c.Subject);

            Property(c => c.Value);

            HasRequired(c => c.Campus)
                .WithMany(c => c.StaarStats)
                .HasForeignKey(c => c.Campus_Id);
        }
    }
}
