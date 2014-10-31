using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Domain
{
    public class DemographicDetail : EntityBase<long>
    {
        public virtual Demographic Demographic { get; set; }
        public string Detail { get; set; }
        public string Description { get; set; }
        public int YearStarted { get; set; }
    }
}
