using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Domain
{
    public class District : EntityBase<long>
    {
        public virtual Region Region { get; set; }
        public long Number { get; set; }
        public string Name { get; set; }

    }
}
