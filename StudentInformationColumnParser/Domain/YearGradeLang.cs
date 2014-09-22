using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommitParser.Helpers;

namespace CommitParser.Domain
{
    public class YearGradeLang : EntityBase<long>
    {
        public long Year { get; set; }
        public Grade Grade { get; set; }
        public Language Language { get; set; }
    }
}
