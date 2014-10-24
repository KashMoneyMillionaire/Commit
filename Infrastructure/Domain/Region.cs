using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Domain
{
    public class Region : EntityBase<long>
    {
        public long Number { get; set; }
        public string Name { get; set; }
    }
}
