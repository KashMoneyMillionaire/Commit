using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommitParser.Helpers;

namespace CommitParser.Domain
{
    public class SubCatField : EntityBase<long>
    {
        public StaarSubjectName Subject { get; set; }
        public StaarCategoryName Category { get; set; }
        public StaarDemographic Field { get; set; }
    }
}
