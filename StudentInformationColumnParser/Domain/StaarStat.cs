﻿using System.ComponentModel.DataAnnotations.Schema;
using CommitParser.Helpers;

namespace CommitParser.Domain
{
    public class StaarStat : EntityBase<long>
    {
        public long Campus_Id { get; set; }
        [ForeignKey("Campus_Id")]
        public virtual Campus Campus { get; set; }

        public long SubCatField_Id { get; set; }
        [ForeignKey("SubCatField_Id")]
        public virtual SubCatField SubCatField { get; set; }
        public long Year { get; set; }
        public Grade Grade { get; set; }
        public Language Language { get; set; }

        public string Value { get; set; }
    }
}
