using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CommitParser.Domain;

namespace CommitParser.EntityMapping
{
    class YearGradeLangMapping : EntityTypeConfiguration<YearGradeLang>
    {
        public YearGradeLangMapping()
        {
            // PRIMARY KEY

            HasKey(c => c.Id);

            // PROPERTIES

            Property(c => c.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.Grade);

            Property(c => c.Year);

            Property(c => c.Language);

        }
    }
}
