using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentInformationColumnParser.Helpers;

namespace StudentInformationColumnParser.Domain
{
    public class StaarStat
    {
        public long Year { get; set; }
        public Grade Grade { get; set; }
        public StaarSubjectName Subject { get; set; }
        public StaarFieldName Field { get; set; }
        public StaarCategoryName Category { get; set; }
        public string Value { get; set; }

        //campus
        //year
        //region
        //district
        //dname
        //cname
        //grade

    }
}
