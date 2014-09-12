using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommitParser.Helpers;

namespace StudentInformationColumnParser.Domain
{
    public class Campus
    {
        public long CampusId { get; set; }
        public string Name { get; set; }
        public bool IsCharterSchool { get; set; }
        public string CountyName { get; set; }
        public long CountyId { get; set; }
        public AccountabilityRating AccountabilityRating { get; set; }
        public string DistrictName { get; set; }
        public long DistrictNumber { get; set; }
        public Grade StartGrade { get; set; }
        public Grade EndGrade { get; set; }
        public GradeType GradeType { get; set; }
        public Campus PairedCampus { get; set; }
        public long RegionNumber { get; set; }
        public CackDtl CackDtl { get; set; }

        public bool CadMath { get; set; }
        public bool CadRead { get; set; }
        public bool CadProgress { get; set; }
        public bool IsRatedUnderAEAProcedures { get; set; }

        public List<YearStat> YearStats { get; set; }
        public List<StaarStat> StaarStats { get; set; }
    }
}
