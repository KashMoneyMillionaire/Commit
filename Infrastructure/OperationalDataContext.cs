using System.Data.Entity;
using Infrastructure.Domain;
using Infrastructure.EntityMapping;

namespace Infrastructure
{
    public class OperationalDataContext : DbContext
    {
        static OperationalDataContext()
        {
            Database.SetInitializer<OperationalDataContext>(null);
        }

        public OperationalDataContext() : base("Name=OperationalDataContext") { }
        public OperationalDataContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            // base.Configuration.AutoDetectChangesEnabled = false;
        }

        public DbSet<Campus> Campuses { get; set; }
        public DbSet<StaarStat> StaarStats { get; set; }
        public DbSet<CompletedFile> CompletedFiles { get; set; }
        public DbSet<SubCatField> SubCatFields { get; set; }
        public DbSet<YearGradeLang> YearGradeLangs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CampusMapping());
            modelBuilder.Configurations.Add(new StaarStatMapping());
            modelBuilder.Configurations.Add(new CompletedFileMapping());
            modelBuilder.Configurations.Add(new SubCatFieldMapping());
            modelBuilder.Configurations.Add(new YearGradeLangMapping());
        }

        
    }
}
