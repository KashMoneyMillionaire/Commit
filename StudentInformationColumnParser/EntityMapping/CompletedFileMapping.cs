using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommitParser.Domain;

namespace CommitParser.EntityMapping
{
    public class CompletedFileMapping : EntityTypeConfiguration<CompletedFile>
    {
        public CompletedFileMapping()
        {
            // PRIMARY KEY

            HasKey(c => c.Id);

            // PROPERTIES

            Property(c => c.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.IsCompleted);

            Property(c => c.FileName);

            Property(c => c.TimeCompleted);
        }
    }
}
