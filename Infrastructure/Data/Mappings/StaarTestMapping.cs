using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Infrastructure.Domain;

namespace Infrastructure.Data.Mappings
{
    class StaarTestMapping : EntityTypeConfiguration<StaarTest>
    {
        public StaarTestMapping()
        {
            // PRIMARY KEY

            HasKey(c => c.Id);

            // PROPERTIES

            Property(c => c.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.Year);

            Property(c => c.Value);
            
            Property(c => c.Grade);

            HasRequired(c => c.Campus)
                .WithMany()
                .HasForeignKey(c => c.Campus_Id);

            HasRequired(c => c.Language)
                .WithMany()
                .HasForeignKey(c => c.Language_Id);

            HasRequired(c => c.Subject)
                .WithMany()
                .HasForeignKey(c => c.Subject_Id);

            HasRequired(c => c.CategoryDetail)
                .WithMany()
                .HasForeignKey(c => c.CategoryDetail_Id);

            HasRequired(c => c.DemographicDetail)
                .WithMany()
                .HasForeignKey(c => c.DemographicDetail_Id);




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
