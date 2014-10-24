using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Infrastructure.Domain;

namespace Infrastructure.Data.Mappings
{
    class StaarStatMapping : EntityTypeConfiguration<StaarTest>
    {
        public StaarStatMapping()
        {
            // PRIMARY KEY

            HasKey(c => c.Id);

            // PROPERTIES

            Property(c => c.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.Year);

            Property(c => c.Value);

            HasRequired(c => c.Campus)
                .WithMany();

            HasRequired(c => c.Language)
                .WithMany();

            HasRequired(c => c.Subject)
                .WithMany();

            HasRequired(c => c.CategoryDetail)
                .WithMany();

            HasRequired(c => c.DemographicDetail)
                .WithMany();

            //HasRequired(c => c.SubCatField)
            //    .WithMany()
            //    .HasForeignKey(c => c.SubCatField_Id);

            //HasRequired(c => c.Campus)
            //    .WithMany(c => c.StaarStats)
            //    .HasForeignKey(c => c.Campus_Id);

            //HasRequired(c => c.YearGradeLang)
            //    .WithMany()
            //    .HasForeignKey(c => c.YearGradeLang_Id);
        }
    }
}
