using System.Data.Entity;
using Infrastructure.Data.Mappings;
using Infrastructure.Domain;

namespace Infrastructure.Data
{
    public class AzureDataContext : DbContext
    {
        static AzureDataContext()
        {
            Database.SetInitializer<AzureDataContext>(null);
        }

        public AzureDataContext() : base("Name=AzureDataContext") { }
        public AzureDataContext(string nameOrConnectionString)
            : base(nameOrConnectionString) { }

        public DbSet<Campus> Campuses { get; set; }
        public DbSet<CategoryDetail> CategoryDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<DemographicDetail> DemographicDetails { get; set; }
        public DbSet<Demographic> Demographics { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<StaarTest> StaarTests { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CampusMapping());
            modelBuilder.Configurations.Add(new CategoryDetailMapping());
            modelBuilder.Configurations.Add(new CategoryMappinng());
            modelBuilder.Configurations.Add(new DemographicDetailMapping());
            modelBuilder.Configurations.Add(new DemographicMapping());
            modelBuilder.Configurations.Add(new DistrictMapping());
            modelBuilder.Configurations.Add(new LanguageMapping());
            modelBuilder.Configurations.Add(new RegionMapping());
            modelBuilder.Configurations.Add(new StaarTestMapping());
            modelBuilder.Configurations.Add(new SubjectMapping());
        }

    }
}
