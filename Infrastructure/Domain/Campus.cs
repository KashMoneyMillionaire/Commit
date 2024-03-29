﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Domain
{
    public class Campus : EntityBase<long>
    {
        public long Number { get; set; }
        public string Name { get; set; }
        public virtual District District { get; set; }
        public long District_Id { get; set; }

        //public long CampusNumber { get; set; }
        //public string Name { get; set; }
        //public bool IsCharterSchool { get; set; }
        //public string CountyName { get; set; }
        //public long CountyId { get; set; }
        //public AccountabilityRating AccountabilityRating { get; set; }
        //public string DistrictName { get; set; }
        //public long DistrictNumber { get; set; }
        //public Grade StartGrade { get; set; }
        //public Grade EndGrade { get; set; }
        //public GradeType GradeType { get; set; }
        //public virtual Campus PairedCampus { get; set; }
        //public long RegionNumber { get; set; }
        //public CackDtl CackDtl { get; set; }

        //public bool CadMath { get; set; }
        //public bool CadRead { get; set; }
        //public bool CadProgress { get; set; }
        //public bool IsRatedUnderAEAProcedures { get; set; }

        //public List<YearStat> YearStats { get; set; }
        //public virtual List<StaarTest> StaarStats { get; set; }
    }
}
