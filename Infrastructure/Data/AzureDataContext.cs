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
            : base(nameOrConnectionString)
        {
            // base.Configuration.AutoDetectChangesEnabled = false;
        }

        public DbSet<Campus> Campuses { get; set; }
        public DbSet<StaarTest> StaarStats { get; set; }
        //public DbSet<CompletedFile> CompletedFiles { get; set; }
        //public DbSet<SubCatField> SubCatFields { get; set; }
        //public DbSet<YearGradeLang> YearGradeLangs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CampusMapping());
            modelBuilder.Configurations.Add(new StaarTestMapping());
            //modelBuilder.Configurations.Add(new CompletedFileMapping());
            //modelBuilder.Configurations.Add(new SubCatFieldMapping());
            //modelBuilder.Configurations.Add(new YearGradeLangMapping());
        }

        
    }
}
