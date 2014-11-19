using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Domain
{
    public class CategoryDetail : EntityBase<long>
    {
        public virtual Category Category { get; set; }
        public string Detail { get; set; }
        public string Description { get; set; }
        public int YearStarted { get; set; }
        public CategoryType CategoryType { get; set; }
        public CategoryDetail CategoryPair { get; set; }
    }

}
