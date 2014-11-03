using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Infrastructure.Domain;

namespace Infrastructure.Data.Mappings
{
    class CampusMapping : EntityTypeConfiguration<Campus>
    {
        public CampusMapping()
        {
            // PRIMARY KEY

            HasKey(c => c.Id);

            // PROPERTIES

            Property(c => c.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.Number);

            Property(c => c.Name);

            HasRequired(c => c.District)
                .WithMany()
                .HasForeignKey(c => c.District_Id);

            //Property(c => c.IsCharterSchool);

            //Property(c => c.CountyName);

            //Property(c => c.CountyId);

            //Property(c => c.AccountabilityRating);

            //Property(c => c.DistrictName);

            //Property(c => c.DistrictNumber);

            //Property(c => c.StartGrade);

            //Property(c => c.EndGrade);

            //Property(c => c.GradeType);

            //Property(c => c.RegionNumber);

            //Property(c => c.CackDtl);

            //Property(c => c.CadMath);

            //Property(c => c.CadRead);

            //Property(c => c.CadProgress);

            //Property(c => c.IsRatedUnderAEAProcedures);

            //HasOptional(r => r.PairedCampus);

            //HasMany(c => c.YearStats)
            //    .WithRequired();

            //HasMany(c => c.StaarStats)
            //    .WithRequired(c => c.Campus);
        }
    }
}
