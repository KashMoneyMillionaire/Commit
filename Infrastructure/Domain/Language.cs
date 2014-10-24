using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Domain
{
    public class Language : EntityBase<long>
    {
        public string Name { get; set; }
        public int YearStarted { get; set; }
    }
}
