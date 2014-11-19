using System.Data.Entity;
using Infrastructure.Data.Mappings;
using Infrastructure.Domain;

namespace Infrastructure.Data
{
    public class OperationalDataContext : DbContext
    {
        static OperationalDataContext()
        {
            Database.SetInitializer<OperationalDataContext>(null);
        }

        public OperationalDataContext() : base("Name=OperationalDataContext") { }
        public OperationalDataContext(string nameOrConnectionString)
            : base(nameOrConnectionString) { }

        
    }
}
