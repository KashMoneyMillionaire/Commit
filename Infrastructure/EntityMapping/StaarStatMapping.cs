using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Infrastructure.Domain;

namespace Infrastructure.EntityMapping
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


            Property(c => c.Value);

            HasRequired(c => c.SubCatField)
                .WithMany()
                .HasForeignKey(c => c.SubCatField_Id);

            HasRequired(c => c.Campus)
                .WithMany(c => c.StaarStats)
                .HasForeignKey(c => c.Campus_Id);

            HasRequired(c => c.YearGradeLang)
                .WithMany()
                .HasForeignKey(c => c.YearGradeLang_Id);
        }
    }
}
