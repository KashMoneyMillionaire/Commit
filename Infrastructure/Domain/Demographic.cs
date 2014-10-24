using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Domain
{
    public class Demographic : EntityBase<long>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
